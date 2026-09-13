namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public interface TlsEncryptionCredentials : TlsCredentials
{
	byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
}
