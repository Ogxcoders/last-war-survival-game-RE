using System.Security.Authentication.ExtendedProtection;

namespace System.Net;

internal class CachedTransportContext : TransportContext
{
	private ChannelBinding binding;

	internal CachedTransportContext(ChannelBinding binding)
	{
		this.binding = binding;
	}

	public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
	{
		if (kind != ChannelBindingKind.Endpoint)
		{
			return null;
		}
		return binding;
	}
}
