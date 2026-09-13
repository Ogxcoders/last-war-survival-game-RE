using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Libs.zlib;
using LZ4;
using Sfs2XLw.Entities.Data;

namespace Sfs2XLw.Util;

public class ByteArray
{
	private byte[] buffer;

	private int position;

	private bool compressed;

	public byte[] Bytes
	{
		get
		{
			return buffer;
		}
		set
		{
			buffer = value;
			compressed = false;
		}
	}

	public int Length => buffer.Length;

	public int Position
	{
		get
		{
			return position;
		}
		set
		{
			position = value;
		}
	}

	public int BytesAvailable
	{
		get
		{
			int num = buffer.Length - position;
			if (num > buffer.Length || num < 0)
			{
				num = 0;
			}
			return num;
		}
	}

	public bool Compressed
	{
		get
		{
			return compressed;
		}
		set
		{
			compressed = value;
		}
	}

	public ByteArray()
	{
		buffer = new byte[0];
	}

	public ByteArray(byte[] buf)
	{
		buffer = buf;
	}

	public void Compress(bool useLZ4 = false, int offset = 0)
	{
		if (compressed)
		{
			throw new Exception("Buffer is already compressed");
		}
		MemoryStream memoryStream = new MemoryStream();
		if (!useLZ4)
		{
			using ZOutputStream zOutputStream = new ZOutputStream(memoryStream, 9);
			zOutputStream.Write(buffer, offset, buffer.Length - offset);
			zOutputStream.Flush();
		}
		else
		{
			using LZ4Stream lZ4Stream = new LZ4Stream(memoryStream, LZ4StreamMode.Compress);
			lZ4Stream.Write(buffer, offset, buffer.Length - offset);
			lZ4Stream.Flush();
		}
		buffer = memoryStream.ToArray();
		position = 0;
		compressed = true;
	}

	public void Uncompress(bool useLZ4 = false, int unCompressSize = 0, int offset = 0)
	{
		if (!useLZ4)
		{
			MemoryStream memoryStream = new MemoryStream();
			using (ZOutputStream zOutputStream = new ZOutputStream(memoryStream))
			{
				zOutputStream.Write(buffer, offset, buffer.Length - offset);
				zOutputStream.Flush();
			}
			buffer = memoryStream.ToArray();
		}
		else
		{
			using LZ4Stream lZ4Stream = new LZ4Stream(new MemoryStream(buffer, offset, buffer.Length - offset), LZ4StreamMode.Decompress);
			if (unCompressSize == 0)
			{
				byte[] array = new byte[1024];
				MemoryStream memoryStream2 = new MemoryStream();
				int num;
				do
				{
					num = lZ4Stream.Read(array, 0, 1024);
					memoryStream2.Write(array, 0, num);
				}
				while (num >= 1024);
				buffer = memoryStream2.ToArray();
			}
			else
			{
				buffer = new byte[unCompressSize];
				lZ4Stream.Read(buffer, 0, unCompressSize);
			}
		}
		position = 0;
		compressed = false;
	}

	private void CheckCompressedWrite()
	{
		if (compressed)
		{
			throw new Exception("Only raw bytes can be written to a compressed array. Call Uncompress first.");
		}
	}

	private void CheckCompressedRead()
	{
		if (compressed)
		{
			throw new Exception("Only raw bytes can be read from a compressed array.");
		}
	}

	private static byte[] ReverseOrder(byte[] buff)
	{
		if (!BitConverter.IsLittleEndian)
		{
			return buff;
		}
		int num = buff.Length;
		if (num < 2)
		{
			return buff;
		}
		int num2 = num / 2;
		for (int i = 0; i < num2; i++)
		{
			byte b = buff[i];
			int num3 = num - i - 1;
			buff[i] = buff[num3];
			buff[num3] = b;
		}
		return buff;
	}

	public void WriteByte(SFSDataType tp)
	{
		WriteByte(Convert.ToByte((int)tp));
	}

	public void WriteByte(byte b)
	{
		WriteBytes(new byte[1] { b });
	}

	public void WriteBytes(byte[] data)
	{
		WriteBytes(data, 0, data.Length);
	}

