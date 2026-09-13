using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal class ExtensionDataReader : XmlReader
{
	private enum ExtensionDataNodeType
	{
		None,
		Element,
		EndElement,
		Text,
		Xml,
		ReferencedElement,
		NullElement
	}

	private Hashtable cache = new Hashtable();

	private ElementData[] elements;

	private ElementData element;

	private ElementData nextElement;

	private ReadState readState;

	private ExtensionDataNodeType internalNodeType;

	private XmlNodeType nodeType;

	private int depth;

	private string localName;

	private string ns;

	private string prefix;

	private string value;

	private int attributeCount;

	private int attributeIndex;

	private XmlNodeReader xmlNodeReader;

	private Queue<IDataNode> deserializedDataNodes;

	private XmlObjectSerializerReadContext context;

	[SecurityCritical]
	private static Dictionary<string, string> nsToPrefixTable;

	[SecurityCritical]
	private static Dictionary<string, string> prefixToNsTable;

	private bool IsXmlDataNode => internalNodeType == ExtensionDataNodeType.Xml;

	public override XmlNodeType NodeType
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return nodeType;
			}
			return xmlNodeReader.NodeType;
		}
	}

	public override string LocalName
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return localName;
			}
			return xmlNodeReader.LocalName;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return ns;
			}
			return xmlNodeReader.NamespaceURI;
		}
	}

	public override string Prefix
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return prefix;
			}
			return xmlNodeReader.Prefix;
		}
	}

	public override string Value
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return value;
			}
			return xmlNodeReader.Value;
		}
	}

	public override int Depth
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return depth;
			}
			return xmlNodeReader.Depth;
		}
	}

	public override int AttributeCount
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return attributeCount;
			}
			return xmlNodeReader.AttributeCount;
		}
	}

	public override bool EOF
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return readState == ReadState.EndOfFile;
			}
			return xmlNodeReader.EOF;
		}
	}

	public override ReadState ReadState
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return readState;
			}
			return xmlNodeReader.ReadState;
		}
	}

	public override bool IsEmptyElement
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return false;
			}
			return xmlNodeReader.IsEmptyElement;
		}
	}

	public override bool IsDefault
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return base.IsDefault;
			}
			return xmlNodeReader.IsDefault;
		}
	}

	public override char QuoteChar
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return base.QuoteChar;
			}
			return xmlNodeReader.QuoteChar;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return base.XmlSpace;
			}
			return xmlNodeReader.XmlSpace;
		}
	}

	public override string XmlLang
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return base.XmlLang;
			}
			return xmlNodeReader.XmlLang;
		}
	}

	public override string this[int i]
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return GetAttribute(i);
			}
			return xmlNodeReader[i];
		}
	}

	public override string this[string name]
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return GetAttribute(name);
			}
			return xmlNodeReader[name];
		}
	}

	public override string this[string name, string namespaceURI]
	{
		get
		{
			if (!IsXmlDataNode)
			{
				return GetAttribute(name, namespaceURI);
			}
			return xmlNodeReader[name, namespaceURI];
		}
	}

	public override string Name
	{
		get
		{
			if (IsXmlDataNode)
			{
				return xmlNodeReader.Name;
			}
			return string.Empty;
		}
	}

	public override bool HasValue
	{
		get
		{
			if (IsXmlDataNode)
			{
				return xmlNodeReader.HasValue;
			}
			return false;
		}
	}

	public override string BaseURI
	{
		get
		{
			if (IsXmlDataNode)
			{
				return xmlNodeReader.BaseURI;
			}
			return string.Empty;
		}
	}

	public override XmlNameTable NameTable
	{
		get
		{
			if (IsXmlDataNode)
			{
				return xmlNodeReader.NameTable;
			}
			return null;
		}
	}

	[SecuritySafeCritical]
	static ExtensionDataReader()
	{
		nsToPrefixTable = new Dictionary<string, string>();
		prefixToNsTable = new Dictionary<string, string>();
		AddPrefix("i", "http://www.w3.org/2001/XMLSchema-instance");
		AddPrefix("z", "http://schemas.microsoft.com/2003/10/Serialization/");
		AddPrefix(string.Empty, string.Empty);
	}

	internal ExtensionDataReader(XmlObjectSerializerReadContext context)
	{
		attributeIndex = -1;
		this.context = context;
	}

	internal void SetDeserializedValue(object obj)
	{
		IDataNode dataNode = ((deserializedDataNodes == null || deserializedDataNodes.Count == 0) ? null : deserializedDataNodes.Dequeue());
		if (dataNode != null && !(obj is IDataNode))
		{
			dataNode.Value = obj;
			dataNode.IsFinalValue = true;
		}
	}

	internal IDataNode GetCurrentNode()
	{
		IDataNode dataNode = element.dataNode;
		Skip();
		return dataNode;
	}

	internal void SetDataNode(IDataNode dataNode, string name, string ns)
	{
		SetNextElement(dataNode, name, ns, null);
		element = nextElement;
		nextElement = null;
		SetElement();
	}

	internal void Reset()
	{
		localName = null;
		ns = null;
		prefix = null;
		value = null;
		attributeCount = 0;
		attributeIndex = -1;
		depth = 0;
		element = null;
		nextElement = null;
		elements = null;
		deserializedDataNodes = null;
	}

	public override bool MoveToFirstAttribute()
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.MoveToFirstAttribute();
		}
		if (attributeCount == 0)
		{
			return false;
		}
		MoveToAttribute(0);
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.MoveToNextAttribute();
		}
		if (attributeIndex + 1 >= attributeCount)
		{
			return false;
		}
		MoveToAttribute(attributeIndex + 1);
		return true;
	}

	public override void MoveToAttribute(int index)
	{
		if (IsXmlDataNode)
		{
			xmlNodeReader.MoveToAttribute(index);
			return;
		}
		if (index < 0 || index >= attributeCount)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid XML while deserializing extension data.")));
		}
		nodeType = XmlNodeType.Attribute;
		AttributeData attributeData = element.attributes[index];
		localName = attributeData.localName;
		ns = attributeData.ns;
		prefix = attributeData.prefix;
		value = attributeData.value;
		attributeIndex = index;
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.GetAttribute(name, namespaceURI);
		}
		for (int i = 0; i < element.attributeCount; i++)
		{
			AttributeData attributeData = element.attributes[i];
			if (attributeData.localName == name && attributeData.ns == namespaceURI)
			{
				return attributeData.value;
			}
		}
		return null;
	}

	public override bool MoveToAttribute(string name, string namespaceURI)
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.MoveToAttribute(name, ns);
		}
		for (int i = 0; i < element.attributeCount; i++)
		{
			AttributeData attributeData = element.attributes[i];
			if (attributeData.localName == name && attributeData.ns == namespaceURI)
			{
				MoveToAttribute(i);
				return true;
			}
		}
		return false;
	}

	public override bool MoveToElement()
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.MoveToElement();
		}
		if (nodeType != XmlNodeType.Attribute)
		{
			return false;
		}
		SetElement();
		return true;
	}

	private void SetElement()
	{
		nodeType = XmlNodeType.Element;
		localName = element.localName;
		ns = element.ns;
		prefix = element.prefix;
		value = string.Empty;
		attributeCount = element.attributeCount;
		attributeIndex = -1;
	}

	[SecuritySafeCritical]
	public override string LookupNamespace(string prefix)
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.LookupNamespace(prefix);
		}
		if (!prefixToNsTable.TryGetValue(prefix, out var result))
		{
			return null;
		}
		return result;
	}

	public override void Skip()
	{
		if (IsXmlDataNode)
		{
			xmlNodeReader.Skip();
		}
		else
		{
			if (ReadState != ReadState.Interactive)
			{
				return;
			}
			MoveToElement();
			if (IsElementNode(internalNodeType))
			{
				int num = 1;
				while (num != 0)
				{
					if (!Read())
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid XML while deserializing extension data.")));
					}
					if (IsElementNode(internalNodeType))
					{
						num++;
					}
					else if (internalNodeType == ExtensionDataNodeType.EndElement)
					{
						ReadEndElement();
						num--;
					}
				}
			}
			else
			{
				Read();
			}
		}
	}

	private bool IsElementNode(ExtensionDataNodeType nodeType)
	{
		if (nodeType != ExtensionDataNodeType.Element && nodeType != ExtensionDataNodeType.ReferencedElement)
		{
			return nodeType == ExtensionDataNodeType.NullElement;
		}
		return true;
	}

	public override void Close()
	{
		if (IsXmlDataNode)
		{
			xmlNodeReader.Close();
			return;
		}
		Reset();
		readState = ReadState.Closed;
	}

	public override bool Read()
	{
		if (nodeType == XmlNodeType.Attribute && MoveToNextAttribute())
		{
			return true;
		}
		MoveNext(element.dataNode);
		switch (internalNodeType)
		{
		case ExtensionDataNodeType.Element:
		case ExtensionDataNodeType.ReferencedElement:
		case ExtensionDataNodeType.NullElement:
			PushElement();
			SetElement();
			break;
		case ExtensionDataNodeType.Text:
			nodeType = XmlNodeType.Text;
			prefix = string.Empty;
			ns = string.Empty;
			localName = string.Empty;
			attributeCount = 0;
			attributeIndex = -1;
			break;
		case ExtensionDataNodeType.EndElement:
			nodeType = XmlNodeType.EndElement;
			prefix = string.Empty;
			ns = string.Empty;
			localName = string.Empty;
			value = string.Empty;
			attributeCount = 0;
			attributeIndex = -1;
			PopElement();
			break;
		case ExtensionDataNodeType.None:
			if (depth != 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid XML while deserializing extension data.")));
			}
			nodeType = XmlNodeType.None;
			prefix = string.Empty;
			ns = string.Empty;
			localName = string.Empty;
			value = string.Empty;
			attributeCount = 0;
			readState = ReadState.EndOfFile;
			return false;
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Invalid state in extension data reader.")));
		case ExtensionDataNodeType.Xml:
			break;
		}
		readState = ReadState.Interactive;
		return true;
	}

	public override string GetAttribute(string name)
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.GetAttribute(name);
		}
		return null;
	}

	public override string GetAttribute(int i)
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.GetAttribute(i);
		}
		return null;
	}

	public override bool MoveToAttribute(string name)
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.MoveToAttribute(name);
		}
		return false;
	}

	public override void ResolveEntity()
	{
		if (IsXmlDataNode)
		{
			xmlNodeReader.ResolveEntity();
		}
	}

	public override bool ReadAttributeValue()
	{
		if (IsXmlDataNode)
		{
			return xmlNodeReader.ReadAttributeValue();
		}
		return false;
	}

	private void MoveNext(IDataNode dataNode)
	{
		ExtensionDataNodeType extensionDataNodeType = internalNodeType;
		if (extensionDataNodeType == ExtensionDataNodeType.Text || (uint)(extensionDataNodeType - 5) <= 1u)
		{
			internalNodeType = ExtensionDataNodeType.EndElement;
			return;
		}
		Type dataType = dataNode.DataType;
		if (dataType == Globals.TypeOfClassDataNode)
		{
			MoveNextInClass((ClassDataNode)dataNode);
			return;
		}
		if (dataType == Globals.TypeOfCollectionDataNode)
		{
			MoveNextInCollection((CollectionDataNode)dataNode);
			return;
		}
		if (dataType == Globals.TypeOfISerializableDataNode)
		{
			MoveNextInISerializable((ISerializableDataNode)dataNode);
			return;
		}
		if (dataType == Globals.TypeOfXmlDataNode)
		{
			MoveNextInXml((XmlDataNode)dataNode);
			return;
		}
		if (dataNode.Value != null)
		{
			MoveToDeserializedObject(dataNode);
			return;
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Invalid state in extension data reader.")));
	}

	private void SetNextElement(IDataNode node, string name, string ns, string prefix)
	{
		internalNodeType = ExtensionDataNodeType.Element;
		nextElement = GetNextElement();
		nextElement.localName = name;
		nextElement.ns = ns;
		nextElement.prefix = prefix;
		if (node == null)
		{
			nextElement.attributeCount = 0;
			nextElement.AddAttribute("i", "http://www.w3.org/2001/XMLSchema-instance", "nil", "true");
			internalNodeType = ExtensionDataNodeType.NullElement;
		}
		else if (!CheckIfNodeHandled(node))
		{
			AddDeserializedDataNode(node);
			node.GetData(nextElement);
			if (node is XmlDataNode)
			{
				MoveNextInXml((XmlDataNode)node);
			}
		}
	}

	private void AddDeserializedDataNode(IDataNode node)
	{
		if (node.Id != Globals.NewObjectId && (node.Value == null || !node.IsFinalValue))
		{
			if (deserializedDataNodes == null)
			{
				deserializedDataNodes = new Queue<IDataNode>();
			}
			deserializedDataNodes.Enqueue(node);
		}
	}

	private bool CheckIfNodeHandled(IDataNode node)
	{
		bool flag = false;
		if (node.Id != Globals.NewObjectId)
		{
			flag = cache[node] != null;
			if (flag)
			{
				if (nextElement == null)
				{
					nextElement = GetNextElement();
				}
				nextElement.attributeCount = 0;
				nextElement.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Ref", node.Id.ToString(NumberFormatInfo.InvariantInfo));
				nextElement.AddAttribute("i", "http://www.w3.org/2001/XMLSchema-instance", "nil", "true");
				internalNodeType = ExtensionDataNodeType.ReferencedElement;
			}
			else
			{
				cache.Add(node, node);
			}
		}
		return flag;
	}

	private void MoveNextInClass(ClassDataNode dataNode)
	{
		if (dataNode.Members != null && element.childElementIndex < dataNode.Members.Count)
		{
			if (element.childElementIndex == 0)
			{
				context.IncrementItemCount(-dataNode.Members.Count);
			}
			ExtensionDataMember extensionDataMember = dataNode.Members[element.childElementIndex++];
			SetNextElement(extensionDataMember.Value, extensionDataMember.Name, extensionDataMember.Namespace, GetPrefix(extensionDataMember.Namespace));
		}
		else
		{
			internalNodeType = ExtensionDataNodeType.EndElement;
			element.childElementIndex = 0;
		}
	}

	private void MoveNextInCollection(CollectionDataNode dataNode)
	{
		if (dataNode.Items != null && element.childElementIndex < dataNode.Items.Count)
		{
			if (element.childElementIndex == 0)
			{
				context.IncrementItemCount(-dataNode.Items.Count);
			}
			IDataNode node = dataNode.Items[element.childElementIndex++];
			SetNextElement(node, dataNode.ItemName, dataNode.ItemNamespace, GetPrefix(dataNode.ItemNamespace));
		}
		else
		{
			internalNodeType = ExtensionDataNodeType.EndElement;
			element.childElementIndex = 0;
		}
	}

	private void MoveNextInISerializable(ISerializableDataNode dataNode)
	{
		if (dataNode.Members != null && element.childElementIndex < dataNode.Members.Count)
		{
			if (element.childElementIndex == 0)
			{
				context.IncrementItemCount(-dataNode.Members.Count);
			}
			ISerializableDataMember serializableDataMember = dataNode.Members[element.childElementIndex++];
			SetNextElement(serializableDataMember.Value, serializableDataMember.Name, string.Empty, string.Empty);
		}
		else
		{
			internalNodeType = ExtensionDataNodeType.EndElement;
			element.childElementIndex = 0;
		}
	}

	private void MoveNextInXml(XmlDataNode dataNode)
	{
		if (IsXmlDataNode)
		{
			xmlNodeReader.Read();
			if (xmlNodeReader.Depth == 0)
			{
				internalNodeType = ExtensionDataNodeType.EndElement;
				xmlNodeReader = null;
			}
			return;
		}
		internalNodeType = ExtensionDataNodeType.Xml;
		if (element == null)
		{
			element = nextElement;
		}
		else
		{
			PushElement();
		}
		XmlNode xmlNode = XmlObjectSerializerReadContext.CreateWrapperXmlElement(dataNode.OwnerDocument, dataNode.XmlAttributes, dataNode.XmlChildNodes, element.prefix, element.localName, element.ns);
		for (int i = 0; i < element.attributeCount; i++)
		{
			AttributeData attributeData = element.attributes[i];
			XmlAttribute xmlAttribute = dataNode.OwnerDocument.CreateAttribute(attributeData.prefix, attributeData.localName, attributeData.ns);
			xmlAttribute.Value = attributeData.value;
			xmlNode.Attributes.Append(xmlAttribute);
		}
		xmlNodeReader = new XmlNodeReader(xmlNode);
		xmlNodeReader.Read();
	}

	private void MoveToDeserializedObject(IDataNode dataNode)
	{
		Type type = dataNode.DataType;
		bool isTypedNode = true;
		if (type == Globals.TypeOfObject)
		{
			type = dataNode.Value.GetType();
			if (type == Globals.TypeOfObject)
			{
				internalNodeType = ExtensionDataNodeType.EndElement;
				return;
			}
			isTypedNode = false;
		}
		if (!MoveToText(type, dataNode, isTypedNode))
		{
			if (!dataNode.IsFinalValue)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid data node for '{0}' type.", DataContract.GetClrTypeFullName(type))));
			}
			internalNodeType = ExtensionDataNodeType.EndElement;
		}
	}

	private bool MoveToText(Type type, IDataNode dataNode, bool isTypedNode)
	{
		bool flag = true;
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.Boolean:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<bool>)dataNode).GetValue() : ((bool)dataNode.Value));
			break;
		case TypeCode.Char:
			value = XmlConvert.ToString((int)(isTypedNode ? ((DataNode<char>)dataNode).GetValue() : ((char)dataNode.Value)));
			break;
		case TypeCode.Byte:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<byte>)dataNode).GetValue() : ((byte)dataNode.Value));
			break;
		case TypeCode.Int16:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<short>)dataNode).GetValue() : ((short)dataNode.Value));
			break;
		case TypeCode.Int32:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<int>)dataNode).GetValue() : ((int)dataNode.Value));
			break;
		case TypeCode.Int64:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<long>)dataNode).GetValue() : ((long)dataNode.Value));
			break;
		case TypeCode.Single:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<float>)dataNode).GetValue() : ((float)dataNode.Value));
			break;
		case TypeCode.Double:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<double>)dataNode).GetValue() : ((double)dataNode.Value));
			break;
		case TypeCode.Decimal:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<decimal>)dataNode).GetValue() : ((decimal)dataNode.Value));
			break;
		case TypeCode.DateTime:
			value = (isTypedNode ? ((DataNode<DateTime>)dataNode).GetValue() : ((DateTime)dataNode.Value)).ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", DateTimeFormatInfo.InvariantInfo);
			break;
		case TypeCode.String:
			value = (isTypedNode ? ((DataNode<string>)dataNode).GetValue() : ((string)dataNode.Value));
			break;
		case TypeCode.SByte:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<sbyte>)dataNode).GetValue() : ((sbyte)dataNode.Value));
			break;
		case TypeCode.UInt16:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<ushort>)dataNode).GetValue() : ((ushort)dataNode.Value));
			break;
		case TypeCode.UInt32:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<uint>)dataNode).GetValue() : ((uint)dataNode.Value));
			break;
		case TypeCode.UInt64:
			value = XmlConvert.ToString(isTypedNode ? ((DataNode<ulong>)dataNode).GetValue() : ((ulong)dataNode.Value));
			break;
		default:
			if (type == Globals.TypeOfByteArray)
			{
				byte[] array = (isTypedNode ? ((DataNode<byte[]>)dataNode).GetValue() : ((byte[])dataNode.Value));
				value = ((array == null) ? string.Empty : Convert.ToBase64String(array));
			}
			else if (type == Globals.TypeOfTimeSpan)
			{
				value = XmlConvert.ToString(isTypedNode ? ((DataNode<TimeSpan>)dataNode).GetValue() : ((TimeSpan)dataNode.Value));
			}
			else if (type == Globals.TypeOfGuid)
			{
				value = (isTypedNode ? ((DataNode<Guid>)dataNode).GetValue() : ((Guid)dataNode.Value)).ToString();
			}
			else if (type == Globals.TypeOfUri)
			{
				Uri uri = (isTypedNode ? ((DataNode<Uri>)dataNode).GetValue() : ((Uri)dataNode.Value));
				value = uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
			}
			else
			{
				flag = false;
			}
			break;
		}
		if (flag)
		{
			internalNodeType = ExtensionDataNodeType.Text;
		}
		return flag;
	}

	private void PushElement()
	{
		GrowElementsIfNeeded();
		elements[depth++] = element;
		if (nextElement == null)
		{
			element = GetNextElement();
			return;
		}
		element = nextElement;
		nextElement = null;
	}

	private void PopElement()
	{
		prefix = element.prefix;
		localName = element.localName;
		ns = element.ns;
		if (depth != 0)
		{
			depth--;
			if (elements != null)
			{
				element = elements[depth];
			}
		}
	}

	private void GrowElementsIfNeeded()
	{
		if (elements == null)
		{
			elements = new ElementData[8];
		}
		else if (elements.Length == depth)
		{
			ElementData[] destinationArray = new ElementData[elements.Length * 2];
			Array.Copy(elements, 0, destinationArray, 0, elements.Length);
			elements = destinationArray;
		}
	}

	private ElementData GetNextElement()
	{
		int num = depth + 1;
		if (elements != null && elements.Length > num && elements[num] != null)
		{
			return elements[num];
		}
		return new ElementData();
	}

	[SecuritySafeCritical]
	internal static string GetPrefix(string ns)
	{
		ns = ns ?? string.Empty;
		if (!nsToPrefixTable.TryGetValue(ns, out var result))
		{
			lock (nsToPrefixTable)
			{
				if (!nsToPrefixTable.TryGetValue(ns, out result))
				{
					result = ((ns == null || ns.Length == 0) ? string.Empty : ("p" + nsToPrefixTable.Count));
					AddPrefix(result, ns);
				}
			}
		}
		return result;
	}

	[SecuritySafeCritical]
	private static void AddPrefix(string prefix, string ns)
	{
		nsToPrefixTable.Add(ns, prefix);
		prefixToNsTable.Add(prefix, ns);
	}
}
