namespace System.Data.SqlClient.SNI;

internal class LocalDB
{
	internal static string GetLocalDBConnectionString(string localDbInstance)
	{
		throw new PlatformNotSupportedException("LocalDB is not supported on this platform.");
	}
}
