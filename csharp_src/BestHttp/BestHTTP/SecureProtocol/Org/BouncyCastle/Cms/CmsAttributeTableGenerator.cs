using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public interface CmsAttributeTableGenerator
{
	AttributeTable GetAttributes(IDictionary parameters);
}
