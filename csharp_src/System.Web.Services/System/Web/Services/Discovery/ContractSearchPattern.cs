namespace System.Web.Services.Discovery;

public sealed class ContractSearchPattern : DiscoverySearchPattern
{
	private string pattern = "*.asmx";

	public override string Pattern => pattern;

	public override DiscoveryReference GetDiscoveryReference(string filename)
	{
		return new ContractReference
		{
			Url = filename,
			Ref = filename,
			DocRef = filename
		};
	}
}
