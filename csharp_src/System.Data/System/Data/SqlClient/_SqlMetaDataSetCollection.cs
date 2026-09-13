using System.Collections.Generic;

namespace System.Data.SqlClient;

internal sealed class _SqlMetaDataSetCollection
{
	private readonly List<_SqlMetaDataSet> _altMetaDataSetArray;

	internal _SqlMetaDataSet metaDataSet;

	internal _SqlMetaDataSetCollection()
	{
		_altMetaDataSetArray = new List<_SqlMetaDataSet>();
	}

	internal void SetAltMetaData(_SqlMetaDataSet altMetaDataSet)
	{
		int id = altMetaDataSet.id;
		for (int i = 0; i < _altMetaDataSetArray.Count; i++)
		{
			if (_altMetaDataSetArray[i].id == id)
			{
				_altMetaDataSetArray[i] = altMetaDataSet;
				return;
			}
		}
		_altMetaDataSetArray.Add(altMetaDataSet);
	}

	internal _SqlMetaDataSet GetAltMetaData(int id)
	{
		foreach (_SqlMetaDataSet item in _altMetaDataSetArray)
		{
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}

	public object Clone()
	{
		_SqlMetaDataSetCollection sqlMetaDataSetCollection = new _SqlMetaDataSetCollection();
		sqlMetaDataSetCollection.metaDataSet = ((metaDataSet == null) ? null : ((_SqlMetaDataSet)metaDataSet.Clone()));
		foreach (_SqlMetaDataSet item in _altMetaDataSetArray)
		{
			sqlMetaDataSetCollection._altMetaDataSetArray.Add((_SqlMetaDataSet)item.Clone());
		}
		return sqlMetaDataSetCollection;
	}
}
