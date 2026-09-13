namespace System.IO.Compression;

internal sealed class OutputBuffer
{
	internal readonly struct BufferState
	{
		internal readonly int _pos;

		internal readonly uint _bitBuf;

		internal readonly int _bitCount;

		internal BufferState(int pos, uint bitBuf, int bitCount)
		{
			_pos = pos;
			_bitBuf = bitBuf;
			_bitCount = bitCount;
		}
	}

	private byte[] _byteBuffer;

	private int _pos;

	private uint _bitBuf;

	private int _bitCount;

	internal int BytesWritten => _pos;

	internal int FreeBytes => _byteBuffer.Length - _pos;

	internal int BitsInBuffer => _bitCount / 8 + 1;

	internal void UpdateBuffer(byte[] output)
	{
		_byteBuffer = output;
		_pos = 0;
	}

	internal void WriteUInt16(ushort value)
	{
		_byteBuffer[_pos++] = (byte)value;
		_byteBuffer[_pos++] = (byte)(value >> 8);
	}

	internal void WriteBits(int n, uint bits)
	{
		_bitBuf |= bits << _bitCount;
		_bitCount += n;
		if (_bitCount >= 16)
		{
			_byteBuffer[_pos++] = (byte)_bitBuf;
			_byteBuffer[_pos++] = (byte)(_bitBuf >> 8);
			_bitCount -= 16;
			_bitBuf >>= 16;
		}
	}

	internal void FlushBits()
	{
		while (_bitCount >= 8)
		{
			_byteBuffer[_pos++] = (byte)_bitBuf;
			_bitCount -= 8;
			_bitBuf >>= 8;
		}
		if (_bitCount > 0)
		{
			_byteBuffer[_pos++] = (byte)_bitBuf;
			_bitBuf = 0u;
			_bitCount = 0;
		}
	}

	internal void WriteBytes(byte[] byteArray, int offset, int count)
	{
		if (_bitCount == 0)
		{
			Array.Copy(byteArray, offset, _byteBuffer, _pos, count);
			_pos += count;
		}
		else
		{
			WriteBytesUnaligned(byteArray, offset, count);
		}
	}

	private void WriteBytesUnaligned(byte[] byteArray, int offset, int count)
	{
		for (int i = 0; i < count; i++)
		{
			byte b = byteArray[offset + i];
			WriteByteUnaligned(b);
		}
	}

	private void WriteByteUnaligned(byte b)
	{
		WriteBits(8, b);
	}

	internal BufferState DumpState()
	{
		return new BufferState(_pos, _bitBuf, _bitCount);
	}

	internal void RestoreState(BufferState state)
	{
		_pos = state._pos;
		_bitBuf = state._bitBuf;
		_bitCount = state._bitCount;
	}
}
