namespace System.Data.SqlClient;

internal enum PreLoginOptions
{
	VERSION = 0,
	ENCRYPT = 1,
	INSTANCE = 2,
	THREADID = 3,
	MARS = 4,
	TRACEID = 5,
	FEDAUTHREQUIRED = 6,
	NUMOPT = 7,
	LASTOPT = 255
}
