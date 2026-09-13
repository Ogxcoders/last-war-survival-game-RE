using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public interface TlsSrpGroupVerifier
{
	bool Accept(Srp6GroupParameters group);
}
