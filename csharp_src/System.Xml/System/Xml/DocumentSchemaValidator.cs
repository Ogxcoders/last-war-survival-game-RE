using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

internal sealed class DocumentSchemaValidator : IXmlNamespaceResolver
{
	private XmlSchemaValidator validator;

	private XmlSchemaSet schemas;

	private XmlNamespaceManager nsManager;

	private XmlNameTable nameTable;

	private ArrayList defaultAttributes;

	private XmlValueGetter nodeValueGetter;

	private XmlSchemaInfo attributeSchemaInfo;

	private XmlSchemaInfo schemaInfo;

	private ValidationEventHandler eventHandler;

	private ValidationEventHandler internalEventHandler;

	private XmlNode startNode;

	private XmlNode currentNode;

	private XmlDocument document;

	private XmlNode[] nodeSequenceToValidate;

	private bool isPartialTreeValid;

	private bool psviAugmentation;

	private bool isValid;

	private string NsXmlNs;

	private string NsXsi;

	private string XsiType;

	private string XsiNil;

	public bool PsviAugmentation
	{
		get
		{
			return psviAugmentation;
		}
		set
		{
			psviAugmentation = value;
		}
	}

	private IXmlNamespaceResolver NamespaceResolver
	{
		get
		{
			if (startNode == document)
			{
				return nsManager;
			}
			return this;
		}
	}

	public DocumentSchemaValidator(XmlDocument ownerDocument, XmlSchemaSet schemas, ValidationEventHandler eventHandler)
	{
		this.schemas = schemas;
		this.eventHandler = eventHandler;
		document = ownerDocument;
		internalEventHandler = InternalValidationCallBack;
		nameTable = document.NameTable;
		nsManager = new XmlNamespaceManager(nameTable);
		nodeValueGetter = GetNodeValue;
		psviAugmentation = true;
		NsXmlNs = nameTable.Add("http://www.w3.org/2000/xmlns/");
		NsXsi = nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
		XsiType = nameTable.Add("type");
		XsiNil = nameTable.Add("nil");
	}

