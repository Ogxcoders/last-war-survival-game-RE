using System.Runtime.Serialization;
using System.Security;

namespace System.Xml;

public class UniqueId
{
	private long idLow;

	private long idHigh;

	[SecurityCritical]
	private string s;

	private const int guidLength = 16;

	private const int uuidLength = 45;

	private static short[] char2val = new short[256]
	{
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 0, 16,
		32, 48, 64, 80, 96, 112, 128, 144, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 160, 176, 192,
		208, 224, 240, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 0, 1, 2, 3,
		4, 5, 6, 7, 8, 9, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 10, 11, 12, 13, 14,
		15, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
		256, 256, 256, 256, 256, 256
	};

	private const string val2char = "0123456789abcdef";

	public int CharArrayLength
	{
		[SecuritySafeCritical]
		get
		{
			if (s != null)
			{
				return s.Length;
			}
			return 45;
		}
	}

	public bool IsGuid => (idLow | idHigh) != 0;

	public UniqueId()
		: this(Guid.NewGuid())
	{
	}

	public UniqueId(Guid guid)
		: this(guid.ToByteArray())
	{
	}

	public UniqueId(byte[] guid)
		: this(guid, 0)
	{
	}

	[SecuritySafeCritical]
	public unsafe UniqueId(byte[] guid, int offset)
	{
		if (guid == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("guid"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > guid.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", guid.Length)));
		}
		if (16 > guid.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Array too small.  Length of available data must be at least {0}.", 16), "guid"));
		}
		fixed (byte* ptr = &guid[offset])
		{
			idLow = UnsafeGetInt64(ptr);
			idHigh = UnsafeGetInt64(ptr + 8);
		}
	}

	[SecuritySafeCritical]
	public unsafe UniqueId(string value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		if (value.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("UniqueId cannot be zero length.")));
		}
		fixed (char* chars = value)
		{
			UnsafeParse(chars, value.Length);
		}
		s = value;
	}

	[SecuritySafeCritical]
	public unsafe UniqueId(char[] chars, int offset, int count)
	{
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		if (count == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("UniqueId cannot be zero length.")));
		}
		fixed (char* chars2 = &chars[offset])
		{
			UnsafeParse(chars2, count);
		}
		if (!IsGuid)
		{
			s = new string(chars, offset, count);
		}
	}

	[SecurityCritical]
	private unsafe int UnsafeDecode(short* char2val, char ch1, char ch2)
	{
		if ((ch1 | ch2) >= 128)
		{
			return 256;
		}
		return char2val[(int)ch1] | char2val[128 + ch2];
	}

	[SecurityCritical]
	private unsafe void UnsafeEncode(char* val2char, byte b, char* pch)
	{
		*pch = val2char[b >> 4];
		pch[1] = val2char[b & 0xF];
	}

	[SecurityCritical]
	private unsafe void UnsafeParse(char* chars, int charCount)
	{
		if (charCount != 45 || *chars != 'u' || chars[1] != 'r' || chars[2] != 'n' || chars[3] != ':' || chars[4] != 'u' || chars[5] != 'u' || chars[6] != 'i' || chars[7] != 'd' || chars[8] != ':' || chars[17] != '-' || chars[22] != '-' || chars[27] != '-' || chars[32] != '-')
		{
			return;
		}
		byte* ptr = stackalloc byte[16];
		int num = 0;
		fixed (short* ptr2 = char2val)
		{
			short* ptr3 = ptr2;
			num = UnsafeDecode(ptr3, chars[15], chars[16]);
			*ptr = (byte)num;
			int num2 = 0 | num;
			num = UnsafeDecode(ptr3, chars[13], chars[14]);
			ptr[1] = (byte)num;
			int num3 = num2 | num;
			num = UnsafeDecode(ptr3, chars[11], chars[12]);
			ptr[2] = (byte)num;
			int num4 = num3 | num;
			num = UnsafeDecode(ptr3, chars[9], chars[10]);
			ptr[3] = (byte)num;
			int num5 = num4 | num;
			num = UnsafeDecode(ptr3, chars[20], chars[21]);
			ptr[4] = (byte)num;
			int num6 = num5 | num;
			num = UnsafeDecode(ptr3, chars[18], chars[19]);
			ptr[5] = (byte)num;
			int num7 = num6 | num;
			num = UnsafeDecode(ptr3, chars[25], chars[26]);
			ptr[6] = (byte)num;
			int num8 = num7 | num;
			num = UnsafeDecode(ptr3, chars[23], chars[24]);
			ptr[7] = (byte)num;
			int num9 = num8 | num;
			num = UnsafeDecode(ptr3, chars[28], chars[29]);
			ptr[8] = (byte)num;
			int num10 = num9 | num;
			num = UnsafeDecode(ptr3, chars[30], chars[31]);
			ptr[9] = (byte)num;
			int num11 = num10 | num;
			num = UnsafeDecode(ptr3, chars[33], chars[34]);
			ptr[10] = (byte)num;
			int num12 = num11 | num;
			num = UnsafeDecode(ptr3, chars[35], chars[36]);
			ptr[11] = (byte)num;
			int num13 = num12 | num;
			num = UnsafeDecode(ptr3, chars[37], chars[38]);
			ptr[12] = (byte)num;
			int num14 = num13 | num;
			num = UnsafeDecode(ptr3, chars[39], chars[40]);
			ptr[13] = (byte)num;
			int num15 = num14 | num;
			num = UnsafeDecode(ptr3, chars[41], chars[42]);
			ptr[14] = (byte)num;
			int num16 = num15 | num;
			num = UnsafeDecode(ptr3, chars[43], chars[44]);
			ptr[15] = (byte)num;
			if ((num16 | num) >= 256)
			{
				return;
			}
			idLow = UnsafeGetInt64(ptr);
			idHigh = UnsafeGetInt64(ptr + 8);
		}
	}

	[SecuritySafeCritical]
	public unsafe int ToCharArray(char[] chars, int offset)
	{
		int charArrayLength = CharArrayLength;
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (charArrayLength > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("chars", SR.GetString("Array too small.  Must be able to hold at least {0}.", charArrayLength)));
		}
		if (s != null)
		{
			s.CopyTo(0, chars, offset, charArrayLength);
		}
		else
		{
			byte* ptr = stackalloc byte[16];
			UnsafeSetInt64(idLow, ptr);
			UnsafeSetInt64(idHigh, ptr + 8);
			fixed (char* ptr2 = &chars[offset])
			{
				*ptr2 = 'u';
				ptr2[1] = 'r';
				ptr2[2] = 'n';
				ptr2[3] = ':';
				ptr2[4] = 'u';
				ptr2[5] = 'u';
				ptr2[6] = 'i';
				ptr2[7] = 'd';
				ptr2[8] = ':';
				ptr2[17] = '-';
				ptr2[22] = '-';
				ptr2[27] = '-';
				ptr2[32] = '-';
				fixed (char* ptr3 = "0123456789abcdef")
				{
					char* ptr4 = ptr3;
					UnsafeEncode(ptr4, *ptr, ptr2 + 15);
					UnsafeEncode(ptr4, ptr[1], ptr2 + 13);
					UnsafeEncode(ptr4, ptr[2], ptr2 + 11);
					UnsafeEncode(ptr4, ptr[3], ptr2 + 9);
					UnsafeEncode(ptr4, ptr[4], ptr2 + 20);
					UnsafeEncode(ptr4, ptr[5], ptr2 + 18);
					UnsafeEncode(ptr4, ptr[6], ptr2 + 25);
					UnsafeEncode(ptr4, ptr[7], ptr2 + 23);
					UnsafeEncode(ptr4, ptr[8], ptr2 + 28);
					UnsafeEncode(ptr4, ptr[9], ptr2 + 30);
					UnsafeEncode(ptr4, ptr[10], ptr2 + 33);
					UnsafeEncode(ptr4, ptr[11], ptr2 + 35);
					UnsafeEncode(ptr4, ptr[12], ptr2 + 37);
					UnsafeEncode(ptr4, ptr[13], ptr2 + 39);
					UnsafeEncode(ptr4, ptr[14], ptr2 + 41);
					UnsafeEncode(ptr4, ptr[15], ptr2 + 43);
				}
			}
		}
		return charArrayLength;
	}

	public bool TryGetGuid(out Guid guid)
	{
		byte[] array = new byte[16];
		if (!TryGetGuid(array, 0))
		{
			guid = Guid.Empty;
			return false;
		}
		guid = new Guid(array);
		return true;
	}

	[SecuritySafeCritical]
	public unsafe bool TryGetGuid(byte[] buffer, int offset)
	{
		if (!IsGuid)
		{
			return false;
		}
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > buffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
		}
		if (16 > buffer.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("buffer", SR.GetString("Array too small.  Must be able to hold at least {0}.", 16)));
		}
		fixed (byte* ptr = &buffer[offset])
		{
			UnsafeSetInt64(idLow, ptr);
			UnsafeSetInt64(idHigh, ptr + 8);
		}
		return true;
	}

	[SecuritySafeCritical]
	public override string ToString()
	{
		if (s == null)
		{
			int charArrayLength = CharArrayLength;
			char[] array = new char[charArrayLength];
			ToCharArray(array, 0);
			s = new string(array, 0, charArrayLength);
		}
		return s;
	}

	public static bool operator ==(UniqueId id1, UniqueId id2)
	{
		if ((object)id1 == null && (object)id2 == null)
		{
			return true;
		}
		if ((object)id1 == null || (object)id2 == null)
		{
			return false;
		}
		if (id1.IsGuid && id2.IsGuid)
		{
			if (id1.idLow == id2.idLow)
			{
				return id1.idHigh == id2.idHigh;
			}
			return false;
		}
		return id1.ToString() == id2.ToString();
	}

	public static bool operator !=(UniqueId id1, UniqueId id2)
	{
		return !(id1 == id2);
	}

	public override bool Equals(object obj)
	{
		return this == obj as UniqueId;
	}

	public override int GetHashCode()
	{
		if (IsGuid)
		{
			long num = idLow ^ idHigh;
			return (int)(num >> 32) ^ (int)num;
		}
		return ToString().GetHashCode();
	}

	[SecurityCritical]
	private unsafe long UnsafeGetInt64(byte* pb)
	{
		int num = UnsafeGetInt32(pb);
		return ((long)UnsafeGetInt32(pb + 4) << 32) | (uint)num;
	}

	[SecurityCritical]
	private unsafe int UnsafeGetInt32(byte* pb)
	{
		return (((((pb[3] << 8) | pb[2]) << 8) | pb[1]) << 8) | *pb;
	}

	[SecurityCritical]
	private unsafe void UnsafeSetInt64(long value, byte* pb)
	{
		UnsafeSetInt32((int)value, pb);
		UnsafeSetInt32((int)(value >> 32), pb + 4);
	}

	[SecurityCritical]
	private unsafe void UnsafeSetInt32(int value, byte* pb)
	{
		*pb = (byte)value;
		value >>= 8;
		pb[1] = (byte)value;
		value >>= 8;
		pb[2] = (byte)value;
		value >>= 8;
		pb[3] = (byte)value;
	}
}
