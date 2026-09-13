using System.Text;

namespace System.Net;

internal class HostHeaderString
{
	private bool m_Converted;

	private string m_String;

	private byte[] m_Bytes;

	internal string String
	{
		get
		{
			return m_String;
		}
		set
		{
			Init(value);
		}
	}

	internal int ByteCount
	{
		get
		{
			Convert();
			return m_Bytes.Length;
		}
	}

	internal byte[] Bytes
	{
		get
		{
			Convert();
			return m_Bytes;
		}
	}

	internal HostHeaderString()
	{
		Init(null);
	}

	internal HostHeaderString(string s)
	{
		Init(s);
	}

	private void Init(string s)
	{
		m_String = s;
		m_Converted = false;
		m_Bytes = null;
	}

	private void Convert()
	{
		if (m_String != null && !m_Converted)
		{
			m_Bytes = Encoding.Default.GetBytes(m_String);
			string strB = Encoding.Default.GetString(m_Bytes);
			if (string.Compare(m_String, strB, StringComparison.Ordinal) != 0)
			{
				m_Bytes = Encoding.UTF8.GetBytes(m_String);
			}
		}
	}

	internal void Copy(byte[] destBytes, int destByteIndex)
	{
		Convert();
		Array.Copy(m_Bytes, 0, destBytes, destByteIndex, m_Bytes.Length);
	}
}
