using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Joker;

public static class NetworkHelper
{
	public static string[] GetAddressIPs()
	{
		List<string> list = new List<string>();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
			{
				continue;
			}
			foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
			{
				list.Add(unicastAddress.Address.ToString());
			}
		}
		return list.ToArray();
	}

	public static IPAddress GetHostAddress(string hostName)
	{
		IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
		IPAddress result = null;
		IPAddress[] array = hostAddresses;
		foreach (IPAddress iPAddress in array)
		{
			result = iPAddress;
			if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
			{
				return iPAddress;
			}
		}
		return result;
	}

	public static IPEndPoint ToIPEndPoint(string host, int port)
	{
		return new IPEndPoint(IPAddress.Parse(host), port);
	}

	public static IPEndPoint ToIPEndPoint(string address)
	{
		int num = address.LastIndexOf(':');
		string host = address.Substring(0, num);
		int port = int.Parse(address.Substring(num + 1));
		return ToIPEndPoint(host, port);
	}

	public static void SetSioUdpConnReset(Socket socket)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			socket.IOControl(-1744830452, new byte[1] { Convert.ToByte(value: false) }, null);
		}
	}
}
