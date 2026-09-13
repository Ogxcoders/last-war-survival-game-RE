namespace System.Data.SqlClient.SNI;

[Flags]
internal enum SNISMUXFlags
{
	SMUX_SYN = 1,
	SMUX_ACK = 2,
	SMUX_FIN = 4,
	SMUX_DATA = 8
}
