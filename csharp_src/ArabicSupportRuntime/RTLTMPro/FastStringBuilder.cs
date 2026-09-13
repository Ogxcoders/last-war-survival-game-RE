using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RTLTMPro;

public class FastStringBuilder
{
	private int length;

	private int[] array;

	private int capacity;

	public int Length
	{
		get
		{
			return length;
		}
		set
		{
			if (value <= length)
			{
				length = value;
			}
		}
	}

	public FastStringBuilder(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		this.capacity = capacity;
		array = new int[capacity];
	}

	public FastStringBuilder(string text)
		: this(text, text.Length)
	{
	}

	public FastStringBuilder(string text, int capacity)
		: this(capacity)
	{
		SetValue(text);
	}

	public static implicit operator string(FastStringBuilder x)
	{
		return x.ToString();
	}

	public static implicit operator FastStringBuilder(string x)
	{
		return new FastStringBuilder(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Get(int index)
	{
		return array[index];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int index, int ch)
	{
		array[index] = ch;
	}

	public void SetValue(string text)
	{
		int num = 0;
		length = text.Length;
		EnsureCapacity(length, keepValues: false);
		for (int i = 0; i < text.Length; i++)
		{
			int num2 = char.ConvertToUtf32(text, i);
			if (num2 > 65535)
			{
				i++;
			}
			array[num++] = num2;
		}
		length = num;
	}

	public void SetValue(FastStringBuilder other)
	{
		EnsureCapacity(other.length, keepValues: false);
		Copy(ref other.array, ref array);
		length = other.length;
	}

	public void Append(int ch)
	{
		length++;
		if (capacity < length)
		{
			EnsureCapacity(length, keepValues: true);
		}
		array[length - 1] = ch;
	}

	public void Append(char ch)
	{
		length++;
		if (capacity < length)
		{
			EnsureCapacity(length, keepValues: true);
		}
		array[length - 1] = ch;
	}

	public void Insert(int pos, FastStringBuilder str, int offset, int count)
	{
		if (str == this)
		{
			throw new InvalidOperationException("You cannot pass the same string builder to insert");
		}
		if (count != 0)
		{
			length += count;
			EnsureCapacity(length, keepValues: true);
			for (int num = length - count - 1; num >= pos; num--)
			{
				array[num + count] = array[num];
			}
			for (int i = 0; i < count; i++)
			{
				array[pos + i] = str.array[offset + i];
			}
		}
	}

	public void Insert(int pos, FastStringBuilder str)
	{
		Insert(pos, str, 0, str.length);
	}

	public void Insert(int pos, int ch)
	{
		length++;
		EnsureCapacity(length, keepValues: true);
		for (int num = length - 2; num >= pos; num--)
		{
			array[num + 1] = array[num];
		}
		array[pos] = ch;
	}

	public void RemoveAll(int character)
	{
		int num = 0;
		for (int i = 0; i < length; i++)
		{
			if (array[i] != character)
			{
				array[num] = array[i];
				num++;
			}
		}
		length = num;
	}

	public void Remove(int start, int length)
	{
		for (int i = start; i < this.length - length; i++)
		{
			array[i] = array[i + length];
		}
		this.length -= length;
	}

	public void Reverse(int startIndex, int length)
	{
		for (int i = 0; i < length / 2; i++)
		{
			int num = startIndex + i;
			int num2 = startIndex + length - i - 1;
			int num3 = array[num];
			int num4 = array[num2];
			array[num] = num4;
			array[num2] = num3;
		}
	}

	public void Reverse()
	{
		Reverse(0, length);
	}

	public void Substring(FastStringBuilder output, int start, int length)
	{
		output.length = 0;
		for (int i = 0; i < length; i++)
		{
			output.Append(array[start + i]);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append(char.ConvertFromUtf32(array[i]));
		}
		return stringBuilder.ToString();
	}

	public string ToDebugString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append("\\");
			stringBuilder.Append(array[i].ToString("X"));
		}
		return stringBuilder.ToString();
	}

	public void Replace(int oldChar, int newChar)
	{
		for (int i = 0; i < length; i++)
		{
			if (array[i] == oldChar)
			{
				array[i] = newChar;
			}
		}
	}

	public void Replace(FastStringBuilder oldStr, FastStringBuilder newStr)
	{
		for (int i = 0; i < length; i++)
		{
			bool flag = true;
			for (int j = 0; j < oldStr.Length; j++)
			{
				if (array[i + j] != oldStr.Get(j))
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				continue;
			}
			if (oldStr.Length == newStr.Length)
			{
				for (int k = 0; k < oldStr.Length; k++)
				{
					array[i + k] = newStr.Get(k);
				}
			}
			else if (oldStr.Length < newStr.Length)
			{
				int num = newStr.Length - oldStr.Length;
				length += num;
				EnsureCapacity(length, keepValues: true);
				for (int num2 = length - num - 1; num2 >= i + oldStr.Length; num2--)
				{
					array[num2 + num] = array[num2];
				}
				for (int l = 0; l < newStr.Length; l++)
				{
					array[i + l] = newStr.Get(l);
				}
			}
			else
			{
				int num3 = oldStr.Length - newStr.Length;
				for (int m = i + num3; m < length - num3; m++)
				{
					array[m] = array[m + num3];
				}
				for (int n = 0; n < newStr.Length; n++)
				{
					array[i + n] = newStr.Get(n);
				}
				length -= num3;
			}
			i += newStr.Length;
		}
	}

	public void Clear()
	{
		length = 0;
	}

	private void EnsureCapacity(int cap, bool keepValues)
	{
		if (capacity < cap)
		{
			if (capacity == 0)
			{
				capacity = 1;
			}
			while (capacity < cap)
			{
				capacity *= 2;
			}
			if (keepValues)
			{
				int[] dst = new int[capacity];
				Copy(ref array, ref dst);
				array = dst;
			}
			else
			{
				array = new int[capacity];
			}
		}
	}

	private static void Copy(ref int[] src, ref int[] dst)
	{
		if (dst.Length < src.Length)
		{
			dst = new int[src.Length];
		}
		for (int i = 0; i < src.Length; i++)
		{
			dst[i] = src[i];
		}
	}
}
