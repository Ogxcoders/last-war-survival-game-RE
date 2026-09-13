namespace System.Data.SqlClient;

internal sealed class SqlEnvChange
{
	internal byte type;

	internal byte oldLength;

	internal int newLength;

	internal int length;

	internal string newValue;

	internal string oldValue;

	internal byte[] newBinValue;

	internal byte[] oldBinValue;

	internal long newLongValue;

	internal long oldLongValue;

	internal SqlCollation newCollation;

	internal SqlCollation oldCollation;

	internal RoutingInfo newRoutingInfo;
}
