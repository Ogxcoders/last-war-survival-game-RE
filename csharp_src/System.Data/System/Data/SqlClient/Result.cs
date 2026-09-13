using System.Collections.Generic;

namespace System.Data.SqlClient;

internal sealed class Result
{
	private readonly _SqlMetaDataSet _metadata;

	private readonly List<Row> _rowset;

	internal int Count => _rowset.Count;

	internal _SqlMetaDataSet MetaData => _metadata;

	internal Row this[int index] => _rowset[index];

	internal Result(_SqlMetaDataSet metadata)
	{
		_metadata = metadata;
		_rowset = new List<Row>();
	}

	internal void AddRow(Row row)
	{
		_rowset.Add(row);
	}
}
