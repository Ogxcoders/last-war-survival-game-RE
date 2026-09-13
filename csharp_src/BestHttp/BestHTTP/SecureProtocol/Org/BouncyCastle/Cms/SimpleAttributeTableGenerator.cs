using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class SimpleAttributeTableGenerator : CmsAttributeTableGenerator
{
	private readonly AttributeTable attributes;

	public SimpleAttributeTableGenerator(AttributeTable attributes)
	{
		this.attributes = attributes;
	}

	public virtual AttributeTable GetAttributes(IDictionary parameters)
	{
		return attributes;
	}
}
