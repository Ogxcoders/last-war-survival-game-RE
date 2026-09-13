using System.Threading.Tasks;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema;

internal sealed class Parser
{
	private SchemaType schemaType;

	private XmlNameTable nameTable;

	private SchemaNames schemaNames;

	private ValidationEventHandler eventHandler;

	private XmlNamespaceManager namespaceManager;

	private XmlReader reader;

	private PositionInfo positionInfo;

	private bool isProcessNamespaces;

	private int schemaXmlDepth;

	private int markupDepth;

	private SchemaBuilder builder;

	private XmlSchema schema;

	private SchemaInfo xdrSchema;

	private XmlResolver xmlResolver;

	private XmlDocument dummyDocument;

	private bool processMarkup;

	private XmlNode parentNode;

	private XmlNamespaceManager annotationNSManager;

	private string xmlns;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	public XmlSchema XmlSchema => schema;

	internal XmlResolver XmlResolver
	{
		set
		{
			xmlResolver = value;
		}
	}

	public SchemaInfo XdrSchema => xdrSchema;

	public Parser(SchemaType schemaType, XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
	{
		this.schemaType = schemaType;
		this.nameTable = nameTable;
		this.schemaNames = schemaNames;
		this.eventHandler = eventHandler;
		xmlResolver = XmlReaderSection.CreateDefaultResolver();
		processMarkup = true;
		dummyDocument = new XmlDocument();
	}

	public SchemaType Parse(XmlReader reader, string targetNamespace)
	{
		StartParsing(reader, targetNamespace);
		while (ParseReaderNode() && reader.Read())
		{
		}
		return FinishParsing();
	}

	public void StartParsing(XmlReader reader, string targetNamespace)
	{
		this.reader = reader;
		positionInfo = PositionInfo.GetPositionInfo(reader);
		namespaceManager = reader.NamespaceManager;
		if (namespaceManager == null)
		{
			namespaceManager = new XmlNamespaceManager(nameTable);
			isProcessNamespaces = true;
		}
		else
		{
			isProcessNamespaces = false;
		}
		while (reader.NodeType != XmlNodeType.Element && reader.Read())
		{
		}
		markupDepth = int.MaxValue;
		schemaXmlDepth = reader.Depth;
		SchemaType rootType = schemaNames.SchemaTypeFromRoot(reader.LocalName, reader.NamespaceURI);
		if (!CheckSchemaRoot(rootType, out var code))
		{
			throw new XmlSchemaException(code, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition);
		}
		if (schemaType == SchemaType.XSD)
		{
			schema = new XmlSchema();
			schema.BaseUri = new Uri(reader.BaseURI, UriKind.RelativeOrAbsolute);
			builder = new XsdBuilder(reader, namespaceManager, schema, nameTable, schemaNames, eventHandler);
		}
		else
		{
			xdrSchema = new SchemaInfo();
			xdrSchema.SchemaType = SchemaType.XDR;
			builder = new XdrBuilder(reader, namespaceManager, xdrSchema, targetNamespace, nameTable, schemaNames, eventHandler);
			((XdrBuilder)builder).XmlResolver = xmlResolver;
		}
	}

	private bool CheckSchemaRoot(SchemaType rootType, out string code)
	{
		code = null;
		if (schemaType == SchemaType.None)
		{
			schemaType = rootType;
		}
		switch (rootType)
		{
		case SchemaType.XSD:
			if (schemaType != SchemaType.XSD)
			{
				code = "Different schema types cannot be mixed.";
				return false;
			}
			break;
		case SchemaType.XDR:
			if (schemaType == SchemaType.XSD)
			{
				code = "'XmlSchemaSet' can load only W3C XML Schemas.";
				return false;
			}
			if (schemaType != SchemaType.XDR)
			{
				code = "Different schema types cannot be mixed.";
				return false;
			}
			break;
		case SchemaType.None:
		case SchemaType.DTD:
			code = "Expected schema root. Make sure the root element is <schema> and the namespace is 'http://www.w3.org/2001/XMLSchema' for an XSD schema or 'urn:schemas-microsoft-com:xml-data' for an XDR schema.";
			if (schemaType == SchemaType.XSD)
			{
				code = "The root element of a W3C XML Schema should be <schema> and its namespace should be 'http://www.w3.org/2001/XMLSchema'.";
			}
			return false;
		}
		return true;
	}

	public SchemaType FinishParsing()
	{
		return schemaType;
	}

	public bool ParseReaderNode()
	{
		if (reader.Depth > markupDepth)
		{
			if (processMarkup)
			{
				ProcessAppInfoDocMarkup(root: false);
			}
			return true;
		}
		if (reader.NodeType == XmlNodeType.Element)
		{
			if (builder.ProcessElement(reader.Prefix, reader.LocalName, reader.NamespaceURI))
			{
				namespaceManager.PushScope();
				if (reader.MoveToFirstAttribute())
				{
					do
					{
						builder.ProcessAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.Value);
						if (Ref.Equal(reader.NamespaceURI, schemaNames.NsXmlNs) && isProcessNamespaces)
						{
							namespaceManager.AddNamespace((reader.Prefix.Length == 0) ? string.Empty : reader.LocalName, reader.Value);
						}
					}
					while (reader.MoveToNextAttribute());
					reader.MoveToElement();
				}
				builder.StartChildren();
				if (reader.IsEmptyElement)
				{
					namespaceManager.PopScope();
					builder.EndChildren();
					if (reader.Depth == schemaXmlDepth)
					{
						return false;
					}
				}
				else if (!builder.IsContentParsed())
				{
					markupDepth = reader.Depth;
					processMarkup = true;
					if (annotationNSManager == null)
					{
						annotationNSManager = new XmlNamespaceManager(nameTable);
						xmlns = nameTable.Add("xmlns");
					}
					ProcessAppInfoDocMarkup(root: true);
				}
			}
			else if (!reader.IsEmptyElement)
			{
				markupDepth = reader.Depth;
				processMarkup = false;
			}
		}
		else if (reader.NodeType == XmlNodeType.Text)
		{
			if (!xmlCharType.IsOnlyWhitespace(reader.Value))
			{
				builder.ProcessCData(reader.Value);
			}
		}
		else if (reader.NodeType == XmlNodeType.EntityReference || reader.NodeType == XmlNodeType.SignificantWhitespace || reader.NodeType == XmlNodeType.CDATA)
		{
			builder.ProcessCData(reader.Value);
		}
		else if (reader.NodeType == XmlNodeType.EndElement)
		{
			if (reader.Depth == markupDepth)
			{
				if (processMarkup)
				{
					XmlNodeList childNodes = parentNode.ChildNodes;
					XmlNode[] array = new XmlNode[childNodes.Count];
					for (int i = 0; i < childNodes.Count; i++)
					{
						array[i] = childNodes[i];
					}
					builder.ProcessMarkup(array);
					namespaceManager.PopScope();
					builder.EndChildren();
				}
				markupDepth = int.MaxValue;
			}
			else
			{
				namespaceManager.PopScope();
				builder.EndChildren();
			}
			if (reader.Depth == schemaXmlDepth)
			{
				return false;
			}
		}
		return true;
	}

