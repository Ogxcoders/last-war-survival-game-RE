using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public interface IBasicAgreement
{
	void Init(ICipherParameters parameters);

	int GetFieldSize();

	BigInteger CalculateAgreement(ICipherParameters pubKey);
}
