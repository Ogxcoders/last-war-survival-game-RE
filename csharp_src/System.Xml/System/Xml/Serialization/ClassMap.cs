using System.Collections;

namespace System.Xml.Serialization;

internal class ClassMap : ObjectMap
{
	private Hashtable _elements = new Hashtable();

	private ArrayList _elementMembers;

	private Hashtable _attributeMembers;

	private XmlTypeMapMemberAttribute[] _attributeMembersArray;

	private XmlTypeMapElementInfo[] _elementsByIndex;

	private ArrayList _flatLists;

	private ArrayList _allMembers = new ArrayList();

	private ArrayList _membersWithDefault;

	private ArrayList _listMembers;

	private XmlTypeMapMemberAnyElement _defaultAnyElement;

	private XmlTypeMapMemberAnyAttribute _defaultAnyAttribute;

	private XmlTypeMapMemberNamespaces _namespaceDeclarations;

	private XmlTypeMapMember _xmlTextCollector;

	private XmlTypeMapMember _returnMember;

	private bool _ignoreMemberNamespace;

	private bool _canBeSimpleType = true;

	private bool? _isOrderDependentMap;

	public ICollection AllElementInfos => _elements.Values;

	public bool IgnoreMemberNamespace
	{
		get
		{
			return _ignoreMemberNamespace;
		}
		set
		{
			_ignoreMemberNamespace = value;
		}
	}

	public bool IsOrderDependentMap
	{
		get
		{
			if (!_isOrderDependentMap.HasValue)
			{
				_isOrderDependentMap = false;
				foreach (XmlTypeMapElementInfo value in _elements.Values)
				{
					if (value.ExplicitOrder >= 0)
					{
						_isOrderDependentMap = true;
						break;
					}
				}
			}
			return _isOrderDependentMap.Value;
		}
	}

	public XmlTypeMapMemberAnyElement DefaultAnyElementMember => _defaultAnyElement;

	public XmlTypeMapMemberAnyAttribute DefaultAnyAttributeMember => _defaultAnyAttribute;

	public XmlTypeMapMemberNamespaces NamespaceDeclarations => _namespaceDeclarations;

	public ICollection AttributeMembers
	{
		get
		{
			if (_attributeMembers == null)
			{
				return null;
			}
			if (_attributeMembersArray != null)
			{
				return _attributeMembersArray;
			}
			_attributeMembersArray = new XmlTypeMapMemberAttribute[_attributeMembers.Count];
			foreach (XmlTypeMapMemberAttribute value in _attributeMembers.Values)
			{
				_attributeMembersArray[value.Index] = value;
			}
			return _attributeMembersArray;
		}
	}

	public ICollection ElementMembers => _elementMembers;

	public ArrayList AllMembers => _allMembers;

	public ArrayList FlatLists => _flatLists;

	public ArrayList MembersWithDefault => _membersWithDefault;

	public ArrayList ListMembers => _listMembers;

	public XmlTypeMapMember XmlTextCollector => _xmlTextCollector;

	public XmlTypeMapMember ReturnMember => _returnMember;

