using System.Collections.ObjectModel;
using System.Data.Common;

namespace System.Data.SqlClient;

internal sealed class _SqlMetaDataSet
{
	internal ushort id;

	internal int[] indexMap;

	internal int visibleColumns;

	internal DataTable schemaTable;

	private readonly _SqlMetaData[] _metaDataArray;

	internal ReadOnlyCollection<DbColumn> dbColumnSchema;

	internal int Length => _metaDataArray.Length;

	internal _SqlMetaData this[int index]
	{
		get
		{
			return _metaDataArray[index];
		}
		set
		{
			_metaDataArray[index] = value;
		}
	}

	internal _SqlMetaDataSet(int count)
	{
		_metaDataArray = new _SqlMetaData[count];
		for (int i = 0; i < _metaDataArray.Length; i++)
		{
			_metaDataArray[i] = new _SqlMetaData(i);
		}
	}

	private _SqlMetaDataSet(_SqlMetaDataSet original)
	{
		id = original.id;
		indexMap = original.indexMap;
		visibleColumns = original.visibleColumns;
		dbColumnSchema = original.dbColumnSchema;
		if (original._metaDataArray == null)
		{
			_metaDataArray = null;
			return;
		}
		_metaDataArray = new _SqlMetaData[original._metaDataArray.Length];
		for (int i = 0; i < _metaDataArray.Length; i++)
		{
			_metaDataArray[i] = (_SqlMetaData)original._metaDataArray[i].Clone();
		}
	}

	public object Clone()
	{
		return new _SqlMetaDataSet(this);
	}
}
