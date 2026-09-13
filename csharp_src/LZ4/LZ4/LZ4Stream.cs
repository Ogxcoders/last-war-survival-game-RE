using System;
using System.IO;
using System.IO.Compression;

namespace LZ4;

public class LZ4Stream : Stream
{
	[Flags]
	public enum ChunkFlags
	{
		None = 0,
		Compressed = 1,
		HighCompression = 2,
		Passes = 0x1C
	}

	private readonly Stream _innerStream;

	private readonly LZ4StreamMode _compressionMode;

	private readonly bool _highCompression;

	private readonly bool _interactiveRead;

	private readonly bool _isolateInnerStream;

	private readonly int _blockSize;

	private byte[] _buffer;

	private int _bufferLength;

	private int _bufferOffset;

	public override bool CanRead => _compressionMode == LZ4StreamMode.Decompress;

	public override bool CanSeek => false;

	public override bool CanWrite => _compressionMode == LZ4StreamMode.Compress;

	public override long Length => -1L;

	public override long Position
	{
		get
		{
			return -1L;
		}
		set
		{
			throw NotSupported("SetPosition");
		}
	}

	[Obsolete("This constructor is obsolete")]
	public LZ4Stream(Stream innerStream, LZ4StreamMode compressionMode, bool highCompression, int blockSize = 1048576, bool interactiveRead = false)
	{
		_innerStream = innerStream;
		_compressionMode = compressionMode;
		_highCompression = highCompression;
		_interactiveRead = interactiveRead;
		_isolateInnerStream = false;
		_blockSize = Math.Max(16, blockSize);
	}

	public LZ4Stream(Stream innerStream, LZ4StreamMode compressionMode, LZ4StreamFlags compressionFlags = LZ4StreamFlags.None, int blockSize = 1048576)
	{
		_innerStream = innerStream;
		_compressionMode = compressionMode;
		_highCompression = (compressionFlags & LZ4StreamFlags.HighCompression) != 0;
		_interactiveRead = (compressionFlags & LZ4StreamFlags.InteractiveRead) != 0;
		_isolateInnerStream = (compressionFlags & LZ4StreamFlags.IsolateInnerStream) != 0;
		_blockSize = Math.Max(16, blockSize);
	}

	private static NotSupportedException NotSupported(string operationName)
	{
		return new NotSupportedException($"Operation '{operationName}' is not supported");
	}

	private static EndOfStreamException EndOfStream()
	{
		return new EndOfStreamException("Unexpected end of stream");
	}

	private bool TryReadVarInt(out ulong result)
	{
		byte[] array = new byte[1];
		int num = 0;
		result = 0uL;
		byte b;
		do
		{
			if (_innerStream.Read(array, 0, 1) == 0)
			{
				if (num == 0)
				{
					return false;
				}
				throw EndOfStream();
			}
			b = array[0];
			result += (ulong)((long)(b & 0x7F) << num);
			num += 7;
		}
		while ((b & 0x80) != 0 && num < 64);
		return true;
	}

	private ulong ReadVarInt()
	{
		if (!TryReadVarInt(out var result))
		{
			throw EndOfStream();
		}
		return result;
	}

	private int ReadBlock(byte[] buffer, int offset, int length)
	{
		int num = 0;
		while (length > 0)
		{
			int num2 = _innerStream.Read(buffer, offset, length);
			if (num2 == 0)
			{
				break;
			}
			offset += num2;
			length -= num2;
			num += num2;
		}
		return num;
	}

	private void WriteVarInt(ulong value)
	{
		byte[] array = new byte[1];
		do
		{
			byte b = (byte)(value & 0x7F);
			value >>= 7;
			array[0] = (byte)(b | ((value != 0) ? 128 : 0));
			_innerStream.Write(array, 0, 1);
		}
		while (value != 0);
	}

	private void FlushCurrentChunk()
	{
		if (_bufferOffset > 0)
		{
			byte[] array = new byte[_bufferOffset];
			int num = (_highCompression ? LZ4Codec.EncodeHC(_buffer, 0, _bufferOffset, array, 0, _bufferOffset) : LZ4Codec.Encode(_buffer, 0, _bufferOffset, array, 0, _bufferOffset));
			if (num <= 0 || num >= _bufferOffset)
			{
				array = _buffer;
				num = _bufferOffset;
			}
			bool flag = num < _bufferOffset;
			ChunkFlags chunkFlags = ChunkFlags.None;
			if (flag)
			{
				chunkFlags |= ChunkFlags.Compressed;
			}
			if (_highCompression)
			{
				chunkFlags |= ChunkFlags.HighCompression;
			}
			WriteVarInt((ulong)chunkFlags);
			WriteVarInt((ulong)_bufferOffset);
			if (flag)
			{
				WriteVarInt((ulong)num);
			}
			_innerStream.Write(array, 0, num);
			_bufferOffset = 0;
		}
	}

