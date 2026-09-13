namespace System.Data.SqlClient;

internal class RoutingInfo
{
	internal byte Protocol { get; private set; }

	internal ushort Port { get; private set; }

	internal string ServerName { get; private set; }

	internal RoutingInfo(byte protocol, ushort port, string servername)
	{
		Protocol = protocol;
		Port = port;
		ServerName = servername;
	}
}
