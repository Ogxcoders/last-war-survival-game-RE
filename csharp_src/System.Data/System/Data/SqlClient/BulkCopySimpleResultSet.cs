using System.Collections.Generic;

namespace System.Data.SqlClient;

internal sealed class BulkCopySimpleResultSet
{
	private readonly List<Result> _results;

	private Result _resultSet;

	private int[] _indexmap;

	internal Result this[int idx] => _results[idx];

	internal BulkCopySimpleResultSet()
	{
		_results = new List<Result>();
	}

	internal void SetMetaData(_SqlMetaDataSet metadata)
	{
		_resultSet = new Result(metadata);
		_results.Add(_resultSet);
		_indexmap = new int[_resultSet.MetaData.Length];
		for (int i = 0; i < _indexmap.Length; i++)
		{
			_indexmap[i] = i;
		}
	}

	internal int[] CreateIndexMap()
	{
		return _indexmap;
	}

	internal object[] CreateRowBuffer()
	{
		Row row = new Row(_resultSet.MetaData.Length);
		_resultSet.AddRow(row);
		return row.DataFields;
	}
}
