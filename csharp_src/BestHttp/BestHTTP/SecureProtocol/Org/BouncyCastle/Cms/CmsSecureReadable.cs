using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

internal interface CmsSecureReadable
{
	AlgorithmIdentifier Algorithm { get; }

	object CryptoObject { get; }

	CmsReadable GetReadable(KeyParameter key);
}
