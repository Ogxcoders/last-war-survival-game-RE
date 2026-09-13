using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WhistleInfo : IMessage<WhistleInfo>, IMessage, IEquatable<WhistleInfo>, IDeepCloneable<WhistleInfo>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public enum State
		{
			[OriginalName("NORMAL")]
			Normal,
			[OriginalName("WHISTLED")]
			Whistled,
			[OriginalName("FOLLOWING")]
			Following,
			[OriginalName("ATTACKING")]
			Attacking
		}
	}

	private static readonly MessageParser<WhistleInfo> _parser = new MessageParser<WhistleInfo>(() => new WhistleInfo());

	private UnknownFieldSet _unknownFields;

	public const int AttractMarchUuidFieldNumber = 1;

	private long attractMarchUuid_;

	public const int FollowingUserFieldNumber = 2;

	private string followingUser_ = "";

	public const int StateFieldNumber = 3;

	private int state_;

	public const int StateChangeTimeFieldNumber = 4;

	private long stateChangeTime_;

	public const int AttractMarchTargetPosFieldNumber = 5;

	private int attractMarchTargetPos_;

	public const int AttractIndexFieldNumber = 6;

	private int attractIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<WhistleInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[41];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long AttractMarchUuid
	{
		get
		{
			return attractMarchUuid_;
		}
		set
		{
			attractMarchUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string FollowingUser
	{
		get
		{
			return followingUser_;
		}
		set
		{
			followingUser_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StateChangeTime
	{
		get
		{
			return stateChangeTime_;
		}
		set
		{
			stateChangeTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AttractMarchTargetPos
	{
		get
		{
			return attractMarchTargetPos_;
		}
		set
		{
			attractMarchTargetPos_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AttractIndex
	{
		get
		{
			return attractIndex_;
		}
		set
		{
			attractIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WhistleInfo()
	{
	}

	[DebuggerNonUserCode]
	public WhistleInfo(WhistleInfo other)
		: this()
	{
		attractMarchUuid_ = other.attractMarchUuid_;
		followingUser_ = other.followingUser_;
		state_ = other.state_;
		stateChangeTime_ = other.stateChangeTime_;
		attractMarchTargetPos_ = other.attractMarchTargetPos_;
		attractIndex_ = other.attractIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WhistleInfo Clone()
	{
		return new WhistleInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WhistleInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WhistleInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AttractMarchUuid != other.AttractMarchUuid)
		{
			return false;
		}
		if (FollowingUser != other.FollowingUser)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (StateChangeTime != other.StateChangeTime)
		{
			return false;
		}
		if (AttractMarchTargetPos != other.AttractMarchTargetPos)
		{
			return false;
		}
		if (AttractIndex != other.AttractIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AttractMarchUuid != 0L)
		{
			num ^= AttractMarchUuid.GetHashCode();
		}
		if (FollowingUser.Length != 0)
		{
			num ^= FollowingUser.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (StateChangeTime != 0L)
		{
			num ^= StateChangeTime.GetHashCode();
		}
		if (AttractMarchTargetPos != 0)
		{
			num ^= AttractMarchTargetPos.GetHashCode();
		}
		if (AttractIndex != 0)
		{
			num ^= AttractIndex.GetHashCode();
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
		if (AttractMarchUuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(AttractMarchUuid);
		}
		if (FollowingUser.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(FollowingUser);
		}
		if (State != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(State);
		}
		if (StateChangeTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(StateChangeTime);
		}
		if (AttractMarchTargetPos != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(AttractMarchTargetPos);
		}
		if (AttractIndex != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(AttractIndex);
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
		if (AttractMarchUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(AttractMarchUuid);
		}
		if (FollowingUser.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FollowingUser);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (StateChangeTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StateChangeTime);
		}
		if (AttractMarchTargetPos != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AttractMarchTargetPos);
		}
		if (AttractIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AttractIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WhistleInfo other)
	{
		if (other != null)
		{
			if (other.AttractMarchUuid != 0L)
			{
				AttractMarchUuid = other.AttractMarchUuid;
			}
			if (other.FollowingUser.Length != 0)
			{
				FollowingUser = other.FollowingUser;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.StateChangeTime != 0L)
			{
				StateChangeTime = other.StateChangeTime;
			}
			if (other.AttractMarchTargetPos != 0)
			{
				AttractMarchTargetPos = other.AttractMarchTargetPos;
			}
			if (other.AttractIndex != 0)
			{
				AttractIndex = other.AttractIndex;
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
			case 8u:
				AttractMarchUuid = input.ReadInt64();
				break;
			case 18u:
				FollowingUser = input.ReadString();
				break;
			case 24u:
				State = input.ReadInt32();
				break;
			case 32u:
				StateChangeTime = input.ReadInt64();
				break;
			case 40u:
				AttractMarchTargetPos = input.ReadInt32();
				break;
			case 48u:
				AttractIndex = input.ReadInt32();
				break;
			}
		}
	}
}
