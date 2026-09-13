using System.Globalization;
using System.Security.Principal;
using System.Text;

namespace System.Security.AccessControl;

public abstract class KnownAce : GenericAce
{
	private int access_mask;

	private SecurityIdentifier identifier;

	public int AccessMask
	{
		get
		{
			return access_mask;
		}
		set
		{
			access_mask = value;
		}
	}

	public SecurityIdentifier SecurityIdentifier
	{
		get
		{
			return identifier;
		}
		set
		{
			identifier = value;
		}
	}

	internal KnownAce(AceType type, AceFlags flags)
		: base(type, flags)
	{
	}

	internal KnownAce(byte[] binaryForm, int offset)
		: base(binaryForm, offset)
	{
	}

	internal static string GetSddlAccessRights(int accessMask)
	{
		string sddlAliasRights = GetSddlAliasRights(accessMask);
		if (!string.IsNullOrEmpty(sddlAliasRights))
		{
			return sddlAliasRights;
		}
		return string.Format(CultureInfo.InvariantCulture, "0x{0:x}", accessMask);
	}

	private static string GetSddlAliasRights(int accessMask)
	{
		SddlAccessRight[] array = SddlAccessRight.Decompose(accessMask);
		if (array == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		SddlAccessRight[] array2 = array;
		foreach (SddlAccessRight sddlAccessRight in array2)
		{
			stringBuilder.Append(sddlAccessRight.Name);
		}
		return stringBuilder.ToString();
	}
}