	private void ProcessAppInfoDocMarkup(bool root)
	{
		XmlNode newChild = null;
		switch (reader.NodeType)
		{
		case XmlNodeType.Element:
			annotationNSManager.PushScope();
			newChild = LoadElementNode(root);
			return;
		case XmlNodeType.Text:
			newChild = dummyDocument.CreateTextNode(reader.Value);
			break;
		case XmlNodeType.SignificantWhitespace:
			newChild = dummyDocument.CreateSignificantWhitespace(reader.Value);
			break;
		case XmlNodeType.CDATA:
			newChild = dummyDocument.CreateCDataSection(reader.Value);
			break;
		case XmlNodeType.EntityReference:
			newChild = dummyDocument.CreateEntityReference(reader.Name);
			break;
		case XmlNodeType.Comment:
			newChild = dummyDocument.CreateComment(reader.Value);
			break;
		case XmlNodeType.ProcessingInstruction:
			newChild = dummyDocument.CreateProcessingInstruction(reader.Name, reader.Value);
			break;
		case XmlNodeType.EndElement:
			annotationNSManager.PopScope();
			parentNode = parentNode.ParentNode;
			return;
		case XmlNodeType.Whitespace:
		case XmlNodeType.EndEntity:
			return;
		}
		parentNode.AppendChild(newChild);
	}

