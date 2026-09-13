namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public abstract class ServerOnlyTlsAuthentication : TlsAuthentication
{
	public abstract void NotifyServerCertificate(Certificate serverCertificate);

	public TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest)
	{
		return null;
	}
}
