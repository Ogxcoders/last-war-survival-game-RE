namespace System.Web.Services.Discovery;

public class DiscoveryDocumentLinksPattern : DiscoverySearchPattern
{
	private string pattern = "*.disco";

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
