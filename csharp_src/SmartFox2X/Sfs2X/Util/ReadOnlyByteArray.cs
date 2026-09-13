using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Libs.zlib;
using Sfs2X.Entities.Data;

namespace Sfs2X.Util;

public class ReadOnlyByteArray : IByteArray, IDisposable
{
	public interface ISharedBufferManager
	{
		byte[] Allocate(int size);

		void Free(byte[] buffer);
	}

	private static readonly bool _isLittleEndian = BitConverter.IsLittleEndian;

	private readonly byte[] _bytes;

	private readonly int _length;

	private int _position;

	private byte[] _sharedBuffer;

	private readonly ISharedBufferManager _sharedBufferManager;

	public byte[] Bytes
	{
		get
		{
			return _bytes;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public int Length => _length;

	public int Position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}

	public int BytesAvailable
	{
		get
		{
			int num = _length - _position;
			if (num > _length || num < 0)
			{
				num = 0;
			}
			return num;
		}
	}

	public ReadOnlyByteArray(byte[] bytes, int length, ISharedBufferManager sharedBufferManager)
	{
		_bytes = bytes;
		_length = length;
		_sharedBufferManager = sharedBufferManager;
		_sharedBuffer = ((sharedBufferManager == null) ? new byte[8] : sharedBufferManager.Allocate(8));
		_position = 0;
	}

	public bool ReadBool()
	{
		return _bytes[_position++] == 1;
	}

	public byte ReadByte()
	{
		return _bytes[_position++];
	}

	public byte[] ReadBytes(int count)
	{
		byte[] array = new byte[count];
		Buffer.BlockCopy(_bytes, _position, array, 0, count);
		_position += count;
		return array;
	}

	public unsafe T Read2Bytes<T>() where T : unmanaged
	{
		Buffer.BlockCopy(_bytes, _position, _sharedBuffer, 0, 2);
		_position += 2;
		fixed (byte* sharedBuffer = _sharedBuffer)
		{
			if (_isLittleEndian)
			{
				byte b = sharedBuffer[1];
				sharedBuffer[1] = *sharedBuffer;
				*sharedBuffer = b;
			}
			return *(T*)sharedBuffer;
		}
	}

	private unsafe T Read4Bytes<T>() where T : unmanaged
	{
		Buffer.BlockCopy(_bytes, _position, _sharedBuffer, 0, 4);
		_position += 4;
		fixed (byte* sharedBuffer = _sharedBuffer)
		{
			if (_isLittleEndian)
			{
				byte b = sharedBuffer[3];
				sharedBuffer[3] = *sharedBuffer;
				*sharedBuffer = b;
				b = sharedBuffer[2];
				sharedBuffer[2] = sharedBuffer[1];
				sharedBuffer[1] = b;
			}
			return *(T*)sharedBuffer;
		}
	}

	public unsafe T Read8Bytes<T>() where T : unmanaged
	{
		Buffer.BlockCopy(_bytes, _position, _sharedBuffer, 0, 8);
		_position += 8;
		fixed (byte* sharedBuffer = _sharedBuffer)
		{
			if (_isLittleEndian)
			{
				byte b = sharedBuffer[7];
				sharedBuffer[7] = *sharedBuffer;
				*sharedBuffer = b;
				b = sharedBuffer[6];
				sharedBuffer[6] = sharedBuffer[1];
				sharedBuffer[1] = b;
				b = sharedBuffer[5];
				sharedBuffer[5] = sharedBuffer[2];
				sharedBuffer[2] = b;
				b = sharedBuffer[4];
				sharedBuffer[4] = sharedBuffer[3];
				sharedBuffer[3] = b;
			}
			return *(T*)sharedBuffer;
		}
	}

	public int ReadInt()
	{
		return Read4Bytes<int>();
	}

	public ushort ReadUShort()
	{
		return Read2Bytes<ushort>();
	}

	public short ReadShort()
	{
		return Read2Bytes<short>();
	}

	public long ReadLong()
	{
		return Read8Bytes<long>();
	}

	public float ReadFloat()
	{
		return Read4Bytes<float>();
	}

	public double ReadDouble()
	{
		return Read8Bytes<double>();
	}

	public string ReadUTF()
	{
		ushort num = ReadUShort();
		string result = Encoding.UTF8.GetString(_bytes, _position, num);
		_position += num;
		return result;
	}

	public string ReadUTF(out int byteCount)
	{
		byteCount = ReadUShort();
		string result = Encoding.UTF8.GetString(_bytes, _position, byteCount);
		_position += byteCount;
		return result;
	}

	public string ReadText()
	{
		int num = ReadInt();
		string result = Encoding.UTF8.GetString(_bytes, _position, num);
		_position += num;
		return result;
	}

	public string ReadText(out int byteCount)
	{
		byteCount = ReadInt();
		string result = Encoding.UTF8.GetString(_bytes, _position, byteCount);
		_position += byteCount;
		return result;
	}

	public void WriteBool(bool b)
	{
		throw new NotImplementedException();
	}

	public void WriteByte(SFSDataType tp)
	{
		throw new NotImplementedException();
	}

	public void WriteByte(byte b)
	{
		throw new NotImplementedException();
	}

	public void WriteBytes(byte[] data)
	{
		throw new NotImplementedException();
	}

	public void WriteBytes(byte[] data, int ofs, int count)
	{
		throw new NotImplementedException();
	}

	public void WriteDouble(double d)
	{
		throw new NotImplementedException();
	}

	public void WriteFloat(float f)
	{
		throw new NotImplementedException();
	}

	public void WriteInt(int i)
	{
		throw new NotImplementedException();
	}

	public void WriteLong(long l)
	{
		throw new NotImplementedException();
	}

	public void WriteShort(short s)
	{
		throw new NotImplementedException();
	}

	public void WriteText(string str)
	{
		throw new NotImplementedException();
	}

	public void WriteUShort(ushort us)
	{
		throw new NotImplementedException();
	}

	public void WriteUTF(string str)
	{
		throw new NotImplementedException();
	}

	public static void ZlibCompressTo(MemoryStream ms, byte[] data, int offset, int dataLen)
	{
		using ZOutputStream zOutputStream = new ZOutputStream(ms);
		zOutputStream.Write(data, offset, dataLen);
		zOutputStream.Flush();
	}

	public void Dispose()
	{
		if (_sharedBufferManager != null && _sharedBuffer != null)
		{
			_sharedBufferManager.Free(_sharedBuffer);
			_sharedBuffer = null;
		}
	}
}
