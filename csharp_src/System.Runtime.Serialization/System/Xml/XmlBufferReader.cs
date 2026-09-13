using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Xml;

internal class XmlBufferReader
{
	private XmlDictionaryReader reader;

	private Stream stream;

	private byte[] streamBuffer;

	private byte[] buffer;

	private int offsetMin;

	private int offsetMax;

	private IXmlDictionary dictionary;

	private XmlBinaryReaderSession session;

	private byte[] guid;

	private int offset;

	private const int maxBytesPerChar = 3;

	private char[] chars;

	private int windowOffset;

	private int windowOffsetMax;

	private ValueHandle listValue;

	private static byte[] emptyByteArray = new byte[0];

	private static XmlBufferReader empty = new XmlBufferReader(emptyByteArray);

	public static XmlBufferReader Empty => empty;

	public byte[] Buffer => buffer;

	public bool IsStreamed => stream != null;

	public bool EndOfFile
	{
		get
		{
			if (offset == offsetMax)
			{
				return !TryEnsureByte();
			}
			return false;
		}
	}

	public int Offset
	{
		get
		{
			return offset;
		}
		set
		{
			offset = value;
		}
	}

	public XmlBufferReader(XmlDictionaryReader reader)
	{
		this.reader = reader;
	}

	public XmlBufferReader(byte[] buffer)
	{
		reader = null;
		this.buffer = buffer;
	}

	public void SetBuffer(Stream stream, IXmlDictionary dictionary, XmlBinaryReaderSession session)
	{
		if (streamBuffer == null)
		{
			streamBuffer = new byte[128];
		}
		SetBuffer(stream, streamBuffer, 0, 0, dictionary, session);
		windowOffset = 0;
		windowOffsetMax = streamBuffer.Length;
	}

	public void SetBuffer(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlBinaryReaderSession session)
	{
		SetBuffer(null, buffer, offset, count, dictionary, session);
	}

	private void SetBuffer(Stream stream, byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlBinaryReaderSession session)
	{
		this.stream = stream;
		this.buffer = buffer;
		offsetMin = offset;
		this.offset = offset;
		offsetMax = offset + count;
		this.dictionary = dictionary;
		this.session = session;
	}

	public void Close()
	{
		if (streamBuffer != null && streamBuffer.Length > 4096)
		{
			streamBuffer = null;
		}
		if (stream != null)
		{
			stream.Close();
			stream = null;
		}
		buffer = emptyByteArray;
		offset = 0;
		offsetMax = 0;
		windowOffset = 0;
		windowOffsetMax = 0;
		dictionary = null;
		session = null;
	}

	public byte GetByte()
	{
		int num = offset;
		if (num < offsetMax)
		{
			return buffer[num];
		}
		return GetByteHard();
	}

	public void SkipByte()
	{
		Advance(1);
	}

	private byte GetByteHard()
	{
		EnsureByte();
		return buffer[offset];
	}

	public byte[] GetBuffer(int count, out int offset)
	{
		offset = this.offset;
		if (offset <= offsetMax - count)
		{
			return buffer;
		}
		return GetBufferHard(count, out offset);
	}

	public byte[] GetBuffer(int count, out int offset, out int offsetMax)
	{
		offset = this.offset;
		if (offset <= this.offsetMax - count)
		{
			offsetMax = this.offset + count;
		}
		else
		{
			TryEnsureBytes(Math.Min(count, windowOffsetMax - offset));
			offsetMax = this.offsetMax;
		}
		return buffer;
	}

	public byte[] GetBuffer(out int offset, out int offsetMax)
	{
		offset = this.offset;
		offsetMax = this.offsetMax;
		return buffer;
	}

	private byte[] GetBufferHard(int count, out int offset)
	{
		offset = this.offset;
		EnsureBytes(count);
		return buffer;
	}

	private void EnsureByte()
	{
		if (!TryEnsureByte())
		{
			XmlExceptionHelper.ThrowUnexpectedEndOfFile(reader);
		}
	}

	private bool TryEnsureByte()
	{
		if (stream == null)
		{
			return false;
		}
		if (offsetMax >= windowOffsetMax)
		{
			XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(reader, windowOffsetMax - windowOffset);
		}
		if (offsetMax >= buffer.Length)
		{
			return TryEnsureBytes(1);
		}
		int num = stream.ReadByte();
		if (num == -1)
		{
			return false;
		}
		buffer[offsetMax++] = (byte)num;
		return true;
	}

