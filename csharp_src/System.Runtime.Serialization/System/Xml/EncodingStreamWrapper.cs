using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class EncodingStreamWrapper : Stream
{
	private enum SupportedEncoding
	{
		UTF8,
		UTF16LE,
		UTF16BE,
		None
	}

	private static readonly UTF8Encoding SafeUTF8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);

	private static readonly UnicodeEncoding SafeUTF16 = new UnicodeEncoding(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: false);

	private static readonly UnicodeEncoding SafeBEUTF16 = new UnicodeEncoding(bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: false);

	private static readonly UTF8Encoding ValidatingUTF8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

	private static readonly UnicodeEncoding ValidatingUTF16 = new UnicodeEncoding(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: true);

	private static readonly UnicodeEncoding ValidatingBEUTF16 = new UnicodeEncoding(bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: true);

	private const int BufferLength = 128;

	private static readonly byte[] encodingAttr = new byte[8] { 101, 110, 99, 111, 100, 105, 110, 103 };

	private static readonly byte[] encodingUTF8 = new byte[5] { 117, 116, 102, 45, 56 };

	private static readonly byte[] encodingUnicode = new byte[6] { 117, 116, 102, 45, 49, 54 };

	private static readonly byte[] encodingUnicodeLE = new byte[8] { 117, 116, 102, 45, 49, 54, 108, 101 };

	private static readonly byte[] encodingUnicodeBE = new byte[8] { 117, 116, 102, 45, 49, 54, 98, 101 };

	private SupportedEncoding encodingCode;

	private Encoding encoding;

	private Encoder enc;

	private Decoder dec;

	private bool isReading;

	private Stream stream;

	private char[] chars;

	private byte[] bytes;

	private int byteOffset;

	private int byteCount;

	private byte[] byteBuffer = new byte[1];

	public override bool CanRead
	{
		get
		{
			if (!isReading)
			{
				return false;
			}
			return stream.CanRead;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite
	{
		get
		{
			if (isReading)
			{
				return false;
			}
			return stream.CanWrite;
		}
	}

	public override long Position
	{
		get
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}
		set
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}
	}

	public override bool CanTimeout => stream.CanTimeout;

	public override long Length => stream.Length;

	public override int ReadTimeout
	{
		get
		{
			return stream.ReadTimeout;
		}
		set
		{
			stream.ReadTimeout = value;
		}
	}

	public override int WriteTimeout
	{
		get
		{
			return stream.WriteTimeout;
		}
		set
		{
			stream.WriteTimeout = value;
		}
	}

	public EncodingStreamWrapper(Stream stream, Encoding encoding)
	{
		try
		{
			isReading = true;
			this.stream = new BufferedStream(stream);
			SupportedEncoding supportedEncoding = GetSupportedEncoding(encoding);
			SupportedEncoding supportedEncoding2 = ReadBOMEncoding(encoding == null);
			if (supportedEncoding != SupportedEncoding.None && supportedEncoding != supportedEncoding2)
			{
				ThrowExpectedEncodingMismatch(supportedEncoding, supportedEncoding2);
			}
			if (supportedEncoding2 == SupportedEncoding.UTF8)
			{
				FillBuffer(2);
				if (bytes[byteOffset + 1] == 63 && bytes[byteOffset] == 60)
				{
					FillBuffer(128);
					CheckUTF8DeclarationEncoding(bytes, byteOffset, byteCount, supportedEncoding2, supportedEncoding);
				}
				return;
			}
			EnsureBuffers();
			FillBuffer(254);
			SetReadDocumentEncoding(supportedEncoding2);
			CleanupCharBreak();
			int charCount = this.encoding.GetChars(bytes, byteOffset, byteCount, chars, 0);
			byteOffset = 0;
			byteCount = ValidatingUTF8.GetBytes(chars, 0, charCount, bytes, 0);
			if (bytes[1] == 63 && bytes[0] == 60)
			{
				CheckUTF8DeclarationEncoding(bytes, 0, byteCount, supportedEncoding2, supportedEncoding);
			}
			else if (supportedEncoding == SupportedEncoding.None)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("An XML declaration with an encoding is required for all non-UTF8 documents.")));
			}
		}
		catch (DecoderFallbackException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid byte encoding."), innerException));
		}
	}

	private void SetReadDocumentEncoding(SupportedEncoding e)
	{
		EnsureBuffers();
		encodingCode = e;
		encoding = GetEncoding(e);
	}

	private static Encoding GetEncoding(SupportedEncoding e)
	{
		return e switch
		{
			SupportedEncoding.UTF8 => ValidatingUTF8, 
			SupportedEncoding.UTF16LE => ValidatingUTF16, 
			SupportedEncoding.UTF16BE => ValidatingBEUTF16, 
			_ => throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("XML encoding not supported."))), 
		};
	}

	private static Encoding GetSafeEncoding(SupportedEncoding e)
	{
		return e switch
		{
			SupportedEncoding.UTF8 => SafeUTF8, 
			SupportedEncoding.UTF16LE => SafeUTF16, 
			SupportedEncoding.UTF16BE => SafeBEUTF16, 
			_ => throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("XML encoding not supported."))), 
		};
	}

	private static string GetEncodingName(SupportedEncoding enc)
	{
		return enc switch
		{
			SupportedEncoding.UTF8 => "utf-8", 
			SupportedEncoding.UTF16LE => "utf-16LE", 
			SupportedEncoding.UTF16BE => "utf-16BE", 
			_ => throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("XML encoding not supported."))), 
		};
	}

	private static SupportedEncoding GetSupportedEncoding(Encoding encoding)
	{
		if (encoding == null)
		{
			return SupportedEncoding.None;
		}
		if (encoding.WebName == ValidatingUTF8.WebName)
		{
			return SupportedEncoding.UTF8;
		}
		if (encoding.WebName == ValidatingUTF16.WebName)
		{
			return SupportedEncoding.UTF16LE;
		}
		if (encoding.WebName == ValidatingBEUTF16.WebName)
		{
			return SupportedEncoding.UTF16BE;
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("XML encoding not supported.")));
	}

	public EncodingStreamWrapper(Stream stream, Encoding encoding, bool emitBOM)
	{
		isReading = false;
		this.encoding = encoding;
		this.stream = new BufferedStream(stream);
		encodingCode = GetSupportedEncoding(encoding);
		if (encodingCode == SupportedEncoding.UTF8)
		{
			return;
		}
		EnsureBuffers();
		dec = ValidatingUTF8.GetDecoder();
		enc = this.encoding.GetEncoder();
		if (emitBOM)
		{
			byte[] preamble = this.encoding.GetPreamble();
			if (preamble.Length != 0)
			{
				this.stream.Write(preamble, 0, preamble.Length);
			}
		}
	}

	private SupportedEncoding ReadBOMEncoding(bool notOutOfBand)
	{
		int num = stream.ReadByte();
		int num2 = stream.ReadByte();
		int num3 = stream.ReadByte();
		int num4 = stream.ReadByte();
		if (num4 == -1)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file.")));
		}
		int preserve;
		SupportedEncoding result = ReadBOMEncoding((byte)num, (byte)num2, (byte)num3, (byte)num4, notOutOfBand, out preserve);
		EnsureByteBuffer();
		switch (preserve)
		{
		case 1:
			bytes[0] = (byte)num4;
			break;
		case 2:
			bytes[0] = (byte)num3;
			bytes[1] = (byte)num4;
			break;
		case 4:
			bytes[0] = (byte)num;
			bytes[1] = (byte)num2;
			bytes[2] = (byte)num3;
			bytes[3] = (byte)num4;
			break;
		}
		byteCount = preserve;
		return result;
	}

	private static SupportedEncoding ReadBOMEncoding(byte b1, byte b2, byte b3, byte b4, bool notOutOfBand, out int preserve)
	{
		SupportedEncoding result = SupportedEncoding.UTF8;
		preserve = 0;
		if (b1 == 60 && b2 != 0)
		{
			result = SupportedEncoding.UTF8;
			preserve = 4;
		}
		else if (b1 == byte.MaxValue && b2 == 254)
		{
			result = SupportedEncoding.UTF16LE;
			preserve = 2;
		}
		else if (b1 == 254 && b2 == byte.MaxValue)
		{
			result = SupportedEncoding.UTF16BE;
			preserve = 2;
		}
		else if (b1 == 0 && b2 == 60)
		{
			result = SupportedEncoding.UTF16BE;
			if (notOutOfBand && (b3 != 0 || b4 != 63))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("An XML declaration is required for all non-UTF8 documents.")));
			}
			preserve = 4;
		}
		else if (b1 == 60 && b2 == 0)
		{
			result = SupportedEncoding.UTF16LE;
			if (notOutOfBand && (b3 != 63 || b4 != 0))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("An XML declaration is required for all non-UTF8 documents.")));
			}
			preserve = 4;
		}
		else if (b1 == 239 && b2 == 187)
		{
			if (notOutOfBand && b3 != 191)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unrecognized Byte Order Mark.")));
			}
			preserve = 1;
		}
		else
		{
			preserve = 4;
		}
		return result;
	}

	private void FillBuffer(int count)
	{
		count -= byteCount;
		while (count > 0)
		{
			int num = stream.Read(bytes, byteOffset + byteCount, count);
			if (num != 0)
			{
				byteCount += num;
				count -= num;
				continue;
			}
			break;
		}
	}

	private void EnsureBuffers()
	{
		EnsureByteBuffer();
		if (chars == null)
		{
			chars = new char[128];
		}
	}

	private void EnsureByteBuffer()
	{
		if (bytes == null)
		{
			bytes = new byte[512];
			byteOffset = 0;
			byteCount = 0;
		}
	}

	private static void CheckUTF8DeclarationEncoding(byte[] buffer, int offset, int count, SupportedEncoding e, SupportedEncoding expectedEnc)
	{
		byte b = 0;
		int num = -1;
		int num2 = offset + Math.Min(count, 128);
		int num3 = 0;
		int num4 = 0;
		for (num3 = offset + 2; num3 < num2; num3++)
		{
			if (b != 0)
			{
				if (buffer[num3] == b)
				{
					b = 0;
				}
			}
			else if (buffer[num3] == 39 || buffer[num3] == 34)
			{
				b = buffer[num3];
			}
			else if (buffer[num3] == 61)
			{
				if (num4 == 1)
				{
					num = num3;
					break;
				}
				num4++;
			}
			else if (buffer[num3] == 63)
			{
				break;
			}
		}
		if (num == -1)
		{
			if (e != SupportedEncoding.UTF8 && expectedEnc == SupportedEncoding.None)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("An XML declaration with an encoding is required for all non-UTF8 documents.")));
			}
			return;
		}
		if (num < 28)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Malformed XML declaration.")));
		}
		num3 = num - 1;
		while (IsWhitespace(buffer[num3]))
		{
			num3--;
		}
		if (!Compare(encodingAttr, buffer, num3 - encodingAttr.Length + 1))
		{
			if (e == SupportedEncoding.UTF8 || expectedEnc != SupportedEncoding.None)
			{
				return;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("An XML declaration with an encoding is required for all non-UTF8 documents.")));
		}
		for (num3 = num + 1; num3 < num2 && IsWhitespace(buffer[num3]); num3++)
		{
		}
		if (buffer[num3] != 39 && buffer[num3] != 34)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Malformed XML declaration.")));
		}
		b = buffer[num3];
		int num5 = num3++;
		for (; buffer[num3] != b && num3 < num2; num3++)
		{
		}
		if (buffer[num3] != b)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Malformed XML declaration.")));
		}
		int num6 = num5 + 1;
		int num7 = num3 - num6;
		SupportedEncoding supportedEncoding = e;
		if (num7 == encodingUTF8.Length && CompareCaseInsensitive(encodingUTF8, buffer, num6))
		{
			supportedEncoding = SupportedEncoding.UTF8;
		}
		else if (num7 == encodingUnicodeLE.Length && CompareCaseInsensitive(encodingUnicodeLE, buffer, num6))
		{
			supportedEncoding = SupportedEncoding.UTF16LE;
		}
		else if (num7 == encodingUnicodeBE.Length && CompareCaseInsensitive(encodingUnicodeBE, buffer, num6))
		{
			supportedEncoding = SupportedEncoding.UTF16BE;
		}
		else if (num7 == encodingUnicode.Length && CompareCaseInsensitive(encodingUnicode, buffer, num6))
		{
			if (e == SupportedEncoding.UTF8)
			{
				ThrowEncodingMismatch(SafeUTF8.GetString(buffer, num6, num7), SafeUTF8.GetString(encodingUTF8, 0, encodingUTF8.Length));
			}
		}
		else
		{
			ThrowEncodingMismatch(SafeUTF8.GetString(buffer, num6, num7), e);
		}
		if (e != supportedEncoding)
		{
			ThrowEncodingMismatch(SafeUTF8.GetString(buffer, num6, num7), e);
		}
	}

	private static bool CompareCaseInsensitive(byte[] key, byte[] buffer, int offset)
	{
		for (int i = 0; i < key.Length; i++)
		{
			if (key[i] != buffer[offset + i] && key[i] != char.ToLower((char)buffer[offset + i], CultureInfo.InvariantCulture))
			{
				return false;
			}
		}
		return true;
	}

	private static bool Compare(byte[] key, byte[] buffer, int offset)
	{
		for (int i = 0; i < key.Length; i++)
		{
			if (key[i] != buffer[offset + i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsWhitespace(byte ch)
	{
		if (ch != 32 && ch != 10 && ch != 9)
		{
			return ch == 13;
		}
		return true;
	}

	internal static ArraySegment<byte> ProcessBuffer(byte[] buffer, int offset, int count, Encoding encoding)
	{
		if (count < 4)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file.")));
		}
		try
		{
			SupportedEncoding supportedEncoding = GetSupportedEncoding(encoding);
			int preserve;
			SupportedEncoding supportedEncoding2 = ReadBOMEncoding(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], encoding == null, out preserve);
			if (supportedEncoding != SupportedEncoding.None && supportedEncoding != supportedEncoding2)
			{
				ThrowExpectedEncodingMismatch(supportedEncoding, supportedEncoding2);
			}
			offset += 4 - preserve;
			count -= 4 - preserve;
			if (supportedEncoding2 == SupportedEncoding.UTF8)
			{
				if (buffer[offset + 1] != 63 || buffer[offset] != 60)
				{
					return new ArraySegment<byte>(buffer, offset, count);
				}
				CheckUTF8DeclarationEncoding(buffer, offset, count, supportedEncoding2, supportedEncoding);
				return new ArraySegment<byte>(buffer, offset, count);
			}
			Encoding safeEncoding = GetSafeEncoding(supportedEncoding2);
			int num = Math.Min(count, 256);
			char[] array = new char[safeEncoding.GetMaxCharCount(num)];
			int charCount = safeEncoding.GetChars(buffer, offset, num, array, 0);
			byte[] array2 = new byte[ValidatingUTF8.GetMaxByteCount(charCount)];
			int count2 = ValidatingUTF8.GetBytes(array, 0, charCount, array2, 0);
			if (array2[1] == 63 && array2[0] == 60)
			{
				CheckUTF8DeclarationEncoding(array2, 0, count2, supportedEncoding2, supportedEncoding);
			}
			else if (supportedEncoding == SupportedEncoding.None)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("An XML declaration with an encoding is required for all non-UTF8 documents.")));
			}
			return new ArraySegment<byte>(ValidatingUTF8.GetBytes(GetEncoding(supportedEncoding2).GetChars(buffer, offset, count)));
		}
		catch (DecoderFallbackException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid byte encoding."), innerException));
		}
	}

	private static void ThrowExpectedEncodingMismatch(SupportedEncoding expEnc, SupportedEncoding actualEnc)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("The expected encoding '{0}' does not match the actual encoding '{1}'.", GetEncodingName(expEnc), GetEncodingName(actualEnc))));
	}

	private static void ThrowEncodingMismatch(string declEnc, SupportedEncoding enc)
	{
		ThrowEncodingMismatch(declEnc, GetEncodingName(enc));
	}

	private static void ThrowEncodingMismatch(string declEnc, string docEnc)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("The encoding in the declaration '{0}' does not match the encoding of the document '{1}'.", declEnc, docEnc)));
	}

	public override void Close()
	{
		Flush();
		base.Close();
		stream.Close();
	}

	public override void Flush()
	{
		stream.Flush();
	}

	public override int ReadByte()
	{
		if (byteCount == 0 && encodingCode == SupportedEncoding.UTF8)
		{
			return stream.ReadByte();
		}
		if (Read(byteBuffer, 0, 1) == 0)
		{
			return -1;
		}
		return byteBuffer[0];
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		try
		{
			if (byteCount == 0)
			{
				if (encodingCode == SupportedEncoding.UTF8)
				{
					return stream.Read(buffer, offset, count);
				}
				byteOffset = 0;
				byteCount = stream.Read(bytes, byteCount, (chars.Length - 1) * 2);
				if (byteCount == 0)
				{
					return 0;
				}
				CleanupCharBreak();
				int charCount = encoding.GetChars(bytes, 0, byteCount, chars, 0);
				byteCount = Encoding.UTF8.GetBytes(chars, 0, charCount, bytes, 0);
			}
			if (byteCount < count)
			{
				count = byteCount;
			}
			Buffer.BlockCopy(bytes, byteOffset, buffer, offset, count);
			byteOffset += count;
			byteCount -= count;
			return count;
		}
		catch (DecoderFallbackException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid byte encoding."), innerException));
		}
	}

	private void CleanupCharBreak()
	{
		int num = byteOffset + byteCount;
		if (byteCount % 2 != 0)
		{
			int num2 = stream.ReadByte();
			if (num2 < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file.")));
			}
			bytes[num++] = (byte)num2;
			byteCount++;
		}
		int num3 = ((encodingCode != SupportedEncoding.UTF16LE) ? (bytes[num - 1] + (bytes[num - 2] << 8)) : (bytes[num - 2] + (bytes[num - 1] << 8)));
		if ((num3 & 0xDC00) != 56320 && num3 >= 55296 && num3 <= 56319)
		{
			int num4 = stream.ReadByte();
			int num5 = stream.ReadByte();
			if (num5 < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file.")));
			}
			bytes[num++] = (byte)num4;
			bytes[num++] = (byte)num5;
			byteCount += 2;
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	public override void WriteByte(byte b)
	{
		if (encodingCode == SupportedEncoding.UTF8)
		{
			stream.WriteByte(b);
			return;
		}
		byteBuffer[0] = b;
		Write(byteBuffer, 0, 1);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (encodingCode == SupportedEncoding.UTF8)
		{
			stream.Write(buffer, offset, count);
			return;
		}
		while (count > 0)
		{
			int num = ((chars.Length < count) ? chars.Length : count);
			int charCount = dec.GetChars(buffer, offset, num, chars, 0, flush: false);
			byteCount = enc.GetBytes(chars, 0, charCount, bytes, 0, flush: false);
			stream.Write(bytes, 0, byteCount);
			offset += num;
			count -= num;
		}
	}

	public override void SetLength(long value)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}
}
