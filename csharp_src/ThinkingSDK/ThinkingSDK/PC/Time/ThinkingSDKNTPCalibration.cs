using System;
using System.Net;
using System.Net.Sockets;

namespace ThinkingSDK.PC.Time;

public class ThinkingSDKNTPCalibration : ThinkingSDKTimeCalibration
{
	public ThinkingSDKNTPCalibration(string ntpServer)
	{
		double num = ConvertDateTimeInt(GetNetworkTime(ntpServer));
		mStartTime = (long)num;
		mSystemElapsedRealtime = Environment.TickCount;
	}

	private static double ConvertDateTimeInt(DateTime time)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (time - dateTime).TotalMilliseconds;
	}

	private static DateTime GetNetworkTime(string ntpServer)
	{
		byte[] array = new byte[48];
		array[0] = 27;
		IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(ntpServer).AddressList[0], 123);
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		socket.Connect(remoteEP);
		socket.ReceiveTimeout = 3000;
		socket.Send(array);
		socket.Receive(array);
		socket.Close();
		long x = BitConverter.ToUInt32(array, 40);
		ulong x2 = BitConverter.ToUInt32(array, 44);
		long num = SwapEndianness((ulong)x);
		x2 = SwapEndianness(x2);
		ulong num2 = (ulong)(num * 1000) + x2 * 1000 / 4294967296L;
		return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)num2);
	}

	private static uint SwapEndianness(ulong x)
	{
		return (uint)(((x & 0xFF) << 24) + ((x & 0xFF00) << 8) + ((x & 0xFF0000) >> 8) + ((x & 0xFF000000u) >> 24));
	}
}
