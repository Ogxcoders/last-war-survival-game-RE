using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using GameFramework;
using GameKit.Base;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;
using XLua;

public class WorldMarchDataManager : WorldManagerBase
{
	private enum BatleResult
	{
		DEFAULT = -1,
		SELF_WIN,
		OTHER_WIN,
		DRAW
	}

	public enum BattleWordType
	{
		Normal,
		Cure,
		Skill
	}

	private enum EMarchStepUpdateMoveState
	{
		Invalid,
		UpdateVisible,
		UpdatePerformance,
		UpdateTroopLine
	}

	public const int normalAttackId = 100000;

	private const string sheldPath = "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab";

	private List<long> removeList = new List<long>();

	private const int __REMAIN_COUNT__ = 1000;

	private HashSet<long> allMarchUuids = new HashSet<long>();

	private Dictionary<long, WorldMarch> allMarches = new Dictionary<long, WorldMarch>();

	private HashSet<long> toMeMarchUuids = new HashSet<long>();

	private Dictionary<long, int> toMeMarchTargetUuids = new Dictionary<long, int>();

	private HashSet<long> myMarchUuids = new HashSet<long>();

	private Dictionary<long, WorldMarch> ownerMarches = new Dictionary<long, WorldMarch>();

	private Dictionary<long, long> team2MarchUuid = new Dictionary<long, long>();

	private Dictionary<long, long> member2LeaderUuid = new Dictionary<long, long>();

	private Dictionary<long, int> myAssitanceUuid2Points = new Dictionary<long, int>();

	private Dictionary<int, List<long>> myAssistancePoint2Uuids = new Dictionary<int, List<long>>();

	private Dictionary<string, List<WorldMarch>> allianceMarches = new Dictionary<string, List<WorldMarch>>();

	private Dictionary<long, List<WorldMarch>> fakeRetreatMarches = new Dictionary<long, List<WorldMarch>>();

	private Dictionary<long, WorldMarch> fakeSampleMarches = new Dictionary<long, WorldMarch>();

	private Dictionary<long, WorldMarch> fakeAttackMonsterMarches = new Dictionary<long, WorldMarch>();

	private Dictionary<int, WorldTrainConfig> trainConfigs = new Dictionary<int, WorldTrainConfig>();

	private Dictionary<int, int> flowerCarLength = new Dictionary<int, int>();

	private HashSet<long> _tmpMarchSet = new HashSet<long>();

	private List<WorldMarch> _tmpMarchList = new List<WorldMarch>();

	public static Action<long, int, int, int> OnMonsterAdd;

	public static Action<long, int, int, int> OnMonsterDelete;

	private Bounds viewBounds;

	private Rect viewRect;

	private static long _fakeUuid = 0L;

	private Dictionary<long, long> _delayDestroyTroop = new Dictionary<long, long>();

	private bool myMarchDirty;

	private bool toMeMarchDirty;

	private Dictionary<string, long> _cacheClientCreateGuidAndTimeDict = new Dictionary<string, long>();

	private int multiKillPVPMin = -1;

	private int multiKillPVEMin = -1;

	private bool? multiKillSwitch;

	private List<long> _toRemoveList = new List<long>();

	private Dictionary<long, WorldMarch> targetForMineMarchDic = new Dictionary<long, WorldMarch>(1024);

	private Dictionary<long, WorldMarch> targetForMineMarchDesertDic = new Dictionary<long, WorldMarch>(1024);

	private bool toMeMarchNeedUpdate = true;

	private bool toMeMarchDesertNeedUpdate = true;

	private WorldMarchBattleSound _marchBattleSound = new WorldMarchBattleSound();

	private StepProcessQueue<(WorldMarchMessageType, ISFSObject)> _messages;

	private Dictionary<long, (ISFSObject lastObjState, WorldMarchMessageType lastMsgType)> _messagesMergeDict;

	private Stopwatch _marchStepUpdateTimer = new Stopwatch();

	private int _marchStepUpdateMoveId = -1;

	private int _tempPerformanceMarchCountAcc;

	private int _tempPerformanceTroopLineCountAcc;

	private EMarchStepUpdateMoveState _marchStepUpdateState;

	private static Dictionary<long, WorldMarch> EmptyDic = new Dictionary<long, WorldMarch>();

	private readonly HashSet<int> allianceMembersHomePos = new HashSet<int>();

	private Dictionary<long, int> myMarchMultiKillPVE = new Dictionary<long, int>();

	private Dictionary<long, int> myMarchMultiKillPVP = new Dictionary<long, int>();

	private Dictionary<int, int> tileBlockIndex = new Dictionary<int, int>();

	private bool isRecordingMarchBlock;

	private int recordingWorldId = int.MinValue;

	public static long SelectMarchUuid = 0L;

	public static long DebugMarchUuid = 0L;

	public static int __INC_UPDATE__ { get; private set; } = 0;

	public int AllMarchesCount => allMarchUuids.Count;

	private int MultiKillPVPMin
	{
		get
		{
			if (multiKillPVPMin == -1)
			{
				multiKillPVPMin = GameEntry.Lua.CallWithReturn<int, string, string, int>("CSharpCallLuaInterface.GetConfigNum", "killstreak_report_UI", "k1", 5);
			}
			return multiKillPVPMin;
		}
	}

	private int MultiKillPVEMin
	{
		get
		{
			if (multiKillPVEMin == -1)
			{
				multiKillPVEMin = GameEntry.Lua.CallWithReturn<int, string, string, int>("CSharpCallLuaInterface.GetConfigNum", "killstreak_report_UI", "k3", 5);
			}
			return multiKillPVEMin;
		}
	}

	private bool? MultiKillSwitch => multiKillSwitch ?? (multiKillSwitch = GameEntry.Data?.Player?.CheckSwitch("killstreak_report_UI_switch", defaultVal: false) ?? false);

	public bool EnableWorldAssistanceOpt => world?.EnableWorldAssistanceOpt ?? false;

	public int lastPerformanceCount { get; private set; } = -1;

	public int lastPerformanceTroopLineCount { get; private set; } = -1;

	public static WorldMarchDataManager EditorInstance => null;

	public Dictionary<int, int> TileBlockIndex => tileBlockIndex;

	private static bool IsCombine(CombatUnitType type)
	{
		switch (type)
		{
		case CombatUnitType.BUILDING:
		case CombatUnitType.TOWER:
		case CombatUnitType.RALLY_TEAM:
		case CombatUnitType.CITY:
		case CombatUnitType.ALLIANCE_OCCUPIED_CITY:
			return true;
		default:
			return false;
		}
	}

	private bool IsArmyInView(long uuid, CombatUnitType combineType)
	{
		switch (combineType)
		{
		case CombatUnitType.BUILDING:
		case CombatUnitType.TOWER:
		case CombatUnitType.CITY:
		case CombatUnitType.EXPLORE_POINT:
		case CombatUnitType.ALLIANCE_NEUTRAL_CITY:
		case CombatUnitType.ALLIANCE_OCCUPIED_CITY:
		{
			PointInfo pointInfoByUuid = world.GetPointInfoByUuid(uuid);
			if (pointInfoByUuid != null && IsInView(world.TileIndexToWorld(pointInfoByUuid.mainIndex)))
			{
				return true;
			}
			break;
		}
		case CombatUnitType.ARMY:
		case CombatUnitType.MONSTER:
		case CombatUnitType.RALLY_TEAM:
		case CombatUnitType.BOSS:
		case CombatUnitType.ACT_BOSS:
		case CombatUnitType.PUZZLE_BOSS:
		case CombatUnitType.CHALLENGE_BOSS:
		{
			WorldMarch march = GetMarch(uuid);
			if (march != null && IsInView(march.position))
			{
				return true;
			}
			break;
		}
		}
		return false;
	}

	public void UpdateBattleMessage(ISFSObject message)
	{
		byte[] data = Base64.Decode(message.GetUtfString("content"));
		BattleRoundPushInfo battleRoundPushInfo = BattleRoundPushInfo.Parser.ParseFrom(data);
		CombatUnitType type = (CombatUnitType)battleRoundPushInfo.Type;
		if (IsCombine(type))
		{
			if (battleRoundPushInfo.CombineArmyInfo != null)
			{
				UpdateCombineArmyInfo(battleRoundPushInfo.CombineArmyInfo, battleRoundPushInfo.OutRange, battleRoundPushInfo.RoundReports, type);
			}
			if (battleRoundPushInfo.SimpleArmyInfo != null)
			{
				UpdateSimpleArmyInfo(battleRoundPushInfo.SimpleArmyInfo, battleRoundPushInfo.OutRange, battleRoundPushInfo.RoundReports, type);
			}
		}
		else
		{
			UpdateSimpleArmyInfo(battleRoundPushInfo.SimpleArmyInfo, battleRoundPushInfo.OutRange, battleRoundPushInfo.RoundReports, type);
		}
	}

	private void UpdateCombineArmyInfo(CombineSelfArmyInfo combineArmyInfo, bool outRange, RepeatedField<BaseRoundReportPush> roundReports, CombatUnitType combineType)
	{
		RepeatedField<SimpleSelfArmyInfo> members = combineArmyInfo.Members;
		SimpleCombatUnitPushObj targetInfo = combineArmyInfo.TargetInfo;
		long num = 0L;
		if (outRange)
		{
			return;
		}
		Dictionary<long, bool> dictionary = new Dictionary<long, bool>();
		foreach (SimpleSelfArmyInfo item in members)
		{
			if (num == 0L && item.ArmyInfo != null)
			{
				num = item.ArmyInfo.TopUuid;
			}
			if (item.ArmyInfo != null && item.ArmyInfo.ArmyInfo != null)
			{
				dictionary[item.ArmyInfo.ArmyInfo.Uuid] = true;
			}
		}
		if (combineType == CombatUnitType.RALLY_TEAM)
		{
			num = 0L;
			foreach (KeyValuePair<long, bool> item2 in dictionary)
			{
				WorldMarch march = GetMarch(item2.Key);
				if (march != null)
				{
					WorldMarch allianceMarchesInTeam = GetAllianceMarchesInTeam(march.allianceUid, march.teamUuid);
					if (allianceMarchesInTeam != null)
					{
						num = allianceMarchesInTeam.uuid;
					}
				}
			}
		}
		if (num == 0L || !IsArmyInView(num, combineType))
		{
			return;
		}
		int heal = 0;
		int normalHurt = 0;
		int skillHurt = 0;
		int showAttackSkillId = 0;
		int showHurtSkillId = 0;
		bool isActiveAttack = false;
		List<int> effectBuffList = new List<int>();
		string text = "";
		for (int i = 0; i < roundReports.Count; i++)
		{
			BaseRoundReportPush reportInfo = roundReports[i];
			foreach (KeyValuePair<long, bool> item3 in dictionary)
			{
				CheckArmyDoSkill(item3.Key, reportInfo, ref showAttackSkillId, ref showHurtSkillId, ref skillHurt, ref normalHurt, ref isActiveAttack, ref effectBuffList);
			}
		}
		for (int j = 0; j < effectBuffList.Count; j++)
		{
			text += effectBuffList[j];
			if (j < effectBuffList.Count - 1)
			{
				text += ";";
			}
		}
		switch (combineType)
		{
		case CombatUnitType.ALLIANCE_OCCUPIED_CITY:
			foreach (SimpleSelfArmyInfo item4 in members)
			{
				if (item4.ArmyInfo != null && item4.ArmyInfo.ArmyInfo != null)
				{
					SimpleCombatUnit armyInfo2 = item4.ArmyInfo.ArmyInfo;
					if (armyInfo2.Uuid == num)
					{
						AllianceCityUpdateHeadUI(num, armyInfo2.Health, armyInfo2.InitHealth);
						break;
					}
				}
			}
			ShowAllianceCityBloodHurt(num, normalHurt, skillHurt, heal);
			ShowAllianceCityBuff(num, text);
			break;
		case CombatUnitType.RALLY_TEAM:
		{
			long defAtkUuid = targetInfo?.TopUuid ?? 0;
			SetTroopAttack(num, defAtkUuid, isActiveAttack);
			ShowTroopBloodHurt(num, normalHurt, skillHurt, heal);
			int anger = 0;
			int num4 = 0;
			int num5 = 0;
			foreach (SimpleSelfArmyInfo item5 in members)
			{
				if (item5.ArmyInfo != null && item5.ArmyInfo.ArmyInfo != null)
				{
					SimpleCombatUnit armyInfo3 = item5.ArmyInfo.ArmyInfo;
					num4 += armyInfo3.Health;
					num5 += armyInfo3.InitHealth;
				}
			}
			TroopUpdateHeadUI(num, anger, num4, num5);
			ShowTroopBuff(num, text);
			break;
		}
		case CombatUnitType.BUILDING:
		case CombatUnitType.TOWER:
		case CombatUnitType.CITY:
		{
			int num2 = 0;
			int num3 = 0;
			foreach (SimpleSelfArmyInfo item6 in members)
			{
				if (item6.ArmyInfo != null && item6.ArmyInfo.ArmyInfo != null)
				{
					SimpleCombatUnit armyInfo = item6.ArmyInfo.ArmyInfo;
					num2 += armyInfo.Health;
					num3 += armyInfo.InitHealth;
				}
			}
			string userData = num + ";" + num2 + ";" + num3;
			GameEntry.Event.Fire(EventId.ShowBuildAttackHeadUI, userData);
			ShowPlayerBuildBloodHurt(num, normalHurt, skillHurt, heal);
			ShowPlayerBuildBuff(num, text);
			break;
		}
		}
	}

