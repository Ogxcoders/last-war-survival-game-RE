using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

public interface IEncoder
{
	int Encode(byte[] data, int off, int length, Stream outStream);

	int Decode(byte[] data, int off, int length, Stream outStream);

	int DecodeString(string data, Stream outStream);
}
