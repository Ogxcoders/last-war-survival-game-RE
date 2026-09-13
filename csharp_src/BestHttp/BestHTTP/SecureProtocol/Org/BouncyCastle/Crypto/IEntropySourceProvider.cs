namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public interface IEntropySourceProvider
{
	IEntropySource Get(int bitsRequired);
}
