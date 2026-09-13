using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

public class Ed448KeyGenerationParameters : KeyGenerationParameters
{
	public Ed448KeyGenerationParameters(SecureRandom random)
		: base(random, 448)
	{
	}
}
