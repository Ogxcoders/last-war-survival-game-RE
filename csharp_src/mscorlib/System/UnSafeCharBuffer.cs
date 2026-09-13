using System.Security;

namespace System;

internal struct UnSafeCharBuffer
{
	[SecurityCritical]
	private unsafe char* m_buffer;

	private int m_totalSize;

	private int m_length;

	public int Length => m_length;

	[SecurityCritical]
	public unsafe UnSafeCharBuffer(char* buffer, int bufferSize)
	{
		m_buffer = buffer;
		m_totalSize = bufferSize;
		m_length = 0;
	}

	[SecuritySafeCritical]
	public unsafe void AppendString(string stringToAppend)
	{
		if (!string.IsNullOrEmpty(stringToAppend))
		{
			if (m_totalSize - m_length < stringToAppend.Length)
			{
				throw new IndexOutOfRangeException();
			}
			fixed (char* src = stringToAppend)
			{
				Buffer.Memcpy((byte*)m_buffer + (nint)m_length * (nint)2, (byte*)src, stringToAppend.Length * 2);
			}
			m_length += stringToAppend.Length;
		}
	}
}
