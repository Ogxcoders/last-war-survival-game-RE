using System.Net.Sockets;

namespace System.Net.NetworkInformation;

internal class LinuxUnicastIPAddressInformation : UnicastIPAddressInformation
{
	private IPAddress address;

	private IPAddress ipv4Mask;

	public override IPAddress Address => address;

	public override bool IsDnsEligible
	{
		get
		{
			byte[] addressBytes = address.GetAddressBytes();
			if (addressBytes[0] == 169)
			{
				return addressBytes[1] != 254;
			}
			return true;
		}
	}

	[System.MonoTODO("Always returns false")]
	public override bool IsTransient => false;

	public override long AddressPreferredLifetime
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long AddressValidLifetime
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long DhcpLeaseLifetime
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override DuplicateAddressDetectionState DuplicateAddressDetectionState
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IPAddress IPv4Mask
	{
		get
		{
			if (Address.AddressFamily != AddressFamily.InterNetwork)
			{
				return IPAddress.Any;
			}
			if (ipv4Mask == null)
			{
				ipv4Mask = SystemNetworkInterface.GetNetMask(address);
			}
			return ipv4Mask;
		}
	}

	public override PrefixOrigin PrefixOrigin
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override SuffixOrigin SuffixOrigin
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public LinuxUnicastIPAddressInformation(IPAddress address)
	{
		this.address = address;
	}
}
