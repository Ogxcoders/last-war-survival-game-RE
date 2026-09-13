namespace System.Xml;

internal class BinHexDecoder : IncrementalReadDecoder
{
	private byte[] buffer;

	private int startIndex;

	private int curIndex;

	private int endIndex;

	private bool hasHalfByteCached;

	private byte cachedHalfByte;

	internal override int DecodedCount => curIndex - startIndex;

	internal override bool IsFull => curIndex == endIndex;

	internal unsafe override int Decode(char[] chars, int startPos, int len)
	{
		if (chars == null)
		{
			throw new ArgumentNullException("chars");
		}
		if (len < 0)
		{
			throw new ArgumentOutOfRangeException("len");
		}
		if (startPos < 0)
		{
			throw new ArgumentOutOfRangeException("startPos");
		}
		if (chars.Length - startPos < len)
		{
			throw new ArgumentOutOfRangeException("len");
		}
		if (len == 0)
		{
			return 0;
		}
		int charsDecoded;
		int bytesDecoded;
		fixed (char* ptr = &chars[startPos])
		{
			fixed (byte* ptr2 = &buffer[curIndex])
			{
				Decode(ptr, ptr + len, ptr2, ptr2 + (endIndex - curIndex), ref hasHalfByteCached, ref cachedHalfByte, out charsDecoded, out bytesDecoded);
			}
		}
		curIndex += bytesDecoded;
		return charsDecoded;
	}

	internal unsafe override int Decode(string str, int startPos, int len)
	{
		if (str == null)
		{
			throw new ArgumentNullException("str");
		}
		if (len < 0)
		{
			throw new ArgumentOutOfRangeException("len");
		}
		if (startPos < 0)
		{
			throw new ArgumentOutOfRangeException("startPos");
		}
		if (str.Length - startPos < len)
		{
			throw new ArgumentOutOfRangeException("len");
		}
		if (len == 0)
		{
			return 0;
		}
		int charsDecoded;
		int bytesDecoded;
		fixed (char* ptr = str)
		{
			fixed (byte* ptr2 = &buffer[curIndex])
			{
				Decode(ptr + startPos, ptr + startPos + len, ptr2, ptr2 + (endIndex - curIndex), ref hasHalfByteCached, ref cachedHalfByte, out charsDecoded, out bytesDecoded);
			}
		}
		curIndex += bytesDecoded;
		return charsDecoded;
	}

	internal override void Reset()
	{
		hasHalfByteCached = false;
		cachedHalfByte = 0;
	}

	internal override void SetNextOutputBuffer(Array buffer, int index, int count)
	{
		this.buffer = (byte[])buffer;
		startIndex = index;
		curIndex = index;
		endIndex = index + count;
	}

	public unsafe static byte[] Decode(char[] chars, bool allowOddChars)
	{
		if (chars == null)
		{
			throw new ArgumentNullException("chars");
		}
		int num = chars.Length;
		if (num == 0)
		{
			return new byte[0];
		}
		byte[] array = new byte[(num + 1) / 2];
		bool flag = false;
		byte b = 0;
		int bytesDecoded;
		fixed (char* ptr = &chars[0])
		{
			fixed (byte* ptr2 = &array[0])
			{
				Decode(ptr, ptr + num, ptr2, ptr2 + array.Length, ref flag, ref b, out var _, out bytesDecoded);
			}
		}
		if (flag && !allowOddChars)
		{
			throw new XmlException("'{0}' is not a valid BinHex text sequence. The sequence must contain an even number of characters.", new string(chars));
		}
		if (bytesDecoded < array.Length)
		{
			byte[] array2 = new byte[bytesDecoded];
			Array.Copy(array, 0, array2, 0, bytesDecoded);
			array = array2;
		}
		return array;
	}

	private unsafe static void Decode(char* pChars, char* pCharsEndPos, byte* pBytes, byte* pBytesEndPos, ref bool hasHalfByteCached, ref byte cachedHalfByte, out int charsDecoded, out int bytesDecoded)
	{
		char* ptr = pChars;
		byte* ptr2 = pBytes;
		XmlCharType instance = XmlCharType.Instance;
		while (ptr < pCharsEndPos && ptr2 < pBytesEndPos)
		{
			char c = *(ptr++);
			byte b;
			if (c >= 'a' && c <= 'f')
			{
				b = (byte)(c - 97 + 10);
			}
			else if (c >= 'A' && c <= 'F')
			{
				b = (byte)(c - 65 + 10);
			}
			else
			{
				if (c < '0' || c > '9')
				{
					if ((instance.charProperties[(uint)c] & 1) == 0)
					{
						throw new XmlException("'{0}' is not a valid BinHex text sequence.", new string(pChars, 0, (int)(pCharsEndPos - pChars)));
					}
					continue;
				}
				b = (byte)(c - 48);
			}
			if (hasHalfByteCached)
			{
				*(ptr2++) = (byte)((cachedHalfByte << 4) + b);
				hasHalfByteCached = false;
			}
			else
			{
				cachedHalfByte = b;
				hasHalfByteCached = true;
			}
		}
		bytesDecoded = (int)(ptr2 - pBytes);
		charsDecoded = (int)(ptr - pChars);
	}
}
