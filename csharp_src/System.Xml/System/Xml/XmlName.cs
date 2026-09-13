using System.Xml.Schema;

namespace System.Xml;

internal class XmlName : IXmlSchemaInfo
{
	private string prefix;

	private string localName;

	private string ns;

	private string name;

	private int hashCode;

	internal XmlDocument ownerDoc;

	internal XmlName next;

	public string LocalName => localName;

	public string NamespaceURI => ns;

	public string Prefix => prefix;

	public int HashCode => hashCode;

	public XmlDocument OwnerDocument => ownerDoc;

	public string Name
	{
		get
		{
			if (name == null)
			{
				if (prefix.Length > 0)
				{
					if (localName.Length > 0)
					{
						string array = prefix + ":" + localName;
						lock (ownerDoc.NameTable)
						{
							if (name == null)
							{
								name = ownerDoc.NameTable.Add(array);
							}
						}
					}
					else
					{
						name = prefix;
					}
				}
				else
				{
					name = localName;
				}
			}
			return name;
		}
	}

	public virtual XmlSchemaValidity Validity => XmlSchemaValidity.NotKnown;

	public virtual bool IsDefault => false;

	public virtual bool IsNil => false;

	public virtual XmlSchemaSimpleType MemberType => null;

	public virtual XmlSchemaType SchemaType => null;

	public virtual XmlSchemaElement SchemaElement => null;

	public virtual XmlSchemaAttribute SchemaAttribute => null;

	public static XmlName Create(string prefix, string localName, string ns, int hashCode, XmlDocument ownerDoc, XmlName next, IXmlSchemaInfo schemaInfo)
	{
		if (schemaInfo == null)
		{
			return new XmlName(prefix, localName, ns, hashCode, ownerDoc, next);
		}
		return new XmlNameEx(prefix, localName, ns, hashCode, ownerDoc, next, schemaInfo);
	}

	internal XmlName(string prefix, string localName, string ns, int hashCode, XmlDocument ownerDoc, XmlName next)
	{
		this.prefix = prefix;
		this.localName = localName;
		this.ns = ns;
		name = null;
		this.hashCode = hashCode;
		this.ownerDoc = ownerDoc;
		this.next = next;
	}

	public virtual bool Equals(IXmlSchemaInfo schemaInfo)
	{
		return schemaInfo == null;
	}

	public static int GetHashCode(string name)
	{
		int num = 0;
		if (name != null)
		{
			for (int num2 = name.Length - 1; num2 >= 0; num2--)
			{
				char c = name[num2];
				if (c == ':')
				{
					break;
				}
				num += (num << 7) ^ c;
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
		}
		return num;
	}
}
