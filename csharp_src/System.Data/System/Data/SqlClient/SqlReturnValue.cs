namespace System.Data.SqlClient;

internal sealed class SqlReturnValue : SqlMetaDataPriv
{
	internal string parameter;

	internal readonly SqlBuffer value;

	internal SqlReturnValue()
	{
		value = new SqlBuffer();
	}
}
