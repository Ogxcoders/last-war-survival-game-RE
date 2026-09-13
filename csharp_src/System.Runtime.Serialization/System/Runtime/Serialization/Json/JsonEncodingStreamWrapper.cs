using System.IO;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonEncodingStreamWrapper : Stream
{
	private enum SupportedEncoding
	{
		UTF8,
		UTF16LE,
		UTF16BE,
		None
	}

	private static readonly UnicodeEncoding SafeBEUTF16 = new UnicodeEncoding(bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: false);

	private static readonly UnicodeEncoding SafeUTF16 = new UnicodeEncoding(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: false);

	private static readonly UTF8Encoding SafeUTF8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);

	private static readonly UnicodeEncoding ValidatingBEUTF16 = new UnicodeEncoding(bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: true);

	private static readonly UnicodeEncoding ValidatingUTF16 = new UnicodeEncoding(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: true);

	private static readonly UTF8Encoding ValidatingUTF8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

	private const int BufferLength = 128;

	private byte[] byteBuffer = new byte[1];

	private int byteCount;

	private int byteOffset;

	private byte[] bytes;

	private char[] chars;

	private Decoder dec;

	private Encoder enc;

	private Encoding encoding;

	private SupportedEncoding encodingCode;

	private bool isReading;

	private Stream stream;

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

	public override bool CanTimeout => stream.CanTimeout;

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

	public override long Length => stream.Length;

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

	public JsonEncodingStreamWrapper(Stream stream, Encoding encoding, bool isReader)
	{
		isReading = isReader;
		if (isReader)
		{
			InitForReading(stream, encoding);
			return;
		}
		if (encoding == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
		}
		InitForWriting(stream, encoding);
	}

	public static ArraySegment<byte> ProcessBuffer(byte[] buffer, int offset, int count, Encoding encoding)
	{
		try
		{
			SupportedEncoding supportedEncoding = GetSupportedEncoding(encoding);
			SupportedEncoding supportedEncoding2 = ((count >= 2) ? ReadEncoding(buffer[offset], buffer[offset + 1]) : SupportedEncoding.UTF8);
			if (supportedEncoding != SupportedEncoding.None && supportedEncoding != supportedEncoding2)
			{
				ThrowExpectedEncodingMismatch(supportedEncoding, supportedEncoding2);
			}
			if (supportedEncoding2 == SupportedEncoding.UTF8)
			{
				return new ArraySegment<byte>(buffer, offset, count);
			}
			return new ArraySegment<byte>(ValidatingUTF8.GetBytes(GetEncoding(supportedEncoding2).GetChars(buffer, offset, count)));
		}
		catch (DecoderFallbackException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON."), innerException));
		}
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
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON."), innerException));
		}
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

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	public override void SetLength(long value)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
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

	private static Encoding GetEncoding(SupportedEncoding e)
	{
		return e switch
		{
			SupportedEncoding.UTF8 => ValidatingUTF8, 
			SupportedEncoding.UTF16LE => ValidatingUTF16, 
			SupportedEncoding.UTF16BE => ValidatingBEUTF16, 
			_ => throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON Encoding is not supported."))), 
		};
	}

	private static string GetEncodingName(SupportedEncoding enc)
	{
		return enc switch
		{
			SupportedEncoding.UTF8 => "utf-8", 
			SupportedEncoding.UTF16LE => "utf-16LE", 
			SupportedEncoding.UTF16BE => "utf-16BE", 
			_ => throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON Encoding is not supported."))), 
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
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON Encoding is not supported.")));
	}

	private static SupportedEncoding ReadEncoding(byte b1, byte b2)
	{
		if (b1 == 0 && b2 != 0)
		{
			return SupportedEncoding.UTF16BE;
		}
		if (b1 != 0 && b2 == 0)
		{
			return SupportedEncoding.UTF16LE;
		}
		if (b1 == 0 && b2 == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON.")));
		}
		return SupportedEncoding.UTF8;
	}

	private static void ThrowExpectedEncodingMismatch(SupportedEncoding expEnc, SupportedEncoding actualEnc)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Expected encoding '{0}', got '{1}' instead.", GetEncodingName(expEnc), GetEncodingName(actualEnc))));
	}

	private void CleanupCharBreak()
	{
		int num = byteOffset + byteCount;
		if (byteCount % 2 != 0)
		{
			int num2 = stream.ReadByte();
			if (num2 < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file in JSON.")));
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
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file in JSON.")));
			}
			bytes[num++] = (byte)num4;
			bytes[num++] = (byte)num5;
			byteCount += 2;
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

	private void InitForReading(Stream inputStream, Encoding expectedEncoding)
	{
		try
		{
			stream = new BufferedStream(inputStream);
			SupportedEncoding supportedEncoding = GetSupportedEncoding(expectedEncoding);
			SupportedEncoding supportedEncoding2 = ReadEncoding();
			if (supportedEncoding != SupportedEncoding.None && supportedEncoding != supportedEncoding2)
			{
				ThrowExpectedEncodingMismatch(supportedEncoding, supportedEncoding2);
			}
			if (supportedEncoding2 != SupportedEncoding.UTF8)
			{
				EnsureBuffers();
				FillBuffer(254);
				encodingCode = supportedEncoding2;
				encoding = GetEncoding(supportedEncoding2);
				CleanupCharBreak();
				int charCount = encoding.GetChars(bytes, byteOffset, byteCount, chars, 0);
				byteOffset = 0;
				byteCount = ValidatingUTF8.GetBytes(chars, 0, charCount, bytes, 0);
			}
		}
		catch (DecoderFallbackException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON."), innerException));
		}
	}

	private void InitForWriting(Stream outputStream, Encoding writeEncoding)
	{
		encoding = writeEncoding;
		stream = new BufferedStream(outputStream);
		encodingCode = GetSupportedEncoding(writeEncoding);
		if (encodingCode != SupportedEncoding.UTF8)
		{
			EnsureBuffers();
			dec = ValidatingUTF8.GetDecoder();
			enc = encoding.GetEncoder();
		}
	}

	private SupportedEncoding ReadEncoding()
	{
		int num = stream.ReadByte();
		int num2 = stream.ReadByte();
		EnsureByteBuffer();
		SupportedEncoding result;
		if (num == -1)
		{
			result = SupportedEncoding.UTF8;
			byteCount = 0;
		}
		else if (num2 == -1)
		{
			result = SupportedEncoding.UTF8;
			bytes[0] = (byte)num;
			byteCount = 1;
		}
		else
		{
			result = ReadEncoding((byte)num, (byte)num2);
			bytes[0] = (byte)num;
			bytes[1] = (byte)num2;
			byteCount = 2;
		}
		return result;
	}
}
