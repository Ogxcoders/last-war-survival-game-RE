namespace SimpleProto;

public class Buffer
{
	private byte[] m_buffer;

	private int m_length;

	public byte[] buffer
	{
		get
		{
			return m_buffer;
		}
		set
		{
			m_buffer = value;
		}
	}

	public int capcity => m_buffer.Length;

	public int length => m_length;

	public Buffer(int iniSize)
	{
		m_buffer = new byte[iniSize];
		m_length = 0;
	}

	public Buffer(byte[] buffer)
	{
		m_buffer = (byte[])buffer.Clone();
		m_length = m_buffer.Length;
	}

	public void ReduceSize(int size)
	{
		if (m_length > size)
		{
			m_length = size;
		}
	}

	public void ReAlloc(int size)
	{
		if (size > capcity)
		{
			int num = capcity;
			do
			{
				num *= 2;
			}
			while (num < size);
			byte[] array = new byte[num];
			m_buffer.CopyTo(array, 0);
			m_buffer = array;
		}
	}

	public void ExpandTo(int size)
	{
		ReAlloc(size);
		if (m_length < size)
		{
			m_length = size;
		}
	}

	public void SetLength(int size)
	{
		m_length = size;
	}
}
