using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FightReport : IMessage<FightReport>, IMessage, IEquatable<FightReport>, IDeepCloneable<FightReport>
{
	private static readonly MessageParser<FightReport> _parser = new MessageParser<FightReport>(() => new FightReport());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int SelfInfoFieldNumber = 2;

	private ReportPlayerInfo selfInfo_;

	public const int OtherInfoFieldNumber = 3;

	private ReportPlayerInfo otherInfo_;

	public const int SelfArmyResultFieldNumber = 4;

	private ArmyResult selfArmyResult_;

	public const int OtherArmyResultFieldNumber = 5;

	private ArmyResult otherArmyResult_;

	public const int RewardFieldNumber = 6;

	private ReportReward reward_;

	public const int FightResultFieldNumber = 7;

	private int fightResult_;

	public const int OtherBattleEffectGroupsFieldNumber = 8;

	private static readonly FieldCodec<BattleEffectGroup> _repeated_otherBattleEffectGroups_codec = FieldCodec.ForMessage(66u, BattleEffectGroup.Parser);

	private readonly RepeatedField<BattleEffectGroup> otherBattleEffectGroups_ = new RepeatedField<BattleEffectGroup>();

	public const int UnitAttrInfoFieldNumber = 9;

	private static readonly FieldCodec<UnitAttrInfo> _repeated_unitAttrInfo_codec = FieldCodec.ForMessage(74u, Protobuf.UnitAttrInfo.Parser);

	private readonly RepeatedField<UnitAttrInfo> unitAttrInfo_ = new RepeatedField<UnitAttrInfo>();

	public const int AllRewardsFieldNumber = 10;

	private static readonly FieldCodec<AllReportReward> _repeated_allRewards_codec = FieldCodec.ForMessage(82u, AllReportReward.Parser);

	private readonly RepeatedField<AllReportReward> allRewards_ = new RepeatedField<AllReportReward>();

	[DebuggerNonUserCode]
	public static MessageParser<FightReport> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[22];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportPlayerInfo SelfInfo
	{
		get
		{
			return selfInfo_;
		}
		set
		{
			selfInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportPlayerInfo OtherInfo
	{
		get
		{
			return otherInfo_;
		}
		set
		{
			otherInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyResult SelfArmyResult
	{
		get
		{
			return selfArmyResult_;
		}
		set
		{
			selfArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyResult OtherArmyResult
	{
		get
		{
			return otherArmyResult_;
		}
		set
		{
			otherArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportReward Reward
	{
		get
		{
			return reward_;
		}
		set
		{
			reward_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FightResult
	{
		get
		{
			return fightResult_;
		}
		set
		{
			fightResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectGroup> OtherBattleEffectGroups => otherBattleEffectGroups_;

	[DebuggerNonUserCode]
	public RepeatedField<UnitAttrInfo> UnitAttrInfo => unitAttrInfo_;

	[DebuggerNonUserCode]
	public RepeatedField<AllReportReward> AllRewards => allRewards_;

	[DebuggerNonUserCode]
	public FightReport()
	{
	}

	[DebuggerNonUserCode]
	public FightReport(FightReport other)
		: this()
	{
		uuid_ = other.uuid_;
		selfInfo_ = ((other.selfInfo_ != null) ? other.selfInfo_.Clone() : null);
		otherInfo_ = ((other.otherInfo_ != null) ? other.otherInfo_.Clone() : null);
		selfArmyResult_ = ((other.selfArmyResult_ != null) ? other.selfArmyResult_.Clone() : null);
		otherArmyResult_ = ((other.otherArmyResult_ != null) ? other.otherArmyResult_.Clone() : null);
		reward_ = ((other.reward_ != null) ? other.reward_.Clone() : null);
		fightResult_ = other.fightResult_;
		otherBattleEffectGroups_ = other.otherBattleEffectGroups_.Clone();
		unitAttrInfo_ = other.unitAttrInfo_.Clone();
		allRewards_ = other.allRewards_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FightReport Clone()
	{
		return new FightReport(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FightReport);
	}

	[DebuggerNonUserCode]
	public bool Equals(FightReport other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (!object.Equals(SelfInfo, other.SelfInfo))
		{
			return false;
		}
		if (!object.Equals(OtherInfo, other.OtherInfo))
		{
			return false;
		}
		if (!object.Equals(SelfArmyResult, other.SelfArmyResult))
		{
			return false;
		}
		if (!object.Equals(OtherArmyResult, other.OtherArmyResult))
		{
			return false;
		}
		if (!object.Equals(Reward, other.Reward))
		{
			return false;
		}
		if (FightResult != other.FightResult)
		{
			return false;
		}
		if (!otherBattleEffectGroups_.Equals(other.otherBattleEffectGroups_))
		{
			return false;
		}
		if (!unitAttrInfo_.Equals(other.unitAttrInfo_))
		{
			return false;
		}
		if (!allRewards_.Equals(other.allRewards_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (selfInfo_ != null)
		{
			num ^= SelfInfo.GetHashCode();
		}
		if (otherInfo_ != null)
		{
			num ^= OtherInfo.GetHashCode();
		}
		if (selfArmyResult_ != null)
		{
			num ^= SelfArmyResult.GetHashCode();
		}
		if (otherArmyResult_ != null)
		{
			num ^= OtherArmyResult.GetHashCode();
		}
		if (reward_ != null)
		{
			num ^= Reward.GetHashCode();
		}
		if (FightResult != 0)
		{
			num ^= FightResult.GetHashCode();
		}
		num ^= otherBattleEffectGroups_.GetHashCode();
		num ^= unitAttrInfo_.GetHashCode();
		num ^= allRewards_.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (selfInfo_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(SelfInfo);
		}
		if (otherInfo_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(OtherInfo);
		}
		if (selfArmyResult_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(SelfArmyResult);
		}
		if (otherArmyResult_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(OtherArmyResult);
		}
		if (reward_ != null)
		{
			output.WriteRawTag(50);
			output.WriteMessage(Reward);
		}
		if (FightResult != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(FightResult);
		}
		otherBattleEffectGroups_.WriteTo(output, _repeated_otherBattleEffectGroups_codec);
		unitAttrInfo_.WriteTo(output, _repeated_unitAttrInfo_codec);
		allRewards_.WriteTo(output, _repeated_allRewards_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (selfInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SelfInfo);
		}
		if (otherInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(OtherInfo);
		}
		if (selfArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SelfArmyResult);
		}
		if (otherArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(OtherArmyResult);
		}
		if (reward_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Reward);
		}
		if (FightResult != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FightResult);
		}
		num += otherBattleEffectGroups_.CalculateSize(_repeated_otherBattleEffectGroups_codec);
		num += unitAttrInfo_.CalculateSize(_repeated_unitAttrInfo_codec);
		num += allRewards_.CalculateSize(_repeated_allRewards_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FightReport other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.selfInfo_ != null)
		{
			if (selfInfo_ == null)
			{
				SelfInfo = new ReportPlayerInfo();
			}
			SelfInfo.MergeFrom(other.SelfInfo);
		}
		if (other.otherInfo_ != null)
		{
			if (otherInfo_ == null)
			{
				OtherInfo = new ReportPlayerInfo();
			}
			OtherInfo.MergeFrom(other.OtherInfo);
		}
		if (other.selfArmyResult_ != null)
		{
			if (selfArmyResult_ == null)
			{
				SelfArmyResult = new ArmyResult();
			}
			SelfArmyResult.MergeFrom(other.SelfArmyResult);
		}
		if (other.otherArmyResult_ != null)
		{
			if (otherArmyResult_ == null)
			{
				OtherArmyResult = new ArmyResult();
			}
			OtherArmyResult.MergeFrom(other.OtherArmyResult);
		}
		if (other.reward_ != null)
		{
			if (reward_ == null)
			{
				Reward = new ReportReward();
			}
			Reward.MergeFrom(other.Reward);
		}
		if (other.FightResult != 0)
		{
			FightResult = other.FightResult;
		}
		otherBattleEffectGroups_.Add(other.otherBattleEffectGroups_);
		unitAttrInfo_.Add(other.unitAttrInfo_);
		allRewards_.Add(other.allRewards_);
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
				Uuid = input.ReadInt64();
				break;
			case 18u:
				if (selfInfo_ == null)
				{
					SelfInfo = new ReportPlayerInfo();
				}
				input.ReadMessage(SelfInfo);
				break;
			case 26u:
				if (otherInfo_ == null)
				{
					OtherInfo = new ReportPlayerInfo();
				}
				input.ReadMessage(OtherInfo);
				break;
			case 34u:
				if (selfArmyResult_ == null)
				{
					SelfArmyResult = new ArmyResult();
				}
				input.ReadMessage(SelfArmyResult);
				break;
			case 42u:
				if (otherArmyResult_ == null)
				{
					OtherArmyResult = new ArmyResult();
				}
				input.ReadMessage(OtherArmyResult);
				break;
			case 50u:
				if (reward_ == null)
				{
					Reward = new ReportReward();
				}
				input.ReadMessage(Reward);
				break;
			case 56u:
				FightResult = input.ReadInt32();
				break;
			case 66u:
				otherBattleEffectGroups_.AddEntriesFrom(input, _repeated_otherBattleEffectGroups_codec);
				break;
			case 74u:
				unitAttrInfo_.AddEntriesFrom(input, _repeated_unitAttrInfo_codec);
				break;
			case 82u:
				allRewards_.AddEntriesFrom(input, _repeated_allRewards_codec);
				break;
			}
		}
	}
}
