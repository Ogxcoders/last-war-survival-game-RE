using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BaseRoundReport : IMessage<BaseRoundReport>, IMessage, IEquatable<BaseRoundReport>, IDeepCloneable<BaseRoundReport>
{
	private static readonly MessageParser<BaseRoundReport> _parser = new MessageParser<BaseRoundReport>(() => new BaseRoundReport());

	private UnknownFieldSet _unknownFields;

	public const int TriggerIndexFieldNumber = 1;

	private int triggerIndex_;

	public const int RoundFieldNumber = 2;

	private int round_;

	public const int ValueFieldNumber = 3;

	private int value_;

	public const int TypeFieldNumber = 4;

	private int type_;

	public const int SkillIdFieldNumber = 5;

	private int skillId_;

	public const int SkillLevelFieldNumber = 6;

	private int skillLevel_;

	public const int HeroIdFieldNumber = 7;

	private int heroId_;

	public const int TargetIndexFieldNumber = 8;

	private int targetIndex_;

	public const int HealthFieldNumber = 9;

	private int health_;

	public const int ParamFieldNumber = 10;

	private int param_;

	[DebuggerNonUserCode]
	public static MessageParser<BaseRoundReport> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int TriggerIndex
	{
		get
		{
			return triggerIndex_;
		}
		set
		{
			triggerIndex_ = value;
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
	public int Value
	{
		get
		{
			return value_;
		}
		set
		{
			value_ = value;
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
	public int SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SkillLevel
	{
		get
		{
			return skillLevel_;
		}
		set
		{
			skillLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeroId
	{
		get
		{
			return heroId_;
		}
		set
		{
			heroId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TargetIndex
	{
		get
		{
			return targetIndex_;
		}
		set
		{
			targetIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Health
	{
		get
		{
			return health_;
		}
		set
		{
			health_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Param
	{
		get
		{
			return param_;
		}
		set
		{
			param_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BaseRoundReport()
	{
	}

	[DebuggerNonUserCode]
	public BaseRoundReport(BaseRoundReport other)
		: this()
	{
		triggerIndex_ = other.triggerIndex_;
		round_ = other.round_;
		value_ = other.value_;
		type_ = other.type_;
		skillId_ = other.skillId_;
		skillLevel_ = other.skillLevel_;
		heroId_ = other.heroId_;
		targetIndex_ = other.targetIndex_;
		health_ = other.health_;
		param_ = other.param_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BaseRoundReport Clone()
	{
		return new BaseRoundReport(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BaseRoundReport);
	}

	[DebuggerNonUserCode]
	public bool Equals(BaseRoundReport other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TriggerIndex != other.TriggerIndex)
		{
			return false;
		}
		if (Round != other.Round)
		{
			return false;
		}
		if (Value != other.Value)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		if (SkillLevel != other.SkillLevel)
		{
			return false;
		}
		if (HeroId != other.HeroId)
		{
			return false;
		}
		if (TargetIndex != other.TargetIndex)
		{
			return false;
		}
		if (Health != other.Health)
		{
			return false;
		}
		if (Param != other.Param)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TriggerIndex != 0)
		{
			num ^= TriggerIndex.GetHashCode();
		}
		if (Round != 0)
		{
			num ^= Round.GetHashCode();
		}
		if (Value != 0)
		{
			num ^= Value.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (SkillId != 0)
		{
			num ^= SkillId.GetHashCode();
		}
		if (SkillLevel != 0)
		{
			num ^= SkillLevel.GetHashCode();
		}
		if (HeroId != 0)
		{
			num ^= HeroId.GetHashCode();
		}
		if (TargetIndex != 0)
		{
			num ^= TargetIndex.GetHashCode();
		}
		if (Health != 0)
		{
			num ^= Health.GetHashCode();
		}
		if (Param != 0)
		{
			num ^= Param.GetHashCode();
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
		if (TriggerIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TriggerIndex);
		}
		if (Round != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Round);
		}
		if (Value != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Value);
		}
		if (Type != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Type);
		}
		if (SkillId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(SkillId);
		}
		if (SkillLevel != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(SkillLevel);
		}
		if (HeroId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(HeroId);
		}
		if (TargetIndex != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(TargetIndex);
		}
		if (Health != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Health);
		}
		if (Param != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(Param);
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
		if (TriggerIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TriggerIndex);
		}
		if (Round != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Round);
		}
		if (Value != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Value);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (SkillId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillId);
		}
		if (SkillLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillLevel);
		}
		if (HeroId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroId);
		}
		if (TargetIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TargetIndex);
		}
		if (Health != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Health);
		}
		if (Param != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Param);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BaseRoundReport other)
	{
		if (other != null)
		{
			if (other.TriggerIndex != 0)
			{
				TriggerIndex = other.TriggerIndex;
			}
			if (other.Round != 0)
			{
				Round = other.Round;
			}
			if (other.Value != 0)
			{
				Value = other.Value;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.SkillId != 0)
			{
				SkillId = other.SkillId;
			}
			if (other.SkillLevel != 0)
			{
				SkillLevel = other.SkillLevel;
			}
			if (other.HeroId != 0)
			{
				HeroId = other.HeroId;
			}
			if (other.TargetIndex != 0)
			{
				TargetIndex = other.TargetIndex;
			}
			if (other.Health != 0)
			{
				Health = other.Health;
			}
			if (other.Param != 0)
			{
				Param = other.Param;
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
				TriggerIndex = input.ReadInt32();
				break;
			case 16u:
				Round = input.ReadInt32();
				break;
			case 24u:
				Value = input.ReadInt32();
				break;
			case 32u:
				Type = input.ReadInt32();
				break;
			case 40u:
				SkillId = input.ReadInt32();
				break;
			case 48u:
				SkillLevel = input.ReadInt32();
				break;
			case 56u:
				HeroId = input.ReadInt32();
				break;
			case 64u:
				TargetIndex = input.ReadInt32();
				break;
			case 72u:
				Health = input.ReadInt32();
				break;
			case 80u:
				Param = input.ReadInt32();
				break;
			}
		}
	}
}
