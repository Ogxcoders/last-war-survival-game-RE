using System.Globalization;
using System.Security.Principal;
using System.Text;

namespace System.Security.AccessControl;

public abstract class GenericAce
{
	private AceFlags ace_flags;

	private AceType ace_type;

	public AceFlags AceFlags
	{
		get
		{
			return ace_flags;
		}
		set
		{
			ace_flags = value;
		}
	}

	public AceType AceType => ace_type;

	public AuditFlags AuditFlags
	{
		get
		{
			AuditFlags auditFlags = AuditFlags.None;
			if ((ace_flags & AceFlags.SuccessfulAccess) != AceFlags.None)
			{
				auditFlags |= AuditFlags.Success;
			}
			if ((ace_flags & AceFlags.FailedAccess) != AceFlags.None)
			{
				auditFlags |= AuditFlags.Failure;
			}
			return auditFlags;
		}
	}

	public abstract int BinaryLength { get; }

	public InheritanceFlags InheritanceFlags
	{
		get
		{
			InheritanceFlags inheritanceFlags = InheritanceFlags.None;
			if ((ace_flags & AceFlags.ObjectInherit) != AceFlags.None)
			{
				inheritanceFlags |= InheritanceFlags.ObjectInherit;
			}
			if ((ace_flags & AceFlags.ContainerInherit) != AceFlags.None)
			{
				inheritanceFlags |= InheritanceFlags.ContainerInherit;
			}
			return inheritanceFlags;
		}
	}

	public bool IsInherited => (ace_flags & AceFlags.Inherited) != 0;

	public PropagationFlags PropagationFlags
	{
		get
		{
			PropagationFlags propagationFlags = PropagationFlags.None;
			if ((ace_flags & AceFlags.InheritOnly) != AceFlags.None)
			{
				propagationFlags |= PropagationFlags.InheritOnly;
			}
			if ((ace_flags & AceFlags.NoPropagateInherit) != AceFlags.None)
			{
				propagationFlags |= PropagationFlags.NoPropagateInherit;
			}
			return propagationFlags;
		}
	}

	internal GenericAce(AceType type, AceFlags flags)
	{
		if ((int)type > 16)
		{
			throw new ArgumentOutOfRangeException("type");
		}
		ace_type = type;
		ace_flags = flags;
	}