	private void EnsureBytes(int count)
	{
		if (!TryEnsureBytes(count))
		{
			XmlExceptionHelper.ThrowUnexpectedEndOfFile(reader);
		}
	}

	private bool TryEnsureBytes(int count)
	{
		if (stream == null)
		{
			return false;
		}
		if (offset > int.MaxValue - count)
		{
			XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(reader, windowOffsetMax - windowOffset);
		}
		int num = offset + count;
		if (num < offsetMax)
		{
			return true;
		}
		if (num > windowOffsetMax)
		{
			XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(reader, windowOffsetMax - windowOffset);
		}
		if (num > buffer.Length)
		{
			byte[] dst = new byte[Math.Max(num, buffer.Length * 2)];
			System.Buffer.BlockCopy(buffer, 0, dst, 0, offsetMax);
			buffer = dst;
			streamBuffer = dst;
		}
		int num2 = num - offsetMax;
		while (num2 > 0)
		{
			int num3 = stream.Read(buffer, offsetMax, num2);
			if (num3 == 0)
			{
				return false;
			}
			offsetMax += num3;
			num2 -= num3;
		}
		return true;
	}

	public void Advance(int count)
	{
		offset += count;
	}

	public void InsertBytes(byte[] buffer, int offset, int count)
	{
		if (offsetMax > buffer.Length - count)
		{
			byte[] dst = new byte[offsetMax + count];
			System.Buffer.BlockCopy(this.buffer, 0, dst, 0, offsetMax);
			this.buffer = dst;
			streamBuffer = dst;
		}
		System.Buffer.BlockCopy(this.buffer, this.offset, this.buffer, this.offset + count, offsetMax - this.offset);
		offsetMax += count;
		System.Buffer.BlockCopy(buffer, offset, this.buffer, this.offset, count);
	}

	public void SetWindow(int windowOffset, int windowLength)
	{
		if (windowOffset > int.MaxValue - windowLength)
		{
			windowLength = int.MaxValue - windowOffset;
		}
		if (offset != windowOffset)
		{
			System.Buffer.BlockCopy(buffer, offset, buffer, windowOffset, offsetMax - offset);
			offsetMax = windowOffset + (offsetMax - offset);
			offset = windowOffset;
		}
		this.windowOffset = windowOffset;
		windowOffsetMax = Math.Max(windowOffset + windowLength, offsetMax);
	}

	public int ReadBytes(int count)
	{
		int num = offset;
		if (num > offsetMax - count)
		{
			EnsureBytes(count);
		}
		offset += count;
		return num;
	}

	public int ReadMultiByteUInt31()
	{
		int num = GetByte();
		Advance(1);
		if ((num & 0x80) == 0)
		{
			return num;
		}
		num &= 0x7F;
		int num2 = GetByte();
		Advance(1);
		num |= (num2 & 0x7F) << 7;
		if ((num2 & 0x80) == 0)
		{
			return num;
		}
		int num3 = GetByte();
		Advance(1);
		num |= (num3 & 0x7F) << 14;
		if ((num3 & 0x80) == 0)
		{
			return num;
		}
		int num4 = GetByte();
		Advance(1);
		num |= (num4 & 0x7F) << 21;
		if ((num4 & 0x80) == 0)
		{
			return num;
		}
		int num5 = GetByte();
		Advance(1);
		num |= num5 << 28;
		if ((num5 & 0xF8) != 0)
		{
			XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		}
		return num;
	}

	public int ReadUInt8()
	{
		byte result = GetByte();
		Advance(1);
		return result;
	}

	public int ReadInt8()
	{
		return (sbyte)ReadUInt8();
	}

	public int ReadUInt16()
	{
		int num;
		byte[] array = GetBuffer(2, out num);
		int result = array[num] + (array[num + 1] << 8);
		Advance(2);
		return result;
	}

	public int ReadInt16()
	{
		return (short)ReadUInt16();
	}

	public int ReadInt32()
	{
		int num;
		byte[] array = GetBuffer(4, out num);
		byte b = array[num];
		byte b2 = array[num + 1];
		byte b3 = array[num + 2];
		byte num2 = array[num + 3];
		Advance(4);
		return (((num2 << 8) + b3 << 8) + b2 << 8) + b;
	}

