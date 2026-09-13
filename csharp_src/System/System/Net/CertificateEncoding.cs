namespace System.Net;

internal enum CertificateEncoding
{
	Zero = 0,
	X509AsnEncoding = 1,
	X509NdrEncoding = 2,
	Pkcs7AsnEncoding = 65536,
	Pkcs7NdrEncoding = 131072,
	AnyAsnEncoding = 65537
}
