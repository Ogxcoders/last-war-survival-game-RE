namespace System.Runtime.Serialization;

internal class BitFlagsGenerator
{
	private int bitCount;

	private byte[] locals;

	public BitFlagsGenerator(int bitCount)
	{
		this.bitCount = bitCount;
		int num = (bitCount + 7) / 8;
		locals = new byte[num];
	}

	public void Store(int bitIndex, bool value)
	{
		if (value)
		{
			locals[GetByteIndex(bitIndex)] |= GetBitValue(bitIndex);
		}
		else
		{
			locals[GetByteIndex(bitIndex)] &= (byte)(~GetBitValue(bitIndex));
		}
	}

	public bool Load(int bitIndex)
	{
		byte num = locals[GetByteIndex(bitIndex)];
		byte bitValue = GetBitValue(bitIndex);
		return (num & bitValue) == bitValue;
	}

	public byte[] LoadArray()
	{
		return (byte[])locals.Clone();
	}

	public int GetLocalCount()
	{
		return locals.Length;
	}

	public int GetBitCount()
	{
		return bitCount;
	}

	public byte GetLocal(int i)
	{
		return locals[i];
	}

	public static bool IsBitSet(byte[] bytes, int bitIndex)
	{
		int byteIndex = GetByteIndex(bitIndex);
		byte bitValue = GetBitValue(bitIndex);
		return (bytes[byteIndex] & bitValue) == bitValue;
	}

	public static void SetBit(byte[] bytes, int bitIndex)
	{
		int byteIndex = GetByteIndex(bitIndex);
		byte bitValue = GetBitValue(bitIndex);
		bytes[byteIndex] |= bitValue;
	}

	private static int GetByteIndex(int bitIndex)
	{
		return bitIndex >> 3;
	}

	private static byte GetBitValue(int bitIndex)
	{
		return (byte)(1 << (bitIndex & 7));
	}
}
