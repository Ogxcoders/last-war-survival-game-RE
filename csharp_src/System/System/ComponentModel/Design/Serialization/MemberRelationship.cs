namespace System.ComponentModel.Design.Serialization;

public readonly struct MemberRelationship
{
	public static readonly MemberRelationship Empty;

	public bool IsEmpty => Owner == null;

	public MemberDescriptor Member { get; }

	public object Owner { get; }

	public MemberRelationship(object owner, MemberDescriptor member)
	{
		if (owner == null)
		{
			throw new ArgumentNullException("owner");
		}
		if (member == null)
		{
			throw new ArgumentNullException("member");
		}
		Owner = owner;
		Member = member;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is MemberRelationship memberRelationship))
		{
			return false;
		}
		if (memberRelationship.Owner == Owner)
		{
			return memberRelationship.Member == Member;
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (Owner == null)
		{
			return base.GetHashCode();
		}
		return Owner.GetHashCode() ^ Member.GetHashCode();
	}

	public static bool operator ==(MemberRelationship left, MemberRelationship right)
	{
		if (left.Owner == right.Owner)
		{
			return left.Member == right.Member;
		}
		return false;
	}

	public static bool operator !=(MemberRelationship left, MemberRelationship right)
	{
		return !(left == right);
	}
}