	public int ReadUInt31()
	{
		int num = ReadInt32();
		if (num < 0)
		{
			XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		}
		return num;
	}

	public long ReadInt64()
	{
		long num = (uint)ReadInt32();
		return (long)((ulong)(uint)ReadInt32() << 32) + num;
	}

	[SecuritySafeCritical]
	public unsafe float ReadSingle()
	{
		int num;
		byte[] array = GetBuffer(4, out num);
		float result = default(float);
		byte* ptr = (byte*)(&result);
		*ptr = array[num];
		ptr[1] = array[num + 1];
		ptr[2] = array[num + 2];
		ptr[3] = array[num + 3];
		Advance(4);
		return result;
	}

	[SecuritySafeCritical]
	public unsafe double ReadDouble()
	{
		int num;
		byte[] array = GetBuffer(8, out num);
		double result = default(double);
		byte* ptr = (byte*)(&result);
		*ptr = array[num];
		ptr[1] = array[num + 1];
		ptr[2] = array[num + 2];
		ptr[3] = array[num + 3];
		ptr[4] = array[num + 4];
		ptr[5] = array[num + 5];
		ptr[6] = array[num + 6];
		ptr[7] = array[num + 7];
		Advance(8);
		return result;
	}

	[SecuritySafeCritical]
	public unsafe decimal ReadDecimal()
	{
		int num;
		byte[] array = GetBuffer(16, out num);
		byte b = array[num];
		byte b2 = array[num + 1];
		byte b3 = array[num + 2];
		int num2 = (((array[num + 3] << 8) + b3 << 8) + b2 << 8) + b;
		if ((num2 & 0x7F00FFFF) == 0 && (num2 & 0xFF0000) <= 1835008)
		{
			decimal result = default(decimal);
			byte* ptr = (byte*)(&result);
			for (int i = 0; i < 16; i++)
			{
				ptr[i] = array[num + i];
			}
			Advance(16);
			return result;
		}
		XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		return 0m;
	}

	public UniqueId ReadUniqueId()
	{
		int num;
		UniqueId result = new UniqueId(GetBuffer(16, out num), num);
		Advance(16);
		return result;
	}