	public bool Validate(XmlNode nodeToValidate)
	{
		XmlSchemaObject xmlSchemaObject = null;
		XmlSchemaValidationFlags xmlSchemaValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes;
		startNode = nodeToValidate;
		XmlNodeType nodeType = nodeToValidate.NodeType;
		if (nodeType <= XmlNodeType.Attribute)
		{
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute || nodeToValidate.XPNodeType == XPathNodeType.Namespace)
				{
					goto IL_00fe;
				}
				xmlSchemaObject = nodeToValidate.SchemaInfo.SchemaAttribute;
				if (xmlSchemaObject == null)
				{
					xmlSchemaObject = FindSchemaInfo(nodeToValidate as XmlAttribute);
					if (xmlSchemaObject == null)
					{
						throw new XmlSchemaValidationException("Schema information could not be found for the node passed into Validate. The node may be invalid in its current position. Navigate to the ancestor that has schema information, then call Validate again.", null, nodeToValidate);
					}
				}
			}
			else
			{
				IXmlSchemaInfo xmlSchemaInfo = nodeToValidate.SchemaInfo;
				XmlSchemaElement schemaElement = xmlSchemaInfo.SchemaElement;
				if (schemaElement != null)
				{
					xmlSchemaObject = (schemaElement.RefName.IsEmpty ? schemaElement : schemas.GlobalElements[schemaElement.QualifiedName]);
				}
				else
				{
					xmlSchemaObject = xmlSchemaInfo.SchemaType;
					if (xmlSchemaObject == null)
					{
						if (nodeToValidate.ParentNode.NodeType == XmlNodeType.Document)
						{
							nodeToValidate = nodeToValidate.ParentNode;
						}
						else
						{
							xmlSchemaObject = FindSchemaInfo(nodeToValidate as XmlElement);
							if (xmlSchemaObject == null)
							{
								throw new XmlSchemaValidationException("Schema information could not be found for the node passed into Validate. The node may be invalid in its current position. Navigate to the ancestor that has schema information, then call Validate again.", null, nodeToValidate);
							}
						}
					}
				}
			}
		}
		else if (nodeType != XmlNodeType.Document)
		{
			if (nodeType != XmlNodeType.DocumentFragment)
			{
				goto IL_00fe;
			}
		}
		else
		{
			xmlSchemaValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;
		}
		isValid = true;
		CreateValidator(xmlSchemaObject, xmlSchemaValidationFlags);
		if (psviAugmentation)
		{
			if (schemaInfo == null)
			{
				schemaInfo = new XmlSchemaInfo();
			}
			attributeSchemaInfo = new XmlSchemaInfo();
		}
		ValidateNode(nodeToValidate);
		validator.EndValidation();
		return isValid;
		IL_00fe:
		throw new InvalidOperationException(Res.GetString("Validate method can be called only on nodes of type Document, DocumentFragment, Element, or Attribute.", null));
	}

	public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		IDictionary<string, string> namespacesInScope = nsManager.GetNamespacesInScope(scope);
		if (scope != XmlNamespaceScope.Local)
		{
			XmlNode xmlNode = startNode;
			while (xmlNode != null)
			{
				switch (xmlNode.NodeType)
				{
				case XmlNodeType.Element:
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (xmlElement.HasAttributes)
					{
						XmlAttributeCollection attributes = xmlElement.Attributes;
						for (int i = 0; i < attributes.Count; i++)
						{
							XmlAttribute xmlAttribute = attributes[i];
							if (!Ref.Equal(xmlAttribute.NamespaceURI, document.strReservedXmlns))
							{
								continue;
							}
							if (xmlAttribute.Prefix.Length == 0)
							{
								if (!namespacesInScope.ContainsKey(string.Empty))
								{
									namespacesInScope.Add(string.Empty, xmlAttribute.Value);
								}
							}
							else if (!namespacesInScope.ContainsKey(xmlAttribute.LocalName))
							{
								namespacesInScope.Add(xmlAttribute.LocalName, xmlAttribute.Value);
							}
						}
					}
					xmlNode = xmlNode.ParentNode;
					break;
				}
				case XmlNodeType.Attribute:
					xmlNode = ((XmlAttribute)xmlNode).OwnerElement;
					break;
				default:
					xmlNode = xmlNode.ParentNode;
					break;
				}
			}
		}
		return namespacesInScope;
	}

	public string LookupNamespace(string prefix)
	{
		string text = nsManager.LookupNamespace(prefix);
		if (text == null)
		{
			text = startNode.GetNamespaceOfPrefixStrict(prefix);
		}
		return text;
	}

	public string LookupPrefix(string namespaceName)
	{
		string text = nsManager.LookupPrefix(namespaceName);
		if (text == null)
		{
			text = startNode.GetPrefixOfNamespaceStrict(namespaceName);
		}
		return text;
	}

	private void CreateValidator(XmlSchemaObject partialValidationType, XmlSchemaValidationFlags validationFlags)
	{
		validator = new XmlSchemaValidator(nameTable, schemas, NamespaceResolver, validationFlags);
		validator.SourceUri = XmlConvert.ToUri(document.BaseURI);
		validator.XmlResolver = null;
		validator.ValidationEventHandler += internalEventHandler;
		validator.ValidationEventSender = this;
		if (partialValidationType != null)
		{
			validator.Initialize(partialValidationType);
		}
		else
		{
			validator.Initialize();
		}
	}

	private void ValidateNode(XmlNode node)
	{
		currentNode = node;
		switch (currentNode.NodeType)
		{
		case XmlNodeType.Document:
		{
			XmlElement documentElement = ((XmlDocument)node).DocumentElement;
			if (documentElement == null)
			{
				throw new InvalidOperationException(Res.GetString("Invalid XML document. {0}", Res.GetString("The document does not have a root element.")));
			}
			ValidateNode(documentElement);
			break;
		}
		case XmlNodeType.EntityReference:
		case XmlNodeType.DocumentFragment:
		{
			for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				ValidateNode(xmlNode);
			}
			break;
		}
		case XmlNodeType.Element:
			ValidateElement();
			break;
		case XmlNodeType.Attribute:
		{
			XmlAttribute xmlAttribute = currentNode as XmlAttribute;
			validator.ValidateAttribute(xmlAttribute.LocalName, xmlAttribute.NamespaceURI, nodeValueGetter, attributeSchemaInfo);
			if (psviAugmentation)
			{
				xmlAttribute.XmlName = document.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, attributeSchemaInfo);
			}
			break;
		}
		case XmlNodeType.Text:
			validator.ValidateText(nodeValueGetter);
			break;
		case XmlNodeType.CDATA:
			validator.ValidateText(nodeValueGetter);
			break;
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			validator.ValidateWhitespace(nodeValueGetter);
			break;
		default:
		{
			object[] args = new string[1] { currentNode.NodeType.ToString() };
			throw new InvalidOperationException(Res.GetString("Unexpected XmlNodeType: '{0}'.", args));
		}
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.Comment:
			break;
		}
	}

	private void ValidateElement()
	{
		nsManager.PushScope();
		XmlElement xmlElement = currentNode as XmlElement;
		XmlAttributeCollection attributes = xmlElement.Attributes;
		XmlAttribute xmlAttribute = null;
		string xsiNil = null;
		string xsiType = null;
		for (int i = 0; i < attributes.Count; i++)
		{
			xmlAttribute = attributes[i];
			string namespaceURI = xmlAttribute.NamespaceURI;
			string localName = xmlAttribute.LocalName;
			if (Ref.Equal(namespaceURI, NsXsi))
			{
				if (Ref.Equal(localName, XsiType))
				{
					xsiType = xmlAttribute.Value;
				}
				else if (Ref.Equal(localName, XsiNil))
				{
					xsiNil = xmlAttribute.Value;
				}
			}
			else if (Ref.Equal(namespaceURI, NsXmlNs))
			{
				nsManager.AddNamespace((xmlAttribute.Prefix.Length == 0) ? string.Empty : xmlAttribute.LocalName, xmlAttribute.Value);
			}
		}
		validator.ValidateElement(xmlElement.LocalName, xmlElement.NamespaceURI, schemaInfo, xsiType, xsiNil, null, null);
		ValidateAttributes(xmlElement);
		validator.ValidateEndOfAttributes(schemaInfo);
		for (XmlNode xmlNode = xmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			ValidateNode(xmlNode);
		}
		currentNode = xmlElement;
		validator.ValidateEndElement(schemaInfo);
		if (psviAugmentation)
		{
			xmlElement.XmlName = document.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, schemaInfo);
			if (schemaInfo.IsDefault)
			{
				XmlText newChild = document.CreateTextNode(schemaInfo.SchemaElement.ElementDecl.DefaultValueRaw);
				xmlElement.AppendChild(newChild);
			}
		}
		nsManager.PopScope();
	}

	private void ValidateAttributes(XmlElement elementNode)
	{
		XmlAttributeCollection attributes = elementNode.Attributes;
		XmlAttribute xmlAttribute = null;
		for (int i = 0; i < attributes.Count; i++)
		{
			xmlAttribute = (XmlAttribute)(currentNode = attributes[i]);
			if (!Ref.Equal(xmlAttribute.NamespaceURI, NsXmlNs))
			{
				validator.ValidateAttribute(xmlAttribute.LocalName, xmlAttribute.NamespaceURI, nodeValueGetter, attributeSchemaInfo);
				if (psviAugmentation)
				{
					xmlAttribute.XmlName = document.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, attributeSchemaInfo);
				}
			}
		}
		if (!psviAugmentation)
		{
			return;
		}
		if (defaultAttributes == null)
		{
			defaultAttributes = new ArrayList();
		}
		else
		{
			defaultAttributes.Clear();
		}
		validator.GetUnspecifiedDefaultAttributes(defaultAttributes);
		XmlSchemaAttribute xmlSchemaAttribute = null;
		xmlAttribute = null;
		for (int j = 0; j < defaultAttributes.Count; j++)
		{
			xmlSchemaAttribute = defaultAttributes[j] as XmlSchemaAttribute;
			XmlQualifiedName qualifiedName = xmlSchemaAttribute.QualifiedName;
			xmlAttribute = document.CreateDefaultAttribute(GetDefaultPrefix(qualifiedName.Namespace), qualifiedName.Name, qualifiedName.Namespace);
			SetDefaultAttributeSchemaInfo(xmlSchemaAttribute);
			xmlAttribute.XmlName = document.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, attributeSchemaInfo);
			xmlAttribute.AppendChild(document.CreateTextNode(xmlSchemaAttribute.AttDef.DefaultValueRaw));
			attributes.Append(xmlAttribute);
			if (xmlAttribute is XmlUnspecifiedAttribute xmlUnspecifiedAttribute)
			{
				xmlUnspecifiedAttribute.SetSpecified(f: false);
			}
		}
	}

	private void SetDefaultAttributeSchemaInfo(XmlSchemaAttribute schemaAttribute)
	{
		attributeSchemaInfo.Clear();
		attributeSchemaInfo.IsDefault = true;
		attributeSchemaInfo.IsNil = false;
		attributeSchemaInfo.SchemaType = schemaAttribute.AttributeSchemaType;
		attributeSchemaInfo.SchemaAttribute = schemaAttribute;
		SchemaAttDef attDef = schemaAttribute.AttDef;
		if (attDef.Datatype.Variety == XmlSchemaDatatypeVariety.Union)
		{
			XsdSimpleValue xsdSimpleValue = attDef.DefaultValueTyped as XsdSimpleValue;
			attributeSchemaInfo.MemberType = xsdSimpleValue.XmlType;
		}
		attributeSchemaInfo.Validity = XmlSchemaValidity.Valid;
	}

	private string GetDefaultPrefix(string attributeNS)
	{
		IDictionary<string, string> namespacesInScope = NamespaceResolver.GetNamespacesInScope(XmlNamespaceScope.All);
		string text = null;
		attributeNS = nameTable.Add(attributeNS);
		foreach (KeyValuePair<string, string> item in namespacesInScope)
		{
			if ((object)nameTable.Add(item.Value) == attributeNS)
			{
				text = item.Key;
				if (text.Length != 0)
				{
					return text;
				}
			}
		}
		return text;
	}

	private object GetNodeValue()
	{
		return currentNode.Value;
	}

	private XmlSchemaObject FindSchemaInfo(XmlElement elementToValidate)
	{
		isPartialTreeValid = true;
		IXmlSchemaInfo xmlSchemaInfo = null;
		int num = 0;
		XmlNode parentNode = elementToValidate.ParentNode;
		do
		{
			xmlSchemaInfo = parentNode.SchemaInfo;
			if (xmlSchemaInfo.SchemaElement != null || xmlSchemaInfo.SchemaType != null)
			{
				break;
			}
			CheckNodeSequenceCapacity(num);
			nodeSequenceToValidate[num++] = parentNode;
			parentNode = parentNode.ParentNode;
		}
		while (parentNode != null);
		if (parentNode == null)
		{
			num--;
			nodeSequenceToValidate[num] = null;
			return GetTypeFromAncestors(elementToValidate, null, num);
		}
		CheckNodeSequenceCapacity(num);
		nodeSequenceToValidate[num++] = parentNode;
		XmlSchemaObject xmlSchemaObject = xmlSchemaInfo.SchemaElement;
		if (xmlSchemaObject == null)
		{
			xmlSchemaObject = xmlSchemaInfo.SchemaType;
		}
		return GetTypeFromAncestors(elementToValidate, xmlSchemaObject, num);
	}

	private void CheckNodeSequenceCapacity(int currentIndex)
	{
		if (nodeSequenceToValidate == null)
		{
			nodeSequenceToValidate = new XmlNode[4];
		}
		else if (currentIndex >= nodeSequenceToValidate.Length - 1)
		{
			XmlNode[] destinationArray = new XmlNode[nodeSequenceToValidate.Length * 2];
			Array.Copy(nodeSequenceToValidate, 0, destinationArray, 0, nodeSequenceToValidate.Length);
			nodeSequenceToValidate = destinationArray;
		}
	}

	private XmlSchemaAttribute FindSchemaInfo(XmlAttribute attributeToValidate)
	{
		XmlElement ownerElement = attributeToValidate.OwnerElement;
		XmlSchemaObject schemaObject = FindSchemaInfo(ownerElement);
		XmlSchemaComplexType complexType = GetComplexType(schemaObject);
		if (complexType == null)
		{
			return null;
		}
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(attributeToValidate.LocalName, attributeToValidate.NamespaceURI);
		XmlSchemaAttribute xmlSchemaAttribute = complexType.AttributeUses[xmlQualifiedName] as XmlSchemaAttribute;
		if (xmlSchemaAttribute == null)
		{
			XmlSchemaAnyAttribute attributeWildcard = complexType.AttributeWildcard;
			if (attributeWildcard != null && attributeWildcard.NamespaceList.Allows(xmlQualifiedName))
			{
				xmlSchemaAttribute = schemas.GlobalAttributes[xmlQualifiedName] as XmlSchemaAttribute;
			}
		}
		return xmlSchemaAttribute;
	}

	private XmlSchemaObject GetTypeFromAncestors(XmlElement elementToValidate, XmlSchemaObject ancestorType, int ancestorsCount)
	{
		validator = CreateTypeFinderValidator(ancestorType);
		schemaInfo = new XmlSchemaInfo();
		int num = ancestorsCount - 1;
		bool flag = AncestorTypeHasWildcard(ancestorType);
		for (int num2 = num; num2 >= 0; num2--)
		{
			XmlNode xmlNode = nodeSequenceToValidate[num2];
			XmlElement xmlElement = xmlNode as XmlElement;
			ValidateSingleElement(xmlElement, skipToEnd: false, schemaInfo);
			if (!flag)
			{
				xmlElement.XmlName = document.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, schemaInfo);
				flag = AncestorTypeHasWildcard(schemaInfo.SchemaElement);
			}
			validator.ValidateEndOfAttributes(null);
			if (num2 > 0)
			{
				ValidateChildrenTillNextAncestor(xmlNode, nodeSequenceToValidate[num2 - 1]);
			}
			else
			{
				ValidateChildrenTillNextAncestor(xmlNode, elementToValidate);
			}
		}
		ValidateSingleElement(elementToValidate, skipToEnd: false, schemaInfo);
		XmlSchemaObject xmlSchemaObject = null;
		xmlSchemaObject = ((schemaInfo.SchemaElement == null) ? ((XmlSchemaAnnotated)schemaInfo.SchemaType) : ((XmlSchemaAnnotated)schemaInfo.SchemaElement));
		if (xmlSchemaObject == null)
		{
			if (validator.CurrentProcessContents == XmlSchemaContentProcessing.Skip)
			{
				if (isPartialTreeValid)
				{
					return XmlSchemaComplexType.AnyTypeSkip;
				}
			}
			else if (validator.CurrentProcessContents == XmlSchemaContentProcessing.Lax)
			{
				return XmlSchemaComplexType.AnyType;
			}
		}
		return xmlSchemaObject;
	}

	private bool AncestorTypeHasWildcard(XmlSchemaObject ancestorType)
	{
		XmlSchemaComplexType complexType = GetComplexType(ancestorType);
		if (ancestorType != null)
		{
			return complexType.HasWildCard;
		}
		return false;
	}

	private XmlSchemaComplexType GetComplexType(XmlSchemaObject schemaObject)
	{
		if (schemaObject == null)
		{
			return null;
		}
		XmlSchemaElement xmlSchemaElement = schemaObject as XmlSchemaElement;
		XmlSchemaComplexType xmlSchemaComplexType = null;
		if (xmlSchemaElement != null)
		{
			return xmlSchemaElement.ElementSchemaType as XmlSchemaComplexType;
		}
		return schemaObject as XmlSchemaComplexType;
	}

	private void ValidateSingleElement(XmlElement elementNode, bool skipToEnd, XmlSchemaInfo newSchemaInfo)
	{
		nsManager.PushScope();
		XmlAttributeCollection attributes = elementNode.Attributes;
		XmlAttribute xmlAttribute = null;
		string xsiNil = null;
		string xsiType = null;
		for (int i = 0; i < attributes.Count; i++)
		{
			xmlAttribute = attributes[i];
			string namespaceURI = xmlAttribute.NamespaceURI;
			string localName = xmlAttribute.LocalName;
			if (Ref.Equal(namespaceURI, NsXsi))
			{
				if (Ref.Equal(localName, XsiType))
				{
					xsiType = xmlAttribute.Value;
				}
				else if (Ref.Equal(localName, XsiNil))
				{
					xsiNil = xmlAttribute.Value;
				}
			}
			else if (Ref.Equal(namespaceURI, NsXmlNs))
			{
				nsManager.AddNamespace((xmlAttribute.Prefix.Length == 0) ? string.Empty : xmlAttribute.LocalName, xmlAttribute.Value);
			}
		}
		validator.ValidateElement(elementNode.LocalName, elementNode.NamespaceURI, newSchemaInfo, xsiType, xsiNil, null, null);
		if (skipToEnd)
		{
			validator.ValidateEndOfAttributes(newSchemaInfo);
			validator.SkipToEndElement(newSchemaInfo);
			nsManager.PopScope();
		}
	}

	private void ValidateChildrenTillNextAncestor(XmlNode parentNode, XmlNode childToStopAt)
	{
		XmlNode xmlNode = parentNode.FirstChild;
		while (xmlNode != null && xmlNode != childToStopAt)
		{
			switch (xmlNode.NodeType)
			{
			case XmlNodeType.EntityReference:
				ValidateChildrenTillNextAncestor(xmlNode, childToStopAt);
				break;
			case XmlNodeType.Element:
				ValidateSingleElement(xmlNode as XmlElement, skipToEnd: true, null);
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				validator.ValidateText(xmlNode.Value);
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				validator.ValidateWhitespace(xmlNode.Value);
				break;
			default:
			{
				object[] args = new string[1] { currentNode.NodeType.ToString() };
				throw new InvalidOperationException(Res.GetString("Unexpected XmlNodeType: '{0}'.", args));
			}
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
				break;
			}
			xmlNode = xmlNode.NextSibling;
		}
	}

	private XmlSchemaValidator CreateTypeFinderValidator(XmlSchemaObject partialValidationType)
	{
		XmlSchemaValidator xmlSchemaValidator = new XmlSchemaValidator(document.NameTable, document.Schemas, nsManager, XmlSchemaValidationFlags.None);
		xmlSchemaValidator.ValidationEventHandler += TypeFinderCallBack;
		if (partialValidationType != null)
		{
			xmlSchemaValidator.Initialize(partialValidationType);
		}
		else
		{
			xmlSchemaValidator.Initialize();
		}
		return xmlSchemaValidator;
	}

	private void TypeFinderCallBack(object sender, ValidationEventArgs arg)
	{
		if (arg.Severity == XmlSeverityType.Error)
		{
			isPartialTreeValid = false;
		}
	}

	private void InternalValidationCallBack(object sender, ValidationEventArgs arg)
	{
		if (arg.Severity == XmlSeverityType.Error)
		{
			isValid = false;
		}
		XmlSchemaValidationException ex = arg.Exception as XmlSchemaValidationException;
		ex.SetSourceObject(currentNode);
		if (eventHandler != null)
		{
			eventHandler(sender, arg);
		}
		else if (arg.Severity == XmlSeverityType.Error)
		{
			throw ex;
		}
	}
}