	private void UpdateSimpleArmyInfo(SimpleSelfArmyInfo selfArmyInfo, bool outRange, RepeatedField<BaseRoundReportPush> roundReports, CombatUnitType combineType)
	{
		SimpleCombatUnitPushObj armyInfo = selfArmyInfo.ArmyInfo;
		SimpleCombatUnitPushObj targetInfo = selfArmyInfo.TargetInfo;
		int heal = selfArmyInfo.Heal;
		int shield = selfArmyInfo.Shield;
		if (armyInfo.TopUuid == 0L || outRange || !IsArmyInView(armyInfo.TopUuid, combineType))
		{
			return;
		}
		int normalHurt = 0;
		int skillHurt = 0;
		int showAttackSkillId = 0;
		int showHurtSkillId = 0;
		bool isActiveAttack = false;
		List<int> effectBuffList = new List<int>();
		string text = "";
		for (int i = 0; i < roundReports.Count; i++)
		{
			BaseRoundReportPush reportInfo = roundReports[i];
			CheckArmyDoSkill(armyInfo.TopUuid, reportInfo, ref showAttackSkillId, ref showHurtSkillId, ref skillHurt, ref normalHurt, ref isActiveAttack, ref effectBuffList);
		}
		for (int j = 0; j < effectBuffList.Count; j++)
		{
			text += effectBuffList[j];
			if (j < effectBuffList.Count - 1)
			{
				text += ";";
			}
		}
		switch (combineType)
		{
		case CombatUnitType.ALLIANCE_NEUTRAL_CITY:
			AllianceCityUpdateHeadUI(armyInfo.TopUuid, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			ShowAllianceCityBloodHurt(armyInfo.TopUuid, normalHurt, skillHurt, heal);
			ShowAllianceCityBuff(armyInfo.TopUuid, text);
			break;
		case CombatUnitType.ARMY:
		case CombatUnitType.MONSTER:
		case CombatUnitType.BOSS:
		case CombatUnitType.ACT_BOSS:
		case CombatUnitType.PUZZLE_BOSS:
		case CombatUnitType.CHALLENGE_BOSS:
		{
			long defAtkUuid = targetInfo?.TopUuid ?? 0;
			if (targetInfo != null && targetInfo.Type == 5)
			{
				defAtkUuid = 0L;
			}
			SetTroopAttack(armyInfo.TopUuid, defAtkUuid, isActiveAttack);
			ShowShieldEffect(armyInfo.TopUuid, shield);
			ShowTroopSkill(armyInfo.TopUuid, showAttackSkillId, showHurtSkillId);
			ShowTroopBloodHurt(armyInfo.TopUuid, normalHurt, skillHurt, heal);
			if (combineType == CombatUnitType.ACT_BOSS || combineType == CombatUnitType.PUZZLE_BOSS)
			{
				ActBossUpdateHeadUI(armyInfo.TopUuid, selfArmyInfo.Anger, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			}
			else
			{
				TroopUpdateHeadUI(armyInfo.TopUuid, selfArmyInfo.Anger, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			}
			ShowTroopBuff(armyInfo.TopUuid, text);
			break;
		}
		case CombatUnitType.TOWER:
			if (targetInfo != null && targetInfo.ArmyInfo != null)
			{
				BuildingAttack(armyInfo.ArmyInfo.Uuid, targetInfo.ArmyInfo.Uuid);
				ShowPlayerBuildBuff(armyInfo.ArmyInfo.Uuid, text);
			}
			break;
		case CombatUnitType.EXPLORE_POINT:
		{
			long defenderUid = targetInfo?.TopUuid ?? 0;
			ExploreAttack(armyInfo.TopUuid, defenderUid, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			ExploreUpdateHeadUI(armyInfo.TopUuid, selfArmyInfo.Anger, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			ShowExploreSkill(armyInfo.TopUuid, showAttackSkillId, showHurtSkillId);
			ShowExploreBloodHurt(armyInfo.TopUuid, normalHurt, skillHurt, heal);
			ShowExploreBuff(armyInfo.TopUuid, text);
			break;
		}
		}
	}

	private void ShowShieldEffect(long atkUuid, int shield)
	{
		WorldTroop troop = world.GetTroop(atkUuid);
		if (troop != null)
		{
			if (shield > 0)
			{
				troop.AddShield();
			}
			else
			{
				troop.DelShield();
			}
		}
	}

	private void SetTroopAttack(long atkUuid, long defAtkUuid, bool isActiveAttack)
	{
		WorldTroop troop = world.GetTroop(atkUuid);
		if (troop == null)
		{
			return;
		}
		troop.SetRotationRoot();
		troop.SetIsBattle(value: true);
		WorldMarch marchInfo = troop.GetMarchInfo();
		if (marchInfo != null && (marchInfo.type == NewMarchType.ACT_BOSS || marchInfo.type == NewMarchType.PUZZLE_BOSS))
		{
			return;
		}
		if (marchInfo != null && (marchInfo.IsMonsterOrOrdinaryBoss() || marchInfo.IsWanderBoss() || marchInfo.type == NewMarchType.DARK_KNIGHT_CITY || marchInfo.type == NewMarchType.CHALLENGE_BOSS))
		{
			troop.defAtkUuid = 0L;
			troop.SetRotation(Quaternion.LookRotation(troop.GetDefenderPosition() - troop.GetPosition()));
			troop.ReSetEntityTarget();
			troop.Attack();
		}
		else
		{
			if (!isActiveAttack)
			{
				return;
			}
			troop.defAtkUuid = defAtkUuid;
			WorldMarch march = world.GetMarch(defAtkUuid);
			if (march != null && march.ownerUid == GameEntry.Data.Player.Uid)
			{
				GameEntry.Event.Fire(EventId.ShowBattleRedName, atkUuid);
			}
			troop.SetRotation(Quaternion.LookRotation(troop.GetDefenderPosition() - troop.GetPosition()));
			troop.ReSetEntityTarget();
			troop.Attack();
			if (marchInfo != null)
			{
				switch (marchInfo.type)
				{
				case NewMarchType.DEFAULT:
				case NewMarchType.NORMAL:
				case NewMarchType.EXPLORE:
				case NewMarchType.DIRECT_MOVE_MARCH:
				case NewMarchType.ALL_OUT:
				case NewMarchType.FAKE_ATTACK:
					troop.ShowAttack();
					break;
				case NewMarchType.ASSEMBLY_MARCH:
					troop.ShowRallyMarchAttack();
					break;
				}
			}
		}
	}

	private void ShowTroopSkill(long skillTargetUuid, int selfSkillId, int hurtSkillId)
	{
		WorldTroop troop = world.GetTroop(skillTargetUuid);
		if (troop != null)
		{
			if (hurtSkillId > 0)
			{
				troop.DoSkill(hurtSkillId, DamageType.ATTACK, 0, null, 0L);
			}
			if (selfSkillId > 0)
			{
				GameEntry.Event.Fire(EventId.ShowHeroIconByUseSkill, skillTargetUuid);
			}
			else if (hurtSkillId > 0)
			{
				GameEntry.Event.Fire(EventId.ShowHeroHitedUiEffect, skillTargetUuid);
			}
		}
	}

	private void ShowTroopBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		WorldTroop troop = world.GetTroop(hurtTargetUuid);
		if (troop == null)
		{
			WorldMarch march = world.GetMarch(hurtTargetUuid);
			if (march != null && march.status == MarchStatus.COLLECTING)
			{
				int targetPos = march.targetPos;
				ShowCollectPointBloodHurt(targetPos, normalHurt, skillHurt, heal);
			}
			return;
		}
		if (normalHurt > 0)
		{
			troop.ShowBattleHurt(normalHurt, BattleWordType.Normal);
		}
		if (skillHurt > 0)
		{
			troop.ShowBattleHurt(skillHurt, BattleWordType.Skill);
		}
		if (heal > 0)
		{
			troop.ShowBattleHurt(heal * -1, BattleWordType.Cure);
		}
	}

	private void TroopUpdateHeadUI(long marchUuid, int anger, int hp, int hpMax)
	{
		if (world.GetTroop(marchUuid) == null)
		{
			WorldMarch march = world.GetMarch(marchUuid);
			if (march != null && march.status == MarchStatus.COLLECTING)
			{
				int targetPos = march.targetPos;
				ShowCollectUpdateHeadUI(marchUuid, targetPos, anger, hp, hpMax);
			}
		}
		else
		{
			string userData = marchUuid + ";" + anger + ";" + hp + ";" + hpMax;
			GameEntry.Event.Fire(EventId.ShowTroopBattleValue, userData);
		}
	}

	private void ActBossUpdateHeadUI(long marchUuid, int anger, int hp, int hpMax)
	{
		string userData = marchUuid + ";" + anger + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowActBossBattleValue, userData);
	}

	private void ShowTroopBuff(long targetUuid, string effectStr)
	{
		if (effectStr.IsNullOrEmpty())
		{
			return;
		}
		WorldTroop troop = world.GetTroop(targetUuid);
		if (troop == null)
		{
			WorldMarch march = world.GetMarch(targetUuid);
			if (march != null && march.status == MarchStatus.COLLECTING)
			{
				int targetPos = march.targetPos;
				ShowCollectPointBuff(targetUuid, targetPos, effectStr);
			}
		}
		else
		{
			string userData = targetUuid + "|" + world.WorldToTileIndex(troop.GetPosition()) + "|" + effectStr;
			GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
		}
	}

	private void ShowExploreSkill(long skillTargetUuid, int selfSkillId, int hurtSkillId)
	{
		if (world.GetObjectByUuid(skillTargetUuid) is WorldExploreObject worldExploreObject)
		{
			if (selfSkillId > 0)
			{
				worldExploreObject.DoSkill(selfSkillId, DamageType.USE_SKILL, 0, null, 0L);
			}
			if (hurtSkillId > 0)
			{
				worldExploreObject.DoSkill(hurtSkillId, DamageType.ATTACK, 0, null, 0L);
			}
		}
	}

	private void ShowExploreBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		if (world.GetObjectByUuid(hurtTargetUuid) is WorldExploreObject worldExploreObject)
		{
			if (normalHurt > 0)
			{
				worldExploreObject.ShowBattleHurt(normalHurt, BattleWordType.Normal);
			}
			if (skillHurt > 0)
			{
				worldExploreObject.ShowBattleHurt(skillHurt, BattleWordType.Skill);
			}
			if (heal > 0)
			{
				worldExploreObject.ShowBattleHurt(heal * -1, BattleWordType.Cure);
			}
		}
	}

	private void ExploreAttack(long atkUuid, long defenderUid, int soliderNum, int hp, int hpMax)
	{
		if (world.GetObjectByUuid(atkUuid) is WorldExploreObject worldExploreObject)
		{
			worldExploreObject.DoWhenAttackExploreStart(defenderUid, soliderNum, hp, hpMax);
		}
	}

	private void ExploreUpdateHeadUI(long marchUuid, int anger, int hp, int hpMax)
	{
		if (world.GetObjectByUuid(marchUuid) is WorldExploreObject worldExploreObject)
		{
			worldExploreObject.UpdateBattleHeadUI(anger, hp, hpMax);
		}
	}

	private void ShowExploreBuff(long targetUuid, string effectStr)
	{
		if (!effectStr.IsNullOrEmpty() && world.GetObjectByUuid(targetUuid) is WorldExploreObject worldExploreObject)
		{
			Transform transform = worldExploreObject.GetTransform();
			if (!(transform == null))
			{
				string userData = targetUuid + "|" + world.WorldToTileIndex(transform.position) + "|" + effectStr + "|" + 8;
				GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
			}
		}
	}

	private void AllianceCityUpdateHeadUI(long cityUuid, int hp, int hpMax)
	{
		string userData = cityUuid + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowAllianceCitySoldierBlood, userData);
	}

	private void ShowAllianceCityBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		PointInfo pointInfoByUuid = world.GetPointInfoByUuid(hurtTargetUuid);
		if (pointInfoByUuid != null)
		{
			Vector3 startPos = world.TileIndexToWorld(pointInfoByUuid.mainIndex) + new Vector3(-6f, 0f, -14f);
			if (normalHurt > 0)
			{
				string path = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildNormalBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = normalHurt,
					path = path
				}, path);
			}
			if (skillHurt > 0)
			{
				string path2 = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildDecBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = skillHurt,
					path = path2
				}, path2);
			}
		}
	}

	private void ShowAllianceCityBuff(long targetUuid, string effectStr)
	{
		PointInfo pointInfoByUuid = world.GetPointInfoByUuid(targetUuid);
		if (pointInfoByUuid != null)
		{
			Vector3 pos = world.TileIndexToWorld(pointInfoByUuid.mainIndex) + new Vector3(-6f, 0f, -14f);
			if (!effectStr.IsNullOrEmpty())
			{
				string userData = targetUuid + "|" + world.WorldToTileIndex(pos) + "|" + effectStr + "|" + 11;
				GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
			}
		}
	}

	private void BuildingAttack(long atkUuid, long defUuid)
	{
		CityBuilding buildingByUuid = world.GetBuildingByUuid(atkUuid);
		if (!(buildingByUuid == null))
		{
			buildingByUuid.OnBattleAtkUpdate(defUuid);
		}
	}

	private void ShowPlayerBuildBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		if (world.GetPointInfoByUuid(hurtTargetUuid) is BuildPointInfo { tileSize: var num } buildPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 startPos = world.TileIndexToWorld(buildPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			if (normalHurt > 0)
			{
				string path = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildNormalBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = normalHurt,
					path = path
				}, path);
			}
			if (skillHurt > 0)
			{
				string path2 = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildDecBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = skillHurt,
					path = path2
				}, path2);
			}
		}
	}

	private void ShowPlayerBuildBuff(long targetUuid, string effectStr)
	{
		if (!effectStr.IsNullOrEmpty() && world.GetPointInfoByUuid(targetUuid) is BuildPointInfo { tileSize: var num } buildPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 pos = world.TileIndexToWorld(buildPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			string userData = targetUuid + "|" + world.WorldToTileIndex(pos) + "|" + effectStr + "|" + 6;
			GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
		}
	}

	public void BattleFinish(ISFSObject message)
	{
		try
		{
			long num = message.GetLong("leaderUuid");
			BatleResult batleResult = (BatleResult)message.GetInt("result");
			WorldTroop troop = world.GetTroop(num);
			try
			{
				if (troop != null && troop.DelayApply)
				{
					float delayApplyTime = troop.DelayApplyTime;
					world.StartCoroutine(DelayApply(delayApplyTime, num, batleResult));
					if (troop.GetMarchInfo().IsEVP())
					{
						switch (batleResult)
						{
						case BatleResult.SELF_WIN:
							troop.ShowZombieRushDefendSuccess();
							break;
						case BatleResult.OTHER_WIN:
							troop.ShowZombieRushDefendFailed();
							break;
						}
					}
				}
				else
				{
					onBattleFinish(num, batleResult);
				}
			}
			catch (Exception arg)
			{
				Log.Error($"Exception when BattleFinish 2 {arg}");
			}
		}
		catch (Exception arg2)
		{
			Log.Error($"Exception when BattleFinish 1 {arg2}");
		}
	}

	private IEnumerator DelayApply(float delay, long uuid, BatleResult result)
	{
		yield return new WaitForSeconds(delay);
		try
		{
			onBattleFinish(uuid, result);
		}
		catch (Exception arg)
		{
			Log.Error($"Exception when BattleFinish 3 {arg}");
		}
	}

	private void onBattleFinish(long uuid, BatleResult result)
	{
		WorldTroop troop = world.GetTroop(uuid);
		world.HideTroopDestination(uuid);
		GameEntry.Event.Fire(EventId.CollectPointOut, uuid);
		GameEntry.Event.Fire(EventId.HideAllianceCitySoliderBlood, uuid);
		GameEntry.Event.Fire(EventId.HideBuildAttackHeadUI, uuid);
		GameEntry.Event.Fire(EventId.HideBattleBuff, uuid);
		world.RemovePosAndRotationDataByMarchUuid(uuid);
		if (troop == null)
		{
			return;
		}
		troop.SetIsBattle(value: false);
		troop.defAtkUuid = 0L;
		switch (result)
		{
		case BatleResult.SELF_WIN:
		{
			if (world.IsSelfInCurrentMarchTeam(uuid))
			{
				GameEntry.Sound.PlayEffect("effect_message");
				troop.ShowBattleSuccess();
				GameEntry.Event.Fire(EventId.MarchEndWithReward, uuid);
			}
			WorldTroop targetTroop2 = troop.GetTargetTroop();
			if (targetTroop2 != null && targetTroop2.IsCityStrongholdMonsterTroop())
			{
				string text = targetTroop2.GetMarchUUID().ToString();
				string key = text + "_reborn";
				string data = GameEntry.Data.Player.GetData(key);
				if (text.Equals(data))
				{
					GameEntry.Data.Player.DeleteData(key);
					targetTroop2.TryPlayBornAnim();
				}
				break;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(GameEntry.Data.Player.GetAllData());
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (item.Key.EndsWith("_reborn"))
				{
					list.Add(item.Key);
					world.GetTroop(long.Parse(item.Value))?.TryPlayBornAnim();
				}
			}
			foreach (string item2 in list)
			{
				GameEntry.Data.Player.DeleteData(item2);
			}
			break;
		}
		case BatleResult.OTHER_WIN:
			if (world.IsSelfInCurrentMarchTeam(uuid))
			{
				GameEntry.Sound.PlayEffect("effect_message");
				GameEntry.Event.Fire(EventId.MarchFail, uuid);
				troop.ShowBattleFailed();
			}
			if (troop.DelayApply && troop.IsMarchTargetAttack() && !troop.IsAttackWorldBoss() && !troop.IsAttackAisilla())
			{
				WorldTroop targetTroop = troop.GetTargetTroop();
				if (targetTroop != null && targetTroop.IsMonsterTroop() && targetTroop.GetMarchTargetType() != MarchTargetType.RUNNING_BOSS_ATTACK_CITY && targetTroop.IsCanPlayAni())
				{
					targetTroop.TryPlay("idle", targetTroop.GetPosition() + Vector3.back);
				}
			}
			break;
		}
		troop.BackTroopUnits();
		troop.ClearEffect();
		MarchStatus marchStatus = troop.GetMarchStatus();
		MarchTargetType marchTargetType = troop.GetMarchTargetType();
		if (!troop.IsDelayDestroy)
		{
			if (marchStatus == MarchStatus.CHASING || marchStatus == MarchStatus.MOVING || marchTargetType == MarchTargetType.BACK_HOME)
			{
				troop.PlayAnim("run");
			}
			else
			{
				troop.PlayAnim("idle");
			}
		}
	}

	private void ShowCollectPointBloodHurt(int pointIndex, int normalHurt, int skillHurt, int heal)
	{
		if (world.GetPointInfo(pointIndex) is ResPointInfo { tileSize: var num } resPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 startPos = world.TileIndexToWorld(resPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			if (normalHurt > 0)
			{
				string path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = normalHurt,
					path = path
				}, path);
			}
			if (skillHurt > 0)
			{
				string path2 = "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = skillHurt,
					path = path2
				}, path2);
			}
		}
	}

	private void ShowCollectPointBuff(long targetUuid, int pointIndex, string effectStr)
	{
		if (!effectStr.IsNullOrEmpty() && world.GetPointInfo(pointIndex) is ResPointInfo { tileSize: var num } resPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 pos = world.TileIndexToWorld(resPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			string userData = targetUuid + "|" + world.WorldToTileIndex(pos) + "|" + effectStr + "|" + 7;
			GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
		}
	}

	private void ShowCollectUpdateHeadUI(long marchUuid, int pointIndex, int anger, int hp, int hpMax)
	{
		string userData = marchUuid + ";" + pointIndex + ";" + anger + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowCollectBattleValue, userData);
	}

	private void CheckArmyDoSkill(long armyUuid, BaseRoundReportPush reportInfo, ref int showAttackSkillId, ref int showHurtSkillId, ref int skillHurt, ref int normalHurt, ref bool isActiveAttack, ref List<int> effectBuffList)
	{
		long triggerUuid = reportInfo.TriggerUuid;
		long targetUuid = reportInfo.TargetUuid;
		BaseRoundReport roundReport = reportInfo.RoundReport;
		DamageType type = (DamageType)roundReport.Type;
		switch (type)
		{
		case DamageType.USE_SKILL:
			if (triggerUuid != armyUuid)
			{
				break;
			}
			if (showAttackSkillId <= 0)
			{
				int skillId2 = roundReport.SkillId;
				string templateData3 = GameEntry.ConfigCache.GetTemplateData("skill", skillId2, "effect_path");
				string templateData4 = GameEntry.ConfigCache.GetTemplateData("skill", skillId2, "effect_point_type");
				int num3 = GameEntry.ConfigCache.GetTemplateData("skill", skillId2, "type").ToInt();
				if (!templateData4.IsNullOrEmpty())
				{
					templateData4.ToInt();
				}
				if (num3 == 11 && !templateData3.IsNullOrEmpty() && "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab" != templateData3)
				{
					showAttackSkillId = skillId2;
				}
			}
			if (roundReport.SkillId == 100000)
			{
				isActiveAttack = true;
			}
			break;
		case DamageType.ATTACK:
			if (targetUuid == armyUuid)
			{
				if (roundReport.SkillId == 100000)
				{
					normalHurt += roundReport.Value;
				}
				else
				{
					skillHurt += roundReport.Value;
					if (showHurtSkillId <= 0)
					{
						int skillId = roundReport.SkillId;
						string templateData = GameEntry.ConfigCache.GetTemplateData("skill", skillId, "effect_path");
						string templateData2 = GameEntry.ConfigCache.GetTemplateData("skill", skillId, "effect_point_type");
						int num = GameEntry.ConfigCache.GetTemplateData("skill", skillId, "type").ToInt();
						int num2 = 0;
						if (!templateData2.IsNullOrEmpty())
						{
							num2 = templateData2.ToInt();
						}
						if (num == 11 && !templateData.IsNullOrEmpty() && type == (DamageType)num2 && "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab" != templateData)
						{
							showHurtSkillId = skillId;
						}
					}
				}
			}
			if (triggerUuid == armyUuid && roundReport.SkillId == 100000)
			{
				isActiveAttack = true;
			}
			break;
		case DamageType.COUNTER_ATTACK:
			if (targetUuid == armyUuid)
			{
				normalHurt += roundReport.Value;
			}
			break;
		case DamageType.ADD_EFFECT:
			if (targetUuid == armyUuid)
			{
				effectBuffList.Add(roundReport.Value);
			}
			break;
		case DamageType.SHIELD_ATTACK:
		case DamageType.SHIELD:
		case DamageType.RECOVER_DAMAGE:
			break;
		}
	}

	public void UpdateBattle(float deltaTime)
	{
	}

	public WorldMarchDataManager(WorldScene scene)
		: base(scene)
	{
		_messages = new StepProcessQueue<(WorldMarchMessageType, ISFSObject)>(HandleMessage, 2L, 1, 500);
		_messagesMergeDict = new Dictionary<long, (ISFSObject, WorldMarchMessageType)>();
	}

	public void SetWorldScene(WorldScene scene)
	{
		if (scene == null)
		{
			_messages.Clear();
			_messagesMergeDict.Clear();
			allMarchUuids.Clear();
			toMeMarchUuids.Clear();
			toMeMarchTargetUuids.Clear();
			myMarchUuids.Clear();
			if (world != null)
			{
				foreach (long key in allMarches.Keys)
				{
					world.DestroyTroop(key);
					world.DestroyTroopLine(key);
				}
			}
			allMarches.Clear();
			ownerMarches.Clear();
			myAssistancePoint2Uuids.Clear();
			myAssitanceUuid2Points.Clear();
			team2MarchUuid.Clear();
			member2LeaderUuid.Clear();
			allianceMarches.Clear();
			fakeSampleMarches.Clear();
			fakeAttackMonsterMarches.Clear();
			_delayDestroyTroop.Clear();
			_cacheClientCreateGuidAndTimeDict.Clear();
			_marchBattleSound.UnInit();
		}
		else
		{
			_marchBattleSound.Init();
		}
		world = scene;
		ResetStepUpdateMove();
	}

	public override void Init()
	{
		base.Init();
		GameEntry.Event.Subscribe(EventId.LOAD_COMPLETE, OnReloadOrReconnect);
		GameEntry.Event.Subscribe(EventId.APP_APPLICATION_PAUSE, OnApplicationPause);
		lastPerformanceCount = -1;
		lastPerformanceTroopLineCount = -1;
		ResetStepUpdateMove();
	}

	private void OnReloadOrReconnect(object obj)
	{
		if (world != null)
		{
			world.OnChangeServerRemove();
			world.SetFirstViewRequestFlag(isFirstTime: true);
			world.UpdateViewRequest(isForce: true);
		}
	}

	private void OnApplicationPause(object userData)
	{
		object obj;
		if ((obj = userData) is bool && !(bool)obj)
		{
			RefreshMAMTroopPosition();
		}
	}

	private void RefreshMAMTroopPosition()
	{
		foreach (WorldMarch value2 in ownerMarches.Values)
		{
			value2.InitMove(CreatePathSegment(value2));
			if (world != null)
			{
				world.TroopRefreshPosition(value2);
			}
		}
		foreach (long toMeMarchUuid in toMeMarchUuids)
		{
			if (allMarches.TryGetValue(toMeMarchUuid, out var value))
			{
				value.InitMove(CreatePathSegment(value));
				if (world != null)
				{
					world.TroopRefreshPosition(value);
				}
			}
		}
	}

	public override void UnInit()
	{
		base.UnInit();
		targetForMineMarchDic.Clear();
		targetForMineMarchDesertDic.Clear();
		_delayDestroyTroop.Clear();
		_cacheClientCreateGuidAndTimeDict.Clear();
		trainConfigs.Clear();
		flowerCarLength.Clear();
		CleanAllianceMembersHomePos();
		GameEntry.Event.Unsubscribe(EventId.LOAD_COMPLETE, OnReloadOrReconnect);
		GameEntry.Event.Unsubscribe(EventId.APP_APPLICATION_PAUSE, OnApplicationPause);
		_marchBattleSound.UnInit();
		StopRecordMarchBlock();
	}

	private void __ReInit()
	{
		_messages.Clear();
		_messagesMergeDict.Clear();
		allMarchUuids.Clear();
		toMeMarchUuids.Clear();
		toMeMarchTargetUuids.Clear();
		myMarchUuids.Clear();
		if (world != null)
		{
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				world.DestroyTroop(allMarch.Key);
				world.DestroyTroopLine(allMarch.Key);
			}
		}
		allMarches.Clear();
		ownerMarches.Clear();
		myAssistancePoint2Uuids.Clear();
		myAssitanceUuid2Points.Clear();
		team2MarchUuid.Clear();
		member2LeaderUuid.Clear();
		allianceMarches.Clear();
		fakeSampleMarches.Clear();
		fakeAttackMonsterMarches.Clear();
		_delayDestroyTroop.Clear();
		_cacheClientCreateGuidAndTimeDict.Clear();
		targetForMineMarchDic.Clear();
		targetForMineMarchDesertDic.Clear();
		WorldGetMarchInfos();
		ResetStepUpdateMove();
	}

	public override void OnUpdate(float deltaTime)
	{
		UpdateViewRect();
		UpdateMessage();
		UpdateMove(deltaTime);
		UpdateDelayDestroy();
	}

	private void UpdateDelayDestroy()
	{
		if (_delayDestroyTroop.Count <= 0)
		{
			return;
		}
		_toRemoveList.Clear();
		Dictionary<long, long>.Enumerator enumerator = _delayDestroyTroop.GetEnumerator();
		long serverTime = GameEntry.Timer.GetServerTime();
		while (enumerator.MoveNext())
		{
			long key = enumerator.Current.Key;
			long value = enumerator.Current.Value;
			if (serverTime >= value)
			{
				_toRemoveList.Add(key);
				if (world != null)
				{
					world.DestroyTroopLine(key);
					RemoveMarch(key);
				}
				GameEntry.Event.Fire(EventId.HideTroopName, key);
			}
		}
		for (int i = 0; i < _toRemoveList.Count; i++)
		{
			_delayDestroyTroop.Remove(_toRemoveList[i]);
		}
	}

	private void UpdateViewRect()
	{
		if (!(world == null))
		{
			Camera main = Camera.main;
			Vector3 curTarget = world.CurTarget;
			viewBounds.center = curTarget;
			viewBounds.extents = Vector3.one;
			viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(0f, 0f, 0f)));
			viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(0f, main.pixelHeight, 0f)));
			viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(main.pixelWidth, main.pixelHeight, 0f)));
			viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(main.pixelWidth, 0f, 0f)));
			viewBounds.Expand(world.TileSize * 5f);
			viewRect.center = new Vector2(viewBounds.center.x, viewBounds.center.z);
			viewRect.size = new Vector2(viewBounds.size.x, viewBounds.size.z);
		}
	}

	private bool IsInView(Vector3 position)
	{
		return IsRectInView(position, 0);
	}

	private bool IsLineInView(Vector3 start, Vector3 end)
	{
		return IntersectsSegment(viewRect, new Vector2(start.x, start.z), new Vector2(end.x, end.z));
	}

	private bool IsRectInView(Vector3 point, int size)
	{
		if (world == null)
		{
			return false;
		}
		float distance = Mathf.Max(size, 3);
		return world.Camera.InCamera(point.x, point.z, distance);
	}

	private bool IsMarchInView(WorldMarch march)
	{
		if (march.type == NewMarchType.TRAIN)
		{
			return IsLineInView(march.position, march.GetTrainTailPos());
		}
		if (march.type == NewMarchType.ZONE_TRAIN)
		{
			return HSRMarchManager.GetInstance().IsInView(viewRect, march.uuid);
		}
		if (march.IsFlowerCar())
		{
			return IsLineInView(march.position, march.GetFlowerCarTailPos());
		}
		if (march.IsFlowerTrain())
		{
			return IsLineInView(march.position, march.GetFlowerTrainTailPos());
		}
		if (march.type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN)
		{
			return IsLineInView(march.position, march.GetZombieBusTrainTailPos());
		}
		if (march.IsBloodyQueenQueenGunner())
		{
			return IsLineInView(march.position, march.bloodyQueenMonster.standWorldPos);
		}
		return IsRectInView(march.position, march.GetMarchBlockSize());
	}

	private static bool IntersectsSegment(Rect rect, Vector2 p1, Vector2 p2)
	{
		float num = Mathf.Min(p1.x, p2.x);
		float num2 = Mathf.Max(p1.x, p2.x);
		if ((double)num2 > (double)rect.xMax)
		{
			num2 = rect.xMax;
		}
		if ((double)num < (double)rect.xMin)
		{
			num = rect.xMin;
		}
		if ((double)num > (double)num2)
		{
			return false;
		}
		float num3 = Mathf.Min(p1.y, p2.y);
		float num4 = Mathf.Max(p1.y, p2.y);
		float num5 = p2.x - p1.x;
		if ((double)Mathf.Abs(num5) > 1.40129846432482E-45)
		{
			float num6 = (p2.y - p1.y) / num5;
			float num7 = p1.y - num6 * p1.x;
			num3 = num6 * num + num7;
			num4 = num6 * num2 + num7;
		}
		if ((double)num3 > (double)num4)
		{
			float num8 = num4;
			num4 = num3;
			num3 = num8;
		}
		if ((double)num4 > (double)rect.yMax)
		{
			num4 = rect.yMax;
		}
		if ((double)num3 < (double)rect.yMin)
		{
			num3 = rect.yMin;
		}
		return (double)num3 <= (double)num4;
	}

	public static bool AxisAlignRectIntersectAxisAlignSegment(Rect rect, Vector2 p1, Vector2 p2)
	{
		if (Math.Abs(p2.x - p1.x) < 0.01f)
		{
			if (p1.x < rect.xMin || p1.x > rect.xMax)
			{
				return false;
			}
			double num = Math.Min(p1.y, p2.y);
			if ((double)Math.Max(p1.y, p2.y) >= (double)rect.yMin)
			{
				return num <= (double)rect.yMax;
			}
			return false;
		}
		if (Math.Abs(p1.y - p2.y) < 0.01f)
		{
			if (p1.y < rect.yMin || p1.y > rect.yMax)
			{
				return false;
			}
			double num2 = Math.Min(p1.x, p2.x);
			if ((double)Math.Max(p1.x, p2.x) >= (double)rect.xMin)
			{
				return num2 <= (double)rect.xMax;
			}
			return false;
		}
		Log.Error("IsAxisAlignedSegmentIntersect 线段没有轴对齐，请使用IntersectsSegment方法 " + p1.ToString() + ", " + p2.ToString());
		return false;
	}

	private void UpdateLastMarchMessage(WorldMarchMessageType type, ISFSObject message)
	{
		if (!ClientSwitch.IsOn(39))
		{
			return;
		}
		if (type == WorldMarchMessageType.BlockGet)
		{
			if (message.ContainsKey("marchInfos"))
			{
				ISFSArray sFSArray = message.GetSFSArray("marchInfos");
				int count = sFSArray.Count;
				for (int i = 0; i < count; i++)
				{
					long key = sFSArray.GetSFSObject(i).TryGetLong("uuid");
					_messagesMergeDict[key] = (message, type);
				}
			}
		}
		else if (message.ContainsKey("uuid"))
		{
			long key2 = message.TryGetLong("uuid");
			_messagesMergeDict[key2] = (message, type);
		}
	}

	private bool TryExcuteMarchMessage(WorldMarchMessageType type, ISFSObject message)
	{
		if (ClientSwitch.IsOn(39))
		{
			if (type == WorldMarchMessageType.BlockGet)
			{
				if (!message.ContainsKey("marchInfos"))
				{
					return true;
				}
				ISFSArray sFSArray = message.GetSFSArray("marchInfos");
				int count = sFSArray.Count;
				for (int i = 0; i < count; i++)
				{
					long key = sFSArray.GetSFSObject(i).TryGetLong("uuid");
					if (_messagesMergeDict.TryGetValue(key, out (ISFSObject, WorldMarchMessageType) value) && message == value.Item1)
					{
						_messagesMergeDict.Remove(key);
					}
				}
			}
			else
			{
				long key2 = message.TryGetLong("uuid");
				if (_messagesMergeDict.TryGetValue(key2, out (ISFSObject, WorldMarchMessageType) value2) && message != value2.Item1)
				{
					return false;
				}
				_messagesMergeDict.Remove(key2);
			}
		}
		return true;
	}

	private void HandleMessage((WorldMarchMessageType type, ISFSObject obj) message)
	{
		if (TryExcuteMarchMessage(message.type, message.obj))
		{
			switch (message.type)
			{
			case WorldMarchMessageType.BlockGet:
				HandleWorldMarchGetImpl(message.obj);
				break;
			case WorldMarchMessageType.PushAdd:
				HandlePushWorldMarchAddImpl(message.obj);
				break;
			case WorldMarchMessageType.PushDel:
				HandlePushWorldMarchDelImpl(message.obj);
				break;
			case WorldMarchMessageType.Formation:
				HandleFormationMarchImpl(message.obj);
				break;
			case WorldMarchMessageType.FormationUpdate:
				HandleFormationMarchChangeImpl(message.obj);
				break;
			default:
				throw new ArgumentException();
			}
		}
	}

	public void HandleWorldMarchGet(ISFSObject message)
	{
		ISFSArray sFSArray = message.GetSFSArray("serverMarchArr");
		if (sFSArray != null)
		{
			int count = sFSArray.Count;
			SFSObject sFSObject = new SFSObject();
			SFSArray sFSArray2 = new SFSArray();
			bool flag = GameEntry.Data?.Player?.IsInBattleField() ?? false;
			List<long> list = new List<long>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < count; i++)
			{
				ISFSObject sFSObject2 = sFSArray.GetSFSObject(i);
				int item = sFSObject2.GetInt("serverId");
				list2.Add(item);
				long[] longArray = sFSObject2.GetLongArray("uuidSet");
				if (longArray != null)
				{
					long[] array = longArray;
					foreach (long item2 in array)
					{
						list.Add(item2);
					}
				}
				ISFSArray sFSArray3 = sFSObject2.GetSFSArray("marchInfos");
				if (sFSArray3 != null)
				{
					int count2 = sFSArray3.Count;
					for (int k = 0; k < count2; k++)
					{
						ISFSObject sFSObject3 = sFSArray3.GetSFSObject(k);
						sFSArray2.AddSFSObject(sFSObject3);
					}
				}
				if (flag)
				{
					sFSObject.PutBool("marchOptimize", sFSObject2.GetBool("marchOptimize"));
				}
				else
				{
					sFSObject.PutBool("marchOptimize", val: true);
				}
			}
			sFSObject.PutBool("serverMarchArrMode", val: true);
			sFSObject.PutIntArray("serverSet", list2.ToArray());
			sFSObject.PutLongArray("uuidSet", list.ToArray());
			sFSObject.PutSFSArray("marchInfos", sFSArray2);
			if (list2.Count != 0)
			{
				if (ClientSwitch.IsOn(40))
				{
					_messages.Push((WorldMarchMessageType.BlockGet, sFSObject));
					UpdateLastMarchMessage(WorldMarchMessageType.BlockGet, sFSObject);
				}
				else
				{
					HandleWorldMarchGetImpl(sFSObject);
				}
			}
		}
		else if (ClientSwitch.IsOn(40))
		{
			_messages.Push((WorldMarchMessageType.BlockGet, message));
			UpdateLastMarchMessage(WorldMarchMessageType.BlockGet, message);
		}
		else
		{
			HandleWorldMarchGetImpl(message);
		}
	}

	public void HandleWorldMarchGetImpl(ISFSObject message)
	{
		bool num = message.ContainsKey("marchOptimize") && message.GetBool("marchOptimize");
		bool flag = false;
		if (num)
		{
			if (__INC_UPDATE__ < 0)
			{
				__ReInit();
				flag = true;
			}
			__INC_UPDATE__ = 1;
		}
		else
		{
			if (__INC_UPDATE__ > 0)
			{
				__ReInit();
				flag = true;
			}
			__INC_UPDATE__ = -1;
		}
		if (flag && SceneManager.World != null)
		{
			SceneManager.World.SetFirstViewRequestFlag(isFirstTime: true);
			SceneManager.World.UpdateViewRequest(isForce: true);
		}
		if (__INC_UPDATE__ > 0)
		{
			HashSet<long> hashSet = new HashSet<long>(message.GetLongArray("uuidSet"));
			hashSet.UnionWith(myMarchUuids);
			foreach (long item in allMarchUuids.Except(hashSet))
			{
				DestroyMarch(item, isBattleFail: false);
			}
			if (CommonUtils.IsDebug())
			{
				ISFSArray sFSArray = message.GetSFSArray("marchInfos");
				if (sFSArray != null)
				{
					for (int i = 0; i < sFSArray.Count; i++)
					{
						long num2 = sFSArray.GetSFSObject(i).TryGetLong("uuid");
						if (!hashSet.Contains(num2))
						{
							Log.Error($"AOI::Get消息中 marchinfo 不在 uuidset 里：{num2}");
						}
					}
				}
			}
			allMarchUuids = hashSet;
			GameEntry.Event.Fire(EventId.UpdateMarchItem);
		}
		if (message.ContainsKey("marchInfos"))
		{
			ParseDataWorldMarchGet(message);
		}
	}

	private void ParseDataWorldMarchGet(ISFSObject message)
	{
		_tmpMarchSet.Clear();
		ISFSArray sFSArray = message.GetSFSArray("marchInfos");
		int count = sFSArray.Count;
		for (int i = 0; i < count; i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			long num = sFSObject.TryGetLong("uuid");
			_tmpMarchSet.Add(num);
			if (!HasMarchUuid(num))
			{
				continue;
			}
			string value = sFSObject.TryGetString("eventId");
			string text = sFSObject.TryGetString("belongUid");
			if (!string.IsNullOrEmpty(value) && text != GameEntry.Data.Player.Uid)
			{
				continue;
			}
			if (_delayDestroyTroop.ContainsKey(num))
			{
				_delayDestroyTroop.Remove(num);
			}
			if (!GameEntry.Data.Player.GetIsAlreadyBerserkBossReward(num))
			{
				if (!allMarches.TryGetValue(num, out var value2))
				{
					value2 = new WorldMarch();
				}
				else
				{
					CheckToMeMarchDirty(value2);
				}
				value2.UpdateWorldMarch(sFSObject);
				UpdateMarch(value2);
			}
		}
		if (__INC_UPDATE__ > 0)
		{
			bool flag = false;
			_toRemoveList.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				long key = allMarch.Key;
				if (!allMarchUuids.Contains(key) && !_delayDestroyTroop.ContainsKey(key) && !myMarchUuids.Contains(key) && !allMarch.Value.isFake)
				{
					flag = true;
					_toRemoveList.Add(key);
					allMarchUuids.Add(key);
				}
			}
			if (flag)
			{
				if (CommonUtils.IsDebug() && _toRemoveList.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder("AOI::客户端有未收管理的数据：");
					for (int j = 0; j < _toRemoveList.Count; j++)
					{
						stringBuilder.Append(_toRemoveList[j] + ",");
					}
					Log.Error(stringBuilder.ToString());
				}
				if (world != null)
				{
					world.SetFirstViewRequestFlag(isFirstTime: true);
				}
			}
		}
		else
		{
			_toRemoveList.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch2 in allMarches)
			{
				if (!IsFakeSampleMarchData(allMarch2.Key) && !IsFakeAttackMonsterMarchData(allMarch2.Key) && !NeedReserveFakeRetreatMarchData(allMarch2.Value) && !_tmpMarchSet.Contains(allMarch2.Key) && !toMeMarchUuids.Contains(allMarch2.Key) && !myMarchUuids.Contains(allMarch2.Key))
				{
					_toRemoveList.Add(allMarch2.Key);
				}
			}
			foreach (long toRemove in _toRemoveList)
			{
				DestroyMarch(toRemove, isBattleFail: false);
			}
		}
		FireMyOrToMeMarchRefresh();
	}

	public void HandleWorldGetRectMarchInfos(ISFSObject message)
	{
		_messages.Clear();
		_messagesMergeDict.Clear();
		HashSet<long> hashSet = new HashSet<long>();
		hashSet.UnionWith(toMeMarchUuids);
		hashSet.UnionWith(myMarchUuids);
		toMeMarchUuids.Clear();
		toMeMarchTargetUuids.Clear();
		myMarchUuids.Clear();
		ownerMarches.Clear();
		myAssistancePoint2Uuids.Clear();
		myAssitanceUuid2Points.Clear();
		team2MarchUuid.Clear();
		member2LeaderUuid.Clear();
		toMeMarchDirty = true;
		toMeMarchNeedUpdate = true;
		toMeMarchDesertNeedUpdate = true;
		myMarchDirty = true;
		if (message.ContainsKey("marchInfos"))
		{
			ISFSArray sFSArray = message.GetSFSArray("marchInfos");
			int count = sFSArray.Count;
			_tmpMarchList.Clear();
			for (int i = 0; i < count; i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				long num = sFSObject.TryGetLong("uuid");
				hashSet.Remove(num);
				string value = sFSObject.TryGetString("eventId");
				string text = sFSObject.TryGetString("belongUid");
				if (string.IsNullOrEmpty(value) || !(text != GameEntry.Data.Player.Uid))
				{
					if (_delayDestroyTroop.ContainsKey(num))
					{
						_delayDestroyTroop.Remove(num);
					}
					if (!allMarches.TryGetValue(num, out var value2))
					{
						value2 = new WorldMarch();
					}
					value2.UpdateWorldMarch(sFSObject);
					if (IsMyMarch(value2) || IsTargetForMine(value2))
					{
						UpdateMarch(value2);
					}
					else
					{
						_tmpMarchList.Add(value2);
					}
				}
			}
			foreach (WorldMarch tmpMarch in _tmpMarchList)
			{
				if (IsMyJoinAssemblyMarch(tmpMarch))
				{
					UpdateMarch(tmpMarch);
				}
			}
		}
		foreach (long item in hashSet)
		{
			DestroyMarch(item, isBattleFail: false);
		}
		FireMyOrToMeMarchRefresh();
	}

	private void BeforeFireMyOrToMeMarchRefresh()
	{
		toMeMarchTargetUuids.Clear();
		foreach (long toMeMarchUuid in toMeMarchUuids)
		{
			if (allMarches.TryGetValue(toMeMarchUuid, out var value) && value != null && value.targetUuid > 0)
			{
				toMeMarchTargetUuids[value.targetUuid] = value.targetPos;
			}
		}
	}

	public bool CheckIsOtherTarget(long uuid, int targetPos)
	{
		if (!toMeMarchTargetUuids.TryGetValue(uuid, out var value))
		{
			return false;
		}
		return value == targetPos;
	}

	private void FireMyOrToMeMarchRefresh()
	{
		if (toMeMarchDirty)
		{
			BeforeFireMyOrToMeMarchRefresh();
			GameEntry.Event.Fire(EventId.MarchItemTargetMeUpdate);
			toMeMarchDirty = false;
		}
		if (!myMarchDirty)
		{
			return;
		}
		GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
		myMarchDirty = false;
		if (MultiKillSwitch != true)
		{
			return;
		}
		foreach (WorldMarch value in ownerMarches.Values)
		{
			if (value.ownerUid == GameEntry.Data.Player.Uid)
			{
				if (MultiKillPVEMin <= value.pveNum && GetMyMarchMultiKillPVE(value.uuid) < value.pveNum)
				{
					myMarchMultiKillPVE[value.uuid] = value.pveNum;
					GameEntry.Event.Fire(EventId.MyMarchMultiKillPVEAdd, value.uuid);
				}
				if (MultiKillPVPMin <= value.pvpNum && GetMyMarchMultiKillPVP(value.uuid) < value.pvpNum)
				{
					myMarchMultiKillPVP[value.uuid] = value.pvpNum;
					GameEntry.Event.Fire(EventId.MyMarchMultiKillPVPAdd, value.uuid);
				}
			}
		}
	}

	public void HandlePushWorldMarchAdd(ISFSObject message)
	{
		if (ClientSwitch.IsOn(40))
		{
			_messages.Push((WorldMarchMessageType.PushAdd, message));
			UpdateLastMarchMessage(WorldMarchMessageType.PushAdd, message);
		}
		else
		{
			HandlePushWorldMarchAddImpl(message);
		}
	}

	public void HandlePushWorldMarchAddImpl(ISFSObject message)
	{
		if (!message.ContainsKey("uuid"))
		{
			return;
		}
		if (__INC_UPDATE__ > 0)
		{
			long item = message.GetLong("uuid");
			if (!allMarchUuids.Contains(item))
			{
				allMarchUuids.Add(item);
			}
		}
		ParsePushWorldMarchAdd(message, WorldMarchMessageType.PushAdd);
	}

	public void ParsePushWorldMarchAdd(ISFSObject message, WorldMarchMessageType msgType)
	{
		long num = message.TryGetLong("uuid");
		if (!HasMarchUuid(num))
		{
			return;
		}
		string value = message.TryGetString("eventId");
		string text = message.TryGetString("belongUid");
		if (!string.IsNullOrEmpty(value) && text != GameEntry.Data.Player.Uid)
		{
			return;
		}
		if (_delayDestroyTroop.ContainsKey(num))
		{
			_delayDestroyTroop.Remove(num);
		}
		if (GameEntry.Data.Player.GetIsAlreadyBerserkBossReward(num))
		{
			return;
		}
		bool flag = false;
		if (!allMarches.TryGetValue(num, out var value2))
		{
			value2 = new WorldMarch();
			value2.UpdateWorldMarch(message, isPushAdd: true);
		}
		else
		{
			CheckToMeMarchDirty(value2);
			flag = value2.IsFrozen();
			value2.UpdateWorldMarch(message);
		}
		UpdateMarch(value2);
		if (world != null)
		{
			world.CreateTroopLine(value2);
			if (flag && !value2.IsFrozen())
			{
				world.OnMonsterIceBroken(value2.uuid);
			}
		}
		FireMyOrToMeMarchRefresh();
		GameEntry.Event.Fire(EventId.SingleMarchStateUpdate, value2.uuid);
		if (msgType == WorldMarchMessageType.Formation)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.OnLaunchMarchSuccess", value2.GetMarchTargetType(), value2.targetUuid, value2.targetPos);
		}
	}

	public void HandlePushWorldMarchDel(ISFSObject message)
	{
		if (ClientSwitch.IsOn(40))
		{
			_messages.Push((WorldMarchMessageType.PushDel, message));
			UpdateLastMarchMessage(WorldMarchMessageType.PushDel, message);
		}
		else
		{
			HandlePushWorldMarchDelImpl(message);
		}
	}

	public void HandlePushWorldMarchDelImpl(ISFSObject message)
	{
		if (message.ContainsKey("uuid"))
		{
			if (__INC_UPDATE__ > 0)
			{
				long item = message.GetLong("uuid");
				allMarchUuids.Remove(item);
			}
			long marchUuid = message.TryGetLong("uuid");
			bool isBattleFail = message.GetBool("isBattleFail");
			DestroyMarch(marchUuid, isBattleFail);
			FireMyOrToMeMarchRefresh();
		}
	}

	public void HandleFormationMarch(ISFSObject message)
	{
		if (ClientSwitch.IsOn(40))
		{
			_messages.Push((WorldMarchMessageType.Formation, message));
			UpdateLastMarchMessage(WorldMarchMessageType.Formation, message);
		}
		else
		{
			HandleFormationMarchImpl(message);
		}
	}

	public void HandleFormationMarchImpl(ISFSObject message)
	{
		if (GameEntry.Data.Player.GetSourceServerId() == 180)
		{
			Log.Info("HandleFormationMarch.receiveMsg");
		}
		ParseFormationUpdate(message, WorldMarchMessageType.Formation);
	}

	public void HandleFormationMarchChange(ISFSObject message)
	{
		if (ClientSwitch.IsOn(40))
		{
			_messages.Push((WorldMarchMessageType.FormationUpdate, message));
			UpdateLastMarchMessage(WorldMarchMessageType.FormationUpdate, message);
		}
		else
		{
			HandleFormationMarchChangeImpl(message);
		}
	}

	public void HandleFormationMarchChangeImpl(ISFSObject message)
	{
		ParseFormationUpdate(message, WorldMarchMessageType.FormationUpdate);
	}

	private void ParseFormationUpdate(ISFSObject message, WorldMarchMessageType msgType)
	{
		if (!message.ContainsKey("uuid"))
		{
			return;
		}
		if (__INC_UPDATE__ > 0)
		{
			long item = message.GetLong("uuid");
			if (!allMarchUuids.Contains(item))
			{
				allMarchUuids.Add(item);
			}
		}
		ParsePushWorldMarchAdd(message, msgType);
		if (message.ContainsKey("resource"))
		{
			ISFSObject sFSObject = message.GetSFSObject("resource");
			GameEntry.Lua.Call("LuaEntry.Resource:UpdateResource", ((SFSObject)sFSObject).ToLuaTable(GameEntry.Lua.Env));
		}
		FireMyOrToMeMarchRefresh();
	}

	public void DelFakeAttackMonsterMarchDataRandom(int count)
	{
		if (count >= fakeAttackMonsterMarches.Count)
		{
			foreach (KeyValuePair<long, WorldMarch> fakeAttackMonsterMarch in fakeAttackMonsterMarches)
			{
				DestroyMarch(fakeAttackMonsterMarch.Key, isBattleFail: false);
			}
			fakeRetreatMarches.Clear();
			return;
		}
		List<long> list = new List<long>(fakeAttackMonsterMarches.Keys);
		while (count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			fakeRetreatMarches.Remove(list[index]);
			DestroyMarch(list[index], isBattleFail: false);
			list.RemoveAt(index);
			count--;
		}
	}

	public void AddFakeAttackMonsterMarchData(long startIndex, long endIndex, float marchTimeSec, string ownerUid)
	{
		if (!(world == null))
		{
			DCPlayer player = GameEntry.Data.Player;
			WorldMarch worldMarch = new WorldMarch();
			worldMarch.startTime = GameEntry.Timer.GetServerTime();
			worldMarch.endTime = worldMarch.startTime + (long)(marchTimeSec * 1000f);
			worldMarch.fakeMarchTime = worldMarch.endTime - worldMarch.startTime;
			worldMarch.type = NewMarchType.NORMAL;
			worldMarch.target = MarchTargetType.ATTACK_MONSTER;
			worldMarch.status = MarchStatus.MOVING;
			worldMarch.uuid = System.DateTime.Now.Ticks;
			worldMarch.ownerUid = ownerUid;
			worldMarch.ownerName = ((ownerUid == player.GetUid()) ? player.GetName() : "Fake Attack March");
			worldMarch.pic = "";
			worldMarch.picVer = 0;
			worldMarch.allianceUid = ((ownerUid == player.GetUid()) ? player.GetAllianceId() : string.Empty);
			worldMarch.allianceAbbr = "";
			worldMarch.allianceIcon = "";
			PointInfo pointInfo = world.GetPointInfo((int)endIndex);
			if (pointInfo != null)
			{
				worldMarch.targetUuid = pointInfo.uuid;
			}
			worldMarch.targetServer = player.GetCurServerId();
			worldMarch.srcServer = player.GetCurServerId();
			worldMarch.startPos = (int)startIndex;
			worldMarch.targetPos = (int)endIndex;
			worldMarch.pathStartServerId = worldMarch.srcServer;
			worldMarch.targetWorldPos = TileCoord.TileIndexToWorld(worldMarch.targetPos, ForceChangeScene.World, worldMarch.targetServer);
			worldMarch.startWorldPos = TileCoord.TileIndexToWorld(worldMarch.startPos, ForceChangeScene.World, worldMarch.srcServer);
			Vector2Int item = world.IndexToTilePos(worldMarch.startPos);
			Vector2Int item2 = world.IndexToTilePos(worldMarch.targetPos);
			double num = Math.Sqrt(Math.Pow(item.x - item2.x, 2.0) + Math.Pow(item.y - item2.y, 2.0));
			worldMarch.speed = (float)num * 1000f / (float)worldMarch.fakeMarchTime;
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { item, item2 });
			worldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			fakeAttackMonsterMarches[worldMarch.uuid] = worldMarch;
			AddMarch(worldMarch);
			ArmyInfo armyInfo = new ArmyInfo();
			worldMarch.armyInfos.Add(armyInfo);
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.RandomHeroIdsAndTacWeaponInfo");
			HeroInfo heroInfo = new HeroInfo();
			heroInfo.index = 1;
			heroInfo.heroId = luaTable.Get<int>("heroId_1");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 2;
			heroInfo.heroId = luaTable.Get<int>("heroId_2");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 3;
			heroInfo.heroId = luaTable.Get<int>("heroId_3");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 4;
			heroInfo.heroId = luaTable.Get<int>("heroId_4");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 5;
			heroInfo.heroId = luaTable.Get<int>("heroId_5");
			armyInfo.HeroInfos.Add(heroInfo);
			TacWeaponInfo tacWeaponInfo = new TacWeaponInfo();
			tacWeaponInfo.weaponId = luaTable.Get<int>("weaponId");
			tacWeaponInfo.weaponLevel = luaTable.Get<int>("weaponLv");
			armyInfo.WeaponInfo = tacWeaponInfo;
			worldMarch.isFake = true;
			worldMarch.isFakeAttack = true;
			worldMarch.IsValid = true;
		}
	}

	public void UpdateFakeAttackMonsterMarch(long uuid)
	{
		if (world == null || !fakeAttackMonsterMarches.TryGetValue(uuid, out var value))
		{
			return;
		}
		if (value.target == MarchTargetType.BACK_HOME)
		{
			allMarches.Remove(uuid);
			fakeAttackMonsterMarches.Remove(uuid);
			value.IsValid = false;
			world.DestroyTroopLine(uuid);
			return;
		}
		DCPlayer player = GameEntry.Data.Player;
		int targetPos = value.targetPos;
		value.targetServer = player.GetCurServerId();
		value.srcServer = player.GetCurServerId();
		value.pathStartServerId = value.srcServer;
		value.targetPos = value.startPos;
		value.startPos = targetPos;
		value.status = MarchStatus.MOVING;
		value.target = MarchTargetType.BACK_HOME;
		value.startTime = GameEntry.Timer.GetServerTime();
		value.endTime = value.startTime + value.fakeMarchTime;
		Vector2Int item = world.IndexToTilePos(value.startPos);
		Vector2Int item2 = world.IndexToTilePos(value.targetPos);
		double num = Math.Sqrt(Math.Pow(item.x - item2.x, 2.0) + Math.Pow(item.y - item2.y, 2.0));
		value.speed = (float)num * 1000f / (float)value.fakeMarchTime;
		string text = WorldPathfinding.PathToString(new List<Vector2Int> { item, item2 });
		value.path = (from a in text.Split(new char[1] { ';' })
			select a.ToInt()).ToArray();
		GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
	}

	private long GenerateFakeUuid()
	{
		return --_fakeUuid;
	}

	public void AddFakeRetreatMarchData(WorldMarch realMarch, PointInfo info, int timeDelta)
	{
		if (!(world == null))
		{
			DCPlayer player = GameEntry.Data.Player;
			WorldMarch worldMarch = new WorldMarch();
			worldMarch.parentUuid = realMarch.uuid;
			worldMarch.allianceAbbr = realMarch.allianceAbbr;
			worldMarch.allianceName = realMarch.allianceName;
			worldMarch.allianceIcon = realMarch.allianceIcon;
			int num = UnityEngine.Random.Range(0, timeDelta);
			worldMarch.startTime = realMarch.startTime + num;
			worldMarch.endTime = realMarch.endTime + num;
			worldMarch.cityId = realMarch.cityId;
			worldMarch.type = NewMarchType.ZOMBIE_RETREAT;
			worldMarch.target = MarchTargetType.NORMAL_FAKE_MARCH;
			worldMarch.status = MarchStatus.MOVING;
			worldMarch.uuid = GenerateFakeUuid();
			worldMarch.pic = "";
			worldMarch.picVer = 0;
			if (info != null)
			{
				worldMarch.targetUuid = info.uuid;
			}
			worldMarch.targetServer = player.GetCurServerId();
			worldMarch.srcServer = player.GetCurServerId();
			worldMarch.pathStartServerId = worldMarch.srcServer;
			worldMarch.startPos = realMarch.startPos;
			worldMarch.targetPos = realMarch.targetPos;
			worldMarch.targetWorldPos = TileCoord.TileIndexToWorld(worldMarch.targetPos, ForceChangeScene.World, worldMarch.targetServer);
			worldMarch.startWorldPos = TileCoord.TileIndexToWorld(worldMarch.startPos, ForceChangeScene.World, worldMarch.srcServer);
			Vector2Int item = world.IndexToTilePos(worldMarch.startPos);
			Vector2Int item2 = world.IndexToTilePos(worldMarch.targetPos);
			worldMarch.speed = realMarch.speed;
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { item, item2 });
			worldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			if (!fakeRetreatMarches.ContainsKey(realMarch.uuid))
			{
				fakeRetreatMarches[realMarch.uuid] = new List<WorldMarch>();
			}
			fakeRetreatMarches[realMarch.uuid].Add(worldMarch);
			AddMarch(worldMarch);
			worldMarch.isFake = true;
		}
	}

	public void RemoveFakeRetreatMarchData(long realUuid)
	{
		if (world == null || !fakeRetreatMarches.TryGetValue(realUuid, out var value))
		{
			return;
		}
		foreach (WorldMarch item in value)
		{
			long uuid = item.uuid;
			world.DestroyTroop(uuid);
			world.DestroyTroopLine(uuid);
			allMarches.Remove(uuid);
		}
		fakeRetreatMarches[realUuid].Clear();
		fakeRetreatMarches.Remove(realUuid);
	}

	private bool NeedReserveFakeRetreatMarchData(WorldMarch march)
	{
		if (march.type == NewMarchType.ZOMBIE_RETREAT)
		{
			if (march.isFake)
			{
				if (allMarches.ContainsKey(march.parentUuid))
				{
					return true;
				}
				if (fakeRetreatMarches.ContainsKey(march.parentUuid))
				{
					fakeRetreatMarches[march.parentUuid].Clear();
					fakeRetreatMarches.Remove(march.parentUuid);
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void AddFakeSampleMarchData(long startIndex, long endIndex, long startTime, long endTime, int marchTargetType, int srcServer = -1, int targetServer = -1)
	{
		if (!(world == null) && !IsFakeSampleMarchData(endIndex))
		{
			DCPlayer player = GameEntry.Data.Player;
			PointInfo pointInfo = world.GetPointInfo((int)endIndex);
			WorldMarch worldMarch = new WorldMarch();
			worldMarch.endTime = endTime;
			worldMarch.allianceUid = player.GetAllianceId();
			worldMarch.allianceAbbr = "";
			worldMarch.allianceIcon = "";
			worldMarch.startTime = startTime;
			worldMarch.type = NewMarchType.NORMAL;
			worldMarch.target = (MarchTargetType)marchTargetType;
			worldMarch.status = MarchStatus.MOVING;
			worldMarch.uuid = endIndex;
			worldMarch.pic = "";
			worldMarch.picVer = 0;
			if (pointInfo != null)
			{
				worldMarch.targetUuid = pointInfo.uuid;
			}
			worldMarch.targetServer = ((targetServer > 0) ? targetServer : player.GetCurServerId());
			worldMarch.srcServer = ((srcServer > 0) ? srcServer : player.GetCurServerId());
			worldMarch.serverId = worldMarch.srcServer;
			worldMarch.pathStartServerId = worldMarch.srcServer;
			worldMarch.ownerName = player.GetName();
			worldMarch.ownerUid = player.GetUid();
			worldMarch.startPos = (int)startIndex;
			worldMarch.targetPos = (int)endIndex;
			worldMarch.targetWorldPos = TileCoord.TileIndexToWorld(worldMarch.targetPos, ForceChangeScene.World, worldMarch.targetServer);
			worldMarch.startWorldPos = TileCoord.TileIndexToWorld(worldMarch.startPos, ForceChangeScene.World, worldMarch.srcServer);
			Vector2Int vector2Int = world.IndexToTilePos(worldMarch.startPos);
			Vector2Int vector2Int2 = world.IndexToTilePos(worldMarch.targetPos);
			double num = Math.Sqrt(Math.Pow(vector2Int.x - vector2Int2.x, 2.0) + Math.Pow(vector2Int.y - vector2Int2.y, 2.0));
			worldMarch.speed = (float)num * 1000f / (float)(endTime - startTime);
			if (worldMarch.target == MarchTargetType.SAMPLE)
			{
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 0f);
			}
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
			worldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			fakeSampleMarches[endIndex] = worldMarch;
			AddMarch(worldMarch);
			worldMarch.isFake = true;
			worldMarch.IsValid = true;
		}
	}

	public Dictionary<long, WorldMarch> GetAllSampleFakeData()
	{
		return fakeSampleMarches;
	}

	public void UpdateFakeSampleMarchDataWhenStartPick(long index, long endTime)
	{
		if (IsFakeSampleMarchData(index))
		{
			WorldMarch worldMarch = fakeSampleMarches[index];
			worldMarch.startTime = worldMarch.endTime;
			worldMarch.endTime = endTime;
			worldMarch.status = MarchStatus.SAMPLING;
			GameEntry.Event.Fire(EventId.GarbageCollectStart, worldMarch.targetUuid);
			GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
		}
	}

	public void UpdateFakeSampleMarchDataWhenBack(long index, long startTime, long endTime)
	{
		if (!(world == null) && IsFakeSampleMarchData(index))
		{
			WorldMarch worldMarch = fakeSampleMarches[index];
			MarchTargetType target = worldMarch.target;
			worldMarch.targetPos = worldMarch.startPos;
			worldMarch.startPos = (int)index;
			worldMarch.status = MarchStatus.MOVING;
			worldMarch.target = MarchTargetType.BACK_HOME;
			worldMarch.startTime = startTime;
			worldMarch.endTime = endTime;
			Vector2Int vector2Int = world.IndexToTilePos(worldMarch.startPos);
			Vector2Int vector2Int2 = world.IndexToTilePos(worldMarch.targetPos);
			double num = Math.Sqrt(Math.Pow(vector2Int.x - vector2Int2.x, 2.0) + Math.Pow(vector2Int.y - vector2Int2.y, 2.0));
			worldMarch.speed = (float)num * 1000f / (float)(endTime - startTime);
			if (target == MarchTargetType.SAMPLE)
			{
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 0f);
			}
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
			worldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
		}
	}

	public void RemoveFakeSampleMarchData(long index)
	{
		if (!(world == null) && IsFakeSampleMarchData(index))
		{
			world.DestroyTroop(index);
			world.DestroyTroopLine(index);
			fakeSampleMarches.Remove(index);
			RemoveMarch(index);
		}
	}

	private bool IsFakeSampleMarchData(long index)
	{
		return fakeSampleMarches.ContainsKey(index);
	}

	private bool IsFakeAttackMonsterMarchData(long uuid)
	{
		return fakeAttackMonsterMarches.ContainsKey(uuid);
	}

	public bool ExistMarch(long uuid)
	{
		return allMarches.ContainsKey(uuid);
	}

	public bool IsInRallyMarch(long uuid)
	{
		bool result = false;
		if (allMarches.ContainsKey(uuid))
		{
			WorldMarch worldMarch = allMarches[uuid];
			if (worldMarch.status == MarchStatus.IN_TEAM || worldMarch.status == MarchStatus.WAIT_RALLY)
			{
				result = true;
			}
		}
		return result;
	}

	public bool IsInCollectMarch(long uuid)
	{
		bool result = false;
		if (allMarches.ContainsKey(uuid) && allMarches[uuid].status == MarchStatus.COLLECTING)
		{
			result = true;
		}
		return result;
	}

	public bool IsInAssistanceMarch(long uuid)
	{
		bool result = false;
		if (allMarches.ContainsKey(uuid) && allMarches[uuid].status == MarchStatus.ASSISTANCE)
		{
			result = true;
		}
		return result;
	}

	public bool IsSelfInCurrentMarchTeam(long rallyMarchUuid)
	{
		WorldMarch march = GetMarch(rallyMarchUuid);
		if (march == null)
		{
			return false;
		}
		if (march.IsMonsterOrBoss())
		{
			return false;
		}
		if (march.ownerUid.IsNullOrEmpty())
		{
			return false;
		}
		if (march.ownerUid == GameEntry.Data.Player.Uid)
		{
			return true;
		}
		long teamUuid = march.teamUuid;
		if (teamUuid <= 0)
		{
			return false;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		List<WorldMarch> list = GetOwnerMarches(GameEntry.Data.Player.Uid, allianceId);
		if (list != null)
		{
			foreach (WorldMarch item in list)
			{
				if (teamUuid == item.teamUuid)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasMarchUuid(long uuid)
	{
		if (__INC_UPDATE__ > 0)
		{
			return allMarchUuids.Contains(uuid);
		}
		return true;
	}

	public WorldMarch GetMarch(long uuid)
	{
		allMarches.TryGetValue(uuid, out var value);
		return value;
	}

	public WorldMarch GetMonster(long targetPoint)
	{
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (allMarch.Value.IsMonsterOrOrdinaryBoss() && allMarch.Value.IsVisibleMarch() && allMarch.Value.startPos == targetPoint && allMarch.Value.startPos == allMarch.Value.targetPos)
			{
				return allMarch.Value;
			}
		}
		return null;
	}

	public List<WorldMarch> GetOwnerMarches(string ownerUid = "", string allianceUid = "")
	{
		List<WorldMarch> list = new List<WorldMarch>();
		list.AddRange(ownerMarches.Values);
		return list;
	}

	public List<WorldMarch> GetMyDisguiseMarches(string ownerUid = "", string allianceUid = "")
	{
		List<WorldMarch> list = new List<WorldMarch>();
		foreach (WorldMarch value in ownerMarches.Values)
		{
			if (value.type == NewMarchType.FAKE_ATTACK)
			{
				list.Add(value);
			}
		}
		return list;
	}

	public WorldMarch GetAllianceMarchesInTeam(string allianceUid, long teamUuid)
	{
		if (!allianceUid.IsNullOrEmpty() && allianceMarches.ContainsKey(allianceUid))
		{
			foreach (WorldMarch item in allianceMarches[allianceUid])
			{
				if (item.teamUuid == teamUuid)
				{
					return item;
				}
			}
		}
		if (GameEntry.Data.Player.IsInBattleField(3))
		{
			string text = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetEpidemicTeammateByAllianceId", allianceUid);
			if (!text.IsNullOrEmpty() && allianceMarches.ContainsKey(text))
			{
				foreach (WorldMarch item2 in allianceMarches[text])
				{
					if (item2.teamUuid == teamUuid)
					{
						return item2;
					}
				}
			}
		}
		return null;
	}

	public bool IsHaveMarchInWorld(string allianceUid, long teamUuid)
	{
		foreach (WorldMarch ownerMarch in GetOwnerMarches())
		{
			if (ownerMarch.status != MarchStatus.DEFAULT && ownerMarch.type != NewMarchType.SCOUT && ownerMarch.type != NewMarchType.TREAT_VIRUS && ownerMarch.type != NewMarchType.LOTTO_RECEIVE && ownerMarch.type != NewMarchType.TRAIN && ownerMarch.type != NewMarchType.ZONE_MOBILIZATION_DONATE && ownerMarch.type != NewMarchType.MONSTER_CHALLENGE_DONATE)
			{
				return true;
			}
		}
		return false;
	}

	public WorldMarch GetBestMarch(int pointId, string allianceId, int worldId)
	{
		WorldMarch worldMarch = null;
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.worldId == worldId && value.targetPos == pointId && (allianceId.IsNullOrEmpty() || value.allianceUid == allianceId) && (worldMarch == null || worldMarch.power < value.power))
			{
				worldMarch = value;
			}
		}
		return worldMarch;
	}

	public void CleanDragonWar()
	{
		List<long> list = new List<long>();
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.worldId > 0)
			{
				list.Add(value.uuid);
			}
		}
		foreach (long item in list)
		{
			DestroyMarch(item, isBattleFail: false);
		}
	}

	public WorldMarch GetOwnerFormationMarch(string ownerUid, long formationUuid, string allianceUid = "")
	{
		return GetOwnerMarches(ownerUid, allianceUid)?.Find((WorldMarch m) => m.ownerFormationUuid == formationUuid);
	}

	public void StartMarch(int targetType, int targetPoint, long targetUuid, int timeIndex, long marchUuid = 0L, long formationUuid = 0L, int backHome = 1, byte[] sfsObjBinary = null, int startPos = 0, int targetServerId = -1)
	{
		if (targetType == 2)
		{
			GameEntry.Event.Fire(EventId.UIMAIN_VISIBLE, true);
			if (world != null)
			{
				world.AutoLookat(SceneManager.World.CurTarget, SceneManager.World.InitZoom, 0.4f);
			}
		}
		if (formationUuid != 0L)
		{
			string allianceId = GameEntry.Data.Player.GetAllianceId();
			WorldMarch ownerFormationMarch = GetOwnerFormationMarch(GameEntry.Data.Player.Uid, formationUuid, allianceId);
			if (ownerFormationMarch != null)
			{
				SendChangeMarchToServer(ownerFormationMarch.uuid, targetType, targetPoint, targetUuid, backHome == 1, targetServerId);
				return;
			}
			SFSObject formationData = null;
			if (sfsObjBinary != null)
			{
				formationData = SFSObject.NewFromBinaryData(new ByteArray(sfsObjBinary));
			}
			SendCreateMarchToServer(formationUuid, targetType, targetPoint, targetUuid, timeIndex, formationData, startPos, backHome == 1, targetServerId);
		}
		else if (marchUuid != 0L)
		{
			SendChangeMarchToServer(marchUuid, targetType, targetPoint, targetUuid, backHome == 1, targetServerId);
		}
	}

	private static Vector2Int GetAttackPos(Vector2Int start, Vector2Int end, float attackOffsetRange)
	{
		float num = Vector2Int.Distance(start, end);
		float num2 = 1f - attackOffsetRange / num;
		return new Vector2Int(Mathf.RoundToInt((float)(end.x - start.x) * num2 + (float)start.x), Mathf.RoundToInt((float)(end.y - start.y) * num2 + (float)start.y));
	}

	private void SendCreateMarchToServer(long formationUuid, int targetType, int targetPoint, long targetUuid, int timeIndex, SFSObject formationData, int startPos, bool backHome = true, int targetServerId = -1)
	{
		int index = startPos;
		if (startPos <= 0)
		{
			LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
			if (buildingDataByBuildId != null)
			{
				index = buildingDataByBuildId.pointId;
			}
		}
		Vector2Int vector2Int = TileCoord.IndexToTilePos(index, ForceChangeScene.World);
		Vector2Int vector2Int2 = TileCoord.IndexToTilePos(targetPoint, ForceChangeScene.World);
		if (targetType == 5 || targetType == 1 || targetType == 4 || targetType == 20 || targetType == 21 || targetType == 41 || targetType == 46 || targetType == 119 || targetType == 43 || targetType == 44 || targetType == 23)
		{
			vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
		}
		else
		{
			switch (targetType)
			{
			case 7:
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
				break;
			case 25:
			case 27:
			case 41:
			case 43:
			case 71:
			case 72:
			{
				Vector2Int end = vector2Int2 + new Vector2Int(-3, -3);
				vector2Int2 = GetAttackPos(vector2Int, end, 5f);
				break;
			}
			}
		}
		int worldId = GameEntry.Data.Player.GetWorldId();
		string path = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
		WorldMarchFormationMessage.Instance.Send(new WorldMarchFormationMessage.Request
		{
			formationUuid = formationUuid,
			path = path,
			targetUid = targetUuid,
			worldId = worldId,
			target = targetType,
			waitTimeIndex = timeIndex,
			autoBackHome = backHome,
			formationParam = formationData,
			targetServerId = targetServerId
		});
	}

	private void SendChangeMarchToServer(long marchUuid, int targetType, int targetPoint, long targetUuid, bool backHome = true, int targetServerId = -1)
	{
		WorldMarch march = GetMarch(marchUuid);
		if (march == null)
		{
			return;
		}
		int num = 0;
		if (march.status == MarchStatus.COLLECTING || march.status == MarchStatus.ASSISTANCE)
		{
			num = march.targetPos;
			if (world != null && march.status == MarchStatus.COLLECTING)
			{
				world.DestroyArmyAnimalObject(marchUuid);
			}
		}
		else
		{
			num = TileCoord.WorldToTileIndex(march.position, ForceChangeScene.World);
		}
		Vector2Int vector2Int = TileCoord.IndexToTilePos(num, ForceChangeScene.World);
		Vector2Int vector2Int2 = TileCoord.IndexToTilePos(targetPoint, ForceChangeScene.World);
		if (targetType == 5 || targetType == 1 || targetType == 4 || targetType == 20 || targetType == 21 || targetType == 41 || targetType == 46 || targetType == 119 || targetType == 43 || targetType == 44 || targetType == 23)
		{
			vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
		}
		else
		{
			switch (targetType)
			{
			case 7:
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
				break;
			case 25:
			case 27:
			case 41:
			case 43:
			case 71:
			case 72:
			{
				Vector2Int end = vector2Int2 + new Vector2Int(-3, -3);
				vector2Int2 = GetAttackPos(vector2Int, end, 5f);
				break;
			}
			}
		}
		int worldId = GameEntry.Data.Player.GetWorldId();
		string path = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
		WorldMarchFormationChangeMessage.Instance.Send(new WorldMarchFormationChangeMessage.Request
		{
			uuid = marchUuid,
			path = path,
			targetUid = targetUuid,
			worldId = worldId,
			target = targetType,
			autoBackHome = backHome,
			targetServerId = targetServerId
		});
	}

	public void WorldGetMarchInfos()
	{
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		if (worldMainPos > 0)
		{
			Vector2Int vector2Int = TileCoord.IndexToTilePos(worldMainPos, ForceChangeScene.World);
			WorldGetRectMarchInfosMessage.Instance.Send(new WorldGetRectMarchInfosMessage.Request
			{
				x = vector2Int.x,
				y = vector2Int.y
			});
		}
	}

	private bool NeedGetRealTargetPos(WorldMarch march)
	{
		if (march.target == MarchTargetType.ATTACK_BUILDING)
		{
			return true;
		}
		if (march.target == MarchTargetType.ATTACK_ARMY || march.target == MarchTargetType.ATTACK_MONSTER || march.target == MarchTargetType.RALLY_FOR_BOSS || march.target == MarchTargetType.EXPLORE || march.target == MarchTargetType.SAMPLE || march.target == MarchTargetType.RALLY_THRONE || march.target == MarchTargetType.RALLY_DRAGON_BUILDING || march.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || march.target == MarchTargetType.ATTACK_THRONE || march.target == MarchTargetType.ASSISTANCE_THRONE || march.target == MarchTargetType.DETECT_TREASURE || march.target == MarchTargetType.PICK_GARBAGE || march.target == MarchTargetType.RALLY_FOR_BUILDING || march.target == MarchTargetType.RALLY_EPIDEMIC_CITY)
		{
			return true;
		}
		return false;
	}

	private int GetRealMarchTargetPos(WorldMarch march)
	{
		if (world == null)
		{
			return 0;
		}
		if (march.target == MarchTargetType.ATTACK_BUILDING || march.target == MarchTargetType.RALLY_FOR_BUILDING)
		{
			PointInfo pointInfoByUuid = world.GetPointInfoByUuid(march.targetUuid);
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding)
			{
				return pointInfoByUuid.mainIndex;
			}
		}
		else if (march.target == MarchTargetType.ATTACK_ARMY || march.target == MarchTargetType.ATTACK_MONSTER || march.target == MarchTargetType.RALLY_THRONE || march.target == MarchTargetType.RALLY_DRAGON_BUILDING || march.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || march.target == MarchTargetType.ATTACK_THRONE || march.target == MarchTargetType.ASSISTANCE_THRONE || march.target == MarchTargetType.RALLY_FOR_BOSS)
		{
			WorldMarch march2 = GetMarch(march.targetUuid);
			if (march2 != null)
			{
				return world.WorldToTileIndex(march2.position);
			}
		}
		else if (march.target == MarchTargetType.EXPLORE || march.target == MarchTargetType.SAMPLE || march.target == MarchTargetType.PICK_GARBAGE)
		{
			PointInfo pointInfoByUuid2 = world.GetPointInfoByUuid(march.targetUuid);
			if (pointInfoByUuid2 != null)
			{
				return pointInfoByUuid2.pointIndex;
			}
		}
		else if (march.target == MarchTargetType.GOLLOES_EXPLORE)
		{
			return (int)march.targetUuid;
		}
		return 0;
	}

	private void UpdateMessage()
	{
		if (ClientSwitch.IsOn(40))
		{
			_messages.UpdateStep();
		}
	}

	private void UpdateMove(float deltaTime)
	{
		if (ClientSwitch.IsOn(37))
		{
			StepUpdateMove(deltaTime);
		}
		else
		{
			if (world == null)
			{
				return;
			}
			long serverTime = GameEntry.Timer.GetServerTime();
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				WorldMarch value = allMarch.Value;
				if (value != null)
				{
					value.UpdateMove(deltaTime, serverTime);
					value.UpdateViewRectState(IsMarchInView(value));
					if (value.IsVisibleMarch() && value.IsInViewRect && value.IsPerformanceMarch())
					{
						num++;
					}
					if (value.NeedCreateTroopLine() && value.SupportLittleSmartTroopLineMode())
					{
						num2++;
					}
				}
			}
			if (num != lastPerformanceCount)
			{
				lastPerformanceCount = num;
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshPerformanceTroopCount", lastPerformanceCount);
			}
			if (num2 != lastPerformanceTroopLineCount)
			{
				lastPerformanceTroopLineCount = num2;
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshPerformanceTroopLineCount", lastPerformanceTroopLineCount);
			}
			foreach (KeyValuePair<long, WorldMarch> allMarch2 in allMarches)
			{
				WorldMarch value2 = allMarch2.Value;
				bool flag = value2.IsVisibleMarch();
				if (value2.IsInViewRect && flag)
				{
					if ((value2.type != NewMarchType.ZOMBIE_RUSH || (value2.type == NewMarchType.ZOMBIE_RUSH && value2.status == MarchStatus.MOVING)) && !world.IsTroopCreate(value2.uuid))
					{
						world.CreateTroop(value2);
					}
				}
				else if (world.IsTroopCreate(value2.uuid))
				{
					world.DestroyTroop(value2.uuid);
				}
				if (flag && (IsLineInView(value2.homeWorldPos, value2.startWorldPos) || IsLineInView(value2.startWorldPos, value2.targetWorldPos)))
				{
					if (value2.type == NewMarchType.ACT_BERSERK_BOSS && value2.status == MarchStatus.ATTACKING)
					{
						if (world.IsTroopLineCreate(value2.uuid))
						{
							world.DestroyTroopLine(value2.uuid);
						}
						continue;
					}
					if (!world.IsTroopLineCreate(value2.uuid) || world.IsMarchOutOfData(value2))
					{
						world.CreateTroopLine(value2);
					}
				}
				else if (world.IsTroopLineCreate(value2.uuid))
				{
					world.DestroyTroopLine(value2.uuid);
				}
				Vector3 position = value2.position;
				WorldTroop troop = world.GetTroop(value2.uuid);
				if (troop != null)
				{
					position = troop.GetPosition();
				}
				world.UpdateTroopLineNew(value2, position);
			}
		}
	}

	private void TryStartStepUpdateMove()
	{
		if (_marchStepUpdateState == EMarchStepUpdateMoveState.Invalid)
		{
			_marchStepUpdateMoveId = Time.frameCount;
			_marchStepUpdateState = EMarchStepUpdateMoveState.UpdateVisible;
		}
		_marchStepUpdateTimer.Restart();
	}

	private void ResetStepUpdateMove()
	{
		_marchStepUpdateState = EMarchStepUpdateMoveState.Invalid;
		_marchStepUpdateTimer.Stop();
		_marchStepUpdateMoveId = -1;
		_tempPerformanceMarchCountAcc = 0;
		_tempPerformanceTroopLineCountAcc = 0;
	}

	private void StepUpdateMove(float deltaTime)
	{
		if (!(world == null))
		{
			TryStartStepUpdateMove();
			if (StepUpdateMarchVisible(deltaTime) && StepUpdateMarchPerformance() && StepUpdateMarchTroop())
			{
				ResetStepUpdateMove();
			}
		}
	}

	private bool StepUpdateMarchVisible(float deltaTime)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (_marchStepUpdateState != EMarchStepUpdateMoveState.UpdateVisible)
		{
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				WorldMarch value = allMarch.Value;
				if (value != null)
				{
					value.UpdateMove(deltaTime, serverTime);
					UpdateTroopLine(value);
				}
			}
			_marchStepUpdateTimer.Restart();
			return true;
		}
		bool flag = true;
		int num = 4;
		foreach (KeyValuePair<long, WorldMarch> allMarch2 in allMarches)
		{
			WorldMarch value2 = allMarch2.Value;
			if (value2 == null)
			{
				continue;
			}
			value2.UpdateMove(deltaTime, serverTime);
			UpdateTroopLine(value2);
			if (value2.updateVisibleID == _marchStepUpdateMoveId)
			{
				continue;
			}
			if (num <= 0 && _marchStepUpdateTimer.ElapsedMilliseconds >= 2)
			{
				flag = false;
				continue;
			}
			num--;
			value2.updateVisibleID = _marchStepUpdateMoveId;
			value2.UpdateViewRectState(IsMarchInView(value2));
			if (value2.IsVisibleMarch() && value2.IsInViewRect && value2.IsPerformanceMarch())
			{
				_tempPerformanceMarchCountAcc++;
			}
			if (value2.NeedCreateTroopLine() && value2.SupportLittleSmartTroopLineMode())
			{
				_tempPerformanceTroopLineCountAcc++;
			}
		}
		if (flag)
		{
			_marchStepUpdateState = EMarchStepUpdateMoveState.UpdatePerformance;
			return true;
		}
		return false;
	}

	private bool StepUpdateMarchPerformance()
	{
		if (_marchStepUpdateState == EMarchStepUpdateMoveState.UpdateTroopLine)
		{
			return true;
		}
		if (_marchStepUpdateState == EMarchStepUpdateMoveState.UpdatePerformance)
		{
			if (_tempPerformanceMarchCountAcc != lastPerformanceCount)
			{
				lastPerformanceCount = _tempPerformanceMarchCountAcc;
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshPerformanceTroopCount", lastPerformanceCount);
			}
			if (_tempPerformanceTroopLineCountAcc != lastPerformanceTroopLineCount)
			{
				lastPerformanceTroopLineCount = _tempPerformanceTroopLineCountAcc;
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshPerformanceTroopLineCount", lastPerformanceTroopLineCount);
			}
			_marchStepUpdateState = EMarchStepUpdateMoveState.UpdateTroopLine;
			return true;
		}
		throw new ArgumentException("StepUpdateMarchPerformance can not be UpdateVisible state.");
	}

	private bool StepUpdateMarchTroop()
	{
		bool flag = true;
		int num = 4;
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			WorldMarch value = allMarch.Value;
			if (!IsMyMarch(value))
			{
				if (value.updateTroopID == _marchStepUpdateMoveId)
				{
					continue;
				}
				if (num <= 0 && _marchStepUpdateTimer.ElapsedMilliseconds >= 2)
				{
					flag = false;
					continue;
				}
			}
			num--;
			value.updateTroopID = _marchStepUpdateMoveId;
			bool flag2 = value.IsVisibleMarch();
			if (value.IsInViewRect && flag2)
			{
				if ((value.type != NewMarchType.ZOMBIE_RUSH || (value.type == NewMarchType.ZOMBIE_RUSH && value.status == MarchStatus.MOVING)) && !world.IsTroopCreate(value.uuid))
				{
					world.CreateTroop(value);
				}
			}
			else if (world.IsTroopCreate(value.uuid))
			{
				world.DestroyTroop(value.uuid);
			}
			if (flag2 && (IsLineInView(value.homeWorldPos, value.startWorldPos) || IsLineInView(value.startWorldPos, value.targetWorldPos)))
			{
				if (value.type == NewMarchType.ACT_BERSERK_BOSS && value.status == MarchStatus.ATTACKING)
				{
					if (world.IsTroopLineCreate(value.uuid))
					{
						world.DestroyTroopLine(value.uuid);
					}
				}
				else if (!world.IsTroopLineCreate(value.uuid) || world.IsMarchOutOfData(value))
				{
					world.CreateTroopLine(value);
				}
			}
			else if (world.IsTroopLineCreate(value.uuid))
			{
				world.DestroyTroopLine(value.uuid);
			}
		}
		if (flag)
		{
			_marchStepUpdateState = EMarchStepUpdateMoveState.Invalid;
			GameEntry.Event.Fire(EventId.WorldGetMarchInfosMsg);
		}
		return flag;
	}

	private void UpdateTroopLine(WorldMarch march)
	{
		Vector3 position = march.position;
		WorldTroop troop = world.GetTroop(march.uuid);
		if (troop != null)
		{
			position = troop.GetPosition();
		}
		world.UpdateTroopLineNew(march, position);
	}

	private void UpdateCollition(WorldMarch march)
	{
		if (world != null && march.status == MarchStatus.COLLECTING)
		{
			int pointIndex = world.WorldToTileIndex(march.position);
			if (world.GetObjectByPoint(pointIndex) is WorldResObject worldResObject)
			{
				worldResObject.UpdateMarch();
			}
		}
	}

	private void UpdateMarch(WorldMarch march)
	{
		AddOrUpdateMarch(march);
		if (world == null)
		{
			return;
		}
		world.RefreshNeedCreateTroop(march);
		if (world.IsTroopCreate(march.uuid))
		{
			if (march.IsMine() || (IsMarchInView(march) && march.IsVisibleMarch()))
			{
				world.UpdateTroop(march);
			}
			else
			{
				world.DestroyTroop(march.uuid);
			}
		}
	}

	private void AddToDelayDestroy(long uuid, float delaySecond)
	{
		if (delaySecond > 3f)
		{
			delaySecond = 3f;
		}
		if (!_delayDestroyTroop.ContainsKey(uuid))
		{
			_delayDestroyTroop.Add(uuid, GameEntry.Timer.GetServerTime() + (long)(delaySecond * 1000f));
		}
	}

	private void DestroyMarch(long marchUuid, bool isBattleFail)
	{
		if (world != null)
		{
			float num = world.DestroyTroop(marchUuid, isBattleFail);
			if (num > 0f)
			{
				AddToDelayDestroy(marchUuid, num);
				RemoveMarch(marchUuid);
			}
			else
			{
				world.DestroyTroopLine(marchUuid);
				RemoveMarch(marchUuid);
				GameEntry.Event.Fire(EventId.HideTroopName, marchUuid);
			}
		}
		else
		{
			RemoveMarch(marchUuid);
		}
	}

	private bool IsVisibleToLocalPlayer(WorldMarch march)
	{
		if (march.IsMonsterOrOrdinaryBoss())
		{
			if (!string.IsNullOrEmpty(march.belongUid) && march.belongUid == GameEntry.Data.Player.Uid)
			{
				return true;
			}
			if (world != null && !world.IsInSelfLandBlock(march.targetPos))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private void RemoveMarch(long marchUuid)
	{
		WorldMarch march = GetMarch(marchUuid);
		try
		{
			if (march == null)
			{
				return;
			}
			CheckMyMarchDirty(march);
			CheckToMeMarchDirty(march);
			toMeMarchUuids.Remove(march.uuid);
			myMarchUuids.Remove(march.uuid);
		}
		catch (Exception arg)
		{
			Log.Error($"Exception when RemoveMarch 1 {arg}");
		}
		try
		{
			if (march.ownerUid == GameEntry.Data.Player.Uid)
			{
				team2MarchUuid.Remove(march.teamUuid);
				GameEntry.Event.Fire(EventId.WorldMarchDelete, marchUuid);
				myMarchMultiKillPVE.Remove(march.uuid);
				myMarchMultiKillPVP.Remove(march.uuid);
			}
		}
		catch (Exception arg2)
		{
			Log.Error($"Exception when RemoveMarch 2 {arg2}");
		}
		try
		{
			if (march.IsMonsterOrOrdinaryBoss())
			{
				OnMonsterDelete?.Invoke(march.uuid, march.monsterId, march.serverId, march.startPos);
			}
			if (world != null && march.IsMonsterOrOrdinaryBoss())
			{
				world.RemoveOccupyPoints(world.IndexToTilePos(march.targetPos), Vector2Int.one, march.targetServer);
			}
			allMarches.Remove(march.uuid);
		}
		catch (Exception arg3)
		{
			Log.Error($"Exception when RemoveMarch 3 {arg3}");
		}
		if (isRecordingMarchBlock)
		{
			march.RecordMarchBlock(tileBlockIndex);
		}
		try
		{
			ownerMarches.Remove(march.uuid);
			if (IsMyMarch(march))
			{
				TryUpdateMyAssistanceMarch(march, isRemove: true);
			}
			if (!string.IsNullOrEmpty(march.allianceUid) && allianceMarches.TryGetValue(march.allianceUid, out var value))
			{
				value.RemoveAll((WorldMarch i) => i.uuid == march.uuid);
			}
			_marchBattleSound.RemoveMarch(march.uuid);
		}
		catch (Exception arg4)
		{
			Log.Error($"Exception when RemoveMarch 4 {arg4}");
		}
	}

	private void CheckMyMarchDirty(WorldMarch march)
	{
		if (!myMarchDirty && (IsMyMarch(march) || IsMyJoinAssemblyMarch(march)))
		{
			myMarchDirty = true;
		}
	}

	private void CheckToMeMarchDirty(WorldMarch march)
	{
		if (!toMeMarchDirty && (IsTargetForMine(march) || IsTargetForMine(march, onlyAttack: false, isDesertBattle: true)))
		{
			toMeMarchDirty = true;
			toMeMarchNeedUpdate = true;
			toMeMarchDesertNeedUpdate = true;
		}
	}

	public static bool IsMyMarch(WorldMarch march)
	{
		if (march.ownerUid == GameEntry.Data.Player.Uid && march.type != NewMarchType.DEFAULT)
		{
			return march.type != NewMarchType.TRAIN;
		}
		return false;
	}

	private bool IsMyAllyRallyMarch(WorldMarch march)
	{
		if (march.type != NewMarchType.ASSEMBLY_MARCH)
		{
			return false;
		}
		if (string.IsNullOrEmpty(march.allianceUid))
		{
			return false;
		}
		int num = 3;
		if (march.worldType == num)
		{
			return !GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", march.allianceUid, num);
		}
		return march.allianceUid == GameEntry.Data.Player.GetAllianceId();
	}

	private bool IsMyJoinAssemblyMarch(WorldMarch march)
	{
		if (!IsMyAllyRallyMarch(march))
		{
			return false;
		}
		if (team2MarchUuid.ContainsKey(march.teamUuid))
		{
			return true;
		}
		return false;
	}

	private void TryUpdateMyAssistanceMarch(WorldMarch march, bool isRemove)
	{
		if (!EnableWorldAssistanceOpt)
		{
			return;
		}
		if ((march.target == MarchTargetType.ASSISTANCE_BUILD || march.target == MarchTargetType.ASSISTANCE_CITY || march.target == MarchTargetType.ASSISTANCE_OUTPOST_BUILDING || march.target == MarchTargetType.ASSISTANCE_ALLIANCE_CITY || march.target == MarchTargetType.ASSISTANCE_CITY_TRADE || march.target == MarchTargetType.ASSISTANCE_ALLIANCE_BUILDING || march.target == MarchTargetType.ASSISTANCE_CITY_STRONGHOLD || march.target == MarchTargetType.ASSISTANCE_WINTER_STORM_CITY || march.target == MarchTargetType.ASSISTANCE_THRONE || march.target == MarchTargetType.ASSISTANCE_WINTER_ENTITY || march.target == MarchTargetType.ASSISTANCE_EPIDEMIC_CITY || march.target == MarchTargetType.ASSISTANCE_EPIDEMIC_BUILDING || march.target == MarchTargetType.ASSISTANCE_DRAGON_BUILDING || march.target == MarchTargetType.ASSISTANCE_SERVER_THRONE_BUILDING) && !isRemove)
		{
			if (myAssitanceUuid2Points.TryGetValue(march.uuid, out var value))
			{
				if (march.targetPos != value)
				{
					myAssitanceUuid2Points[march.uuid] = march.targetPos;
					if (myAssistancePoint2Uuids.TryGetValue(value, out var value2))
					{
						value2.Remove(march.uuid);
						if (value2.Count <= 0)
						{
							myAssistancePoint2Uuids.Remove(value);
						}
					}
					if (!myAssistancePoint2Uuids.TryGetValue(march.targetPos, out value2))
					{
						value2 = new List<long>();
						value2.Add(march.uuid);
						myAssistancePoint2Uuids.Add(march.targetPos, value2);
					}
					else if (!value2.Contains(march.uuid))
					{
						value2.Add(march.uuid);
					}
					PointInfo pointInfo = world?.GetPointInfo(march.targetPos);
					if (pointInfo != null)
					{
						world?.MarkPointIsDirty(pointInfo.uuid);
					}
				}
				else
				{
					PointInfo pointInfo2 = world?.GetPointInfo(march.targetPos);
					if (pointInfo2 != null)
					{
						world?.MarkPointIsDirty(pointInfo2.uuid);
					}
				}
			}
			else
			{
				myAssitanceUuid2Points[march.uuid] = march.targetPos;
				if (!myAssistancePoint2Uuids.TryGetValue(march.targetPos, out var value3))
				{
					value3 = new List<long>();
					value3.Add(march.uuid);
					myAssistancePoint2Uuids.Add(march.targetPos, value3);
				}
				else if (!value3.Contains(march.uuid))
				{
					value3.Add(march.uuid);
				}
				PointInfo pointInfo3 = world?.GetPointInfo(march.targetPos);
				if (pointInfo3 != null)
				{
					world?.MarkPointIsDirty(pointInfo3.uuid);
				}
			}
		}
		else
		{
			if (!myAssitanceUuid2Points.TryGetValue(march.uuid, out var value4))
			{
				return;
			}
			myAssitanceUuid2Points.Remove(march.uuid);
			if (myAssistancePoint2Uuids.TryGetValue(value4, out var value5))
			{
				value5.Remove(march.uuid);
				if (value5.Count <= 0)
				{
					myAssistancePoint2Uuids.Remove(value4);
				}
			}
			PointInfo pointInfo4 = world?.GetPointInfo(value4);
			if (pointInfo4 != null)
			{
				world?.MarkPointIsDirty(pointInfo4.uuid);
			}
		}
	}

	public int GetMyAssistanceCountByPointIndex(int pointIndex)
	{
		if (myAssistancePoint2Uuids.TryGetValue(pointIndex, out var value))
		{
			return value.Count;
		}
		return 0;
	}

	public int GetMyAssistanceFirstHero(int pointIndex)
	{
		long firstMyAssistanceMarchUuid = GetFirstMyAssistanceMarchUuid(pointIndex);
		if (firstMyAssistanceMarchUuid == 0L)
		{
			return 0;
		}
		WorldMarch march = GetMarch(firstMyAssistanceMarchUuid);
		if (march == null)
		{
			return 0;
		}
		if (GameEntry.Timer.GetServerTime() >= march.endTime)
		{
			return march.GetLeaderHero()?.heroId ?? 0;
		}
		return 0;
	}

	public long GetFirstMyAssistanceMarchUuid(int pointIndex)
	{
		if (myAssistancePoint2Uuids.TryGetValue(pointIndex, out var value) && value.Count > 0)
		{
			return value[0];
		}
		return 0L;
	}

	private void AddOrUpdateMarch(WorldMarch march)
	{
		AddMarch(march);
		if (world != null && march.IsMonsterOrOrdinaryBoss())
		{
			OnMonsterAdd?.Invoke(march.uuid, march.monsterId, march.serverId, march.startPos);
			world.AddOccupyPoints(world.IndexToTilePos(march.targetPos), Vector2Int.one, march.targetServer);
		}
		if (IsMyMarch(march))
		{
			myMarchDirty = true;
			myMarchUuids.Add(march.uuid);
			ownerMarches[march.uuid] = march;
			if (march.teamUuid > 0)
			{
				team2MarchUuid[march.teamUuid] = march.uuid;
			}
			if (march.target == MarchTargetType.BACK_HOME && member2LeaderUuid.TryGetValue(march.uuid, out var value))
			{
				myMarchUuids.Remove(value);
				ownerMarches.Remove(value);
				member2LeaderUuid.Remove(march.uuid);
			}
			TryUpdateMyAssistanceMarch(march, isRemove: false);
		}
		else if (IsMyJoinAssemblyMarch(march))
		{
			myMarchDirty = true;
			myMarchUuids.Add(march.uuid);
			ownerMarches[march.uuid] = march;
			if (team2MarchUuid.TryGetValue(march.teamUuid, out var value2))
			{
				member2LeaderUuid[value2] = march.uuid;
			}
		}
		else
		{
			ownerMarches.Remove(march.uuid);
			if (IsTargetForMine(march) || IsTargetForMine(march, onlyAttack: false, isDesertBattle: true))
			{
				toMeMarchDirty = true;
				toMeMarchNeedUpdate = true;
				toMeMarchDesertNeedUpdate = true;
				toMeMarchUuids.Add(march.uuid);
			}
		}
		List<WorldMarch> value4;
		if (march.type == NewMarchType.ASSEMBLY_MARCH)
		{
			if (!string.IsNullOrEmpty(march.allianceUid))
			{
				if (!allianceMarches.TryGetValue(march.allianceUid, out var value3))
				{
					value3 = new List<WorldMarch>();
					allianceMarches.Add(march.allianceUid, value3);
				}
				int num = value3.FindIndex((WorldMarch i) => i.uuid == march.uuid);
				if (num != -1)
				{
					value3[num] = march;
				}
				else
				{
					value3.Add(march);
				}
			}
		}
		else if (!string.IsNullOrEmpty(march.allianceUid) && allianceMarches.TryGetValue(march.allianceUid, out value4))
		{
			value4.RemoveAll((WorldMarch i) => i.uuid == march.uuid);
		}
		if (march.status == MarchStatus.COLLECTING)
		{
			UpdateCollition(march);
		}
		if (!(world != null))
		{
			return;
		}
		if (march.type == NewMarchType.ZOMBIE_RETREAT)
		{
			if (!fakeRetreatMarches.ContainsKey(march.uuid))
			{
				fakeRetreatMarches[march.uuid] = new List<WorldMarch>();
			}
			int num2 = Mathf.Min(march.npcNum - 1, 5);
			if (fakeRetreatMarches[march.uuid].Count < num2)
			{
				int timeDelta = Mathf.Max((int)(GameEntry.Timer.GetServerTime() - march.startTime), 0);
				PointInfo pointInfo = world.GetPointInfo(march.targetPos);
				for (int num3 = fakeRetreatMarches[march.uuid].Count; num3 < num2; num3++)
				{
					AddFakeRetreatMarchData(march, pointInfo, timeDelta);
				}
			}
		}
		else if (march.type == NewMarchType.TRAIN)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.AddOrUpdateTrain", ((SFSObject)march.train.trainData).ToLuaTable(GameEntry.Lua.Env));
		}
		else if (march.type != NewMarchType.ZONE_TRAIN)
		{
			if (march.type == NewMarchType.FLOWER_TRAIN)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateFlowerTrainData", march);
			}
			else if (march.type == NewMarchType.BOSS && march.allianceBoss != null)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshAllyDrillBase", march);
			}
			else if (march.type == NewMarchType.BOSS && march.invasionBossInfo != null)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshAisilaCtrl", march);
			}
			else if (march.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshZMBossActionCtrl", march);
			}
			else if (march.IsAlChallengeKirov())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshKillZombieKirovActionCtrl", march);
			}
		}
		MarchTargetType target = march.target;
		if (target == MarchTargetType.ATTACK_ALLIANCE_CITY || target == MarchTargetType.ATTACK_SERVER_THRONE_BUILDING || target == MarchTargetType.ATTACK_THRONE || target == MarchTargetType.RALLY_THRONE || target == MarchTargetType.ASSISTANCE_THRONE || target == MarchTargetType.ASSISTANCE_SERVER_THRONE_BUILDING || target == MarchTargetType.RALLY_SERVER_THRONE_BUILDING || target == MarchTargetType.RALLY_FOR_ALLIANCE_CITY)
		{
			_marchBattleSound.AddMarch(march.uuid, march.targetPos);
		}
	}

	private WorldTroopPathSegment[] CreatePathSegment(WorldMarch march)
	{
		if (march.status == MarchStatus.MOVING || march.status == MarchStatus.BACK_HOME || march.status == MarchStatus.CHASING || march.status == MarchStatus.IN_WORM_HOLE || march.status == MarchStatus.TRAIN_PULL_IN || march.status == MarchStatus.ZOMBIE_RUSH_WAITING || march.status == MarchStatus.BERSERK_BOSS_WAITING)
		{
			return march.CreatePathSegment();
		}
		if (march.status == MarchStatus.STATION && march.type == NewMarchType.RUNNING_MUMMY)
		{
			return march.CreatePathSegment();
		}
		return null;
	}

	public IEnumerator UIMainWarningHide(WarningType warningType)
	{
		yield return new WaitForSeconds(5f);
		GameEntry.Event.Fire(EventId.UIMainWarningHide, (int)warningType);
	}

	public Dictionary<long, WorldMarch> GetAllMarchesByCS()
	{
		return allMarches;
	}

	public WorldMarch GetMarchesByStartIndex(int posIndex)
	{
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.startPos == posIndex)
			{
				return value;
			}
		}
		return null;
	}

	public WorldMarch GetMarchByTargetPos(int targetPos)
	{
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.targetPos == targetPos)
			{
				return value;
			}
		}
		return null;
	}

	public WorldMarch GetMarchByType(int marchType)
	{
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.type == (NewMarchType)marchType)
			{
				return value;
			}
		}
		return null;
	}

	public Dictionary<string, int> GetMarchCountToTargetGroupByOwnerUid(int targetPos)
	{
		Dictionary<string, int> dictionary = null;
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.targetPos != targetPos)
			{
				continue;
			}
			string ownerUid = value.ownerUid;
			if (!string.IsNullOrEmpty(ownerUid))
			{
				dictionary = dictionary ?? new Dictionary<string, int>();
				if (dictionary.ContainsKey(ownerUid))
				{
					dictionary[ownerUid]++;
				}
				else
				{
					dictionary.Add(ownerUid, 1);
				}
			}
		}
		return dictionary;
	}

	public void GetMonsterListInArea(Vector2Int center, int size, Dictionary<int, int> monsterIds, Dictionary<long, Vector2Int> result)
	{
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (allMarch.Value.IsMonster() && allMarch.Value.startPos == allMarch.Value.targetPos && monsterIds.ContainsKey(allMarch.Value.monsterId))
			{
				Vector2Int value = TileCoord.IndexToTilePos(allMarch.Value.startPos, ForceChangeScene.World);
				if (value.x >= center.x - size && value.x <= center.x + size && value.y >= center.y - size && value.y <= center.y + size)
				{
					result[allMarch.Value.uuid] = value;
				}
			}
		}
	}

	public Dictionary<long, WorldMarch> GetMarchesBossInfo()
	{
		Dictionary<long, WorldMarch> dictionary = new Dictionary<long, WorldMarch>();
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (allMarch.Value.IsOrdinaryBoss())
			{
				dictionary.Add(allMarch.Key, allMarch.Value);
			}
		}
		if (dictionary != null)
		{
			return dictionary;
		}
		return null;
	}

	public Dictionary<long, WorldMarch> GetMarchesTargetForMine(bool onlyDesertBattle)
	{
		if (ClientSwitch.IsOn(37))
		{
			return GetMarchesTargetForMineCached(onlyDesertBattle);
		}
		Dictionary<long, WorldMarch> dictionary = new Dictionary<long, WorldMarch>();
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (IsTargetForMine(allMarch.Value, onlyAttack: false, onlyDesertBattle))
			{
				dictionary.Add(allMarch.Key, allMarch.Value);
			}
		}
		return dictionary;
	}

	public Dictionary<long, WorldMarch> GetMarchesTargetForMineLite()
	{
		if (ClientSwitch.IsOn(37))
		{
			return GetMarchesTargetForMineCached(onlyDesertBattle: false);
		}
		if (toMeMarchNeedUpdate)
		{
			targetForMineMarchDic.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				if (IsTargetForMine(allMarch.Value))
				{
					targetForMineMarchDic.Add(allMarch.Key, allMarch.Value);
				}
			}
			toMeMarchNeedUpdate = false;
		}
		return targetForMineMarchDic;
	}

	public Dictionary<long, WorldMarch> GetMarchesTargetForMineCached(bool onlyDesertBattle)
	{
		Dictionary<long, WorldMarch> dictionary = (onlyDesertBattle ? targetForMineMarchDesertDic : targetForMineMarchDic);
		if (onlyDesertBattle ? toMeMarchDesertNeedUpdate : toMeMarchNeedUpdate)
		{
			dictionary.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				if (IsTargetForMine(allMarch.Value, onlyAttack: false, onlyDesertBattle))
				{
					dictionary.Add(allMarch.Key, allMarch.Value);
				}
			}
			if (onlyDesertBattle)
			{
				toMeMarchDesertNeedUpdate = false;
			}
			else
			{
				toMeMarchNeedUpdate = false;
			}
		}
		return dictionary;
	}

	public Dictionary<long, WorldMarch> GetInimicalMarchesTargetForMine()
	{
		Dictionary<long, WorldMarch> dictionary = new Dictionary<long, WorldMarch>();
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (IsTargetForMine(allMarch.Value, onlyAttack: true) && (allMarch.Value.targetPos == 0 || allMarch.Value.targetPos == worldMainPos))
			{
				dictionary.Add(allMarch.Key, allMarch.Value);
			}
		}
		return dictionary;
	}

	public bool IsTargetForMine(WorldMarch march, bool onlyAttack = false, bool isDesertBattle = false)
	{
		if (isDesertBattle && march.worldId <= 0)
		{
			return false;
		}
		switch (march.target)
		{
		case MarchTargetType.ATTACK_BUILDING:
		case MarchTargetType.RALLY_FOR_BUILDING:
		case MarchTargetType.ATTACK_CITY:
		case MarchTargetType.RALLY_FOR_CITY:
		case MarchTargetType.SCOUT_CITY:
		case MarchTargetType.SCOUT_BUILDING:
		case MarchTargetType.SCOUT_WINTER_STORM_CITY:
		case MarchTargetType.ATTACK_WINTER_STORM_CITY:
		case MarchTargetType.FAKE_ATTACK:
		case MarchTargetType.ATTACK_EPIDEMIC_CITY:
		case MarchTargetType.SCOUT_EPIDEMIC_CITY:
		case MarchTargetType.RALLY_EPIDEMIC_CITY:
		case MarchTargetType.SCOUT_OUTPOST_BUILDING:
			if (isDesertBattle)
			{
				return march.targetPos == GameEntry.Data.Building.GetDragonWorldPos();
			}
			return GameEntry.Data.Building.CheckIsMyBuilding(march.targetUuid);
		case MarchTargetType.SCOUT_TREAT:
		case MarchTargetType.ZOMBIE_BOSS_ATTACK_CITY:
		case MarchTargetType.DIG_ICE_ENEMY:
		case MarchTargetType.SEASON_FARMER_SEND_RES:
		case MarchTargetType.LOTTO_RECEIVE_BASE_REWARD:
		case MarchTargetType.SCOUT_ZONE_MOBILIZATION_DONATE:
		case MarchTargetType.RUNNING_MUMMY:
		case MarchTargetType.VALENTINE_RECEIVE_BASE_REWARD:
		case MarchTargetType.DARKNESS_MONSTER_ATTACK_CITY:
		case MarchTargetType.ALLIANCE_BOSS_SAND_ATTACK_CITY:
		case MarchTargetType.POWER_WORKER_BACK:
		case MarchTargetType.POWER_WORK_HELPER_CHARGE:
		case MarchTargetType.CHARGE_SUPPLIES:
		case MarchTargetType.ALLIANCE_MONSTER_CHALLENGE_NEW_DONATE:
			return GameEntry.Data.Building.CheckIsMyBuilding(march.targetUuid);
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.ATTACK_ARMY_COLLECT:
		case MarchTargetType.SCOUT_ARMY_COLLECT:
		case MarchTargetType.SCOUT_TROOP:
		case MarchTargetType.ATTACK_METEORITE:
		case MarchTargetType.SCOUT_METEORITE:
			return IsSelfInCurrentMarchTeam(march.targetUuid);
		case MarchTargetType.RUNNING_BOSS_ATTACK_CITY:
			return true;
		case MarchTargetType.ATTACK_ROAD:
			return GameEntry.Lua.CallWithReturn<LuaTable, long>("CSharpCallLuaInterface.GetBoardData", march.targetUuid) != null;
		case MarchTargetType.ASSISTANCE_BUILD:
		case MarchTargetType.ASSISTANCE_CITY:
		case MarchTargetType.DIG_ICE_ALLY:
		case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
		case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
			if (!onlyAttack)
			{
				return GameEntry.Data.Building.CheckIsMyBuilding(march.targetUuid);
			}
			return false;
		default:
			return false;
		}
	}

	public bool IsTargetForAlly(WorldMarch march)
	{
		switch (march.target)
		{
		case MarchTargetType.BACK_HOME:
			if (march.allianceUid == GameEntry.Data.Player.GetAllianceId() && march.ownerUid != GameEntry.Data.Player.Uid)
			{
				return true;
			}
			break;
		case MarchTargetType.ATTACK_BUILDING:
		case MarchTargetType.RALLY_FOR_BUILDING:
		case MarchTargetType.ATTACK_CITY:
		case MarchTargetType.ASSISTANCE_BUILD:
		case MarchTargetType.ASSISTANCE_CITY:
		case MarchTargetType.SCOUT_CITY:
		case MarchTargetType.SCOUT_BUILDING:
		case MarchTargetType.RALLY_THRONE:
		case MarchTargetType.SCOUT_THRONE:
		case MarchTargetType.ATTACK_THRONE:
		case MarchTargetType.ASSISTANCE_THRONE:
		case MarchTargetType.RALLY_DRAGON_BUILDING:
		case MarchTargetType.SCOUT_TREAT:
		case MarchTargetType.ZOMBIE_BOSS_ATTACK_CITY:
		case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
		case MarchTargetType.SCOUT_WINTER_STORM_CITY:
		case MarchTargetType.ATTACK_WINTER_STORM_CITY:
		case MarchTargetType.SEASON_FARMER_SEND_RES:
		case MarchTargetType.LOTTO_RECEIVE_BASE_REWARD:
		case MarchTargetType.SCOUT_ZONE_MOBILIZATION_DONATE:
		case MarchTargetType.VALENTINE_RECEIVE_BASE_REWARD:
		case MarchTargetType.RALLY_EPIDEMIC_BUILDING:
		case MarchTargetType.ATTACK_EPIDEMIC_CITY:
		case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
		case MarchTargetType.SCOUT_EPIDEMIC_CITY:
		case MarchTargetType.POWER_WORKER_BACK:
		case MarchTargetType.POWER_WORK_HELPER_CHARGE:
		case MarchTargetType.CHARGE_SUPPLIES:
		case MarchTargetType.ALLIANCE_MONSTER_CHALLENGE_NEW_DONATE:
		case MarchTargetType.SCOUT_OUTPOST_BUILDING:
			if (SceneManager.World != null && SceneManager.World.GetPointInfoByUuid(march.targetUuid) is BuildPointInfo buildPointInfo && buildPointInfo.allianceId == GameEntry.Data.Player.GetAllianceId())
			{
				return true;
			}
			break;
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.ATTACK_ARMY_COLLECT:
		case MarchTargetType.SCOUT_ARMY_COLLECT:
		case MarchTargetType.SCOUT_TROOP:
		{
			WorldMarch march2 = GetMarch(march.targetUuid);
			if (march2 != null && march2.allianceUid == GameEntry.Data.Player.GetAllianceId())
			{
				return true;
			}
			break;
		}
		case MarchTargetType.ATTACK_DESERT:
		case MarchTargetType.SCOUT_DESERT:
		{
			if (!(world != null))
			{
				break;
			}
			WorldTileInfo worldTileInfo = world.GetWorldTileInfo(march.targetUuid.ToInt());
			if (worldTileInfo != null)
			{
				WorldDesertInfo desertInfoByUuid = worldTileInfo.GetWorldDesertInfo();
				if (desertInfoByUuid != null && desertInfoByUuid.GetPlayerType() == PlayerType.PlayerSelf)
				{
					return true;
				}
			}
			break;
		}
		case MarchTargetType.ASSISTANCE_DESERT:
			if (world != null)
			{
				WorldDesertInfo desertInfoByUuid = world.GetDesertInfoByUuid(march.targetUuid);
				if (desertInfoByUuid != null && desertInfoByUuid.GetPlayerType() == PlayerType.PlayerSelf)
				{
					return true;
				}
			}
			break;
		}
		return false;
	}

	private void InitTrainConfig()
	{
		float.TryParse(GameEntry.ConfigCache.GetTemplateData("train_para", 10, "val"), out WorldTrain.Carriage_Length);
		WorldTrain.Carriage_Length *= 2f;
		trainConfigs.Clear();
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllTrainConfig")?.ForEach(delegate(int id, LuaTable data)
		{
			if (data.ContainsKey("quality") && data.ContainsKey("length"))
			{
				int quality = data.Get<int>("quality");
				int carriageNum = data.Get<int>("length");
				trainConfigs[id] = new WorldTrainConfig(id, quality, carriageNum);
			}
		});
	}

	private void InitFlowerCarConfig()
	{
		flowerCarLength.Clear();
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetFlowerCarLength")?.ForEach(delegate(int id, int data)
		{
			flowerCarLength[id] = data;
		});
	}

	public WorldTrainConfig GetTrainConfig(int id)
	{
		if (trainConfigs.Count == 0)
		{
			InitTrainConfig();
		}
		if (trainConfigs.TryGetValue(id, out var value))
		{
			return value;
		}
		Log.Error("火车配置找不到，cfgId = " + id);
		return new WorldTrainConfig(0, 1, 1);
	}

	public int GetFlowerCarLength(int id)
	{
		if (flowerCarLength.Count == 0)
		{
			InitFlowerCarConfig();
		}
		if (flowerCarLength.TryGetValue(id, out var value))
		{
			return value;
		}
		Log.Error("花车长度配置找不到，cfgId = " + id);
		return 77;
	}

	public void CacheAllianceMembersHomePos()
	{
		allianceMembersHomePos.Clear();
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllianceMembersHomePos")?.ForEach(delegate(int pointId, string uid)
		{
			allianceMembersHomePos.Add(pointId);
		});
	}

	public void CleanAllianceMembersHomePos()
	{
		allianceMembersHomePos.Clear();
	}

	public bool IsMemberByPointId(int pointId)
	{
		return allianceMembersHomePos.Contains(pointId);
	}

	public void DestroyBerserkBossMarchData(long marchUuid)
	{
		DestroyMarch(marchUuid, isBattleFail: true);
	}

	public string SaveCreateMarchRecordTime()
	{
		string text = Guid.NewGuid().ToString();
		long serverTime = GameEntry.Timer.GetServerTime();
		_cacheClientCreateGuidAndTimeDict[text] = serverTime;
		return text;
	}

	public void RemoveCreateMarchRecordTime(WorldMarch worldMarch)
	{
		if (!string.IsNullOrEmpty(worldMarch.clientCreateGuid) && _cacheClientCreateGuidAndTimeDict.ContainsKey(worldMarch.clientCreateGuid))
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = _cacheClientCreateGuidAndTimeDict[worldMarch.clientCreateGuid];
			long num2 = serverTime - num;
			_cacheClientCreateGuidAndTimeDict.Remove(worldMarch.clientCreateGuid);
			PostEventLog.TrackMap("CreateMarchDeltaTime", new Dictionary<string, object>
			{
				{ "uuid", worldMarch.uuid },
				{ "timeDiff", num2 }
			});
		}
	}

	private int GetMyMarchMultiKillPVE(long marchUuid)
	{
		if (!myMarchMultiKillPVE.TryGetValue(marchUuid, out var value))
		{
			return 0;
		}
		return value;
	}

	private int GetMyMarchMultiKillPVP(long marchUuid)
	{
		if (!myMarchMultiKillPVP.TryGetValue(marchUuid, out var value))
		{
			return 0;
		}
		return value;
	}

	public void UpdateBattleSoundData()
	{
		_marchBattleSound.Init();
	}

	[Conditional("UNITY_EDITOR")]
	public static void EditorLog(string log)
	{
	}

	public string EditorDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("---World March Data Manager---");
		stringBuilder.AppendLine($"当前所有WorldMarch数量:{allMarches.Count}");
		stringBuilder.AppendLine($"性能相关行军数量:{lastPerformanceCount}");
		int num = 0;
		int num2 = 0;
		if (allMarches.Count > 0)
		{
			Dictionary<NewMarchType, int> dictionary = new Dictionary<NewMarchType, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				WorldMarch value = allMarch.Value;
				if (value.IsVisibleMarch())
				{
					num++;
					if (value.IsInViewRect)
					{
						num2++;
					}
				}
				NewMarchType type = allMarch.Value.type;
				if (dictionary.TryGetValue(type, out var value2))
				{
					dictionary[type] = value2 + 1;
				}
				else
				{
					dictionary[type] = 1;
				}
				int worldId = allMarch.Value.worldId;
				if (dictionary2.TryGetValue(worldId, out var value3))
				{
					dictionary2[worldId] = value3 + 1;
				}
				else
				{
					dictionary2[worldId] = 1;
				}
			}
			stringBuilder.AppendLine("按照类型分组统计:");
			foreach (KeyValuePair<NewMarchType, int> item in dictionary)
			{
				stringBuilder.AppendLine($"{item.Key.ToString()}:{item.Value}");
			}
			stringBuilder.AppendLine("按照世界id分组统计:");
			foreach (KeyValuePair<int, int> item2 in dictionary2)
			{
				stringBuilder.AppendLine($"{item2.Key.ToString()}:{item2.Value}");
			}
		}
		stringBuilder.AppendLine($"可见行军:{num}");
		stringBuilder.AppendLine($"视野内行军:{num2}");
		stringBuilder.AppendLine($"目标是我的行军:{toMeMarchUuids.Count}");
		stringBuilder.AppendLine($"目标是我的行军的目标...:{toMeMarchTargetUuids.Count}");
		stringBuilder.AppendLine($"是否在记录地格占用:{isRecordingMarchBlock}");
		if (isRecordingMarchBlock)
		{
			stringBuilder.AppendLine($"已记录数量:{tileBlockIndex.Count}");
		}
		stringBuilder.AppendLine($"我驻防的行军数量:{myAssitanceUuid2Points.Count}");
		if (myAssitanceUuid2Points.Count > 0)
		{
			foreach (KeyValuePair<long, int> myAssitanceUuid2Point in myAssitanceUuid2Points)
			{
				stringBuilder.AppendLine($"{myAssitanceUuid2Point.Key}:{world.IndexToTilePos(myAssitanceUuid2Point.Value)}");
			}
		}
		stringBuilder.AppendLine($"我驻防的目标数量:{myAssistancePoint2Uuids.Count}");
		if (myAssistancePoint2Uuids.Count > 0)
		{
			foreach (KeyValuePair<int, List<long>> myAssistancePoint2Uuid in myAssistancePoint2Uuids)
			{
				stringBuilder.AppendLine($"{world.IndexToTilePos(myAssistancePoint2Uuid.Key)}:{myAssistancePoint2Uuid.Value?.Count}");
				if (myAssistancePoint2Uuid.Value.Count <= 0)
				{
					continue;
				}
				foreach (long item3 in myAssistancePoint2Uuid.Value)
				{
					stringBuilder.AppendLine($"\t{item3}");
				}
			}
		}
		return stringBuilder.ToString();
	}

	public void AddMarch(WorldMarch march)
	{
		allMarches[march.uuid] = march;
		march.InitMove(CreatePathSegment(march));
		if (isRecordingMarchBlock && recordingWorldId == march.worldId)
		{
			march?.RecordMarchBlock(tileBlockIndex);
		}
	}

	public void StartRecordMarchBlock(int worldId)
	{
		if (isRecordingMarchBlock)
		{
			return;
		}
		isRecordingMarchBlock = true;
		tileBlockIndex.Clear();
		recordingWorldId = worldId;
		int num = 0;
		int num2 = -1;
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			WorldMarch value = allMarch.Value;
			if (value != null)
			{
				if (value.worldId != recordingWorldId)
				{
					num++;
					num2 = value.worldId;
				}
				else
				{
					allMarch.Value?.RecordMarchBlock(tileBlockIndex);
				}
			}
		}
		if (num > 0)
		{
			Log.Info($"WorldMapGridRenderer.StartRecordTileBlock skip march count => {num}! target worldId:{recordingWorldId}, skip worldId:{num2}");
		}
	}

	public void StopRecordMarchBlock()
	{
		tileBlockIndex.Clear();
		isRecordingMarchBlock = false;
		recordingWorldId = int.MinValue;
	}
}
