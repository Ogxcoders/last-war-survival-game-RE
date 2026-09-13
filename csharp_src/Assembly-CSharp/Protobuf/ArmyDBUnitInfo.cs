using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyDBUnitInfo : IMessage<ArmyDBUnitInfo>, IMessage, IEquatable<ArmyDBUnitInfo>, IDeepCloneable<ArmyDBUnitInfo>
{
	private static readonly MessageParser<ArmyDBUnitInfo> _parser = new MessageParser<ArmyDBUnitInfo>(() => new ArmyDBUnitInfo());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int SoldiersFieldNumber = 2;

	private static readonly FieldCodec<SoldierProto> _repeated_soldiers_codec = FieldCodec.ForMessage(18u, SoldierProto.Parser);

	private readonly RepeatedField<SoldierProto> soldiers_ = new RepeatedField<SoldierProto>();

	public const int HeroesFieldNumber = 3;

	private static readonly FieldCodec<HeroInfoProto> _repeated_heroes_codec = FieldCodec.ForMessage(26u, HeroInfoProto.Parser);

	private readonly RepeatedField<HeroInfoProto> heroes_ = new RepeatedField<HeroInfoProto>();

	public const int EffectInfoFieldNumber = 4;

	private ArmyUnitEffectInfoProto effectInfo_;

	public const int FormationIndexFieldNumber = 5;

	private int formationIndex_;

	public const int ArmyDataTypeFieldNumber = 10;

	private int armyDataType_;

	public const int ArmyIdFieldNumber = 11;

	private int armyId_;

	public const int ArmyHolderIdFieldNumber = 12;

	private int armyHolderId_;

	public const int EffectStringFieldNumber = 13;

	private string effectString_ = "";

	public const int SpecialUnitTypeFieldNumber = 14;

	private int specialUnitType_;

	public const int FormationUuidFieldNumber = 15;

	private long formationUuid_;

	public const int ProgressFieldNumber = 16;

	private ArmyProgress progress_;

	public const int WeaponFieldNumber = 17;

	private WeaponProto weapon_;

	public const int MeteoriteInfoFieldNumber = 18;

	private MeteoriteInfo meteoriteInfo_;

	public const int CostMummyRecordFieldNumber = 19;

	private string costMummyRecord_ = "";

	public const int HeroModuleFieldNumber = 20;

	private HeroModuleProto heroModule_;

	public const int StatisticsFieldNumber = 21;

	private ArmyStatistics statistics_;

	public const int BattleCardFieldNumber = 22;

	private BattleCardModule battleCard_;

	public const int SoldierElevenFieldNumber = 23;

	private SoldierElevenProto soldierEleven_;

	public const int ParallelExtInfoFieldNumber = 24;

	private ParallelExtInfo parallelExtInfo_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyDBUnitInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[29];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<SoldierProto> Soldiers => soldiers_;

	[DebuggerNonUserCode]
	public RepeatedField<HeroInfoProto> Heroes => heroes_;

	[DebuggerNonUserCode]
	public ArmyUnitEffectInfoProto EffectInfo
	{
		get
		{
			return effectInfo_;
		}
		set
		{
			effectInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FormationIndex
	{
		get
		{
			return formationIndex_;
		}
		set
		{
			formationIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ArmyDataType
	{
		get
		{
			return armyDataType_;
		}
		set
		{
			armyDataType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ArmyId
	{
		get
		{
			return armyId_;
		}
		set
		{
			armyId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ArmyHolderId
	{
		get
		{
			return armyHolderId_;
		}
		set
		{
			armyHolderId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string EffectString
	{
		get
		{
			return effectString_;
		}
		set
		{
			effectString_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public long FormationUuid
	{
		get
		{
			return formationUuid_;
		}
		set
		{
			formationUuid_ = value;
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
	public WeaponProto Weapon
	{
		get
		{
			return weapon_;
		}
		set
		{
			weapon_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MeteoriteInfo MeteoriteInfo
	{
		get
		{
			return meteoriteInfo_;
		}
		set
		{
			meteoriteInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CostMummyRecord
	{
		get
		{
			return costMummyRecord_;
		}
		set
		{
			costMummyRecord_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public HeroModuleProto HeroModule
	{
		get
		{
			return heroModule_;
		}
		set
		{
			heroModule_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyStatistics Statistics
	{
		get
		{
			return statistics_;
		}
		set
		{
			statistics_ = value;
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
	public ParallelExtInfo ParallelExtInfo
	{
		get
		{
			return parallelExtInfo_;
		}
		set
		{
			parallelExtInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyDBUnitInfo()
	{
	}

	[DebuggerNonUserCode]
	public ArmyDBUnitInfo(ArmyDBUnitInfo other)
		: this()
	{
		uid_ = other.uid_;
		soldiers_ = other.soldiers_.Clone();
		heroes_ = other.heroes_.Clone();
		effectInfo_ = ((other.effectInfo_ != null) ? other.effectInfo_.Clone() : null);
		formationIndex_ = other.formationIndex_;
		armyDataType_ = other.armyDataType_;
		armyId_ = other.armyId_;
		armyHolderId_ = other.armyHolderId_;
		effectString_ = other.effectString_;
		specialUnitType_ = other.specialUnitType_;
		formationUuid_ = other.formationUuid_;
		progress_ = ((other.progress_ != null) ? other.progress_.Clone() : null);
		weapon_ = ((other.weapon_ != null) ? other.weapon_.Clone() : null);
		meteoriteInfo_ = ((other.meteoriteInfo_ != null) ? other.meteoriteInfo_.Clone() : null);
		costMummyRecord_ = other.costMummyRecord_;
		heroModule_ = ((other.heroModule_ != null) ? other.heroModule_.Clone() : null);
		statistics_ = ((other.statistics_ != null) ? other.statistics_.Clone() : null);
		battleCard_ = ((other.battleCard_ != null) ? other.battleCard_.Clone() : null);
		soldierEleven_ = ((other.soldierEleven_ != null) ? other.soldierEleven_.Clone() : null);
		parallelExtInfo_ = ((other.parallelExtInfo_ != null) ? other.parallelExtInfo_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyDBUnitInfo Clone()
	{
		return new ArmyDBUnitInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyDBUnitInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyDBUnitInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (!soldiers_.Equals(other.soldiers_))
		{
			return false;
		}
		if (!heroes_.Equals(other.heroes_))
		{
			return false;
		}
		if (!object.Equals(EffectInfo, other.EffectInfo))
		{
			return false;
		}
		if (FormationIndex != other.FormationIndex)
		{
			return false;
		}
		if (ArmyDataType != other.ArmyDataType)
		{
			return false;
		}
		if (ArmyId != other.ArmyId)
		{
			return false;
		}
		if (ArmyHolderId != other.ArmyHolderId)
		{
			return false;
		}
		if (EffectString != other.EffectString)
		{
			return false;
		}
		if (SpecialUnitType != other.SpecialUnitType)
		{
			return false;
		}
		if (FormationUuid != other.FormationUuid)
		{
			return false;
		}
		if (!object.Equals(Progress, other.Progress))
		{
			return false;
		}
		if (!object.Equals(Weapon, other.Weapon))
		{
			return false;
		}
		if (!object.Equals(MeteoriteInfo, other.MeteoriteInfo))
		{
			return false;
		}
		if (CostMummyRecord != other.CostMummyRecord)
		{
			return false;
		}
		if (!object.Equals(HeroModule, other.HeroModule))
		{
			return false;
		}
		if (!object.Equals(Statistics, other.Statistics))
		{
			return false;
		}
		if (!object.Equals(BattleCard, other.BattleCard))
		{
			return false;
		}
		if (!object.Equals(SoldierEleven, other.SoldierEleven))
		{
			return false;
		}
		if (!object.Equals(ParallelExtInfo, other.ParallelExtInfo))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		num ^= soldiers_.GetHashCode();
		num ^= heroes_.GetHashCode();
		if (effectInfo_ != null)
		{
			num ^= EffectInfo.GetHashCode();
		}
		if (FormationIndex != 0)
		{
			num ^= FormationIndex.GetHashCode();
		}
		if (ArmyDataType != 0)
		{
			num ^= ArmyDataType.GetHashCode();
		}
		if (ArmyId != 0)
		{
			num ^= ArmyId.GetHashCode();
		}
		if (ArmyHolderId != 0)
		{
			num ^= ArmyHolderId.GetHashCode();
		}
		if (EffectString.Length != 0)
		{
			num ^= EffectString.GetHashCode();
		}
		if (SpecialUnitType != 0)
		{
			num ^= SpecialUnitType.GetHashCode();
		}
		if (FormationUuid != 0L)
		{
			num ^= FormationUuid.GetHashCode();
		}
		if (progress_ != null)
		{
			num ^= Progress.GetHashCode();
		}
		if (weapon_ != null)
		{
			num ^= Weapon.GetHashCode();
		}
		if (meteoriteInfo_ != null)
		{
			num ^= MeteoriteInfo.GetHashCode();
		}
		if (CostMummyRecord.Length != 0)
		{
			num ^= CostMummyRecord.GetHashCode();
		}
		if (heroModule_ != null)
		{
			num ^= HeroModule.GetHashCode();
		}
		if (statistics_ != null)
		{
			num ^= Statistics.GetHashCode();
		}
		if (battleCard_ != null)
		{
			num ^= BattleCard.GetHashCode();
		}
		if (soldierEleven_ != null)
		{
			num ^= SoldierEleven.GetHashCode();
		}
		if (parallelExtInfo_ != null)
		{
			num ^= ParallelExtInfo.GetHashCode();
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
		if (Uid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uid);
		}
		soldiers_.WriteTo(output, _repeated_soldiers_codec);
		heroes_.WriteTo(output, _repeated_heroes_codec);
		if (effectInfo_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(EffectInfo);
		}
		if (FormationIndex != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FormationIndex);
		}
		if (ArmyDataType != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(ArmyDataType);
		}
		if (ArmyId != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(ArmyId);
		}
		if (ArmyHolderId != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(ArmyHolderId);
		}
		if (EffectString.Length != 0)
		{
			output.WriteRawTag(106);
			output.WriteString(EffectString);
		}
		if (SpecialUnitType != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(SpecialUnitType);
		}
		if (FormationUuid != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(FormationUuid);
		}
		if (progress_ != null)
		{
			output.WriteRawTag(130, 1);
			output.WriteMessage(Progress);
		}
		if (weapon_ != null)
		{
			output.WriteRawTag(138, 1);
			output.WriteMessage(Weapon);
		}
		if (meteoriteInfo_ != null)
		{
			output.WriteRawTag(146, 1);
			output.WriteMessage(MeteoriteInfo);
		}
		if (CostMummyRecord.Length != 0)
		{
			output.WriteRawTag(154, 1);
			output.WriteString(CostMummyRecord);
		}
		if (heroModule_ != null)
		{
			output.WriteRawTag(162, 1);
			output.WriteMessage(HeroModule);
		}
		if (statistics_ != null)
		{
			output.WriteRawTag(170, 1);
			output.WriteMessage(Statistics);
		}
		if (battleCard_ != null)
		{
			output.WriteRawTag(178, 1);
			output.WriteMessage(BattleCard);
		}
		if (soldierEleven_ != null)
		{
			output.WriteRawTag(186, 1);
			output.WriteMessage(SoldierEleven);
		}
		if (parallelExtInfo_ != null)
		{
			output.WriteRawTag(194, 1);
			output.WriteMessage(ParallelExtInfo);
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
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		num += soldiers_.CalculateSize(_repeated_soldiers_codec);
		num += heroes_.CalculateSize(_repeated_heroes_codec);
		if (effectInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(EffectInfo);
		}
		if (FormationIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FormationIndex);
		}
		if (ArmyDataType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ArmyDataType);
		}
		if (ArmyId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ArmyId);
		}
		if (ArmyHolderId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ArmyHolderId);
		}
		if (EffectString.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(EffectString);
		}
		if (SpecialUnitType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SpecialUnitType);
		}
		if (FormationUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FormationUuid);
		}
		if (progress_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Progress);
		}
		if (weapon_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Weapon);
		}
		if (meteoriteInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MeteoriteInfo);
		}
		if (CostMummyRecord.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(CostMummyRecord);
		}
		if (heroModule_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(HeroModule);
		}
		if (statistics_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Statistics);
		}
		if (battleCard_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(BattleCard);
		}
		if (soldierEleven_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(SoldierEleven);
		}
		if (parallelExtInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ParallelExtInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyDBUnitInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uid.Length != 0)
		{
			Uid = other.Uid;
		}
		soldiers_.Add(other.soldiers_);
		heroes_.Add(other.heroes_);
		if (other.effectInfo_ != null)
		{
			if (effectInfo_ == null)
			{
				EffectInfo = new ArmyUnitEffectInfoProto();
			}
			EffectInfo.MergeFrom(other.EffectInfo);
		}
		if (other.FormationIndex != 0)
		{
			FormationIndex = other.FormationIndex;
		}
		if (other.ArmyDataType != 0)
		{
			ArmyDataType = other.ArmyDataType;
		}
		if (other.ArmyId != 0)
		{
			ArmyId = other.ArmyId;
		}
		if (other.ArmyHolderId != 0)
		{
			ArmyHolderId = other.ArmyHolderId;
		}
		if (other.EffectString.Length != 0)
		{
			EffectString = other.EffectString;
		}
		if (other.SpecialUnitType != 0)
		{
			SpecialUnitType = other.SpecialUnitType;
		}
		if (other.FormationUuid != 0L)
		{
			FormationUuid = other.FormationUuid;
		}
		if (other.progress_ != null)
		{
			if (progress_ == null)
			{
				Progress = new ArmyProgress();
			}
			Progress.MergeFrom(other.Progress);
		}
		if (other.weapon_ != null)
		{
			if (weapon_ == null)
			{
				Weapon = new WeaponProto();
			}
			Weapon.MergeFrom(other.Weapon);
		}
		if (other.meteoriteInfo_ != null)
		{
			if (meteoriteInfo_ == null)
			{
				MeteoriteInfo = new MeteoriteInfo();
			}
			MeteoriteInfo.MergeFrom(other.MeteoriteInfo);
		}
		if (other.CostMummyRecord.Length != 0)
		{
			CostMummyRecord = other.CostMummyRecord;
		}
		if (other.heroModule_ != null)
		{
			if (heroModule_ == null)
			{
				HeroModule = new HeroModuleProto();
			}
			HeroModule.MergeFrom(other.HeroModule);
		}
		if (other.statistics_ != null)
		{
			if (statistics_ == null)
			{
				Statistics = new ArmyStatistics();
			}
			Statistics.MergeFrom(other.Statistics);
		}
		if (other.battleCard_ != null)
		{
			if (battleCard_ == null)
			{
				BattleCard = new BattleCardModule();
			}
			BattleCard.MergeFrom(other.BattleCard);
		}
		if (other.soldierEleven_ != null)
		{
			if (soldierEleven_ == null)
			{
				SoldierEleven = new SoldierElevenProto();
			}
			SoldierEleven.MergeFrom(other.SoldierEleven);
		}
		if (other.parallelExtInfo_ != null)
		{
			if (parallelExtInfo_ == null)
			{
				ParallelExtInfo = new ParallelExtInfo();
			}
			ParallelExtInfo.MergeFrom(other.ParallelExtInfo);
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
				Uid = input.ReadString();
				break;
			case 18u:
				soldiers_.AddEntriesFrom(input, _repeated_soldiers_codec);
				break;
			case 26u:
				heroes_.AddEntriesFrom(input, _repeated_heroes_codec);
				break;
			case 34u:
				if (effectInfo_ == null)
				{
					EffectInfo = new ArmyUnitEffectInfoProto();
				}
				input.ReadMessage(EffectInfo);
				break;
			case 40u:
				FormationIndex = input.ReadInt32();
				break;
			case 80u:
				ArmyDataType = input.ReadInt32();
				break;
			case 88u:
				ArmyId = input.ReadInt32();
				break;
			case 96u:
				ArmyHolderId = input.ReadInt32();
				break;
			case 106u:
				EffectString = input.ReadString();
				break;
			case 112u:
				SpecialUnitType = input.ReadInt32();
				break;
			case 120u:
				FormationUuid = input.ReadInt64();
				break;
			case 130u:
				if (progress_ == null)
				{
					Progress = new ArmyProgress();
				}
				input.ReadMessage(Progress);
				break;
			case 138u:
				if (weapon_ == null)
				{
					Weapon = new WeaponProto();
				}
				input.ReadMessage(Weapon);
				break;
			case 146u:
				if (meteoriteInfo_ == null)
				{
					MeteoriteInfo = new MeteoriteInfo();
				}
				input.ReadMessage(MeteoriteInfo);
				break;
			case 154u:
				CostMummyRecord = input.ReadString();
				break;
			case 162u:
				if (heroModule_ == null)
				{
					HeroModule = new HeroModuleProto();
				}
				input.ReadMessage(HeroModule);
				break;
			case 170u:
				if (statistics_ == null)
				{
					Statistics = new ArmyStatistics();
				}
				input.ReadMessage(Statistics);
				break;
			case 178u:
				if (battleCard_ == null)
				{
					BattleCard = new BattleCardModule();
				}
				input.ReadMessage(BattleCard);
				break;
			case 186u:
				if (soldierEleven_ == null)
				{
					SoldierEleven = new SoldierElevenProto();
				}
				input.ReadMessage(SoldierEleven);
				break;
			case 194u:
				if (parallelExtInfo_ == null)
				{
					ParallelExtInfo = new ParallelExtInfo();
				}
				input.ReadMessage(ParallelExtInfo);
				break;
			}
		}
	}
}
