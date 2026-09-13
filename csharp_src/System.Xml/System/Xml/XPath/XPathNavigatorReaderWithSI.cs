using System.Xml.Schema;

namespace System.Xml.XPath;

internal class XPathNavigatorReaderWithSI : XPathNavigatorReader, IXmlSchemaInfo
{
	public virtual XmlSchemaValidity Validity
	{
		get
		{
			if (!base.IsReading)
			{
				return XmlSchemaValidity.NotKnown;
			}
			return schemaInfo.Validity;
		}
	}

	public override bool IsDefault
	{
		get
		{
			if (!base.IsReading)
			{
				return false;
			}
			return schemaInfo.IsDefault;
		}
	}

	public virtual bool IsNil
	{
		get
		{
			if (!base.IsReading)
			{
				return false;
			}
			return schemaInfo.IsNil;
		}
	}

	public virtual XmlSchemaSimpleType MemberType
	{
		get
		{
			if (!base.IsReading)
			{
				return null;
			}
			return schemaInfo.MemberType;
		}
	}

	public virtual XmlSchemaType SchemaType
	{
		get
		{
			if (!base.IsReading)
			{
				return null;
			}
			return schemaInfo.SchemaType;
		}
	}

	public virtual XmlSchemaElement SchemaElement
	{
		get
		{
			if (!base.IsReading)
			{
				return null;
			}
			return schemaInfo.SchemaElement;
		}
	}

	public virtual XmlSchemaAttribute SchemaAttribute
	{
		get
		{
			if (!base.IsReading)
			{
				return null;
			}
			return schemaInfo.SchemaAttribute;
		}
	}

	internal XPathNavigatorReaderWithSI(XPathNavigator navToRead, IXmlLineInfo xli, IXmlSchemaInfo xsi)
		: base(navToRead, xli, xsi)
	{
	}
}
