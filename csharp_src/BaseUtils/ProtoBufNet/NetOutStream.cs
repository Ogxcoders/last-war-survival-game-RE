using System;
using System.Text;
using SimpleProto;

namespace ProtoBufNet;

public class NetOutStream
{
	private NetBuffer m_buffer;

	private int m_offset;

	public NetOutStream(NetBuffer buffer)
	{
		m_buffer = buffer;
	}

	public void Write(bool val)
	{
		m_buffer.Resize(m_offset + 1);
		SimpleBitConverter.GetBytes(val, m_buffer.buffer, m_offset);
		m_offset++;
		m_buffer.length++;
	}

	public void Write(double val)
	{
		m_buffer.Resize(m_offset + 8);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 8;
		m_buffer.length += 8;
	}

	public void Write(short val)
	{
		m_buffer.Resize(m_offset + 2);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 2;
		m_buffer.length += 2;
	}

	public void Write(ushort val)
	{
		m_buffer.Resize(m_offset + 2);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 2;
		m_buffer.length += 2;
	}

	public void Write(int val)
	{
		m_buffer.Resize(m_offset + 4);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 4;
		m_buffer.length += 4;
	}

	public void Write(uint val)
	{
		m_buffer.Resize(m_offset + 4);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 4;
		m_buffer.length += 4;
	}

	public void Write(long val)
	{
		m_buffer.Resize(m_offset + 8);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 8;
		m_buffer.length += 8;
	}

	public void Write(ulong val)
	{
		m_buffer.Resize(m_offset + 8);
		BitConverter.GetBytes(val).CopyTo(m_buffer.buffer, m_offset);
		m_offset += 8;
		m_buffer.length += 8;
	}

	public void Write(byte val)
	{
		m_buffer.Resize(m_offset + 1);
		m_buffer.buffer[m_offset] = val;
		m_offset++;
		m_buffer.length++;
	}

	public void Write(sbyte val)
	{
		Write((byte)val);
	}

	public void Write(float val)
	{
		byte[] bytes = BitConverter.GetBytes(val);
		m_buffer.Resize(m_offset + bytes.Length);
		bytes.CopyTo(m_buffer.buffer, m_offset);
		m_offset += bytes.Length;
		m_buffer.length += bytes.Length;
	}

	public void Write(string val)
	{
		byte[] bytes = Encoding.Default.GetBytes(val);
		Write(bytes.Length);
		m_buffer.Resize(m_offset + bytes.Length);
		bytes.CopyTo(m_buffer.buffer, m_offset);
		m_offset += bytes.Length;
		m_buffer.length += bytes.Length;
		byte val2 = 0;
		Write(val2);
	}

	public void Write(NetBuffer buffer)
	{
		Write(buffer.length);
		m_buffer.Resize(m_offset + buffer.length);
		Array.Copy(buffer.buffer, 0, m_buffer.buffer, m_offset, buffer.length);
		m_offset += buffer.length;
		m_buffer.length += buffer.length;
	}
}
