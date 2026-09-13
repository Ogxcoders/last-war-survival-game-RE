using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattleReport : IMessage<LwBattleReport>, IMessage, IEquatable<LwBattleReport>, IDeepCloneable<LwBattleReport>
{
	private static readonly MessageParser<LwBattleReport> _parser = new MessageParser<LwBattleReport>(() => new LwBattleReport());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int TypeFieldNumber = 2;

	private int type_;

	public const int BattleTimeFieldNumber = 3;

	private long battleTime_;

	public const int BattlePointInfoFieldNumber = 4;

	private BattlePointInfo battlePointInfo_;

	public const int FightResultFieldNumber = 5;

	private int fightResult_;

	public const int PlayerFieldNumber = 6;

	private static readonly FieldCodec<LwBattlePlayerStat> _repeated_player_codec = FieldCodec.ForMessage(50u, LwBattlePlayerStat.Parser);

	private readonly RepeatedField<LwBattlePlayerStat> player_ = new RepeatedField<LwBattlePlayerStat>();

	public const int UnitsFieldNumber = 8;

	private static readonly FieldCodec<LwBattleUnit> _repeated_units_codec = FieldCodec.ForMessage(66u, LwBattleUnit.Parser);

	private readonly RepeatedField<LwBattleUnit> units_ = new RepeatedField<LwBattleUnit>();

	public const int RewardFieldNumber = 10;

	private Reward reward_;

	public const int DetailFieldNumber = 12;

	private LwBattleDetail detail_;

	public const int WinKillSoldierFieldNumber = 13;

	private long winKillSoldier_;

	public const int PlunderValueFieldNumber = 14;

	private long plunderValue_;

	public const int MaxPlunderValueFieldNumber = 15;

	private long maxPlunderValue_;

	public const int TotalTimeFieldNumber = 16;

	private int totalTime_;

	public const int OverTimeFieldNumber = 17;

	private int overTime_;

	public const int AddressFieldNumber = 19;

	private string address_ = "";

	public const int VersionFieldNumber = 20;

	private int version_;

	public const int CombinePlayerFieldNumber = 21;

	private static readonly FieldCodec<LwBattlePlayerCombineStat> _repeated_combinePlayer_codec = FieldCodec.ForMessage(170u, LwBattlePlayerCombineStat.Parser);

	private readonly RepeatedField<LwBattlePlayerCombineStat> combinePlayer_ = new RepeatedField<LwBattlePlayerCombineStat>();

	public const int RoundFieldNumber = 22;

	private static readonly FieldCodec<LwBattleCombineRound> _repeated_round_codec = FieldCodec.ForMessage(178u, LwBattleCombineRound.Parser);

	private readonly RepeatedField<LwBattleCombineRound> round_ = new RepeatedField<LwBattleCombineRound>();

	public const int IsCombineReportFieldNumber = 23;

	private int isCombineReport_;

	public const int IsAllKillFieldNumber = 24;

	private int isAllKill_;

	public const int AllianceCityInfoFieldNumber = 30;

	private LwAllianceCityInfo allianceCityInfo_;

	public const int WinKillSoldierDetailFieldNumber = 31;

	private static readonly FieldCodec<LwSoldierLost> _repeated_winKillSoldierDetail_codec = FieldCodec.ForMessage(250u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> winKillSoldierDetail_ = new RepeatedField<LwSoldierLost>();

	public const int HospitalFullDeadDetailFieldNumber = 32;

	private static readonly FieldCodec<LwSoldierLost> _repeated_hospitalFullDeadDetail_codec = FieldCodec.ForMessage(258u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> hospitalFullDeadDetail_ = new RepeatedField<LwSoldierLost>();

	public const int RicochetDetailFieldNumber = 33;

	private static readonly FieldCodec<LwSoldierLost> _repeated_ricochetDetail_codec = FieldCodec.ForMessage(266u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> ricochetDetail_ = new RepeatedField<LwSoldierLost>();

	public const int MonsterInvasionInfoFieldNumber = 34;

	private LwMonsterInvasionInfo monsterInvasionInfo_;

	public const int AtkMummyBlowUpResultDetailFieldNumber = 35;

	private static readonly FieldCodec<LwSoldierLost> _repeated_atkMummyBlowUpResultDetail_codec = FieldCodec.ForMessage(282u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> atkMummyBlowUpResultDetail_ = new RepeatedField<LwSoldierLost>();

	public const int DefMummyBlowUpResultDetailFieldNumber = 36;

	private static readonly FieldCodec<LwSoldierLost> _repeated_defMummyBlowUpResultDetail_codec = FieldCodec.ForMessage(290u, LwSoldierLost.Parser);

	private readonly RepeatedField<LwSoldierLost> defMummyBlowUpResultDetail_ = new RepeatedField<LwSoldierLost>();

	public const int PlunderMeteoriteInfoFieldNumber = 37;

	private MeteoriteInfo plunderMeteoriteInfo_;

	public const int AllianceBossSandFieldNumber = 38;

	private static readonly FieldCodec<LwAllianceBossSand> _repeated_allianceBossSand_codec = FieldCodec.ForMessage(306u, LwAllianceBossSand.Parser);

	private readonly RepeatedField<LwAllianceBossSand> allianceBossSand_ = new RepeatedField<LwAllianceBossSand>();

	public const int MonsterBuffListFieldNumber = 39;

	private static readonly FieldCodec<int> _repeated_monsterBuffList_codec = FieldCodec.ForInt32(314u);

	private readonly RepeatedField<int> monsterBuffList_ = new RepeatedField<int>();

	public const int ChampionDuelScoreFieldNumber = 40;

	private static readonly FieldCodec<LwBattleChampionDuelScore> _repeated_championDuelScore_codec = FieldCodec.ForMessage(322u, LwBattleChampionDuelScore.Parser);

	private readonly RepeatedField<LwBattleChampionDuelScore> championDuelScore_ = new RepeatedField<LwBattleChampionDuelScore>();

	public const int MarchTargetTypeFieldNumber = 41;

	private int marchTargetType_;

	public const int ParallelExecuteProcessFieldNumber = 42;

	private int parallelExecuteProcess_;

	[DebuggerNonUserCode]
	public static MessageParser<LwBattleReport> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[15];

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
	public long BattleTime
	{
		get
		{
			return battleTime_;
		}
		set
		{
			battleTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BattlePointInfo BattlePointInfo
	{
		get
		{
			return battlePointInfo_;
		}
		set
		{
			battlePointInfo_ = value;
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
	public RepeatedField<LwBattlePlayerStat> Player => player_;

	[DebuggerNonUserCode]
	public RepeatedField<LwBattleUnit> Units => units_;

	[DebuggerNonUserCode]
	public Reward Reward
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
	public LwBattleDetail Detail
	{
		get
		{
			return detail_;
		}
		set
		{
			detail_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long WinKillSoldier
	{
		get
		{
			return winKillSoldier_;
		}
		set
		{
			winKillSoldier_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PlunderValue
	{
		get
		{
			return plunderValue_;
		}
		set
		{
			plunderValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxPlunderValue
	{
		get
		{
			return maxPlunderValue_;
		}
		set
		{
			maxPlunderValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TotalTime
	{
		get
		{
			return totalTime_;
		}
		set
		{
			totalTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OverTime
	{
		get
		{
			return overTime_;
		}
		set
		{
			overTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Address
	{
		get
		{
			return address_;
		}
		set
		{
			address_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Version
	{
		get
		{
			return version_;
		}
		set
		{
			version_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwBattlePlayerCombineStat> CombinePlayer => combinePlayer_;

	[DebuggerNonUserCode]
	public RepeatedField<LwBattleCombineRound> Round => round_;

	[DebuggerNonUserCode]
	public int IsCombineReport
	{
		get
		{
			return isCombineReport_;
		}
		set
		{
			isCombineReport_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsAllKill
	{
		get
		{
			return isAllKill_;
		}
		set
		{
			isAllKill_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwAllianceCityInfo AllianceCityInfo
	{
		get
		{
			return allianceCityInfo_;
		}
		set
		{
			allianceCityInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> WinKillSoldierDetail => winKillSoldierDetail_;

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> HospitalFullDeadDetail => hospitalFullDeadDetail_;

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> RicochetDetail => ricochetDetail_;

	[DebuggerNonUserCode]
	public LwMonsterInvasionInfo MonsterInvasionInfo
	{
		get
		{
			return monsterInvasionInfo_;
		}
		set
		{
			monsterInvasionInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> AtkMummyBlowUpResultDetail => atkMummyBlowUpResultDetail_;

	[DebuggerNonUserCode]
	public RepeatedField<LwSoldierLost> DefMummyBlowUpResultDetail => defMummyBlowUpResultDetail_;

	[DebuggerNonUserCode]
	public MeteoriteInfo PlunderMeteoriteInfo
	{
		get
		{
			return plunderMeteoriteInfo_;
		}
		set
		{
			plunderMeteoriteInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<LwAllianceBossSand> AllianceBossSand => allianceBossSand_;

	[DebuggerNonUserCode]
	public RepeatedField<int> MonsterBuffList => monsterBuffList_;

	[DebuggerNonUserCode]
	public RepeatedField<LwBattleChampionDuelScore> ChampionDuelScore => championDuelScore_;

	[DebuggerNonUserCode]
	public int MarchTargetType
	{
		get
		{
			return marchTargetType_;
		}
		set
		{
			marchTargetType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ParallelExecuteProcess
	{
		get
		{
			return parallelExecuteProcess_;
		}
		set
		{
			parallelExecuteProcess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwBattleReport()
	{
	}

	[DebuggerNonUserCode]
	public LwBattleReport(LwBattleReport other)
		: this()
	{
		uuid_ = other.uuid_;
		type_ = other.type_;
		battleTime_ = other.battleTime_;
		battlePointInfo_ = ((other.battlePointInfo_ != null) ? other.battlePointInfo_.Clone() : null);
		fightResult_ = other.fightResult_;
		player_ = other.player_.Clone();
		units_ = other.units_.Clone();
		reward_ = ((other.reward_ != null) ? other.reward_.Clone() : null);
		detail_ = ((other.detail_ != null) ? other.detail_.Clone() : null);
		winKillSoldier_ = other.winKillSoldier_;
		plunderValue_ = other.plunderValue_;
		maxPlunderValue_ = other.maxPlunderValue_;
		totalTime_ = other.totalTime_;
		overTime_ = other.overTime_;
		address_ = other.address_;
		version_ = other.version_;
		combinePlayer_ = other.combinePlayer_.Clone();
		round_ = other.round_.Clone();
		isCombineReport_ = other.isCombineReport_;
		isAllKill_ = other.isAllKill_;
		allianceCityInfo_ = ((other.allianceCityInfo_ != null) ? other.allianceCityInfo_.Clone() : null);
		winKillSoldierDetail_ = other.winKillSoldierDetail_.Clone();
		hospitalFullDeadDetail_ = other.hospitalFullDeadDetail_.Clone();
		ricochetDetail_ = other.ricochetDetail_.Clone();
		monsterInvasionInfo_ = ((other.monsterInvasionInfo_ != null) ? other.monsterInvasionInfo_.Clone() : null);
		atkMummyBlowUpResultDetail_ = other.atkMummyBlowUpResultDetail_.Clone();
		defMummyBlowUpResultDetail_ = other.defMummyBlowUpResultDetail_.Clone();
		plunderMeteoriteInfo_ = ((other.plunderMeteoriteInfo_ != null) ? other.plunderMeteoriteInfo_.Clone() : null);
		allianceBossSand_ = other.allianceBossSand_.Clone();
		monsterBuffList_ = other.monsterBuffList_.Clone();
		championDuelScore_ = other.championDuelScore_.Clone();
		marchTargetType_ = other.marchTargetType_;
		parallelExecuteProcess_ = other.parallelExecuteProcess_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattleReport Clone()
	{
		return new LwBattleReport(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattleReport);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattleReport other)
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
		if (Type != other.Type)
		{
			return false;
		}
		if (BattleTime != other.BattleTime)
		{
			return false;
		}
		if (!object.Equals(BattlePointInfo, other.BattlePointInfo))
		{
			return false;
		}
		if (FightResult != other.FightResult)
		{
			return false;
		}
		if (!player_.Equals(other.player_))
		{
			return false;
		}
		if (!units_.Equals(other.units_))
		{
			return false;
		}
		if (!object.Equals(Reward, other.Reward))
		{
			return false;
		}
		if (!object.Equals(Detail, other.Detail))
		{
			return false;
		}
		if (WinKillSoldier != other.WinKillSoldier)
		{
			return false;
		}
		if (PlunderValue != other.PlunderValue)
		{
			return false;
		}
		if (MaxPlunderValue != other.MaxPlunderValue)
		{
			return false;
		}
		if (TotalTime != other.TotalTime)
		{
			return false;
		}
		if (OverTime != other.OverTime)
		{
			return false;
		}
		if (Address != other.Address)
		{
			return false;
		}
		if (Version != other.Version)
		{
			return false;
		}
		if (!combinePlayer_.Equals(other.combinePlayer_))
		{
			return false;
		}
		if (!round_.Equals(other.round_))
		{
			return false;
		}
		if (IsCombineReport != other.IsCombineReport)
		{
			return false;
		}
		if (IsAllKill != other.IsAllKill)
		{
			return false;
		}
		if (!object.Equals(AllianceCityInfo, other.AllianceCityInfo))
		{
			return false;
		}
		if (!winKillSoldierDetail_.Equals(other.winKillSoldierDetail_))
		{
			return false;
		}
		if (!hospitalFullDeadDetail_.Equals(other.hospitalFullDeadDetail_))
		{
			return false;
		}
		if (!ricochetDetail_.Equals(other.ricochetDetail_))
		{
			return false;
		}
		if (!object.Equals(MonsterInvasionInfo, other.MonsterInvasionInfo))
		{
			return false;
		}
		if (!atkMummyBlowUpResultDetail_.Equals(other.atkMummyBlowUpResultDetail_))
		{
			return false;
		}
		if (!defMummyBlowUpResultDetail_.Equals(other.defMummyBlowUpResultDetail_))
		{
			return false;
		}
		if (!object.Equals(PlunderMeteoriteInfo, other.PlunderMeteoriteInfo))
		{
			return false;
		}
		if (!allianceBossSand_.Equals(other.allianceBossSand_))
		{
			return false;
		}
		if (!monsterBuffList_.Equals(other.monsterBuffList_))
		{
			return false;
		}
		if (!championDuelScore_.Equals(other.championDuelScore_))
		{
			return false;
		}
		if (MarchTargetType != other.MarchTargetType)
		{
			return false;
		}
		if (ParallelExecuteProcess != other.ParallelExecuteProcess)
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
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (BattleTime != 0L)
		{
			num ^= BattleTime.GetHashCode();
		}
		if (battlePointInfo_ != null)
		{
			num ^= BattlePointInfo.GetHashCode();
		}
		if (FightResult != 0)
		{
			num ^= FightResult.GetHashCode();
		}
		num ^= player_.GetHashCode();
		num ^= units_.GetHashCode();
		if (reward_ != null)
		{
			num ^= Reward.GetHashCode();
		}
		if (detail_ != null)
		{
			num ^= Detail.GetHashCode();
		}
		if (WinKillSoldier != 0L)
		{
			num ^= WinKillSoldier.GetHashCode();
		}
		if (PlunderValue != 0L)
		{
			num ^= PlunderValue.GetHashCode();
		}
		if (MaxPlunderValue != 0L)
		{
			num ^= MaxPlunderValue.GetHashCode();
		}
		if (TotalTime != 0)
		{
			num ^= TotalTime.GetHashCode();
		}
		if (OverTime != 0)
		{
			num ^= OverTime.GetHashCode();
		}
		if (Address.Length != 0)
		{
			num ^= Address.GetHashCode();
		}
		if (Version != 0)
		{
			num ^= Version.GetHashCode();
		}
		num ^= combinePlayer_.GetHashCode();
		num ^= round_.GetHashCode();
		if (IsCombineReport != 0)
		{
			num ^= IsCombineReport.GetHashCode();
		}
		if (IsAllKill != 0)
		{
			num ^= IsAllKill.GetHashCode();
		}
		if (allianceCityInfo_ != null)
		{
			num ^= AllianceCityInfo.GetHashCode();
		}
		num ^= winKillSoldierDetail_.GetHashCode();
		num ^= hospitalFullDeadDetail_.GetHashCode();
		num ^= ricochetDetail_.GetHashCode();
		if (monsterInvasionInfo_ != null)
		{
			num ^= MonsterInvasionInfo.GetHashCode();
		}
		num ^= atkMummyBlowUpResultDetail_.GetHashCode();
		num ^= defMummyBlowUpResultDetail_.GetHashCode();
		if (plunderMeteoriteInfo_ != null)
		{
			num ^= PlunderMeteoriteInfo.GetHashCode();
		}
		num ^= allianceBossSand_.GetHashCode();
		num ^= monsterBuffList_.GetHashCode();
		num ^= championDuelScore_.GetHashCode();
		if (MarchTargetType != 0)
		{
			num ^= MarchTargetType.GetHashCode();
		}
		if (ParallelExecuteProcess != 0)
		{
			num ^= ParallelExecuteProcess.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
		}
		if (BattleTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(BattleTime);
		}
		if (battlePointInfo_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(BattlePointInfo);
		}
		if (FightResult != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FightResult);
		}
		player_.WriteTo(output, _repeated_player_codec);
		units_.WriteTo(output, _repeated_units_codec);
		if (reward_ != null)
		{
			output.WriteRawTag(82);
			output.WriteMessage(Reward);
		}
		if (detail_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(Detail);
		}
		if (WinKillSoldier != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(WinKillSoldier);
		}
		if (PlunderValue != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(PlunderValue);
		}
		if (MaxPlunderValue != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(MaxPlunderValue);
		}
		if (TotalTime != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(TotalTime);
		}
		if (OverTime != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(OverTime);
		}
		if (Address.Length != 0)
		{
			output.WriteRawTag(154, 1);
			output.WriteString(Address);
		}
		if (Version != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(Version);
		}
		combinePlayer_.WriteTo(output, _repeated_combinePlayer_codec);
		round_.WriteTo(output, _repeated_round_codec);
		if (IsCombineReport != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(IsCombineReport);
		}
		if (IsAllKill != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(IsAllKill);
		}
		if (allianceCityInfo_ != null)
		{
			output.WriteRawTag(242, 1);
			output.WriteMessage(AllianceCityInfo);
		}
		winKillSoldierDetail_.WriteTo(output, _repeated_winKillSoldierDetail_codec);
		hospitalFullDeadDetail_.WriteTo(output, _repeated_hospitalFullDeadDetail_codec);
		ricochetDetail_.WriteTo(output, _repeated_ricochetDetail_codec);
		if (monsterInvasionInfo_ != null)
		{
			output.WriteRawTag(146, 2);
			output.WriteMessage(MonsterInvasionInfo);
		}
		atkMummyBlowUpResultDetail_.WriteTo(output, _repeated_atkMummyBlowUpResultDetail_codec);
		defMummyBlowUpResultDetail_.WriteTo(output, _repeated_defMummyBlowUpResultDetail_codec);
		if (plunderMeteoriteInfo_ != null)
		{
			output.WriteRawTag(170, 2);
			output.WriteMessage(PlunderMeteoriteInfo);
		}
		allianceBossSand_.WriteTo(output, _repeated_allianceBossSand_codec);
		monsterBuffList_.WriteTo(output, _repeated_monsterBuffList_codec);
		championDuelScore_.WriteTo(output, _repeated_championDuelScore_codec);
		if (MarchTargetType != 0)
		{
			output.WriteRawTag(200, 2);
			output.WriteInt32(MarchTargetType);
		}
		if (ParallelExecuteProcess != 0)
		{
			output.WriteRawTag(208, 2);
			output.WriteInt32(ParallelExecuteProcess);
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
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (BattleTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BattleTime);
		}
		if (battlePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BattlePointInfo);
		}
		if (FightResult != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FightResult);
		}
		num += player_.CalculateSize(_repeated_player_codec);
		num += units_.CalculateSize(_repeated_units_codec);
		if (reward_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Reward);
		}
		if (detail_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Detail);
		}
		if (WinKillSoldier != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(WinKillSoldier);
		}
		if (PlunderValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PlunderValue);
		}
		if (MaxPlunderValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxPlunderValue);
		}
		if (TotalTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TotalTime);
		}
		if (OverTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(OverTime);
		}
		if (Address.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Address);
		}
		if (Version != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Version);
		}
		num += combinePlayer_.CalculateSize(_repeated_combinePlayer_codec);
		num += round_.CalculateSize(_repeated_round_codec);
		if (IsCombineReport != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(IsCombineReport);
		}
		if (IsAllKill != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(IsAllKill);
		}
		if (allianceCityInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(AllianceCityInfo);
		}
		num += winKillSoldierDetail_.CalculateSize(_repeated_winKillSoldierDetail_codec);
		num += hospitalFullDeadDetail_.CalculateSize(_repeated_hospitalFullDeadDetail_codec);
		num += ricochetDetail_.CalculateSize(_repeated_ricochetDetail_codec);
		if (monsterInvasionInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MonsterInvasionInfo);
		}
		num += atkMummyBlowUpResultDetail_.CalculateSize(_repeated_atkMummyBlowUpResultDetail_codec);
		num += defMummyBlowUpResultDetail_.CalculateSize(_repeated_defMummyBlowUpResultDetail_codec);
		if (plunderMeteoriteInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(PlunderMeteoriteInfo);
		}
		num += allianceBossSand_.CalculateSize(_repeated_allianceBossSand_codec);
		num += monsterBuffList_.CalculateSize(_repeated_monsterBuffList_codec);
		num += championDuelScore_.CalculateSize(_repeated_championDuelScore_codec);
		if (MarchTargetType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MarchTargetType);
		}
		if (ParallelExecuteProcess != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ParallelExecuteProcess);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattleReport other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.BattleTime != 0L)
		{
			BattleTime = other.BattleTime;
		}
		if (other.battlePointInfo_ != null)
		{
			if (battlePointInfo_ == null)
			{
				BattlePointInfo = new BattlePointInfo();
			}
			BattlePointInfo.MergeFrom(other.BattlePointInfo);
		}
		if (other.FightResult != 0)
		{
			FightResult = other.FightResult;
		}
		player_.Add(other.player_);
		units_.Add(other.units_);
		if (other.reward_ != null)
		{
			if (reward_ == null)
			{
				Reward = new Reward();
			}
			Reward.MergeFrom(other.Reward);
		}
		if (other.detail_ != null)
		{
			if (detail_ == null)
			{
				Detail = new LwBattleDetail();
			}
			Detail.MergeFrom(other.Detail);
		}
		if (other.WinKillSoldier != 0L)
		{
			WinKillSoldier = other.WinKillSoldier;
		}
		if (other.PlunderValue != 0L)
		{
			PlunderValue = other.PlunderValue;
		}
		if (other.MaxPlunderValue != 0L)
		{
			MaxPlunderValue = other.MaxPlunderValue;
		}
		if (other.TotalTime != 0)
		{
			TotalTime = other.TotalTime;
		}
		if (other.OverTime != 0)
		{
			OverTime = other.OverTime;
		}
		if (other.Address.Length != 0)
		{
			Address = other.Address;
		}
		if (other.Version != 0)
		{
			Version = other.Version;
		}
		combinePlayer_.Add(other.combinePlayer_);
		round_.Add(other.round_);
		if (other.IsCombineReport != 0)
		{
			IsCombineReport = other.IsCombineReport;
		}
		if (other.IsAllKill != 0)
		{
			IsAllKill = other.IsAllKill;
		}
		if (other.allianceCityInfo_ != null)
		{
			if (allianceCityInfo_ == null)
			{
				AllianceCityInfo = new LwAllianceCityInfo();
			}
			AllianceCityInfo.MergeFrom(other.AllianceCityInfo);
		}
		winKillSoldierDetail_.Add(other.winKillSoldierDetail_);
		hospitalFullDeadDetail_.Add(other.hospitalFullDeadDetail_);
		ricochetDetail_.Add(other.ricochetDetail_);
		if (other.monsterInvasionInfo_ != null)
		{
			if (monsterInvasionInfo_ == null)
			{
				MonsterInvasionInfo = new LwMonsterInvasionInfo();
			}
			MonsterInvasionInfo.MergeFrom(other.MonsterInvasionInfo);
		}
		atkMummyBlowUpResultDetail_.Add(other.atkMummyBlowUpResultDetail_);
		defMummyBlowUpResultDetail_.Add(other.defMummyBlowUpResultDetail_);
		if (other.plunderMeteoriteInfo_ != null)
		{
			if (plunderMeteoriteInfo_ == null)
			{
				PlunderMeteoriteInfo = new MeteoriteInfo();
			}
			PlunderMeteoriteInfo.MergeFrom(other.PlunderMeteoriteInfo);
		}
		allianceBossSand_.Add(other.allianceBossSand_);
		monsterBuffList_.Add(other.monsterBuffList_);
		championDuelScore_.Add(other.championDuelScore_);
		if (other.MarchTargetType != 0)
		{
			MarchTargetType = other.MarchTargetType;
		}
		if (other.ParallelExecuteProcess != 0)
		{
			ParallelExecuteProcess = other.ParallelExecuteProcess;
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
				Uuid = input.ReadInt64();
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			case 24u:
				BattleTime = input.ReadInt64();
				break;
			case 34u:
				if (battlePointInfo_ == null)
				{
					BattlePointInfo = new BattlePointInfo();
				}
				input.ReadMessage(BattlePointInfo);
				break;
			case 40u:
				FightResult = input.ReadInt32();
				break;
			case 50u:
				player_.AddEntriesFrom(input, _repeated_player_codec);
				break;
			case 66u:
				units_.AddEntriesFrom(input, _repeated_units_codec);
				break;
			case 82u:
				if (reward_ == null)
				{
					Reward = new Reward();
				}
				input.ReadMessage(Reward);
				break;
			case 98u:
				if (detail_ == null)
				{
					Detail = new LwBattleDetail();
				}
				input.ReadMessage(Detail);
				break;
			case 104u:
				WinKillSoldier = input.ReadInt64();
				break;
			case 112u:
				PlunderValue = input.ReadInt64();
				break;
			case 120u:
				MaxPlunderValue = input.ReadInt64();
				break;
			case 128u:
				TotalTime = input.ReadInt32();
				break;
			case 136u:
				OverTime = input.ReadInt32();
				break;
			case 154u:
				Address = input.ReadString();
				break;
			case 160u:
				Version = input.ReadInt32();
				break;
			case 170u:
				combinePlayer_.AddEntriesFrom(input, _repeated_combinePlayer_codec);
				break;
			case 178u:
				round_.AddEntriesFrom(input, _repeated_round_codec);
				break;
			case 184u:
				IsCombineReport = input.ReadInt32();
				break;
			case 192u:
				IsAllKill = input.ReadInt32();
				break;
			case 242u:
				if (allianceCityInfo_ == null)
				{
					AllianceCityInfo = new LwAllianceCityInfo();
				}
				input.ReadMessage(AllianceCityInfo);
				break;
			case 250u:
				winKillSoldierDetail_.AddEntriesFrom(input, _repeated_winKillSoldierDetail_codec);
				break;
			case 258u:
				hospitalFullDeadDetail_.AddEntriesFrom(input, _repeated_hospitalFullDeadDetail_codec);
				break;
			case 266u:
				ricochetDetail_.AddEntriesFrom(input, _repeated_ricochetDetail_codec);
				break;
			case 274u:
				if (monsterInvasionInfo_ == null)
				{
					MonsterInvasionInfo = new LwMonsterInvasionInfo();
				}
				input.ReadMessage(MonsterInvasionInfo);
				break;
			case 282u:
				atkMummyBlowUpResultDetail_.AddEntriesFrom(input, _repeated_atkMummyBlowUpResultDetail_codec);
				break;
			case 290u:
				defMummyBlowUpResultDetail_.AddEntriesFrom(input, _repeated_defMummyBlowUpResultDetail_codec);
				break;
			case 298u:
				if (plunderMeteoriteInfo_ == null)
				{
					PlunderMeteoriteInfo = new MeteoriteInfo();
				}
				input.ReadMessage(PlunderMeteoriteInfo);
				break;
			case 306u:
				allianceBossSand_.AddEntriesFrom(input, _repeated_allianceBossSand_codec);
				break;
			case 312u:
			case 314u:
				monsterBuffList_.AddEntriesFrom(input, _repeated_monsterBuffList_codec);
				break;
			case 322u:
				championDuelScore_.AddEntriesFrom(input, _repeated_championDuelScore_codec);
				break;
			case 328u:
				MarchTargetType = input.ReadInt32();
				break;
			case 336u:
				ParallelExecuteProcess = input.ReadInt32();
				break;
			}
		}
	}
}
