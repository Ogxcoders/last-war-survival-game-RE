namespace System.Globalization;

[Serializable]
public sealed class SortVersion : IEquatable<SortVersion>
{
	private int m_NlsVersion;

	private Guid m_SortId;

	public int FullVersion => m_NlsVersion;

	public Guid SortId => m_SortId;

	public SortVersion(int fullVersion, Guid sortId)
	{
		m_SortId = sortId;
		m_NlsVersion = fullVersion;
	}

	internal SortVersion(int nlsVersion, int effectiveId, Guid customVersion)
	{
		m_NlsVersion = nlsVersion;
		if (customVersion == Guid.Empty)
		{
			byte h = (byte)(effectiveId >> 24);
			byte i = (byte)((effectiveId & 0xFF0000) >> 16);
			byte j = (byte)((effectiveId & 0xFF00) >> 8);
			byte k = (byte)(effectiveId & 0xFF);
			customVersion = new Guid(0, 0, 0, 0, 0, 0, 0, h, i, j, k);
		}
		m_SortId = customVersion;
	}

	public override bool Equals(object obj)
	{
		SortVersion sortVersion = obj as SortVersion;
		if (sortVersion != null)
		{
			return Equals(sortVersion);
		}
		return false;
	}

	public bool Equals(SortVersion other)
	{
		if (other == null)
		{
			return false;
		}
		if (m_NlsVersion == other.m_NlsVersion)
		{
			return m_SortId == other.m_SortId;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (m_NlsVersion * 7) | m_SortId.GetHashCode();
	}

	public static bool operator ==(SortVersion left, SortVersion right)
	{
		return left?.Equals(right) ?? right?.Equals(left) ?? true;
	}

	public static bool operator !=(SortVersion left, SortVersion right)
	{
		return !(left == right);
	}
}
