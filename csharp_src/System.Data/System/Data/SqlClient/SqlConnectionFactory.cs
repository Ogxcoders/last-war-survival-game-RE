using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Reflection;

namespace System.Data.SqlClient;

internal sealed class SqlConnectionFactory : DbConnectionFactory
{
	private const string _metaDataXml = "MetaDataXml";

	public static readonly SqlConnectionFactory SingletonInstance = new SqlConnectionFactory();

	public override DbProviderFactory ProviderFactory => SqlClientFactory.Instance;

	private SqlConnectionFactory()
	{
	}

	protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection)
	{
		return CreateConnection(options, poolKey, poolGroupProviderInfo, pool, owningConnection, null);
	}

	protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions userOptions)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)options;
		SqlConnectionPoolKey sqlConnectionPoolKey = (SqlConnectionPoolKey)poolKey;
		SessionData reconnectSessionData = null;
		SqlConnection sqlConnection = (SqlConnection)owningConnection;
		bool applyTransientFaultHandling = sqlConnection?._applyTransientFaultHandling ?? false;
		SqlConnectionString userConnectionOptions = null;
		if (userOptions != null)
		{
			userConnectionOptions = (SqlConnectionString)userOptions;
		}
		else if (sqlConnection != null)
		{
			userConnectionOptions = (SqlConnectionString)sqlConnection.UserConnectionOptions;
		}
		if (sqlConnection != null)
		{
			reconnectSessionData = sqlConnection._recoverySessionData;
		}
		bool redirectedUserInstance = false;
		DbConnectionPoolIdentity identity = null;
		if (sqlConnectionString.IntegratedSecurity)
		{
			identity = ((pool == null) ? DbConnectionPoolIdentity.GetCurrent() : pool.Identity);
		}
		if (sqlConnectionString.UserInstance)
		{
			redirectedUserInstance = true;
			string instanceName;
			if (pool == null || (pool != null && pool.Count <= 0))
			{
				SqlInternalConnectionTds sqlInternalConnectionTds = null;
				try
				{
					SqlConnectionString connectionOptions = new SqlConnectionString(sqlConnectionString, sqlConnectionString.DataSource, userInstance: true, false);
					sqlInternalConnectionTds = new SqlInternalConnectionTds(identity, connectionOptions, sqlConnectionPoolKey.Credential, null, "", null, redirectedUserInstance: false, null, null, applyTransientFaultHandling);
					instanceName = sqlInternalConnectionTds.InstanceName;
					if (!instanceName.StartsWith("\\\\.\\", StringComparison.Ordinal))
					{
						throw SQL.NonLocalSSEInstance();
					}
					if (pool != null)
					{
						((SqlConnectionPoolProviderInfo)pool.ProviderInfo).InstanceName = instanceName;
					}
				}
				finally
				{
					sqlInternalConnectionTds?.Dispose();
				}
			}
			else
			{
				instanceName = ((SqlConnectionPoolProviderInfo)pool.ProviderInfo).InstanceName;
			}
			sqlConnectionString = new SqlConnectionString(sqlConnectionString, instanceName, userInstance: false, null);
			poolGroupProviderInfo = null;
		}
		return new SqlInternalConnectionTds(identity, sqlConnectionString, sqlConnectionPoolKey.Credential, poolGroupProviderInfo, "", null, redirectedUserInstance, userConnectionOptions, reconnectSessionData, applyTransientFaultHandling, sqlConnectionPoolKey.AccessToken);
	}

	protected override DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous)
	{
		return new SqlConnectionString(connectionString);
	}

	internal override DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
	{
		DbConnectionPoolProviderInfo result = null;
		if (((SqlConnectionString)connectionOptions).UserInstance)
		{
			result = new SqlConnectionPoolProviderInfo();
		}
		return result;
	}

	protected override DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions connectionOptions)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)connectionOptions;
		DbConnectionPoolGroupOptions result = null;
		if (sqlConnectionString.Pooling)
		{
			int num = sqlConnectionString.ConnectTimeout;
			if (0 < num && num < 2147483)
			{
				num *= 1000;
			}
			else if (num >= 2147483)
			{
				num = int.MaxValue;
			}
			result = new DbConnectionPoolGroupOptions(sqlConnectionString.IntegratedSecurity, sqlConnectionString.MinPoolSize, sqlConnectionString.MaxPoolSize, num, sqlConnectionString.LoadBalanceTimeout, sqlConnectionString.Enlist);
		}
		return result;
	}

	internal override DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
	{
		return new SqlConnectionPoolGroupProviderInfo((SqlConnectionString)connectionOptions);
	}

	internal static SqlConnectionString FindSqlConnectionOptions(SqlConnectionPoolKey key)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)SingletonInstance.FindConnectionOptions(key);
		if (sqlConnectionString == null)
		{
			sqlConnectionString = new SqlConnectionString(key.ConnectionString);
		}
		if (sqlConnectionString.IsEmpty)
		{
			throw ADP.NoConnectionString();
		}
		return sqlConnectionString;
	}

	internal override DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection)
	{
		if (connection is SqlConnection sqlConnection)
		{
			return sqlConnection.PoolGroup;
		}
		return null;
	}

	internal override DbConnectionInternal GetInnerConnection(DbConnection connection)
	{
		if (connection is SqlConnection sqlConnection)
		{
			return sqlConnection.InnerConnection;
		}
		return null;
	}

	internal override void PermissionDemand(DbConnection outerConnection)
	{
		if (outerConnection is SqlConnection sqlConnection)
		{
			sqlConnection.PermissionDemand();
		}
	}

	internal override void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup)
	{
		if (outerConnection is SqlConnection sqlConnection)
		{
			sqlConnection.PoolGroup = poolGroup;
		}
	}

	internal override void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to)
	{
		if (owningObject is SqlConnection sqlConnection)
		{
			sqlConnection.SetInnerConnectionEvent(to);
		}
	}

	internal override bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from)
	{
		if (owningObject is SqlConnection sqlConnection)
		{
			return sqlConnection.SetInnerConnectionFrom(to, from);
		}
		return false;
	}

	internal override void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to)
	{
		if (owningObject is SqlConnection sqlConnection)
		{
			sqlConnection.SetInnerConnectionTo(to);
		}
	}

	protected override DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
	{
		Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("System.Data.SqlClient.SqlMetaData.xml");
		cacheMetaDataFactory = true;
		return new SqlMetaDataFactory(manifestResourceStream, internalConnection.ServerVersion, internalConnection.ServerVersion);
	}
}
