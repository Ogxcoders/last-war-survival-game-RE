using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BuildInfo : IMessage<BuildInfo>, IMessage, IEquatable<BuildInfo>, IDeepCloneable<BuildInfo>
{
	private static readonly MessageParser<BuildInfo> _parser = new MessageParser<BuildInfo>(() => new BuildInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int BuildIdFieldNumber = 3;

	private int buildId_;

	public const int LevelFieldNumber = 4;

	private int level_;

	public const int BuildStateFieldNumber = 5;

	private int buildState_;

	public const int QueueStateFieldNumber = 6;

	private int queueState_;

	public const int AllianceIdFieldNumber = 7;

	private string allianceId_ = "";

	public const int UpdateEndTimeFieldNumber = 8;

	private int updateEndTime_;

	public const int UpdateStartTimeFieldNumber = 9;

	private int updateStartTime_;

	public const int LastHpTimeFieldNumber = 10;

	private int lastHpTime_;

	public const int ProtectEndTimeFieldNumber = 11;

	private int protectEndTime_;

	public const int InsideFieldNumber = 12;

	private int inside_;

	public const int CurrentHpFieldNumber = 13;

	private int currentHp_;

	public const int NameFieldNumber = 14;

	private string name_ = "";

	public const int AlAbbrFieldNumber = 15;

	private string alAbbr_ = "";

	public const int LastCollectTimeFieldNumber = 16;

	private int lastCollectTime_;

	public const int UnavailableTimeFieldNumber = 17;

	private int unavailableTime_;

	public const int MonthCardEndTimeFieldNumber = 18;

	private int monthCardEndTime_;

	public const int QueueItemIdFieldNumber = 19;

	private int queueItemId_;

	public const int QueueStartTimeFieldNumber = 20;

	private int queueStartTime_;

	public const int QueueUpdateTimeFieldNumber = 21;

	private int queueUpdateTime_;

	public const int DestroyStartTimeFieldNumber = 22;

	private int destroyStartTime_;

	public const int AppearanceIdFieldNumber = 23;

	private int appearanceId_;

	public const int SpecialTypeFieldNumber = 24;

	private SpecialType specialType_;

	public const int PositionIdFieldNumber = 25;

	private string positionId_ = "";

	public const int SkinsFieldNumber = 26;

	private static readonly FieldCodec<Skin> _repeated_skins_codec = FieldCodec.ForMessage(210u, Skin.Parser);

	private readonly RepeatedField<Skin> skins_ = new RepeatedField<Skin>();

	public const int CountryFieldNumber = 27;

	private string country_ = "";

	public const int RecoverSpeedFieldNumber = 28;

	private float recoverSpeed_;

	public const int VirusLayerFieldNumber = 29;

	private int virusLayer_;

	public const int VirusEndTimeFieldNumber = 30;

	private int virusEndTime_;

	public const int FireSpeedFieldNumber = 31;

	private float fireSpeed_;

	public const int StatusFieldNumber = 32;

	private static readonly FieldCodec<Status> _repeated_status_codec = FieldCodec.ForMessage(258u, Protobuf.Status.Parser);

	private readonly RepeatedField<Status> status_ = new RepeatedField<Status>();

	public const int ThermalConductorFieldNumber = 33;

	private ThermalConductor thermalConductor_;

	public const int SkillChantInfoFieldNumber = 34;

	private SkillChantInfo skillChantInfo_;

	public const int MonsterInvasionFieldNumber = 35;

	private MonsterInvasion monsterInvasion_;

	public const int RefuseTreadVirusFieldNumber = 36;

	private bool refuseTreadVirus_;

	public const int SeasonRoleFieldNumber = 37;

	private int seasonRole_;

	public const int ZoneMobilizationFieldNumber = 38;

	private ZoneMobilization zoneMobilization_;

	public const int SandwormFieldNumber = 39;

	private Sandworm sandworm_;

	public const int MummyChangeInfoFieldNumber = 40;

	private MummyChangeInfo mummyChangeInfo_;

	public const int DestroyEndTimeMsFieldNumber = 41;

	private long destroyEndTimeMs_;

	public const int MeteoriteInfoFieldNumber = 42;

	private MeteoriteInfo meteoriteInfo_;

	public const int CommonMonsterSkillInfoFieldNumber = 43;

	private CommonMonsterSkillInfo commonMonsterSkillInfo_;

	public const int ChallengeNewDonateFieldNumber = 44;

	private MonsterChallengeNewDonate challengeNewDonate_;

	public const int RefusePowerHelperFieldNumber = 45;

	private bool refusePowerHelper_;

	public const int LightHouseInfoFieldNumber = 46;

	private LightHouseInfo lightHouseInfo_;

	public const int WolfDecrHpFieldNumber = 47;

	private int wolfDecrHp_;

	public const int QuarantineRoleFieldNumber = 48;

	private int quarantineRole_;

	public const int QuarantineArbiterFieldNumber = 49;

	private int quarantineArbiter_;

	public const int QuarantineSkillFieldNumber = 50;

	private QuarantineSkillInfo quarantineSkill_;

	public const int QuarantineLeaveFieldNumber = 51;

	private int quarantineLeave_;

	public const int CurrentMaxHpFieldNumber = 52;

	private int currentMaxHp_;

	public const int FireworksFieldNumber = 53;

	private static readonly FieldCodec<FireWorksInfo> _repeated_fireworks_codec = FieldCodec.ForMessage(426u, FireWorksInfo.Parser);

	private readonly RepeatedField<FireWorksInfo> fireworks_ = new RepeatedField<FireWorksInfo>();

	public const int FireWorksGiftListFieldNumber = 54;

	private static readonly FieldCodec<FireWorksGift> _repeated_fireWorksGiftList_codec = FieldCodec.ForMessage(434u, FireWorksGift.Parser);

	private readonly RepeatedField<FireWorksGift> fireWorksGiftList_ = new RepeatedField<FireWorksGift>();

	[DebuggerNonUserCode]
	public static MessageParser<BuildInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string OwnerUid
	{
		get
		{
			return ownerUid_;
		}
		set
		{
			ownerUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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
	public int BuildId
	{
		get
		{
			return buildId_;
		}
		set
		{
			buildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuildState
	{
		get
		{
			return buildState_;
		}
		set
		{
			buildState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QueueState
	{
		get
		{
			return queueState_;
		}
		set
		{
			queueState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int UpdateEndTime
	{
		get
		{
			return updateEndTime_;
		}
		set
		{
			updateEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int UpdateStartTime
	{
		get
		{
			return updateStartTime_;
		}
		set
		{
			updateStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LastHpTime
	{
		get
		{
			return lastHpTime_;
		}
		set
		{
			lastHpTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ProtectEndTime
	{
		get
		{
			return protectEndTime_;
		}
		set
		{
			protectEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Inside
	{
		get
		{
			return inside_;
		}
		set
		{
			inside_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentHp
	{
		get
		{
			return currentHp_;
		}
		set
		{
			currentHp_ = value;
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
	public string AlAbbr
	{
		get
		{
			return alAbbr_;
		}
		set
		{
			alAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int LastCollectTime
	{
		get
		{
			return lastCollectTime_;
		}
		set
		{
			lastCollectTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int UnavailableTime
	{
		get
		{
			return unavailableTime_;
		}
		set
		{
			unavailableTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MonthCardEndTime
	{
		get
		{
			return monthCardEndTime_;
		}
		set
		{
			monthCardEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QueueItemId
	{
		get
		{
			return queueItemId_;
		}
		set
		{
			queueItemId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QueueStartTime
	{
		get
		{
			return queueStartTime_;
		}
		set
		{
			queueStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QueueUpdateTime
	{
		get
		{
			return queueUpdateTime_;
		}
		set
		{
			queueUpdateTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DestroyStartTime
	{
		get
		{
			return destroyStartTime_;
		}
		set
		{
			destroyStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AppearanceId
	{
		get
		{
			return appearanceId_;
		}
		set
		{
			appearanceId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SpecialType SpecialType
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
	public string PositionId
	{
		get
		{
			return positionId_;
		}
		set
		{
			positionId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Skin> Skins => skins_;

	[DebuggerNonUserCode]
	public string Country
	{
		get
		{
			return country_;
		}
		set
		{
			country_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public float RecoverSpeed
	{
		get
		{
			return recoverSpeed_;
		}
		set
		{
			recoverSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int VirusLayer
	{
		get
		{
			return virusLayer_;
		}
		set
		{
			virusLayer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int VirusEndTime
	{
		get
		{
			return virusEndTime_;
		}
		set
		{
			virusEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float FireSpeed
	{
		get
		{
			return fireSpeed_;
		}
		set
		{
			fireSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Status> Status => status_;

	[DebuggerNonUserCode]
	public ThermalConductor ThermalConductor
	{
		get
		{
			return thermalConductor_;
		}
		set
		{
			thermalConductor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SkillChantInfo SkillChantInfo
	{
		get
		{
			return skillChantInfo_;
		}
		set
		{
			skillChantInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterInvasion MonsterInvasion
	{
		get
		{
			return monsterInvasion_;
		}
		set
		{
			monsterInvasion_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool RefuseTreadVirus
	{
		get
		{
			return refuseTreadVirus_;
		}
		set
		{
			refuseTreadVirus_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SeasonRole
	{
		get
		{
			return seasonRole_;
		}
		set
		{
			seasonRole_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ZoneMobilization ZoneMobilization
	{
		get
		{
			return zoneMobilization_;
		}
		set
		{
			zoneMobilization_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Sandworm Sandworm
	{
		get
		{
			return sandworm_;
		}
		set
		{
			sandworm_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MummyChangeInfo MummyChangeInfo
	{
		get
		{
			return mummyChangeInfo_;
		}
		set
		{
			mummyChangeInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long DestroyEndTimeMs
	{
		get
		{
			return destroyEndTimeMs_;
		}
		set
		{
			destroyEndTimeMs_ = value;
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
	public CommonMonsterSkillInfo CommonMonsterSkillInfo
	{
		get
		{
			return commonMonsterSkillInfo_;
		}
		set
		{
			commonMonsterSkillInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterChallengeNewDonate ChallengeNewDonate
	{
		get
		{
			return challengeNewDonate_;
		}
		set
		{
			challengeNewDonate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool RefusePowerHelper
	{
		get
		{
			return refusePowerHelper_;
		}
		set
		{
			refusePowerHelper_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LightHouseInfo LightHouseInfo
	{
		get
		{
			return lightHouseInfo_;
		}
		set
		{
			lightHouseInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WolfDecrHp
	{
		get
		{
			return wolfDecrHp_;
		}
		set
		{
			wolfDecrHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QuarantineRole
	{
		get
		{
			return quarantineRole_;
		}
		set
		{
			quarantineRole_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QuarantineArbiter
	{
		get
		{
			return quarantineArbiter_;
		}
		set
		{
			quarantineArbiter_ = value;
		}
	}

	[DebuggerNonUserCode]
	public QuarantineSkillInfo QuarantineSkill
	{
		get
		{
			return quarantineSkill_;
		}
		set
		{
			quarantineSkill_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int QuarantineLeave
	{
		get
		{
			return quarantineLeave_;
		}
		set
		{
			quarantineLeave_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentMaxHp
	{
		get
		{
			return currentMaxHp_;
		}
		set
		{
			currentMaxHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<FireWorksInfo> Fireworks => fireworks_;

	[DebuggerNonUserCode]
	public RepeatedField<FireWorksGift> FireWorksGiftList => fireWorksGiftList_;

	[DebuggerNonUserCode]
	public BuildInfo()
	{
	}

	[DebuggerNonUserCode]
	public BuildInfo(BuildInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		uuid_ = other.uuid_;
		buildId_ = other.buildId_;
		level_ = other.level_;
		buildState_ = other.buildState_;
		queueState_ = other.queueState_;
		allianceId_ = other.allianceId_;
		updateEndTime_ = other.updateEndTime_;
		updateStartTime_ = other.updateStartTime_;
		lastHpTime_ = other.lastHpTime_;
		protectEndTime_ = other.protectEndTime_;
		inside_ = other.inside_;
		currentHp_ = other.currentHp_;
		name_ = other.name_;
		alAbbr_ = other.alAbbr_;
		lastCollectTime_ = other.lastCollectTime_;
		unavailableTime_ = other.unavailableTime_;
		monthCardEndTime_ = other.monthCardEndTime_;
		queueItemId_ = other.queueItemId_;
		queueStartTime_ = other.queueStartTime_;
		queueUpdateTime_ = other.queueUpdateTime_;
		destroyStartTime_ = other.destroyStartTime_;
		appearanceId_ = other.appearanceId_;
		specialType_ = other.specialType_;
		positionId_ = other.positionId_;
		skins_ = other.skins_.Clone();
		country_ = other.country_;
		recoverSpeed_ = other.recoverSpeed_;
		virusLayer_ = other.virusLayer_;
		virusEndTime_ = other.virusEndTime_;
		fireSpeed_ = other.fireSpeed_;
		status_ = other.status_.Clone();
		thermalConductor_ = ((other.thermalConductor_ != null) ? other.thermalConductor_.Clone() : null);
		skillChantInfo_ = ((other.skillChantInfo_ != null) ? other.skillChantInfo_.Clone() : null);
		monsterInvasion_ = ((other.monsterInvasion_ != null) ? other.monsterInvasion_.Clone() : null);
		refuseTreadVirus_ = other.refuseTreadVirus_;
		seasonRole_ = other.seasonRole_;
		zoneMobilization_ = ((other.zoneMobilization_ != null) ? other.zoneMobilization_.Clone() : null);
		sandworm_ = ((other.sandworm_ != null) ? other.sandworm_.Clone() : null);
		mummyChangeInfo_ = ((other.mummyChangeInfo_ != null) ? other.mummyChangeInfo_.Clone() : null);
		destroyEndTimeMs_ = other.destroyEndTimeMs_;
		meteoriteInfo_ = ((other.meteoriteInfo_ != null) ? other.meteoriteInfo_.Clone() : null);
		commonMonsterSkillInfo_ = ((other.commonMonsterSkillInfo_ != null) ? other.commonMonsterSkillInfo_.Clone() : null);
		challengeNewDonate_ = ((other.challengeNewDonate_ != null) ? other.challengeNewDonate_.Clone() : null);
		refusePowerHelper_ = other.refusePowerHelper_;
		lightHouseInfo_ = ((other.lightHouseInfo_ != null) ? other.lightHouseInfo_.Clone() : null);
		wolfDecrHp_ = other.wolfDecrHp_;
		quarantineRole_ = other.quarantineRole_;
		quarantineArbiter_ = other.quarantineArbiter_;
		quarantineSkill_ = ((other.quarantineSkill_ != null) ? other.quarantineSkill_.Clone() : null);
		quarantineLeave_ = other.quarantineLeave_;
		currentMaxHp_ = other.currentMaxHp_;
		fireworks_ = other.fireworks_.Clone();
		fireWorksGiftList_ = other.fireWorksGiftList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuildInfo Clone()
	{
		return new BuildInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuildInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuildInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (OwnerUid != other.OwnerUid)
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (BuildState != other.BuildState)
		{
			return false;
		}
		if (QueueState != other.QueueState)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (UpdateEndTime != other.UpdateEndTime)
		{
			return false;
		}
		if (UpdateStartTime != other.UpdateStartTime)
		{
			return false;
		}
		if (LastHpTime != other.LastHpTime)
		{
			return false;
		}
		if (ProtectEndTime != other.ProtectEndTime)
		{
			return false;
		}
		if (Inside != other.Inside)
		{
			return false;
		}
		if (CurrentHp != other.CurrentHp)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (LastCollectTime != other.LastCollectTime)
		{
			return false;
		}
		if (UnavailableTime != other.UnavailableTime)
		{
			return false;
		}
		if (MonthCardEndTime != other.MonthCardEndTime)
		{
			return false;
		}
		if (QueueItemId != other.QueueItemId)
		{
			return false;
		}
		if (QueueStartTime != other.QueueStartTime)
		{
			return false;
		}
		if (QueueUpdateTime != other.QueueUpdateTime)
		{
			return false;
		}
		if (DestroyStartTime != other.DestroyStartTime)
		{
			return false;
		}
		if (AppearanceId != other.AppearanceId)
		{
			return false;
		}
		if (SpecialType != other.SpecialType)
		{
			return false;
		}
		if (PositionId != other.PositionId)
		{
			return false;
		}
		if (!skins_.Equals(other.skins_))
		{
			return false;
		}
		if (Country != other.Country)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(RecoverSpeed, other.RecoverSpeed))
		{
			return false;
		}
		if (VirusLayer != other.VirusLayer)
		{
			return false;
		}
		if (VirusEndTime != other.VirusEndTime)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(FireSpeed, other.FireSpeed))
		{
			return false;
		}
		if (!status_.Equals(other.status_))
		{
			return false;
		}
		if (!object.Equals(ThermalConductor, other.ThermalConductor))
		{
			return false;
		}
		if (!object.Equals(SkillChantInfo, other.SkillChantInfo))
		{
			return false;
		}
		if (!object.Equals(MonsterInvasion, other.MonsterInvasion))
		{
			return false;
		}
		if (RefuseTreadVirus != other.RefuseTreadVirus)
		{
			return false;
		}
		if (SeasonRole != other.SeasonRole)
		{
			return false;
		}
		if (!object.Equals(ZoneMobilization, other.ZoneMobilization))
		{
			return false;
		}
		if (!object.Equals(Sandworm, other.Sandworm))
		{
			return false;
		}
		if (!object.Equals(MummyChangeInfo, other.MummyChangeInfo))
		{
			return false;
		}
		if (DestroyEndTimeMs != other.DestroyEndTimeMs)
		{
			return false;
		}
		if (!object.Equals(MeteoriteInfo, other.MeteoriteInfo))
		{
			return false;
		}
		if (!object.Equals(CommonMonsterSkillInfo, other.CommonMonsterSkillInfo))
		{
			return false;
		}
		if (!object.Equals(ChallengeNewDonate, other.ChallengeNewDonate))
		{
			return false;
		}
		if (RefusePowerHelper != other.RefusePowerHelper)
		{
			return false;
		}
		if (!object.Equals(LightHouseInfo, other.LightHouseInfo))
		{
			return false;
		}
		if (WolfDecrHp != other.WolfDecrHp)
		{
			return false;
		}
		if (QuarantineRole != other.QuarantineRole)
		{
			return false;
		}
		if (QuarantineArbiter != other.QuarantineArbiter)
		{
			return false;
		}
		if (!object.Equals(QuarantineSkill, other.QuarantineSkill))
		{
			return false;
		}
		if (QuarantineLeave != other.QuarantineLeave)
		{
			return false;
		}
		if (CurrentMaxHp != other.CurrentMaxHp)
		{
			return false;
		}
		if (!fireworks_.Equals(other.fireworks_))
		{
			return false;
		}
		if (!fireWorksGiftList_.Equals(other.fireWorksGiftList_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (OwnerUid.Length != 0)
		{
			num ^= OwnerUid.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (BuildId != 0)
		{
			num ^= BuildId.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (BuildState != 0)
		{
			num ^= BuildState.GetHashCode();
		}
		if (QueueState != 0)
		{
			num ^= QueueState.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (UpdateEndTime != 0)
		{
			num ^= UpdateEndTime.GetHashCode();
		}
		if (UpdateStartTime != 0)
		{
			num ^= UpdateStartTime.GetHashCode();
		}
		if (LastHpTime != 0)
		{
			num ^= LastHpTime.GetHashCode();
		}
		if (ProtectEndTime != 0)
		{
			num ^= ProtectEndTime.GetHashCode();
		}
		if (Inside != 0)
		{
			num ^= Inside.GetHashCode();
		}
		if (CurrentHp != 0)
		{
			num ^= CurrentHp.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (LastCollectTime != 0)
		{
			num ^= LastCollectTime.GetHashCode();
		}
		if (UnavailableTime != 0)
		{
			num ^= UnavailableTime.GetHashCode();
		}
		if (MonthCardEndTime != 0)
		{
			num ^= MonthCardEndTime.GetHashCode();
		}
		if (QueueItemId != 0)
		{
			num ^= QueueItemId.GetHashCode();
		}
		if (QueueStartTime != 0)
		{
			num ^= QueueStartTime.GetHashCode();
		}
		if (QueueUpdateTime != 0)
		{
			num ^= QueueUpdateTime.GetHashCode();
		}
		if (DestroyStartTime != 0)
		{
			num ^= DestroyStartTime.GetHashCode();
		}
		if (AppearanceId != 0)
		{
			num ^= AppearanceId.GetHashCode();
		}
		if (SpecialType != SpecialType.None)
		{
			num ^= SpecialType.GetHashCode();
		}
		if (PositionId.Length != 0)
		{
			num ^= PositionId.GetHashCode();
		}
		num ^= skins_.GetHashCode();
		if (Country.Length != 0)
		{
			num ^= Country.GetHashCode();
		}
		if (RecoverSpeed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(RecoverSpeed);
		}
		if (VirusLayer != 0)
		{
			num ^= VirusLayer.GetHashCode();
		}
		if (VirusEndTime != 0)
		{
			num ^= VirusEndTime.GetHashCode();
		}
		if (FireSpeed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(FireSpeed);
		}
		num ^= status_.GetHashCode();
		if (thermalConductor_ != null)
		{
			num ^= ThermalConductor.GetHashCode();
		}
		if (skillChantInfo_ != null)
		{
			num ^= SkillChantInfo.GetHashCode();
		}
		if (monsterInvasion_ != null)
		{
			num ^= MonsterInvasion.GetHashCode();
		}
		if (RefuseTreadVirus)
		{
			num ^= RefuseTreadVirus.GetHashCode();
		}
		if (SeasonRole != 0)
		{
			num ^= SeasonRole.GetHashCode();
		}
		if (zoneMobilization_ != null)
		{
			num ^= ZoneMobilization.GetHashCode();
		}
		if (sandworm_ != null)
		{
			num ^= Sandworm.GetHashCode();
		}
		if (mummyChangeInfo_ != null)
		{
			num ^= MummyChangeInfo.GetHashCode();
		}
		if (DestroyEndTimeMs != 0L)
		{
			num ^= DestroyEndTimeMs.GetHashCode();
		}
		if (meteoriteInfo_ != null)
		{
			num ^= MeteoriteInfo.GetHashCode();
		}
		if (commonMonsterSkillInfo_ != null)
		{
			num ^= CommonMonsterSkillInfo.GetHashCode();
		}
		if (challengeNewDonate_ != null)
		{
			num ^= ChallengeNewDonate.GetHashCode();
		}
		if (RefusePowerHelper)
		{
			num ^= RefusePowerHelper.GetHashCode();
		}
		if (lightHouseInfo_ != null)
		{
			num ^= LightHouseInfo.GetHashCode();
		}
		if (WolfDecrHp != 0)
		{
			num ^= WolfDecrHp.GetHashCode();
		}
		if (QuarantineRole != 0)
		{
			num ^= QuarantineRole.GetHashCode();
		}
		if (QuarantineArbiter != 0)
		{
			num ^= QuarantineArbiter.GetHashCode();
		}
		if (quarantineSkill_ != null)
		{
			num ^= QuarantineSkill.GetHashCode();
		}
		if (QuarantineLeave != 0)
		{
			num ^= QuarantineLeave.GetHashCode();
		}
		if (CurrentMaxHp != 0)
		{
			num ^= CurrentMaxHp.GetHashCode();
		}
		num ^= fireworks_.GetHashCode();
		num ^= fireWorksGiftList_.GetHashCode();
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
		if (OwnerUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(OwnerUid);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Uuid);
		}
		if (BuildId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(BuildId);
		}
		if (Level != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Level);
		}
		if (BuildState != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(BuildState);
		}
		if (QueueState != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(QueueState);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(AllianceId);
		}
		if (UpdateEndTime != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(UpdateEndTime);
		}
		if (UpdateStartTime != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(UpdateStartTime);
		}
		if (LastHpTime != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(LastHpTime);
		}
		if (ProtectEndTime != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(ProtectEndTime);
		}
		if (Inside != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(Inside);
		}
		if (CurrentHp != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(CurrentHp);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(Name);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(122);
			output.WriteString(AlAbbr);
		}
		if (LastCollectTime != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(LastCollectTime);
		}
		if (UnavailableTime != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(UnavailableTime);
		}
		if (MonthCardEndTime != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(MonthCardEndTime);
		}
		if (QueueItemId != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(QueueItemId);
		}
		if (QueueStartTime != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(QueueStartTime);
		}
		if (QueueUpdateTime != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(QueueUpdateTime);
		}
		if (DestroyStartTime != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(DestroyStartTime);
		}
		if (AppearanceId != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(AppearanceId);
		}
		if (SpecialType != SpecialType.None)
		{
			output.WriteRawTag(192, 1);
			output.WriteEnum((int)SpecialType);
		}
		if (PositionId.Length != 0)
		{
			output.WriteRawTag(202, 1);
			output.WriteString(PositionId);
		}
		skins_.WriteTo(output, _repeated_skins_codec);
		if (Country.Length != 0)
		{
			output.WriteRawTag(218, 1);
			output.WriteString(Country);
		}
		if (RecoverSpeed != 0f)
		{
			output.WriteRawTag(229, 1);
			output.WriteFloat(RecoverSpeed);
		}
		if (VirusLayer != 0)
		{
			output.WriteRawTag(232, 1);
			output.WriteInt32(VirusLayer);
		}
		if (VirusEndTime != 0)
		{
			output.WriteRawTag(240, 1);
			output.WriteInt32(VirusEndTime);
		}
		if (FireSpeed != 0f)
		{
			output.WriteRawTag(253, 1);
			output.WriteFloat(FireSpeed);
		}
		status_.WriteTo(output, _repeated_status_codec);
		if (thermalConductor_ != null)
		{
			output.WriteRawTag(138, 2);
			output.WriteMessage(ThermalConductor);
		}
		if (skillChantInfo_ != null)
		{
			output.WriteRawTag(146, 2);
			output.WriteMessage(SkillChantInfo);
		}
		if (monsterInvasion_ != null)
		{
			output.WriteRawTag(154, 2);
			output.WriteMessage(MonsterInvasion);
		}
		if (RefuseTreadVirus)
		{
			output.WriteRawTag(160, 2);
			output.WriteBool(RefuseTreadVirus);
		}
		if (SeasonRole != 0)
		{
			output.WriteRawTag(168, 2);
			output.WriteInt32(SeasonRole);
		}
		if (zoneMobilization_ != null)
		{
			output.WriteRawTag(178, 2);
			output.WriteMessage(ZoneMobilization);
		}
		if (sandworm_ != null)
		{
			output.WriteRawTag(186, 2);
			output.WriteMessage(Sandworm);
		}
		if (mummyChangeInfo_ != null)
		{
			output.WriteRawTag(194, 2);
			output.WriteMessage(MummyChangeInfo);
		}
		if (DestroyEndTimeMs != 0L)
		{
			output.WriteRawTag(200, 2);
			output.WriteInt64(DestroyEndTimeMs);
		}
		if (meteoriteInfo_ != null)
		{
			output.WriteRawTag(210, 2);
			output.WriteMessage(MeteoriteInfo);
		}
		if (commonMonsterSkillInfo_ != null)
		{
			output.WriteRawTag(218, 2);
			output.WriteMessage(CommonMonsterSkillInfo);
		}
		if (challengeNewDonate_ != null)
		{
			output.WriteRawTag(226, 2);
			output.WriteMessage(ChallengeNewDonate);
		}
		if (RefusePowerHelper)
		{
			output.WriteRawTag(232, 2);
			output.WriteBool(RefusePowerHelper);
		}
		if (lightHouseInfo_ != null)
		{
			output.WriteRawTag(242, 2);
			output.WriteMessage(LightHouseInfo);
		}
		if (WolfDecrHp != 0)
		{
			output.WriteRawTag(248, 2);
			output.WriteInt32(WolfDecrHp);
		}
		if (QuarantineRole != 0)
		{
			output.WriteRawTag(128, 3);
			output.WriteInt32(QuarantineRole);
		}
		if (QuarantineArbiter != 0)
		{
			output.WriteRawTag(136, 3);
			output.WriteInt32(QuarantineArbiter);
		}
		if (quarantineSkill_ != null)
		{
			output.WriteRawTag(146, 3);
			output.WriteMessage(QuarantineSkill);
		}
		if (QuarantineLeave != 0)
		{
			output.WriteRawTag(152, 3);
			output.WriteInt32(QuarantineLeave);
		}
		if (CurrentMaxHp != 0)
		{
			output.WriteRawTag(160, 3);
			output.WriteInt32(CurrentMaxHp);
		}
		fireworks_.WriteTo(output, _repeated_fireworks_codec);
		fireWorksGiftList_.WriteTo(output, _repeated_fireWorksGiftList_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (OwnerUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerUid);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (BuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildId);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (BuildState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildState);
		}
		if (QueueState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(QueueState);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (UpdateEndTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(UpdateEndTime);
		}
		if (UpdateStartTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(UpdateStartTime);
		}
		if (LastHpTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LastHpTime);
		}
		if (ProtectEndTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ProtectEndTime);
		}
		if (Inside != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Inside);
		}
		if (CurrentHp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentHp);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (LastCollectTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LastCollectTime);
		}
		if (UnavailableTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(UnavailableTime);
		}
		if (MonthCardEndTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MonthCardEndTime);
		}
		if (QueueItemId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QueueItemId);
		}
		if (QueueStartTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QueueStartTime);
		}
		if (QueueUpdateTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QueueUpdateTime);
		}
		if (DestroyStartTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(DestroyStartTime);
		}
		if (AppearanceId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(AppearanceId);
		}
		if (SpecialType != SpecialType.None)
		{
			num += 2 + CodedOutputStream.ComputeEnumSize((int)SpecialType);
		}
		if (PositionId.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(PositionId);
		}
		num += skins_.CalculateSize(_repeated_skins_codec);
		if (Country.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Country);
		}
		if (RecoverSpeed != 0f)
		{
			num += 6;
		}
		if (VirusLayer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(VirusLayer);
		}
		if (VirusEndTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(VirusEndTime);
		}
		if (FireSpeed != 0f)
		{
			num += 6;
		}
		num += status_.CalculateSize(_repeated_status_codec);
		if (thermalConductor_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ThermalConductor);
		}
		if (skillChantInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(SkillChantInfo);
		}
		if (monsterInvasion_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MonsterInvasion);
		}
		if (RefuseTreadVirus)
		{
			num += 3;
		}
		if (SeasonRole != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SeasonRole);
		}
		if (zoneMobilization_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ZoneMobilization);
		}
		if (sandworm_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Sandworm);
		}
		if (mummyChangeInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MummyChangeInfo);
		}
		if (DestroyEndTimeMs != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(DestroyEndTimeMs);
		}
		if (meteoriteInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MeteoriteInfo);
		}
		if (commonMonsterSkillInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(CommonMonsterSkillInfo);
		}
		if (challengeNewDonate_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ChallengeNewDonate);
		}
		if (RefusePowerHelper)
		{
			num += 3;
		}
		if (lightHouseInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(LightHouseInfo);
		}
		if (WolfDecrHp != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WolfDecrHp);
		}
		if (QuarantineRole != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QuarantineRole);
		}
		if (QuarantineArbiter != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QuarantineArbiter);
		}
		if (quarantineSkill_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(QuarantineSkill);
		}
		if (QuarantineLeave != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QuarantineLeave);
		}
		if (CurrentMaxHp != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentMaxHp);
		}
		num += fireworks_.CalculateSize(_repeated_fireworks_codec);
		num += fireWorksGiftList_.CalculateSize(_repeated_fireWorksGiftList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuildInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.OwnerUid.Length != 0)
		{
			OwnerUid = other.OwnerUid;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.BuildId != 0)
		{
			BuildId = other.BuildId;
		}
		if (other.Level != 0)
		{
			Level = other.Level;
		}
		if (other.BuildState != 0)
		{
			BuildState = other.BuildState;
		}
		if (other.QueueState != 0)
		{
			QueueState = other.QueueState;
		}
		if (other.AllianceId.Length != 0)
		{
			AllianceId = other.AllianceId;
		}
		if (other.UpdateEndTime != 0)
		{
			UpdateEndTime = other.UpdateEndTime;
		}
		if (other.UpdateStartTime != 0)
		{
			UpdateStartTime = other.UpdateStartTime;
		}
		if (other.LastHpTime != 0)
		{
			LastHpTime = other.LastHpTime;
		}
		if (other.ProtectEndTime != 0)
		{
			ProtectEndTime = other.ProtectEndTime;
		}
		if (other.Inside != 0)
		{
			Inside = other.Inside;
		}
		if (other.CurrentHp != 0)
		{
			CurrentHp = other.CurrentHp;
		}
		if (other.Name.Length != 0)
		{
			Name = other.Name;
		}
		if (other.AlAbbr.Length != 0)
		{
			AlAbbr = other.AlAbbr;
		}
		if (other.LastCollectTime != 0)
		{
			LastCollectTime = other.LastCollectTime;
		}
		if (other.UnavailableTime != 0)
		{
			UnavailableTime = other.UnavailableTime;
		}
		if (other.MonthCardEndTime != 0)
		{
			MonthCardEndTime = other.MonthCardEndTime;
		}
		if (other.QueueItemId != 0)
		{
			QueueItemId = other.QueueItemId;
		}
		if (other.QueueStartTime != 0)
		{
			QueueStartTime = other.QueueStartTime;
		}
		if (other.QueueUpdateTime != 0)
		{
			QueueUpdateTime = other.QueueUpdateTime;
		}
		if (other.DestroyStartTime != 0)
		{
			DestroyStartTime = other.DestroyStartTime;
		}
		if (other.AppearanceId != 0)
		{
			AppearanceId = other.AppearanceId;
		}
		if (other.SpecialType != SpecialType.None)
		{
			SpecialType = other.SpecialType;
		}
		if (other.PositionId.Length != 0)
		{
			PositionId = other.PositionId;
		}
		skins_.Add(other.skins_);
		if (other.Country.Length != 0)
		{
			Country = other.Country;
		}
		if (other.RecoverSpeed != 0f)
		{
			RecoverSpeed = other.RecoverSpeed;
		}
		if (other.VirusLayer != 0)
		{
			VirusLayer = other.VirusLayer;
		}
		if (other.VirusEndTime != 0)
		{
			VirusEndTime = other.VirusEndTime;
		}
		if (other.FireSpeed != 0f)
		{
			FireSpeed = other.FireSpeed;
		}
		status_.Add(other.status_);
		if (other.thermalConductor_ != null)
		{
			if (thermalConductor_ == null)
			{
				ThermalConductor = new ThermalConductor();
			}
			ThermalConductor.MergeFrom(other.ThermalConductor);
		}
		if (other.skillChantInfo_ != null)
		{
			if (skillChantInfo_ == null)
			{
				SkillChantInfo = new SkillChantInfo();
			}
			SkillChantInfo.MergeFrom(other.SkillChantInfo);
		}
		if (other.monsterInvasion_ != null)
		{
			if (monsterInvasion_ == null)
			{
				MonsterInvasion = new MonsterInvasion();
			}
			MonsterInvasion.MergeFrom(other.MonsterInvasion);
		}
		if (other.RefuseTreadVirus)
		{
			RefuseTreadVirus = other.RefuseTreadVirus;
		}
		if (other.SeasonRole != 0)
		{
			SeasonRole = other.SeasonRole;
		}
		if (other.zoneMobilization_ != null)
		{
			if (zoneMobilization_ == null)
			{
				ZoneMobilization = new ZoneMobilization();
			}
			ZoneMobilization.MergeFrom(other.ZoneMobilization);
		}
		if (other.sandworm_ != null)
		{
			if (sandworm_ == null)
			{
				Sandworm = new Sandworm();
			}
			Sandworm.MergeFrom(other.Sandworm);
		}
		if (other.mummyChangeInfo_ != null)
		{
			if (mummyChangeInfo_ == null)
			{
				MummyChangeInfo = new MummyChangeInfo();
			}
			MummyChangeInfo.MergeFrom(other.MummyChangeInfo);
		}
		if (other.DestroyEndTimeMs != 0L)
		{
			DestroyEndTimeMs = other.DestroyEndTimeMs;
		}
		if (other.meteoriteInfo_ != null)
		{
			if (meteoriteInfo_ == null)
			{
				MeteoriteInfo = new MeteoriteInfo();
			}
			MeteoriteInfo.MergeFrom(other.MeteoriteInfo);
		}
		if (other.commonMonsterSkillInfo_ != null)
		{
			if (commonMonsterSkillInfo_ == null)
			{
				CommonMonsterSkillInfo = new CommonMonsterSkillInfo();
			}
			CommonMonsterSkillInfo.MergeFrom(other.CommonMonsterSkillInfo);
		}
		if (other.challengeNewDonate_ != null)
		{
			if (challengeNewDonate_ == null)
			{
				ChallengeNewDonate = new MonsterChallengeNewDonate();
			}
			ChallengeNewDonate.MergeFrom(other.ChallengeNewDonate);
		}
		if (other.RefusePowerHelper)
		{
			RefusePowerHelper = other.RefusePowerHelper;
		}
		if (other.lightHouseInfo_ != null)
		{
			if (lightHouseInfo_ == null)
			{
				LightHouseInfo = new LightHouseInfo();
			}
			LightHouseInfo.MergeFrom(other.LightHouseInfo);
		}
		if (other.WolfDecrHp != 0)
		{
			WolfDecrHp = other.WolfDecrHp;
		}
		if (other.QuarantineRole != 0)
		{
			QuarantineRole = other.QuarantineRole;
		}
		if (other.QuarantineArbiter != 0)
		{
			QuarantineArbiter = other.QuarantineArbiter;
		}
		if (other.quarantineSkill_ != null)
		{
			if (quarantineSkill_ == null)
			{
				QuarantineSkill = new QuarantineSkillInfo();
			}
			QuarantineSkill.MergeFrom(other.QuarantineSkill);
		}
		if (other.QuarantineLeave != 0)
		{
			QuarantineLeave = other.QuarantineLeave;
		}
		if (other.CurrentMaxHp != 0)
		{
			CurrentMaxHp = other.CurrentMaxHp;
		}
		fireworks_.Add(other.fireworks_);
		fireWorksGiftList_.Add(other.fireWorksGiftList_);
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
				OwnerUid = input.ReadString();
				break;
			case 16u:
				Uuid = input.ReadInt64();
				break;
			case 24u:
				BuildId = input.ReadInt32();
				break;
			case 32u:
				Level = input.ReadInt32();
				break;
			case 40u:
				BuildState = input.ReadInt32();
				break;
			case 48u:
				QueueState = input.ReadInt32();
				break;
			case 58u:
				AllianceId = input.ReadString();
				break;
			case 64u:
				UpdateEndTime = input.ReadInt32();
				break;
			case 72u:
				UpdateStartTime = input.ReadInt32();
				break;
			case 80u:
				LastHpTime = input.ReadInt32();
				break;
			case 88u:
				ProtectEndTime = input.ReadInt32();
				break;
			case 96u:
				Inside = input.ReadInt32();
				break;
			case 104u:
				CurrentHp = input.ReadInt32();
				break;
			case 114u:
				Name = input.ReadString();
				break;
			case 122u:
				AlAbbr = input.ReadString();
				break;
			case 128u:
				LastCollectTime = input.ReadInt32();
				break;
			case 136u:
				UnavailableTime = input.ReadInt32();
				break;
			case 144u:
				MonthCardEndTime = input.ReadInt32();
				break;
			case 152u:
				QueueItemId = input.ReadInt32();
				break;
			case 160u:
				QueueStartTime = input.ReadInt32();
				break;
			case 168u:
				QueueUpdateTime = input.ReadInt32();
				break;
			case 176u:
				DestroyStartTime = input.ReadInt32();
				break;
			case 184u:
				AppearanceId = input.ReadInt32();
				break;
			case 192u:
				SpecialType = (SpecialType)input.ReadEnum();
				break;
			case 202u:
				PositionId = input.ReadString();
				break;
			case 210u:
				skins_.AddEntriesFrom(input, _repeated_skins_codec);
				break;
			case 218u:
				Country = input.ReadString();
				break;
			case 229u:
				RecoverSpeed = input.ReadFloat();
				break;
			case 232u:
				VirusLayer = input.ReadInt32();
				break;
			case 240u:
				VirusEndTime = input.ReadInt32();
				break;
			case 253u:
				FireSpeed = input.ReadFloat();
				break;
			case 258u:
				status_.AddEntriesFrom(input, _repeated_status_codec);
				break;
			case 266u:
				if (thermalConductor_ == null)
				{
					ThermalConductor = new ThermalConductor();
				}
				input.ReadMessage(ThermalConductor);
				break;
			case 274u:
				if (skillChantInfo_ == null)
				{
					SkillChantInfo = new SkillChantInfo();
				}
				input.ReadMessage(SkillChantInfo);
				break;
			case 282u:
				if (monsterInvasion_ == null)
				{
					MonsterInvasion = new MonsterInvasion();
				}
				input.ReadMessage(MonsterInvasion);
				break;
			case 288u:
				RefuseTreadVirus = input.ReadBool();
				break;
			case 296u:
				SeasonRole = input.ReadInt32();
				break;
			case 306u:
				if (zoneMobilization_ == null)
				{
					ZoneMobilization = new ZoneMobilization();
				}
				input.ReadMessage(ZoneMobilization);
				break;
			case 314u:
				if (sandworm_ == null)
				{
					Sandworm = new Sandworm();
				}
				input.ReadMessage(Sandworm);
				break;
			case 322u:
				if (mummyChangeInfo_ == null)
				{
					MummyChangeInfo = new MummyChangeInfo();
				}
				input.ReadMessage(MummyChangeInfo);
				break;
			case 328u:
				DestroyEndTimeMs = input.ReadInt64();
				break;
			case 338u:
				if (meteoriteInfo_ == null)
				{
					MeteoriteInfo = new MeteoriteInfo();
				}
				input.ReadMessage(MeteoriteInfo);
				break;
			case 346u:
				if (commonMonsterSkillInfo_ == null)
				{
					CommonMonsterSkillInfo = new CommonMonsterSkillInfo();
				}
				input.ReadMessage(CommonMonsterSkillInfo);
				break;
			case 354u:
				if (challengeNewDonate_ == null)
				{
					ChallengeNewDonate = new MonsterChallengeNewDonate();
				}
				input.ReadMessage(ChallengeNewDonate);
				break;
			case 360u:
				RefusePowerHelper = input.ReadBool();
				break;
			case 370u:
				if (lightHouseInfo_ == null)
				{
					LightHouseInfo = new LightHouseInfo();
				}
				input.ReadMessage(LightHouseInfo);
				break;
			case 376u:
				WolfDecrHp = input.ReadInt32();
				break;
			case 384u:
				QuarantineRole = input.ReadInt32();
				break;
			case 392u:
				QuarantineArbiter = input.ReadInt32();
				break;
			case 402u:
				if (quarantineSkill_ == null)
				{
					QuarantineSkill = new QuarantineSkillInfo();
				}
				input.ReadMessage(QuarantineSkill);
				break;
			case 408u:
				QuarantineLeave = input.ReadInt32();
				break;
			case 416u:
				CurrentMaxHp = input.ReadInt32();
				break;
			case 426u:
				fireworks_.AddEntriesFrom(input, _repeated_fireworks_codec);
				break;
			case 434u:
				fireWorksGiftList_.AddEntriesFrom(input, _repeated_fireWorksGiftList_codec);
				break;
			}
		}
	}
}
