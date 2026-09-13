using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

public interface IX509Selector : ICloneable
{
	bool Match(object obj);
}
