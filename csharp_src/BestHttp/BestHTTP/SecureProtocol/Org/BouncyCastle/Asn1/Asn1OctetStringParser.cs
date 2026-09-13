using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;

public interface Asn1OctetStringParser : IAsn1Convertible
{
	Stream GetOctetStream();
}
