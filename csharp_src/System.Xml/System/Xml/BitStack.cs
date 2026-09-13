namespace System.Xml;

internal class BitStack
{
	private uint[] bitStack;

	private int stackPos;

	private uint curr;

	public bool IsEmpty => curr == 1;

	public BitStack()
	{
		curr = 1u;
	}

	public void PushBit(bool bit)
	{
		if ((curr & 0x80000000u) != 0)
		{
			PushCurr();
		}
		curr = (curr << 1) | (uint)(bit ? 1 : 0);
	}

	public bool PopBit()
	{
		bool result = (curr & 1) != 0;
		curr >>= 1;
		if (curr == 1)
		{
			PopCurr();
		}
		return result;
	}

	public bool PeekBit()
	{
		return (curr & 1) != 0;
	}

	private void PushCurr()
	{
		if (bitStack == null)
		{
			bitStack = new uint[16];
		}
		bitStack[stackPos++] = curr;
		curr = 1u;
		int num = bitStack.Length;
		if (stackPos >= num)
		{
			uint[] destinationArray = new uint[2 * num];
			Array.Copy(bitStack, destinationArray, num);
			bitStack = destinationArray;
		}
	}

	private void PopCurr()
	{
		if (stackPos > 0)
		{
			curr = bitStack[--stackPos];
		}
	}
}
