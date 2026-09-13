namespace UnityEngine.XR.WSA;

public class HolographicRemoting
{
	public static HolographicStreamerConnectionState ConnectionState => HolographicStreamerConnectionState.Disconnected;

	public static void Connect(string clientName, int maxBitRate = 9999)
	{
		Connect(clientName, maxBitRate, RemoteDeviceVersion.V1);
	}

	public static void Connect(string clientName, int maxBitRate, RemoteDeviceVersion deviceVersion)
	{
	}

	public static void Disconnect()
	{
	}
}