	private bool AcquireNextChunk()
	{
		do
		{
			if (!TryReadVarInt(out var result))
			{
				return false;
			}
			ChunkFlags chunkFlags = (ChunkFlags)result;
			bool flag = (chunkFlags & ChunkFlags.Compressed) != 0;
			int num = (int)ReadVarInt();
			int num2 = (flag ? ((int)ReadVarInt()) : num);
			if (num2 > num)
			{
				throw EndOfStream();
			}
			byte[] array = new byte[num2];
			int num3 = ReadBlock(array, 0, num2);
			if (num3 != num2)
			{
				throw EndOfStream();
			}
			if (!flag)
			{
				_buffer = array;
				_bufferLength = num2;
			}
			else
			{
				if (_buffer == null || _buffer.Length < num)
				{
					_buffer = new byte[num];
				}
				if ((int)chunkFlags >> 2 != 0)
				{
					throw new NotSupportedException("Chunks with multiple passes are not supported.");
				}
				LZ4Codec.Decode(array, 0, num2, _buffer, 0, num, knownOutputLength: true);
				_bufferLength = num;
			}
			_bufferOffset = 0;
		}
		while (_bufferLength == 0);
		return true;
	}

	public override void Flush()
	{
		if (_bufferOffset > 0 && CanWrite)
		{
			FlushCurrentChunk();
		}
	}

	public override int ReadByte()
	{
		if (!CanRead)
		{
			throw NotSupported("Read");
		}
		if (_bufferOffset >= _bufferLength && !AcquireNextChunk())
		{
			return -1;
		}
		return _buffer[_bufferOffset++];
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (!CanRead)
		{
			throw NotSupported("Read");
		}
		int num = 0;
		while (count > 0)
		{
			int num2 = Math.Min(count, _bufferLength - _bufferOffset);
			if (num2 > 0)
			{
				Buffer.BlockCopy(_buffer, _bufferOffset, buffer, offset, num2);
				_bufferOffset += num2;
				num += num2;
				if (_interactiveRead)
				{
					break;
				}
				offset += num2;
				count -= num2;
			}
			else if (!AcquireNextChunk())
			{
				break;
			}
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw NotSupported("Seek");
	}

	public override void SetLength(long value)
	{
		throw NotSupported("SetLength");
	}

	public override void WriteByte(byte value)
	{
		if (!CanWrite)
		{
			throw NotSupported("Write");
		}
		if (_buffer == null)
		{
			_buffer = new byte[_blockSize];
			_bufferLength = _blockSize;
			_bufferOffset = 0;
		}
		if (_bufferOffset >= _bufferLength)
		{
			FlushCurrentChunk();
		}
		_buffer[_bufferOffset++] = value;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (!CanWrite)
		{
			throw NotSupported("Write");
		}
		if (_buffer == null)
		{
			_buffer = new byte[_blockSize];
			_bufferLength = _blockSize;
			_bufferOffset = 0;
		}
		while (count > 0)
		{
			int num = Math.Min(count, _bufferLength - _bufferOffset);
			if (num > 0)
			{
				Buffer.BlockCopy(buffer, offset, _buffer, _bufferOffset, num);
				offset += num;
				count -= num;
				_bufferOffset += num;
			}
			else
			{
				FlushCurrentChunk();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		Flush();
		if (!_isolateInnerStream)
		{
			_innerStream.Dispose();
		}
		base.Dispose(disposing);
	}

	[Obsolete("This constructor is obsolete")]
	public LZ4Stream(Stream innerStream, CompressionMode compressionMode, bool highCompression, int blockSize = 1048576, bool interactiveRead = false)
		: this(innerStream, ToLZ4StreamMode(compressionMode), CombineLZ4Flags(highCompression, interactiveRead), blockSize)
	{
	}

	public LZ4Stream(Stream innerStream, CompressionMode compressionMode, LZ4StreamFlags compressionFlags = LZ4StreamFlags.None, int blockSize = 1048576)
		: this(innerStream, ToLZ4StreamMode(compressionMode), compressionFlags, blockSize)
	{
	}

	private static LZ4StreamMode ToLZ4StreamMode(CompressionMode compressionMode)
	{
		return compressionMode switch
		{
			CompressionMode.Compress => LZ4StreamMode.Compress, 
			CompressionMode.Decompress => LZ4StreamMode.Decompress, 
			_ => throw new ArgumentException($"Unhandled compression mode: {compressionMode}"), 
		};
	}

	private static LZ4StreamFlags CombineLZ4Flags(bool highCompression, bool interactiveRead)
	{
		LZ4StreamFlags lZ4StreamFlags = LZ4StreamFlags.None;
		if (highCompression)
		{
			lZ4StreamFlags |= LZ4StreamFlags.HighCompression;
		}
		if (interactiveRead)
		{
			lZ4StreamFlags |= LZ4StreamFlags.InteractiveRead;
		}
		return lZ4StreamFlags;
	}
}
