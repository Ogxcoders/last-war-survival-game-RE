using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Xml;

internal class MimeHeaderReader
{
	private enum ReadState
	{
		ReadName,
		SkipWS,
		ReadValue,
		ReadLF,
		ReadWS,
		EOF
	}

	private string value;

	private byte[] buffer = new byte[1024];

	private int maxOffset;

	private string name;

	private int offset;

	private ReadState readState;

	private Stream stream;

	public string Value => value;

	public string Name => name;

	public MimeHeaderReader(Stream stream)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		this.stream = stream;
	}

	public void Close()
	{
		stream.Close();
		readState = ReadState.EOF;
	}

	public bool Read(int maxBuffer, ref int remaining)
	{
		name = null;
		value = null;
		while (readState != ReadState.EOF)
		{
			if (offset == maxOffset)
			{
				maxOffset = stream.Read(buffer, 0, buffer.Length);
				offset = 0;
				if (BufferEnd())
				{
					break;
				}
			}
			if (ProcessBuffer(maxBuffer, ref remaining))
			{
				break;
			}
		}
		return value != null;
	}

	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private unsafe bool ProcessBuffer(int maxBuffer, ref int remaining)
	{
		fixed (byte* ptr = buffer)
		{
			byte* ptr2 = ptr + offset;
			byte* ptr3 = ptr + maxOffset;
			byte* ptr4 = ptr2;
			object[] obj;
			int num;
			switch (readState)
			{
			case ReadState.ReadName:
				for (; ptr4 < ptr3; ptr4++)
				{
					if (*ptr4 == 58)
					{
						goto IL_0065;
					}
					if (*ptr4 >= 65 && *ptr4 <= 90)
					{
						byte* intPtr = ptr4;
						*intPtr += 32;
						continue;
					}
					if (*ptr4 >= 33 && *ptr4 <= 126)
					{
						continue;
					}
					goto IL_00b1;
				}
				AppendName(new string((sbyte*)ptr2, 0, (int)(ptr4 - ptr2)), maxBuffer, ref remaining);
				readState = ReadState.ReadName;
				break;
			case ReadState.SkipWS:
				while (ptr4 < ptr3)
				{
					if (*ptr4 == 9 || *ptr4 == 32)
					{
						ptr4++;
						continue;
					}
					goto case ReadState.ReadValue;
				}
				readState = ReadState.SkipWS;
				break;
			case ReadState.ReadValue:
				ptr2 = ptr4;
				while (ptr4 < ptr3)
				{
					if (*ptr4 != 13)
					{
						if (*ptr4 == 10)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Malformed MIME header.")));
						}
						ptr4++;
						continue;
					}
					goto IL_018b;
				}
				AppendValue(new string((sbyte*)ptr2, 0, (int)(ptr4 - ptr2)), maxBuffer, ref remaining);
				readState = ReadState.ReadValue;
				break;
			case ReadState.ReadLF:
				if (ptr4 < ptr3)
				{
					if (*ptr4 != 10)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Malformed MIME header.")));
					}
					ptr4++;
					goto case ReadState.ReadWS;
				}
				readState = ReadState.ReadLF;
				break;
			case ReadState.ReadWS:
				if (ptr4 < ptr3)
				{
					if (*ptr4 != 32 && *ptr4 != 9)
					{
						readState = ReadState.ReadName;
						offset = (int)(ptr4 - ptr);
						return true;
					}
					goto case ReadState.ReadValue;
				}
				readState = ReadState.ReadWS;
				break;
			case ReadState.EOF:
				{
					readState = ReadState.EOF;
					offset = (int)(ptr4 - ptr);
					return true;
				}
				IL_0065:
				AppendName(new string((sbyte*)ptr2, 0, (int)(ptr4 - ptr2)), maxBuffer, ref remaining);
				ptr4++;
				goto case ReadState.SkipWS;
				IL_00b1:
				if (name == null && *ptr4 == 13)
				{
					ptr4++;
					if (ptr4 >= ptr3 || *ptr4 != 10)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Malformed MIME header.")));
					}
					goto case ReadState.EOF;
				}
				obj = new object[2]
				{
					(char)(*ptr4),
					null
				};
				num = *ptr4;
				obj[1] = num.ToString("X", CultureInfo.InvariantCulture);
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME header has an invalid character ('{0}', {1} in hexadecimal value).", obj)));
				IL_018b:
				AppendValue(new string((sbyte*)ptr2, 0, (int)(ptr4 - ptr2)), maxBuffer, ref remaining);
				ptr4++;
				goto case ReadState.ReadLF;
			}
			offset = (int)(ptr4 - ptr);
		}
		return false;
	}

	private bool BufferEnd()
	{
		if (maxOffset == 0)
		{
			if (readState != ReadState.ReadWS && readState != ReadState.ReadValue)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Malformed MIME header.")));
			}
			readState = ReadState.EOF;
			return true;
		}
		return false;
	}

	public void Reset(Stream stream)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (readState != ReadState.EOF)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("On MimeReader, Reset method is called before EOF.")));
		}
		this.stream = stream;
		readState = ReadState.ReadName;
		maxOffset = 0;
		offset = 0;
	}

	private void AppendValue(string value, int maxBuffer, ref int remaining)
	{
		XmlMtomReader.DecrementBufferQuota(maxBuffer, ref remaining, value.Length * 2);
		if (this.value == null)
		{
			this.value = value;
		}
		else
		{
			this.value += value;
		}
	}

	private void AppendName(string value, int maxBuffer, ref int remaining)
	{
		XmlMtomReader.DecrementBufferQuota(maxBuffer, ref remaining, value.Length * 2);
		if (name == null)
		{
			name = value;
		}
		else
		{
			name += value;
		}
	}
}
