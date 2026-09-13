using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System.Text;

internal class BinHexEncoding : Encoding
{
	private static byte[] char2val = new byte[128]
	{
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 0, 1,
		2, 3, 4, 5, 6, 7, 8, 9, 255, 255,
		255, 255, 255, 255, 255, 10, 11, 12, 13, 14,
		15, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 10, 11, 12,
		13, 14, 15, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255
	};

	private static string val2char = "0123456789ABCDEF";

	public override int GetMaxByteCount(int charCount)
	{
		if (charCount < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charCount", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (charCount % 2 != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("BinHex sequence length ({0}) not valid. Must be a multiple of 2.", charCount.ToString(NumberFormatInfo.CurrentInfo))));
		}
		return charCount / 2;
	}

	public override int GetByteCount(char[] chars, int index, int count)
	{
		return GetMaxByteCount(count);
	}

	[SecuritySafeCritical]
	public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
	{
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (charIndex < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charIndex", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (charIndex > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charIndex", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (charCount < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charCount", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (charCount > chars.Length - charIndex)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charCount", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - charIndex)));
		}
		if (bytes == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("bytes"));
		}
		if (byteIndex < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteIndex", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (byteIndex > bytes.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteIndex", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", bytes.Length)));
		}
		int byteCount = GetByteCount(chars, charIndex, charCount);
		if (byteCount < 0 || byteCount > bytes.Length - byteIndex)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Array too small."), "bytes"));
		}
		if (charCount > 0)
		{
			fixed (byte* ptr = char2val)
			{
				fixed (byte* ptr2 = &bytes[byteIndex])
				{
					fixed (char* ptr3 = &chars[charIndex])
					{
						char* ptr4 = ptr3;
						char* ptr5 = ptr3 + charCount;
						byte* ptr6 = ptr2;
						while (ptr4 < ptr5)
						{
							char c = *ptr4;
							char c2 = ptr4[1];
							if ((c | c2) >= 128)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid BinHex sequence.", new string(ptr4, 0, 2), charIndex + (int)(ptr4 - ptr3))));
							}
							byte b = ptr[(int)c];
							byte b2 = ptr[(int)c2];
							if ((b | b2) == 255)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid BinHex sequence.", new string(ptr4, 0, 2), charIndex + (int)(ptr4 - ptr3))));
							}
							*ptr6 = (byte)((b << 4) + b2);
							ptr4 += 2;
							ptr6++;
						}
					}
				}
			}
		}
		return byteCount;
	}

	public override int GetMaxCharCount(int byteCount)
	{
		if (byteCount < 0 || byteCount > 1073741823)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteCount", SR.GetString("The value of this argument must fall within the range {0} to {1}.", 0, 1073741823)));
		}
		return byteCount * 2;
	}

	public override int GetCharCount(byte[] bytes, int index, int count)
	{
		return GetMaxCharCount(count);
	}

	[SecuritySafeCritical]
	public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
	{
		if (bytes == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("bytes"));
		}
		if (byteIndex < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteIndex", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (byteIndex > bytes.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteIndex", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", bytes.Length)));
		}
		if (byteCount < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteCount", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (byteCount > bytes.Length - byteIndex)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteCount", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", bytes.Length - byteIndex)));
		}
		int charCount = GetCharCount(bytes, byteIndex, byteCount);
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (charIndex < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charIndex", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (charIndex > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charIndex", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (charCount < 0 || charCount > chars.Length - charIndex)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Array too small."), "chars"));
		}
		if (byteCount > 0)
		{
			fixed (char* ptr = val2char)
			{
				fixed (byte* ptr2 = &bytes[byteIndex])
				{
					fixed (char* ptr3 = &chars[charIndex])
					{
						char* ptr4 = ptr3;
						byte* ptr5 = ptr2;
						byte* ptr6 = ptr2 + byteCount;
						while (ptr5 < ptr6)
						{
							*ptr4 = ptr[*ptr5 >> 4];
							ptr4[1] = ptr[*ptr5 & 0xF];
							ptr5++;
							ptr4 += 2;
						}
					}
				}
			}
		}
		return charCount;
	}
}
