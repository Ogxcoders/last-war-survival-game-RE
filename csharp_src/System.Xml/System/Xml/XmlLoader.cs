using System.Collections;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml;

internal class XmlLoader
{
	private XmlDocument doc;

	private XmlReader reader;

	private bool preserveWhitespace;

	internal void Load(XmlDocument doc, XmlReader reader, bool preserveWhitespace)
	{
		this.doc = doc;
		if (reader.GetType() == typeof(XmlTextReader))
		{
			this.reader = ((XmlTextReader)reader).Impl;
		}
		else
		{
			this.reader = reader;
		}
		this.preserveWhitespace = preserveWhitespace;
		if (doc == null)
		{
			throw new ArgumentException(Res.GetString("The document to be loaded could not be found."));
		}
		if (reader == null)
		{
			throw new ArgumentException(Res.GetString("There is no reader from which to load the document."));
		}
		doc.SetBaseURI(reader.BaseURI);
		if (reader.Settings != null && reader.Settings.ValidationType == ValidationType.Schema)
		{
			doc.Schemas = reader.Settings.Schemas;
		}
		if (this.reader.ReadState == ReadState.Interactive || this.reader.Read())
		{
			LoadDocSequence(doc);
		}
	}

	private void LoadDocSequence(XmlDocument parentDoc)
	{
		XmlNode xmlNode = null;
		while ((xmlNode = LoadNode(skipOverWhitespace: true)) != null)
		{
			parentDoc.AppendChildForLoad(xmlNode, parentDoc);
			if (!reader.Read())
			{
				break;
			}
		}
	}

	internal XmlNode ReadCurrentNode(XmlDocument doc, XmlReader reader)
	{
		this.doc = doc;
		this.reader = reader;
		preserveWhitespace = true;
		if (doc == null)
		{
			throw new ArgumentException(Res.GetString("The document to be loaded could not be found."));
		}
		if (reader == null)
		{
			throw new ArgumentException(Res.GetString("There is no reader from which to load the document."));
		}
		if (reader.ReadState == ReadState.Initial)
		{
			reader.Read();
		}
		if (reader.ReadState == ReadState.Interactive)
		{
			XmlNode xmlNode = LoadNode(skipOverWhitespace: true);
			if (xmlNode.NodeType != XmlNodeType.Attribute)
			{
				reader.Read();
			}
			return xmlNode;
		}
		return null;
	}

