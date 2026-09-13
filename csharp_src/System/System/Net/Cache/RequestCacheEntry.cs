using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace System.Net.Cache;

internal class RequestCacheEntry
{
	private bool m_IsPrivateEntry;

	private long m_StreamSize;

	private DateTime m_ExpiresUtc;

	private int m_HitCount;

	private DateTime m_LastAccessedUtc;

	private DateTime m_LastModifiedUtc;

	private DateTime m_LastSynchronizedUtc;

	private TimeSpan m_MaxStale;

	private int m_UsageCount;

	private bool m_IsPartialEntry;

	private StringCollection m_EntryMetadata;

	private StringCollection m_SystemMetadata;

	internal bool IsPrivateEntry
	{
		get
		{
			return m_IsPrivateEntry;
		}
		set
		{
			m_IsPrivateEntry = value;
		}
	}

	internal long StreamSize
	{
		get
		{
			return m_StreamSize;
		}
		set
		{
			m_StreamSize = value;
		}
	}

	internal DateTime ExpiresUtc
	{
		get
		{
			return m_ExpiresUtc;
		}
		set
		{
			m_ExpiresUtc = value;
		}
	}

	internal DateTime LastAccessedUtc
	{
		get
		{
			return m_LastAccessedUtc;
		}
		set
		{
			m_LastAccessedUtc = value;
		}
	}

	internal DateTime LastModifiedUtc
	{
		get
		{
			return m_LastModifiedUtc;
		}
		set
		{
			m_LastModifiedUtc = value;
		}
	}

	internal DateTime LastSynchronizedUtc
	{
		get
		{
			return m_LastSynchronizedUtc;
		}
		set
		{
			m_LastSynchronizedUtc = value;
		}
	}

	internal TimeSpan MaxStale
	{
		get
		{
			return m_MaxStale;
		}
		set
		{
			m_MaxStale = value;
		}
	}

	internal int HitCount
	{
		get
		{
			return m_HitCount;
		}
		set
		{
			m_HitCount = value;
		}
	}

	internal int UsageCount
	{
		get
		{
			return m_UsageCount;
		}
		set
		{
			m_UsageCount = value;
		}
	}

	internal bool IsPartialEntry
	{
		get
		{
			return m_IsPartialEntry;
		}
		set
		{
			m_IsPartialEntry = value;
		}
	}

	internal StringCollection EntryMetadata
	{
		get
		{
			return m_EntryMetadata;
		}
		set
		{
			m_EntryMetadata = value;
		}
	}

	internal StringCollection SystemMetadata
	{
		get
		{
			return m_SystemMetadata;
		}
		set
		{
			m_SystemMetadata = value;
		}
	}

	internal RequestCacheEntry()
	{
		m_ExpiresUtc = (m_LastAccessedUtc = (m_LastModifiedUtc = (m_LastSynchronizedUtc = DateTime.MinValue)));
	}

	internal virtual string ToString(bool verbose)
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		stringBuilder.Append("\r\nIsPrivateEntry   = ").Append(IsPrivateEntry);
		stringBuilder.Append("\r\nIsPartialEntry   = ").Append(IsPartialEntry);
		stringBuilder.Append("\r\nStreamSize       = ").Append(StreamSize);
		stringBuilder.Append("\r\nExpires          = ").Append((ExpiresUtc == DateTime.MinValue) ? "" : ExpiresUtc.ToString("r", CultureInfo.CurrentCulture));
		stringBuilder.Append("\r\nLastAccessed     = ").Append((LastAccessedUtc == DateTime.MinValue) ? "" : LastAccessedUtc.ToString("r", CultureInfo.CurrentCulture));
		stringBuilder.Append("\r\nLastModified     = ").Append((LastModifiedUtc == DateTime.MinValue) ? "" : LastModifiedUtc.ToString("r", CultureInfo.CurrentCulture));
		stringBuilder.Append("\r\nLastSynchronized = ").Append((LastSynchronizedUtc == DateTime.MinValue) ? "" : LastSynchronizedUtc.ToString("r", CultureInfo.CurrentCulture));
		stringBuilder.Append("\r\nMaxStale(sec)    = ").Append((MaxStale == TimeSpan.MinValue) ? "" : ((int)MaxStale.TotalSeconds).ToString(NumberFormatInfo.CurrentInfo));
		stringBuilder.Append("\r\nHitCount         = ").Append(HitCount.ToString(NumberFormatInfo.CurrentInfo));
		stringBuilder.Append("\r\nUsageCount       = ").Append(UsageCount.ToString(NumberFormatInfo.CurrentInfo));
		stringBuilder.Append("\r\n");
		if (verbose)
		{
			stringBuilder.Append("EntryMetadata:\r\n");
			if (m_EntryMetadata != null)
			{
				StringEnumerator enumerator = m_EntryMetadata.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						stringBuilder.Append(current).Append("\r\n");
					}
				}
				finally
				{
					if (enumerator is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}
			}
			stringBuilder.Append("---\r\nSystemMetadata:\r\n");
			if (m_SystemMetadata != null)
			{
				StringEnumerator enumerator = m_SystemMetadata.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current2 = enumerator.Current;
						stringBuilder.Append(current2).Append("\r\n");
					}
				}
				finally
				{
					if (enumerator is IDisposable disposable2)
					{
						disposable2.Dispose();
					}
				}
			}
		}
		return stringBuilder.ToString();
	}
}
