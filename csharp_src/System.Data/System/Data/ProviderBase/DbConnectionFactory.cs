using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.ProviderBase;

internal abstract class DbConnectionFactory
{
	private Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> _connectionPoolGroups;

	private readonly List<DbConnectionPool> _poolsToRelease;

	private readonly List<DbConnectionPoolGroup> _poolGroupsToRelease;

	private readonly Timer _pruningTimer;

	private const int PruningDueTime = 240000;

	private const int PruningPeriod = 30000;

	private static uint s_pendingOpenNonPooledNext = 0u;

	private static Task<DbConnectionInternal>[] s_pendingOpenNonPooled = new Task<DbConnectionInternal>[Environment.ProcessorCount];

	private static Task<DbConnectionInternal> s_completedTask;

	public abstract DbProviderFactory ProviderFactory { get; }

	protected DbConnectionFactory()
	{
		_connectionPoolGroups = new Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup>();
		_poolsToRelease = new List<DbConnectionPool>();
		_poolGroupsToRelease = new List<DbConnectionPoolGroup>();
		_pruningTimer = CreatePruningTimer();
	}

	public void ClearAllPools()
	{
		foreach (KeyValuePair<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroup in _connectionPoolGroups)
		{
			connectionPoolGroup.Value?.Clear();
		}
	}

	public void ClearPool(DbConnection connection)
	{
		ADP.CheckArgumentNull(connection, "connection");
		GetConnectionPoolGroup(connection)?.Clear();
	}

	public void ClearPool(DbConnectionPoolKey key)
	{
		ADP.CheckArgumentNull(key.ConnectionString, "key.ConnectionString");
		if (_connectionPoolGroups.TryGetValue(key, out var value))
		{
			value.Clear();
		}
	}

	internal virtual DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
	{
		return null;
	}

	internal DbConnectionInternal CreateNonPooledConnection(DbConnection owningConnection, DbConnectionPoolGroup poolGroup, DbConnectionOptions userOptions)
	{
		DbConnectionOptions connectionOptions = poolGroup.ConnectionOptions;
		DbConnectionPoolGroupProviderInfo providerInfo = poolGroup.ProviderInfo;
		DbConnectionPoolKey poolKey = poolGroup.PoolKey;
		DbConnectionInternal dbConnectionInternal = CreateConnection(connectionOptions, poolKey, providerInfo, null, owningConnection, userOptions);
		dbConnectionInternal?.MakeNonPooledObject(owningConnection);
		return dbConnectionInternal;
	}

	internal DbConnectionInternal CreatePooledConnection(DbConnectionPool pool, DbConnection owningObject, DbConnectionOptions options, DbConnectionPoolKey poolKey, DbConnectionOptions userOptions)
	{
		DbConnectionPoolGroupProviderInfo providerInfo = pool.PoolGroup.ProviderInfo;
		DbConnectionInternal dbConnectionInternal = CreateConnection(options, poolKey, providerInfo, pool, owningObject, userOptions);
		dbConnectionInternal?.MakePooledConnection(pool);
		return dbConnectionInternal;
	}

	internal virtual DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
	{
		return null;
	}

	private Timer CreatePruningTimer()
	{
		return ADP.UnsafeCreateTimer(PruneConnectionPoolGroups, null, 240000, 30000);
	}

	protected DbConnectionOptions FindConnectionOptions(DbConnectionPoolKey key)
	{
		if (!string.IsNullOrEmpty(key.ConnectionString) && _connectionPoolGroups.TryGetValue(key, out var value))
		{
			return value.ConnectionOptions;
		}
		return null;
	}

	private static Task<DbConnectionInternal> GetCompletedTask()
	{
		return s_completedTask ?? (s_completedTask = Task.FromResult<DbConnectionInternal>(null));
	}

	private DbConnectionPool GetConnectionPool(DbConnection owningObject, DbConnectionPoolGroup connectionPoolGroup)
	{
		if (connectionPoolGroup.IsDisabled && connectionPoolGroup.PoolGroupOptions != null)
		{
			DbConnectionPoolGroupOptions poolGroupOptions = connectionPoolGroup.PoolGroupOptions;
			DbConnectionOptions userConnectionOptions = connectionPoolGroup.ConnectionOptions;
			connectionPoolGroup = GetConnectionPoolGroup(connectionPoolGroup.PoolKey, poolGroupOptions, ref userConnectionOptions);
			SetConnectionPoolGroup(owningObject, connectionPoolGroup);
		}
		return connectionPoolGroup.GetConnectionPool(this);
	}