	private XmlNode LoadNode(bool skipOverWhitespace)
	{
		XmlReader xmlReader = reader;
		XmlNode xmlNode = null;
		do
		{
			XmlNode xmlNode2 = null;
			switch (xmlReader.NodeType)
			{
			case XmlNodeType.Element:
			{
				bool isEmptyElement = xmlReader.IsEmptyElement;
				XmlElement xmlElement2 = doc.CreateElement(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
				xmlElement2.IsEmpty = isEmptyElement;
				if (xmlReader.MoveToFirstAttribute())
				{
					XmlAttributeCollection attributes = xmlElement2.Attributes;
					do
					{
						XmlAttribute node = LoadAttributeNode();
						attributes.Append(node);
					}
					while (xmlReader.MoveToNextAttribute());
					xmlReader.MoveToElement();
				}
				if (!isEmptyElement)
				{
					xmlNode?.AppendChildForLoad(xmlElement2, doc);
					xmlNode = xmlElement2;
					continue;
				}
				IXmlSchemaInfo schemaInfo = xmlReader.SchemaInfo;
				if (schemaInfo != null)
				{
					xmlElement2.XmlName = doc.AddXmlName(xmlElement2.Prefix, xmlElement2.LocalName, xmlElement2.NamespaceURI, schemaInfo);
				}
				xmlNode2 = xmlElement2;
				break;
			}
			case XmlNodeType.EndElement:
			{
				if (xmlNode == null)
				{
					return null;
				}
				IXmlSchemaInfo schemaInfo = xmlReader.SchemaInfo;
				if (schemaInfo != null && xmlNode is XmlElement xmlElement)
				{
					xmlElement.XmlName = doc.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, schemaInfo);
				}
				if (xmlNode.ParentNode == null)
				{
					return xmlNode;
				}
				xmlNode = xmlNode.ParentNode;
				continue;
			}
			case XmlNodeType.EntityReference:
				xmlNode2 = LoadEntityReferenceNode(direct: false);
				break;
			case XmlNodeType.EndEntity:
				return null;
			case XmlNodeType.Attribute:
				xmlNode2 = LoadAttributeNode();
				break;
			case XmlNodeType.Text:
				xmlNode2 = doc.CreateTextNode(xmlReader.Value);
				break;
			case XmlNodeType.SignificantWhitespace:
				xmlNode2 = doc.CreateSignificantWhitespace(xmlReader.Value);
				break;
			case XmlNodeType.Whitespace:
				if (preserveWhitespace)
				{
					xmlNode2 = doc.CreateWhitespace(xmlReader.Value);
					break;
				}
				if (xmlNode == null && !skipOverWhitespace)
				{
					return null;
				}
				continue;
			case XmlNodeType.CDATA:
				xmlNode2 = doc.CreateCDataSection(xmlReader.Value);
				break;
			case XmlNodeType.XmlDeclaration:
				xmlNode2 = LoadDeclarationNode();
				break;
			case XmlNodeType.ProcessingInstruction:
				xmlNode2 = doc.CreateProcessingInstruction(xmlReader.Name, xmlReader.Value);
				break;
			case XmlNodeType.Comment:
				xmlNode2 = doc.CreateComment(xmlReader.Value);
				break;
			case XmlNodeType.DocumentType:
				xmlNode2 = LoadDocumentTypeNode();
				break;
			default:
				throw UnexpectedNodeType(xmlReader.NodeType);
			}
			if (xmlNode != null)
			{
				xmlNode.AppendChildForLoad(xmlNode2, doc);
				continue;
			}
			return xmlNode2;
		}
		while (xmlReader.Read());
		if (xmlNode != null)
		{
			while (xmlNode.ParentNode != null)
			{
				xmlNode = xmlNode.ParentNode;
			}
		}
		return xmlNode;
	}

