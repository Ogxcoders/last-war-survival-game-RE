using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RawSecurityDescriptor : GenericSecurityDescriptor
{
	private ControlFlags control_flags;

	private SecurityIdentifier owner_sid;

	private SecurityIdentifier group_sid;

	private RawAcl system_acl;

	private RawAcl discretionary_acl;

	private byte resourcemgr_control;

	public override ControlFlags ControlFlags => control_flags;

	public RawAcl DiscretionaryAcl
	{
		get
		{
			return discretionary_acl;
		}
		set
		{
			discretionary_acl = value;
		}
	}

	public override SecurityIdentifier Group
	{
		get
		{
			return group_sid;
		}
		set
		{
			group_sid = value;
		}
	}

	public override SecurityIdentifier Owner
	{
		get
		{
			return owner_sid;
		}
		set
		{
			owner_sid = value;
		}
	}

	public byte ResourceManagerControl
	{
		get
		{
			return resourcemgr_control;
		}
		set
		{
			resourcemgr_control = value;
		}
	}

	public RawAcl SystemAcl
	{
		get
		{
			return system_acl;
		}
		set
		{
			system_acl = value;
		}
	}

	internal override GenericAcl InternalDacl => DiscretionaryAcl;

	internal override GenericAcl InternalSacl => SystemAcl;

	internal override byte InternalReservedField => ResourceManagerControl;

	public RawSecurityDescriptor(string sddlForm)
	{
		if (sddlForm == null)
		{
			throw new ArgumentNullException("sddlForm");
		}
		ParseSddl(sddlForm.Replace(" ", ""));
		control_flags |= ControlFlags.SelfRelative;
	}

	public RawSecurityDescriptor(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - 20)
		{
			throw new ArgumentOutOfRangeException("offset", offset, "Offset out of range");
		}
		if (binaryForm[offset] != 1)
		{
			throw new ArgumentException("Unrecognized Security Descriptor revision.", "binaryForm");
		}
		resourcemgr_control = binaryForm[offset + 1];
		control_flags = (ControlFlags)ReadUShort(binaryForm, offset + 2);
		int num = ReadInt(binaryForm, offset + 4);
		int num2 = ReadInt(binaryForm, offset + 8);
		int num3 = ReadInt(binaryForm, offset + 12);
		int num4 = ReadInt(binaryForm, offset + 16);
		if (num != 0)
		{
			owner_sid = new SecurityIdentifier(binaryForm, num);
		}
		if (num2 != 0)
		{
			group_sid = new SecurityIdentifier(binaryForm, num2);
		}
		if (num3 != 0)
		{
			system_acl = new RawAcl(binaryForm, num3);
		}
		if (num4 != 0)
		{
			discretionary_acl = new RawAcl(binaryForm, num4);
		}
	}

	public RawSecurityDescriptor(ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl)
	{
		control_flags = flags;
		owner_sid = owner;
		group_sid = group;
		system_acl = systemAcl;
		discretionary_acl = discretionaryAcl;
	}

	public void SetFlags(ControlFlags flags)
	{
		control_flags = flags | ControlFlags.SelfRelative;
	}

	private void ParseSddl(string sddlForm)
	{
		ControlFlags sdFlags = ControlFlags.None;
		int pos = 0;
		while (pos < sddlForm.Length - 2)
		{
			switch (sddlForm.Substring(pos, 2))
			{
			case "O:":
				pos += 2;
				Owner = SecurityIdentifier.ParseSddlForm(sddlForm, ref pos);
				break;
			case "G:":
				pos += 2;
				Group = SecurityIdentifier.ParseSddlForm(sddlForm, ref pos);
				break;
			case "D:":
				pos += 2;
				DiscretionaryAcl = RawAcl.ParseSddlForm(sddlForm, isDacl: true, ref sdFlags, ref pos);
				sdFlags |= ControlFlags.DiscretionaryAclPresent;
				break;
			case "S:":
				pos += 2;
				SystemAcl = RawAcl.ParseSddlForm(sddlForm, isDacl: false, ref sdFlags, ref pos);
				sdFlags |= ControlFlags.SystemAclPresent;
				break;
			default:
				throw new ArgumentException("Invalid SDDL.", "sddlForm");
			}
		}
		if (pos != sddlForm.Length)
		{
			throw new ArgumentException("Invalid SDDL.", "sddlForm");
		}
		SetFlags(sdFlags);
	}

	private ushort ReadUShort(byte[] buffer, int offset)
	{
		return (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
	}

	private int ReadInt(byte[] buffer, int offset)
	{
		return buffer[offset] | (buffer[offset + 1] << 8) | (buffer[offset + 2] << 16) | (buffer[offset + 3] << 24);
	}
}
