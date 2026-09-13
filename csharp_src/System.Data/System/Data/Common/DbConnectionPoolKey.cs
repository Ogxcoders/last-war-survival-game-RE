namespace System.Data.Common;

internal class DbConnectionPoolKey : ICloneable
{
	private string _connectionString;

	internal virtual string ConnectionString
	{
		get
		{
			return _connectionString;
		}
		set
		{
			_connectionString = value;
		}
	}

	internal DbConnectionPoolKey(string connectionString)
	{
		_connectionString = connectionString;
	}

	protected DbConnectionPoolKey(DbConnectionPoolKey key)
	{
		_connectionString = key.ConnectionString;
	}

	public virtual object Clone()
	{
		return new DbConnectionPoolKey(this);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || obj.GetType() != typeof(DbConnectionPoolKey))
		{
			return false;
		}
		if (obj is DbConnectionPoolKey dbConnectionPoolKey)
		{
			return _connectionString == dbConnectionPoolKey._connectionString;
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (_connectionString != null)
		{
			return _connectionString.GetHashCode();
		}
		return 0;
	}
}
