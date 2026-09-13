namespace System.Xml.Schema;

internal sealed class BitSet
{
	private const int bitSlotShift = 5;

	private const int bitSlotMask = 31;

	private int count;

	private uint[] bits;

	public int Count => count;

	public bool this[int index] => Get(index);

	public bool IsEmpty
	{
		get
		{
			uint num = 0u;
			for (int i = 0; i < bits.Length; i++)
			{
				num |= bits[i];
			}
			return num == 0;
		}
	}

	private BitSet()
	{
	}

	public BitSet(int count)
	{
		this.count = count;
		bits = new uint[Subscript(count + 31)];
	}

	public void Clear()
	{
		int num = bits.Length;
		while (num-- > 0)
		{
			bits[num] = 0u;
		}
	}

	public void Clear(int index)
	{
		int num = Subscript(index);
		EnsureLength(num + 1);
		bits[num] &= (uint)(~(1 << index));
	}

	public void Set(int index)
	{
		int num = Subscript(index);
		EnsureLength(num + 1);
		bits[num] |= (uint)(1 << index);
	}

	public bool Get(int index)
	{
		bool result = false;
		if (index < count)
		{
			int num = Subscript(index);
			result = (bits[num] & (1 << index)) != 0;
		}
		return result;
	}

	public int NextSet(int startFrom)
	{
		int num = startFrom + 1;
		if (num == count)
		{
			return -1;
		}
		int num2 = Subscript(num);
		num &= 0x1F;
		uint num3;
		for (num3 = bits[num2] >> num; num3 == 0; num3 = bits[num2])
		{
			if (++num2 == bits.Length)
			{
				return -1;
			}
			num = 0;
		}
		while ((num3 & 1) == 0)
		{
			num3 >>= 1;
			num++;
		}
		return (num2 << 5) + num;
	}

	public void And(BitSet other)
	{
		if (this != other)
		{
			int num = bits.Length;
			int num2 = other.bits.Length;
			int i = ((num > num2) ? num2 : num);
			int num3 = i;
			while (num3-- > 0)
			{
				bits[num3] &= other.bits[num3];
			}
			for (; i < num; i++)
			{
				bits[i] = 0u;
			}
		}
	}

	public void Or(BitSet other)
	{
		if (this != other)
		{
			int num = other.bits.Length;
			EnsureLength(num);
			int num2 = num;
			while (num2-- > 0)
			{
				bits[num2] |= other.bits[num2];
			}
		}
	}

	public override int GetHashCode()
	{
		int num = 1234;
		int num2 = bits.Length;
		while (--num2 >= 0)
		{
			num ^= (int)bits[num2] * (num2 + 1);
		}
		return num ^ num;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this == obj)
			{
				return true;
			}
			BitSet bitSet = (BitSet)obj;
			int num = bits.Length;
			int num2 = bitSet.bits.Length;
			int num3 = ((num > num2) ? num2 : num);
			int num4 = num3;
			while (num4-- > 0)
			{
				if (bits[num4] != bitSet.bits[num4])
				{
					return false;
				}
			}
			if (num > num3)
			{
				int num5 = num;
				while (num5-- > num3)
				{
					if (bits[num5] != 0)
					{
						return false;
					}
				}
			}
			else
			{
				int num6 = num2;
				while (num6-- > num3)
				{
					if (bitSet.bits[num6] != 0)
					{
						return false;
					}
				}
			}
			return true;
		}
		return false;
	}

	public BitSet Clone()
	{
		return new BitSet
		{
			count = count,
			bits = (uint[])bits.Clone()
		};
	}

	public bool Intersects(BitSet other)
	{
		int num = Math.Min(bits.Length, other.bits.Length);
		while (--num >= 0)
		{
			if ((bits[num] & other.bits[num]) != 0)
			{
				return true;
			}
		}
		return false;
	}

	private int Subscript(int bitIndex)
	{
		return bitIndex >> 5;
	}

	private void EnsureLength(int nRequiredLength)
	{
		if (nRequiredLength > bits.Length)
		{
			int num = 2 * bits.Length;
			if (num < nRequiredLength)
			{
				num = nRequiredLength;
			}
			uint[] destinationArray = new uint[num];
			Array.Copy(bits, destinationArray, bits.Length);
			bits = destinationArray;
		}
	}
}
