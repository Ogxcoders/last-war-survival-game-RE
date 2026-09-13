using System.Data.Common;
using System.Globalization;
using System.IO;

namespace System.Data.ProviderBase;

internal class DbMetaDataFactory
{
	private DataSet _metaDataCollectionsDataSet;

	private string _normalizedServerVersion;

	private string _serverVersionString;

	private const string _collectionName = "CollectionName";

	private const string _populationMechanism = "PopulationMechanism";

	private const string _populationString = "PopulationString";

	private const string _maximumVersion = "MaximumVersion";

	private const string _minimumVersion = "MinimumVersion";

	private const string _dataSourceProductVersionNormalized = "DataSourceProductVersionNormalized";

	private const string _dataSourceProductVersion = "DataSourceProductVersion";

	private const string _restrictionDefault = "RestrictionDefault";

	private const string _restrictionNumber = "RestrictionNumber";

	private const string _numberOfRestrictions = "NumberOfRestrictions";

	private const string _restrictionName = "RestrictionName";

	private const string _parameterName = "ParameterName";

	private const string _dataTable = "DataTable";

	private const string _sqlCommand = "SQLCommand";

	private const string _prepareCollection = "PrepareCollection";

	protected DataSet CollectionDataSet => _metaDataCollectionsDataSet;

	protected string ServerVersion => _serverVersionString;

	protected string ServerVersionNormalized => _normalizedServerVersion;

	public DbMetaDataFactory(Stream xmlStream, string serverVersion, string normalizedServerVersion)
	{
		ADP.CheckArgumentNull(xmlStream, "xmlStream");
		ADP.CheckArgumentNull(serverVersion, "serverVersion");
		ADP.CheckArgumentNull(normalizedServerVersion, "normalizedServerVersion");
		LoadDataSetFromXml(xmlStream);
		_serverVersionString = serverVersion;
		_normalizedServerVersion = normalizedServerVersion;
	}

