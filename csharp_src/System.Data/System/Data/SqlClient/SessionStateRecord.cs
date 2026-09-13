namespace System.Data.SqlClient;

internal class SessionStateRecord
{
	internal bool _recoverable;

	internal uint _version;

	internal int _dataLength;

	internal byte[] _data;
}
