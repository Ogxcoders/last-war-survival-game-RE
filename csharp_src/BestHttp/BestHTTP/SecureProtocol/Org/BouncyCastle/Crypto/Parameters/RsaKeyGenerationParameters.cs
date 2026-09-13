using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

public class RsaKeyGenerationParameters : KeyGenerationParameters
{
	private readonly BigInteger publicExponent;

	private readonly int certainty;

	public BigInteger PublicExponent => publicExponent;

	public int Certainty => certainty;

	public RsaKeyGenerationParameters(BigInteger publicExponent, SecureRandom random, int strength, int certainty)
		: base(random, strength)
	{
		this.publicExponent = publicExponent;
		this.certainty = certainty;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is RsaKeyGenerationParameters rsaKeyGenerationParameters))
		{
			return false;
		}
		if (certainty == rsaKeyGenerationParameters.certainty)
		{
			return publicExponent.Equals(rsaKeyGenerationParameters.publicExponent);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = certainty;
		return num.GetHashCode() ^ publicExponent.GetHashCode();
	}
}
