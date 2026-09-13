using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls;

public interface ICertificateVerifyer
{
	bool IsValid(Uri targetUri, X509CertificateStructure[] certs);
}
