using System.Data.Common;

namespace System.Data.SqlClient;

public sealed class SqlBulkCopyColumnMapping
{
	internal string _destinationColumnName;

	internal int _destinationColumnOrdinal;

	internal string _sourceColumnName;

	internal int _sourceColumnOrdinal;

	internal int _internalDestinationColumnOrdinal;

	internal int _internalSourceColumnOrdinal;

	public string DestinationColumn
	{
		get
		{
			if (_destinationColumnName != null)
			{
				return _destinationColumnName;
			}
			return string.Empty;
		}
		set
		{
			_destinationColumnOrdinal = (_internalDestinationColumnOrdinal = -1);
			_destinationColumnName = value;
		}
	}

	public int DestinationOrdinal
	{
		get
		{
			return _destinationColumnOrdinal;
		}
		set
		{
			if (value >= 0)
			{
				_destinationColumnName = null;
				_destinationColumnOrdinal = (_internalDestinationColumnOrdinal = value);
				return;
			}
			throw ADP.IndexOutOfRange(value);
		}
	}

	public string SourceColumn
	{
		get
		{
			if (_sourceColumnName != null)
			{
				return _sourceColumnName;
			}
			return string.Empty;
		}
		set
		{
			_sourceColumnOrdinal = (_internalSourceColumnOrdinal = -1);
			_sourceColumnName = value;
		}
	}

	public int SourceOrdinal
	{
		get
		{
			return _sourceColumnOrdinal;
		}
		set
		{
			if (value >= 0)
			{
				_sourceColumnName = null;
				_sourceColumnOrdinal = (_internalSourceColumnOrdinal = value);
				return;
			}
			throw ADP.IndexOutOfRange(value);
		}
	}

	public SqlBulkCopyColumnMapping()
	{
		_internalSourceColumnOrdinal = -1;
	}

	public SqlBulkCopyColumnMapping(string sourceColumn, string destinationColumn)
	{
		SourceColumn = sourceColumn;
		DestinationColumn = destinationColumn;
	}

	public SqlBulkCopyColumnMapping(int sourceColumnOrdinal, string destinationColumn)
	{
		SourceOrdinal = sourceColumnOrdinal;
		DestinationColumn = destinationColumn;
	}

	public SqlBulkCopyColumnMapping(string sourceColumn, int destinationOrdinal)
	{
		SourceColumn = sourceColumn;
		DestinationOrdinal = destinationOrdinal;
	}

	public SqlBulkCopyColumnMapping(int sourceColumnOrdinal, int destinationOrdinal)
	{
		SourceOrdinal = sourceColumnOrdinal;
		DestinationOrdinal = destinationOrdinal;
	}
}
