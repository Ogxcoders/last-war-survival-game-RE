namespace System.Xml;

internal class Base64Decoder : IncrementalReadDecoder
{
	private byte[] buffer;

	private int startIndex;

	private int curIndex;

	private int endIndex;

	private int bits;

	private int bitsFilled;

	private static readonly string CharsBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

	private static readonly byte[] MapBase64 = ConstructMapBase64();

	private const int MaxValidChar = 122;

	private const byte Invalid = byte.MaxValue;

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
				Decode(ptr, ptr + len, ptr2, ptr2 + (endIndex - curIndex), out charsDecoded, out bytesDecoded);
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
				Decode(ptr + startPos, ptr + startPos + len, ptr2, ptr2 + (endIndex - curIndex), out charsDecoded, out bytesDecoded);
			}
		}
		curIndex += bytesDecoded;
		return charsDecoded;
	}

	internal override void Reset()
	{
		bitsFilled = 0;
		bits = 0;
	}

	internal override void SetNextOutputBuffer(Array buffer, int index, int count)
	{
		this.buffer = (byte[])buffer;
		startIndex = index;
		curIndex = index;
		endIndex = index + count;
	}

	private static byte[] ConstructMapBase64()
	{
		byte[] array = new byte[123];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = byte.MaxValue;
		}
		for (int j = 0; j < CharsBase64.Length; j++)
		{
			array[(uint)CharsBase64[j]] = (byte)j;
		}
		return array;
	}

	private unsafe void Decode(char* pChars, char* pCharsEndPos, byte* pBytes, byte* pBytesEndPos, out int charsDecoded, out int bytesDecoded)
	{
		byte* ptr = pBytes;
		char* ptr2 = pChars;
		int num = bits;
		int num2 = bitsFilled;
		XmlCharType instance = XmlCharType.Instance;
		while (true)
		{
			if (ptr2 < pCharsEndPos && ptr < pBytesEndPos)
			{
				char c = *ptr2;
				if (c != '=')
				{
					ptr2++;
					if ((instance.charProperties[(uint)c] & 1) != 0)
					{
						continue;
					}
					int num3;
					if (c > 'z' || (num3 = MapBase64[(uint)c]) == 255)
					{
						throw new XmlException("'{0}' is not a valid Base64 text sequence.", new string(pChars, 0, (int)(pCharsEndPos - pChars)));
					}
					num = (num << 6) | num3;
					num2 += 6;
					if (num2 >= 8)
					{
						*(ptr++) = (byte)((num >> num2 - 8) & 0xFF);
						num2 -= 8;
						if (ptr == pBytesEndPos)
						{
							break;
						}
					}
					continue;
				}
			}
			if (ptr2 >= pCharsEndPos || *ptr2 != '=')
			{
				break;
			}
			num2 = 0;
			do
			{
				ptr2++;
			}
			while (ptr2 < pCharsEndPos && *ptr2 == '=');
			if (ptr2 >= pCharsEndPos)
			{
				break;
			}
			do
			{
				if ((instance.charProperties[(uint)(*(ptr2++))] & 1) == 0)
				{
					throw new XmlException("'{0}' is not a valid Base64 text sequence.", new string(pChars, 0, (int)(pCharsEndPos - pChars)));
				}
			}
			while (ptr2 < pCharsEndPos);
			break;
		}
		bits = num;
		bitsFilled = num2;
		bytesDecoded = (int)(ptr - pBytes);
		charsDecoded = (int)(ptr2 - pChars);
	}
}