	public XmlQualifiedName SimpleContentBaseType
	{
		get
		{
			if (!_canBeSimpleType || _elementMembers == null || _elementMembers.Count != 1)
			{
				return null;
			}
			XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)_elementMembers[0];
			if (xmlTypeMapMemberElement.ElementInfo.Count != 1)
			{
				return null;
			}
			XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0];
			if (!xmlTypeMapElementInfo.IsTextElement)
			{
				return null;
			}
			if (xmlTypeMapMemberElement.TypeData.SchemaType == SchemaTypes.Primitive || xmlTypeMapMemberElement.TypeData.SchemaType == SchemaTypes.Enum)
			{
				return new XmlQualifiedName(xmlTypeMapElementInfo.TypeData.XmlType, xmlTypeMapElementInfo.DataTypeNamespace);
			}
			return null;
		}
	}

	public bool HasSimpleContent => SimpleContentBaseType != null;

	public void AddMember(XmlTypeMapMember member)
	{
		if (member.GlobalIndex == -1)
		{
			member.GlobalIndex = _allMembers.Count;
		}
		_allMembers.Add(member);
		if (!(member.DefaultValue is DBNull) && member.DefaultValue != null)
		{
			if (_membersWithDefault == null)
			{
				_membersWithDefault = new ArrayList();
			}
			_membersWithDefault.Add(member);
		}
		if (member.IsReturnValue)
		{
			_returnMember = member;
		}
		if (member is XmlTypeMapMemberAttribute)
		{
			XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)member;
			if (_attributeMembers == null)
			{
				_attributeMembers = new Hashtable();
			}
			string key = BuildKey(xmlTypeMapMemberAttribute.AttributeName, xmlTypeMapMemberAttribute.Namespace, -1);
			if (_attributeMembers.ContainsKey(key))
			{
				throw new InvalidOperationException("The XML attribute named '" + xmlTypeMapMemberAttribute.AttributeName + "' from namespace '" + xmlTypeMapMemberAttribute.Namespace + "' is already present in the current scope. Use XML attributes to specify another XML name or namespace for the attribute.");
			}
			member.Index = _attributeMembers.Count;
			_attributeMembers.Add(key, member);
			return;
		}
		if (member is XmlTypeMapMemberFlatList)
		{
			RegisterFlatList((XmlTypeMapMemberFlatList)member);
		}
		else if (member is XmlTypeMapMemberAnyElement)
		{
			XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = (XmlTypeMapMemberAnyElement)member;
			if (xmlTypeMapMemberAnyElement.IsDefaultAny)
			{
				_defaultAnyElement = xmlTypeMapMemberAnyElement;
			}
			if (xmlTypeMapMemberAnyElement.TypeData.IsListType)
			{
				RegisterFlatList(xmlTypeMapMemberAnyElement);
			}
		}
		else
		{
			if (member is XmlTypeMapMemberAnyAttribute)
			{
				_defaultAnyAttribute = (XmlTypeMapMemberAnyAttribute)member;
				return;
			}
			if (member is XmlTypeMapMemberNamespaces)
			{
				_namespaceDeclarations = (XmlTypeMapMemberNamespaces)member;
				return;
			}
		}
		if (member is XmlTypeMapMemberElement && ((XmlTypeMapMemberElement)member).IsXmlTextCollector)
		{
			if (_xmlTextCollector != null)
			{
				throw new InvalidOperationException("XmlTextAttribute can only be applied once in a class");
			}
			_xmlTextCollector = member;
		}
		if (_elementMembers == null)
		{
			_elementMembers = new ArrayList();
			_elements = new Hashtable();
		}
		member.Index = _elementMembers.Count;
		_elementMembers.Add(member);
		foreach (XmlTypeMapElementInfo item in (IEnumerable)((XmlTypeMapMemberElement)member).ElementInfo)
		{
			string key2 = BuildKey(item.ElementName, item.Namespace, item.ExplicitOrder);
			if (_elements.ContainsKey(key2))
			{
				throw new InvalidOperationException("The XML element named '" + item.ElementName + "' from namespace '" + item.Namespace + "' is already present in the current scope. Use XML attributes to specify another XML name or namespace for the element.");
			}
			_elements.Add(key2, item);
		}
		if (member.TypeData.IsListType && member.TypeData.Type != null && !member.TypeData.Type.IsArray)
		{
			if (_listMembers == null)
			{
				_listMembers = new ArrayList();
			}
			_listMembers.Add(member);
		}
	}

	private void RegisterFlatList(XmlTypeMapMemberExpandable member)
	{
		if (_flatLists == null)
		{
			_flatLists = new ArrayList();
		}
		member.FlatArrayIndex = _flatLists.Count;
		_flatLists.Add(member);
	}

	public XmlTypeMapMemberAttribute GetAttribute(string name, string ns)
	{
		if (_attributeMembers == null)
		{
			return null;
		}
		return (XmlTypeMapMemberAttribute)_attributeMembers[BuildKey(name, ns, -1)];
	}

	public XmlTypeMapElementInfo GetElement(string name, string ns, int minimalOrder)
	{
		if (_elements == null)
		{
			return null;
		}
		XmlTypeMapElementInfo xmlTypeMapElementInfo = null;
		foreach (XmlTypeMapElementInfo value in _elements.Values)
		{
			if (value.ElementName == name && value.Namespace == ns && value.ExplicitOrder >= minimalOrder && (xmlTypeMapElementInfo == null || xmlTypeMapElementInfo.ExplicitOrder > value.ExplicitOrder))
			{
				xmlTypeMapElementInfo = value;
			}
		}
		return xmlTypeMapElementInfo;
	}

	public XmlTypeMapElementInfo GetElement(string name, string ns)
	{
		if (_elements == null)
		{
			return null;
		}
		foreach (XmlTypeMapElementInfo value in _elements.Values)
		{
			if (value.ElementName == name && value.Namespace == ns)
			{
				return value;
			}
		}
		return null;
	}

	public XmlTypeMapElementInfo GetElement(int index)
	{
		if (_elements == null)
		{
			return null;
		}
		if (_elementsByIndex == null)
		{
			_elementsByIndex = new XmlTypeMapElementInfo[_elementMembers.Count];
			foreach (XmlTypeMapMemberElement elementMember in _elementMembers)
			{
				if (elementMember.ElementInfo.Count != 1)
				{
					throw new InvalidOperationException("Read by order only possible for encoded/bare format");
				}
				_elementsByIndex[elementMember.Index] = (XmlTypeMapElementInfo)elementMember.ElementInfo[0];
			}
		}
		if (index >= _elementMembers.Count)
		{
			return null;
		}
		return _elementsByIndex[index];
	}

	private string BuildKey(string name, string ns, int explicitOrder)
	{
		if (_ignoreMemberNamespace)
		{
			return name;
		}
		return name + " / " + ns + ((explicitOrder < 0) ? "" : ("/" + explicitOrder));
	}

	public XmlTypeMapMember FindMember(string name)
	{
		for (int i = 0; i < _allMembers.Count; i++)
		{
			if (((XmlTypeMapMember)_allMembers[i]).Name == name)
			{
				return (XmlTypeMapMember)_allMembers[i];
			}
		}
		return null;
	}

	public void SetCanBeSimpleType(bool can)
	{
		_canBeSimpleType = can;
	}
}
