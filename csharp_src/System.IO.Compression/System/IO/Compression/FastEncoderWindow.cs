using System.Diagnostics;

namespace System.IO.Compression;

internal sealed class FastEncoderWindow
{
	private byte[] _window;

	private int _bufPos;

	private int _bufEnd;

	private const int FastEncoderHashShift = 4;

	private const int FastEncoderHashtableSize = 2048;

	private const int FastEncoderHashMask = 2047;

	private const int FastEncoderWindowSize = 8192;

	private const int FastEncoderWindowMask = 8191;

	private const int FastEncoderMatch3DistThreshold = 16384;

	internal const int MaxMatch = 258;

	internal const int MinMatch = 3;

	private const int SearchDepth = 32;

	private const int GoodLength = 4;

	private const int NiceLength = 32;

	private const int LazyMatchThreshold = 6;

	private ushort[] _prev;

	private ushort[] _lookup;

	public int BytesAvailable => _bufEnd - _bufPos;

	public DeflateInput UnprocessedInput => new DeflateInput
	{
		Buffer = _window,
		StartIndex = _bufPos,
		Count = _bufEnd - _bufPos
	};

	public int FreeWindowSpace => 16384 - _bufEnd;

	public FastEncoderWindow()
	{
		ResetWindow();
	}

	public void FlushWindow()
	{
		ResetWindow();
	}

	private void ResetWindow()
	{
		_window = new byte[16646];
		_prev = new ushort[8450];
		_lookup = new ushort[2048];
		_bufPos = 8192;
		_bufEnd = _bufPos;
	}

	public void CopyBytes(byte[] inputBuffer, int startIndex, int count)
	{
		Array.Copy(inputBuffer, startIndex, _window, _bufEnd, count);
		_bufEnd += count;
	}

	public void MoveWindows()
	{
		Array.Copy(_window, _bufPos - 8192, _window, 0, 8192);
		for (int i = 0; i < 2048; i++)
		{
			int num = _lookup[i] - 8192;
			if (num <= 0)
			{
				_lookup[i] = 0;
			}
			else
			{
				_lookup[i] = (ushort)num;
			}
		}
		for (int i = 0; i < 8192; i++)
		{
			long num2 = (long)_prev[i] - 8192L;
			if (num2 <= 0)
			{
				_prev[i] = 0;
			}
			else
			{
				_prev[i] = (ushort)num2;
			}
		}
		_bufPos = 8192;
		_bufEnd = _bufPos;
	}

	private uint HashValue(uint hash, byte b)
	{
		return (hash << 4) ^ b;
	}

	private uint InsertString(ref uint hash)
	{
		hash = HashValue(hash, _window[_bufPos + 2]);
		uint num = _lookup[hash & 0x7FF];
		_lookup[hash & 0x7FF] = (ushort)_bufPos;
		_prev[_bufPos & 0x1FFF] = (ushort)num;
		return num;
	}

	private void InsertStrings(ref uint hash, int matchLen)
	{
		if (_bufEnd - _bufPos <= matchLen)
		{
			_bufPos += matchLen - 1;
			return;
		}
		while (--matchLen > 0)
		{
			InsertString(ref hash);
			_bufPos++;
		}
	}

	internal bool GetNextSymbolOrMatch(Match match)
	{
		uint hash = HashValue(0u, _window[_bufPos]);
		hash = HashValue(hash, _window[_bufPos + 1]);
		int matchPos = 0;
		int num;
		if (_bufEnd - _bufPos <= 3)
		{
			num = 0;
		}
		else
		{
			int num2 = (int)InsertString(ref hash);
			if (num2 != 0)
			{
				num = FindMatch(num2, out matchPos, 32, 32);
				if (_bufPos + num > _bufEnd)
				{
					num = _bufEnd - _bufPos;
				}
			}
			else
			{
				num = 0;
			}
		}
		if (num < 3)
		{
			match.State = MatchState.HasSymbol;
			match.Symbol = _window[_bufPos];
			_bufPos++;
		}
		else
		{
			_bufPos++;
			if (num <= 6)
			{
				int matchPos2 = 0;
				int num3 = (int)InsertString(ref hash);
				int num4;
				if (num3 != 0)
				{
					num4 = FindMatch(num3, out matchPos2, (num < 4) ? 32 : 8, 32);
					if (_bufPos + num4 > _bufEnd)
					{
						num4 = _bufEnd - _bufPos;
					}
				}
				else
				{
					num4 = 0;
				}
				if (num4 > num)
				{
					match.State = MatchState.HasSymbolAndMatch;
					match.Symbol = _window[_bufPos - 1];
					match.Position = matchPos2;
					match.Length = num4;
					_bufPos++;
					num = num4;
					InsertStrings(ref hash, num);
				}
				else
				{
					match.State = MatchState.HasMatch;
					match.Position = matchPos;
					match.Length = num;
					num--;
					_bufPos++;
					InsertStrings(ref hash, num);
				}
			}
			else
			{
				match.State = MatchState.HasMatch;
				match.Position = matchPos;
				match.Length = num;
				InsertStrings(ref hash, num);
			}
		}
		if (_bufPos == 16384)
		{
			MoveWindows();
		}
		return true;
	}

	private int FindMatch(int search, out int matchPos, int searchDepth, int niceLength)
	{
		int num = 0;
		int num2 = 0;
		int num3 = _bufPos - 8192;
		byte b = _window[_bufPos];
		while (search > num3)
		{
			if (_window[search + num] == b)
			{
				int i;
				for (i = 0; i < 258 && _window[_bufPos + i] == _window[search + i]; i++)
				{
				}
				if (i > num)
				{
					num = i;
					num2 = search;
					if (i > 32)
					{
						break;
					}
					b = _window[_bufPos + i];
				}
			}
			if (--searchDepth == 0)
			{
				break;
			}
			search = _prev[search & 0x1FFF];
		}
		matchPos = _bufPos - num2 - 1;
		if (num == 3 && matchPos >= 16384)
		{
			return 0;
		}
		return num;
	}

	[Conditional("DEBUG")]
	private void DebugAssertVerifyHashes()
	{
	}

	[Conditional("DEBUG")]
	private void DebugAssertRecalculatedHashesAreEqual(int position1, int position2, string message = "")
	{
	}
}
