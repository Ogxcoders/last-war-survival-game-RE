using System.Threading;

namespace System.Collections;

[Serializable]
public sealed class BitArray : ICollection, IEnumerable, ICloneable
{
	[Serializable]
	private class BitArrayEnumeratorSimple : IEnumerator, ICloneable
	{
		private BitArray bitarray;

		private int index;

		private int version;

		private bool currentElement;

		public virtual object Current
		{
			get
			{
				if (index == -1)
				{
					throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
				}
				if (index >= ((ICollection)bitarray).Count)
				{
					throw new InvalidOperationException("Enumeration already finished.");
				}
				return currentElement;
			}
		}

		internal BitArrayEnumeratorSimple(BitArray bitarray)
		{
			this.bitarray = bitarray;
			index = -1;
			version = bitarray._version;
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public virtual bool MoveNext()
		{
			ICollection collection = bitarray;
			if (version != bitarray._version)
			{
				throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
			}
			if (index < collection.Count - 1)
			{
				index++;
				currentElement = bitarray.Get(index);
				return true;
			}
			index = collection.Count;
			return false;
		}

		public void Reset()
		{
			if (version != bitarray._version)
			{
				throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
			}
			index = -1;
		}
	}

	private int[] m_array;

	private int m_length;

	private int _version;

	[NonSerialized]
	private object _syncRoot;

	private const int _ShrinkThreshold = 256;

	private const int BitsPerInt32 = 32;

	private const int BytesPerInt32 = 4;

	private const int BitsPerByte = 8;

	public bool this[int index]
	{
		get
		{
			return Get(index);
		}
		set
		{
			Set(index, value);
		}
	}

	public int Length
	{
		get
		{
			return m_length;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", value, "Non-negative number required.");
			}
			int arrayLength = GetArrayLength(value, 32);
			if (arrayLength > m_array.Length || arrayLength + 256 < m_array.Length)
			{
				Array.Resize(ref m_array, arrayLength);
			}
			if (value > m_length)
			{
				int num = GetArrayLength(m_length, 32) - 1;
				int num2 = m_length % 32;
				if (num2 > 0)
				{
					m_array[num] &= (1 << num2) - 1;
				}
				Array.Clear(m_array, num + 1, arrayLength - num - 1);
			}
			m_length = value;
			_version++;
		}
	}

	public int Count => m_length;

	public object SyncRoot
	{
		get
		{
			if (_syncRoot == null)
			{
				Interlocked.CompareExchange<object>(ref _syncRoot, new object(), (object)null);
			}
			return _syncRoot;
		}
	}

	public bool IsSynchronized => false;

	public bool IsReadOnly => false;

	public BitArray(int length)
		: this(length, defaultValue: false)
	{
	}

