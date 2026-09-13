using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;

namespace System.Data.ProviderBase;

internal sealed class DbConnectionPoolGroup
{
	private readonly DbConnectionOptions _connectionOptions;

	private readonly DbConnectionPoolKey _poolKey;

	private readonly DbConnectionPoolGroupOptions _poolGroupOptions;

	private ConcurrentDictionary<DbConnectionPoolIdentity, DbConnectionPool> _poolCollection;

	private int _state;

	private DbConnectionPoolGroupProviderInfo _providerInfo;

	private DbMetaDataFactory _metaDataFactory;

	private const int PoolGroupStateActive = 1;

	private const int PoolGroupStateIdle = 2;

	private const int PoolGroupStateDisabled = 4;

	internal DbConnectionOptions ConnectionOptions => _connectionOptions;

	internal DbConnectionPoolKey PoolKey => _poolKey;

	internal DbConnectionPoolGroupProviderInfo ProviderInfo
	{
		get
		{
			return _providerInfo;
		}
		set
		{
			_providerInfo = value;
			if (value != null)
			{
				_providerInfo.PoolGroup = this;
			}
		}
	}

	internal bool IsDisabled => 4 == _state;

	internal DbConnectionPoolGroupOptions PoolGroupOptions => _poolGroupOptions;

	internal DbMetaDataFactory MetaDataFactory
	{
		get
		{
			return _metaDataFactory;
		}
		set
		{
			_metaDataFactory = value;
		}
	}

	internal DbConnectionPoolGroup(DbConnectionOptions connectionOptions, DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolGroupOptions)
	{
		_connectionOptions = connectionOptions;
		_poolKey = key;
		_poolGroupOptions = poolGroupOptions;
		_poolCollection = new ConcurrentDictionary<DbConnectionPoolIdentity, DbConnectionPool>();
		_state = 1;
	}

	internal int Clear()
	{
		ConcurrentDictionary<DbConnectionPoolIdentity, DbConnectionPool> concurrentDictionary = null;
		lock (this)
		{
			if (_poolCollection.Count > 0)
			{
				concurrentDictionary = _poolCollection;
				_poolCollection = new ConcurrentDictionary<DbConnectionPoolIdentity, DbConnectionPool>();
			}
		}
		if (concurrentDictionary != null)
		{
			foreach (KeyValuePair<DbConnectionPoolIdentity, DbConnectionPool> item in concurrentDictionary)
			{
				DbConnectionPool value = item.Value;
				value?.ConnectionFactory.QueuePoolForRelease(value, clearing: true);
			}
		}
		return _poolCollection.Count;
	}

	internal DbConnectionPool GetConnectionPool(DbConnectionFactory connectionFactory)
	{
		DbConnectionPool value = null;
		if (_poolGroupOptions != null)
		{
			DbConnectionPoolIdentity dbConnectionPoolIdentity = DbConnectionPoolIdentity.NoIdentity;
			if (_poolGroupOptions.PoolByIdentity)
			{
				dbConnectionPoolIdentity = DbConnectionPoolIdentity.GetCurrent();
				if (dbConnectionPoolIdentity.IsRestricted)
				{
					dbConnectionPoolIdentity = null;
				}
			}
			if (dbConnectionPoolIdentity != null && !_poolCollection.TryGetValue(dbConnectionPoolIdentity, out value))
			{
				lock (this)
				{
					if (!_poolCollection.TryGetValue(dbConnectionPoolIdentity, out value))
					{
						DbConnectionPoolProviderInfo connectionPoolProviderInfo = connectionFactory.CreateConnectionPoolProviderInfo(ConnectionOptions);
						DbConnectionPool dbConnectionPool = new DbConnectionPool(connectionFactory, this, dbConnectionPoolIdentity, connectionPoolProviderInfo);
						if (MarkPoolGroupAsActive())
						{
							dbConnectionPool.Startup();
							_poolCollection.TryAdd(dbConnectionPoolIdentity, dbConnectionPool);
							value = dbConnectionPool;
						}
						else
						{
							dbConnectionPool.Shutdown();
						}
					}
				}
			}
		}
		if (value == null)
		{
			lock (this)
			{
				MarkPoolGroupAsActive();
			}
		}
		return value;
	}

	private bool MarkPoolGroupAsActive()
	{
		if (2 == _state)
		{
			_state = 1;
		}
		return 1 == _state;
	}

	internal bool Prune()
	{
		lock (this)
		{
			if (_poolCollection.Count > 0)
			{
				ConcurrentDictionary<DbConnectionPoolIdentity, DbConnectionPool> concurrentDictionary = new ConcurrentDictionary<DbConnectionPoolIdentity, DbConnectionPool>();
				foreach (KeyValuePair<DbConnectionPoolIdentity, DbConnectionPool> item in _poolCollection)
				{
					DbConnectionPool value = item.Value;
					if (value != null)
					{
						if (!value.ErrorOccurred && value.Count == 0)
						{
							value.ConnectionFactory.QueuePoolForRelease(value, clearing: false);
						}
						else
						{
							concurrentDictionary.TryAdd(item.Key, item.Value);
						}
					}
				}
				_poolCollection = concurrentDictionary;
			}
			if (_poolCollection.Count == 0)
			{
				if (1 == _state)
				{
					_state = 2;
				}
				else if (2 == _state)
				{
					_state = 4;
				}
			}
			return 4 == _state;
		}
	}
}