	private XmlElement LoadElementNode(bool root)
	{
		XmlReader xmlReader = reader;
		bool isEmptyElement = xmlReader.IsEmptyElement;
		XmlElement xmlElement = dummyDocument.CreateElement(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
		xmlElement.IsEmpty = isEmptyElement;
		if (root)
		{
			parentNode = xmlElement;
		}
		else
		{
			XmlAttributeCollection attributes = xmlElement.Attributes;
			if (xmlReader.MoveToFirstAttribute())
			{
				do
				{
					if (Ref.Equal(xmlReader.NamespaceURI, schemaNames.NsXmlNs))
					{
						annotationNSManager.AddNamespace((xmlReader.Prefix.Length == 0) ? string.Empty : reader.LocalName, reader.Value);
					}
					XmlAttribute node = LoadAttributeNode();
					attributes.Append(node);
				}
				while (xmlReader.MoveToNextAttribute());
			}
			xmlReader.MoveToElement();
			string text = annotationNSManager.LookupNamespace(xmlReader.Prefix);
			if (text == null)
			{
				XmlAttribute node2 = CreateXmlNsAttribute(xmlReader.Prefix, namespaceManager.LookupNamespace(xmlReader.Prefix));
				attributes.Append(node2);
			}
			else if (text.Length == 0)
			{
				string text2 = namespaceManager.LookupNamespace(xmlReader.Prefix);
				if (text2 != string.Empty)
				{
					XmlAttribute node3 = CreateXmlNsAttribute(xmlReader.Prefix, text2);
					attributes.Append(node3);
				}
			}
			while (xmlReader.MoveToNextAttribute())
			{
				if (xmlReader.Prefix.Length != 0 && annotationNSManager.LookupNamespace(xmlReader.Prefix) == null)
				{
					XmlAttribute node4 = CreateXmlNsAttribute(xmlReader.Prefix, namespaceManager.LookupNamespace(xmlReader.Prefix));
					attributes.Append(node4);
				}
			}
			xmlReader.MoveToElement();
			parentNode.AppendChild(xmlElement);
			if (!xmlReader.IsEmptyElement)
			{
				parentNode = xmlElement;
			}
		}
		return xmlElement;
	}

	private XmlAttribute CreateXmlNsAttribute(string prefix, string value)
	{
		XmlAttribute xmlAttribute = ((prefix.Length != 0) ? dummyDocument.CreateAttribute(xmlns, prefix, "http://www.w3.org/2000/xmlns/") : dummyDocument.CreateAttribute(string.Empty, xmlns, "http://www.w3.org/2000/xmlns/"));
		xmlAttribute.AppendChild(dummyDocument.CreateTextNode(value));
		annotationNSManager.AddNamespace(prefix, value);
		return xmlAttribute;
	}

	private XmlAttribute LoadAttributeNode()
	{
		XmlReader xmlReader = reader;
		XmlAttribute xmlAttribute = dummyDocument.CreateAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
		while (xmlReader.ReadAttributeValue())
		{
			switch (xmlReader.NodeType)
			{
			case XmlNodeType.Text:
				xmlAttribute.AppendChild(dummyDocument.CreateTextNode(xmlReader.Value));
				break;
			case XmlNodeType.EntityReference:
				xmlAttribute.AppendChild(LoadEntityReferenceInAttribute());
				break;
			default:
				throw XmlLoader.UnexpectedNodeType(xmlReader.NodeType);
			}
		}
		return xmlAttribute;
	}

	private XmlEntityReference LoadEntityReferenceInAttribute()
	{
		XmlEntityReference xmlEntityReference = dummyDocument.CreateEntityReference(reader.LocalName);
		if (!reader.CanResolveEntity)
		{
			return xmlEntityReference;
		}
		reader.ResolveEntity();
		while (reader.ReadAttributeValue())
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Text:
				xmlEntityReference.AppendChild(dummyDocument.CreateTextNode(reader.Value));
				break;
			case XmlNodeType.EndEntity:
				if (xmlEntityReference.ChildNodes.Count == 0)
				{
					xmlEntityReference.AppendChild(dummyDocument.CreateTextNode(string.Empty));
				}
				return xmlEntityReference;
			case XmlNodeType.EntityReference:
				xmlEntityReference.AppendChild(LoadEntityReferenceInAttribute());
				break;
			default:
				throw XmlLoader.UnexpectedNodeType(reader.NodeType);
			}
		}
		return xmlEntityReference;
	}

	public async Task<SchemaType> ParseAsync(XmlReader reader, string targetNamespace)
	{
		await StartParsingAsync(reader, targetNamespace).ConfigureAwait(continueOnCapturedContext: false);
		bool flag;
		do
		{
			flag = ParseReaderNode();
			if (flag)
			{
				flag = await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (flag);
		return FinishParsing();
	}

	public async Task StartParsingAsync(XmlReader reader, string targetNamespace)
	{
		this.reader = reader;
		positionInfo = PositionInfo.GetPositionInfo(reader);
		namespaceManager = reader.NamespaceManager;
		if (namespaceManager == null)
		{
			namespaceManager = new XmlNamespaceManager(nameTable);
			isProcessNamespaces = true;
		}
		else
		{
			isProcessNamespaces = false;
		}
		bool flag;
		do
		{
			flag = reader.NodeType != XmlNodeType.Element;
			if (flag)
			{
				flag = await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (flag);
		markupDepth = int.MaxValue;
		schemaXmlDepth = reader.Depth;
		SchemaType rootType = schemaNames.SchemaTypeFromRoot(reader.LocalName, reader.NamespaceURI);
		if (!CheckSchemaRoot(rootType, out var code))
		{
			throw new XmlSchemaException(code, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition);
		}
		if (schemaType == SchemaType.XSD)
		{
			schema = new XmlSchema();
			schema.BaseUri = new Uri(reader.BaseURI, UriKind.RelativeOrAbsolute);
			builder = new XsdBuilder(reader, namespaceManager, schema, nameTable, schemaNames, eventHandler);
		}
		else
		{
			xdrSchema = new SchemaInfo();
			xdrSchema.SchemaType = SchemaType.XDR;
			builder = new XdrBuilder(reader, namespaceManager, xdrSchema, targetNamespace, nameTable, schemaNames, eventHandler);
			((XdrBuilder)builder).XmlResolver = xmlResolver;
		}
	}
}
