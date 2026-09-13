namespace System.IO.Compression;

internal sealed class OutputWindow
{
	private const int WindowSize = 262144;

	private const int WindowMask = 262143;

	private readonly byte[] _window = new byte[262144];

	private int _end;

	private int _bytesUsed;

	public int FreeBytes => 262144 - _bytesUsed;

	public int AvailableBytes => _bytesUsed;

	public void Write(byte b)
	{
		_window[_end++] = b;
		_end &= 262143;
		_bytesUsed++;
	}

	public void WriteLengthDistance(int length, int distance)
	{
		_bytesUsed += length;
		int num = (_end - distance) & 0x3FFFF;
		int num2 = 262144 - length;
		if (num <= num2 && _end < num2)
		{
			if (length <= distance)
			{
				Array.Copy(_window, num, _window, _end, length);
				_end += length;
			}
			else
			{
				while (length-- > 0)
				{
					_window[_end++] = _window[num++];
				}
			}
		}
		else
		{
			while (length-- > 0)
			{
				_window[_end++] = _window[num++];
				_end &= 262143;
				num &= 0x3FFFF;
			}
		}
	}

	public int CopyFrom(InputBuffer input, int length)
	{
		length = Math.Min(Math.Min(length, 262144 - _bytesUsed), input.AvailableBytes);
		int num = 262144 - _end;
		int num2;
		if (length > num)
		{
			num2 = input.CopyTo(_window, _end, num);
			if (num2 == num)
			{
				num2 += input.CopyTo(_window, 0, length - num);
			}
		}
		else
		{
			num2 = input.CopyTo(_window, _end, length);
		}
		_end = (_end + num2) & 0x3FFFF;
		_bytesUsed += num2;
		return num2;
	}

	public int CopyTo(byte[] output, int offset, int length)
	{
		int num;
		if (length > _bytesUsed)
		{
			num = _end;
			length = _bytesUsed;
		}
		else
		{
			num = (_end - _bytesUsed + length) & 0x3FFFF;
		}
		int num2 = length;
		int num3 = length - num;
		if (num3 > 0)
		{
			Array.Copy(_window, 262144 - num3, output, offset, num3);
			offset += num3;
			length = num;
		}
		Array.Copy(_window, num - length, output, offset, length);
		_bytesUsed -= num2;
		return num2;
	}
}
