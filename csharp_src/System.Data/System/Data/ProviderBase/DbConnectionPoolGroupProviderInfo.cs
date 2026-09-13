namespace System.Data.ProviderBase;

internal class DbConnectionPoolGroupProviderInfo
{
	private DbConnectionPoolGroup _poolGroup;

	internal DbConnectionPoolGroup PoolGroup
	{
		get
		{
			return _poolGroup;
		}
		set
		{
			_poolGroup = value;
		}
	}
}
