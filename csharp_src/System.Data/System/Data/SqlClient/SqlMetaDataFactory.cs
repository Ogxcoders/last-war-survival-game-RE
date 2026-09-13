using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Text;

namespace System.Data.SqlClient;

internal sealed class SqlMetaDataFactory : DbMetaDataFactory
{
	private const string _serverVersionNormalized90 = "09.00.0000";

	private const string _serverVersionNormalized90782 = "09.00.0782";

	private const string _serverVersionNormalized10 = "10.00.0000";

	public SqlMetaDataFactory(Stream XMLStream, string serverVersion, string serverVersionNormalized)
		: base(XMLStream, serverVersion, serverVersionNormalized)
	{
	}

	private void addUDTsToDataTypesTable(DataTable dataTypesTable, SqlConnection connection, string ServerVersion)
	{
		if (0 > string.Compare(ServerVersion, "09.00.0000", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		SqlCommand sqlCommand = connection.CreateCommand();
		sqlCommand.CommandText = "select assemblies.name, types.assembly_class, ASSEMBLYPROPERTY(assemblies.name, 'VersionMajor') as version_major, ASSEMBLYPROPERTY(assemblies.name, 'VersionMinor') as version_minor, ASSEMBLYPROPERTY(assemblies.name, 'VersionBuild') as version_build, ASSEMBLYPROPERTY(assemblies.name, 'VersionRevision') as version_revision, ASSEMBLYPROPERTY(assemblies.name, 'CultureInfo') as culture_info, ASSEMBLYPROPERTY(assemblies.name, 'PublicKey') as public_key, is_nullable, is_fixed_length, max_length from sys.assemblies as assemblies  join sys.assembly_types as types on assemblies.assembly_id = types.assembly_id ";
		DataRow dataRow = null;
		DataColumn dataColumn = dataTypesTable.Columns[DbMetaDataColumnNames.ProviderDbType];
		DataColumn dataColumn2 = dataTypesTable.Columns[DbMetaDataColumnNames.ColumnSize];
		DataColumn dataColumn3 = dataTypesTable.Columns[DbMetaDataColumnNames.IsFixedLength];
		DataColumn dataColumn4 = dataTypesTable.Columns[DbMetaDataColumnNames.IsSearchable];
		DataColumn dataColumn5 = dataTypesTable.Columns[DbMetaDataColumnNames.IsLiteralSupported];
		DataColumn dataColumn6 = dataTypesTable.Columns[DbMetaDataColumnNames.TypeName];
		DataColumn dataColumn7 = dataTypesTable.Columns[DbMetaDataColumnNames.IsNullable];
		if (dataColumn == null || dataColumn2 == null || dataColumn3 == null || dataColumn4 == null || dataColumn5 == null || dataColumn6 == null || dataColumn7 == null)
		{
			throw ADP.InvalidXml();
		}
		using IDataReader dataReader = sqlCommand.ExecuteReader();
		object[] array = new object[11];
		while (dataReader.Read())
		{
			dataReader.GetValues(array);
			dataRow = dataTypesTable.NewRow();
			dataRow[dataColumn] = SqlDbType.Udt;
			if (array[10] != DBNull.Value)
			{
				dataRow[dataColumn2] = array[10];
			}
			if (array[9] != DBNull.Value)
			{
				dataRow[dataColumn3] = array[9];
			}
			dataRow[dataColumn4] = true;
			dataRow[dataColumn5] = false;
			if (array[8] != DBNull.Value)
			{
				dataRow[dataColumn7] = array[8];
			}
			if (array[0] == DBNull.Value || array[1] == DBNull.Value || array[2] == DBNull.Value || array[3] == DBNull.Value || array[4] == DBNull.Value || array[5] == DBNull.Value)
			{
				continue;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(array[1].ToString());
			stringBuilder.Append(", ");
			stringBuilder.Append(array[0].ToString());
			stringBuilder.Append(", Version=");
			stringBuilder.Append(array[2].ToString());
			stringBuilder.Append(".");
			stringBuilder.Append(array[3].ToString());
			stringBuilder.Append(".");
			stringBuilder.Append(array[4].ToString());
			stringBuilder.Append(".");
			stringBuilder.Append(array[5].ToString());
			if (array[6] != DBNull.Value)
			{
				stringBuilder.Append(", Culture=");
				stringBuilder.Append(array[6].ToString());
			}
			if (array[7] != DBNull.Value)
			{
				stringBuilder.Append(", PublicKeyToken=");
				StringBuilder stringBuilder2 = new StringBuilder();
				byte[] array2 = (byte[])array[7];
				foreach (byte b in array2)
				{
					stringBuilder2.Append(string.Format(null, "{0,-2:x2}", b));
				}
				stringBuilder.Append(stringBuilder2.ToString());
			}
			dataRow[dataColumn6] = stringBuilder.ToString();
			dataTypesTable.Rows.Add(dataRow);
			dataRow.AcceptChanges();
		}
	}

	private void AddTVPsToDataTypesTable(DataTable dataTypesTable, SqlConnection connection, string ServerVersion)
	{
		if (0 > string.Compare(ServerVersion, "10.00.0000", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		SqlCommand sqlCommand = connection.CreateCommand();
		sqlCommand.CommandText = "select name, is_nullable, max_length from sys.types where is_table_type = 1";
		DataRow dataRow = null;
		DataColumn dataColumn = dataTypesTable.Columns[DbMetaDataColumnNames.ProviderDbType];
		DataColumn dataColumn2 = dataTypesTable.Columns[DbMetaDataColumnNames.ColumnSize];
		DataColumn dataColumn3 = dataTypesTable.Columns[DbMetaDataColumnNames.IsSearchable];
		DataColumn dataColumn4 = dataTypesTable.Columns[DbMetaDataColumnNames.IsLiteralSupported];
		DataColumn dataColumn5 = dataTypesTable.Columns[DbMetaDataColumnNames.TypeName];
		DataColumn dataColumn6 = dataTypesTable.Columns[DbMetaDataColumnNames.IsNullable];
		if (dataColumn == null || dataColumn2 == null || dataColumn3 == null || dataColumn4 == null || dataColumn5 == null || dataColumn6 == null)
		{
			throw ADP.InvalidXml();
		}
		using IDataReader dataReader = sqlCommand.ExecuteReader();
		object[] array = new object[11];
		while (dataReader.Read())
		{
			dataReader.GetValues(array);
			dataRow = dataTypesTable.NewRow();
			dataRow[dataColumn] = SqlDbType.Structured;
			if (array[2] != DBNull.Value)
			{
				dataRow[dataColumn2] = array[2];
			}
			dataRow[dataColumn3] = false;
			dataRow[dataColumn4] = false;
			if (array[1] != DBNull.Value)
			{
				dataRow[dataColumn6] = array[1];
			}
			if (array[0] != DBNull.Value)
			{
				dataRow[dataColumn5] = array[0];
				dataTypesTable.Rows.Add(dataRow);
				dataRow.AcceptChanges();
			}
		}
	}

	private DataTable GetDataTypesTable(SqlConnection connection)
	{
		DataTable dataTable = base.CollectionDataSet.Tables[DbMetaDataCollectionNames.DataTypes];
		if (dataTable == null)
		{
			throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.DataTypes);
		}
		dataTable = CloneAndFilterCollection(DbMetaDataCollectionNames.DataTypes, null);
		addUDTsToDataTypesTable(dataTable, connection, base.ServerVersionNormalized);
		AddTVPsToDataTypesTable(dataTable, connection, base.ServerVersionNormalized);
		dataTable.AcceptChanges();
		return dataTable;
	}

	protected override DataTable PrepareCollection(string collectionName, string[] restrictions, DbConnection connection)
	{
		SqlConnection connection2 = (SqlConnection)connection;
		DataTable dataTable = null;
		if (collectionName == DbMetaDataCollectionNames.DataTypes)
		{
			if (!ADP.IsEmptyArray(restrictions))
			{
				throw ADP.TooManyRestrictions(DbMetaDataCollectionNames.DataTypes);
			}
			dataTable = GetDataTypesTable(connection2);
		}
		if (dataTable == null)
		{
			throw ADP.UnableToBuildCollection(collectionName);
		}
		return dataTable;
	}
}
