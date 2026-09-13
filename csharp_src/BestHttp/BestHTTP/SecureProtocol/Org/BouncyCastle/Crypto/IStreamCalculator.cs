using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public interface IStreamCalculator
{
	Stream Stream { get; }

	object GetResult();
}
