using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public class TlsECDsaSigner : TlsDsaSigner
{
	protected override byte SignatureAlgorithm => 3;

	public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
	{
		return publicKey is ECPublicKeyParameters;
	}

	protected override IDsa CreateDsaImpl(byte hashAlgorithm)
	{
		return new ECDsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
	}
}
