using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class CmsAuthenticatedGenerator : CmsEnvelopedGenerator
{
	public CmsAuthenticatedGenerator()
	{
	}

	public CmsAuthenticatedGenerator(SecureRandom rand)
		: base(rand)
	{
	}
}