	internal GenericAce(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - 2)
		{
			throw new ArgumentOutOfRangeException("offset", offset, "Offset out of range");
		}
		ace_type = (AceType)binaryForm[offset];
		ace_flags = (AceFlags)binaryForm[offset + 1];
	}

	public GenericAce Copy()
	{
		byte[] binaryForm = new byte[BinaryLength];
		GetBinaryForm(binaryForm, 0);
		return CreateFromBinaryForm(binaryForm, 0);
	}

	public static GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - 1)
		{
			throw new ArgumentOutOfRangeException("offset", offset, "Offset out of range");
		}
		if (IsObjectType((AceType)binaryForm[offset]))
		{
			return new ObjectAce(binaryForm, offset);
		}
		return new CommonAce(binaryForm, offset);
	}

	public sealed override bool Equals(object o)
	{
		return this == o as GenericAce;
	}

	public abstract void GetBinaryForm(byte[] binaryForm, int offset);

	public sealed override int GetHashCode()
	{
		byte[] array = new byte[BinaryLength];
		GetBinaryForm(array, 0);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num = (num << 3) | ((num >> 29) & 7);
			num ^= array[i] & 0xFF;
		}
		return num;
	}

	public static bool operator ==(GenericAce left, GenericAce right)
	{
		if ((object)left == null)
		{
			return (object)right == null;
		}
		if ((object)right == null)
		{
			return false;
		}
		int binaryLength = left.BinaryLength;
		int binaryLength2 = right.BinaryLength;
		if (binaryLength != binaryLength2)
		{
			return false;
		}
		byte[] array = new byte[binaryLength];
		byte[] array2 = new byte[binaryLength2];
		left.GetBinaryForm(array, 0);
		right.GetBinaryForm(array2, 0);
		for (int i = 0; i < binaryLength; i++)
		{
			if (array[i] != array2[i])
			{
				return false;
			}
		}
		return true;
	}

	public static bool operator !=(GenericAce left, GenericAce right)
	{
		if ((object)left == null)
		{
			return (object)right != null;
		}
		if ((object)right == null)
		{
			return true;
		}
		int binaryLength = left.BinaryLength;
		int binaryLength2 = right.BinaryLength;
		if (binaryLength != binaryLength2)
		{
			return true;
		}
		byte[] array = new byte[binaryLength];
		byte[] array2 = new byte[binaryLength2];
		left.GetBinaryForm(array, 0);
		right.GetBinaryForm(array2, 0);
		for (int i = 0; i < binaryLength; i++)
		{
			if (array[i] != array2[i])
			{
				return true;
			}
		}
		return false;
	}

	internal abstract string GetSddlForm();

	internal static GenericAce CreateFromSddlForm(string sddlForm, ref int pos)
	{
		if (sddlForm[pos] != '(')
		{
			throw new ArgumentException("Invalid SDDL string.", "sddlForm");
		}
		int num = sddlForm.IndexOf(')', pos);
		if (num < 0)
		{
			throw new ArgumentException("Invalid SDDL string.", "sddlForm");
		}
		int length = num - (pos + 1);
		string[] array = sddlForm.Substring(pos + 1, length).ToUpperInvariant().Split(';');
		if (array.Length != 6)
		{
			throw new ArgumentException("Invalid SDDL string.", "sddlForm");
		}
		ObjectAceFlags objectAceFlags = ObjectAceFlags.None;
		AceType aceType = ParseSddlAceType(array[0]);
		AceFlags flags = ParseSddlAceFlags(array[1]);
		int accessMask = ParseSddlAccessRights(array[2]);
		Guid objType = Guid.Empty;
		if (!string.IsNullOrEmpty(array[3]))
		{
			objType = new Guid(array[3]);
			objectAceFlags |= ObjectAceFlags.ObjectAceTypePresent;
		}
		Guid inheritedType = Guid.Empty;
		if (!string.IsNullOrEmpty(array[4]))
		{
			inheritedType = new Guid(array[4]);
			objectAceFlags |= ObjectAceFlags.InheritedObjectAceTypePresent;
		}
		SecurityIdentifier sid = new SecurityIdentifier(array[5]);
		if (aceType == AceType.AccessAllowedCallback || aceType == AceType.AccessDeniedCallback)
		{
			throw new NotImplementedException("Conditional ACEs not supported");
		}
		pos = num + 1;
		if (IsObjectType(aceType))
		{
			return new ObjectAce(aceType, flags, accessMask, sid, objectAceFlags, objType, inheritedType, null);
		}
		if (objectAceFlags != ObjectAceFlags.None)
		{
			throw new ArgumentException("Invalid SDDL string.", "sddlForm");
		}
		return new CommonAce(aceType, flags, accessMask, sid, null);
	}

	private static bool IsObjectType(AceType type)
	{
		if (type != AceType.AccessAllowedCallbackObject && type != AceType.AccessAllowedObject && type != AceType.AccessDeniedCallbackObject && type != AceType.AccessDeniedObject && type != AceType.SystemAlarmCallbackObject && type != AceType.SystemAlarmObject && type != AceType.SystemAuditCallbackObject)
		{
			return type == AceType.SystemAuditObject;
		}
		return true;
	}

	internal static string GetSddlAceType(AceType type)
	{
		return type switch
		{
			AceType.AccessAllowed => "A", 
			AceType.AccessDenied => "D", 
			AceType.AccessAllowedObject => "OA", 
			AceType.AccessDeniedObject => "OD", 
			AceType.SystemAudit => "AU", 
			AceType.SystemAlarm => "AL", 
			AceType.SystemAuditObject => "OU", 
			AceType.SystemAlarmObject => "OL", 
			AceType.AccessAllowedCallback => "XA", 
			AceType.AccessDeniedCallback => "XD", 
			_ => throw new ArgumentException("Unable to convert to SDDL ACE type: " + type, "type"), 
		};
	}

	private static AceType ParseSddlAceType(string type)
	{
		return type switch
		{
			"A" => AceType.AccessAllowed, 
			"D" => AceType.AccessDenied, 
			"OA" => AceType.AccessAllowedObject, 
			"OD" => AceType.AccessDeniedObject, 
			"AU" => AceType.SystemAudit, 
			"AL" => AceType.SystemAlarm, 
			"OU" => AceType.SystemAuditObject, 
			"OL" => AceType.SystemAlarmObject, 
			"XA" => AceType.AccessAllowedCallback, 
			"XD" => AceType.AccessDeniedCallback, 
			_ => throw new ArgumentException("Unable to convert SDDL to ACE type: " + type, "type"), 
		};
	}

	internal static string GetSddlAceFlags(AceFlags flags)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if ((flags & AceFlags.ObjectInherit) != AceFlags.None)
		{
			stringBuilder.Append("OI");
		}
		if ((flags & AceFlags.ContainerInherit) != AceFlags.None)
		{
			stringBuilder.Append("CI");
		}
		if ((flags & AceFlags.NoPropagateInherit) != AceFlags.None)
		{
			stringBuilder.Append("NP");
		}
		if ((flags & AceFlags.InheritOnly) != AceFlags.None)
		{
			stringBuilder.Append("IO");
		}
		if ((flags & AceFlags.Inherited) != AceFlags.None)
		{
			stringBuilder.Append("ID");
		}
		if ((flags & AceFlags.SuccessfulAccess) != AceFlags.None)
		{
			stringBuilder.Append("SA");
		}
		if ((flags & AceFlags.FailedAccess) != AceFlags.None)
		{
			stringBuilder.Append("FA");
		}
		return stringBuilder.ToString();
	}

	private static AceFlags ParseSddlAceFlags(string flags)
	{
		AceFlags aceFlags = AceFlags.None;
		int i;
		for (i = 0; i < flags.Length - 1; i += 2)
		{
			aceFlags = flags.Substring(i, 2) switch
			{
				"CI" => aceFlags | AceFlags.ContainerInherit, 
				"OI" => aceFlags | AceFlags.ObjectInherit, 
				"NP" => aceFlags | AceFlags.NoPropagateInherit, 
				"IO" => aceFlags | AceFlags.InheritOnly, 
				"ID" => aceFlags | AceFlags.Inherited, 
				"SA" => aceFlags | AceFlags.SuccessfulAccess, 
				"FA" => aceFlags | AceFlags.FailedAccess, 
				_ => throw new ArgumentException("Invalid SDDL string.", "flags"), 
			};
		}
		if (i != flags.Length)
		{
			throw new ArgumentException("Invalid SDDL string.", "flags");
		}
		return aceFlags;
	}

	private static int ParseSddlAccessRights(string accessMask)
	{
		if (accessMask.StartsWith("0X"))
		{
			return int.Parse(accessMask.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
		if (char.IsDigit(accessMask, 0))
		{
			return int.Parse(accessMask, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}
		return ParseSddlAliasRights(accessMask);
	}

	private static int ParseSddlAliasRights(string accessMask)
	{
		int num = 0;
		int i;
		for (i = 0; i < accessMask.Length - 1; i += 2)
		{
			SddlAccessRight sddlAccessRight = SddlAccessRight.LookupByName(accessMask.Substring(i, 2));
			if (sddlAccessRight == null)
			{
				throw new ArgumentException("Invalid SDDL string.", "accessMask");
			}
			num |= sddlAccessRight.Value;
		}
		if (i != accessMask.Length)
		{
			throw new ArgumentException("Invalid SDDL string.", "accessMask");
		}
		return num;
	}

	internal static ushort ReadUShort(byte[] buffer, int offset)
	{
		return (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
	}

	internal static int ReadInt(byte[] buffer, int offset)
	{
		return buffer[offset] | (buffer[offset + 1] << 8) | (buffer[offset + 2] << 16) | (buffer[offset + 3] << 24);
	}

	internal static void WriteInt(int val, byte[] buffer, int offset)
	{
		buffer[offset] = (byte)val;
		buffer[offset + 1] = (byte)(val >> 8);
		buffer[offset + 2] = (byte)(val >> 16);
		buffer[offset + 3] = (byte)(val >> 24);
	}

	internal static void WriteUShort(ushort val, byte[] buffer, int offset)
	{
		buffer[offset] = (byte)val;
		buffer[offset + 1] = (byte)(val >> 8);
	}
}
