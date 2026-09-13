using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal sealed class LinuxNetworkInterface : UnixNetworkInterface
{
	private string iface_path;

	private string iface_operstate_path;

	private string iface_flags_path;

	private bool android_use_java_api;

	internal string IfacePath => iface_path;

	public override OperationalStatus OperationalStatus
	{
		get
		{
			if (android_use_java_api)
			{
				bool is_up = false;
				if (_monodroid_get_network_interface_up_state(Name, ref is_up))
				{
					if (!is_up)
					{
						return OperationalStatus.Down;
					}
					return OperationalStatus.Up;
				}
				return OperationalStatus.Unknown;
			}
			if (!Directory.Exists(iface_path))
			{
				return OperationalStatus.Unknown;
			}
			try
			{
				switch (ReadLine(iface_operstate_path))
				{
				case "unknown":
					return OperationalStatus.Unknown;
				case "notpresent":
					return OperationalStatus.NotPresent;
				case "down":
					return OperationalStatus.Down;
				case "lowerlayerdown":
					return OperationalStatus.LowerLayerDown;
				case "testing":
					return OperationalStatus.Testing;
				case "dormant":
					return OperationalStatus.Dormant;
				case "up":
					return OperationalStatus.Up;
				}
			}
			catch
			{
			}
			return OperationalStatus.Unknown;
		}
	}

	public override bool SupportsMulticast
	{
		get
		{
			if (android_use_java_api)
			{
				bool supports_multicast = false;
				_monodroid_get_network_interface_supports_multicast(Name, ref supports_multicast);
				return supports_multicast;
			}
			if (!Directory.Exists(iface_path))
			{
				return false;
			}
			try
			{
				string text = ReadLine(iface_flags_path);
				if (text.Length > 2 && text[0] == '0' && text[1] == 'x')
				{
					text = text.Substring(2);
				}
				return (ulong.Parse(text, NumberStyles.HexNumber) & 0x1000) == 4096;
			}
			catch
			{
				return false;
			}
		}
	}

	[DllImport("__Internal")]
	private static extern int _monodroid_get_android_api_level();

	[DllImport("__Internal")]
	private static extern bool _monodroid_get_network_interface_up_state(string ifname, ref bool is_up);

	[DllImport("__Internal")]
	private static extern bool _monodroid_get_network_interface_supports_multicast(string ifname, ref bool supports_multicast);

	internal LinuxNetworkInterface(string name)
		: base(name)
	{
		iface_path = "/sys/class/net/" + name + "/";
		iface_operstate_path = iface_path + "operstate";
		iface_flags_path = iface_path + "flags";
		android_use_java_api = _monodroid_get_android_api_level() >= 24;
	}

	public override IPInterfaceProperties GetIPProperties()
	{
		if (ipproperties == null)
		{
			ipproperties = new LinuxIPInterfaceProperties(this, addresses);
		}
		return ipproperties;
	}

	public override IPv4InterfaceStatistics GetIPv4Statistics()
	{
		if (ipv4stats == null)
		{
			ipv4stats = new LinuxIPv4InterfaceStatistics(this);
		}
		return ipv4stats;
	}

	internal static string ReadLine(string path)
	{
		using FileStream stream = File.OpenRead(path);
		using StreamReader streamReader = new StreamReader(stream);
		return streamReader.ReadLine();
	}
}