	private XmlAttribute LoadAttributeNode()
	{
		XmlReader xmlReader = reader;
		if (xmlReader.IsDefault)
		{
			return LoadDefaultAttribute();
		}
		XmlAttribute xmlAttribute = doc.CreateAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
		IXmlSchemaInfo schemaInfo = xmlReader.SchemaInfo;
		if (schemaInfo != null)
		{
			xmlAttribute.XmlName = doc.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, schemaInfo);
		}
		while (xmlReader.ReadAttributeValue())
		{
			XmlNode xmlNode;
			switch (xmlReader.NodeType)
			{
			case XmlNodeType.Text:
				xmlNode = doc.CreateTextNode(xmlReader.Value);
				break;
			case XmlNodeType.EntityReference:
				xmlNode = doc.CreateEntityReference(xmlReader.LocalName);
				if (xmlReader.CanResolveEntity)
				{
					xmlReader.ResolveEntity();
					LoadAttributeValue(xmlNode, direct: false);
					if (xmlNode.FirstChild == null)
					{
						xmlNode.AppendChildForLoad(doc.CreateTextNode(string.Empty), doc);
					}
				}
				break;
			default:
				throw UnexpectedNodeType(xmlReader.NodeType);
			}
			xmlAttribute.AppendChildForLoad(xmlNode, doc);
		}
		return xmlAttribute;
	}

	private XmlAttribute LoadDefaultAttribute()
	{
		XmlReader xmlReader = reader;
		XmlAttribute xmlAttribute = doc.CreateDefaultAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
		IXmlSchemaInfo schemaInfo = xmlReader.SchemaInfo;
		if (schemaInfo != null)
		{
			xmlAttribute.XmlName = doc.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, schemaInfo);
		}
		LoadAttributeValue(xmlAttribute, direct: false);
		if (xmlAttribute is XmlUnspecifiedAttribute xmlUnspecifiedAttribute)
		{
			xmlUnspecifiedAttribute.SetSpecified(f: false);
		}
		return xmlAttribute;
	}

	private void LoadAttributeValue(XmlNode parent, bool direct)
	{
		XmlReader xmlReader = reader;
		while (xmlReader.ReadAttributeValue())
		{
			XmlNode xmlNode;
			switch (xmlReader.NodeType)
			{
			case XmlNodeType.Text:
				xmlNode = (direct ? new XmlText(xmlReader.Value, doc) : doc.CreateTextNode(xmlReader.Value));
				break;
			case XmlNodeType.EndEntity:
				return;
			case XmlNodeType.EntityReference:
				xmlNode = (direct ? new XmlEntityReference(reader.LocalName, doc) : doc.CreateEntityReference(reader.LocalName));
				if (xmlReader.CanResolveEntity)
				{
					xmlReader.ResolveEntity();
					LoadAttributeValue(xmlNode, direct);
					if (xmlNode.FirstChild == null)
					{
						xmlNode.AppendChildForLoad(direct ? new XmlText(string.Empty) : doc.CreateTextNode(string.Empty), doc);
					}
				}
				break;
			default:
				throw UnexpectedNodeType(xmlReader.NodeType);
			}
			parent.AppendChildForLoad(xmlNode, doc);
		}
	}

	private XmlEntityReference LoadEntityReferenceNode(bool direct)
	{
		XmlEntityReference xmlEntityReference = (direct ? new XmlEntityReference(reader.Name, doc) : doc.CreateEntityReference(reader.Name));
		if (reader.CanResolveEntity)
		{
			reader.ResolveEntity();
			while (reader.Read() && reader.NodeType != XmlNodeType.EndEntity)
			{
				XmlNode xmlNode = (direct ? LoadNodeDirect() : LoadNode(skipOverWhitespace: false));
				if (xmlNode != null)
				{
					xmlEntityReference.AppendChildForLoad(xmlNode, doc);
				}
			}
			if (xmlEntityReference.LastChild == null)
			{
				xmlEntityReference.AppendChildForLoad(doc.CreateTextNode(string.Empty), doc);
			}
		}
		return xmlEntityReference;
	}

	private XmlDeclaration LoadDeclarationNode()
	{
		string version = null;
		string encoding = null;
		string standalone = null;
		while (reader.MoveToNextAttribute())
		{
			switch (reader.Name)
			{
			case "version":
				version = reader.Value;
				break;
			case "encoding":
				encoding = reader.Value;
				break;
			case "standalone":
				standalone = reader.Value;
				break;
			}
		}
		if (version == null)
		{
			ParseXmlDeclarationValue(reader.Value, out version, out encoding, out standalone);
		}
		return doc.CreateXmlDeclaration(version, encoding, standalone);
	}

	private XmlDocumentType LoadDocumentTypeNode()
	{
		string publicId = null;
		string systemId = null;
		string value = reader.Value;
		string localName = reader.LocalName;
		while (reader.MoveToNextAttribute())
		{
			string name = reader.Name;
			if (!(name == "PUBLIC"))
			{
				if (name == "SYSTEM")
				{
					systemId = reader.Value;
				}
			}
			else
			{
				publicId = reader.Value;
			}
		}
		XmlDocumentType xmlDocumentType = doc.CreateDocumentType(localName, publicId, systemId, value);
		IDtdInfo dtdInfo = reader.DtdInfo;
		if (dtdInfo != null)
		{
			LoadDocumentType(dtdInfo, xmlDocumentType);
		}
		else
		{
			ParseDocumentType(xmlDocumentType);
		}
		return xmlDocumentType;
	}

	private XmlNode LoadNodeDirect()
	{
		XmlReader xmlReader = reader;
		XmlNode xmlNode = null;
		do
		{
			XmlNode xmlNode2 = null;
			switch (xmlReader.NodeType)
			{
			case XmlNodeType.Element:
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlElement xmlElement = new XmlElement(reader.Prefix, reader.LocalName, reader.NamespaceURI, doc);
				xmlElement.IsEmpty = isEmptyElement;
				if (reader.MoveToFirstAttribute())
				{
					XmlAttributeCollection attributes = xmlElement.Attributes;
					do
					{
						XmlAttribute node = LoadAttributeNodeDirect();
						attributes.Append(node);
					}
					while (xmlReader.MoveToNextAttribute());
				}
				if (!isEmptyElement)
				{
					xmlNode.AppendChildForLoad(xmlElement, doc);
					xmlNode = xmlElement;
					continue;
				}
				xmlNode2 = xmlElement;
				break;
			}
			case XmlNodeType.EndElement:
				if (xmlNode.ParentNode == null)
				{
					return xmlNode;
				}
				xmlNode = xmlNode.ParentNode;
				continue;
			case XmlNodeType.EntityReference:
				xmlNode2 = LoadEntityReferenceNode(direct: true);
				break;
			case XmlNodeType.Attribute:
				xmlNode2 = LoadAttributeNodeDirect();
				break;
			case XmlNodeType.SignificantWhitespace:
				xmlNode2 = new XmlSignificantWhitespace(reader.Value, doc);
				break;
			case XmlNodeType.Whitespace:
				if (preserveWhitespace)
				{
					xmlNode2 = new XmlWhitespace(reader.Value, doc);
					break;
				}
				continue;
			case XmlNodeType.Text:
				xmlNode2 = new XmlText(reader.Value, doc);
				break;
			case XmlNodeType.CDATA:
				xmlNode2 = new XmlCDataSection(reader.Value, doc);
				break;
			case XmlNodeType.ProcessingInstruction:
				xmlNode2 = new XmlProcessingInstruction(reader.Name, reader.Value, doc);
				break;
			case XmlNodeType.Comment:
				xmlNode2 = new XmlComment(reader.Value, doc);
				break;
			default:
				throw UnexpectedNodeType(reader.NodeType);
			case XmlNodeType.EndEntity:
				continue;
			}
			if (xmlNode != null)
			{
				xmlNode.AppendChildForLoad(xmlNode2, doc);
				continue;
			}
			return xmlNode2;
		}
		while (xmlReader.Read());
		return null;
	}

	private XmlAttribute LoadAttributeNodeDirect()
	{
		XmlReader xmlReader = reader;
		if (xmlReader.IsDefault)
		{
			XmlUnspecifiedAttribute xmlUnspecifiedAttribute = new XmlUnspecifiedAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI, doc);
			LoadAttributeValue(xmlUnspecifiedAttribute, direct: true);
			xmlUnspecifiedAttribute.SetSpecified(f: false);
			return xmlUnspecifiedAttribute;
		}
		XmlAttribute xmlAttribute = new XmlAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI, doc);
		LoadAttributeValue(xmlAttribute, direct: true);
		return xmlAttribute;
	}

	internal void ParseDocumentType(XmlDocumentType dtNode)
	{
		XmlDocument ownerDocument = dtNode.OwnerDocument;
		if (ownerDocument.HasSetResolver)
		{
			ParseDocumentType(dtNode, bUseResolver: true, ownerDocument.GetResolver());
		}
		else
		{
			ParseDocumentType(dtNode, bUseResolver: false, null);
		}
	}

	private void ParseDocumentType(XmlDocumentType dtNode, bool bUseResolver, XmlResolver resolver)
	{
		doc = dtNode.OwnerDocument;
		XmlParserContext context = new XmlParserContext(null, new XmlNamespaceManager(doc.NameTable), null, null, null, null, doc.BaseURI, string.Empty, XmlSpace.None);
		XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl("", XmlNodeType.Element, context);
		xmlTextReaderImpl.Namespaces = dtNode.ParseWithNamespaces;
		if (bUseResolver)
		{
			xmlTextReaderImpl.XmlResolver = resolver;
		}
		IDtdInfo dtdInfo = DtdParser.Create().ParseFreeFloatingDtd(adapter: new XmlTextReaderImpl.DtdParserProxy(xmlTextReaderImpl), baseUri: doc.BaseURI, docTypeName: dtNode.Name, publicId: dtNode.PublicId, systemId: dtNode.SystemId, internalSubset: dtNode.InternalSubset);
		LoadDocumentType(dtdInfo, dtNode);
	}

	private void LoadDocumentType(IDtdInfo dtdInfo, XmlDocumentType dtNode)
	{
		if (!(dtdInfo is SchemaInfo schemaInfo))
		{
			throw new XmlException("An internal error has occurred.", string.Empty);
		}
		dtNode.DtdSchemaInfo = schemaInfo;
		if (schemaInfo == null)
		{
			return;
		}
		doc.DtdSchemaInfo = schemaInfo;
		if (schemaInfo.Notations != null)
		{
			foreach (SchemaNotation value in schemaInfo.Notations.Values)
			{
				dtNode.Notations.SetNamedItem(new XmlNotation(value.Name.Name, value.Pubid, value.SystemLiteral, doc));
			}
		}
		if (schemaInfo.GeneralEntities != null)
		{
			foreach (SchemaEntity value2 in schemaInfo.GeneralEntities.Values)
			{
				XmlEntity xmlEntity = new XmlEntity(value2.Name.Name, value2.Text, value2.Pubid, value2.Url, value2.NData.IsEmpty ? null : value2.NData.Name, doc);
				xmlEntity.SetBaseURI(value2.DeclaredURI);
				dtNode.Entities.SetNamedItem(xmlEntity);
			}
		}
		if (schemaInfo.ParameterEntities != null)
		{
			foreach (SchemaEntity value3 in schemaInfo.ParameterEntities.Values)
			{
				XmlEntity xmlEntity2 = new XmlEntity(value3.Name.Name, value3.Text, value3.Pubid, value3.Url, value3.NData.IsEmpty ? null : value3.NData.Name, doc);
				xmlEntity2.SetBaseURI(value3.DeclaredURI);
				dtNode.Entities.SetNamedItem(xmlEntity2);
			}
		}
		doc.Entities = dtNode.Entities;
		IDictionaryEnumerator dictionaryEnumerator = schemaInfo.ElementDecls.GetEnumerator();
		if (dictionaryEnumerator == null)
		{
			return;
		}
		dictionaryEnumerator.Reset();
		while (dictionaryEnumerator.MoveNext())
		{
			SchemaElementDecl schemaElementDecl = (SchemaElementDecl)dictionaryEnumerator.Value;
			if (schemaElementDecl.AttDefs == null)
			{
				continue;
			}
			IDictionaryEnumerator dictionaryEnumerator2 = schemaElementDecl.AttDefs.GetEnumerator();
			while (dictionaryEnumerator2.MoveNext())
			{
				SchemaAttDef schemaAttDef = (SchemaAttDef)dictionaryEnumerator2.Value;
				if (schemaAttDef.Datatype.TokenizedType == XmlTokenizedType.ID)
				{
					doc.AddIdInfo(doc.AddXmlName(schemaElementDecl.Prefix, schemaElementDecl.Name.Name, string.Empty, null), doc.AddAttrXmlName(schemaAttDef.Prefix, schemaAttDef.Name.Name, string.Empty, null));
					break;
				}
			}
		}
	}

	private XmlParserContext GetContext(XmlNode node)
	{
		string text = null;
		XmlSpace xmlSpace = XmlSpace.None;
		XmlDocumentType documentType = doc.DocumentType;
		string baseURI = doc.BaseURI;
		Hashtable hashtable = new Hashtable();
		XmlNameTable nameTable = doc.NameTable;
		XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
		bool flag = false;
		while (node != null && node != doc)
		{
			if (node is XmlElement && ((XmlElement)node).HasAttributes)
			{
				xmlNamespaceManager.PushScope();
				foreach (XmlAttribute attribute in ((XmlElement)node).Attributes)
				{
					if (attribute.Prefix == doc.strXmlns && !hashtable.Contains(attribute.LocalName))
					{
						hashtable.Add(attribute.LocalName, attribute.LocalName);
						xmlNamespaceManager.AddNamespace(attribute.LocalName, attribute.Value);
					}
					else if (!flag && attribute.Prefix.Length == 0 && attribute.LocalName == doc.strXmlns)
					{
						xmlNamespaceManager.AddNamespace(string.Empty, attribute.Value);
						flag = true;
					}
					else if (xmlSpace == XmlSpace.None && attribute.Prefix == doc.strXml && attribute.LocalName == doc.strSpace)
					{
						if (attribute.Value == "default")
						{
							xmlSpace = XmlSpace.Default;
						}
						else if (attribute.Value == "preserve")
						{
							xmlSpace = XmlSpace.Preserve;
						}
					}
					else if (text == null && attribute.Prefix == doc.strXml && attribute.LocalName == doc.strLang)
					{
						text = attribute.Value;
					}
				}
			}
			node = node.ParentNode;
		}
		return new XmlParserContext(nameTable, xmlNamespaceManager, documentType?.Name, documentType?.PublicId, documentType?.SystemId, documentType?.InternalSubset, baseURI, text, xmlSpace);
	}

	internal XmlNamespaceManager ParsePartialContent(XmlNode parentNode, string innerxmltext, XmlNodeType nt)
	{
		doc = parentNode.OwnerDocument;
		XmlParserContext context = GetContext(parentNode);
		reader = CreateInnerXmlReader(innerxmltext, nt, context, doc);
		try
		{
			preserveWhitespace = true;
			bool isLoading = doc.IsLoading;
			doc.IsLoading = true;
			if (nt == XmlNodeType.Entity)
			{
				XmlNode xmlNode = null;
				while (reader.Read() && (xmlNode = LoadNodeDirect()) != null)
				{
					parentNode.AppendChildForLoad(xmlNode, doc);
				}
			}
			else
			{
				XmlNode xmlNode2 = null;
				while (reader.Read() && (xmlNode2 = LoadNode(skipOverWhitespace: true)) != null)
				{
					parentNode.AppendChildForLoad(xmlNode2, doc);
				}
			}
			doc.IsLoading = isLoading;
		}
		finally
		{
			reader.Close();
		}
		return context.NamespaceManager;
	}

	internal void LoadInnerXmlElement(XmlElement node, string innerxmltext)
	{
		XmlNamespaceManager mgr = ParsePartialContent(node, innerxmltext, XmlNodeType.Element);
		if (node.ChildNodes.Count > 0)
		{
			RemoveDuplicateNamespace(node, mgr, fCheckElemAttrs: false);
		}
	}

	internal void LoadInnerXmlAttribute(XmlAttribute node, string innerxmltext)
	{
		ParsePartialContent(node, innerxmltext, XmlNodeType.Attribute);
	}

	private void RemoveDuplicateNamespace(XmlElement elem, XmlNamespaceManager mgr, bool fCheckElemAttrs)
	{
		mgr.PushScope();
		XmlAttributeCollection attributes = elem.Attributes;
		int count = attributes.Count;
		if (fCheckElemAttrs && count > 0)
		{
			for (int num = count - 1; num >= 0; num--)
			{
				XmlAttribute xmlAttribute = attributes[num];
				if (xmlAttribute.Prefix == doc.strXmlns)
				{
					string text = mgr.LookupNamespace(xmlAttribute.LocalName);
					if (text != null)
					{
						if (xmlAttribute.Value == text)
						{
							elem.Attributes.RemoveNodeAt(num);
						}
					}
					else
					{
						mgr.AddNamespace(xmlAttribute.LocalName, xmlAttribute.Value);
					}
				}
				else if (xmlAttribute.Prefix.Length == 0 && xmlAttribute.LocalName == doc.strXmlns)
				{
					string defaultNamespace = mgr.DefaultNamespace;
					if (defaultNamespace != null)
					{
						if (xmlAttribute.Value == defaultNamespace)
						{
							elem.Attributes.RemoveNodeAt(num);
						}
					}
					else
					{
						mgr.AddNamespace(xmlAttribute.LocalName, xmlAttribute.Value);
					}
				}
			}
		}
		for (XmlNode xmlNode = elem.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode is XmlElement elem2)
			{
				RemoveDuplicateNamespace(elem2, mgr, fCheckElemAttrs: true);
			}
		}
		mgr.PopScope();
	}

	private string EntitizeName(string name)
	{
		return "&" + name + ";";
	}

	internal void ExpandEntity(XmlEntity ent)
	{
		ParsePartialContent(ent, EntitizeName(ent.Name), XmlNodeType.Entity);
	}

	internal void ExpandEntityReference(XmlEntityReference eref)
	{
		doc = eref.OwnerDocument;
		bool isLoading = doc.IsLoading;
		doc.IsLoading = true;
		switch (eref.Name)
		{
		case "lt":
			eref.AppendChildForLoad(doc.CreateTextNode("<"), doc);
			doc.IsLoading = isLoading;
			return;
		case "gt":
			eref.AppendChildForLoad(doc.CreateTextNode(">"), doc);
			doc.IsLoading = isLoading;
			return;
		case "amp":
			eref.AppendChildForLoad(doc.CreateTextNode("&"), doc);
			doc.IsLoading = isLoading;
			return;
		case "apos":
			eref.AppendChildForLoad(doc.CreateTextNode("'"), doc);
			doc.IsLoading = isLoading;
			return;
		case "quot":
			eref.AppendChildForLoad(doc.CreateTextNode("\""), doc);
			doc.IsLoading = isLoading;
			return;
		}
		foreach (XmlEntity entity in doc.Entities)
		{
			if (Ref.Equal(entity.Name, eref.Name))
			{
				ParsePartialContent(eref, EntitizeName(eref.Name), XmlNodeType.EntityReference);
				return;
			}
		}
		if (!doc.ActualLoadingStatus)
		{
			eref.AppendChildForLoad(doc.CreateTextNode(""), doc);
			doc.IsLoading = isLoading;
			return;
		}
		doc.IsLoading = isLoading;
		throw new XmlException("Reference to undeclared parameter entity '{0}'.", eref.Name);
	}

	private XmlReader CreateInnerXmlReader(string xmlFragment, XmlNodeType nt, XmlParserContext context, XmlDocument doc)
	{
		XmlNodeType xmlNodeType = nt;
		if (xmlNodeType == XmlNodeType.Entity || xmlNodeType == XmlNodeType.EntityReference)
		{
			xmlNodeType = XmlNodeType.Element;
		}
		XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(xmlFragment, xmlNodeType, context);
		xmlTextReaderImpl.XmlValidatingReaderCompatibilityMode = true;
		if (doc.HasSetResolver)
		{
			xmlTextReaderImpl.XmlResolver = doc.GetResolver();
		}
		if (!doc.ActualLoadingStatus)
		{
			xmlTextReaderImpl.DisableUndeclaredEntityCheck = true;
		}
		XmlDocumentType documentType = doc.DocumentType;
		if (documentType != null)
		{
			xmlTextReaderImpl.Namespaces = documentType.ParseWithNamespaces;
			if (documentType.DtdSchemaInfo != null)
			{
				xmlTextReaderImpl.SetDtdInfo(documentType.DtdSchemaInfo);
			}
			else
			{
				IDtdInfo dtdInfo = DtdParser.Create().ParseFreeFloatingDtd(adapter: new XmlTextReaderImpl.DtdParserProxy(xmlTextReaderImpl), baseUri: context.BaseURI, docTypeName: context.DocTypeName, publicId: context.PublicId, systemId: context.SystemId, internalSubset: context.InternalSubset);
				documentType.DtdSchemaInfo = dtdInfo as SchemaInfo;
				xmlTextReaderImpl.SetDtdInfo(dtdInfo);
			}
		}
		if (nt == XmlNodeType.Entity || nt == XmlNodeType.EntityReference)
		{
			xmlTextReaderImpl.Read();
			xmlTextReaderImpl.ResolveEntity();
		}
		return xmlTextReaderImpl;
	}

	internal static void ParseXmlDeclarationValue(string strValue, out string version, out string encoding, out string standalone)
	{
		version = null;
		encoding = null;
		standalone = null;
		XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(strValue, (XmlParserContext)null);
		try
		{
			xmlTextReaderImpl.Read();
			if (xmlTextReaderImpl.MoveToAttribute("version"))
			{
				version = xmlTextReaderImpl.Value;
			}
			if (xmlTextReaderImpl.MoveToAttribute("encoding"))
			{
				encoding = xmlTextReaderImpl.Value;
			}
			if (xmlTextReaderImpl.MoveToAttribute("standalone"))
			{
				standalone = xmlTextReaderImpl.Value;
			}
		}
		finally
		{
			xmlTextReaderImpl.Close();
		}
	}

	internal static Exception UnexpectedNodeType(XmlNodeType nodetype)
	{
		return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Res.GetString("Unexpected XmlNodeType: '{0}'."), nodetype.ToString()));
	}
}
