using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FightAction : IMessage<FightAction>, IMessage, IEquatable<FightAction>, IDeepCloneable<FightAction>
{
	private static readonly MessageParser<FightAction> _parser = new MessageParser<FightAction>(() => new FightAction());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int OrderFieldNumber = 2;

	private int order_;

	public const int TimeFieldNumber = 3;

	private int time_;

	public const int CasterIndexFieldNumber = 4;

	private int casterIndex_;

	public const int PhaseFieldNumber = 5;

	private int phase_;

	public const int SkillIdFieldNumber = 6;

	private int skillId_;

	public const int TargetsFieldNumber = 7;

	private static readonly FieldCodec<TargetHit> _repeated_targets_codec = FieldCodec.ForMessage(58u, TargetHit.Parser);

	private readonly RepeatedField<TargetHit> targets_ = new RepeatedField<TargetHit>();

	public const int SkillBuffIdFieldNumber = 8;

	private int skillBuffId_;

	public const int SkillLevelFieldNumber = 9;

	private int skillLevel_;

	public const int DebugInfoFieldNumber = 10;

	private string debugInfo_ = "";

	public const int BulletIndexFieldNumber = 11;

	private int bulletIndex_;

	public const int ActionParamTypeFieldNumber = 12;

	private int actionParamType_;

	public const int SummonUnitFieldNumber = 13;

	private static readonly FieldCodec<LwBattleUnit> _repeated_summonUnit_codec = FieldCodec.ForMessage(106u, LwBattleUnit.Parser);

	private readonly RepeatedField<LwBattleUnit> summonUnit_ = new RepeatedField<LwBattleUnit>();

	public const int CasterStateFieldNumber = 14;

	private int casterState_;

	public const int EffectIdFieldNumber = 15;

	private int effectId_;

	[DebuggerNonUserCode]
	public static MessageParser<FightAction> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Order
	{
		get
		{
			return order_;
		}
		set
		{
			order_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Time
	{
		get
		{
			return time_;
		}
		set
		{
			time_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CasterIndex
	{
		get
		{
			return casterIndex_;
		}
		set
		{
			casterIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Phase
	{
		get
		{
			return phase_;
		}
		set
		{
			phase_ = value;
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
	public RepeatedField<TargetHit> Targets => targets_;

	[DebuggerNonUserCode]
	public int SkillBuffId
	{
		get
		{
			return skillBuffId_;
		}
		set
		{
			skillBuffId_ = value;
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
	public string DebugInfo
	{
		get
		{
			return debugInfo_;
		}
		set
		{
			debugInfo_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int BulletIndex
	{
		get
		{
			return bulletIndex_;
		}
		set
		{
			bulletIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ActionParamType
	{
		get
		{
			return actionParamType_;
		}
		set
		{
			actionParamType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwBattleUnit> SummonUnit => summonUnit_;

	[DebuggerNonUserCode]
	public int CasterState
	{
		get
		{
			return casterState_;
		}
		set
		{
			casterState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectId
	{
		get
		{
			return effectId_;
		}
		set
		{
			effectId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public FightAction()
	{
	}

	[DebuggerNonUserCode]
	public FightAction(FightAction other)
		: this()
	{
		id_ = other.id_;
		order_ = other.order_;
		time_ = other.time_;
		casterIndex_ = other.casterIndex_;
		phase_ = other.phase_;
		skillId_ = other.skillId_;
		targets_ = other.targets_.Clone();
		skillBuffId_ = other.skillBuffId_;
		skillLevel_ = other.skillLevel_;
		debugInfo_ = other.debugInfo_;
		bulletIndex_ = other.bulletIndex_;
		actionParamType_ = other.actionParamType_;
		summonUnit_ = other.summonUnit_.Clone();
		casterState_ = other.casterState_;
		effectId_ = other.effectId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FightAction Clone()
	{
		return new FightAction(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FightAction);
	}

	[DebuggerNonUserCode]
	public bool Equals(FightAction other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (Order != other.Order)
		{
			return false;
		}
		if (Time != other.Time)
		{
			return false;
		}
		if (CasterIndex != other.CasterIndex)
		{
			return false;
		}
		if (Phase != other.Phase)
		{
			return false;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		if (!targets_.Equals(other.targets_))
		{
			return false;
		}
		if (SkillBuffId != other.SkillBuffId)
		{
			return false;
		}
		if (SkillLevel != other.SkillLevel)
		{
			return false;
		}
		if (DebugInfo != other.DebugInfo)
		{
			return false;
		}
		if (BulletIndex != other.BulletIndex)
		{
			return false;
		}
		if (ActionParamType != other.ActionParamType)
		{
			return false;
		}
		if (!summonUnit_.Equals(other.summonUnit_))
		{
			return false;
		}
		if (CasterState != other.CasterState)
		{
			return false;
		}
		if (EffectId != other.EffectId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (Order != 0)
		{
			num ^= Order.GetHashCode();
		}
		if (Time != 0)
		{
			num ^= Time.GetHashCode();
		}
		if (CasterIndex != 0)
		{
			num ^= CasterIndex.GetHashCode();
		}
		if (Phase != 0)
		{
			num ^= Phase.GetHashCode();
		}
		if (SkillId != 0)
		{
			num ^= SkillId.GetHashCode();
		}
		num ^= targets_.GetHashCode();
		if (SkillBuffId != 0)
		{
			num ^= SkillBuffId.GetHashCode();
		}
		if (SkillLevel != 0)
		{
			num ^= SkillLevel.GetHashCode();
		}
		if (DebugInfo.Length != 0)
		{
			num ^= DebugInfo.GetHashCode();
		}
		if (BulletIndex != 0)
		{
			num ^= BulletIndex.GetHashCode();
		}
		if (ActionParamType != 0)
		{
			num ^= ActionParamType.GetHashCode();
		}
		num ^= summonUnit_.GetHashCode();
		if (CasterState != 0)
		{
			num ^= CasterState.GetHashCode();
		}
		if (EffectId != 0)
		{
			num ^= EffectId.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
		}
		if (Order != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Order);
		}
		if (Time != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Time);
		}
		if (CasterIndex != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CasterIndex);
		}
		if (Phase != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Phase);
		}
		if (SkillId != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(SkillId);
		}
		targets_.WriteTo(output, _repeated_targets_codec);
		if (SkillBuffId != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(SkillBuffId);
		}
		if (SkillLevel != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(SkillLevel);
		}
		if (DebugInfo.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(DebugInfo);
		}
		if (BulletIndex != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(BulletIndex);
		}
		if (ActionParamType != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(ActionParamType);
		}
		summonUnit_.WriteTo(output, _repeated_summonUnit_codec);
		if (CasterState != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(CasterState);
		}
		if (EffectId != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(EffectId);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (Order != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Order);
		}
		if (Time != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Time);
		}
		if (CasterIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CasterIndex);
		}
		if (Phase != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Phase);
		}
		if (SkillId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillId);
		}
		num += targets_.CalculateSize(_repeated_targets_codec);
		if (SkillBuffId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillBuffId);
		}
		if (SkillLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillLevel);
		}
		if (DebugInfo.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DebugInfo);
		}
		if (BulletIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BulletIndex);
		}
		if (ActionParamType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ActionParamType);
		}
		num += summonUnit_.CalculateSize(_repeated_summonUnit_codec);
		if (CasterState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CasterState);
		}
		if (EffectId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FightAction other)
	{
		if (other != null)
		{
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.Order != 0)
			{
				Order = other.Order;
			}
			if (other.Time != 0)
			{
				Time = other.Time;
			}
			if (other.CasterIndex != 0)
			{
				CasterIndex = other.CasterIndex;
			}
			if (other.Phase != 0)
			{
				Phase = other.Phase;
			}
			if (other.SkillId != 0)
			{
				SkillId = other.SkillId;
			}
			targets_.Add(other.targets_);
			if (other.SkillBuffId != 0)
			{
				SkillBuffId = other.SkillBuffId;
			}
			if (other.SkillLevel != 0)
			{
				SkillLevel = other.SkillLevel;
			}
			if (other.DebugInfo.Length != 0)
			{
				DebugInfo = other.DebugInfo;
			}
			if (other.BulletIndex != 0)
			{
				BulletIndex = other.BulletIndex;
			}
			if (other.ActionParamType != 0)
			{
				ActionParamType = other.ActionParamType;
			}
			summonUnit_.Add(other.summonUnit_);
			if (other.CasterState != 0)
			{
				CasterState = other.CasterState;
			}
			if (other.EffectId != 0)
			{
				EffectId = other.EffectId;
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
				Id = input.ReadInt32();
				break;
			case 16u:
				Order = input.ReadInt32();
				break;
			case 24u:
				Time = input.ReadInt32();
				break;
			case 32u:
				CasterIndex = input.ReadInt32();
				break;
			case 40u:
				Phase = input.ReadInt32();
				break;
			case 48u:
				SkillId = input.ReadInt32();
				break;
			case 58u:
				targets_.AddEntriesFrom(input, _repeated_targets_codec);
				break;
			case 64u:
				SkillBuffId = input.ReadInt32();
				break;
			case 72u:
				SkillLevel = input.ReadInt32();
				break;
			case 82u:
				DebugInfo = input.ReadString();
				break;
			case 88u:
				BulletIndex = input.ReadInt32();
				break;
			case 96u:
				ActionParamType = input.ReadInt32();
				break;
			case 106u:
				summonUnit_.AddEntriesFrom(input, _repeated_summonUnit_codec);
				break;
			case 112u:
				CasterState = input.ReadInt32();
				break;
			case 120u:
				EffectId = input.ReadInt32();
				break;
			}
		}
	}
}
