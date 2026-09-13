namespace System.Runtime.Collections;

internal class ObjectCacheSettings
{
	private int cacheLimit;

	private TimeSpan idleTimeout;

	private TimeSpan leaseTimeout;

	private int purgeFrequency;

	private const int DefaultCacheLimit = 64;

	private const int DefaultPurgeFrequency = 32;

	private static TimeSpan DefaultIdleTimeout = TimeSpan.FromMinutes(2.0);

	private static TimeSpan DefaultLeaseTimeout = TimeSpan.FromMinutes(5.0);

	public int CacheLimit
	{
		get
		{
			return cacheLimit;
		}
		set
		{
			cacheLimit = value;
		}
	}

	public TimeSpan IdleTimeout
	{
		get
		{
			return idleTimeout;
		}
		set
		{
			idleTimeout = value;
		}
	}

	public TimeSpan LeaseTimeout
	{
		get
		{
			return leaseTimeout;
		}
		set
		{
			leaseTimeout = value;
		}
	}

	public int PurgeFrequency
	{
		get
		{
			return purgeFrequency;
		}
		set
		{
			purgeFrequency = value;
		}
	}

	public ObjectCacheSettings()
	{
		CacheLimit = 64;
		IdleTimeout = DefaultIdleTimeout;
		LeaseTimeout = DefaultLeaseTimeout;
		PurgeFrequency = 32;
	}

	private ObjectCacheSettings(ObjectCacheSettings other)
	{
		CacheLimit = other.CacheLimit;
		IdleTimeout = other.IdleTimeout;
		LeaseTimeout = other.LeaseTimeout;
		PurgeFrequency = other.PurgeFrequency;
	}

	internal ObjectCacheSettings Clone()
	{
		return new ObjectCacheSettings(this);
	}
}
