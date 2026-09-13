using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

public interface IX509Store
{
	ICollection GetMatches(IX509Selector selector);
}
