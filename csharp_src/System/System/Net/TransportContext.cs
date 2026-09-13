using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net;

public abstract class TransportContext
{
	public abstract ChannelBinding GetChannelBinding(ChannelBindingKind kind);

	public virtual IEnumerable<TokenBinding> GetTlsTokenBindings()
	{
		throw new NotSupportedException();
	}
}
