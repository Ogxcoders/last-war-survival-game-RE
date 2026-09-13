namespace System.Xml.Schema;

public class XmlSchemaInfo : IXmlSchemaInfo
{
	private bool isDefault;

	private bool isNil;

	private XmlSchemaElement schemaElement;

	private XmlSchemaAttribute schemaAttribute;

	private XmlSchemaType schemaType;

	private XmlSchemaSimpleType memberType;

	private XmlSchemaValidity validity;

	private XmlSchemaContentType contentType;

	public XmlSchemaValidity Validity
	{
		get
		{
			return validity;
		}
		set
		{
			validity = value;
		}
	}

	public bool IsDefault
	{
		get
		{
			return isDefault;
		}
		set
		{
			isDefault = value;
		}
	}

	public bool IsNil
	{
		get
		{
			return isNil;
		}
		set
		{
			isNil = value;
		}
	}

	public XmlSchemaSimpleType MemberType
	{
		get
		{
			return memberType;
		}
		set
		{
			memberType = value;
		}
	}

	public XmlSchemaType SchemaType
	{
		get
		{
			return schemaType;
		}
		set
		{
			schemaType = value;
			if (schemaType != null)
			{
				contentType = schemaType.SchemaContentType;
			}
			else
			{
				contentType = XmlSchemaContentType.Empty;
			}
		}
	}

	public XmlSchemaElement SchemaElement
	{
		get
		{
			return schemaElement;
		}
		set
		{
			schemaElement = value;
			if (value != null)
			{
				schemaAttribute = null;
			}
		}
	}

	public XmlSchemaAttribute SchemaAttribute
	{
		get
		{
			return schemaAttribute;
		}
		set
		{
			schemaAttribute = value;
			if (value != null)
			{
				schemaElement = null;
			}
		}
	}

	public XmlSchemaContentType ContentType
	{
		get
		{
			return contentType;
		}
		set
		{
			contentType = value;
		}
	}

	internal XmlSchemaType XmlType
	{
		get
		{
			if (memberType != null)
			{
				return memberType;
			}
			return schemaType;
		}
	}

	internal bool HasDefaultValue
	{
		get
		{
			if (schemaElement != null)
			{
				return schemaElement.ElementDecl.DefaultValueTyped != null;
			}
			return false;
		}
	}

	internal bool IsUnionType
	{
		get
		{
			if (schemaType == null || schemaType.Datatype == null)
			{
				return false;
			}
			return schemaType.Datatype.Variety == XmlSchemaDatatypeVariety.Union;
		}
	}

	public XmlSchemaInfo()
	{
		Clear();
	}

	internal XmlSchemaInfo(XmlSchemaValidity validity)
		: this()
	{
		this.validity = validity;
	}

	internal void Clear()
	{
		isNil = false;
		isDefault = false;
		schemaType = null;
		schemaElement = null;
		schemaAttribute = null;
		memberType = null;
		validity = XmlSchemaValidity.NotKnown;
		contentType = XmlSchemaContentType.Empty;
	}
}
