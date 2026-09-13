namespace System.Web.Services.Discovery;

public abstract class DiscoverySearchPattern
{
	public abstract string Pattern { get; }

	public abstract DiscoveryReference GetDiscoveryReference(string filename);
}
