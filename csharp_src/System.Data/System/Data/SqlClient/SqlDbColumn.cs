using System.Data.Common;

namespace System.Data.SqlClient;

internal class SqlDbColumn : DbColumn
{
	private readonly _SqlMetaData _metadata;

	internal bool? SqlIsAliased
	{
		set
		{
			base.IsAliased = value;
		}
	}

	internal bool? SqlIsKey
	{
		set
		{
			base.IsKey = value;
		}
	}

	internal bool? SqlIsHidden
	{
		set
		{
			base.IsHidden = value;
		}
	}

	internal bool? SqlIsExpression
	{
		set
		{
			base.IsExpression = value;
		}
	}

	internal Type SqlDataType
	{
		set
		{
			base.DataType = value;
		}
	}

	internal string SqlDataTypeName
	{
		set
		{
			base.DataTypeName = value;
		}
	}

	internal int? SqlNumericScale
	{
		set
		{
			base.NumericScale = value;
		}
	}

	internal SqlDbColumn(_SqlMetaData md)
	{
		_metadata = md;
		Populate();
	}

	private void Populate()
	{
		base.AllowDBNull = _metadata.isNullable;
		base.BaseCatalogName = _metadata.catalogName;
		base.BaseColumnName = _metadata.baseColumn;
		base.BaseSchemaName = _metadata.schemaName;
		base.BaseServerName = _metadata.serverName;
		base.BaseTableName = _metadata.tableName;
		base.ColumnName = _metadata.column;
		base.ColumnOrdinal = _metadata.ordinal;
		base.ColumnSize = ((_metadata.metaType.IsSizeInCharacters && _metadata.length != int.MaxValue) ? (_metadata.length / 2) : _metadata.length);
		base.IsAutoIncrement = _metadata.isIdentity;
		base.IsIdentity = _metadata.isIdentity;
		base.IsLong = _metadata.metaType.IsLong;
		if (SqlDbType.Timestamp == _metadata.type)
		{
			base.IsUnique = true;
		}
		else
		{
			base.IsUnique = false;
		}
		if (byte.MaxValue != _metadata.precision)
		{
			base.NumericPrecision = _metadata.precision;
		}
		else
		{
			base.NumericPrecision = _metadata.metaType.Precision;
		}
		base.IsReadOnly = _metadata.updatability == 0;
		base.UdtAssemblyQualifiedName = _metadata.udtAssemblyQualifiedName;
	}
}
