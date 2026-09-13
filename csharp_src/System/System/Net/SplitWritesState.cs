namespace System.Net;

internal class SplitWritesState
{
	private const int c_SplitEncryptedBuffersSize = 65536;

	private BufferOffsetSize[] _UserBuffers;

	private int _Index;

	private int _LastBufferConsumed;

	private BufferOffsetSize[] _RealBuffers;

	internal bool IsDone
	{
		get
		{
			if (_LastBufferConsumed != 0)
			{
				return false;
			}
			for (int i = _Index; i < _UserBuffers.Length; i++)
			{
				if (_UserBuffers[i].Size != 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	internal SplitWritesState(BufferOffsetSize[] buffers)
	{
		_UserBuffers = buffers;
		_LastBufferConsumed = 0;
		_Index = 0;
		_RealBuffers = null;
	}

	internal BufferOffsetSize[] GetNextBuffers()
	{
		int i = _Index;
		int num = 0;
		int num2 = 0;
		int num3 = _LastBufferConsumed;
		while (_Index < _UserBuffers.Length)
		{
			num2 = _UserBuffers[_Index].Size - _LastBufferConsumed;
			num += num2;
			if (num > 65536)
			{
				num2 -= num - 65536;
				num = 65536;
				break;
			}
			num2 = 0;
			_LastBufferConsumed = 0;
			_Index++;
		}
		if (num == 0)
		{
			return null;
		}
		if (num3 == 0 && i == 0 && _Index == _UserBuffers.Length)
		{
			return _UserBuffers;
		}
		int num4 = ((num2 == 0) ? (_Index - i) : (_Index - i + 1));
		if (_RealBuffers == null || _RealBuffers.Length != num4)
		{
			_RealBuffers = new BufferOffsetSize[num4];
		}
		int num5 = 0;
		for (; i < _Index; i++)
		{
			_RealBuffers[num5++] = new BufferOffsetSize(_UserBuffers[i].Buffer, _UserBuffers[i].Offset + num3, _UserBuffers[i].Size - num3, copyBuffer: false);
			num3 = 0;
		}
		if (num2 != 0)
		{
			_RealBuffers[num5] = new BufferOffsetSize(_UserBuffers[i].Buffer, _UserBuffers[i].Offset + _LastBufferConsumed, num2, copyBuffer: false);
			if ((_LastBufferConsumed += num2) == _UserBuffers[_Index].Size)
			{
				_Index++;
				_LastBufferConsumed = 0;
			}
		}
		return _RealBuffers;
	}
}