	public DateTime ReadDateTime()
	{
		long dateData = 0L;
		try
		{
			dateData = ReadInt64();
			return DateTime.FromBinary(dateData);
		}
		catch (ArgumentException exception)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(dateData.ToString(CultureInfo.InvariantCulture), "DateTime", exception));
		}
		catch (FormatException exception2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(dateData.ToString(CultureInfo.InvariantCulture), "DateTime", exception2));
		}
		catch (OverflowException exception3)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(dateData.ToString(CultureInfo.InvariantCulture), "DateTime", exception3));
		}
	}

	public TimeSpan ReadTimeSpan()
	{
		long value = 0L;
		try
		{
			value = ReadInt64();
			return TimeSpan.FromTicks(value);
		}
		catch (ArgumentException exception)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value.ToString(CultureInfo.InvariantCulture), "TimeSpan", exception));
		}
		catch (FormatException exception2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value.ToString(CultureInfo.InvariantCulture), "TimeSpan", exception2));
		}
		catch (OverflowException exception3)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value.ToString(CultureInfo.InvariantCulture), "TimeSpan", exception3));
		}
	}

	public Guid ReadGuid()
	{
		GetBuffer(16, out var num);
		Guid result = GetGuid(num);
		Advance(16);
		return result;
	}

	public string ReadUTF8String(int length)
	{
		GetBuffer(length, out var num);
		char[] charBuffer = GetCharBuffer(length);
		int length2 = GetChars(num, length, charBuffer);
		string result = new string(charBuffer, 0, length2);
		Advance(length);
		return result;
	}

	[SecurityCritical]
	public unsafe void UnsafeReadArray(byte* dst, byte* dstMax)
	{
		UnsafeReadArray(dst, (int)(dstMax - dst));
	}

	[SecurityCritical]
	private unsafe void UnsafeReadArray(byte* dst, int length)
	{
		if (stream != null)
		{
			while (length >= 256)
			{
				byte[] array = GetBuffer(256, out offset);
				for (int i = 0; i < 256; i++)
				{
					*(dst++) = array[offset + i];
				}
				Advance(256);
				length -= 256;
			}
		}
		if (length <= 0)
		{
			return;
		}
		fixed (byte* ptr = &GetBuffer(length, out offset)[offset])
		{
			byte* ptr2 = ptr;
			byte* ptr3 = dst + length;
			while (dst < ptr3)
			{
				*dst = *ptr2;
				dst++;
				ptr2++;
			}
		}
		Advance(length);
	}

	private char[] GetCharBuffer(int count)
	{
		if (count > 1024)
		{
			return new char[count];
		}
		if (chars == null || chars.Length < count)
		{
			chars = new char[count];
		}
		return chars;
	}

	private int GetChars(int offset, int length, char[] chars)
	{
		byte[] array = buffer;
		for (int i = 0; i < length; i++)
		{
			byte b = array[offset + i];
			if (b >= 128)
			{
				return i + XmlConverter.ToChars(array, offset + i, length - i, chars, i);
			}
			chars[i] = (char)b;
		}
		return length;
	}

	private int GetChars(int offset, int length, char[] chars, int charOffset)
	{
		byte[] array = buffer;
		for (int i = 0; i < length; i++)
		{
			byte b = array[offset + i];
			if (b >= 128)
			{
				return i + XmlConverter.ToChars(array, offset + i, length - i, chars, charOffset + i);
			}
			chars[charOffset + i] = (char)b;
		}
		return length;
	}

	public string GetString(int offset, int length)
	{
		char[] charBuffer = GetCharBuffer(length);
		int length2 = GetChars(offset, length, charBuffer);
		return new string(charBuffer, 0, length2);
	}

	public string GetUnicodeString(int offset, int length)
	{
		return XmlConverter.ToStringUnicode(buffer, offset, length);
	}

	public string GetString(int offset, int length, XmlNameTable nameTable)
	{
		char[] charBuffer = GetCharBuffer(length);
		int length2 = GetChars(offset, length, charBuffer);
		return nameTable.Add(charBuffer, 0, length2);
	}

	public int GetEscapedChars(int offset, int length, char[] chars)
	{
		byte[] array = buffer;
		int num = 0;
		int num2 = offset;
		int num3 = offset + length;
		while (true)
		{
			if (offset < num3 && IsAttrChar(array[offset]))
			{
				offset++;
				continue;
			}
			num += GetChars(num2, offset - num2, chars, num);
			if (offset == num3)
			{
				break;
			}
			num2 = offset;
			if (array[offset] == 38)
			{
				while (offset < num3 && array[offset] != 59)
				{
					offset++;
				}
				offset++;
				int charEntity = GetCharEntity(num2, offset - num2);
				num2 = offset;
				if (charEntity > 65535)
				{
					SurrogateChar surrogateChar = new SurrogateChar(charEntity);
					chars[num++] = surrogateChar.HighChar;
					chars[num++] = surrogateChar.LowChar;
				}
				else
				{
					chars[num++] = (char)charEntity;
				}
			}
			else if (array[offset] == 10 || array[offset] == 9)
			{
				chars[num++] = ' ';
				offset++;
				num2 = offset;
			}
			else
			{
				chars[num++] = ' ';
				offset++;
				if (offset < num3 && array[offset] == 10)
				{
					offset++;
				}
				num2 = offset;
			}
		}
		return num;
	}

	private bool IsAttrChar(int ch)
	{
		if ((uint)(ch - 9) <= 1u || ch == 13 || ch == 38)
		{
			return false;
		}
		return true;
	}

	public string GetEscapedString(int offset, int length)
	{
		char[] charBuffer = GetCharBuffer(length);
		int escapedChars = GetEscapedChars(offset, length, charBuffer);
		return new string(charBuffer, 0, escapedChars);
	}

	public string GetEscapedString(int offset, int length, XmlNameTable nameTable)
	{
		char[] charBuffer = GetCharBuffer(length);
		int escapedChars = GetEscapedChars(offset, length, charBuffer);
		return nameTable.Add(charBuffer, 0, escapedChars);
	}

	private int GetLessThanCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		if (length != 4 || array[offset + 1] != 108 || array[offset + 2] != 116)
		{
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
		}
		return 60;
	}

	private int GetGreaterThanCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		if (length != 4 || array[offset + 1] != 103 || array[offset + 2] != 116)
		{
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
		}
		return 62;
	}

	private int GetQuoteCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		if (length != 6 || array[offset + 1] != 113 || array[offset + 2] != 117 || array[offset + 3] != 111 || array[offset + 4] != 116)
		{
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
		}
		return 34;
	}

	private int GetAmpersandCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		if (length != 5 || array[offset + 1] != 97 || array[offset + 2] != 109 || array[offset + 3] != 112)
		{
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
		}
		return 38;
	}

	private int GetApostropheCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		if (length != 6 || array[offset + 1] != 97 || array[offset + 2] != 112 || array[offset + 3] != 111 || array[offset + 4] != 115)
		{
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
		}
		return 39;
	}

	private int GetDecimalCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		int num = 0;
		for (int i = 2; i < length - 1; i++)
		{
			byte b = array[offset + i];
			if (b < 48 || b > 57)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(reader);
			}
			num = num * 10 + (b - 48);
			if (num > 1114111)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(reader);
			}
		}
		return num;
	}

	private int GetHexCharEntity(int offset, int length)
	{
		byte[] array = buffer;
		int num = 0;
		for (int i = 3; i < length - 1; i++)
		{
			byte b = array[offset + i];
			int num2 = 0;
			if (b >= 48 && b <= 57)
			{
				num2 = b - 48;
			}
			else if (b >= 97 && b <= 102)
			{
				num2 = 10 + (b - 97);
			}
			else if (b >= 65 && b <= 70)
			{
				num2 = 10 + (b - 65);
			}
			else
			{
				XmlExceptionHelper.ThrowInvalidCharRef(reader);
			}
			num = num * 16 + num2;
			if (num > 1114111)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(reader);
			}
		}
		return num;
	}

	public int GetCharEntity(int offset, int length)
	{
		if (length < 3)
		{
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
		}
		byte[] array = buffer;
		switch (array[offset + 1])
		{
		case 108:
			return GetLessThanCharEntity(offset, length);
		case 103:
			return GetGreaterThanCharEntity(offset, length);
		case 97:
			if (array[offset + 2] == 109)
			{
				return GetAmpersandCharEntity(offset, length);
			}
			return GetApostropheCharEntity(offset, length);
		case 113:
			return GetQuoteCharEntity(offset, length);
		case 35:
			if (array[offset + 2] == 120)
			{
				return GetHexCharEntity(offset, length);
			}
			return GetDecimalCharEntity(offset, length);
		default:
			XmlExceptionHelper.ThrowInvalidCharRef(reader);
			return 0;
		}
	}

	public bool IsWhitespaceKey(int key)
	{
		string value = GetDictionaryString(key).Value;
		for (int i = 0; i < value.Length; i++)
		{
			if (!XmlConverter.IsWhitespace(value[i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsWhitespaceUTF8(int offset, int length)
	{
		byte[] array = buffer;
		for (int i = 0; i < length; i++)
		{
			if (!XmlConverter.IsWhitespace((char)array[offset + i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsWhitespaceUnicode(int offset, int length)
	{
		_ = buffer;
		for (int i = 0; i < length; i += 2)
		{
			if (!XmlConverter.IsWhitespace((char)GetInt16(offset + i)))
			{
				return false;
			}
		}
		return true;
	}

	public bool Equals2(int key1, int key2, XmlBufferReader bufferReader2)
	{
		if (key1 == key2)
		{
			return true;
		}
		return GetDictionaryString(key1).Value == bufferReader2.GetDictionaryString(key2).Value;
	}

	public bool Equals2(int key1, XmlDictionaryString xmlString2)
	{
		if ((key1 & 1) == 0 && xmlString2.Dictionary == dictionary)
		{
			return xmlString2.Key == key1 >> 1;
		}
		return GetDictionaryString(key1).Value == xmlString2.Value;
	}

	public bool Equals2(int offset1, int length1, byte[] buffer2)
	{
		int num = buffer2.Length;
		if (length1 != num)
		{
			return false;
		}
		byte[] array = buffer;
		for (int i = 0; i < length1; i++)
		{
			if (array[offset1 + i] != buffer2[i])
			{
				return false;
			}
		}
		return true;
	}

	public bool Equals2(int offset1, int length1, XmlBufferReader bufferReader2, int offset2, int length2)
	{
		if (length1 != length2)
		{
			return false;
		}
		byte[] array = buffer;
		byte[] array2 = bufferReader2.buffer;
		for (int i = 0; i < length1; i++)
		{
			if (array[offset1 + i] != array2[offset2 + i])
			{
				return false;
			}
		}
		return true;
	}

	public bool Equals2(int offset1, int length1, int offset2, int length2)
	{
		if (length1 != length2)
		{
			return false;
		}
		if (offset1 == offset2)
		{
			return true;
		}
		byte[] array = buffer;
		for (int i = 0; i < length1; i++)
		{
			if (array[offset1 + i] != array[offset2 + i])
			{
				return false;
			}
		}
		return true;
	}

	[SecuritySafeCritical]
	public unsafe bool Equals2(int offset1, int length1, string s2)
	{
		int length2 = s2.Length;
		if (length1 < length2 || length1 > length2 * 3)
		{
			return false;
		}
		byte[] array = buffer;
		if (length1 < 8)
		{
			int num = Math.Min(length1, length2);
			for (int i = 0; i < num; i++)
			{
				byte b = array[offset1 + i];
				if (b >= 128)
				{
					return XmlConverter.ToString(array, offset1, length1) == s2;
				}
				if (s2[i] != b)
				{
					return false;
				}
			}
			return length1 == length2;
		}
		int num2 = Math.Min(length1, length2);
		fixed (byte* ptr = &array[offset1])
		{
			byte* ptr2 = ptr;
			byte* ptr3 = ptr2 + num2;
			fixed (char* ptr4 = s2)
			{
				char* ptr5 = ptr4;
				int num3 = 0;
				while (ptr2 < ptr3 && *ptr2 < 128)
				{
					num3 = *ptr2 - (byte)(*ptr5);
					if (num3 != 0)
					{
						break;
					}
					ptr2++;
					ptr5++;
				}
				if (num3 != 0)
				{
					return false;
				}
				if (ptr2 == ptr3)
				{
					return length1 == length2;
				}
			}
		}
		return XmlConverter.ToString(array, offset1, length1) == s2;
	}

	public int Compare(int offset1, int length1, int offset2, int length2)
	{
		byte[] array = buffer;
		int num = Math.Min(length1, length2);
		for (int i = 0; i < num; i++)
		{
			int num2 = array[offset1 + i] - array[offset2 + i];
			if (num2 != 0)
			{
				return num2;
			}
		}
		return length1 - length2;
	}

	public byte GetByte(int offset)
	{
		return buffer[offset];
	}

	public int GetInt8(int offset)
	{
		return (sbyte)GetByte(offset);
	}

	public int GetInt16(int offset)
	{
		byte[] array = buffer;
		return (short)(array[offset] + (array[offset + 1] << 8));
	}

	public int GetInt32(int offset)
	{
		byte[] array = buffer;
		byte b = array[offset];
		byte b2 = array[offset + 1];
		byte b3 = array[offset + 2];
		return (((array[offset + 3] << 8) + b3 << 8) + b2 << 8) + b;
	}

	public long GetInt64(int offset)
	{
		byte[] array = buffer;
		byte b = array[offset];
		byte b2 = array[offset + 1];
		byte b3 = array[offset + 2];
		long num = (uint)((((array[offset + 3] << 8) + b3 << 8) + b2 << 8) + b);
		b = array[offset + 4];
		b2 = array[offset + 5];
		b3 = array[offset + 6];
		return (long)((ulong)(uint)((((array[offset + 7] << 8) + b3 << 8) + b2 << 8) + b) << 32) + num;
	}

	public ulong GetUInt64(int offset)
	{
		return (ulong)GetInt64(offset);
	}

	[SecuritySafeCritical]
	public unsafe float GetSingle(int offset)
	{
		byte[] array = buffer;
		float result = default(float);
		byte* ptr = (byte*)(&result);
		*ptr = array[offset];
		ptr[1] = array[offset + 1];
		ptr[2] = array[offset + 2];
		ptr[3] = array[offset + 3];
		return result;
	}

	[SecuritySafeCritical]
	public unsafe double GetDouble(int offset)
	{
		byte[] array = buffer;
		double result = default(double);
		byte* ptr = (byte*)(&result);
		*ptr = array[offset];
		ptr[1] = array[offset + 1];
		ptr[2] = array[offset + 2];
		ptr[3] = array[offset + 3];
		ptr[4] = array[offset + 4];
		ptr[5] = array[offset + 5];
		ptr[6] = array[offset + 6];
		ptr[7] = array[offset + 7];
		return result;
	}

	[SecuritySafeCritical]
	public unsafe decimal GetDecimal(int offset)
	{
		byte[] array = buffer;
		byte b = array[offset];
		byte b2 = array[offset + 1];
		byte b3 = array[offset + 2];
		int num = (((array[offset + 3] << 8) + b3 << 8) + b2 << 8) + b;
		if ((num & 0x7F00FFFF) == 0 && (num & 0xFF0000) <= 1835008)
		{
			decimal result = default(decimal);
			byte* ptr = (byte*)(&result);
			for (int i = 0; i < 16; i++)
			{
				ptr[i] = array[offset + i];
			}
			return result;
		}
		XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		return 0m;
	}

	public UniqueId GetUniqueId(int offset)
	{
		return new UniqueId(buffer, offset);
	}

	public Guid GetGuid(int offset)
	{
		if (guid == null)
		{
			guid = new byte[16];
		}
		System.Buffer.BlockCopy(buffer, offset, guid, 0, guid.Length);
		return new Guid(guid);
	}

	public void GetBase64(int srcOffset, byte[] buffer, int dstOffset, int count)
	{
		System.Buffer.BlockCopy(this.buffer, srcOffset, buffer, dstOffset, count);
	}

	public XmlBinaryNodeType GetNodeType()
	{
		return (XmlBinaryNodeType)GetByte();
	}

	public void SkipNodeType()
	{
		SkipByte();
	}

	public object[] GetList(int offset, int count)
	{
		int num = Offset;
		Offset = offset;
		try
		{
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				XmlBinaryNodeType nodeType = GetNodeType();
				SkipNodeType();
				ReadValue(nodeType, listValue);
				array[i] = listValue.ToObject();
			}
			return array;
		}
		finally
		{
			Offset = num;
		}
	}

	public XmlDictionaryString GetDictionaryString(int key)
	{
		IXmlDictionary xmlDictionary = (((key & 1) == 0) ? dictionary : session);
		if (!xmlDictionary.TryLookup(key >> 1, out var result))
		{
			XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		}
		return result;
	}

	public int ReadDictionaryKey()
	{
		int num = ReadMultiByteUInt31();
		if ((num & 1) != 0)
		{
			if (session == null)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
			}
			int num2 = num >> 1;
			if (!session.TryLookup(num2, out var _))
			{
				if (num2 < 0 || num2 > 536870911)
				{
					XmlExceptionHelper.ThrowXmlDictionaryStringIDOutOfRange(reader);
				}
				XmlExceptionHelper.ThrowXmlDictionaryStringIDUndefinedSession(reader, num2);
			}
		}
		else
		{
			if (dictionary == null)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
			}
			int num3 = num >> 1;
			if (!dictionary.TryLookup(num3, out var _))
			{
				if (num3 < 0 || num3 > 536870911)
				{
					XmlExceptionHelper.ThrowXmlDictionaryStringIDOutOfRange(reader);
				}
				XmlExceptionHelper.ThrowXmlDictionaryStringIDUndefinedStatic(reader, num3);
			}
		}
		return num;
	}

	public void ReadValue(XmlBinaryNodeType nodeType, ValueHandle value)
	{
		switch (nodeType)
		{
		case XmlBinaryNodeType.EmptyText:
			value.SetValue(ValueHandleType.Empty);
			break;
		case XmlBinaryNodeType.MinText:
			value.SetValue(ValueHandleType.Zero);
			break;
		case XmlBinaryNodeType.OneText:
			value.SetValue(ValueHandleType.One);
			break;
		case XmlBinaryNodeType.TrueText:
			value.SetValue(ValueHandleType.True);
			break;
		case XmlBinaryNodeType.FalseText:
			value.SetValue(ValueHandleType.False);
			break;
		case XmlBinaryNodeType.BoolText:
			value.SetValue((ReadUInt8() != 0) ? ValueHandleType.True : ValueHandleType.False);
			break;
		case XmlBinaryNodeType.Chars8Text:
			ReadValue(value, ValueHandleType.UTF8, ReadUInt8());
			break;
		case XmlBinaryNodeType.Chars16Text:
			ReadValue(value, ValueHandleType.UTF8, ReadUInt16());
			break;
		case XmlBinaryNodeType.Chars32Text:
			ReadValue(value, ValueHandleType.UTF8, ReadUInt31());
			break;
		case XmlBinaryNodeType.UnicodeChars8Text:
			ReadUnicodeValue(value, ReadUInt8());
			break;
		case XmlBinaryNodeType.UnicodeChars16Text:
			ReadUnicodeValue(value, ReadUInt16());
			break;
		case XmlBinaryNodeType.UnicodeChars32Text:
			ReadUnicodeValue(value, ReadUInt31());
			break;
		case XmlBinaryNodeType.Bytes8Text:
			ReadValue(value, ValueHandleType.Base64, ReadUInt8());
			break;
		case XmlBinaryNodeType.Bytes16Text:
			ReadValue(value, ValueHandleType.Base64, ReadUInt16());
			break;
		case XmlBinaryNodeType.Bytes32Text:
			ReadValue(value, ValueHandleType.Base64, ReadUInt31());
			break;
		case XmlBinaryNodeType.DictionaryText:
			value.SetDictionaryValue(ReadDictionaryKey());
			break;
		case XmlBinaryNodeType.UniqueIdText:
			ReadValue(value, ValueHandleType.UniqueId, 16);
			break;
		case XmlBinaryNodeType.GuidText:
			ReadValue(value, ValueHandleType.Guid, 16);
			break;
		case XmlBinaryNodeType.DecimalText:
			ReadValue(value, ValueHandleType.Decimal, 16);
			break;
		case XmlBinaryNodeType.Int8Text:
			ReadValue(value, ValueHandleType.Int8, 1);
			break;
		case XmlBinaryNodeType.Int16Text:
			ReadValue(value, ValueHandleType.Int16, 2);
			break;
		case XmlBinaryNodeType.Int32Text:
			ReadValue(value, ValueHandleType.Int32, 4);
			break;
		case XmlBinaryNodeType.Int64Text:
			ReadValue(value, ValueHandleType.Int64, 8);
			break;
		case XmlBinaryNodeType.UInt64Text:
			ReadValue(value, ValueHandleType.UInt64, 8);
			break;
		case XmlBinaryNodeType.FloatText:
			ReadValue(value, ValueHandleType.Single, 4);
			break;
		case XmlBinaryNodeType.DoubleText:
			ReadValue(value, ValueHandleType.Double, 8);
			break;
		case XmlBinaryNodeType.TimeSpanText:
			ReadValue(value, ValueHandleType.TimeSpan, 8);
			break;
		case XmlBinaryNodeType.DateTimeText:
			ReadValue(value, ValueHandleType.DateTime, 8);
			break;
		case XmlBinaryNodeType.StartListText:
			ReadList(value);
			break;
		case XmlBinaryNodeType.QNameDictionaryText:
			ReadQName(value);
			break;
		default:
			XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
			break;
		}
	}

	private void ReadValue(ValueHandle value, ValueHandleType type, int length)
	{
		int num = ReadBytes(length);
		value.SetValue(type, num, length);
	}

	private void ReadUnicodeValue(ValueHandle value, int length)
	{
		if ((length & 1) != 0)
		{
			XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		}
		ReadValue(value, ValueHandleType.Unicode, length);
	}

	private void ReadList(ValueHandle value)
	{
		if (listValue == null)
		{
			listValue = new ValueHandle(this);
		}
		int num = 0;
		int num2 = Offset;
		while (true)
		{
			XmlBinaryNodeType nodeType = GetNodeType();
			SkipNodeType();
			if (nodeType == XmlBinaryNodeType.StartListText)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
			}
			if (nodeType == XmlBinaryNodeType.EndListText)
			{
				break;
			}
			ReadValue(nodeType, listValue);
			num++;
		}
		value.SetValue(ValueHandleType.List, num2, num);
	}

	public void ReadQName(ValueHandle value)
	{
		int num = ReadUInt8();
		if (num >= 26)
		{
			XmlExceptionHelper.ThrowInvalidBinaryFormat(reader);
		}
		int key = ReadDictionaryKey();
		value.SetQNameValue(num, key);
	}

	public int[] GetRows()
	{
		if (buffer == null)
		{
			return new int[1];
		}
		ArrayList arrayList = new ArrayList();
		arrayList.Add(offsetMin);
		for (int i = offsetMin; i < offsetMax; i++)
		{
			if (buffer[i] == 13 || buffer[i] == 10)
			{
				if (i + 1 < offsetMax && buffer[i + 1] == 10)
				{
					i++;
				}
				arrayList.Add(i + 1);
			}
		}
		return (int[])arrayList.ToArray(typeof(int));
	}
}
