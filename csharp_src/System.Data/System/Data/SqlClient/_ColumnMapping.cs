namespace System.Data.SqlClient;

internal sealed class _ColumnMapping
{
	internal int _sourceColumnOrdinal;

	internal _SqlMetaData _metadata;

	internal _ColumnMapping(int columnId, _SqlMetaData metadata)
	{
		_sourceColumnOrdinal = columnId;
		_metadata = metadata;
	}
}
