using System;
using System.Text;

namespace SimpleProto;

public class OutStream
{
	private Buffer m_buffer;

	private int m_offset;

	public int Offset => m_offset;

	public OutStream(Buffer buffer)
	{
		m_buffer = buffer;
	}

	public Buffer GetBuffer()
	{
		return m_buffer;
	}

	public void Seek(int offset)
	{
		m_offset = offset;
	}

	public void Write(bool val)
	{
		m_buffer.ExpandTo(m_offset + 1);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset++;
	}

	public void Write(double val)
	{
		m_buffer.ExpandTo(m_offset + 8);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset += 8;
	}

	public void Write(short val)
	{
		m_buffer.ExpandTo(m_offset + 2);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset += 2;
	}

	public void Write(ushort val)
	{
		m_buffer.ExpandTo(m_offset + 2);
		SimpleBitConverter.GetBytes((short)val, m_buffer.buffer, m_offset);
		m_offset += 2;
	}

	public void Write(int val)
	{
		m_buffer.ExpandTo(m_offset + 4);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset += 4;
	}

	public void Write(uint val)
	{
		m_buffer.ExpandTo(m_offset + 4);
		SimpleBitConverter.GetBytes((int)val, m_buffer.buffer, m_offset);
		m_offset += 4;
	}

	public void Write(long val)
	{
		m_buffer.ExpandTo(m_offset + 8);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset += 8;
	}

	public void Write(ulong val)
	{
		m_buffer.ExpandTo(m_offset + 8);
		SimpleBitConverter.GetBytes((long)val, m_buffer.buffer, m_offset);
		m_offset += 8;
	}

	public void Write(byte val)
	{
		m_buffer.ExpandTo(m_offset + 1);
		m_buffer.buffer[m_offset] = val;
		m_offset++;
	}

	public void Write(sbyte val)
	{
		Write((byte)val);
	}

	public void Write(float val)
	{
		m_buffer.ExpandTo(m_offset + 4);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset += 4;
	}

	public void Write(string val)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(val);
		Write(bytes.Length);
		m_buffer.ExpandTo(m_offset + bytes.Length);
		bytes.CopyTo(m_buffer.buffer, m_offset);
		m_offset += bytes.Length;
		byte val2 = 0;
		Write(val2);
	}

	public void Write(byte[] bytearray)
	{
		int num = bytearray.Length;
		Write(num);
		m_buffer.ExpandTo(m_offset + num);
		Array.Copy(bytearray, 0, m_buffer.buffer, m_offset, num);
		m_offset += num;
	}

	public void Write(Buffer buffer)
	{
		int length = buffer.length;
		Write(length);
		m_buffer.ExpandTo(m_offset + length);
		Array.Copy(buffer.buffer, 0, m_buffer.buffer, m_offset, length);
		m_offset += length;
	}
}
