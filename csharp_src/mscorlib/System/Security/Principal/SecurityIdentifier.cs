using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Principal;

[ComVisible(false)]
public sealed class SecurityIdentifier : IdentityReference, IComparable<SecurityIdentifier>
{
	private byte[] buffer;

	public static readonly int MaxBinaryLength = 68;

	public static readonly int MinBinaryLength = 8;

	public SecurityIdentifier AccountDomainSid
	{
		get
		{
			if (!Value.StartsWith("S-1-5-21") || buffer[1] < 4)
			{
				return null;
			}
			byte[] array = new byte[24];
			Array.Copy(buffer, 0, array, 0, array.Length);
			array[1] = 4;
			return new SecurityIdentifier(array, 0);
		}
	}

	public int BinaryLength => buffer.Length;

	public override string Value
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			ulong sidAuthority = GetSidAuthority();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "S-1-{0}", sidAuthority);
			for (byte b = 0; b < GetSidSubAuthorityCount(); b++)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "-{0}", GetSidSubAuthority(b));
			}
			return stringBuilder.ToString();
		}
	}

	public SecurityIdentifier(string sddlForm)
	{
		if (sddlForm == null)
		{
			throw new ArgumentNullException("sddlForm");
		}
		buffer = ParseSddlForm(sddlForm);
	}

	public unsafe SecurityIdentifier(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - 2)
		{
			throw new ArgumentException("offset");
		}
		fixed (byte* ptr = binaryForm)
		{
			CreateFromBinaryForm((IntPtr)(ptr + offset), binaryForm.Length - offset);
		}
	}

	public SecurityIdentifier(IntPtr binaryForm)
	{
		CreateFromBinaryForm(binaryForm, int.MaxValue);
	}

	private void CreateFromBinaryForm(IntPtr binaryForm, int length)
	{
		byte num = Marshal.ReadByte(binaryForm, 0);
		int num2 = Marshal.ReadByte(binaryForm, 1);
		if (num != 1 || num2 > 15)
		{
			throw new ArgumentException("Value was invalid.");
		}
		if (length < 8 + num2 * 4)
		{
			throw new ArgumentException("offset");
		}
		buffer = new byte[8 + num2 * 4];
		Marshal.Copy(binaryForm, buffer, 0, buffer.Length);
	}

	public SecurityIdentifier(WellKnownSidType sidType, SecurityIdentifier domainSid)
	{
		WellKnownAccount wellKnownAccount = WellKnownAccount.LookupByType(sidType);
		if (wellKnownAccount == null)
		{
			throw new ArgumentException("Unable to convert SID type: " + sidType);
		}
		if (wellKnownAccount.IsAbsolute)
		{
			buffer = ParseSddlForm(wellKnownAccount.Sid);
			return;
		}
		if (domainSid == null)
		{
			throw new ArgumentNullException("domainSid");
		}
		buffer = ParseSddlForm(domainSid.Value + "-" + wellKnownAccount.Rid);
	}

	private ulong GetSidAuthority()
	{
		return ((ulong)buffer[2] << 40) | ((ulong)buffer[3] << 32) | ((ulong)buffer[4] << 24) | ((ulong)buffer[5] << 16) | ((ulong)buffer[6] << 8) | buffer[7];
	}

	private byte GetSidSubAuthorityCount()
	{
		return buffer[1];
	}

	private uint GetSidSubAuthority(byte index)
	{
		int num = 8 + index * 4;
		return (uint)(buffer[num] | (buffer[num + 1] << 8) | (buffer[num + 2] << 16) | (buffer[num + 3] << 24));
	}

	public int CompareTo(SecurityIdentifier sid)
	{
		if (sid == null)
		{
			throw new ArgumentNullException("sid");
		}
		int result;
		if ((result = GetSidAuthority().CompareTo(sid.GetSidAuthority())) != 0)
		{
			return result;
		}
		if ((result = GetSidSubAuthorityCount().CompareTo(sid.GetSidSubAuthorityCount())) != 0)
		{
			return result;
		}
		for (byte b = 0; b < GetSidSubAuthorityCount(); b++)
		{
			if ((result = GetSidSubAuthority(b).CompareTo(sid.GetSidSubAuthority(b))) != 0)
			{
				return result;
			}
		}
		return 0;
	}

	public override bool Equals(object o)
	{
		return Equals(o as SecurityIdentifier);
	}

	public bool Equals(SecurityIdentifier sid)
	{
		if (sid == null)
		{
			return false;
		}
		return sid.Value == Value;
	}

	public void GetBinaryForm(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - buffer.Length)
		{
			throw new ArgumentException("offset");
		}
		Array.Copy(buffer, 0, binaryForm, offset, buffer.Length);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	public bool IsAccountSid()
	{
		return AccountDomainSid != null;
	}

	public bool IsEqualDomainSid(SecurityIdentifier sid)
	{
		SecurityIdentifier accountDomainSid = AccountDomainSid;
		if (accountDomainSid == null)
		{
			return false;
		}
		return accountDomainSid.Equals(sid.AccountDomainSid);
	}

	public override bool IsValidTargetType(Type targetType)
	{
		if (targetType == typeof(SecurityIdentifier))
		{
			return true;
		}
		if (targetType == typeof(NTAccount))
		{
			return true;
		}
		return false;
	}

	public bool IsWellKnown(WellKnownSidType type)
	{
		WellKnownAccount wellKnownAccount = WellKnownAccount.LookupByType(type);
		if (wellKnownAccount == null)
		{
			return false;
		}
		string value = Value;
		if (wellKnownAccount.IsAbsolute)
		{
			return value == wellKnownAccount.Sid;
		}
		if (value.StartsWith("S-1-5-21", StringComparison.OrdinalIgnoreCase))
		{
			return value.EndsWith("-" + wellKnownAccount.Rid, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public override string ToString()
	{
		return Value;
	}

	public override IdentityReference Translate(Type targetType)
	{
		if (targetType == typeof(SecurityIdentifier))
		{
			return this;
		}
		if (targetType == typeof(NTAccount))
		{
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupBySid(Value);
			if (wellKnownAccount == null || wellKnownAccount.Name == null)
			{
				throw new IdentityNotMappedException("Unable to map SID: " + Value);
			}
			return new NTAccount(wellKnownAccount.Name);
		}
		throw new ArgumentException("Unknown type.", "targetType");
	}

	public static bool operator ==(SecurityIdentifier left, SecurityIdentifier right)
	{
		if ((object)left == null)
		{
			return (object)right == null;
		}
		if ((object)right == null)
		{
			return false;
		}
		return left.Value == right.Value;
	}

	public static bool operator !=(SecurityIdentifier left, SecurityIdentifier right)
	{
		if ((object)left == null)
		{
			return (object)right != null;
		}
		if ((object)right == null)
		{
			return true;
		}
		return left.Value != right.Value;
	}

	internal string GetSddlForm()
	{
		string value = Value;
		WellKnownAccount wellKnownAccount = WellKnownAccount.LookupBySid(value);
		if (wellKnownAccount == null || wellKnownAccount.SddlForm == null)
		{
			return value;
		}
		return wellKnownAccount.SddlForm;
	}

	internal static SecurityIdentifier ParseSddlForm(string sddlForm, ref int pos)
	{
		if (sddlForm.Length - pos < 2)
		{
			throw new ArgumentException("Invalid SDDL string.", "sddlForm");
		}
		string text = sddlForm.Substring(pos, 2).ToUpperInvariant();
		string sddlForm2;
		int num2;
		if (text == "S-")
		{
			int num = pos;
			char c = char.ToUpperInvariant(sddlForm[num]);
			while (true)
			{
				switch (c)
				{
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case 'S':
				case 'X':
					goto IL_004b;
				default:
					if (c >= 'A' && c <= 'F')
					{
						goto IL_004b;
					}
					break;
				}
				break;
				IL_004b:
				num++;
				c = char.ToUpperInvariant(sddlForm[num]);
			}
			if (c == ':' && sddlForm[num - 1] == 'D')
			{
				num--;
			}
			sddlForm2 = sddlForm.Substring(pos, num - pos);
			num2 = num - pos;
		}
		else
		{
			sddlForm2 = text;
			num2 = 2;
		}
		SecurityIdentifier result = new SecurityIdentifier(sddlForm2);
		pos += num2;
		return result;
	}

	private static byte[] ParseSddlForm(string sddlForm)
	{
		string text = sddlForm;
		if (sddlForm.Length == 2)
		{
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupBySddlForm(sddlForm);
			if (wellKnownAccount == null)
			{
				throw new ArgumentException("Invalid SDDL string - unrecognized account: " + sddlForm, "sddlForm");
			}
			if (!wellKnownAccount.IsAbsolute)
			{
				throw new NotImplementedException("Mono unable to convert account to SID: " + ((wellKnownAccount.Name != null) ? wellKnownAccount.Name : sddlForm));
			}
			text = wellKnownAccount.Sid;
		}
		string[] array = text.ToUpperInvariant().Split('-');
		int num = array.Length - 3;
		if (array.Length < 3 || array[0] != "S" || num > 15)
		{
			throw new ArgumentException("Value was invalid.");
		}
		if (array[1] != "1")
		{
			throw new ArgumentException("Only SIDs with revision 1 are supported");
		}
		byte[] array2 = new byte[8 + num * 4];
		array2[0] = 1;
		array2[1] = (byte)num;
		if (!TryParseAuthority(array[2], out var result))
		{
			throw new ArgumentException("Value was invalid.");
		}
		array2[2] = (byte)((result >> 40) & 0xFF);
		array2[3] = (byte)((result >> 32) & 0xFF);
		array2[4] = (byte)((result >> 24) & 0xFF);
		array2[5] = (byte)((result >> 16) & 0xFF);
		array2[6] = (byte)((result >> 8) & 0xFF);
		array2[7] = (byte)(result & 0xFF);
		for (int i = 0; i < num; i++)
		{
			if (!TryParseSubAuthority(array[i + 3], out var result2))
			{
				throw new ArgumentException("Value was invalid.");
			}
			int num2 = 8 + i * 4;
			array2[num2] = (byte)result2;
			array2[num2 + 1] = (byte)(result2 >> 8);
			array2[num2 + 2] = (byte)(result2 >> 16);
			array2[num2 + 3] = (byte)(result2 >> 24);
		}
		return array2;
	}

	private static bool TryParseAuthority(string s, out ulong result)
	{
		if (s.StartsWith("0X"))
		{
			return ulong.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
		}
		return ulong.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	}

	private static bool TryParseSubAuthority(string s, out uint result)
	{
		if (s.StartsWith("0X"))
		{
			return uint.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
		}
		return uint.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	}
}
