using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public interface TlsCompression
{
	Stream Compress(Stream output);

	Stream Decompress(Stream output);
}
