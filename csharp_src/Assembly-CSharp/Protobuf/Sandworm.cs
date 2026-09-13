using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Sandworm : IMessage<Sandworm>, IMessage, IEquatable<Sandworm>, IDeepCloneable<Sandworm>
{
	private static readonly MessageParser<Sandworm> _parser = new MessageParser<Sandworm>(() => new Sandworm());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	public const int StateFieldNumber = 2;

	private int state_;

	public const int CurHpFieldNumber = 3;

	private long curHp_;

	public const int MaxHpFieldNumber = 4;

	private long maxHp_;

	public const int MonsterIdFieldNumber = 5;

	private int monsterId_;

	public const int StateEndTimeFieldNumber = 6;

	private long stateEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<Sandworm> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[57];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
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
	public long CurHp
	{
		get
		{
			return curHp_;
		}
		set
		{
			curHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxHp
	{
		get
		{
			return maxHp_;
		}
		set
		{
			maxHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MonsterId
	{
		get
		{
			return monsterId_;
		}
		set
		{
			monsterId_ = value;
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
	public Sandworm()
	{
	}

	[DebuggerNonUserCode]
	public Sandworm(Sandworm other)
		: this()
	{
		type_ = other.type_;
		state_ = other.state_;
		curHp_ = other.curHp_;
		maxHp_ = other.maxHp_;
		monsterId_ = other.monsterId_;
		stateEndTime_ = other.stateEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Sandworm Clone()
	{
		return new Sandworm(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Sandworm);
	}

	[DebuggerNonUserCode]
	public bool Equals(Sandworm other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (CurHp != other.CurHp)
		{
			return false;
		}
		if (MaxHp != other.MaxHp)
		{
			return false;
		}
		if (MonsterId != other.MonsterId)
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
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (CurHp != 0L)
		{
			num ^= CurHp.GetHashCode();
		}
		if (MaxHp != 0L)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (MonsterId != 0)
		{
			num ^= MonsterId.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Type);
		}
		if (State != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(State);
		}
		if (CurHp != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurHp);
		}
		if (MaxHp != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MaxHp);
		}
		if (MonsterId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(MonsterId);
		}
		if (StateEndTime != 0L)
		{
			output.WriteRawTag(48);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (CurHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurHp);
		}
		if (MaxHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxHp);
		}
		if (MonsterId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterId);
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
	public void MergeFrom(Sandworm other)
	{
		if (other != null)
		{
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.CurHp != 0L)
			{
				CurHp = other.CurHp;
			}
			if (other.MaxHp != 0L)
			{
				MaxHp = other.MaxHp;
			}
			if (other.MonsterId != 0)
			{
				MonsterId = other.MonsterId;
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
				Type = input.ReadInt32();
				break;
			case 16u:
				State = input.ReadInt32();
				break;
			case 24u:
				CurHp = input.ReadInt64();
				break;
			case 32u:
				MaxHp = input.ReadInt64();
				break;
			case 40u:
				MonsterId = input.ReadInt32();
				break;
			case 48u:
				StateEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
