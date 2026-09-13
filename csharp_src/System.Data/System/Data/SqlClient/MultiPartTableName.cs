using System.Data.Common;

namespace System.Data.SqlClient;

internal struct MultiPartTableName
{
	private string _multipartName;

	private string _serverName;

	private string _catalogName;

	private string _schemaName;

	private string _tableName;

	internal static readonly MultiPartTableName Null = new MultiPartTableName(new string[4]);

	internal string ServerName
	{
		get
		{
			ParseMultipartName();
			return _serverName;
		}
		set
		{
			_serverName = value;
		}
	}

	internal string CatalogName
	{
		get
		{
			ParseMultipartName();
			return _catalogName;
		}
		set
		{
			_catalogName = value;
		}
	}

	internal string SchemaName
	{
		get
		{
			ParseMultipartName();
			return _schemaName;
		}
		set
		{
			_schemaName = value;
		}
	}

	internal string TableName
	{
		get
		{
			ParseMultipartName();
			return _tableName;
		}
		set
		{
			_tableName = value;
		}
	}

	internal MultiPartTableName(string[] parts)
	{
		_multipartName = null;
		_serverName = parts[0];
		_catalogName = parts[1];
		_schemaName = parts[2];
		_tableName = parts[3];
	}

	internal MultiPartTableName(string multipartName)
	{
		_multipartName = multipartName;
		_serverName = null;
		_catalogName = null;
		_schemaName = null;
		_tableName = null;
	}

	private void ParseMultipartName()
	{
		if (_multipartName != null)
		{
			string[] array = MultipartIdentifier.ParseMultipartIdentifier(_multipartName, "[\"", "]\"", "Processing of results from SQL Server failed because of an invalid multipart name", ThrowOnEmptyMultipartName: false);
			_serverName = array[0];
			_catalogName = array[1];
			_schemaName = array[2];
			_tableName = array[3];
			_multipartName = null;
		}
	}
}
