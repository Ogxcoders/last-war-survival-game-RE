using System;
using System.Text;
using SimpleProto;

namespace ProtoBufNet;

public class NetInStream
{
	private NetBuffer m_buffer;

	private int m_offset;

	public NetInStream(NetBuffer buffer)
	{
		m_buffer = buffer;
	}

	public void Read(ref bool val)
	{
		Skip(1);
		val = SimpleBitConverter.ToBool(m_buffer.buffer, m_offset - 1);
	}

	public void Read(ref float val)
	{
		Skip(4);
		val = SimpleBitConverter.ToFloat(m_buffer.buffer, m_offset - 4);
	}

	public void Read(ref short val)
	{
		Skip(2);
		val = SimpleBitConverter.ToShort(m_buffer.buffer, m_offset - 2);
	}

	public void Read(ref int val)
	{
		Skip(4);
		val = SimpleBitConverter.ToInt(m_buffer.buffer, m_offset - 4);
	}

	public void Read(ref long val)
	{
		Skip(8);
		val = SimpleBitConverter.ToLong(m_buffer.buffer, m_offset - 8);
	}

	public void Read(ref byte val)
	{
		Skip(1);
		val = m_buffer.buffer[m_offset - 1];
	}

	public void Read(ref double val)
	{
		Skip(8);
		val = SimpleBitConverter.ToDouble(m_buffer.buffer, m_offset - 8);
	}

	public void Read(ref string val)
	{
		int val2 = 0;
		Read(ref val2);
		Skip(val2);
		val = Encoding.Default.GetString(m_buffer.buffer, m_offset - val2, val2);
		byte val3 = 0;
		Read(ref val3);
	}

	public void Read(ref NetBuffer buffer)
	{
		int val = 0;
		Read(ref val);
		buffer.Resize(val);
		Array.Copy(m_buffer.buffer, m_offset, buffer.buffer, 0, val);
		m_offset += val;
		buffer.length = val;
	}

	private void Skip(int size)
	{
		if (m_offset + size > m_buffer.length)
		{
			throw new NetStreamException("stream out of rang", size);
		}
		m_offset += size;
	}
}
