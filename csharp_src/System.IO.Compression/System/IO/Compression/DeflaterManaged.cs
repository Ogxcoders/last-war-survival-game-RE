namespace System.IO.Compression;

internal sealed class DeflaterManaged : IDisposable
{
	private enum DeflaterState
	{
		NotStarted,
		SlowDownForIncompressible1,
		SlowDownForIncompressible2,
		StartingSmallData,
		CompressThenCheck,
		CheckingForIncompressible,
		HandlingSmallData
	}

	private const int MinBlockSize = 256;

	private const int MaxHeaderFooterGoo = 120;

	private const int CleanCopySize = 8072;

	private const double BadCompressionThreshold = 1.0;

	private readonly FastEncoder _deflateEncoder;

	private readonly CopyEncoder _copyEncoder;

	private readonly DeflateInput _input;

	private readonly OutputBuffer _output;

	private DeflaterState _processingState;

	private DeflateInput _inputFromHistory;

	internal DeflaterManaged()
	{
		_deflateEncoder = new FastEncoder();
		_copyEncoder = new CopyEncoder();
		_input = new DeflateInput();
		_output = new OutputBuffer();
		_processingState = DeflaterState.NotStarted;
	}

	internal bool NeedsInput()
	{
		if (_input.Count == 0)
		{
			return _deflateEncoder.BytesInHistory == 0;
		}
		return false;
	}

	internal void SetInput(byte[] inputBuffer, int startIndex, int count)
	{
		_input.Buffer = inputBuffer;
		_input.Count = count;
		_input.StartIndex = startIndex;
		if (count > 0 && count < 256)
		{
			switch (_processingState)
			{
			case DeflaterState.NotStarted:
			case DeflaterState.CheckingForIncompressible:
				_processingState = DeflaterState.StartingSmallData;
				break;
			case DeflaterState.CompressThenCheck:
				_processingState = DeflaterState.HandlingSmallData;
				break;
			}
		}
	}

	internal int GetDeflateOutput(byte[] outputBuffer)
	{
		_output.UpdateBuffer(outputBuffer);
		switch (_processingState)
		{
		case DeflaterState.NotStarted:
		{
			DeflateInput.InputState state3 = _input.DumpState();
			OutputBuffer.BufferState state4 = _output.DumpState();
			_deflateEncoder.GetBlockHeader(_output);
			_deflateEncoder.GetCompressedData(_input, _output);
			if (!UseCompressed(_deflateEncoder.LastCompressionRatio))
			{
				_input.RestoreState(state3);
				_output.RestoreState(state4);
				_copyEncoder.GetBlock(_input, _output, isFinal: false);
				FlushInputWindows();
				_processingState = DeflaterState.CheckingForIncompressible;
			}
			else
			{
				_processingState = DeflaterState.CompressThenCheck;
			}
			break;
		}
		case DeflaterState.CompressThenCheck:
			_deflateEncoder.GetCompressedData(_input, _output);
			if (!UseCompressed(_deflateEncoder.LastCompressionRatio))
			{
				_processingState = DeflaterState.SlowDownForIncompressible1;
				_inputFromHistory = _deflateEncoder.UnprocessedInput;
			}
			break;
		case DeflaterState.SlowDownForIncompressible1:
			_deflateEncoder.GetBlockFooter(_output);
			_processingState = DeflaterState.SlowDownForIncompressible2;
			goto case DeflaterState.SlowDownForIncompressible2;
		case DeflaterState.SlowDownForIncompressible2:
			if (_inputFromHistory.Count > 0)
			{
				_copyEncoder.GetBlock(_inputFromHistory, _output, isFinal: false);
			}
			if (_inputFromHistory.Count == 0)
			{
				_deflateEncoder.FlushInput();
				_processingState = DeflaterState.CheckingForIncompressible;
			}
			break;
		case DeflaterState.CheckingForIncompressible:
		{
			DeflateInput.InputState state = _input.DumpState();
			OutputBuffer.BufferState state2 = _output.DumpState();
			_deflateEncoder.GetBlock(_input, _output, 8072);
			if (!UseCompressed(_deflateEncoder.LastCompressionRatio))
			{
				_input.RestoreState(state);
				_output.RestoreState(state2);
				_copyEncoder.GetBlock(_input, _output, isFinal: false);
				FlushInputWindows();
			}
			break;
		}
		case DeflaterState.StartingSmallData:
			_deflateEncoder.GetBlockHeader(_output);
			_processingState = DeflaterState.HandlingSmallData;
			goto case DeflaterState.HandlingSmallData;
		case DeflaterState.HandlingSmallData:
			_deflateEncoder.GetCompressedData(_input, _output);
			break;
		}
		return _output.BytesWritten;
	}

	internal bool Finish(byte[] outputBuffer, out int bytesRead)
	{
		if (_processingState == DeflaterState.NotStarted)
		{
			bytesRead = 0;
			return true;
		}
		_output.UpdateBuffer(outputBuffer);
		if (_processingState == DeflaterState.CompressThenCheck || _processingState == DeflaterState.HandlingSmallData || _processingState == DeflaterState.SlowDownForIncompressible1)
		{
			_deflateEncoder.GetBlockFooter(_output);
		}
		WriteFinal();
		bytesRead = _output.BytesWritten;
		return true;
	}

	private bool UseCompressed(double ratio)
	{
		return ratio <= 1.0;
	}

	private void FlushInputWindows()
	{
		_deflateEncoder.FlushInput();
	}

	private void WriteFinal()
	{
		_copyEncoder.GetBlock(null, _output, isFinal: true);
	}

	public void Dispose()
	{
	}
}
