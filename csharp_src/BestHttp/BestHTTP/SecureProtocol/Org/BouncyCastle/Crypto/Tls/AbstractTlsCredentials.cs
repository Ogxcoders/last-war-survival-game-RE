namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public abstract class AbstractTlsCredentials : TlsCredentials
{
	public abstract Certificate Certificate { get; }
}
