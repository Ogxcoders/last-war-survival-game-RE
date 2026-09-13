using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattlePlayerStat : IMessage<LwBattlePlayerStat>, IMessage, IEquatable<LwBattlePlayerStat>, IDeepCloneable<LwBattlePlayerStat>
{
	private static readonly MessageParser<LwBattlePlayerStat> _parser = new MessageParser<LwBattlePlayerStat>(() => new LwBattlePlayerStat());

	private UnknownFieldSet _unknownFields;

	public const int UserFieldNumber = 1;

	private ReportPlayerInfo user_;

	public const int ContentIdFieldNumber = 2;

	private int contentId_;

	public const int MaxSoldierPowerFieldNumber = 3;

	private int maxSoldierPower_;

	public const int SoldierPowerFieldNumber = 4;

	private int soldierPower_;

	public const int EffectsFieldNumber = 5;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effects_codec = FieldCodec.ForMessage(42u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effects_ = new RepeatedField<BattleEffectInfo>();

	public const int TotalHeroPowerFieldNumber = 6;

	private int totalHeroPower_;

	public const int SoldierLostFieldNumber = 7;

	private static readonly FieldCodec<LwSoldierLost> _repeated_soldierLost_codec = FieldCodec.ForMessage(58u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> soldierLost_ = new RepeatedField<LwSoldierLost>();

	public const int ArmyTypeFieldNumber = 8;

	private int armyType_;

	public const int SoldierPowerBeforeStartFieldNumber = 9;

	private int soldierPowerBeforeStart_;

	public const int IsHospitalFullFieldNumber = 10;

	private bool isHospitalFull_;

	public const int ProgressFieldNumber = 11;

	private ArmyProgress progress_;

	public const int SoldierBeforeStartFieldNumber = 12;

	private static readonly FieldCodec<LwSoldierLost> _repeated_soldierBeforeStart_codec = FieldCodec.ForMessage(98u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> soldierBeforeStart_ = new RepeatedField<LwSoldierLost>();

	public const int MaxSoldierCountFieldNumber = 13;

	private int maxSoldierCount_;

	public const int SoldierCountBeforeStartFieldNumber = 14;

	private int soldierCountBeforeStart_;

	public const int SoldierCountFieldNumber = 15;

	private int soldierCount_;

	public const int ChipTotalLevelFieldNumber = 16;

	private int chipTotalLevel_;

	public const int DeadRateFieldNumber = 17;

	private float deadRate_;

	public const int ExtraPowersFieldNumber = 18;

	private static readonly FieldCodec<BattleExtraPowerInfo> _repeated_extraPowers_codec = FieldCodec.ForMessage(146u, BattleExtraPowerInfo.Parser);

	private readonly RepeatedField<BattleExtraPowerInfo> extraPowers_ = new RepeatedField<BattleExtraPowerInfo>();

	public const int PowerTabMapFieldNumber = 19;

	private static readonly MapField<int, float>.Codec _map_powerTabMap_codec = new MapField<int, float>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForFloat(21u, 0f), 154u);

	private readonly MapField<int, float> powerTabMap_ = new MapField<int, float>();

	public const int WeaponStrengTotalLvFieldNumber = 20;

	private int weaponStrengTotalLv_;

	public const int BattleCardFieldNumber = 21;

	private BattleCardModule battleCard_;

	public const int WoundedToRemainDetailFieldNumber = 22;

	private static readonly FieldCodec<LwSoldierCount> _repeated_woundedToRemainDetail_codec = FieldCodec.ForMessage(178u, LwSoldierCount.Parser);

	private readonly RepeatedField<LwSoldierCount> woundedToRemainDetail_ = new RepeatedField<LwSoldierCount>();

	public const int SoldierElevenFieldNumber = 23;

	private SoldierElevenProto soldierEleven_;

	public const int SpecialUnitTypeFieldNumber = 24;

	private int specialUnitType_;

	public const int ArmyStartPowerFieldNumber = 25;

	private int armyStartPower_;

	[DebuggerNonUserCode]
	public static MessageParser<LwBattlePlayerStat> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[10];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public ReportPlayerInfo User
	{
		get
		{
			return user_;
		}
		set
		{
			user_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ContentId
	{
		get
		{
			return contentId_;
		}
		set
		{
			contentId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxSoldierPower
	{
		get
		{
			return maxSoldierPower_;
		}
		set
		{
			maxSoldierPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SoldierPower
	{
		get
		{
			return soldierPower_;
		}
		set
		{
			soldierPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> Effects => effects_;

	[DebuggerNonUserCode]
	public int TotalHeroPower
	{
		get
		{
			return totalHeroPower_;
		}
		set
		{
			totalHeroPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> SoldierLost => soldierLost_;

	[DebuggerNonUserCode]
	public int ArmyType
	{
		get
		{
			return armyType_;
		}
		set
		{
			armyType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SoldierPowerBeforeStart
	{
		get
		{
			return soldierPowerBeforeStart_;
		}
		set
		{
			soldierPowerBeforeStart_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsHospitalFull
	{
		get
		{
			return isHospitalFull_;
		}
		set
		{
			isHospitalFull_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyProgress Progress
	{
		get
		{
			return progress_;
		}
		set
		{
			progress_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> SoldierBeforeStart => soldierBeforeStart_;

	[DebuggerNonUserCode]
	public int MaxSoldierCount
	{
		get
		{
			return maxSoldierCount_;
		}
		set
		{
			maxSoldierCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SoldierCountBeforeStart
	{
		get
		{
			return soldierCountBeforeStart_;
		}
		set
		{
			soldierCountBeforeStart_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SoldierCount
	{
		get
		{
			return soldierCount_;
		}
		set
		{
			soldierCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ChipTotalLevel
	{
		get
		{
			return chipTotalLevel_;
		}
		set
		{
			chipTotalLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float DeadRate
	{
		get
		{
			return deadRate_;
		}
		set
		{
			deadRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BattleExtraPowerInfo> ExtraPowers => extraPowers_;

	[DebuggerNonUserCode]
	public MapField<int, float> PowerTabMap => powerTabMap_;

	[DebuggerNonUserCode]
	public int WeaponStrengTotalLv
	{
		get
		{
			return weaponStrengTotalLv_;
		}
		set
		{
			weaponStrengTotalLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BattleCardModule BattleCard
	{
		get
		{
			return battleCard_;
		}
		set
		{
			battleCard_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierCount> WoundedToRemainDetail => woundedToRemainDetail_;

	[DebuggerNonUserCode]
	public SoldierElevenProto SoldierEleven
	{
		get
		{
			return soldierEleven_;
		}
		set
		{
			soldierEleven_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SpecialUnitType
	{
		get
		{
			return specialUnitType_;
		}
		set
		{
			specialUnitType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ArmyStartPower
	{
		get
		{
			return armyStartPower_;
		}
		set
		{
			armyStartPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwBattlePlayerStat()
	{
	}

	[DebuggerNonUserCode]
	public LwBattlePlayerStat(LwBattlePlayerStat other)
		: this()
	{
		user_ = ((other.user_ != null) ? other.user_.Clone() : null);
		contentId_ = other.contentId_;
		maxSoldierPower_ = other.maxSoldierPower_;
		soldierPower_ = other.soldierPower_;
		effects_ = other.effects_.Clone();
		totalHeroPower_ = other.totalHeroPower_;
		soldierLost_ = other.soldierLost_.Clone();
		armyType_ = other.armyType_;
		soldierPowerBeforeStart_ = other.soldierPowerBeforeStart_;
		isHospitalFull_ = other.isHospitalFull_;
		progress_ = ((other.progress_ != null) ? other.progress_.Clone() : null);
		soldierBeforeStart_ = other.soldierBeforeStart_.Clone();
		maxSoldierCount_ = other.maxSoldierCount_;
		soldierCountBeforeStart_ = other.soldierCountBeforeStart_;
		soldierCount_ = other.soldierCount_;
		chipTotalLevel_ = other.chipTotalLevel_;
		deadRate_ = other.deadRate_;
		extraPowers_ = other.extraPowers_.Clone();
		powerTabMap_ = other.powerTabMap_.Clone();
		weaponStrengTotalLv_ = other.weaponStrengTotalLv_;
		battleCard_ = ((other.battleCard_ != null) ? other.battleCard_.Clone() : null);
		woundedToRemainDetail_ = other.woundedToRemainDetail_.Clone();
		soldierEleven_ = ((other.soldierEleven_ != null) ? other.soldierEleven_.Clone() : null);
		specialUnitType_ = other.specialUnitType_;
		armyStartPower_ = other.armyStartPower_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattlePlayerStat Clone()
	{
		return new LwBattlePlayerStat(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattlePlayerStat);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattlePlayerStat other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(User, other.User))
		{
			return false;
		}
		if (ContentId != other.ContentId)
		{
			return false;
		}
		if (MaxSoldierPower != other.MaxSoldierPower)
		{
			return false;
		}
		if (SoldierPower != other.SoldierPower)
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		if (TotalHeroPower != other.TotalHeroPower)
		{
			return false;
		}
		if (!soldierLost_.Equals(other.soldierLost_))
		{
			return false;
		}
		if (ArmyType != other.ArmyType)
		{
			return false;
		}
		if (SoldierPowerBeforeStart != other.SoldierPowerBeforeStart)
		{
			return false;
		}
		if (IsHospitalFull != other.IsHospitalFull)
		{
			return false;
		}
		if (!object.Equals(Progress, other.Progress))
		{
			return false;
		}
		if (!soldierBeforeStart_.Equals(other.soldierBeforeStart_))
		{
			return false;
		}
		if (MaxSoldierCount != other.MaxSoldierCount)
		{
			return false;
		}
		if (SoldierCountBeforeStart != other.SoldierCountBeforeStart)
		{
			return false;
		}
		if (SoldierCount != other.SoldierCount)
		{
			return false;
		}
		if (ChipTotalLevel != other.ChipTotalLevel)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DeadRate, other.DeadRate))
		{
			return false;
		}
		if (!extraPowers_.Equals(other.extraPowers_))
		{
			return false;
		}
		if (!PowerTabMap.Equals(other.PowerTabMap))
		{
			return false;
		}
		if (WeaponStrengTotalLv != other.WeaponStrengTotalLv)
		{
			return false;
		}
		if (!object.Equals(BattleCard, other.BattleCard))
		{
			return false;
		}
		if (!woundedToRemainDetail_.Equals(other.woundedToRemainDetail_))
		{
			return false;
		}
		if (!object.Equals(SoldierEleven, other.SoldierEleven))
		{
			return false;
		}
		if (SpecialUnitType != other.SpecialUnitType)
		{
			return false;
		}
		if (ArmyStartPower != other.ArmyStartPower)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (user_ != null)
		{
			num ^= User.GetHashCode();
		}
		if (ContentId != 0)
		{
			num ^= ContentId.GetHashCode();
		}
		if (MaxSoldierPower != 0)
		{
			num ^= MaxSoldierPower.GetHashCode();
		}
		if (SoldierPower != 0)
		{
			num ^= SoldierPower.GetHashCode();
		}
		num ^= effects_.GetHashCode();
		if (TotalHeroPower != 0)
		{
			num ^= TotalHeroPower.GetHashCode();
		}
		num ^= soldierLost_.GetHashCode();
		if (ArmyType != 0)
		{
			num ^= ArmyType.GetHashCode();
		}
		if (SoldierPowerBeforeStart != 0)
		{
			num ^= SoldierPowerBeforeStart.GetHashCode();
		}
		if (IsHospitalFull)
		{
			num ^= IsHospitalFull.GetHashCode();
		}
		if (progress_ != null)
		{
			num ^= Progress.GetHashCode();
		}
		num ^= soldierBeforeStart_.GetHashCode();
		if (MaxSoldierCount != 0)
		{
			num ^= MaxSoldierCount.GetHashCode();
		}
		if (SoldierCountBeforeStart != 0)
		{
			num ^= SoldierCountBeforeStart.GetHashCode();
		}
		if (SoldierCount != 0)
		{
			num ^= SoldierCount.GetHashCode();
		}
		if (ChipTotalLevel != 0)
		{
			num ^= ChipTotalLevel.GetHashCode();
		}
		if (DeadRate != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DeadRate);
		}
		num ^= extraPowers_.GetHashCode();
		num ^= PowerTabMap.GetHashCode();
		if (WeaponStrengTotalLv != 0)
		{
			num ^= WeaponStrengTotalLv.GetHashCode();
		}
		if (battleCard_ != null)
		{
			num ^= BattleCard.GetHashCode();
		}
		num ^= woundedToRemainDetail_.GetHashCode();
		if (soldierEleven_ != null)
		{
			num ^= SoldierEleven.GetHashCode();
		}
		if (SpecialUnitType != 0)
		{
			num ^= SpecialUnitType.GetHashCode();
		}
		if (ArmyStartPower != 0)
		{
			num ^= ArmyStartPower.GetHashCode();
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
		if (user_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(User);
		}
		if (ContentId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ContentId);
		}
		if (MaxSoldierPower != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(MaxSoldierPower);
		}
		if (SoldierPower != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SoldierPower);
		}
		effects_.WriteTo(output, _repeated_effects_codec);
		if (TotalHeroPower != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(TotalHeroPower);
		}
		soldierLost_.WriteTo(output, _repeated_soldierLost_codec);
		if (ArmyType != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(ArmyType);
		}
		if (SoldierPowerBeforeStart != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(SoldierPowerBeforeStart);
		}
		if (IsHospitalFull)
		{
			output.WriteRawTag(80);
			output.WriteBool(IsHospitalFull);
		}
		if (progress_ != null)
		{
			output.WriteRawTag(90);
			output.WriteMessage(Progress);
		}
		soldierBeforeStart_.WriteTo(output, _repeated_soldierBeforeStart_codec);
		if (MaxSoldierCount != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(MaxSoldierCount);
		}
		if (SoldierCountBeforeStart != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(SoldierCountBeforeStart);
		}
		if (SoldierCount != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(SoldierCount);
		}
		if (ChipTotalLevel != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(ChipTotalLevel);
		}
		if (DeadRate != 0f)
		{
			output.WriteRawTag(141, 1);
			output.WriteFloat(DeadRate);
		}
		extraPowers_.WriteTo(output, _repeated_extraPowers_codec);
		powerTabMap_.WriteTo(output, _map_powerTabMap_codec);
		if (WeaponStrengTotalLv != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(WeaponStrengTotalLv);
		}
		if (battleCard_ != null)
		{
			output.WriteRawTag(170, 1);
			output.WriteMessage(BattleCard);
		}
		woundedToRemainDetail_.WriteTo(output, _repeated_woundedToRemainDetail_codec);
		if (soldierEleven_ != null)
		{
			output.WriteRawTag(186, 1);
			output.WriteMessage(SoldierEleven);
		}
		if (SpecialUnitType != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(SpecialUnitType);
		}
		if (ArmyStartPower != 0)
		{
			output.WriteRawTag(200, 1);
			output.WriteInt32(ArmyStartPower);
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
		if (user_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(User);
		}
		if (ContentId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ContentId);
		}
		if (MaxSoldierPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxSoldierPower);
		}
		if (SoldierPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierPower);
		}
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (TotalHeroPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalHeroPower);
		}
		num += soldierLost_.CalculateSize(_repeated_soldierLost_codec);
		if (ArmyType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ArmyType);
		}
		if (SoldierPowerBeforeStart != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierPowerBeforeStart);
		}
		if (IsHospitalFull)
		{
			num += 2;
		}
		if (progress_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Progress);
		}
		num += soldierBeforeStart_.CalculateSize(_repeated_soldierBeforeStart_codec);
		if (MaxSoldierCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxSoldierCount);
		}
		if (SoldierCountBeforeStart != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierCountBeforeStart);
		}
		if (SoldierCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierCount);
		}
		if (ChipTotalLevel != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ChipTotalLevel);
		}
		if (DeadRate != 0f)
		{
			num += 6;
		}
		num += extraPowers_.CalculateSize(_repeated_extraPowers_codec);
		num += powerTabMap_.CalculateSize(_map_powerTabMap_codec);
		if (WeaponStrengTotalLv != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WeaponStrengTotalLv);
		}
		if (battleCard_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(BattleCard);
		}
		num += woundedToRemainDetail_.CalculateSize(_repeated_woundedToRemainDetail_codec);
		if (soldierEleven_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(SoldierEleven);
		}
		if (SpecialUnitType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SpecialUnitType);
		}
		if (ArmyStartPower != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ArmyStartPower);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattlePlayerStat other)
	{
		if (other == null)
		{
			return;
		}
		if (other.user_ != null)
		{
			if (user_ == null)
			{
				User = new ReportPlayerInfo();
			}
			User.MergeFrom(other.User);
		}
		if (other.ContentId != 0)
		{
			ContentId = other.ContentId;
		}
		if (other.MaxSoldierPower != 0)
		{
			MaxSoldierPower = other.MaxSoldierPower;
		}
		if (other.SoldierPower != 0)
		{
			SoldierPower = other.SoldierPower;
		}
		effects_.Add(other.effects_);
		if (other.TotalHeroPower != 0)
		{
			TotalHeroPower = other.TotalHeroPower;
		}
		soldierLost_.Add(other.soldierLost_);
		if (other.ArmyType != 0)
		{
			ArmyType = other.ArmyType;
		}
		if (other.SoldierPowerBeforeStart != 0)
		{
			SoldierPowerBeforeStart = other.SoldierPowerBeforeStart;
		}
		if (other.IsHospitalFull)
		{
			IsHospitalFull = other.IsHospitalFull;
		}
		if (other.progress_ != null)
		{
			if (progress_ == null)
			{
				Progress = new ArmyProgress();
			}
			Progress.MergeFrom(other.Progress);
		}
		soldierBeforeStart_.Add(other.soldierBeforeStart_);
		if (other.MaxSoldierCount != 0)
		{
			MaxSoldierCount = other.MaxSoldierCount;
		}
		if (other.SoldierCountBeforeStart != 0)
		{
			SoldierCountBeforeStart = other.SoldierCountBeforeStart;
		}
		if (other.SoldierCount != 0)
		{
			SoldierCount = other.SoldierCount;
		}
		if (other.ChipTotalLevel != 0)
		{
			ChipTotalLevel = other.ChipTotalLevel;
		}
		if (other.DeadRate != 0f)
		{
			DeadRate = other.DeadRate;
		}
		extraPowers_.Add(other.extraPowers_);
		powerTabMap_.Add(other.powerTabMap_);
		if (other.WeaponStrengTotalLv != 0)
		{
			WeaponStrengTotalLv = other.WeaponStrengTotalLv;
		}
		if (other.battleCard_ != null)
		{
			if (battleCard_ == null)
			{
				BattleCard = new BattleCardModule();
			}
			BattleCard.MergeFrom(other.BattleCard);
		}
		woundedToRemainDetail_.Add(other.woundedToRemainDetail_);
		if (other.soldierEleven_ != null)
		{
			if (soldierEleven_ == null)
			{
				SoldierEleven = new SoldierElevenProto();
			}
			SoldierEleven.MergeFrom(other.SoldierEleven);
		}
		if (other.SpecialUnitType != 0)
		{
			SpecialUnitType = other.SpecialUnitType;
		}
		if (other.ArmyStartPower != 0)
		{
			ArmyStartPower = other.ArmyStartPower;
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
			case 10u:
				if (user_ == null)
				{
					User = new ReportPlayerInfo();
				}
				input.ReadMessage(User);
				break;
			case 16u:
				ContentId = input.ReadInt32();
				break;
			case 24u:
				MaxSoldierPower = input.ReadInt32();
				break;
			case 32u:
				SoldierPower = input.ReadInt32();
				break;
			case 42u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			case 48u:
				TotalHeroPower = input.ReadInt32();
				break;
			case 58u:
				soldierLost_.AddEntriesFrom(input, _repeated_soldierLost_codec);
				break;
			case 64u:
				ArmyType = input.ReadInt32();
				break;
			case 72u:
				SoldierPowerBeforeStart = input.ReadInt32();
				break;
			case 80u:
				IsHospitalFull = input.ReadBool();
				break;
			case 90u:
				if (progress_ == null)
				{
					Progress = new ArmyProgress();
				}
				input.ReadMessage(Progress);
				break;
			case 98u:
				soldierBeforeStart_.AddEntriesFrom(input, _repeated_soldierBeforeStart_codec);
				break;
			case 104u:
				MaxSoldierCount = input.ReadInt32();
				break;
			case 112u:
				SoldierCountBeforeStart = input.ReadInt32();
				break;
			case 120u:
				SoldierCount = input.ReadInt32();
				break;
			case 128u:
				ChipTotalLevel = input.ReadInt32();
				break;
			case 141u:
				DeadRate = input.ReadFloat();
				break;
			case 146u:
				extraPowers_.AddEntriesFrom(input, _repeated_extraPowers_codec);
				break;
			case 154u:
				powerTabMap_.AddEntriesFrom(input, _map_powerTabMap_codec);
				break;
			case 160u:
				WeaponStrengTotalLv = input.ReadInt32();
				break;
			case 170u:
				if (battleCard_ == null)
				{
					BattleCard = new BattleCardModule();
				}
				input.ReadMessage(BattleCard);
				break;
			case 178u:
				woundedToRemainDetail_.AddEntriesFrom(input, _repeated_woundedToRemainDetail_codec);
				break;
			case 186u:
				if (soldierEleven_ == null)
				{
					SoldierEleven = new SoldierElevenProto();
				}
				input.ReadMessage(SoldierEleven);
				break;
			case 192u:
				SpecialUnitType = input.ReadInt32();
				break;
			case 200u:
				ArmyStartPower = input.ReadInt32();
				break;
			}
		}
	}
}