	public BitArray(int length, bool defaultValue)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length", length, "Non-negative number required.");
		}
		m_array = new int[GetArrayLength(length, 32)];
		m_length = length;
		int num = (defaultValue ? (-1) : 0);
		for (int i = 0; i < m_array.Length; i++)
		{
			m_array[i] = num;
		}
		_version = 0;
	}

	public BitArray(byte[] bytes)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (bytes.Length > 268435455)
		{
			throw new ArgumentException(SR.Format("The input array length must not exceed Int32.MaxValue / {0}. Otherwise BitArray.Length would exceed Int32.MaxValue.", 8), "bytes");
		}
		m_array = new int[GetArrayLength(bytes.Length, 4)];
		m_length = bytes.Length * 8;
		int num = 0;
		int i;
		for (i = 0; bytes.Length - i >= 4; i += 4)
		{
			m_array[num++] = (bytes[i] & 0xFF) | ((bytes[i + 1] & 0xFF) << 8) | ((bytes[i + 2] & 0xFF) << 16) | ((bytes[i + 3] & 0xFF) << 24);
		}
		switch (bytes.Length - i)
		{
		case 3:
			m_array[num] = (bytes[i + 2] & 0xFF) << 16;
			goto case 2;
		case 2:
			m_array[num] |= (bytes[i + 1] & 0xFF) << 8;
			goto case 1;
		case 1:
			m_array[num] |= bytes[i] & 0xFF;
			break;
		}
		_version = 0;
	}

	public BitArray(bool[] values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		m_array = new int[GetArrayLength(values.Length, 32)];
		m_length = values.Length;
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i])
			{
				m_array[i / 32] |= 1 << i % 32;
			}
		}
		_version = 0;
	}

	public BitArray(int[] values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (values.Length > 67108863)
		{
			throw new ArgumentException(SR.Format("The input array length must not exceed Int32.MaxValue / {0}. Otherwise BitArray.Length would exceed Int32.MaxValue.", 32), "values");
		}
		m_array = new int[values.Length];
		Array.Copy(values, 0, m_array, 0, values.Length);
		m_length = values.Length * 32;
		_version = 0;
	}

	public BitArray(BitArray bits)
	{
		if (bits == null)
		{
			throw new ArgumentNullException("bits");
		}
		int arrayLength = GetArrayLength(bits.m_length, 32);
		m_array = new int[arrayLength];
		Array.Copy(bits.m_array, 0, m_array, 0, arrayLength);
		m_length = bits.m_length;
		_version = bits._version;
	}

	public bool Get(int index)
	{
		if (index < 0 || index >= Length)
		{
			throw new ArgumentOutOfRangeException("index", index, "Index was out of range. Must be non-negative and less than the size of the collection.");
		}
		return (m_array[index / 32] & (1 << index % 32)) != 0;
	}

	public void Set(int index, bool value)
	{
		if (index < 0 || index >= Length)
		{
			throw new ArgumentOutOfRangeException("index", index, "Index was out of range. Must be non-negative and less than the size of the collection.");
		}
		if (value)
		{
			m_array[index / 32] |= 1 << index % 32;
		}
		else
		{
			m_array[index / 32] &= ~(1 << index % 32);
		}
		_version++;
	}

	public void SetAll(bool value)
	{
		int num = (value ? (-1) : 0);
		int arrayLength = GetArrayLength(m_length, 32);
		for (int i = 0; i < arrayLength; i++)
		{
			m_array[i] = num;
		}
		_version++;
	}

	public BitArray And(BitArray value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (Length != value.Length)
		{
			throw new ArgumentException("Array lengths must be the same.");
		}
		int arrayLength = GetArrayLength(m_length, 32);
		for (int i = 0; i < arrayLength; i++)
		{
			m_array[i] &= value.m_array[i];
		}
		_version++;
		return this;
	}

	public BitArray Or(BitArray value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (Length != value.Length)
		{
			throw new ArgumentException("Array lengths must be the same.");
		}
		int arrayLength = GetArrayLength(m_length, 32);
		for (int i = 0; i < arrayLength; i++)
		{
			m_array[i] |= value.m_array[i];
		}
		_version++;
		return this;
	}

	public BitArray Xor(BitArray value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (Length != value.Length)
		{
			throw new ArgumentException("Array lengths must be the same.");
		}
		int arrayLength = GetArrayLength(m_length, 32);
		for (int i = 0; i < arrayLength; i++)
		{
			m_array[i] ^= value.m_array[i];
		}
		_version++;
		return this;
	}

	public BitArray Not()
	{
		int arrayLength = GetArrayLength(m_length, 32);
		for (int i = 0; i < arrayLength; i++)
		{
			m_array[i] = ~m_array[i];
		}
		_version++;
		return this;
	}

	public BitArray RightShift(int count)
	{
		if (count <= 0)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "Non-negative number required.");
			}
			_version++;
			return this;
		}
		int num = 0;
		int arrayLength = GetArrayLength(m_length, 32);
		if (count < m_length)
		{
			int num2 = count / 32;
			int num3 = count - num2 * 32;
			if (num3 == 0)
			{
				uint num4 = uint.MaxValue >> 32 - m_length % 32;
				m_array[arrayLength - 1] &= (int)num4;
				Array.Copy(m_array, num2, m_array, 0, arrayLength - num2);
				num = arrayLength - num2;
			}
			else
			{
				int num5 = arrayLength - 1;
				while (num2 < num5)
				{
					uint num6 = (uint)m_array[num2] >> num3;
					int num7 = m_array[++num2] << 32 - num3;
					m_array[num++] = num7 | (int)num6;
				}
				uint num8 = uint.MaxValue >> 32 - m_length % 32;
				num8 &= (uint)m_array[num2];
				m_array[num++] = (int)(num8 >> num3);
			}
		}
		Array.Clear(m_array, num, arrayLength - num);
		_version++;
		return this;
	}

	public BitArray LeftShift(int count)
	{
		if (count <= 0)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "Non-negative number required.");
			}
			_version++;
			return this;
		}
		int num2;
		if (count < m_length)
		{
			int num = (m_length - 1) / 32;
			num2 = count / 32;
			int num3 = count - num2 * 32;
			if (num3 == 0)
			{
				Array.Copy(m_array, 0, m_array, num2, num + 1 - num2);
			}
			else
			{
				int num4 = num - num2;
				while (num4 > 0)
				{
					int num5 = m_array[num4] << num3;
					uint num6 = (uint)m_array[--num4] >> 32 - num3;
					m_array[num] = num5 | (int)num6;
					num--;
				}
				m_array[num] = m_array[num4] << num3;
			}
		}
		else
		{
			num2 = GetArrayLength(m_length, 32);
		}
		Array.Clear(m_array, 0, num2);
		_version++;
		return this;
	}

	public void CopyTo(Array array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", index, "Non-negative number required.");
		}
		if (array.Rank != 1)
		{
			throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", "array");
		}
		if (array is int[] array2)
		{
			int num = GetArrayLength(m_length, 32) - 1;
			int num2 = m_length % 32;
			if (num2 == 0)
			{
				Array.Copy(m_array, 0, array2, index, GetArrayLength(m_length, 32));
				return;
			}
			Array.Copy(m_array, 0, array2, index, GetArrayLength(m_length, 32) - 1);
			array2[index + num] = m_array[num] & ((1 << num2) - 1);
		}
		else if (array is byte[])
		{
			int num3 = m_length % 8;
			int num4 = GetArrayLength(m_length, 8);
			if (array.Length - index < num4)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (num3 > 0)
			{
				num4--;
			}
			byte[] array3 = (byte[])array;
			for (int i = 0; i < num4; i++)
			{
				array3[index + i] = (byte)((m_array[i / 4] >> i % 4 * 8) & 0xFF);
			}
			if (num3 > 0)
			{
				int num5 = num4;
				array3[index + num5] = (byte)((m_array[num5 / 4] >> num5 % 4 * 8) & ((1 << num3) - 1));
			}
		}
		else
		{
			if (!(array is bool[]))
			{
				throw new ArgumentException("Only supported array types for CopyTo on BitArrays are Boolean[], Int32[] and Byte[].", "array");
			}
			if (array.Length - index < m_length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			bool[] array4 = (bool[])array;
			for (int j = 0; j < m_length; j++)
			{
				array4[index + j] = ((m_array[j / 32] >> j % 32) & 1) != 0;
			}
		}
	}

	public object Clone()
	{
		return new BitArray(this);
	}

	public IEnumerator GetEnumerator()
	{
		return new BitArrayEnumeratorSimple(this);
	}

	private static int GetArrayLength(int n, int div)
	{
		if (n <= 0)
		{
			return 0;
		}
		return (n - 1) / div + 1;
	}
}