	internal DbConnectionPoolGroup GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, ref DbConnectionOptions userConnectionOptions)
	{
		if (string.IsNullOrEmpty(key.ConnectionString))
		{
			return null;
		}
		Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
		if (!connectionPoolGroups.TryGetValue(key, out var value) || (value.IsDisabled && value.PoolGroupOptions != null))
		{
			DbConnectionOptions dbConnectionOptions = CreateConnectionOptions(key.ConnectionString, userConnectionOptions);
			if (dbConnectionOptions == null)
			{
				throw ADP.InternalConnectionError(ADP.ConnectionError.ConnectionOptionsMissing);
			}
			if (userConnectionOptions == null)
			{
				userConnectionOptions = dbConnectionOptions;
			}
			if (poolOptions == null)
			{
				poolOptions = ((value == null) ? CreateConnectionPoolGroupOptions(dbConnectionOptions) : value.PoolGroupOptions);
			}
			lock (this)
			{
				connectionPoolGroups = _connectionPoolGroups;
				if (!connectionPoolGroups.TryGetValue(key, out value))
				{
					DbConnectionPoolGroup dbConnectionPoolGroup = new DbConnectionPoolGroup(dbConnectionOptions, key, poolOptions);
					dbConnectionPoolGroup.ProviderInfo = CreateConnectionPoolGroupProviderInfo(dbConnectionOptions);
					Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> dictionary = new Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup>(1 + connectionPoolGroups.Count);
					foreach (KeyValuePair<DbConnectionPoolKey, DbConnectionPoolGroup> item in connectionPoolGroups)
					{
						dictionary.Add(item.Key, item.Value);
					}
					dictionary.Add(key, dbConnectionPoolGroup);
					value = dbConnectionPoolGroup;
					_connectionPoolGroups = dictionary;
				}
			}
		}
		else if (userConnectionOptions == null)
		{
			userConnectionOptions = value.ConnectionOptions;
		}
		return value;
	}

	private void PruneConnectionPoolGroups(object state)
	{
		lock (_poolsToRelease)
		{
			if (_poolsToRelease.Count != 0)
			{
				DbConnectionPool[] array = _poolsToRelease.ToArray();
				foreach (DbConnectionPool dbConnectionPool in array)
				{
					if (dbConnectionPool != null)
					{
						dbConnectionPool.Clear();
						if (dbConnectionPool.Count == 0)
						{
							_poolsToRelease.Remove(dbConnectionPool);
						}
					}
				}
			}
		}
		lock (_poolGroupsToRelease)
		{
			if (_poolGroupsToRelease.Count != 0)
			{
				DbConnectionPoolGroup[] array2 = _poolGroupsToRelease.ToArray();
				foreach (DbConnectionPoolGroup dbConnectionPoolGroup in array2)
				{
					if (dbConnectionPoolGroup != null && dbConnectionPoolGroup.Clear() == 0)
					{
						_poolGroupsToRelease.Remove(dbConnectionPoolGroup);
					}
				}
			}
		}
		lock (this)
		{
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> dictionary = new Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup>(connectionPoolGroups.Count);
			foreach (KeyValuePair<DbConnectionPoolKey, DbConnectionPoolGroup> item in connectionPoolGroups)
			{
				if (item.Value != null)
				{
					if (item.Value.Prune())
					{
						QueuePoolGroupForRelease(item.Value);
					}
					else
					{
						dictionary.Add(item.Key, item.Value);
					}
				}
			}
			_connectionPoolGroups = dictionary;
		}
	}

	internal void QueuePoolForRelease(DbConnectionPool pool, bool clearing)
	{
		pool.Shutdown();
		lock (_poolsToRelease)
		{
			if (clearing)
			{
				pool.Clear();
			}
			_poolsToRelease.Add(pool);
		}
	}

	internal void QueuePoolGroupForRelease(DbConnectionPoolGroup poolGroup)
	{
		lock (_poolGroupsToRelease)
		{
			_poolGroupsToRelease.Add(poolGroup);
		}
	}

	protected virtual DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions userOptions)
	{
		return CreateConnection(options, poolKey, poolGroupProviderInfo, pool, owningConnection);
	}

	internal DbMetaDataFactory GetMetaDataFactory(DbConnectionPoolGroup connectionPoolGroup, DbConnectionInternal internalConnection)
	{
		DbMetaDataFactory dbMetaDataFactory = connectionPoolGroup.MetaDataFactory;
		if (dbMetaDataFactory == null)
		{
			bool cacheMetaDataFactory = false;
			dbMetaDataFactory = CreateMetaDataFactory(internalConnection, out cacheMetaDataFactory);
			if (cacheMetaDataFactory)
			{
				connectionPoolGroup.MetaDataFactory = dbMetaDataFactory;
			}
		}
		return dbMetaDataFactory;
	}

	protected virtual DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
	{
		cacheMetaDataFactory = false;
		throw ADP.NotSupported();
	}

	protected abstract DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection);

	protected abstract DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous);

	protected abstract DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions options);

	internal abstract DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection);

	internal abstract DbConnectionInternal GetInnerConnection(DbConnection connection);

	internal abstract void PermissionDemand(DbConnection outerConnection);

	internal abstract void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup);

	internal abstract void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to);

	internal abstract bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from);

	internal abstract void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to);

	internal bool TryGetConnection(DbConnection owningConnection, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions, DbConnectionInternal oldConnection, out DbConnectionInternal connection)
	{
		connection = null;
		int num = 10;
		int num2 = 1;
		do
		{
			DbConnectionPoolGroup poolGroup = GetConnectionPoolGroup(owningConnection);
			DbConnectionPool connectionPool = GetConnectionPool(owningConnection, poolGroup);
			if (connectionPool == null)
			{
				poolGroup = GetConnectionPoolGroup(owningConnection);
				if (retry != null)
				{
					CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
					Task<DbConnectionInternal> task2;
					lock (s_pendingOpenNonPooled)
					{
						int i;
						for (i = 0; i < s_pendingOpenNonPooled.Length; i++)
						{
							Task task = s_pendingOpenNonPooled[i];
							if (task == null)
							{
								s_pendingOpenNonPooled[i] = GetCompletedTask();
								break;
							}
							if (task.IsCompleted)
							{
								break;
							}
						}
						if (i == s_pendingOpenNonPooled.Length)
						{
							i = (int)(s_pendingOpenNonPooledNext % s_pendingOpenNonPooled.Length);
							s_pendingOpenNonPooledNext++;
						}
						task2 = s_pendingOpenNonPooled[i].ContinueWith(delegate
						{
							Transaction currentTransaction = ADP.GetCurrentTransaction();
							try
							{
								ADP.SetCurrentTransaction(retry.Task.AsyncState as Transaction);
								DbConnectionInternal result = CreateNonPooledConnection(owningConnection, poolGroup, userOptions);
								if (oldConnection != null && oldConnection.State == ConnectionState.Open)
								{
									oldConnection.PrepareForReplaceConnection();
									oldConnection.Dispose();
								}
								return result;
							}
							finally
							{
								ADP.SetCurrentTransaction(currentTransaction);
							}
						}, cancellationTokenSource.Token, TaskContinuationOptions.LongRunning, TaskScheduler.Default);
						s_pendingOpenNonPooled[i] = task2;
					}
					if (owningConnection.ConnectionTimeout > 0)
					{
						int millisecondsDelay = owningConnection.ConnectionTimeout * 1000;
						cancellationTokenSource.CancelAfter(millisecondsDelay);
					}
					task2.ContinueWith(delegate(Task<DbConnectionInternal> task3)
					{
						cancellationTokenSource.Dispose();
						if (task3.IsCanceled)
						{
							retry.TrySetException(ADP.ExceptionWithStackTrace(ADP.NonPooledOpenTimeout()));
						}
						else if (task3.IsFaulted)
						{
							retry.TrySetException(task3.Exception.InnerException);
						}
						else if (!retry.TrySetResult(task3.Result))
						{
							task3.Result.DoomThisConnection();
							task3.Result.Dispose();
						}
					}, TaskScheduler.Default);
					return false;
				}
				connection = CreateNonPooledConnection(owningConnection, poolGroup, userOptions);
				continue;
			}
			if (((SqlConnection)owningConnection).ForceNewConnection)
			{
				connection = connectionPool.ReplaceConnection(owningConnection, userOptions, oldConnection);
			}
			else if (!connectionPool.TryGetConnection(owningConnection, retry, userOptions, out connection))
			{
				return false;
			}
			if (connection == null)
			{
				if (connectionPool.IsRunning)
				{
					throw ADP.PooledOpenTimeout();
				}
				Thread.Sleep(num2);
				num2 *= 2;
			}
		}
		while (connection == null && num-- > 0);
		if (connection == null)
		{
			throw ADP.PooledOpenTimeout();
		}
		return true;
	}
}
