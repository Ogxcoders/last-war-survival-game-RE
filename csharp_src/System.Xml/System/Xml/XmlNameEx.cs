using System.Xml.Schema;

namespace System.Xml;

internal sealed class XmlNameEx : XmlName
{
	private byte flags;

	private XmlSchemaSimpleType memberType;

	private XmlSchemaType schemaType;

	private object decl;

	private const byte ValidityMask = 3;

	private const byte IsDefaultBit = 4;

	private const byte IsNilBit = 8;

	public override XmlSchemaValidity Validity
	{
		get
		{
			if (!ownerDoc.CanReportValidity)
			{
				return XmlSchemaValidity.NotKnown;
			}
			return (XmlSchemaValidity)(flags & 3);
		}
	}

	public override bool IsDefault => (flags & 4) != 0;

	public override bool IsNil => (flags & 8) != 0;

	public override XmlSchemaSimpleType MemberType => memberType;

	public override XmlSchemaType SchemaType => schemaType;

	public override XmlSchemaElement SchemaElement => decl as XmlSchemaElement;

	public override XmlSchemaAttribute SchemaAttribute => decl as XmlSchemaAttribute;

	internal XmlNameEx(string prefix, string localName, string ns, int hashCode, XmlDocument ownerDoc, XmlName next, IXmlSchemaInfo schemaInfo)
		: base(prefix, localName, ns, hashCode, ownerDoc, next)
	{
		SetValidity(schemaInfo.Validity);
		SetIsDefault(schemaInfo.IsDefault);
		SetIsNil(schemaInfo.IsNil);
		memberType = schemaInfo.MemberType;
		schemaType = schemaInfo.SchemaType;
		decl = ((schemaInfo.SchemaElement != null) ? ((XmlSchemaAnnotated)schemaInfo.SchemaElement) : ((XmlSchemaAnnotated)schemaInfo.SchemaAttribute));
	}

	public void SetValidity(XmlSchemaValidity value)
	{
		flags = (byte)((flags & -4) | (byte)value);
	}

	public void SetIsDefault(bool value)
	{
		if (value)
		{
			flags |= 4;
		}
		else
		{
			flags = (byte)(flags & -5);
		}
	}

	public void SetIsNil(bool value)
	{
		if (value)
		{
			flags |= 8;
		}
		else
		{
			flags = (byte)(flags & -9);
		}
	}

	public override bool Equals(IXmlSchemaInfo schemaInfo)
	{
		if (schemaInfo != null && schemaInfo.Validity == (XmlSchemaValidity)(flags & 3) && schemaInfo.IsDefault == ((flags & 4) != 0) && schemaInfo.IsNil == ((flags & 8) != 0) && schemaInfo.MemberType == memberType && schemaInfo.SchemaType == schemaType && schemaInfo.SchemaElement == decl as XmlSchemaElement && schemaInfo.SchemaAttribute == decl as XmlSchemaAttribute)
		{
			return true;
		}
		return false;
	}
}
