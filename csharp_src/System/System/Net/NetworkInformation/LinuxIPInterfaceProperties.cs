using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace System.Net.NetworkInformation;

internal class LinuxIPInterfaceProperties : UnixIPInterfaceProperties
{
	public override GatewayIPAddressInformationCollection GatewayAddresses => SystemGatewayIPAddressInformation.ToGatewayIpAddressInformationCollection(ParseRouteInfo(iface.Name.ToString()));

	public LinuxIPInterfaceProperties(LinuxNetworkInterface iface, List<IPAddress> addresses)
		: base(iface, addresses)
	{
	}

	public override IPv4InterfaceProperties GetIPv4Properties()
	{
		if (ipv4iface_properties == null)
		{
			ipv4iface_properties = new LinuxIPv4InterfaceProperties(iface as LinuxNetworkInterface);
		}
		return ipv4iface_properties;
	}

	private IPAddressCollection ParseRouteInfo(string iface)
	{
		IPAddressCollection iPAddressCollection = new IPAddressCollection();
		try
		{
			using StreamReader streamReader = new StreamReader("/proc/net/route");
			streamReader.ReadLine();
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				text = text.Trim();
				if (text.Length == 0)
				{
					continue;
				}
				string[] array = text.Split('\t');
				if (array.Length < 3)
				{
					continue;
				}
				string text2 = array[2].Trim();
				byte[] array2 = new byte[4];
				if (text2.Length == 8 && iface.Equals(array[0], StringComparison.OrdinalIgnoreCase))
				{
					for (int i = 0; i < 4; i++)
					{
						byte.TryParse(text2.Substring(i * 2, 2), NumberStyles.HexNumber, null, out array2[3 - i]);
					}
					IPAddress iPAddress = new IPAddress(array2);
					if (!iPAddress.Equals(IPAddress.Any) && !iPAddressCollection.Contains(iPAddress))
					{
						iPAddressCollection.InternalAdd(iPAddress);
					}
				}
			}
		}
		catch
		{
		}
		return iPAddressCollection;
	}
}
