using System.Globalization;

namespace System.Net.Cache;

public class HttpRequestCachePolicy : RequestCachePolicy
{
	internal static readonly HttpRequestCachePolicy BypassCache = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);

	private HttpRequestCacheLevel m_Level;

	private DateTime m_LastSyncDateUtc = DateTime.MinValue;

	private TimeSpan m_MaxAge = TimeSpan.MaxValue;

	private TimeSpan m_MinFresh = TimeSpan.MinValue;

	private TimeSpan m_MaxStale = TimeSpan.MinValue;

	public new HttpRequestCacheLevel Level => m_Level;

	public DateTime CacheSyncDate
	{
		get
		{
			if (m_LastSyncDateUtc == DateTime.MinValue || m_LastSyncDateUtc == DateTime.MaxValue)
			{
				return m_LastSyncDateUtc;
			}
			return m_LastSyncDateUtc.ToLocalTime();
		}
	}

	internal DateTime InternalCacheSyncDateUtc => m_LastSyncDateUtc;

	public TimeSpan MaxAge => m_MaxAge;

	public TimeSpan MinFresh => m_MinFresh;

	public TimeSpan MaxStale => m_MaxStale;

	public HttpRequestCachePolicy()
		: this(HttpRequestCacheLevel.Default)
	{
	}

	public HttpRequestCachePolicy(HttpRequestCacheLevel level)
		: base(MapLevel(level))
	{
		m_Level = level;
	}

	public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan ageOrFreshOrStale)
		: this(HttpRequestCacheLevel.Default)
	{
		switch (cacheAgeControl)
		{
		case HttpCacheAgeControl.MinFresh:
			m_MinFresh = ageOrFreshOrStale;
			break;
		case HttpCacheAgeControl.MaxAge:
			m_MaxAge = ageOrFreshOrStale;
			break;
		case HttpCacheAgeControl.MaxStale:
			m_MaxStale = ageOrFreshOrStale;
			break;
		default:
			throw new ArgumentException(global::SR.GetString("The specified value is not valid in the '{0}' enumeration.", "HttpCacheAgeControl"), "cacheAgeControl");
		}
	}

	public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale)
		: this(HttpRequestCacheLevel.Default)
	{
		switch (cacheAgeControl)
		{
		case HttpCacheAgeControl.MinFresh:
			m_MinFresh = freshOrStale;
			break;
		case HttpCacheAgeControl.MaxAge:
			m_MaxAge = maxAge;
			break;
		case HttpCacheAgeControl.MaxStale:
			m_MaxStale = freshOrStale;
			break;
		case HttpCacheAgeControl.MaxAgeAndMinFresh:
			m_MaxAge = maxAge;
			m_MinFresh = freshOrStale;
			break;
		case HttpCacheAgeControl.MaxAgeAndMaxStale:
			m_MaxAge = maxAge;
			m_MaxStale = freshOrStale;
			break;
		default:
			throw new ArgumentException(global::SR.GetString("The specified value is not valid in the '{0}' enumeration.", "HttpCacheAgeControl"), "cacheAgeControl");
		}
	}

	public HttpRequestCachePolicy(DateTime cacheSyncDate)
		: this(HttpRequestCacheLevel.Default)
	{
		m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
	}

	public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale, DateTime cacheSyncDate)
		: this(cacheAgeControl, maxAge, freshOrStale)
	{
		m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
	}

	public override string ToString()
	{
		return "Level:" + m_Level.ToString() + ((m_MaxAge == TimeSpan.MaxValue) ? string.Empty : (" MaxAge:" + m_MaxAge)) + ((m_MinFresh == TimeSpan.MinValue) ? string.Empty : (" MinFresh:" + m_MinFresh)) + ((m_MaxStale == TimeSpan.MinValue) ? string.Empty : (" MaxStale:" + m_MaxStale)) + ((CacheSyncDate == DateTime.MinValue) ? string.Empty : (" CacheSyncDate:" + CacheSyncDate.ToString(CultureInfo.CurrentCulture)));
	}

	private static RequestCacheLevel MapLevel(HttpRequestCacheLevel level)
	{
		if (level <= HttpRequestCacheLevel.NoCacheNoStore)
		{
			return (RequestCacheLevel)level;
		}
		return level switch
		{
			HttpRequestCacheLevel.CacheOrNextCacheOnly => RequestCacheLevel.CacheOnly, 
			HttpRequestCacheLevel.Refresh => RequestCacheLevel.Reload, 
			_ => throw new ArgumentOutOfRangeException("level"), 
		};
	}
}
