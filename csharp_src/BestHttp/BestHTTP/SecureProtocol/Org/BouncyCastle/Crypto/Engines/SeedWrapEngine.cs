namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;

public class SeedWrapEngine : Rfc3394WrapEngine
{
	public SeedWrapEngine()
		: base(new SeedEngine())
	{
	}
}
