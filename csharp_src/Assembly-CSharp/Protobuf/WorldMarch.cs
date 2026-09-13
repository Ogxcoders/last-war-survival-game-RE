using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldMarch : IMessage<WorldMarch>, IMessage, IEquatable<WorldMarch>, IDeepCloneable<WorldMarch>
{
	public enum ExtraOneofCase
	{
		None = 0,
		BankDeposit = 200
	}

	private static readonly MessageParser<WorldMarch> _parser = new MessageParser<WorldMarch>(() => new WorldMarch());

	private UnknownFieldSet _unknownFields;

	public const int OwnerNameFieldNumber = 1;

	private string ownerName_ = "";

	public const int OwnerFormationUuidFieldNumber = 2;

	private long ownerFormationUuid_;

	public const int WorldIdFieldNumber = 3;

	private int worldId_;

	public const int CollectSpeedFieldNumber = 4;

	private float collectSpeed_;

	public const int ArmyWeightFieldNumber = 5;

	private long armyWeight_;

	public const int AllianceUidFieldNumber = 6;

	private string allianceUid_ = "";

	public const int StatusFieldNumber = 7;

	private int status_;

	public const int PathFieldNumber = 8;

	private string path_ = "";

	public const int StartPosFieldNumber = 9;

	private int startPos_;

	public const int TargetPosFieldNumber = 10;

	private int targetPos_;

	public const int TargetFieldNumber = 11;

	private int target_;

	public const int TargetUuidFieldNumber = 12;

	private long targetUuid_;

	public const int StartTimeFieldNumber = 13;

	private long startTime_;

	public const int EndTimeFieldNumber = 14;

	private long endTime_;

	public const int TeamUuidFieldNumber = 15;

	private long teamUuid_;

	public const int TypeFieldNumber = 16;

	private int type_;

	public const int SpeedFieldNumber = 17;

	private float speed_;

	public const int InBattleFieldNumber = 18;

	private bool inBattle_;

	public const int IsBrokenFieldNumber = 19;

	private bool isBroken_;

	public const int DiffPointFieldNumber = 20;

	private string diffPoint_ = "";

	public const int IsAnonymityFieldNumber = 21;

	private bool isAnonymity_;

	public const int PvpNumFieldNumber = 22;

	private int pvpNum_;

	public const int PveNumFieldNumber = 23;

	private int pveNum_;

	public const int SquadNoFieldNumber = 24;

	private int squadNo_;

	public const int SrcActionFieldNumber = 25;

	private string srcAction_ = "";

	public const int SrcServerFieldNumber = 26;

	private int srcServer_;

	public const int ServerFieldNumber = 27;

	private int server_;

	public const int TargetServerFieldNumber = 28;

	private int targetServer_;

	public const int FightMonsterFieldNumber = 29;

	private bool fightMonster_;

	public const int MainPointIdFieldNumber = 30;

	private int mainPointId_;

	public const int BlackStartTimeFieldNumber = 31;

	private long blackStartTime_;

	public const int BlackEndTimeFieldNumber = 32;

	private long blackEndTime_;

	public const int BlackSpeedFieldNumber = 33;

	private float blackSpeed_;

	public const int AllianceAbbrFieldNumber = 34;

	private string allianceAbbr_ = "";

	public const int AllianceNameFieldNumber = 35;

	private string allianceName_ = "";

	public const int AllianceIconFieldNumber = 36;

	private string allianceIcon_ = "";

	public const int PicFieldNumber = 37;

	private string pic_ = "";

	public const int PicVerFieldNumber = 38;

	private int picVer_;

	public const int HeadFrameFieldNumber = 39;

	private int headFrame_;

	public const int HeadSkinIdFieldNumber = 40;

	private int headSkinId_;

	public const int HeadSkinETimeFieldNumber = 41;

	private long headSkinETime_;

	public const int ChatBubbleIdFieldNumber = 42;

	private int chatBubbleId_;

	public const int ChatBubbleETimeFieldNumber = 43;

	private long chatBubbleETime_;

	public const int WorldTypeFieldNumber = 44;

	private int worldType_;

	public const int PowerFieldNumber = 45;

	private long power_;

	public const int BaseVirusLayerFieldNumber = 46;

	private int baseVirusLayer_;

	public const int ExtraVirusLayerFieldNumber = 47;

	private int extraVirusLayer_;

	public const int ArmyCombatUnitsFieldNumber = 48;

	private static readonly FieldCodec<ArmyCombatUnit> _repeated_armyCombatUnits_codec = FieldCodec.ForMessage(386u, ArmyCombatUnit.Parser);

	private readonly RepeatedField<ArmyCombatUnit> armyCombatUnits_ = new RepeatedField<ArmyCombatUnit>();

	public const int MonsterIdFieldNumber = 49;

	private int monsterId_;

	public const int MonsterSpecialTypeFieldNumber = 52;

	private int monsterSpecialType_;

	public const int FixedSoldierTypeFieldNumber = 50;

	private int fixedSoldierType_;

	public const int MonsterInfoFieldNumber = 51;

	private MonsterInfo monsterInfo_;

	public const int HoldUuidFieldNumber = 53;

	private long holdUuid_;

	public const int OwnerServerFieldNumber = 54;

	private int ownerServer_;

	public const int MeteoriteInfoFieldNumber = 55;

	private MeteoriteInfo meteoriteInfo_;

	public const int OwnerLightUuidFieldNumber = 56;

	private long ownerLightUuid_;

	public const int CatchZombieNumFieldNumber = 57;

	private int catchZombieNum_;

	public const int BloodyQueenMonsterUuidFieldNumber = 58;

	private long bloodyQueenMonsterUuid_;

	public const int CityBattleS1RestBossUuidFieldNumber = 59;

	private long cityBattleS1RestBossUuid_;

	public const int PlunderResFieldNumber = 60;

	private string plunderRes_ = "";

	public const int PathStartServerIdFieldNumber = 61;

	private int pathStartServerId_;

	public const int OwnerCurServerIdFieldNumber = 62;

	private int ownerCurServerId_;

	public const int BankDepositFieldNumber = 200;

	public const int StationPointInfoFieldNumber = 63;

	private WorldPointInfo stationPointInfo_;

	private object extra_;

	private ExtraOneofCase extraCase_;

	[DebuggerNonUserCode]
	public static MessageParser<WorldMarch> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string OwnerName
	{
		get
		{
			return ownerName_;
		}
		set
		{
			ownerName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long OwnerFormationUuid
	{
		get
		{
			return ownerFormationUuid_;
		}
		set
		{
			ownerFormationUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WorldId
	{
		get
		{
			return worldId_;
		}
		set
		{
			worldId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float CollectSpeed
	{
		get
		{
			return collectSpeed_;
		}
		set
		{
			collectSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ArmyWeight
	{
		get
		{
			return armyWeight_;
		}
		set
		{
			armyWeight_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AllianceUid
	{
		get
		{
			return allianceUid_;
		}
		set
		{
			allianceUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Status
	{
		get
		{
			return status_;
		}
		set
		{
			status_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Path
	{
		get
		{
			return path_;
		}
		set
		{
			path_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int StartPos
	{
		get
		{
			return startPos_;
		}
		set
		{
			startPos_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TargetPos
	{
		get
		{
			return targetPos_;
		}
		set
		{
			targetPos_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Target
	{
		get
		{
			return target_;
		}
		set
		{
			target_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TargetUuid
	{
		get
		{
			return targetUuid_;
		}
		set
		{
			targetUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long EndTime
	{
		get
		{
			return endTime_;
		}
		set
		{
			endTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TeamUuid
	{
		get
		{
			return teamUuid_;
		}
		set
		{
			teamUuid_ = value;
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
	public float Speed
	{
		get
		{
			return speed_;
		}
		set
		{
			speed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool InBattle
	{
		get
		{
			return inBattle_;
		}
		set
		{
			inBattle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsBroken
	{
		get
		{
			return isBroken_;
		}
		set
		{
			isBroken_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string DiffPoint
	{
		get
		{
			return diffPoint_;
		}
		set
		{
			diffPoint_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool IsAnonymity
	{
		get
		{
			return isAnonymity_;
		}
		set
		{
			isAnonymity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PvpNum
	{
		get
		{
			return pvpNum_;
		}
		set
		{
			pvpNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PveNum
	{
		get
		{
			return pveNum_;
		}
		set
		{
			pveNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SquadNo
	{
		get
		{
			return squadNo_;
		}
		set
		{
			squadNo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string SrcAction
	{
		get
		{
			return srcAction_;
		}
		set
		{
			srcAction_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int SrcServer
	{
		get
		{
			return srcServer_;
		}
		set
		{
			srcServer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Server
	{
		get
		{
			return server_;
		}
		set
		{
			server_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TargetServer
	{
		get
		{
			return targetServer_;
		}
		set
		{
			targetServer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool FightMonster
	{
		get
		{
			return fightMonster_;
		}
		set
		{
			fightMonster_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MainPointId
	{
		get
		{
			return mainPointId_;
		}
		set
		{
			mainPointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BlackStartTime
	{
		get
		{
			return blackStartTime_;
		}
		set
		{
			blackStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BlackEndTime
	{
		get
		{
			return blackEndTime_;
		}
		set
		{
			blackEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float BlackSpeed
	{
		get
		{
			return blackSpeed_;
		}
		set
		{
			blackSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AllianceAbbr
	{
		get
		{
			return allianceAbbr_;
		}
		set
		{
			allianceAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AllianceName
	{
		get
		{
			return allianceName_;
		}
		set
		{
			allianceName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AllianceIcon
	{
		get
		{
			return allianceIcon_;
		}
		set
		{
			allianceIcon_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadFrame
	{
		get
		{
			return headFrame_;
		}
		set
		{
			headFrame_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadSkinId
	{
		get
		{
			return headSkinId_;
		}
		set
		{
			headSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeadSkinETime
	{
		get
		{
			return headSkinETime_;
		}
		set
		{
			headSkinETime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ChatBubbleId
	{
		get
		{
			return chatBubbleId_;
		}
		set
		{
			chatBubbleId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ChatBubbleETime
	{
		get
		{
			return chatBubbleETime_;
		}
		set
		{
			chatBubbleETime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WorldType
	{
		get
		{
			return worldType_;
		}
		set
		{
			worldType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Power
	{
		get
		{
			return power_;
		}
		set
		{
			power_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BaseVirusLayer
	{
		get
		{
			return baseVirusLayer_;
		}
		set
		{
			baseVirusLayer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ExtraVirusLayer
	{
		get
		{
			return extraVirusLayer_;
		}
		set
		{
			extraVirusLayer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ArmyCombatUnit> ArmyCombatUnits => armyCombatUnits_;

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
	public int MonsterSpecialType
	{
		get
		{
			return monsterSpecialType_;
		}
		set
		{
			monsterSpecialType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FixedSoldierType
	{
		get
		{
			return fixedSoldierType_;
		}
		set
		{
			fixedSoldierType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterInfo MonsterInfo
	{
		get
		{
			return monsterInfo_;
		}
		set
		{
			monsterInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HoldUuid
	{
		get
		{
			return holdUuid_;
		}
		set
		{
			holdUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OwnerServer
	{
		get
		{
			return ownerServer_;
		}
		set
		{
			ownerServer_ = value;
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
	public long OwnerLightUuid
	{
		get
		{
			return ownerLightUuid_;
		}
		set
		{
			ownerLightUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CatchZombieNum
	{
		get
		{
			return catchZombieNum_;
		}
		set
		{
			catchZombieNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BloodyQueenMonsterUuid
	{
		get
		{
			return bloodyQueenMonsterUuid_;
		}
		set
		{
			bloodyQueenMonsterUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CityBattleS1RestBossUuid
	{
		get
		{
			return cityBattleS1RestBossUuid_;
		}
		set
		{
			cityBattleS1RestBossUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string PlunderRes
	{
		get
		{
			return plunderRes_;
		}
		set
		{
			plunderRes_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PathStartServerId
	{
		get
		{
			return pathStartServerId_;
		}
		set
		{
			pathStartServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OwnerCurServerId
	{
		get
		{
			return ownerCurServerId_;
		}
		set
		{
			ownerCurServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BankDeposit BankDeposit
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.BankDeposit)
			{
				return null;
			}
			return (BankDeposit)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.BankDeposit : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldPointInfo StationPointInfo
	{
		get
		{
			return stationPointInfo_;
		}
		set
		{
			stationPointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ExtraOneofCase ExtraCase => extraCase_;

	[DebuggerNonUserCode]
	public WorldMarch()
	{
	}

	[DebuggerNonUserCode]
	public WorldMarch(WorldMarch other)
		: this()
	{
		ownerName_ = other.ownerName_;
		ownerFormationUuid_ = other.ownerFormationUuid_;
		worldId_ = other.worldId_;
		collectSpeed_ = other.collectSpeed_;
		armyWeight_ = other.armyWeight_;
		allianceUid_ = other.allianceUid_;
		status_ = other.status_;
		path_ = other.path_;
		startPos_ = other.startPos_;
		targetPos_ = other.targetPos_;
		target_ = other.target_;
		targetUuid_ = other.targetUuid_;
		startTime_ = other.startTime_;
		endTime_ = other.endTime_;
		teamUuid_ = other.teamUuid_;
		type_ = other.type_;
		speed_ = other.speed_;
		inBattle_ = other.inBattle_;
		isBroken_ = other.isBroken_;
		diffPoint_ = other.diffPoint_;
		isAnonymity_ = other.isAnonymity_;
		pvpNum_ = other.pvpNum_;
		pveNum_ = other.pveNum_;
		squadNo_ = other.squadNo_;
		srcAction_ = other.srcAction_;
		srcServer_ = other.srcServer_;
		server_ = other.server_;
		targetServer_ = other.targetServer_;
		fightMonster_ = other.fightMonster_;
		mainPointId_ = other.mainPointId_;
		blackStartTime_ = other.blackStartTime_;
		blackEndTime_ = other.blackEndTime_;
		blackSpeed_ = other.blackSpeed_;
		allianceAbbr_ = other.allianceAbbr_;
		allianceName_ = other.allianceName_;
		allianceIcon_ = other.allianceIcon_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		headFrame_ = other.headFrame_;
		headSkinId_ = other.headSkinId_;
		headSkinETime_ = other.headSkinETime_;
		chatBubbleId_ = other.chatBubbleId_;
		chatBubbleETime_ = other.chatBubbleETime_;
		worldType_ = other.worldType_;
		power_ = other.power_;
		baseVirusLayer_ = other.baseVirusLayer_;
		extraVirusLayer_ = other.extraVirusLayer_;
		armyCombatUnits_ = other.armyCombatUnits_.Clone();
		monsterId_ = other.monsterId_;
		monsterSpecialType_ = other.monsterSpecialType_;
		fixedSoldierType_ = other.fixedSoldierType_;
		monsterInfo_ = ((other.monsterInfo_ != null) ? other.monsterInfo_.Clone() : null);
		holdUuid_ = other.holdUuid_;
		ownerServer_ = other.ownerServer_;
		meteoriteInfo_ = ((other.meteoriteInfo_ != null) ? other.meteoriteInfo_.Clone() : null);
		ownerLightUuid_ = other.ownerLightUuid_;
		catchZombieNum_ = other.catchZombieNum_;
		bloodyQueenMonsterUuid_ = other.bloodyQueenMonsterUuid_;
		cityBattleS1RestBossUuid_ = other.cityBattleS1RestBossUuid_;
		plunderRes_ = other.plunderRes_;
		pathStartServerId_ = other.pathStartServerId_;
		ownerCurServerId_ = other.ownerCurServerId_;
		stationPointInfo_ = ((other.stationPointInfo_ != null) ? other.stationPointInfo_.Clone() : null);
		ExtraOneofCase extraCase = other.ExtraCase;
		if (extraCase == ExtraOneofCase.BankDeposit)
		{
			BankDeposit = other.BankDeposit.Clone();
		}
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldMarch Clone()
	{
		return new WorldMarch(this);
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
		return Equals(other as WorldMarch);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldMarch other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (OwnerName != other.OwnerName)
		{
			return false;
		}
		if (OwnerFormationUuid != other.OwnerFormationUuid)
		{
			return false;
		}
		if (WorldId != other.WorldId)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(CollectSpeed, other.CollectSpeed))
		{
			return false;
		}
		if (ArmyWeight != other.ArmyWeight)
		{
			return false;
		}
		if (AllianceUid != other.AllianceUid)
		{
			return false;
		}
		if (Status != other.Status)
		{
			return false;
		}
		if (Path != other.Path)
		{
			return false;
		}
		if (StartPos != other.StartPos)
		{
			return false;
		}
		if (TargetPos != other.TargetPos)
		{
			return false;
		}
		if (Target != other.Target)
		{
			return false;
		}
		if (TargetUuid != other.TargetUuid)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		if (TeamUuid != other.TeamUuid)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Speed, other.Speed))
		{
			return false;
		}
		if (InBattle != other.InBattle)
		{
			return false;
		}
		if (IsBroken != other.IsBroken)
		{
			return false;
		}
		if (DiffPoint != other.DiffPoint)
		{
			return false;
		}
		if (IsAnonymity != other.IsAnonymity)
		{
			return false;
		}
		if (PvpNum != other.PvpNum)
		{
			return false;
		}
		if (PveNum != other.PveNum)
		{
			return false;
		}
		if (SquadNo != other.SquadNo)
		{
			return false;
		}
		if (SrcAction != other.SrcAction)
		{
			return false;
		}
		if (SrcServer != other.SrcServer)
		{
			return false;
		}
		if (Server != other.Server)
		{
			return false;
		}
		if (TargetServer != other.TargetServer)
		{
			return false;
		}
		if (FightMonster != other.FightMonster)
		{
			return false;
		}
		if (MainPointId != other.MainPointId)
		{
			return false;
		}
		if (BlackStartTime != other.BlackStartTime)
		{
			return false;
		}
		if (BlackEndTime != other.BlackEndTime)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(BlackSpeed, other.BlackSpeed))
		{
			return false;
		}
		if (AllianceAbbr != other.AllianceAbbr)
		{
			return false;
		}
		if (AllianceName != other.AllianceName)
		{
			return false;
		}
		if (AllianceIcon != other.AllianceIcon)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (HeadFrame != other.HeadFrame)
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (HeadSkinETime != other.HeadSkinETime)
		{
			return false;
		}
		if (ChatBubbleId != other.ChatBubbleId)
		{
			return false;
		}
		if (ChatBubbleETime != other.ChatBubbleETime)
		{
			return false;
		}
		if (WorldType != other.WorldType)
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (BaseVirusLayer != other.BaseVirusLayer)
		{
			return false;
		}
		if (ExtraVirusLayer != other.ExtraVirusLayer)
		{
			return false;
		}
		if (!armyCombatUnits_.Equals(other.armyCombatUnits_))
		{
			return false;
		}
		if (MonsterId != other.MonsterId)
		{
			return false;
		}
		if (MonsterSpecialType != other.MonsterSpecialType)
		{
			return false;
		}
		if (FixedSoldierType != other.FixedSoldierType)
		{
			return false;
		}
		if (!object.Equals(MonsterInfo, other.MonsterInfo))
		{
			return false;
		}
		if (HoldUuid != other.HoldUuid)
		{
			return false;
		}
		if (OwnerServer != other.OwnerServer)
		{
			return false;
		}
		if (!object.Equals(MeteoriteInfo, other.MeteoriteInfo))
		{
			return false;
		}
		if (OwnerLightUuid != other.OwnerLightUuid)
		{
			return false;
		}
		if (CatchZombieNum != other.CatchZombieNum)
		{
			return false;
		}
		if (BloodyQueenMonsterUuid != other.BloodyQueenMonsterUuid)
		{
			return false;
		}
		if (CityBattleS1RestBossUuid != other.CityBattleS1RestBossUuid)
		{
			return false;
		}
		if (PlunderRes != other.PlunderRes)
		{
			return false;
		}
		if (PathStartServerId != other.PathStartServerId)
		{
			return false;
		}
		if (OwnerCurServerId != other.OwnerCurServerId)
		{
			return false;
		}
		if (!object.Equals(BankDeposit, other.BankDeposit))
		{
			return false;
		}
		if (!object.Equals(StationPointInfo, other.StationPointInfo))
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
		if (OwnerName.Length != 0)
		{
			num ^= OwnerName.GetHashCode();
		}
		if (OwnerFormationUuid != 0L)
		{
			num ^= OwnerFormationUuid.GetHashCode();
		}
		if (WorldId != 0)
		{
			num ^= WorldId.GetHashCode();
		}
		if (CollectSpeed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(CollectSpeed);
		}
		if (ArmyWeight != 0L)
		{
			num ^= ArmyWeight.GetHashCode();
		}
		if (AllianceUid.Length != 0)
		{
			num ^= AllianceUid.GetHashCode();
		}
		if (Status != 0)
		{
			num ^= Status.GetHashCode();
		}
		if (Path.Length != 0)
		{
			num ^= Path.GetHashCode();
		}
		if (StartPos != 0)
		{
			num ^= StartPos.GetHashCode();
		}
		if (TargetPos != 0)
		{
			num ^= TargetPos.GetHashCode();
		}
		if (Target != 0)
		{
			num ^= Target.GetHashCode();
		}
		if (TargetUuid != 0L)
		{
			num ^= TargetUuid.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
		}
		if (TeamUuid != 0L)
		{
			num ^= TeamUuid.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (Speed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Speed);
		}
		if (InBattle)
		{
			num ^= InBattle.GetHashCode();
		}
		if (IsBroken)
		{
			num ^= IsBroken.GetHashCode();
		}
		if (DiffPoint.Length != 0)
		{
			num ^= DiffPoint.GetHashCode();
		}
		if (IsAnonymity)
		{
			num ^= IsAnonymity.GetHashCode();
		}
		if (PvpNum != 0)
		{
			num ^= PvpNum.GetHashCode();
		}
		if (PveNum != 0)
		{
			num ^= PveNum.GetHashCode();
		}
		if (SquadNo != 0)
		{
			num ^= SquadNo.GetHashCode();
		}
		if (SrcAction.Length != 0)
		{
			num ^= SrcAction.GetHashCode();
		}
		if (SrcServer != 0)
		{
			num ^= SrcServer.GetHashCode();
		}
		if (Server != 0)
		{
			num ^= Server.GetHashCode();
		}
		if (TargetServer != 0)
		{
			num ^= TargetServer.GetHashCode();
		}
		if (FightMonster)
		{
			num ^= FightMonster.GetHashCode();
		}
		if (MainPointId != 0)
		{
			num ^= MainPointId.GetHashCode();
		}
		if (BlackStartTime != 0L)
		{
			num ^= BlackStartTime.GetHashCode();
		}
		if (BlackEndTime != 0L)
		{
			num ^= BlackEndTime.GetHashCode();
		}
		if (BlackSpeed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(BlackSpeed);
		}
		if (AllianceAbbr.Length != 0)
		{
			num ^= AllianceAbbr.GetHashCode();
		}
		if (AllianceName.Length != 0)
		{
			num ^= AllianceName.GetHashCode();
		}
		if (AllianceIcon.Length != 0)
		{
			num ^= AllianceIcon.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (HeadFrame != 0)
		{
			num ^= HeadFrame.GetHashCode();
		}
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (HeadSkinETime != 0L)
		{
			num ^= HeadSkinETime.GetHashCode();
		}
		if (ChatBubbleId != 0)
		{
			num ^= ChatBubbleId.GetHashCode();
		}
		if (ChatBubbleETime != 0L)
		{
			num ^= ChatBubbleETime.GetHashCode();
		}
		if (WorldType != 0)
		{
			num ^= WorldType.GetHashCode();
		}
		if (Power != 0L)
		{
			num ^= Power.GetHashCode();
		}
		if (BaseVirusLayer != 0)
		{
			num ^= BaseVirusLayer.GetHashCode();
		}
		if (ExtraVirusLayer != 0)
		{
			num ^= ExtraVirusLayer.GetHashCode();
		}
		num ^= armyCombatUnits_.GetHashCode();
		if (MonsterId != 0)
		{
			num ^= MonsterId.GetHashCode();
		}
		if (MonsterSpecialType != 0)
		{
			num ^= MonsterSpecialType.GetHashCode();
		}
		if (FixedSoldierType != 0)
		{
			num ^= FixedSoldierType.GetHashCode();
		}
		if (monsterInfo_ != null)
		{
			num ^= MonsterInfo.GetHashCode();
		}
		if (HoldUuid != 0L)
		{
			num ^= HoldUuid.GetHashCode();
		}
		if (OwnerServer != 0)
		{
			num ^= OwnerServer.GetHashCode();
		}
		if (meteoriteInfo_ != null)
		{
			num ^= MeteoriteInfo.GetHashCode();
		}
		if (OwnerLightUuid != 0L)
		{
			num ^= OwnerLightUuid.GetHashCode();
		}
		if (CatchZombieNum != 0)
		{
			num ^= CatchZombieNum.GetHashCode();
		}
		if (BloodyQueenMonsterUuid != 0L)
		{
			num ^= BloodyQueenMonsterUuid.GetHashCode();
		}
		if (CityBattleS1RestBossUuid != 0L)
		{
			num ^= CityBattleS1RestBossUuid.GetHashCode();
		}
		if (PlunderRes.Length != 0)
		{
			num ^= PlunderRes.GetHashCode();
		}
		if (PathStartServerId != 0)
		{
			num ^= PathStartServerId.GetHashCode();
		}
		if (OwnerCurServerId != 0)
		{
			num ^= OwnerCurServerId.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.BankDeposit)
		{
			num ^= BankDeposit.GetHashCode();
		}
		if (stationPointInfo_ != null)
		{
			num ^= StationPointInfo.GetHashCode();
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
		if (OwnerName.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(OwnerName);
		}
		if (OwnerFormationUuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(OwnerFormationUuid);
		}
		if (WorldId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(WorldId);
		}
		if (CollectSpeed != 0f)
		{
			output.WriteRawTag(37);
			output.WriteFloat(CollectSpeed);
		}
		if (ArmyWeight != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(ArmyWeight);
		}
		if (AllianceUid.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceUid);
		}
		if (Status != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Status);
		}
		if (Path.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(Path);
		}
		if (StartPos != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(StartPos);
		}
		if (TargetPos != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(TargetPos);
		}
		if (Target != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Target);
		}
		if (TargetUuid != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(TargetUuid);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(StartTime);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(EndTime);
		}
		if (TeamUuid != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(TeamUuid);
		}
		if (Type != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(Type);
		}
		if (Speed != 0f)
		{
			output.WriteRawTag(141, 1);
			output.WriteFloat(Speed);
		}
		if (InBattle)
		{
			output.WriteRawTag(144, 1);
			output.WriteBool(InBattle);
		}
		if (IsBroken)
		{
			output.WriteRawTag(152, 1);
			output.WriteBool(IsBroken);
		}
		if (DiffPoint.Length != 0)
		{
			output.WriteRawTag(162, 1);
			output.WriteString(DiffPoint);
		}
		if (IsAnonymity)
		{
			output.WriteRawTag(168, 1);
			output.WriteBool(IsAnonymity);
		}
		if (PvpNum != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(PvpNum);
		}
		if (PveNum != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(PveNum);
		}
		if (SquadNo != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(SquadNo);
		}
		if (SrcAction.Length != 0)
		{
			output.WriteRawTag(202, 1);
			output.WriteString(SrcAction);
		}
		if (SrcServer != 0)
		{
			output.WriteRawTag(208, 1);
			output.WriteInt32(SrcServer);
		}
		if (Server != 0)
		{
			output.WriteRawTag(216, 1);
			output.WriteInt32(Server);
		}
		if (TargetServer != 0)
		{
			output.WriteRawTag(224, 1);
			output.WriteInt32(TargetServer);
		}
		if (FightMonster)
		{
			output.WriteRawTag(232, 1);
			output.WriteBool(FightMonster);
		}
		if (MainPointId != 0)
		{
			output.WriteRawTag(240, 1);
			output.WriteInt32(MainPointId);
		}
		if (BlackStartTime != 0L)
		{
			output.WriteRawTag(248, 1);
			output.WriteInt64(BlackStartTime);
		}
		if (BlackEndTime != 0L)
		{
			output.WriteRawTag(128, 2);
			output.WriteInt64(BlackEndTime);
		}
		if (BlackSpeed != 0f)
		{
			output.WriteRawTag(141, 2);
			output.WriteFloat(BlackSpeed);
		}
		if (AllianceAbbr.Length != 0)
		{
			output.WriteRawTag(146, 2);
			output.WriteString(AllianceAbbr);
		}
		if (AllianceName.Length != 0)
		{
			output.WriteRawTag(154, 2);
			output.WriteString(AllianceName);
		}
		if (AllianceIcon.Length != 0)
		{
			output.WriteRawTag(162, 2);
			output.WriteString(AllianceIcon);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(170, 2);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(176, 2);
			output.WriteInt32(PicVer);
		}
		if (HeadFrame != 0)
		{
			output.WriteRawTag(184, 2);
			output.WriteInt32(HeadFrame);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(192, 2);
			output.WriteInt32(HeadSkinId);
		}
		if (HeadSkinETime != 0L)
		{
			output.WriteRawTag(200, 2);
			output.WriteInt64(HeadSkinETime);
		}
		if (ChatBubbleId != 0)
		{
			output.WriteRawTag(208, 2);
			output.WriteInt32(ChatBubbleId);
		}
		if (ChatBubbleETime != 0L)
		{
			output.WriteRawTag(216, 2);
			output.WriteInt64(ChatBubbleETime);
		}
		if (WorldType != 0)
		{
			output.WriteRawTag(224, 2);
			output.WriteInt32(WorldType);
		}
		if (Power != 0L)
		{
			output.WriteRawTag(232, 2);
			output.WriteInt64(Power);
		}
		if (BaseVirusLayer != 0)
		{
			output.WriteRawTag(240, 2);
			output.WriteInt32(BaseVirusLayer);
		}
		if (ExtraVirusLayer != 0)
		{
			output.WriteRawTag(248, 2);
			output.WriteInt32(ExtraVirusLayer);
		}
		armyCombatUnits_.WriteTo(output, _repeated_armyCombatUnits_codec);
		if (MonsterId != 0)
		{
			output.WriteRawTag(136, 3);
			output.WriteInt32(MonsterId);
		}
		if (FixedSoldierType != 0)
		{
			output.WriteRawTag(144, 3);
			output.WriteInt32(FixedSoldierType);
		}
		if (monsterInfo_ != null)
		{
			output.WriteRawTag(154, 3);
			output.WriteMessage(MonsterInfo);
		}
		if (MonsterSpecialType != 0)
		{
			output.WriteRawTag(160, 3);
			output.WriteInt32(MonsterSpecialType);
		}
		if (HoldUuid != 0L)
		{
			output.WriteRawTag(168, 3);
			output.WriteInt64(HoldUuid);
		}
		if (OwnerServer != 0)
		{
			output.WriteRawTag(176, 3);
			output.WriteInt32(OwnerServer);
		}
		if (meteoriteInfo_ != null)
		{
			output.WriteRawTag(186, 3);
			output.WriteMessage(MeteoriteInfo);
		}
		if (OwnerLightUuid != 0L)
		{
			output.WriteRawTag(192, 3);
			output.WriteInt64(OwnerLightUuid);
		}
		if (CatchZombieNum != 0)
		{
			output.WriteRawTag(200, 3);
			output.WriteInt32(CatchZombieNum);
		}
		if (BloodyQueenMonsterUuid != 0L)
		{
			output.WriteRawTag(208, 3);
			output.WriteInt64(BloodyQueenMonsterUuid);
		}
		if (CityBattleS1RestBossUuid != 0L)
		{
			output.WriteRawTag(216, 3);
			output.WriteInt64(CityBattleS1RestBossUuid);
		}
		if (PlunderRes.Length != 0)
		{
			output.WriteRawTag(226, 3);
			output.WriteString(PlunderRes);
		}
		if (PathStartServerId != 0)
		{
			output.WriteRawTag(232, 3);
			output.WriteInt32(PathStartServerId);
		}
		if (OwnerCurServerId != 0)
		{
			output.WriteRawTag(240, 3);
			output.WriteInt32(OwnerCurServerId);
		}
		if (stationPointInfo_ != null)
		{
			output.WriteRawTag(250, 3);
			output.WriteMessage(StationPointInfo);
		}
		if (extraCase_ == ExtraOneofCase.BankDeposit)
		{
			output.WriteRawTag(194, 12);
			output.WriteMessage(BankDeposit);
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
		if (OwnerName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerName);
		}
		if (OwnerFormationUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OwnerFormationUuid);
		}
		if (WorldId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WorldId);
		}
		if (CollectSpeed != 0f)
		{
			num += 5;
		}
		if (ArmyWeight != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ArmyWeight);
		}
		if (AllianceUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceUid);
		}
		if (Status != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Status);
		}
		if (Path.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Path);
		}
		if (StartPos != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StartPos);
		}
		if (TargetPos != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TargetPos);
		}
		if (Target != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Target);
		}
		if (TargetUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TargetUuid);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (TeamUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TeamUuid);
		}
		if (Type != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (Speed != 0f)
		{
			num += 6;
		}
		if (InBattle)
		{
			num += 3;
		}
		if (IsBroken)
		{
			num += 3;
		}
		if (DiffPoint.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(DiffPoint);
		}
		if (IsAnonymity)
		{
			num += 3;
		}
		if (PvpNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PvpNum);
		}
		if (PveNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PveNum);
		}
		if (SquadNo != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SquadNo);
		}
		if (SrcAction.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(SrcAction);
		}
		if (SrcServer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SrcServer);
		}
		if (Server != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Server);
		}
		if (TargetServer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TargetServer);
		}
		if (FightMonster)
		{
			num += 3;
		}
		if (MainPointId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MainPointId);
		}
		if (BlackStartTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(BlackStartTime);
		}
		if (BlackEndTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(BlackEndTime);
		}
		if (BlackSpeed != 0f)
		{
			num += 6;
		}
		if (AllianceAbbr.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(AllianceAbbr);
		}
		if (AllianceName.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(AllianceName);
		}
		if (AllianceIcon.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(AllianceIcon);
		}
		if (Pic.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (HeadFrame != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(HeadFrame);
		}
		if (HeadSkinId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (HeadSkinETime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(HeadSkinETime);
		}
		if (ChatBubbleId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ChatBubbleId);
		}
		if (ChatBubbleETime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(ChatBubbleETime);
		}
		if (WorldType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WorldType);
		}
		if (Power != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(Power);
		}
		if (BaseVirusLayer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(BaseVirusLayer);
		}
		if (ExtraVirusLayer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ExtraVirusLayer);
		}
		num += armyCombatUnits_.CalculateSize(_repeated_armyCombatUnits_codec);
		if (MonsterId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MonsterId);
		}
		if (MonsterSpecialType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MonsterSpecialType);
		}
		if (FixedSoldierType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(FixedSoldierType);
		}
		if (monsterInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MonsterInfo);
		}
		if (HoldUuid != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(HoldUuid);
		}
		if (OwnerServer != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(OwnerServer);
		}
		if (meteoriteInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MeteoriteInfo);
		}
		if (OwnerLightUuid != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(OwnerLightUuid);
		}
		if (CatchZombieNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CatchZombieNum);
		}
		if (BloodyQueenMonsterUuid != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(BloodyQueenMonsterUuid);
		}
		if (CityBattleS1RestBossUuid != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(CityBattleS1RestBossUuid);
		}
		if (PlunderRes.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(PlunderRes);
		}
		if (PathStartServerId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PathStartServerId);
		}
		if (OwnerCurServerId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(OwnerCurServerId);
		}
		if (extraCase_ == ExtraOneofCase.BankDeposit)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(BankDeposit);
		}
		if (stationPointInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(StationPointInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldMarch other)
	{
		if (other == null)
		{
			return;
		}
		if (other.OwnerName.Length != 0)
		{
			OwnerName = other.OwnerName;
		}
		if (other.OwnerFormationUuid != 0L)
		{
			OwnerFormationUuid = other.OwnerFormationUuid;
		}
		if (other.WorldId != 0)
		{
			WorldId = other.WorldId;
		}
		if (other.CollectSpeed != 0f)
		{
			CollectSpeed = other.CollectSpeed;
		}
		if (other.ArmyWeight != 0L)
		{
			ArmyWeight = other.ArmyWeight;
		}
		if (other.AllianceUid.Length != 0)
		{
			AllianceUid = other.AllianceUid;
		}
		if (other.Status != 0)
		{
			Status = other.Status;
		}
		if (other.Path.Length != 0)
		{
			Path = other.Path;
		}
		if (other.StartPos != 0)
		{
			StartPos = other.StartPos;
		}
		if (other.TargetPos != 0)
		{
			TargetPos = other.TargetPos;
		}
		if (other.Target != 0)
		{
			Target = other.Target;
		}
		if (other.TargetUuid != 0L)
		{
			TargetUuid = other.TargetUuid;
		}
		if (other.StartTime != 0L)
		{
			StartTime = other.StartTime;
		}
		if (other.EndTime != 0L)
		{
			EndTime = other.EndTime;
		}
		if (other.TeamUuid != 0L)
		{
			TeamUuid = other.TeamUuid;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.Speed != 0f)
		{
			Speed = other.Speed;
		}
		if (other.InBattle)
		{
			InBattle = other.InBattle;
		}
		if (other.IsBroken)
		{
			IsBroken = other.IsBroken;
		}
		if (other.DiffPoint.Length != 0)
		{
			DiffPoint = other.DiffPoint;
		}
		if (other.IsAnonymity)
		{
			IsAnonymity = other.IsAnonymity;
		}
		if (other.PvpNum != 0)
		{
			PvpNum = other.PvpNum;
		}
		if (other.PveNum != 0)
		{
			PveNum = other.PveNum;
		}
		if (other.SquadNo != 0)
		{
			SquadNo = other.SquadNo;
		}
		if (other.SrcAction.Length != 0)
		{
			SrcAction = other.SrcAction;
		}
		if (other.SrcServer != 0)
		{
			SrcServer = other.SrcServer;
		}
		if (other.Server != 0)
		{
			Server = other.Server;
		}
		if (other.TargetServer != 0)
		{
			TargetServer = other.TargetServer;
		}
		if (other.FightMonster)
		{
			FightMonster = other.FightMonster;
		}
		if (other.MainPointId != 0)
		{
			MainPointId = other.MainPointId;
		}
		if (other.BlackStartTime != 0L)
		{
			BlackStartTime = other.BlackStartTime;
		}
		if (other.BlackEndTime != 0L)
		{
			BlackEndTime = other.BlackEndTime;
		}
		if (other.BlackSpeed != 0f)
		{
			BlackSpeed = other.BlackSpeed;
		}
		if (other.AllianceAbbr.Length != 0)
		{
			AllianceAbbr = other.AllianceAbbr;
		}
		if (other.AllianceName.Length != 0)
		{
			AllianceName = other.AllianceName;
		}
		if (other.AllianceIcon.Length != 0)
		{
			AllianceIcon = other.AllianceIcon;
		}
		if (other.Pic.Length != 0)
		{
			Pic = other.Pic;
		}
		if (other.PicVer != 0)
		{
			PicVer = other.PicVer;
		}
		if (other.HeadFrame != 0)
		{
			HeadFrame = other.HeadFrame;
		}
		if (other.HeadSkinId != 0)
		{
			HeadSkinId = other.HeadSkinId;
		}
		if (other.HeadSkinETime != 0L)
		{
			HeadSkinETime = other.HeadSkinETime;
		}
		if (other.ChatBubbleId != 0)
		{
			ChatBubbleId = other.ChatBubbleId;
		}
		if (other.ChatBubbleETime != 0L)
		{
			ChatBubbleETime = other.ChatBubbleETime;
		}
		if (other.WorldType != 0)
		{
			WorldType = other.WorldType;
		}
		if (other.Power != 0L)
		{
			Power = other.Power;
		}
		if (other.BaseVirusLayer != 0)
		{
			BaseVirusLayer = other.BaseVirusLayer;
		}
		if (other.ExtraVirusLayer != 0)
		{
			ExtraVirusLayer = other.ExtraVirusLayer;
		}
		armyCombatUnits_.Add(other.armyCombatUnits_);
		if (other.MonsterId != 0)
		{
			MonsterId = other.MonsterId;
		}
		if (other.MonsterSpecialType != 0)
		{
			MonsterSpecialType = other.MonsterSpecialType;
		}
		if (other.FixedSoldierType != 0)
		{
			FixedSoldierType = other.FixedSoldierType;
		}
		if (other.monsterInfo_ != null)
		{
			if (monsterInfo_ == null)
			{
				MonsterInfo = new MonsterInfo();
			}
			MonsterInfo.MergeFrom(other.MonsterInfo);
		}
		if (other.HoldUuid != 0L)
		{
			HoldUuid = other.HoldUuid;
		}
		if (other.OwnerServer != 0)
		{
			OwnerServer = other.OwnerServer;
		}
		if (other.meteoriteInfo_ != null)
		{
			if (meteoriteInfo_ == null)
			{
				MeteoriteInfo = new MeteoriteInfo();
			}
			MeteoriteInfo.MergeFrom(other.MeteoriteInfo);
		}
		if (other.OwnerLightUuid != 0L)
		{
			OwnerLightUuid = other.OwnerLightUuid;
		}
		if (other.CatchZombieNum != 0)
		{
			CatchZombieNum = other.CatchZombieNum;
		}
		if (other.BloodyQueenMonsterUuid != 0L)
		{
			BloodyQueenMonsterUuid = other.BloodyQueenMonsterUuid;
		}
		if (other.CityBattleS1RestBossUuid != 0L)
		{
			CityBattleS1RestBossUuid = other.CityBattleS1RestBossUuid;
		}
		if (other.PlunderRes.Length != 0)
		{
			PlunderRes = other.PlunderRes;
		}
		if (other.PathStartServerId != 0)
		{
			PathStartServerId = other.PathStartServerId;
		}
		if (other.OwnerCurServerId != 0)
		{
			OwnerCurServerId = other.OwnerCurServerId;
		}
		if (other.stationPointInfo_ != null)
		{
			if (stationPointInfo_ == null)
			{
				StationPointInfo = new WorldPointInfo();
			}
			StationPointInfo.MergeFrom(other.StationPointInfo);
		}
		ExtraOneofCase extraCase = other.ExtraCase;
		if (extraCase == ExtraOneofCase.BankDeposit)
		{
			if (BankDeposit == null)
			{
				BankDeposit = new BankDeposit();
			}
			BankDeposit.MergeFrom(other.BankDeposit);
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
				OwnerName = input.ReadString();
				break;
			case 16u:
				OwnerFormationUuid = input.ReadInt64();
				break;
			case 24u:
				WorldId = input.ReadInt32();
				break;
			case 37u:
				CollectSpeed = input.ReadFloat();
				break;
			case 40u:
				ArmyWeight = input.ReadInt64();
				break;
			case 50u:
				AllianceUid = input.ReadString();
				break;
			case 56u:
				Status = input.ReadInt32();
				break;
			case 66u:
				Path = input.ReadString();
				break;
			case 72u:
				StartPos = input.ReadInt32();
				break;
			case 80u:
				TargetPos = input.ReadInt32();
				break;
			case 88u:
				Target = input.ReadInt32();
				break;
			case 96u:
				TargetUuid = input.ReadInt64();
				break;
			case 104u:
				StartTime = input.ReadInt64();
				break;
			case 112u:
				EndTime = input.ReadInt64();
				break;
			case 120u:
				TeamUuid = input.ReadInt64();
				break;
			case 128u:
				Type = input.ReadInt32();
				break;
			case 141u:
				Speed = input.ReadFloat();
				break;
			case 144u:
				InBattle = input.ReadBool();
				break;
			case 152u:
				IsBroken = input.ReadBool();
				break;
			case 162u:
				DiffPoint = input.ReadString();
				break;
			case 168u:
				IsAnonymity = input.ReadBool();
				break;
			case 176u:
				PvpNum = input.ReadInt32();
				break;
			case 184u:
				PveNum = input.ReadInt32();
				break;
			case 192u:
				SquadNo = input.ReadInt32();
				break;
			case 202u:
				SrcAction = input.ReadString();
				break;
			case 208u:
				SrcServer = input.ReadInt32();
				break;
			case 216u:
				Server = input.ReadInt32();
				break;
			case 224u:
				TargetServer = input.ReadInt32();
				break;
			case 232u:
				FightMonster = input.ReadBool();
				break;
			case 240u:
				MainPointId = input.ReadInt32();
				break;
			case 248u:
				BlackStartTime = input.ReadInt64();
				break;
			case 256u:
				BlackEndTime = input.ReadInt64();
				break;
			case 269u:
				BlackSpeed = input.ReadFloat();
				break;
			case 274u:
				AllianceAbbr = input.ReadString();
				break;
			case 282u:
				AllianceName = input.ReadString();
				break;
			case 290u:
				AllianceIcon = input.ReadString();
				break;
			case 298u:
				Pic = input.ReadString();
				break;
			case 304u:
				PicVer = input.ReadInt32();
				break;
			case 312u:
				HeadFrame = input.ReadInt32();
				break;
			case 320u:
				HeadSkinId = input.ReadInt32();
				break;
			case 328u:
				HeadSkinETime = input.ReadInt64();
				break;
			case 336u:
				ChatBubbleId = input.ReadInt32();
				break;
			case 344u:
				ChatBubbleETime = input.ReadInt64();
				break;
			case 352u:
				WorldType = input.ReadInt32();
				break;
			case 360u:
				Power = input.ReadInt64();
				break;
			case 368u:
				BaseVirusLayer = input.ReadInt32();
				break;
			case 376u:
				ExtraVirusLayer = input.ReadInt32();
				break;
			case 386u:
				armyCombatUnits_.AddEntriesFrom(input, _repeated_armyCombatUnits_codec);
				break;
			case 392u:
				MonsterId = input.ReadInt32();
				break;
			case 400u:
				FixedSoldierType = input.ReadInt32();
				break;
			case 410u:
				if (monsterInfo_ == null)
				{
					MonsterInfo = new MonsterInfo();
				}
				input.ReadMessage(MonsterInfo);
				break;
			case 416u:
				MonsterSpecialType = input.ReadInt32();
				break;
			case 424u:
				HoldUuid = input.ReadInt64();
				break;
			case 432u:
				OwnerServer = input.ReadInt32();
				break;
			case 442u:
				if (meteoriteInfo_ == null)
				{
					MeteoriteInfo = new MeteoriteInfo();
				}
				input.ReadMessage(MeteoriteInfo);
				break;
			case 448u:
				OwnerLightUuid = input.ReadInt64();
				break;
			case 456u:
				CatchZombieNum = input.ReadInt32();
				break;
			case 464u:
				BloodyQueenMonsterUuid = input.ReadInt64();
				break;
			case 472u:
				CityBattleS1RestBossUuid = input.ReadInt64();
				break;
			case 482u:
				PlunderRes = input.ReadString();
				break;
			case 488u:
				PathStartServerId = input.ReadInt32();
				break;
			case 496u:
				OwnerCurServerId = input.ReadInt32();
				break;
			case 506u:
				if (stationPointInfo_ == null)
				{
					StationPointInfo = new WorldPointInfo();
				}
				input.ReadMessage(StationPointInfo);
				break;
			case 1602u:
			{
				BankDeposit bankDeposit = new BankDeposit();
				if (extraCase_ == ExtraOneofCase.BankDeposit)
				{
					bankDeposit.MergeFrom(BankDeposit);
				}
				input.ReadMessage(bankDeposit);
				BankDeposit = bankDeposit;
				break;
			}
			}
		}
	}
}
