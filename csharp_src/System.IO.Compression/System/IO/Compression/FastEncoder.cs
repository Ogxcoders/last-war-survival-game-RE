namespace System.IO.Compression;

internal sealed class FastEncoder
{
	private readonly FastEncoderWindow _inputWindow;

	private readonly Match _currentMatch;

	private double _lastCompressionRatio;

	internal int BytesInHistory => _inputWindow.BytesAvailable;

	internal DeflateInput UnprocessedInput => _inputWindow.UnprocessedInput;

	internal double LastCompressionRatio => _lastCompressionRatio;

	public FastEncoder()
	{
		_inputWindow = new FastEncoderWindow();
		_currentMatch = new Match();
	}

	internal void FlushInput()
	{
		_inputWindow.FlushWindow();
	}

	internal void GetBlock(DeflateInput input, OutputBuffer output, int maxBytesToCopy)
	{
		WriteDeflatePreamble(output);
		GetCompressedOutput(input, output, maxBytesToCopy);
		WriteEndOfBlock(output);
	}

	internal void GetCompressedData(DeflateInput input, OutputBuffer output)
	{
		GetCompressedOutput(input, output, -1);
	}

	internal void GetBlockHeader(OutputBuffer output)
	{
		WriteDeflatePreamble(output);
	}

	internal void GetBlockFooter(OutputBuffer output)
	{
		WriteEndOfBlock(output);
	}

	private void GetCompressedOutput(DeflateInput input, OutputBuffer output, int maxBytesToCopy)
	{
		int bytesWritten = output.BytesWritten;
		int num = 0;
		int num2 = BytesInHistory + input.Count;
		do
		{
			int num3 = ((input.Count < _inputWindow.FreeWindowSpace) ? input.Count : _inputWindow.FreeWindowSpace);
			if (maxBytesToCopy >= 1)
			{
				num3 = Math.Min(num3, maxBytesToCopy - num);
			}
			if (num3 > 0)
			{
				_inputWindow.CopyBytes(input.Buffer, input.StartIndex, num3);
				input.ConsumeBytes(num3);
				num += num3;
			}
			GetCompressedOutput(output);
		}
		while (SafeToWriteTo(output) && InputAvailable(input) && (maxBytesToCopy < 1 || num < maxBytesToCopy));
		int num4 = output.BytesWritten - bytesWritten;
		int num5 = BytesInHistory + input.Count;
		int num6 = num2 - num5;
		if (num4 != 0)
		{
			_lastCompressionRatio = (double)num4 / (double)num6;
		}
	}

	private void GetCompressedOutput(OutputBuffer output)
	{
		while (_inputWindow.BytesAvailable > 0 && SafeToWriteTo(output))
		{
			_inputWindow.GetNextSymbolOrMatch(_currentMatch);
			if (_currentMatch.State == MatchState.HasSymbol)
			{
				WriteChar(_currentMatch.Symbol, output);
				continue;
			}
			if (_currentMatch.State == MatchState.HasMatch)
			{
				WriteMatch(_currentMatch.Length, _currentMatch.Position, output);
				continue;
			}
			WriteChar(_currentMatch.Symbol, output);
			WriteMatch(_currentMatch.Length, _currentMatch.Position, output);
		}
	}

	private bool InputAvailable(DeflateInput input)
	{
		if (input.Count <= 0)
		{
			return BytesInHistory > 0;
		}
		return true;
	}

	private bool SafeToWriteTo(OutputBuffer output)
	{
		return output.FreeBytes > 16;
	}

	private void WriteEndOfBlock(OutputBuffer output)
	{
		uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[256];
		int n = (int)(num & 0x1F);
		output.WriteBits(n, num >> 5);
	}

	internal static void WriteMatch(int matchLen, int matchPos, OutputBuffer output)
	{
		uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[254 + matchLen];
		int num2 = (int)(num & 0x1F);
		if (num2 <= 16)
		{
			output.WriteBits(num2, num >> 5);
		}
		else
		{
			output.WriteBits(16, (num >> 5) & 0xFFFF);
			output.WriteBits(num2 - 16, num >> 21);
		}
		num = FastEncoderStatics.FastEncoderDistanceCodeInfo[FastEncoderStatics.GetSlot(matchPos)];
		output.WriteBits((int)(num & 0xF), num >> 8);
		int num3 = (int)((num >> 4) & 0xF);
		if (num3 != 0)
		{
			output.WriteBits(num3, (uint)matchPos & FastEncoderStatics.BitMask[num3]);
		}
	}

	internal static void WriteChar(byte b, OutputBuffer output)
	{
		uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[b];
		output.WriteBits((int)(num & 0x1F), num >> 5);
	}

	internal static void WriteDeflatePreamble(OutputBuffer output)
	{
		output.WriteBytes(FastEncoderStatics.FastEncoderTreeStructureData, 0, FastEncoderStatics.FastEncoderTreeStructureData.Length);
		output.WriteBits(9, 34u);
	}
}
