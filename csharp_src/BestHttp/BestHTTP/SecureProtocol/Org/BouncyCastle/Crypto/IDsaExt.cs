using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public interface IDsaExt : IDsa
{
	BigInteger Order { get; }
}
