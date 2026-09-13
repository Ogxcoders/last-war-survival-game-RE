using System.Security;

namespace System.Runtime.InteropServices;

[Serializable]
[ComVisible(true)]
public struct ArrayWithOffset
{
	private object m_array;

	private int m_offset;

	private int m_count;

	[SecuritySafeCritical]
	public ArrayWithOffset(object array, int offset)
	{
		m_array = array;
		m_offset = offset;
		m_count = 0;
		m_count = CalculateCount();
	}

	public object GetArray()
	{
		return m_array;
	}

	public int GetOffset()
	{
		return m_offset;
	}

	public override int GetHashCode()
	{
		return m_count + m_offset;
	}

	public override bool Equals(object obj)
	{
		if (obj is ArrayWithOffset)
		{
			return Equals((ArrayWithOffset)obj);
		}
		return false;
	}

	public bool Equals(ArrayWithOffset obj)
	{
		if (obj.m_array == m_array && obj.m_offset == m_offset)
		{
			return obj.m_count == m_count;
		}
		return false;
	}

	public static bool operator ==(ArrayWithOffset a, ArrayWithOffset b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(ArrayWithOffset a, ArrayWithOffset b)
	{
		return !(a == b);
	}

	private int CalculateCount()
	{
		if (!(m_array is Array array))
		{
			throw new ArgumentException();
		}
		return array.Rank * array.Length - m_offset;
	}
}
