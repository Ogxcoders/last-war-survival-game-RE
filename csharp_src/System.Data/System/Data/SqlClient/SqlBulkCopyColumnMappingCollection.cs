using System.Collections;

namespace System.Data.SqlClient;

public sealed class SqlBulkCopyColumnMappingCollection : CollectionBase
{
	private enum MappingSchema
	{
		Undefined,
		NamesNames,
		NemesOrdinals,
		OrdinalsNames,
		OrdinalsOrdinals
	}

	private MappingSchema _mappingSchema;

	internal bool ReadOnly { get; set; }

	public SqlBulkCopyColumnMapping this[int index] => (SqlBulkCopyColumnMapping)base.List[index];

	internal SqlBulkCopyColumnMappingCollection()
	{
	}

	public SqlBulkCopyColumnMapping Add(SqlBulkCopyColumnMapping bulkCopyColumnMapping)
	{
		AssertWriteAccess();
		if ((string.IsNullOrEmpty(bulkCopyColumnMapping.SourceColumn) && bulkCopyColumnMapping.SourceOrdinal == -1) || (string.IsNullOrEmpty(bulkCopyColumnMapping.DestinationColumn) && bulkCopyColumnMapping.DestinationOrdinal == -1))
		{
			throw SQL.BulkLoadNonMatchingColumnMapping();
		}
		base.InnerList.Add(bulkCopyColumnMapping);
		return bulkCopyColumnMapping;
	}

	public SqlBulkCopyColumnMapping Add(string sourceColumn, string destinationColumn)
	{
		AssertWriteAccess();
		return Add(new SqlBulkCopyColumnMapping(sourceColumn, destinationColumn));
	}

	public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, string destinationColumn)
	{
		AssertWriteAccess();
		return Add(new SqlBulkCopyColumnMapping(sourceColumnIndex, destinationColumn));
	}

	public SqlBulkCopyColumnMapping Add(string sourceColumn, int destinationColumnIndex)
	{
		AssertWriteAccess();
		return Add(new SqlBulkCopyColumnMapping(sourceColumn, destinationColumnIndex));
	}

	public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, int destinationColumnIndex)
	{
		AssertWriteAccess();
		return Add(new SqlBulkCopyColumnMapping(sourceColumnIndex, destinationColumnIndex));
	}

	private void AssertWriteAccess()
	{
		if (ReadOnly)
		{
			throw SQL.BulkLoadMappingInaccessible();
		}
	}

	public new void Clear()
	{
		AssertWriteAccess();
		base.Clear();
	}

	public bool Contains(SqlBulkCopyColumnMapping value)
	{
		return base.InnerList.Contains(value);
	}

	public void CopyTo(SqlBulkCopyColumnMapping[] array, int index)
	{
		base.InnerList.CopyTo(array, index);
	}

	internal void CreateDefaultMapping(int columnCount)
	{
		for (int i = 0; i < columnCount; i++)
		{
			base.InnerList.Add(new SqlBulkCopyColumnMapping(i, i));
		}
	}

	public int IndexOf(SqlBulkCopyColumnMapping value)
	{
		return base.InnerList.IndexOf(value);
	}

	public void Insert(int index, SqlBulkCopyColumnMapping value)
	{
		AssertWriteAccess();
		base.InnerList.Insert(index, value);
	}

	public void Remove(SqlBulkCopyColumnMapping value)
	{
		AssertWriteAccess();
		base.InnerList.Remove(value);
	}

	public new void RemoveAt(int index)
	{
		AssertWriteAccess();
		base.RemoveAt(index);
	}

	internal void ValidateCollection()
	{
		foreach (SqlBulkCopyColumnMapping inner in base.InnerList)
		{
			MappingSchema mappingSchema = ((inner.SourceOrdinal == -1) ? ((inner.DestinationOrdinal == -1) ? MappingSchema.NamesNames : MappingSchema.NemesOrdinals) : ((inner.DestinationOrdinal != -1) ? MappingSchema.OrdinalsOrdinals : MappingSchema.OrdinalsNames));
			if (_mappingSchema == MappingSchema.Undefined)
			{
				_mappingSchema = mappingSchema;
			}
			else if (_mappingSchema != mappingSchema)
			{
				throw SQL.BulkLoadMappingsNamesOrOrdinalsOnly();
			}
		}
	}
}
