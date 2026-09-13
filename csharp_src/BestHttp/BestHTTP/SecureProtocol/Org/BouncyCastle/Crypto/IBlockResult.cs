namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public interface IBlockResult
{
	byte[] Collect();

	int Collect(byte[] destination, int offset);
}
