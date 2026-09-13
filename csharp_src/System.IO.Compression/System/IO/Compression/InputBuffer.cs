namespace System.IO.Compression;

internal sealed class InputBuffer
{
	private byte[] _buffer;

	private int _start;

	private int _end;

	private uint _bitBuffer;

	private int _bitsInBuffer;

	public int AvailableBits => _bitsInBuffer;

	public int AvailableBytes => _end - _start + _bitsInBuffer / 8;

	public bool EnsureBitsAvailable(int count)
	{
		if (_bitsInBuffer < count)
		{
			if (NeedsInput())
			{
				return false;
			}
			_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
			_bitsInBuffer += 8;
			if (_bitsInBuffer < count)
			{
				if (NeedsInput())
				{
					return false;
				}
				_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
				_bitsInBuffer += 8;
			}
		}
		return true;
	}

	public uint TryLoad16Bits()
	{
		if (_bitsInBuffer < 8)
		{
			if (_start < _end)
			{
				_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
				_bitsInBuffer += 8;
			}
			if (_start < _end)
			{
				_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
				_bitsInBuffer += 8;
			}
		}
		else if (_bitsInBuffer < 16 && _start < _end)
		{
			_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
			_bitsInBuffer += 8;
		}
		return _bitBuffer;
	}

	private uint GetBitMask(int count)
	{
		return (uint)((1 << count) - 1);
	}

	public int GetBits(int count)
	{
		if (!EnsureBitsAvailable(count))
		{
			return -1;
		}
		uint result = _bitBuffer & GetBitMask(count);
		_bitBuffer >>= count;
		_bitsInBuffer -= count;
		return (int)result;
	}

	public int CopyTo(byte[] output, int offset, int length)
	{
		int num = 0;
		while (_bitsInBuffer > 0 && length > 0)
		{
			output[offset++] = (byte)_bitBuffer;
			_bitBuffer >>= 8;
			_bitsInBuffer -= 8;
			length--;
			num++;
		}
		if (length == 0)
		{
			return num;
		}
		int num2 = _end - _start;
		if (length > num2)
		{
			length = num2;
		}
		Array.Copy(_buffer, _start, output, offset, length);
		_start += length;
		return num + length;
	}

	public bool NeedsInput()
	{
		return _start == _end;
	}

	public void SetInput(byte[] buffer, int offset, int length)
	{
		_buffer = buffer;
		_start = offset;
		_end = offset + length;
	}

	public void SkipBits(int n)
	{
		_bitBuffer >>= n;
		_bitsInBuffer -= n;
	}

	public void SkipToByteBoundary()
	{
		_bitBuffer >>= _bitsInBuffer % 8;
		_bitsInBuffer -= _bitsInBuffer % 8;
	}
}
