using System;
using System.Text;
using GameFramework;

namespace SimpleProto;

public class InStream
{
	private Buffer m_buffer;

	private int m_offset;

	public int Offset => m_offset;

	public InStream(Buffer buffer)
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

	public void Read(ref ushort val)
	{
		Skip(2);
		val = (ushort)SimpleBitConverter.ToShort(m_buffer.buffer, m_offset - 2);
	}

	public void Read(ref int val)
	{
		Skip(4);
		val = SimpleBitConverter.ToInt(m_buffer.buffer, m_offset - 4);
	}

	public void Read(ref uint val)
	{
		Skip(4);
		val = (uint)SimpleBitConverter.ToInt(m_buffer.buffer, m_offset - 4);
	}

	public void Read(ref long val)
	{
		Skip(8);
		val = SimpleBitConverter.ToLong(m_buffer.buffer, m_offset - 8);
	}

	public void Read(ref ulong val)
	{
		Skip(8);
		val = (ulong)SimpleBitConverter.ToLong(m_buffer.buffer, m_offset - 8);
	}

	public void Read(ref byte val)
	{
		Skip(1);
		val = m_buffer.buffer[m_offset - 1];
	}

	public void Read(ref sbyte val)
	{
		Skip(1);
		val = (sbyte)m_buffer.buffer[m_offset - 1];
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
		val = Encoding.UTF8.GetString(m_buffer.buffer, m_offset - val2, val2);
		byte val3 = 0;
		Read(ref val3);
	}

	public void Read(ref byte[] buffer)
	{
		int val = 0;
		Read(ref val);
		buffer = new byte[val];
		Array.Copy(m_buffer.buffer, m_offset, buffer, 0, val);
		m_offset += val;
	}

	public void Read(ref Buffer buffer)
	{
		int val = 0;
		Read(ref val);
		buffer.ExpandTo(val);
		Array.Copy(m_buffer.buffer, m_offset, buffer.buffer, 0, val);
		buffer.SetLength(val);
		m_offset += val;
	}

	public void Skip(int size)
	{
		if (m_offset + size > m_buffer.length)
		{
			Log.Error("stream out of range {0}", size);
		}
		else
		{
			m_offset += size;
		}
	}

	public int BytesLeft()
	{
		return m_buffer.length - m_offset;
	}

	public void Rewind()
	{
		m_offset = 0;
	}

	public Buffer GetBuffer()
	{
		return m_buffer;
	}
}
