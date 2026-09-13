using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CompoundAce : KnownAce
{
	private CompoundAceType compound_ace_type;

	[MonoTODO]
	public override int BinaryLength
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CompoundAceType CompoundAceType
	{
		get
		{
			return compound_ace_type;
		}
		set
		{
			compound_ace_type = value;
		}
	}

	public CompoundAce(AceFlags flags, int accessMask, CompoundAceType compoundAceType, SecurityIdentifier sid)
		: base(AceType.AccessAllowedCompound, flags)
	{
		compound_ace_type = compoundAceType;
		base.AccessMask = accessMask;
		base.SecurityIdentifier = sid;
	}

	[MonoTODO]
	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
		throw new NotImplementedException();
	}

	internal override string GetSddlForm()
	{
		throw new NotImplementedException();
	}
}
