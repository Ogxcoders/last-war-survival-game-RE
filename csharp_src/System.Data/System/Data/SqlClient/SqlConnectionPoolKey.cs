using System.Data.Common;

namespace System.Data.SqlClient;

internal class SqlConnectionPoolKey : DbConnectionPoolKey
{
	private int _hashValue;

	private SqlCredential _credential;

	private readonly string _accessToken;

	internal override string ConnectionString
	{
		get
		{
			return base.ConnectionString;
		}
		set
		{
			base.ConnectionString = value;
			CalculateHashCode();
		}
	}

	internal SqlCredential Credential => _credential;

	internal string AccessToken => _accessToken;

	internal SqlConnectionPoolKey(string connectionString, SqlCredential credential, string accessToken)
		: base(connectionString)
	{
		_credential = credential;
		_accessToken = accessToken;
		CalculateHashCode();
	}

	private SqlConnectionPoolKey(SqlConnectionPoolKey key)
		: base(key)
	{
		_credential = key.Credential;
		_accessToken = key.AccessToken;
		CalculateHashCode();
	}

	public override object Clone()
	{
		return new SqlConnectionPoolKey(this);
	}

	public override bool Equals(object obj)
	{
		if (obj is SqlConnectionPoolKey sqlConnectionPoolKey && _credential == sqlConnectionPoolKey._credential && ConnectionString == sqlConnectionPoolKey.ConnectionString)
		{
			return (object)_accessToken == sqlConnectionPoolKey._accessToken;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _hashValue;
	}

	private void CalculateHashCode()
	{
		_hashValue = base.GetHashCode();
		if (_credential != null)
		{
			_hashValue = _hashValue * 17 + _credential.GetHashCode();
		}
		else if (_accessToken != null)
		{
			_hashValue = _hashValue * 17 + _accessToken.GetHashCode();
		}
	}
}
