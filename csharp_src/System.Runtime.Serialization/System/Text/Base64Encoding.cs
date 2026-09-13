using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System.Text;

internal class Base64Encoding : Encoding
{
	private static byte[] char2val = new byte[128]
	{
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 62, 255, 255, 255, 63, 52, 53,
		54, 55, 56, 57, 58, 59, 60, 61, 255, 255,
		255, 64, 255, 255, 255, 0, 1, 2, 3, 4,
		5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
		15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
		25, 255, 255, 255, 255, 255, 255, 26, 27, 28,
		29, 30, 31, 32, 33, 34, 35, 36, 37, 38,
		39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
		49, 50, 51, 255, 255, 255, 255, 255
	};

	private static string val2char = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

	private static byte[] val2byte = new byte[64]
	{
		65, 66, 67, 68, 69, 70, 71, 72, 73, 74,
		75, 76, 77, 78, 79, 80, 81, 82, 83, 84,
		85, 86, 87, 88, 89, 90, 97, 98, 99, 100,
		101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
		111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
		121, 122, 48, 49, 50, 51, 52, 53, 54, 55,
		56, 57, 43, 47
	};

	public override int GetMaxByteCount(int charCount)
	{
		if (charCount < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("charCount", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (charCount % 4 != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Base64 sequence length ({0}) not valid. Must be a multiple of 4.", charCount.ToString(NumberFormatInfo.CurrentInfo))));
		}
		return charCount / 4 * 3;
	}

	private bool IsValidLeadBytes(int v1, int v2, int v3, int v4)
	{
		if ((v1 | v2) < 64)
		{
			return (v3 | v4) != 255;
		}
		return false;
	}

	private bool IsValidTailBytes(int v3, int v4)
	{
		if (v3 == 64)
		{
			return v4 == 64;
		}
		return true;
	}

	[SecuritySafeCritical]
	public unsafe override int GetByteCount(char[] chars, int index, int count)
	{
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (index < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (index > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - index)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - index)));
		}
		if (count == 0)
		{
			return 0;
		}
		if (count % 4 != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Base64 sequence length ({0}) not valid. Must be a multiple of 4.", count.ToString(NumberFormatInfo.CurrentInfo))));
		}
		fixed (byte* ptr = char2val)
		{
			fixed (char* ptr2 = &chars[index])
			{
				int num = 0;
				char* ptr3 = ptr2;
				for (char* ptr4 = ptr2 + count; ptr3 < ptr4; ptr3 += 4)
				{
					char c = *ptr3;
					char c2 = ptr3[1];
					char c3 = ptr3[2];
					char c4 = ptr3[3];
					if ((c | c2 | c3 | c4) >= 128)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid Base64 sequence.", new string(ptr3, 0, 4), index + (int)(ptr3 - ptr2))));
					}
					int v = ptr[(int)c];
					int v2 = ptr[(int)c2];
					int num2 = ptr[(int)c3];
					int num3 = ptr[(int)c4];
					if (!IsValidLeadBytes(v, v2, num2, num3) || !IsValidTailBytes(num2, num3))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid Base64 sequence.", new string(ptr3, 0, 4), index + (int)(ptr3 - ptr2))));
					}
					int num4 = ((num3 != 64) ? 3 : ((num2 == 64) ? 1 : 2));
					num += num4;
				}
				return num;
			}
		}
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
		if (charCount == 0)
		{
			return 0;
		}
		if (charCount % 4 != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Base64 sequence length ({0}) not valid. Must be a multiple of 4.", charCount.ToString(NumberFormatInfo.CurrentInfo))));
		}
		fixed (byte* ptr = char2val)
		{
			fixed (char* ptr2 = &chars[charIndex])
			{
				fixed (byte* ptr3 = &bytes[byteIndex])
				{
					char* ptr4 = ptr2;
					char* ptr5 = ptr2 + charCount;
					byte* ptr6 = ptr3;
					byte* ptr7 = ptr3 + bytes.Length - byteIndex;
					for (; ptr4 < ptr5; ptr4 += 4)
					{
						char c = *ptr4;
						char c2 = ptr4[1];
						char c3 = ptr4[2];
						char c4 = ptr4[3];
						if ((c | c2 | c3 | c4) >= 128)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid Base64 sequence.", new string(ptr4, 0, 4), charIndex + (int)(ptr4 - ptr2))));
						}
						int num = ptr[(int)c];
						int num2 = ptr[(int)c2];
						int num3 = ptr[(int)c3];
						int num4 = ptr[(int)c4];
						if (!IsValidLeadBytes(num, num2, num3, num4) || !IsValidTailBytes(num3, num4))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid Base64 sequence.", new string(ptr4, 0, 4), charIndex + (int)(ptr4 - ptr2))));
						}
						int num5 = ((num4 != 64) ? 3 : ((num3 == 64) ? 1 : 2));
						if (ptr6 + num5 > ptr7)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Array too small."), "bytes"));
						}
						*ptr6 = (byte)((num << 2) | ((num2 >> 4) & 3));
						if (num5 > 1)
						{
							ptr6[1] = (byte)((num2 << 4) | ((num3 >> 2) & 0xF));
							if (num5 > 2)
							{
								ptr6[2] = (byte)((num3 << 6) | (num4 & 0x3F));
							}
						}
						ptr6 += num5;
					}
					return (int)(ptr6 - ptr3);
				}
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe virtual int GetBytes(byte[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
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
		if (charCount == 0)
		{
			return 0;
		}
		if (charCount % 4 != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Base64 sequence length ({0}) not valid. Must be a multiple of 4.", charCount.ToString(NumberFormatInfo.CurrentInfo))));
		}
		fixed (byte* ptr = char2val)
		{
			fixed (byte* ptr2 = &chars[charIndex])
			{
				fixed (byte* ptr3 = &bytes[byteIndex])
				{
					byte* ptr4 = ptr2;
					byte* ptr5 = ptr2 + charCount;
					byte* ptr6 = ptr3;
					byte* ptr7 = ptr3 + bytes.Length - byteIndex;
					for (; ptr4 < ptr5; ptr4 += 4)
					{
						byte b = *ptr4;
						byte b2 = ptr4[1];
						byte b3 = ptr4[2];
						byte b4 = ptr4[3];
						if ((b | b2 | b3 | b4) >= 128)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid Base64 sequence.", new string((sbyte*)ptr4, 0, 4), charIndex + (int)(ptr4 - ptr2))));
						}
						int num = ptr[(int)b];
						int num2 = ptr[(int)b2];
						int num3 = ptr[(int)b3];
						int num4 = ptr[(int)b4];
						if (!IsValidLeadBytes(num, num2, num3, num4) || !IsValidTailBytes(num3, num4))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("The characters '{0}' at offset {1} are not a valid Base64 sequence.", new string((sbyte*)ptr4, 0, 4), charIndex + (int)(ptr4 - ptr2))));
						}
						int num5 = ((num4 != 64) ? 3 : ((num3 == 64) ? 1 : 2));
						if (ptr6 + num5 > ptr7)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Array too small."), "bytes"));
						}
						*ptr6 = (byte)((num << 2) | ((num2 >> 4) & 3));
						if (num5 > 1)
						{
							ptr6[1] = (byte)((num2 << 4) | ((num3 >> 2) & 0xF));
							if (num5 > 2)
							{
								ptr6[2] = (byte)((num3 << 6) | (num4 & 0x3F));
							}
						}
						ptr6 += num5;
					}
					return (int)(ptr6 - ptr3);
				}
			}
		}
	}

	public override int GetMaxCharCount(int byteCount)
	{
		if (byteCount < 0 || byteCount > 1610612731)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("byteCount", SR.GetString("The value of this argument must fall within the range {0} to {1}.", 0, 1610612731)));
		}
		return (byteCount + 2) / 3 * 4;
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
						byte* ptr4 = ptr2;
						byte* ptr5 = ptr4 + byteCount - 3;
						char* ptr6 = ptr3;
						while (ptr4 <= ptr5)
						{
							*ptr6 = ptr[*ptr4 >> 2];
							ptr6[1] = ptr[((*ptr4 & 3) << 4) | (ptr4[1] >> 4)];
							ptr6[2] = ptr[((ptr4[1] & 0xF) << 2) | (ptr4[2] >> 6)];
							ptr6[3] = ptr[ptr4[2] & 0x3F];
							ptr4 += 3;
							ptr6 += 4;
						}
						if (ptr4 - ptr5 == 2)
						{
							*ptr6 = ptr[*ptr4 >> 2];
							ptr6[1] = ptr[(*ptr4 & 3) << 4];
							ptr6[2] = '=';
							ptr6[3] = '=';
						}
						else if (ptr4 - ptr5 == 1)
						{
							*ptr6 = ptr[*ptr4 >> 2];
							ptr6[1] = ptr[((*ptr4 & 3) << 4) | (ptr4[1] >> 4)];
							ptr6[2] = ptr[(ptr4[1] & 0xF) << 2];
							ptr6[3] = '=';
						}
					}
				}
			}
		}
		return charCount;
	}

	[SecuritySafeCritical]
	public unsafe int GetChars(byte[] bytes, int byteIndex, int byteCount, byte[] chars, int charIndex)
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
			fixed (byte* ptr = val2byte)
			{
				fixed (byte* ptr2 = &bytes[byteIndex])
				{
					fixed (byte* ptr3 = &chars[charIndex])
					{
						byte* ptr4 = ptr2;
						byte* ptr5 = ptr4 + byteCount - 3;
						byte* ptr6 = ptr3;
						while (ptr4 <= ptr5)
						{
							*ptr6 = ptr[*ptr4 >> 2];
							ptr6[1] = ptr[((*ptr4 & 3) << 4) | (ptr4[1] >> 4)];
							ptr6[2] = ptr[((ptr4[1] & 0xF) << 2) | (ptr4[2] >> 6)];
							ptr6[3] = ptr[ptr4[2] & 0x3F];
							ptr4 += 3;
							ptr6 += 4;
						}
						if (ptr4 - ptr5 == 2)
						{
							*ptr6 = ptr[*ptr4 >> 2];
							ptr6[1] = ptr[(*ptr4 & 3) << 4];
							ptr6[2] = 61;
							ptr6[3] = 61;
						}
						else if (ptr4 - ptr5 == 1)
						{
							*ptr6 = ptr[*ptr4 >> 2];
							ptr6[1] = ptr[((*ptr4 & 3) << 4) | (ptr4[1] >> 4)];
							ptr6[2] = ptr[(ptr4[1] & 0xF) << 2];
							ptr6[3] = 61;
						}
					}
				}
			}
		}
		return charCount;
	}
}
