using System.Globalization;
using System.Runtime.Serialization;

namespace System.Xml;

internal class BufferedWrite
{
	private byte[] buffer;

	private int offset;

	internal int Length => offset;

	internal BufferedWrite()
		: this(256)
	{
	}

	internal BufferedWrite(int initialSize)
	{
		buffer = new byte[initialSize];
	}

	private void EnsureBuffer(int count)
	{
		int num = buffer.Length;
		if (count <= num - offset)
		{
			return;
		}
		int num2 = num;
		do
		{
			if (num2 == int.MaxValue)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Write buffer overflow.")));
			}
			num2 = ((num2 < 1073741823) ? (num2 * 2) : int.MaxValue);
		}
		while (count > num2 - offset);
		byte[] dst = new byte[num2];
		Buffer.BlockCopy(buffer, 0, dst, 0, offset);
		buffer = dst;
	}

	internal byte[] GetBuffer()
	{
		return buffer;
	}

	internal void Reset()
	{
		offset = 0;
	}

	internal void Write(byte[] value)
	{
		Write(value, 0, value.Length);
	}

	internal void Write(byte[] value, int index, int count)
	{
		EnsureBuffer(count);
		Buffer.BlockCopy(value, index, buffer, offset, count);
		offset += count;
	}

	internal void Write(string value)
	{
		Write(value, 0, value.Length);
	}

	internal void Write(string value, int index, int count)
	{
		EnsureBuffer(count);
		for (int i = 0; i < count; i++)
		{
			char c = value[index + i];
			if (c > 'ÿ')
			{
				object[] obj = new object[2] { c, null };
				int num = c;
				obj[1] = num.ToString("X", CultureInfo.InvariantCulture);
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME header has an invalid character ('{0}', {1} in hexadecimal value).", obj)));
			}
			buffer[offset + i] = (byte)c;
		}
		offset += count;
	}
}
