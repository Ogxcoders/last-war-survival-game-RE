namespace System.Web.Services.Discovery;

public sealed class DiscoveryDocumentSearchPattern : DiscoverySearchPattern
{
	private string pattern = "*.vsdisco";

	public override string Pattern => pattern;

	public override DiscoveryReference GetDiscoveryReference(string filename)
	{
		return new DiscoveryDocumentReference
		{
			Url = filename,
			Ref = filename
		};
	}
}
