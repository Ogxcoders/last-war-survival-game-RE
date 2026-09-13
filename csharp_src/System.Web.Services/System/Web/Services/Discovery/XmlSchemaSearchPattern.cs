namespace System.Web.Services.Discovery;

public sealed class XmlSchemaSearchPattern : DiscoverySearchPattern
{
	private string pattern = "*.xsd";

	public override string Pattern => pattern;

	public override DiscoveryReference GetDiscoveryReference(string filename)
	{
		return new SchemaReference
		{
			Url = filename,
			Ref = filename
		};
	}
}
