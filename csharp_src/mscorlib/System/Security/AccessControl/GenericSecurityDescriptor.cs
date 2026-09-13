using System.Globalization;
using System.Security.Principal;
using System.Text;

namespace System.Security.AccessControl;

public abstract class GenericSecurityDescriptor
{
	public int BinaryLength
	{
		get
		{
			int num = 20;
			if (Owner != null)
			{
				num += Owner.BinaryLength;
			}
			if (Group != null)
			{
				num += Group.BinaryLength;
			}
			if (DaclPresent && !DaclIsUnmodifiedAefa)
			{
				num += InternalDacl.BinaryLength;
			}
			if (SaclPresent)
			{
				num += InternalSacl.BinaryLength;
			}
			return num;
		}
	}

	public abstract ControlFlags ControlFlags { get; }

	public abstract SecurityIdentifier Group { get; set; }

	public abstract SecurityIdentifier Owner { get; set; }

	public static byte Revision => 1;

	internal virtual GenericAcl InternalDacl => null;

	internal virtual GenericAcl InternalSacl => null;

	internal virtual byte InternalReservedField => 0;

	internal virtual bool DaclIsUnmodifiedAefa => false;

	private bool DaclPresent
	{
		get
		{
			if (InternalDacl != null)
			{
				return (ControlFlags & ControlFlags.DiscretionaryAclPresent) != 0;
			}
			return false;
		}
	}

	private bool SaclPresent
	{
		get
		{
			if (InternalSacl != null)
			{
				return (ControlFlags & ControlFlags.SystemAclPresent) != 0;
			}
			return false;
		}
	}

	public void GetBinaryForm(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		int binaryLength = BinaryLength;
		if (offset < 0 || offset > binaryForm.Length - binaryLength)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		ControlFlags controlFlags = ControlFlags;
		if (DaclIsUnmodifiedAefa)
		{
			controlFlags &= ~ControlFlags.DiscretionaryAclPresent;
		}
		binaryForm[offset] = Revision;
		binaryForm[offset + 1] = InternalReservedField;
		WriteUShort((ushort)controlFlags, binaryForm, offset + 2);
		int num = 20;
		if (Owner != null)
		{
			WriteInt(num, binaryForm, offset + 4);
			Owner.GetBinaryForm(binaryForm, offset + num);
			num += Owner.BinaryLength;
		}
		else
		{
			WriteInt(0, binaryForm, offset + 4);
		}
		if (Group != null)
		{
			WriteInt(num, binaryForm, offset + 8);
			Group.GetBinaryForm(binaryForm, offset + num);
			num += Group.BinaryLength;
		}
		else
		{
			WriteInt(0, binaryForm, offset + 8);
		}
		GenericAcl internalSacl = InternalSacl;
		if (SaclPresent)
		{
			WriteInt(num, binaryForm, offset + 12);
			internalSacl.GetBinaryForm(binaryForm, offset + num);
			num += InternalSacl.BinaryLength;
		}
		else
		{
			WriteInt(0, binaryForm, offset + 12);
		}
		GenericAcl internalDacl = InternalDacl;
		if (DaclPresent && !DaclIsUnmodifiedAefa)
		{
			WriteInt(num, binaryForm, offset + 16);
			internalDacl.GetBinaryForm(binaryForm, offset + num);
			num += InternalDacl.BinaryLength;
		}
		else
		{
			WriteInt(0, binaryForm, offset + 16);
		}
	}

	public string GetSddlForm(AccessControlSections includeSections)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None && Owner != null)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "O:{0}", Owner.GetSddlForm());
		}
		if ((includeSections & AccessControlSections.Group) != AccessControlSections.None && Group != null)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "G:{0}", Group.GetSddlForm());
		}
		if ((includeSections & AccessControlSections.Access) != AccessControlSections.None && DaclPresent && !DaclIsUnmodifiedAefa)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "D:{0}", InternalDacl.GetSddlForm(ControlFlags, isDacl: true));
		}
		if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None && SaclPresent)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "S:{0}", InternalSacl.GetSddlForm(ControlFlags, isDacl: false));
		}
		return stringBuilder.ToString();
	}

	public static bool IsSddlConversionSupported()
	{
		return true;
	}

	private void WriteUShort(ushort val, byte[] buffer, int offset)
	{
		buffer[offset] = (byte)val;
		buffer[offset + 1] = (byte)(val >> 8);
	}

	private void WriteInt(int val, byte[] buffer, int offset)
	{
		buffer[offset] = (byte)val;
		buffer[offset + 1] = (byte)(val >> 8);
		buffer[offset + 2] = (byte)(val >> 16);
		buffer[offset + 3] = (byte)(val >> 24);
	}
}