	protected DataTable CloneAndFilterCollection(string collectionName, string[] hiddenColumnNames)
	{
		DataTable dataTable = _metaDataCollectionsDataSet.Tables[collectionName];
		if (dataTable == null || collectionName != dataTable.TableName)
		{
			throw ADP.DataTableDoesNotExist(collectionName);
		}
		DataTable dataTable2 = new DataTable(collectionName)
		{
			Locale = CultureInfo.InvariantCulture
		};
		DataColumnCollection columns = dataTable2.Columns;
		DataColumn[] array = FilterColumns(dataTable, hiddenColumnNames, columns);
		foreach (DataRow row in dataTable.Rows)
		{
			if (SupportedByCurrentVersion(row))
			{
				DataRow dataRow2 = dataTable2.NewRow();
				for (int i = 0; i < columns.Count; i++)
				{
					dataRow2[columns[i]] = row[array[i], DataRowVersion.Current];
				}
				dataTable2.Rows.Add(dataRow2);
				dataRow2.AcceptChanges();
			}
		}
		return dataTable2;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_normalizedServerVersion = null;
			_serverVersionString = null;
			_metaDataCollectionsDataSet.Dispose();
		}
	}

	private DataTable ExecuteCommand(DataRow requestedCollectionRow, string[] restrictions, DbConnection connection)
	{
		DataTable dataTable = _metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections];
		DataColumn column = dataTable.Columns["PopulationString"];
		DataColumn column2 = dataTable.Columns["NumberOfRestrictions"];
		DataColumn column3 = dataTable.Columns["CollectionName"];
		DataTable dataTable2 = null;
		DbCommand dbCommand = null;
		string commandText = requestedCollectionRow[column, DataRowVersion.Current] as string;
		int num = (int)requestedCollectionRow[column2, DataRowVersion.Current];
		string text = requestedCollectionRow[column3, DataRowVersion.Current] as string;
		if (restrictions != null && restrictions.Length > num)
		{
			throw ADP.TooManyRestrictions(text);
		}
		dbCommand = connection.CreateCommand();
		dbCommand.CommandText = commandText;
		dbCommand.CommandTimeout = Math.Max(dbCommand.CommandTimeout, 180);
		for (int i = 0; i < num; i++)
		{
			DbParameter dbParameter = dbCommand.CreateParameter();
			if (restrictions != null && restrictions.Length > i && restrictions[i] != null)
			{
				dbParameter.Value = restrictions[i];
			}
			else
			{
				dbParameter.Value = DBNull.Value;
			}
			dbParameter.ParameterName = GetParameterName(text, i + 1);
			dbParameter.Direction = ParameterDirection.Input;
			dbCommand.Parameters.Add(dbParameter);
		}
		DbDataReader dbDataReader = null;
		try
		{
			try
			{
				dbDataReader = dbCommand.ExecuteReader();
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				throw ADP.QueryFailed(text, e);
			}
			dataTable2 = new DataTable(text)
			{
				Locale = CultureInfo.InvariantCulture
			};
			foreach (DataRow row in dbDataReader.GetSchemaTable().Rows)
			{
				dataTable2.Columns.Add(row["ColumnName"] as string, (Type)row["DataType"]);
			}
			object[] values = new object[dataTable2.Columns.Count];
			while (dbDataReader.Read())
			{
				dbDataReader.GetValues(values);
				dataTable2.Rows.Add(values);
			}
			return dataTable2;
		}
		finally
		{
			if (dbDataReader != null)
			{
				dbDataReader.Dispose();
				dbDataReader = null;
			}
		}
	}

	private DataColumn[] FilterColumns(DataTable sourceTable, string[] hiddenColumnNames, DataColumnCollection destinationColumns)
	{
		int num = 0;
		foreach (DataColumn column2 in sourceTable.Columns)
		{
			if (IncludeThisColumn(column2, hiddenColumnNames))
			{
				num++;
			}
		}
		if (num == 0)
		{
			throw ADP.NoColumns();
		}
		int num2 = 0;
		DataColumn[] array = new DataColumn[num];
		foreach (DataColumn column3 in sourceTable.Columns)
		{
			if (IncludeThisColumn(column3, hiddenColumnNames))
			{
				DataColumn column = new DataColumn(column3.ColumnName, column3.DataType);
				destinationColumns.Add(column);
				array[num2] = column3;
				num2++;
			}
		}
		return array;
	}

	internal DataRow FindMetaDataCollectionRow(string collectionName)
	{
		DataTable obj = _metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections] ?? throw ADP.InvalidXml();
		DataColumn dataColumn = obj.Columns[DbMetaDataColumnNames.CollectionName];
		if (dataColumn == null || typeof(string) != dataColumn.DataType)
		{
			throw ADP.InvalidXmlMissingColumn(DbMetaDataCollectionNames.MetaDataCollections, DbMetaDataColumnNames.CollectionName);
		}
		DataRow dataRow = null;
		string text = null;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (DataRow row in obj.Rows)
		{
			string text2 = row[dataColumn, DataRowVersion.Current] as string;
			if (string.IsNullOrEmpty(text2))
			{
				throw ADP.InvalidXmlInvalidValue(DbMetaDataCollectionNames.MetaDataCollections, DbMetaDataColumnNames.CollectionName);
			}
			if (!ADP.CompareInsensitiveInvariant(text2, collectionName))
			{
				continue;
			}
			if (!SupportedByCurrentVersion(row))
			{
				flag = true;
			}
			else if (collectionName == text2)
			{
				if (flag2)
				{
					throw ADP.CollectionNameIsNotUnique(collectionName);
				}
				dataRow = row;
				text = text2;
				flag2 = true;
			}
			else
			{
				if (text != null)
				{
					flag3 = true;
				}
				dataRow = row;
				text = text2;
			}
		}
		if (dataRow == null)
		{
			if (!flag)
			{
				throw ADP.UndefinedCollection(collectionName);
			}
			throw ADP.UnsupportedVersion(collectionName);
		}
		if (!flag2 && flag3)
		{
			throw ADP.AmbigousCollectionName(collectionName);
		}
		return dataRow;
	}

	private void FixUpVersion(DataTable dataSourceInfoTable)
	{
		DataColumn dataColumn = dataSourceInfoTable.Columns["DataSourceProductVersion"];
		DataColumn dataColumn2 = dataSourceInfoTable.Columns["DataSourceProductVersionNormalized"];
		if (dataColumn == null || dataColumn2 == null)
		{
			throw ADP.MissingDataSourceInformationColumn();
		}
		if (dataSourceInfoTable.Rows.Count != 1)
		{
			throw ADP.IncorrectNumberOfDataSourceInformationRows();
		}
		DataRow dataRow = dataSourceInfoTable.Rows[0];
		dataRow[dataColumn] = _serverVersionString;
		dataRow[dataColumn2] = _normalizedServerVersion;
		dataRow.AcceptChanges();
	}

	private string GetParameterName(string neededCollectionName, int neededRestrictionNumber)
	{
		DataTable dataTable = null;
		DataColumnCollection dataColumnCollection = null;
		DataColumn dataColumn = null;
		DataColumn dataColumn2 = null;
		DataColumn dataColumn3 = null;
		DataColumn dataColumn4 = null;
		string text = null;
		dataTable = _metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.Restrictions];
		if (dataTable != null)
		{
			dataColumnCollection = dataTable.Columns;
			if (dataColumnCollection != null)
			{
				dataColumn = dataColumnCollection["CollectionName"];
				dataColumn2 = dataColumnCollection["ParameterName"];
				dataColumn3 = dataColumnCollection["RestrictionName"];
				dataColumn4 = dataColumnCollection["RestrictionNumber"];
			}
		}
		if (dataColumn2 == null || dataColumn == null || dataColumn3 == null || dataColumn4 == null)
		{
			throw ADP.MissingRestrictionColumn();
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if ((string)row[dataColumn] == neededCollectionName && (int)row[dataColumn4] == neededRestrictionNumber && SupportedByCurrentVersion(row))
			{
				text = (string)row[dataColumn2];
				break;
			}
		}
		if (text == null)
		{
			throw ADP.MissingRestrictionRow();
		}
		return text;
	}

	public virtual DataTable GetSchema(DbConnection connection, string collectionName, string[] restrictions)
	{
		DataTable dataTable = _metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections];
		DataColumn column = dataTable.Columns["PopulationMechanism"];
		DataColumn column2 = dataTable.Columns[DbMetaDataColumnNames.CollectionName];
		DataRow dataRow = null;
		DataTable dataTable2 = null;
		string text = null;
		dataRow = FindMetaDataCollectionRow(collectionName);
		text = dataRow[column2, DataRowVersion.Current] as string;
		if (!ADP.IsEmptyArray(restrictions))
		{
			for (int i = 0; i < restrictions.Length; i++)
			{
				if (restrictions[i] != null && restrictions[i].Length > 4096)
				{
					throw ADP.NotSupported();
				}
			}
		}
		string text2 = dataRow[column, DataRowVersion.Current] as string;
		switch (text2)
		{
		case "DataTable":
		{
			string[] hiddenColumnNames = ((!(text == DbMetaDataCollectionNames.MetaDataCollections)) ? null : new string[2] { "PopulationMechanism", "PopulationString" });
			if (!ADP.IsEmptyArray(restrictions))
			{
				throw ADP.TooManyRestrictions(text);
			}
			dataTable2 = CloneAndFilterCollection(text, hiddenColumnNames);
			if (text == DbMetaDataCollectionNames.DataSourceInformation)
			{
				FixUpVersion(dataTable2);
			}
			break;
		}
		case "SQLCommand":
			dataTable2 = ExecuteCommand(dataRow, restrictions, connection);
			break;
		case "PrepareCollection":
			dataTable2 = PrepareCollection(text, restrictions, connection);
			break;
		default:
			throw ADP.UndefinedPopulationMechanism(text2);
		}
		return dataTable2;
	}

	private bool IncludeThisColumn(DataColumn sourceColumn, string[] hiddenColumnNames)
	{
		bool result = true;
		string columnName = sourceColumn.ColumnName;
		if (columnName == "MinimumVersion" || columnName == "MaximumVersion")
		{
			result = false;
		}
		else if (hiddenColumnNames != null)
		{
			for (int i = 0; i < hiddenColumnNames.Length; i++)
			{
				if (hiddenColumnNames[i] == columnName)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	private void LoadDataSetFromXml(Stream XmlStream)
	{
		_metaDataCollectionsDataSet = new DataSet();
		_metaDataCollectionsDataSet.Locale = CultureInfo.InvariantCulture;
		_metaDataCollectionsDataSet.ReadXml(XmlStream);
	}

	protected virtual DataTable PrepareCollection(string collectionName, string[] restrictions, DbConnection connection)
	{
		throw ADP.NotSupported();
	}

	private bool SupportedByCurrentVersion(DataRow requestedCollectionRow)
	{
		bool flag = true;
		DataColumnCollection columns = requestedCollectionRow.Table.Columns;
		DataColumn dataColumn = columns["MinimumVersion"];
		if (dataColumn != null)
		{
			object obj = requestedCollectionRow[dataColumn];
			if (obj != null && obj != DBNull.Value && 0 > string.Compare(_normalizedServerVersion, (string)obj, StringComparison.OrdinalIgnoreCase))
			{
				flag = false;
			}
		}
		if (flag)
		{
			dataColumn = columns["MaximumVersion"];
			if (dataColumn != null)
			{
				object obj = requestedCollectionRow[dataColumn];
				if (obj != null && obj != DBNull.Value && 0 < string.Compare(_normalizedServerVersion, (string)obj, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
			}
		}
		return flag;
	}
}
