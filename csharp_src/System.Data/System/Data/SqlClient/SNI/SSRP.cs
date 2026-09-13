using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.SqlClient.SNI;

internal class SSRP
{
	private const char SemicolonSeparator = ';';

	private const int SqlServerBrowserPort = 1434;

	internal static int GetPortByInstanceName(string browserHostName, string instanceName)
	{
		byte[] requestPacket = CreateInstanceInfoRequest(instanceName);
		byte[] array = null;
		try
		{
			array = SendUDPRequest(browserHostName, 1434, requestPacket);
		}
		catch (SocketException innerException)
		{
			throw new Exception(SQLMessage.SqlServerBrowserNotAccessible(), innerException);
		}
		if (array == null || array.Length <= 3 || array[0] != 5 || BitConverter.ToUInt16(array, 1) != array.Length - 3)
		{
			throw new SocketException();
		}
		string[] array2 = Encoding.ASCII.GetString(array, 3, array.Length - 3).Split(';');
		int num = Array.IndexOf(array2, "tcp");
		if (num < 0 || num == array2.Length - 1)
		{
			throw new SocketException();
		}
		return ushort.Parse(array2[num + 1]);
	}

	private static byte[] CreateInstanceInfoRequest(string instanceName)
	{
		instanceName += "\0";
		byte[] array = new byte[Encoding.ASCII.GetByteCount(instanceName) + 1];
		array[0] = 4;
		Encoding.ASCII.GetBytes(instanceName, 0, instanceName.Length, array, 1);
		return array;
	}

	internal static int GetDacPortByInstanceName(string browserHostName, string instanceName)
	{
		byte[] requestPacket = CreateDacPortInfoRequest(instanceName);
		byte[] array = SendUDPRequest(browserHostName, 1434, requestPacket);
		if (array == null || array.Length <= 4 || array[0] != 5 || BitConverter.ToUInt16(array, 1) != 6 || array[3] != 1)
		{
			throw new SocketException();
		}
		return BitConverter.ToUInt16(array, 4);
	}

	private static byte[] CreateDacPortInfoRequest(string instanceName)
	{
		instanceName += "\0";
		byte[] array = new byte[Encoding.ASCII.GetByteCount(instanceName) + 2];
		array[0] = 15;
		array[1] = 1;
		Encoding.ASCII.GetBytes(instanceName, 0, instanceName.Length, array, 2);
		return array;
	}

	private static byte[] SendUDPRequest(string browserHostname, int port, byte[] requestPacket)
	{
		IPAddress address = null;
		bool num = IPAddress.TryParse(browserHostname, out address);
		byte[] result = null;
		using (UdpClient udpClient = new UdpClient((!num) ? AddressFamily.InterNetwork : address.AddressFamily))
		{
			Task<int> task = udpClient.SendAsync(requestPacket, requestPacket.Length, browserHostname, port);
			Task<UdpReceiveResult> task2 = null;
			if (task.Wait(1000) && (task2 = udpClient.ReceiveAsync()).Wait(1000))
			{
				result = task2.Result.Buffer;
			}
		}
		return result;
	}
}
