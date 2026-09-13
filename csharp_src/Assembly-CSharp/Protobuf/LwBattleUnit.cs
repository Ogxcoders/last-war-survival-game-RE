using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattleUnit : IMessage<LwBattleUnit>, IMessage, IEquatable<LwBattleUnit>, IDeepCloneable<LwBattleUnit>
{
	private static readonly MessageParser<LwBattleUnit> _parser = new MessageParser<LwBattleUnit>(() => new LwBattleUnit());

	private UnknownFieldSet _unknownFields;

	public const int HeroIdFieldNumber = 1;

	private int heroId_;

	public const int HeroLevelFieldNumber = 2;

	private int heroLevel_;

	public const int IndexFieldNumber = 3;

	private int index_;

	public const int HeroUuidFieldNumber = 4;

	private long heroUuid_;

	public const int SkillInfosFieldNumber = 5;

	private static readonly FieldCodec<HeroSkillInfoProto> _repeated_skillInfos_codec = FieldCodec.ForMessage(42u, HeroSkillInfoProto.Parser);

	private readonly RepeatedField<HeroSkillInfoProto> skillInfos_ = new RepeatedField<HeroSkillInfoProto>();

	public const int EffectsFieldNumber = 6;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effects_codec = FieldCodec.ForMessage(50u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effects_ = new RepeatedField<BattleEffectInfo>();

	public const int MaxHpFieldNumber = 7;

	private long maxHp_;

	public const int HpFieldNumber = 8;

	private long hp_;

	public const int HpBeforeStartFieldNumber = 9;

	private long hpBeforeStart_;

	public const int RankLvFieldNumber = 10;

	private int rankLv_;

	public const int EquipInfosFieldNumber = 11;

	private static readonly FieldCodec<HeroEquipInfoProto> _repeated_equipInfos_codec = FieldCodec.ForMessage(90u, HeroEquipInfoProto.Parser);

	private readonly RepeatedField<HeroEquipInfoProto> equipInfos_ = new RepeatedField<HeroEquipInfoProto>();

	public const int StatFieldNumber = 12;

	private LwBattleUnitStat stat_;

	public const int MaxSoldierCountFieldNumber = 13;

	private int maxSoldierCount_;

	public const int SoldierCountFieldNumber = 14;

	private int soldierCount_;

	public const int SkinIdFieldNumber = 15;

	private int skinId_;

	public const int UnitTypeFieldNumber = 16;

	private int unitType_;

	public const int SkillChipInfosFieldNumber = 17;

	private static readonly FieldCodec<SkillChipProto> _repeated_skillChipInfos_codec = FieldCodec.ForMessage(138u, SkillChipProto.Parser);

	private readonly RepeatedField<SkillChipProto> skillChipInfos_ = new RepeatedField<SkillChipProto>();

	public const int WeaponLevelFieldNumber = 18;

	private int weaponLevel_;

	public const int WeaponPowerFieldNumber = 19;

	private int weaponPower_;

	public const int SummonIdFieldNumber = 20;

	private int summonId_;

	public const int ExprireTimeFieldNumber = 21;

	private int exprireTime_;

	public const int NameFieldNumber = 22;

	private string name_ = "";

	public const int WeaponStrengthenFieldNumber = 23;

	private static readonly FieldCodec<WeaponStrengthen> _repeated_weaponStrengthen_codec = FieldCodec.ForMessage(186u, Protobuf.WeaponStrengthen.Parser);

	private readonly RepeatedField<WeaponStrengthen> weaponStrengthen_ = new RepeatedField<WeaponStrengthen>();

	public const int SoldierBeforeStartFieldNumber = 24;

	private static readonly MapField<int, LwSoldierLost>.Codec _map_soldierBeforeStart_codec = new MapField<int, LwSoldierLost>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, LwSoldierLost.Parser), 194u);

	private readonly MapField<int, LwSoldierLost> soldierBeforeStart_ = new MapField<int, LwSoldierLost>();

	[DebuggerNonUserCode]
	public static MessageParser<LwBattleUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int HeroLevel
	{
		get
		{
			return heroLevel_;
		}
		set
		{
			heroLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Index
	{
		get
		{
			return index_;
		}
		set
		{
			index_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeroUuid
	{
		get
		{
			return heroUuid_;
		}
		set
		{
			heroUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HeroSkillInfoProto> SkillInfos => skillInfos_;

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> Effects => effects_;

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
	public long Hp
	{
		get
		{
			return hp_;
		}
		set
		{
			hp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HpBeforeStart
	{
		get
		{
			return hpBeforeStart_;
		}
		set
		{
			hpBeforeStart_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RankLv
	{
		get
		{
			return rankLv_;
		}
		set
		{
			rankLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HeroEquipInfoProto> EquipInfos => equipInfos_;

	[DebuggerNonUserCode]
	public LwBattleUnitStat Stat
	{
		get
		{
			return stat_;
		}
		set
		{
			stat_ = value;
		}
	}

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
	public int SkinId
	{
		get
		{
			return skinId_;
		}
		set
		{
			skinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int UnitType
	{
		get
		{
			return unitType_;
		}
		set
		{
			unitType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<SkillChipProto> SkillChipInfos => skillChipInfos_;

	[DebuggerNonUserCode]
	public int WeaponLevel
	{
		get
		{
			return weaponLevel_;
		}
		set
		{
			weaponLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WeaponPower
	{
		get
		{
			return weaponPower_;
		}
		set
		{
			weaponPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SummonId
	{
		get
		{
			return summonId_;
		}
		set
		{
			summonId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ExprireTime
	{
		get
		{
			return exprireTime_;
		}
		set
		{
			exprireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<WeaponStrengthen> WeaponStrengthen => weaponStrengthen_;

	[DebuggerNonUserCode]
	public MapField<int, LwSoldierLost> SoldierBeforeStart => soldierBeforeStart_;

	[DebuggerNonUserCode]
	public LwBattleUnit()
	{
	}

	[DebuggerNonUserCode]
	public LwBattleUnit(LwBattleUnit other)
		: this()
	{
		heroId_ = other.heroId_;
		heroLevel_ = other.heroLevel_;
		index_ = other.index_;
		heroUuid_ = other.heroUuid_;
		skillInfos_ = other.skillInfos_.Clone();
		effects_ = other.effects_.Clone();
		maxHp_ = other.maxHp_;
		hp_ = other.hp_;
		hpBeforeStart_ = other.hpBeforeStart_;
		rankLv_ = other.rankLv_;
		equipInfos_ = other.equipInfos_.Clone();
		stat_ = ((other.stat_ != null) ? other.stat_.Clone() : null);
		maxSoldierCount_ = other.maxSoldierCount_;
		soldierCount_ = other.soldierCount_;
		skinId_ = other.skinId_;
		unitType_ = other.unitType_;
		skillChipInfos_ = other.skillChipInfos_.Clone();
		weaponLevel_ = other.weaponLevel_;
		weaponPower_ = other.weaponPower_;
		summonId_ = other.summonId_;
		exprireTime_ = other.exprireTime_;
		name_ = other.name_;
		weaponStrengthen_ = other.weaponStrengthen_.Clone();
		soldierBeforeStart_ = other.soldierBeforeStart_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattleUnit Clone()
	{
		return new LwBattleUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattleUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattleUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (HeroId != other.HeroId)
		{
			return false;
		}
		if (HeroLevel != other.HeroLevel)
		{
			return false;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (HeroUuid != other.HeroUuid)
		{
			return false;
		}
		if (!skillInfos_.Equals(other.skillInfos_))
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		if (MaxHp != other.MaxHp)
		{
			return false;
		}
		if (Hp != other.Hp)
		{
			return false;
		}
		if (HpBeforeStart != other.HpBeforeStart)
		{
			return false;
		}
		if (RankLv != other.RankLv)
		{
			return false;
		}
		if (!equipInfos_.Equals(other.equipInfos_))
		{
			return false;
		}
		if (!object.Equals(Stat, other.Stat))
		{
			return false;
		}
		if (MaxSoldierCount != other.MaxSoldierCount)
		{
			return false;
		}
		if (SoldierCount != other.SoldierCount)
		{
			return false;
		}
		if (SkinId != other.SkinId)
		{
			return false;
		}
		if (UnitType != other.UnitType)
		{
			return false;
		}
		if (!skillChipInfos_.Equals(other.skillChipInfos_))
		{
			return false;
		}
		if (WeaponLevel != other.WeaponLevel)
		{
			return false;
		}
		if (WeaponPower != other.WeaponPower)
		{
			return false;
		}
		if (SummonId != other.SummonId)
		{
			return false;
		}
		if (ExprireTime != other.ExprireTime)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (!weaponStrengthen_.Equals(other.weaponStrengthen_))
		{
			return false;
		}
		if (!SoldierBeforeStart.Equals(other.SoldierBeforeStart))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HeroId != 0)
		{
			num ^= HeroId.GetHashCode();
		}
		if (HeroLevel != 0)
		{
			num ^= HeroLevel.GetHashCode();
		}
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		if (HeroUuid != 0L)
		{
			num ^= HeroUuid.GetHashCode();
		}
		num ^= skillInfos_.GetHashCode();
		num ^= effects_.GetHashCode();
		if (MaxHp != 0L)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (Hp != 0L)
		{
			num ^= Hp.GetHashCode();
		}
		if (HpBeforeStart != 0L)
		{
			num ^= HpBeforeStart.GetHashCode();
		}
		if (RankLv != 0)
		{
			num ^= RankLv.GetHashCode();
		}
		num ^= equipInfos_.GetHashCode();
		if (stat_ != null)
		{
			num ^= Stat.GetHashCode();
		}
		if (MaxSoldierCount != 0)
		{
			num ^= MaxSoldierCount.GetHashCode();
		}
		if (SoldierCount != 0)
		{
			num ^= SoldierCount.GetHashCode();
		}
		if (SkinId != 0)
		{
			num ^= SkinId.GetHashCode();
		}
		if (UnitType != 0)
		{
			num ^= UnitType.GetHashCode();
		}
		num ^= skillChipInfos_.GetHashCode();
		if (WeaponLevel != 0)
		{
			num ^= WeaponLevel.GetHashCode();
		}
		if (WeaponPower != 0)
		{
			num ^= WeaponPower.GetHashCode();
		}
		if (SummonId != 0)
		{
			num ^= SummonId.GetHashCode();
		}
		if (ExprireTime != 0)
		{
			num ^= ExprireTime.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		num ^= weaponStrengthen_.GetHashCode();
		num ^= SoldierBeforeStart.GetHashCode();
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
		if (HeroId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(HeroId);
		}
		if (HeroLevel != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(HeroLevel);
		}
		if (Index != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Index);
		}
		if (HeroUuid != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(HeroUuid);
		}
		skillInfos_.WriteTo(output, _repeated_skillInfos_codec);
		effects_.WriteTo(output, _repeated_effects_codec);
		if (MaxHp != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(MaxHp);
		}
		if (Hp != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(Hp);
		}
		if (HpBeforeStart != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(HpBeforeStart);
		}
		if (RankLv != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(RankLv);
		}
		equipInfos_.WriteTo(output, _repeated_equipInfos_codec);
		if (stat_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(Stat);
		}
		if (MaxSoldierCount != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(MaxSoldierCount);
		}
		if (SoldierCount != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(SoldierCount);
		}
		if (SkinId != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(SkinId);
		}
		if (UnitType != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(UnitType);
		}
		skillChipInfos_.WriteTo(output, _repeated_skillChipInfos_codec);
		if (WeaponLevel != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(WeaponLevel);
		}
		if (WeaponPower != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(WeaponPower);
		}
		if (SummonId != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(SummonId);
		}
		if (ExprireTime != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(ExprireTime);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(178, 1);
			output.WriteString(Name);
		}
		weaponStrengthen_.WriteTo(output, _repeated_weaponStrengthen_codec);
		soldierBeforeStart_.WriteTo(output, _map_soldierBeforeStart_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (HeroId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroId);
		}
		if (HeroLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroLevel);
		}
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		if (HeroUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeroUuid);
		}
		num += skillInfos_.CalculateSize(_repeated_skillInfos_codec);
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (MaxHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxHp);
		}
		if (Hp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Hp);
		}
		if (HpBeforeStart != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HpBeforeStart);
		}
		if (RankLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RankLv);
		}
		num += equipInfos_.CalculateSize(_repeated_equipInfos_codec);
		if (stat_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Stat);
		}
		if (MaxSoldierCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxSoldierCount);
		}
		if (SoldierCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierCount);
		}
		if (SkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkinId);
		}
		if (UnitType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(UnitType);
		}
		num += skillChipInfos_.CalculateSize(_repeated_skillChipInfos_codec);
		if (WeaponLevel != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WeaponLevel);
		}
		if (WeaponPower != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WeaponPower);
		}
		if (SummonId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SummonId);
		}
		if (ExprireTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ExprireTime);
		}
		if (Name.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Name);
		}
		num += weaponStrengthen_.CalculateSize(_repeated_weaponStrengthen_codec);
		num += soldierBeforeStart_.CalculateSize(_map_soldierBeforeStart_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattleUnit other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HeroId != 0)
		{
			HeroId = other.HeroId;
		}
		if (other.HeroLevel != 0)
		{
			HeroLevel = other.HeroLevel;
		}
		if (other.Index != 0)
		{
			Index = other.Index;
		}
		if (other.HeroUuid != 0L)
		{
			HeroUuid = other.HeroUuid;
		}
		skillInfos_.Add(other.skillInfos_);
		effects_.Add(other.effects_);
		if (other.MaxHp != 0L)
		{
			MaxHp = other.MaxHp;
		}
		if (other.Hp != 0L)
		{
			Hp = other.Hp;
		}
		if (other.HpBeforeStart != 0L)
		{
			HpBeforeStart = other.HpBeforeStart;
		}
		if (other.RankLv != 0)
		{
			RankLv = other.RankLv;
		}
		equipInfos_.Add(other.equipInfos_);
		if (other.stat_ != null)
		{
			if (stat_ == null)
			{
				Stat = new LwBattleUnitStat();
			}
			Stat.MergeFrom(other.Stat);
		}
		if (other.MaxSoldierCount != 0)
		{
			MaxSoldierCount = other.MaxSoldierCount;
		}
		if (other.SoldierCount != 0)
		{
			SoldierCount = other.SoldierCount;
		}
		if (other.SkinId != 0)
		{
			SkinId = other.SkinId;
		}
		if (other.UnitType != 0)
		{
			UnitType = other.UnitType;
		}
		skillChipInfos_.Add(other.skillChipInfos_);
		if (other.WeaponLevel != 0)
		{
			WeaponLevel = other.WeaponLevel;
		}
		if (other.WeaponPower != 0)
		{
			WeaponPower = other.WeaponPower;
		}
		if (other.SummonId != 0)
		{
			SummonId = other.SummonId;
		}
		if (other.ExprireTime != 0)
		{
			ExprireTime = other.ExprireTime;
		}
		if (other.Name.Length != 0)
		{
			Name = other.Name;
		}
		weaponStrengthen_.Add(other.weaponStrengthen_);
		soldierBeforeStart_.Add(other.soldierBeforeStart_);
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
				HeroId = input.ReadInt32();
				break;
			case 16u:
				HeroLevel = input.ReadInt32();
				break;
			case 24u:
				Index = input.ReadInt32();
				break;
			case 32u:
				HeroUuid = input.ReadInt64();
				break;
			case 42u:
				skillInfos_.AddEntriesFrom(input, _repeated_skillInfos_codec);
				break;
			case 50u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			case 56u:
				MaxHp = input.ReadInt64();
				break;
			case 64u:
				Hp = input.ReadInt64();
				break;
			case 72u:
				HpBeforeStart = input.ReadInt64();
				break;
			case 80u:
				RankLv = input.ReadInt32();
				break;
			case 90u:
				equipInfos_.AddEntriesFrom(input, _repeated_equipInfos_codec);
				break;
			case 98u:
				if (stat_ == null)
				{
					Stat = new LwBattleUnitStat();
				}
				input.ReadMessage(Stat);
				break;
			case 104u:
				MaxSoldierCount = input.ReadInt32();
				break;
			case 112u:
				SoldierCount = input.ReadInt32();
				break;
			case 120u:
				SkinId = input.ReadInt32();
				break;
			case 128u:
				UnitType = input.ReadInt32();
				break;
			case 138u:
				skillChipInfos_.AddEntriesFrom(input, _repeated_skillChipInfos_codec);
				break;
			case 144u:
				WeaponLevel = input.ReadInt32();
				break;
			case 152u:
				WeaponPower = input.ReadInt32();
				break;
			case 160u:
				SummonId = input.ReadInt32();
				break;
			case 168u:
				ExprireTime = input.ReadInt32();
				break;
			case 178u:
				Name = input.ReadString();
				break;
			case 186u:
				weaponStrengthen_.AddEntriesFrom(input, _repeated_weaponStrengthen_codec);
				break;
			case 194u:
				soldierBeforeStart_.AddEntriesFrom(input, _map_soldierBeforeStart_codec);
				break;
			}
		}
	}
}
