using System.Collections.Generic;

namespace System.ComponentModel.Design.Serialization;

public abstract class MemberRelationshipService
{
	private struct RelationshipEntry
	{
		internal WeakReference Owner;

		internal MemberDescriptor Member;

		private int _hashCode;

		internal RelationshipEntry(MemberRelationship rel)
		{
			Owner = new WeakReference(rel.Owner);
			Member = rel.Member;
			_hashCode = ((rel.Owner != null) ? rel.Owner.GetHashCode() : 0);
		}

		public override bool Equals(object o)
		{
			if (o is RelationshipEntry relationshipEntry)
			{
				return this == relationshipEntry;
			}
			return false;
		}

		public static bool operator ==(RelationshipEntry re1, RelationshipEntry re2)
		{
			object obj = (re1.Owner.IsAlive ? re1.Owner.Target : null);
			object obj2 = (re2.Owner.IsAlive ? re2.Owner.Target : null);
			if (obj == obj2)
			{
				return re1.Member.Equals(re2.Member);
			}
			return false;
		}

		public static bool operator !=(RelationshipEntry re1, RelationshipEntry re2)
		{
			return !(re1 == re2);
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}
	}

	private Dictionary<RelationshipEntry, RelationshipEntry> _relationships = new Dictionary<RelationshipEntry, RelationshipEntry>();

	public MemberRelationship this[MemberRelationship source]
	{
		get
		{
			if (source.Owner == null)
			{
				throw new ArgumentNullException("Owner");
			}
			if (source.Member == null)
			{
				throw new ArgumentNullException("Member");
			}
			return GetRelationship(source);
		}
		set
		{
			if (source.Owner == null)
			{
				throw new ArgumentNullException("Owner");
			}
			if (source.Member == null)
			{
				throw new ArgumentNullException("Member");
			}
			SetRelationship(source, value);
		}
	}

	public MemberRelationship this[object sourceOwner, MemberDescriptor sourceMember]
	{
		get
		{
			if (sourceOwner == null)
			{
				throw new ArgumentNullException("sourceOwner");
			}
			if (sourceMember == null)
			{
				throw new ArgumentNullException("sourceMember");
			}
			return GetRelationship(new MemberRelationship(sourceOwner, sourceMember));
		}
		set
		{
			if (sourceOwner == null)
			{
				throw new ArgumentNullException("sourceOwner");
			}
			if (sourceMember == null)
			{
				throw new ArgumentNullException("sourceMember");
			}
			SetRelationship(new MemberRelationship(sourceOwner, sourceMember), value);
		}
	}

	protected virtual MemberRelationship GetRelationship(MemberRelationship source)
	{
		if (_relationships != null && _relationships.TryGetValue(new RelationshipEntry(source), out var value) && value.Owner.IsAlive)
		{
			return new MemberRelationship(value.Owner.Target, value.Member);
		}
		return MemberRelationship.Empty;
	}

	protected virtual void SetRelationship(MemberRelationship source, MemberRelationship relationship)
	{
		if (!relationship.IsEmpty && !SupportsRelationship(source, relationship))
		{
			string text = TypeDescriptor.GetComponentName(source.Owner);
			string text2 = TypeDescriptor.GetComponentName(relationship.Owner);
			if (text == null)
			{
				text = source.Owner.ToString();
			}
			if (text2 == null)
			{
				text2 = relationship.Owner.ToString();
			}
			throw new ArgumentException(global::SR.Format("Relationships between {0}.{1} and {2}.{3} are not supported.", text, source.Member.Name, text2, relationship.Member.Name));
		}
		if (_relationships == null)
		{
			_relationships = new Dictionary<RelationshipEntry, RelationshipEntry>();
		}
		_relationships[new RelationshipEntry(source)] = new RelationshipEntry(relationship);
	}

	public abstract bool SupportsRelationship(MemberRelationship source, MemberRelationship relationship);
}
