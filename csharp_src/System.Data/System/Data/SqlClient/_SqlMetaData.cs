namespace System.Data.SqlClient;

internal sealed class _SqlMetaData : SqlMetaDataPriv
{
	internal string column;

	internal string baseColumn;

	internal MultiPartTableName multiPartTableName;

	internal readonly int ordinal;

	internal byte updatability;

	internal byte tableNum;

	internal bool isDifferentName;

	internal bool isKey;

	internal bool isHidden;

	internal bool isExpression;

	internal bool isIdentity;

	internal bool isColumnSet;

	internal byte op;

	internal ushort operand;

	internal string serverName => multiPartTableName.ServerName;

	internal string catalogName => multiPartTableName.CatalogName;

	internal string schemaName => multiPartTableName.SchemaName;

	internal string tableName => multiPartTableName.TableName;

	internal bool IsNewKatmaiDateTimeType
	{
		get
		{
			if (SqlDbType.Date != type && SqlDbType.Time != type && SqlDbType.DateTime2 != type)
			{
				return SqlDbType.DateTimeOffset == type;
			}
			return true;
		}
	}

	internal bool IsLargeUdt
	{
		get
		{
			if (type == SqlDbType.Udt)
			{
				return length == int.MaxValue;
			}
			return false;
		}
	}

	internal _SqlMetaData(int ordinal)
	{
		this.ordinal = ordinal;
	}

	public object Clone()
	{
		_SqlMetaData sqlMetaData = new _SqlMetaData(ordinal);
		sqlMetaData.CopyFrom(this);
		sqlMetaData.column = column;
		sqlMetaData.baseColumn = baseColumn;
		sqlMetaData.multiPartTableName = multiPartTableName;
		sqlMetaData.updatability = updatability;
		sqlMetaData.tableNum = tableNum;
		sqlMetaData.isDifferentName = isDifferentName;
		sqlMetaData.isKey = isKey;
		sqlMetaData.isHidden = isHidden;
		sqlMetaData.isExpression = isExpression;
		sqlMetaData.isIdentity = isIdentity;
		sqlMetaData.isColumnSet = isColumnSet;
		sqlMetaData.op = op;
		sqlMetaData.operand = operand;
		return sqlMetaData;
	}
}
