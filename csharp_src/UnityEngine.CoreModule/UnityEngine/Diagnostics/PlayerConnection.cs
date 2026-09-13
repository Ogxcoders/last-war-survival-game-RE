using System;
using System.ComponentModel;
using UnityEngine.Networking.PlayerConnection;

namespace UnityEngine.Diagnostics;

public static class PlayerConnection
{
	[Obsolete("Use UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.isConnected instead.")]
	public static bool connected => UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.isConnected;

	[Obsolete("PlayerConnection.SendFile is no longer supported.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void SendFile(string remoteFilePath, byte[] data)
	{
	}
}
