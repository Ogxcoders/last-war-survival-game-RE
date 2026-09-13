using System.IO;

namespace System.Net.NetworkInformation;

internal sealed class LinuxIPv4InterfaceProperties : UnixIPv4InterfaceProperties
{
	public override bool IsForwardingEnabled
	{
		get
		{
			string path = "/proc/sys/net/ipv4/conf/" + iface.Name + "/forwarding";
			if (File.Exists(path))
			{
				return LinuxNetworkInterface.ReadLine(path) != "0";
			}
			return false;
		}
	}

	public override int Mtu
	{
		get
		{
			string path = (iface as LinuxNetworkInterface).IfacePath + "mtu";
			int result = 0;
			if (File.Exists(path))
			{
				string s = LinuxNetworkInterface.ReadLine(path);
				try
				{
					result = int.Parse(s);
				}
				catch
				{
				}
			}
			return result;
		}
	}

	public LinuxIPv4InterfaceProperties(LinuxNetworkInterface iface)
		: base(iface)
	{
	}
}
