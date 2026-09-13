namespace System.Xml;

internal class IncrementalReadCharsDecoder : IncrementalReadDecoder
{
	private char[] buffer;

	private int startIndex;

	private int curIndex;

	private int endIndex;

	internal override int DecodedCount => curIndex - startIndex;

	internal override bool IsFull => curIndex == endIndex;

	internal IncrementalReadCharsDecoder()
	{
	}

	internal override int Decode(char[] chars, int startPos, int len)
	{
		int num = endIndex - curIndex;
		if (num > len)
		{
			num = len;
		}
		Buffer.BlockCopy(chars, startPos * 2, buffer, curIndex * 2, num * 2);
		curIndex += num;
		return num;
	}

	internal override int Decode(string str, int startPos, int len)
	{
		int num = endIndex - curIndex;
		if (num > len)
		{
			num = len;
		}
		str.CopyTo(startPos, buffer, curIndex, num);
		curIndex += num;
		return num;
	}

	internal override void Reset()
	{
	}

	internal override void SetNextOutputBuffer(Array buffer, int index, int count)
	{
		this.buffer = (char[])buffer;
		startIndex = index;
		curIndex = index;
		endIndex = index + count;
	}
}
