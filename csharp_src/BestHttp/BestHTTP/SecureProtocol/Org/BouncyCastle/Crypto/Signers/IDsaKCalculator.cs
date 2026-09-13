using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

public interface IDsaKCalculator
{
	bool IsDeterministic { get; }

	void Init(BigInteger n, SecureRandom random);

	void Init(BigInteger n, BigInteger d, byte[] message);

	BigInteger NextK();
}