	public void WriteBytes(byte[] data, int ofs, int count)
	{
		byte[] dst = new byte[count + buffer.Length];
		Buffer.BlockCopy(buffer, 0, dst, 0, buffer.Length);
		Buffer.BlockCopy(data, ofs, dst, buffer.Length, count);
		buffer = dst;
	}

	public void WriteBool(bool b)
	{
		CheckCompressedWrite();
		WriteBytes(new byte[1] { (byte)(b ? 1u : 0u) });
	}

	public void WriteInt(int i)
	{
		CheckCompressedWrite();
		byte[] bytes = BitConverter.GetBytes(i);
		WriteBytes(ReverseOrder(bytes));
	}

	public void WriteUShort(ushort us)
	{
		CheckCompressedWrite();
		byte[] bytes = BitConverter.GetBytes(us);
		WriteBytes(ReverseOrder(bytes));
	}

	public void WriteShort(short s)
	{
		CheckCompressedWrite();
		byte[] bytes = BitConverter.GetBytes(s);
		WriteBytes(ReverseOrder(bytes));
	}

	public void WriteLong(long l)
	{
		CheckCompressedWrite();
		byte[] bytes = BitConverter.GetBytes(l);
		WriteBytes(ReverseOrder(bytes));
	}

	public void WriteFloat(float f)
	{
		CheckCompressedWrite();
		byte[] bytes = BitConverter.GetBytes(f);
		WriteBytes(ReverseOrder(bytes));
	}

	public void WriteDouble(double d)
	{
		CheckCompressedWrite();
		byte[] bytes = BitConverter.GetBytes(d);
		WriteBytes(ReverseOrder(bytes));
	}

	public void WriteUTF(string str)
	{
		CheckCompressedWrite();
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		int num = bytes.Length;
		if (num > 32767)
		{
			throw new FormatException("String length cannot be greater than " + short.MaxValue + " bytes!");
		}
		WriteUShort(Convert.ToUInt16(num));
		WriteBytes(bytes);
	}

	public void WriteText(string str)
	{
		CheckCompressedWrite();
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		WriteInt(bytes.Length);
		WriteBytes(bytes);
	}

	public byte ReadByte()
	{
		CheckCompressedRead();
		return buffer[position++];
	}

	public byte[] ReadBytes(int count)
	{
		byte[] array = new byte[count];
		Buffer.BlockCopy(buffer, position, array, 0, count);
		position += count;
		return array;
	}

	public bool ReadBool()
	{
		CheckCompressedRead();
		return buffer[position++] == 1;
	}

	public int ReadInt()
	{
		CheckCompressedRead();
		return BitConverter.ToInt32(ReverseOrder(ReadBytes(4)), 0);
	}

	public ushort ReadUShort()
	{
		CheckCompressedRead();
		return BitConverter.ToUInt16(ReverseOrder(ReadBytes(2)), 0);
	}

	public short ReadShort()
	{
		CheckCompressedRead();
		return BitConverter.ToInt16(ReverseOrder(ReadBytes(2)), 0);
	}

	public long ReadLong()
	{
		CheckCompressedRead();
		return BitConverter.ToInt64(ReverseOrder(ReadBytes(8)), 0);
	}

	public float ReadFloat()
	{
		CheckCompressedRead();
		return BitConverter.ToSingle(ReverseOrder(ReadBytes(4)), 0);
	}

	public double ReadDouble()
	{
		CheckCompressedRead();
		return BitConverter.ToDouble(ReverseOrder(ReadBytes(8)), 0);
	}

	public string ReadUTF()
	{
		CheckCompressedRead();
		ushort num = ReadUShort();
		string result = Encoding.UTF8.GetString(buffer, position, num);
		position += num;
		return result;
	}

	public string ReadUTF(out int byteCount)
	{
		CheckCompressedRead();
		byteCount = ReadUShort();
		string result = Encoding.UTF8.GetString(buffer, position, byteCount);
		position += byteCount;
		return result;
	}

	public string ReadText()
	{
		CheckCompressedRead();
		int num = ReadInt();
		string result = Encoding.UTF8.GetString(buffer, position, num);
		position += num;
		return result;
	}

	public string ReadText(out int byteCount)
	{
		CheckCompressedRead();
		byteCount = ReadInt();
		string result = Encoding.UTF8.GetString(buffer, position, byteCount);
		position += byteCount;
		return result;
	}
}
