namespace ProtoBufNet;

public class NetBuffer
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

	public int length
	{
		get
		{
			return m_length;
		}
		set
		{
			m_length = value;
		}
	}

	public NetBuffer(int iniSize)
	{
		m_buffer = new byte[iniSize];
	}

	public void Resize(int size)
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
}
