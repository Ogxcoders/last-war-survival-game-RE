namespace System.Data.ProviderBase;

internal sealed class DbConnectionPoolGroupOptions
{
	private readonly bool _poolByIdentity;

	private readonly int _minPoolSize;

	private readonly int _maxPoolSize;

	private readonly int _creationTimeout;

	private readonly TimeSpan _loadBalanceTimeout;

	private readonly bool _hasTransactionAffinity;

	private readonly bool _useLoadBalancing;

	public int CreationTimeout => _creationTimeout;

	public bool HasTransactionAffinity => _hasTransactionAffinity;

	public TimeSpan LoadBalanceTimeout => _loadBalanceTimeout;

	public int MaxPoolSize => _maxPoolSize;

	public int MinPoolSize => _minPoolSize;

	public bool PoolByIdentity => _poolByIdentity;

	public bool UseLoadBalancing => _useLoadBalancing;

	public DbConnectionPoolGroupOptions(bool poolByIdentity, int minPoolSize, int maxPoolSize, int creationTimeout, int loadBalanceTimeout, bool hasTransactionAffinity)
	{
		_poolByIdentity = poolByIdentity;
		_minPoolSize = minPoolSize;
		_maxPoolSize = maxPoolSize;
		_creationTimeout = creationTimeout;
		if (loadBalanceTimeout != 0)
		{
			_loadBalanceTimeout = new TimeSpan(0, 0, loadBalanceTimeout);
			_useLoadBalancing = true;
		}
		_hasTransactionAffinity = hasTransactionAffinity;
	}
}
