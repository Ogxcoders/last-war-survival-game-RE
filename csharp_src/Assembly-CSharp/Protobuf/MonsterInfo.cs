using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MonsterInfo : IMessage<MonsterInfo>, IMessage, IEquatable<MonsterInfo>, IDeepCloneable<MonsterInfo>
{
	public enum ExtraOneofCase
	{
		None = 0,
		DarknessMonster = 101
	}

	private static readonly MessageParser<MonsterInfo> _parser = new MessageParser<MonsterInfo>(() => new MonsterInfo());

	private UnknownFieldSet _unknownFields;

	public const int MonsterIdFieldNumber = 1;

	private int monsterId_;

	public const int ExpireTimeFieldNumber = 2;

	private long expireTime_;

	public const int CurHpFieldNumber = 3;

	private long curHp_;

	public const int MaxHpFieldNumber = 4;

	private long maxHp_;

	public const int StateFieldNumber = 5;

	private int state_;

	public const int StateEndTimeFieldNumber = 6;

	private long stateEndTime_;

	public const int StateTriggerInfoFieldNumber = 7;

	private string stateTriggerInfo_ = "";

	public const int TypeFieldNumber = 8;

	private int type_;

	public const int SpecialTypeFieldNumber = 9;

	private int specialType_;

	public const int DarknessMonsterFieldNumber = 101;

	private object extra_;

	private ExtraOneofCase extraCase_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[39];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public long ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
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
	public string StateTriggerInfo
	{
		get
		{
			return stateTriggerInfo_;
		}
		set
		{
			stateTriggerInfo_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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
	public int SpecialType
	{
		get
		{
			return specialType_;
		}
		set
		{
			specialType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DarknessMonster DarknessMonster
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.DarknessMonster)
			{
				return null;
			}
			return (DarknessMonster)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.DarknessMonster : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public ExtraOneofCase ExtraCase => extraCase_;

	[DebuggerNonUserCode]
	public MonsterInfo()
	{
	}

	[DebuggerNonUserCode]
	public MonsterInfo(MonsterInfo other)
		: this()
	{
		monsterId_ = other.monsterId_;
		expireTime_ = other.expireTime_;
		curHp_ = other.curHp_;
		maxHp_ = other.maxHp_;
		state_ = other.state_;
		stateEndTime_ = other.stateEndTime_;
		stateTriggerInfo_ = other.stateTriggerInfo_;
		type_ = other.type_;
		specialType_ = other.specialType_;
		ExtraOneofCase extraCase = other.ExtraCase;
		if (extraCase == ExtraOneofCase.DarknessMonster)
		{
			DarknessMonster = other.DarknessMonster.Clone();
		}
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterInfo Clone()
	{
		return new MonsterInfo(this);
	}

	[DebuggerNonUserCode]
	public void ClearExtra()
	{
		extraCase_ = ExtraOneofCase.None;
		extra_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MonsterId != other.MonsterId)
		{
			return false;
		}
		if (ExpireTime != other.ExpireTime)
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
		if (State != other.State)
		{
			return false;
		}
		if (StateEndTime != other.StateEndTime)
		{
			return false;
		}
		if (StateTriggerInfo != other.StateTriggerInfo)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (SpecialType != other.SpecialType)
		{
			return false;
		}
		if (!object.Equals(DarknessMonster, other.DarknessMonster))
		{
			return false;
		}
		if (ExtraCase != other.ExtraCase)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MonsterId != 0)
		{
			num ^= MonsterId.GetHashCode();
		}
		if (ExpireTime != 0L)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (CurHp != 0L)
		{
			num ^= CurHp.GetHashCode();
		}
		if (MaxHp != 0L)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (StateEndTime != 0L)
		{
			num ^= StateEndTime.GetHashCode();
		}
		if (StateTriggerInfo.Length != 0)
		{
			num ^= StateTriggerInfo.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (SpecialType != 0)
		{
			num ^= SpecialType.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.DarknessMonster)
		{
			num ^= DarknessMonster.GetHashCode();
		}
		num ^= (int)extraCase_;
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
		if (MonsterId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(MonsterId);
		}
		if (ExpireTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(ExpireTime);
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
		if (State != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(State);
		}
		if (StateEndTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(StateEndTime);
		}
		if (StateTriggerInfo.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(StateTriggerInfo);
		}
		if (Type != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Type);
		}
		if (SpecialType != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(SpecialType);
		}
		if (extraCase_ == ExtraOneofCase.DarknessMonster)
		{
			output.WriteRawTag(170, 6);
			output.WriteMessage(DarknessMonster);
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
		if (MonsterId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterId);
		}
		if (ExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExpireTime);
		}
		if (CurHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurHp);
		}
		if (MaxHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxHp);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (StateEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StateEndTime);
		}
		if (StateTriggerInfo.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(StateTriggerInfo);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (SpecialType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SpecialType);
		}
		if (extraCase_ == ExtraOneofCase.DarknessMonster)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(DarknessMonster);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.MonsterId != 0)
		{
			MonsterId = other.MonsterId;
		}
		if (other.ExpireTime != 0L)
		{
			ExpireTime = other.ExpireTime;
		}
		if (other.CurHp != 0L)
		{
			CurHp = other.CurHp;
		}
		if (other.MaxHp != 0L)
		{
			MaxHp = other.MaxHp;
		}
		if (other.State != 0)
		{
			State = other.State;
		}
		if (other.StateEndTime != 0L)
		{
			StateEndTime = other.StateEndTime;
		}
		if (other.StateTriggerInfo.Length != 0)
		{
			StateTriggerInfo = other.StateTriggerInfo;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.SpecialType != 0)
		{
			SpecialType = other.SpecialType;
		}
		ExtraOneofCase extraCase = other.ExtraCase;
		if (extraCase == ExtraOneofCase.DarknessMonster)
		{
			if (DarknessMonster == null)
			{
				DarknessMonster = new DarknessMonster();
			}
			DarknessMonster.MergeFrom(other.DarknessMonster);
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				MonsterId = input.ReadInt32();
				break;
			case 16u:
				ExpireTime = input.ReadInt64();
				break;
			case 24u:
				CurHp = input.ReadInt64();
				break;
			case 32u:
				MaxHp = input.ReadInt64();
				break;
			case 40u:
				State = input.ReadInt32();
				break;
			case 48u:
				StateEndTime = input.ReadInt64();
				break;
			case 58u:
				StateTriggerInfo = input.ReadString();
				break;
			case 64u:
				Type = input.ReadInt32();
				break;
			case 72u:
				SpecialType = input.ReadInt32();
				break;
			case 810u:
			{
				DarknessMonster darknessMonster = new DarknessMonster();
				if (extraCase_ == ExtraOneofCase.DarknessMonster)
				{
					darknessMonster.MergeFrom(DarknessMonster);
				}
				input.ReadMessage(darknessMonster);
				DarknessMonster = darknessMonster;
				break;
			}
			}
		}
	}
}
