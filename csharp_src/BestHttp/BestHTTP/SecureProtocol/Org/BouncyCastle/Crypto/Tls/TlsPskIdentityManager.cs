namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public interface TlsPskIdentityManager
{
	byte[] GetHint();

	byte[] GetPsk(byte[] identity);
}
