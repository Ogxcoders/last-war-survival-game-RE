namespace System.Net.NetworkInformation;

internal class SystemIPAddressInformation : IPAddressInformation
{
	private IPAddress address;

	internal bool transient;

	internal bool dnsEligible = true;

	public override IPAddress Address => address;

	public override bool IsTransient => transient;

	public override bool IsDnsEligible => dnsEligible;

	public SystemIPAddressInformation(IPAddress address, bool isDnsEligible, bool isTransient)
	{
		this.address = address;
		dnsEligible = isDnsEligible;
		transient = isTransient;
	}
}
