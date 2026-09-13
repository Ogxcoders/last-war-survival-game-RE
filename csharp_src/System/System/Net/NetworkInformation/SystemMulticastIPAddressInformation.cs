namespace System.Net.NetworkInformation;

internal class SystemMulticastIPAddressInformation : MulticastIPAddressInformation
{
	private SystemIPAddressInformation innerInfo;

	public override IPAddress Address => innerInfo.Address;

	public override bool IsTransient => innerInfo.IsTransient;

	public override bool IsDnsEligible => innerInfo.IsDnsEligible;

	public override PrefixOrigin PrefixOrigin => PrefixOrigin.Other;

	public override SuffixOrigin SuffixOrigin => SuffixOrigin.Other;

	public override DuplicateAddressDetectionState DuplicateAddressDetectionState => DuplicateAddressDetectionState.Invalid;

	public override long AddressValidLifetime => 0L;

	public override long AddressPreferredLifetime => 0L;

	public override long DhcpLeaseLifetime => 0L;

	private SystemMulticastIPAddressInformation()
	{
	}

	public SystemMulticastIPAddressInformation(SystemIPAddressInformation addressInfo)
	{
		innerInfo = addressInfo;
	}

	internal static MulticastIPAddressInformationCollection ToMulticastIpAddressInformationCollection(IPAddressInformationCollection addresses)
	{
		MulticastIPAddressInformationCollection multicastIPAddressInformationCollection = new MulticastIPAddressInformationCollection();
		foreach (IPAddressInformation address in addresses)
		{
			multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation((SystemIPAddressInformation)address));
		}
		return multicastIPAddressInformationCollection;
	}
}
