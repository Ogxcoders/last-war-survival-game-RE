namespace System.IO.Compression;

internal sealed class DeflateInput
{
	internal readonly struct InputState
	{
		internal readonly int _count;

		internal readonly int _startIndex;

		internal InputState(int count, int startIndex)
		{
			_count = count;
			_startIndex = startIndex;
		}
	}

	internal byte[] Buffer { get; set; }

	internal int Count { get; set; }

	internal int StartIndex { get; set; }

	internal void ConsumeBytes(int n)
	{
		StartIndex += n;
		Count -= n;
	}

	internal InputState DumpState()
	{
		return new InputState(Count, StartIndex);
	}

	internal void RestoreState(InputState state)
	{
		Count = state._count;
		StartIndex = state._startIndex;
	}
}
