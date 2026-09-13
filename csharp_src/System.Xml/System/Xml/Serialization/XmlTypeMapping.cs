using System.Collections;

namespace System.Xml.Serialization;

public class XmlTypeMapping : XmlMapping
{
	private string xmlType;

	private string xmlTypeNamespace;

	private TypeData type;

	private XmlTypeMapping baseMap;

	private bool multiReferenceType;

	private bool isSimpleType;

	private string documentation;

	private bool includeInSchema;

	private bool isNullable = true;

	private bool isAny;

	private ArrayList _derivedTypes = new ArrayList();

	public string TypeFullName => type.FullTypeName;

	public string TypeName => type.TypeName;

	public string XsdTypeName => XmlType;

	public string XsdTypeNamespace => XmlTypeNamespace;

	internal TypeData TypeData => type;

	internal string XmlType
	{
		get
		{
			return xmlType;
		}
		set
		{
			xmlType = value;
		}
	}

	internal string XmlTypeNamespace
	{
		get
		{
			return xmlTypeNamespace ?? string.Empty;
		}
		set
		{
			xmlTypeNamespace = value;
		}
	}

	internal bool HasXmlTypeNamespace => xmlTypeNamespace != null;

	internal ArrayList DerivedTypes
	{
		get
		{
			return _derivedTypes;
		}
		set
		{
			_derivedTypes = value;
		}
	}

	internal bool MultiReferenceType
	{
		get
		{
			return multiReferenceType;
		}
		set
		{
			multiReferenceType = value;
		}
	}

	internal XmlTypeMapping BaseMap
	{
		get
		{
			return baseMap;
		}
		set
		{
			baseMap = value;
		}
	}

	internal bool IsSimpleType
	{
		get
		{
			return isSimpleType;
		}
		set
		{
			isSimpleType = value;
		}
	}

	internal string Documentation
	{
		get
		{
			return documentation;
		}
		set
		{
			documentation = value;
		}
	}

	internal bool IncludeInSchema
	{
		get
		{
			return includeInSchema;
		}
		set
		{
			includeInSchema = value;
		}
	}

	internal bool IsNullable
	{
		get
		{
			return isNullable;
		}
		set
		{
			isNullable = value;
		}
	}

	internal bool IsAny
	{
		get
		{
			return isAny;
		}
		set
		{
			isAny = value;
		}
	}

	internal XmlTypeMapping(string elementName, string ns, TypeData typeData, string xmlType, string xmlTypeNamespace)
		: base(elementName, ns)
	{
		type = typeData;
		this.xmlType = xmlType;
		this.xmlTypeNamespace = xmlTypeNamespace;
	}

	internal XmlTypeMapping GetRealTypeMap(Type objectType)
	{
		if (TypeData.SchemaType == SchemaTypes.Enum)
		{
			return this;
		}
		if (TypeData.Type == objectType)
		{
			return this;
		}
		for (int i = 0; i < _derivedTypes.Count; i++)
		{
			XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)_derivedTypes[i];
			if (xmlTypeMapping.TypeData.Type == objectType)
			{
				return xmlTypeMapping;
			}
		}
		return null;
	}

	internal XmlTypeMapping GetRealElementMap(string name, string ens)
	{
		if (xmlType == name && XmlTypeNamespace == ens)
		{
			return this;
		}
		foreach (XmlTypeMapping derivedType in _derivedTypes)
		{
			if (derivedType.xmlType == name && derivedType.XmlTypeNamespace == ens)
			{
				return derivedType;
			}
		}
		return null;
	}

	internal void UpdateRoot(XmlQualifiedName qname)
	{
		if (qname != null)
		{
			_elementName = qname.Name;
			_namespace = qname.Namespace;
		}
	}
}
