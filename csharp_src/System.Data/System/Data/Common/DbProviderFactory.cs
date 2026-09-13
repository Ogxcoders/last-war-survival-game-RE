using System.Security;
using System.Security.Permissions;

namespace System.Data.Common;

public abstract class DbProviderFactory
{
	private bool? _canCreateDataAdapter;

	private bool? _canCreateCommandBuilder;

	public virtual bool CanCreateDataSourceEnumerator => false;

	public virtual bool CanCreateDataAdapter
	{
		get
		{
			if (!_canCreateDataAdapter.HasValue)
			{
				using DbDataAdapter dbDataAdapter = CreateDataAdapter();
				_canCreateDataAdapter = dbDataAdapter != null;
			}
			return _canCreateDataAdapter.Value;
		}
	}

	public virtual bool CanCreateCommandBuilder
	{
		get
		{
			if (!_canCreateCommandBuilder.HasValue)
			{
				using DbCommandBuilder dbCommandBuilder = CreateCommandBuilder();
				_canCreateCommandBuilder = dbCommandBuilder != null;
			}
			return _canCreateCommandBuilder.Value;
		}
	}

	public virtual CodeAccessPermission CreatePermission(PermissionState state)
	{
		return null;
	}

	public virtual DbCommand CreateCommand()
	{
		return null;
	}

	public virtual DbCommandBuilder CreateCommandBuilder()
	{
		return null;
	}

	public virtual DbConnection CreateConnection()
	{
		return null;
	}

	public virtual DbConnectionStringBuilder CreateConnectionStringBuilder()
	{
		return null;
	}

	public virtual DbDataAdapter CreateDataAdapter()
	{
		return null;
	}

	public virtual DbParameter CreateParameter()
	{
		return null;
	}

	public virtual DbDataSourceEnumerator CreateDataSourceEnumerator()
	{
		return null;
	}
}
