using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

public class X25519KeyGenerationParameters : KeyGenerationParameters
{
	public X25519KeyGenerationParameters(SecureRandom random)
		: base(random, 255)
	{
	}
}
