using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class GhostReconMemberData : IMessage<GhostReconMemberData>, IMessage, IEquatable<GhostReconMemberData>, IDeepCloneable<GhostReconMemberData>
{
	private static readonly MessageParser<GhostReconMemberData> _parser = new MessageParser<GhostReconMemberData>(() => new GhostReconMemberData());

	private UnknownFieldSet _unknownFields;

	public const int MemberUidFieldNumber = 1;

	private string memberUid_ = "";

	public const int RewardedFieldNumber = 2;

	private int rewarded_;

	public const int CanRewardFieldNumber = 3;

	private int canReward_;

	[DebuggerNonUserCode]
	public static MessageParser<GhostReconMemberData> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[49];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string MemberUid
	{
		get
		{
			return memberUid_;
		}
		set
		{
			memberUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Rewarded
	{
		get
		{
			return rewarded_;
		}
		set
		{
			rewarded_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CanReward
	{
		get
		{
			return canReward_;
		}
		set
		{
			canReward_ = value;
		}
	}

	[DebuggerNonUserCode]
	public GhostReconMemberData()
	{
	}

	[DebuggerNonUserCode]
	public GhostReconMemberData(GhostReconMemberData other)
		: this()
	{
		memberUid_ = other.memberUid_;
		rewarded_ = other.rewarded_;
		canReward_ = other.canReward_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GhostReconMemberData Clone()
	{
		return new GhostReconMemberData(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GhostReconMemberData);
	}

	[DebuggerNonUserCode]
	public bool Equals(GhostReconMemberData other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MemberUid != other.MemberUid)
		{
			return false;
		}
		if (Rewarded != other.Rewarded)
		{
			return false;
		}
		if (CanReward != other.CanReward)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MemberUid.Length != 0)
		{
			num ^= MemberUid.GetHashCode();
		}
		if (Rewarded != 0)
		{
			num ^= Rewarded.GetHashCode();
		}
		if (CanReward != 0)
		{
			num ^= CanReward.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (MemberUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MemberUid);
		}
		if (Rewarded != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Rewarded);
		}
		if (CanReward != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CanReward);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (MemberUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MemberUid);
		}
		if (Rewarded != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rewarded);
		}
		if (CanReward != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CanReward);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GhostReconMemberData other)
	{
		if (other != null)
		{
			if (other.MemberUid.Length != 0)
			{
				MemberUid = other.MemberUid;
			}
			if (other.Rewarded != 0)
			{
				Rewarded = other.Rewarded;
			}
			if (other.CanReward != 0)
			{
				CanReward = other.CanReward;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 10u:
				MemberUid = input.ReadString();
				break;
			case 16u:
				Rewarded = input.ReadInt32();
				break;
			case 24u:
				CanReward = input.ReadInt32();
				break;
			}
		}
	}
}
