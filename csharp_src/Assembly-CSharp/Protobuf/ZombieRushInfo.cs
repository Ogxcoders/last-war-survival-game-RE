using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ZombieRushInfo : IMessage<ZombieRushInfo>, IMessage, IEquatable<ZombieRushInfo>, IDeepCloneable<ZombieRushInfo>
{
	private static readonly MessageParser<ZombieRushInfo> _parser = new MessageParser<ZombieRushInfo>(() => new ZombieRushInfo());

	private UnknownFieldSet _unknownFields;

	public const int ZombieRushFieldNumber = 1;

	private int zombieRush_;

	public const int RoundFieldNumber = 2;

	private int round_;

	public const int StateFieldNumber = 3;

	private int state_;

	public const int StateEndTimeFieldNumber = 4;

	private long stateEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ZombieRushInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[37];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ZombieRush
	{
		get
		{
			return zombieRush_;
		}
		set
		{
			zombieRush_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Round
	{
		get
		{
			return round_;
		}
		set
		{
			round_ = value;
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
	public long StateEndTime
	{
		get
		{
			return stateEndTime_;
		}
		set
		{
			stateEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ZombieRushInfo()
	{
	}

	[DebuggerNonUserCode]
	public ZombieRushInfo(ZombieRushInfo other)
		: this()
	{
		zombieRush_ = other.zombieRush_;
		round_ = other.round_;
		state_ = other.state_;
		stateEndTime_ = other.stateEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ZombieRushInfo Clone()
	{
		return new ZombieRushInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ZombieRushInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ZombieRushInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ZombieRush != other.ZombieRush)
		{
			return false;
		}
		if (Round != other.Round)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (StateEndTime != other.StateEndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ZombieRush != 0)
		{
			num ^= ZombieRush.GetHashCode();
		}
		if (Round != 0)
		{
			num ^= Round.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (StateEndTime != 0L)
		{
			num ^= StateEndTime.GetHashCode();
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
		if (ZombieRush != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ZombieRush);
		}
		if (Round != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Round);
		}
		if (State != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(State);
		}
		if (StateEndTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(StateEndTime);
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
		if (ZombieRush != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZombieRush);
		}
		if (Round != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Round);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (StateEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StateEndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ZombieRushInfo other)
	{
		if (other != null)
		{
			if (other.ZombieRush != 0)
			{
				ZombieRush = other.ZombieRush;
			}
			if (other.Round != 0)
			{
				Round = other.Round;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.StateEndTime != 0L)
			{
				StateEndTime = other.StateEndTime;
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
				ZombieRush = input.ReadInt32();
				break;
			case 16u:
				Round = input.ReadInt32();
				break;
			case 24u:
				State = input.ReadInt32();
				break;
			case 32u:
				StateEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
