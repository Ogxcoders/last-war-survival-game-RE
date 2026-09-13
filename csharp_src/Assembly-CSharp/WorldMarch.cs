using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using GameFramework;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;

public class WorldMarch : IDisposable
{
	private long _uuid;

	public int updateVisibleID = -1;

	public int updateTroopID = -1;

	public WorldTrain train;

	public WorldFlowerTrain flowerTrain;

	public AllianceBossInfo allianceBoss;

	public InvasionBossInfo invasionBossInfo;

	public ThermalConductor thermalConductor;

	public SandWormData sandWormData;

	public DarknessMonsterData darknessMonsterData;

	public ZMBossInfo zMBossInfo;

	public DetectZombieBusTrain detectZombieBusTrain;

	public AllianceChallengeInfo allianceChallengeInfo;

	public CityBattleS1MonsterInfo cityBattleS1MonsterInfo;

	public BloodyQueenMonster bloodyQueenMonster;

	public string ownerUid;

	public string ownerName;

	public int ownerServer;

	public int ownerCurServerId;

	public long teamUuid;

	public string allianceUid;

	public long ownerFormationUuid;

	public int[] path;

	public int targetPos;

	public int startPos;

	public int homePos;

	public long power;

	public Vector3 startWorldPos;

	public Vector3 targetWorldPos;

	public Vector3 homeWorldPos;

	public long startTime;

	public long endTime;

	public long blackStartTime;

	public long blackEndTime;

	public MarchTargetType target;

	public MarchStatus status = MarchStatus.DEFAULT;

	public NewMarchType type = NewMarchType.DEFAULT;

	public bool isAnonymity;

	public long targetUuid;

	public float speed;

	public float oriSpeed;

	public float blackSpeed;

	public string plunderRes;

	public int worldId;

	public int worldType;

	public float collectSpd;

	public long armyWeight;

	public bool inBattle;

	public List<ArmyInfo> armyInfos = new List<ArmyInfo>();

	public int monsterId;

	public int monsterSpecialType;

	public int monsterType;

	public float monsterHpRatio;

	public int monsterRallyNum;

	public int zombieRushRound;

	public int zombieRushId;

	public int allianceBuildingCfgId;

	public long refreshTime;

	public long createTime;

	public long expireTime;

	public long actStartTime;

	public long actEndTime;

	public int cityId;

	public int npcNum;

	public bool isBroken;

	public Vector2Int stationDir;

	public string allianceAbbr;

	public string allianceName;

	public string allianceIcon;

	public string eventId;

	public long eventUuid;

	public string belongUid;

	public string pic;

	public int picVer;

	public int headSkinId;

	public long headSkinET;

	public int pvpNum;

	public int pveNum;

	public int baseVirusLayer;

	public int extraVirusLayer;

	public int serverId;

	public int targetServer;

	public int srcServer;

	public int pathStartServerId;

	public int realTargetPos;

	public bool isSelect;

	public bool isCameraFollow;

	public Vector3 position;

	public WorldTroopPathSegment[] pathList;

	private float curPathLen;

	private float moveSpeed;

	private Vector3 moveDir;

	public string bossOwnerUid;

	public int callHelp;

	public int secretKey;

	public bool isFake;

	public bool isFakeAttack;

	public long fakeMarchTime;

	public long parentUuid;

	public bool delayApply;

	public float delayApplyTime;

	public bool IsStrongholdBoss;

	public long strongholdBossNum;

	public long strongholdBossMax;

	public bool IsCityBoss;

	public long cityBossNum;

	public long cityBossMax;

	public bool isMummyMarch;

	public int fixedSoldierType = 1;

	public long ownerLightUuid;

	public int catchZombieNum;

	public long bankDeposit;

	public string itemId;

	public bool globalArmy;

	private static Dictionary<long, int> catchZombieAnim = new Dictionary<long, int>();

	private bool fightMonster;

	private long _curHp;

	private int _berserkBossMetaId;

	private string _clientCreateGuid = "";

	private long bornTime;

	public float LightLength;

	public int actBossState;

	public ISFSObject userInfo;

	public int armyUnit;

	public int totalArmyUnit;

	public long nextSkillTime;

	public int crystal;

	public int nucleus;

	public long holdUuid;

	public long bloodyQueenMonsterUuid;

	public long cityBattleS1RestBossUuid;

	private float clientSpend;

	private long firstTick;

	private long firstTick_Real;

	private float firstTick_unity;

	private int blockRecordSize = -1;

	public long uuid
	{
		get
		{
			return _uuid;
		}
		set
		{
			_uuid = value;
		}
	}

	public PointInfo StationPointInfo { get; private set; }

	public Vector3 MoveDir => moveDir;

	public long curHp => _curHp;

	public int berserkBossMetaId => _berserkBossMetaId;

	public string clientCreateGuid => _clientCreateGuid;

	public bool IsValid { get; set; }

	public bool HasMeteorite
	{
		get
		{
			if (crystal <= 0)
			{
				return nucleus > 0;
			}
			return true;
		}
	}

	public bool IsInViewRect { get; private set; }

	public bool IsBirthing()
	{
		if (bornTime <= 0)
		{
			return false;
		}
		if (GameEntry.Timer.GetServerTime() < bornTime + 1000)
		{
			return true;
		}
		bornTime = 0L;
		return false;
	}

	public void Dispose()
	{
		_uuid = 0L;
		updateVisibleID = -1;
		updateTroopID = -1;
		train = null;
		flowerTrain = null;
		allianceBoss = null;
		invasionBossInfo = null;
		thermalConductor = null;
		ownerUid = null;
		ownerName = null;
		ownerServer = 0;
		ownerCurServerId = 0;
		teamUuid = 0L;
		allianceUid = null;
		ownerFormationUuid = 0L;
		path = null;
		targetPos = 0;
		startPos = 0;
		homePos = 0;
		power = 0L;
		startWorldPos = Vector3.zero;
		targetWorldPos = Vector3.zero;
		homeWorldPos = Vector3.zero;
		startTime = 0L;
		endTime = 0L;
		blackStartTime = 0L;
		blackEndTime = 0L;
		target = MarchTargetType.STATE;
		status = MarchStatus.DEFAULT;
		type = NewMarchType.DEFAULT;
		targetUuid = 0L;
		speed = 0f;
		oriSpeed = 0f;
		plunderRes = null;
		worldId = 0;
		worldType = 0;
		collectSpd = 0f;
		armyWeight = 0L;
		inBattle = false;
		armyInfos.Clear();
		monsterId = 0;
		monsterHpRatio = 0f;
		refreshTime = 0L;
		createTime = 0L;
		actStartTime = 0L;
		actEndTime = 0L;
		cityId = 0;
		npcNum = 0;
		isBroken = false;
		stationDir = Vector2Int.zero;
		allianceAbbr = null;
		allianceName = null;
		allianceIcon = null;
		eventId = null;
		belongUid = null;
		pic = null;
		picVer = 0;
		headSkinId = 0;
		headSkinET = 0L;
		serverId = 0;
		targetServer = 0;
		srcServer = 0;
		pathStartServerId = 0;
		realTargetPos = 0;
		isSelect = false;
		isCameraFollow = false;
		position = Vector3.zero;
		pathList = null;
		curPathLen = 0f;
		moveSpeed = 0f;
		moveDir = Vector3.zero;
		bossOwnerUid = null;
		callHelp = 0;
		secretKey = 0;
		isFake = false;
		isFakeAttack = false;
		fakeMarchTime = 0L;
		parentUuid = 0L;
		delayApply = false;
		delayApplyTime = 0f;
		fightMonster = false;
		IsValid = false;
		_curHp = 0L;
		_berserkBossMetaId = 0;
		blackSpeed = 0f;
		_clientCreateGuid = "";
		armyUnit = 0;
		totalArmyUnit = 0;
		nextSkillTime = 0L;
		zombieRushId = 0;
		zombieRushRound = 0;
		allianceBuildingCfgId = 0;
		holdUuid = 0L;
		isMummyMarch = false;
		fixedSoldierType = 1;
		blockRecordSize = -1;
		zMBossInfo = null;
		allianceChallengeInfo = null;
		cityBattleS1MonsterInfo = null;
		bloodyQueenMonster = null;
		crystal = 0;
		nucleus = 0;
		eventUuid = 0L;
		detectZombieBusTrain?.Dispose();
		detectZombieBusTrain = null;
		bloodyQueenMonsterUuid = 0L;
		cityBattleS1RestBossUuid = 0L;
		LightLength = 0f;
		StationPointInfo = null;
		actBossState = 0;
	}

	public bool IsMine()
	{
		return ownerUid == GameEntry.Data.Player.Uid;
	}

	public bool IsMineOrAllyAssembly()
	{
		if (!(ownerUid == GameEntry.Data.Player.Uid))
		{
			if (type == NewMarchType.ASSEMBLY_MARCH)
			{
				return allianceUid == GameEntry.Data.Player.GetAllianceId();
			}
			return false;
		}
		return true;
	}

	public bool IsScoutMarch()
	{
		if (type != NewMarchType.SCOUT && type != NewMarchType.TREAT_VIRUS && type != NewMarchType.LOTTO_RECEIVE && type != NewMarchType.ZONE_MOBILIZATION_DONATE)
		{
			return type == NewMarchType.MONSTER_CHALLENGE_DONATE;
		}
		return true;
	}

	public ArmyInfo GetArmyInfoIndexOne()
	{
		if (armyInfos.Count > 0)
		{
			return armyInfos[0];
		}
		return null;
	}

	public int GetWorldType()
	{
		if (worldId > 0)
		{
			if (worldType > 0)
			{
				return worldType;
			}
			if (target == MarchTargetType.ATTACK_WINTER_ENTITY || target == MarchTargetType.ASSISTANCE_WINTER_ENTITY || target == MarchTargetType.SCOUT_WINTER_ENTITY)
			{
				return 2;
			}
			if (target >= MarchTargetType.ATTACK_EPIDEMIC_BUILDING && target <= MarchTargetType.GATHER_EPIDEMIC_RES)
			{
				return 3;
			}
			return 1;
		}
		return 0;
	}

	public bool ShowFiveHero()
	{
		if (isFake && isFakeAttack)
		{
			return true;
		}
		if (!isFake)
		{
			if (type != NewMarchType.NORMAL && type != NewMarchType.CROSS_NORMAL && type != NewMarchType.FAKE_ATTACK && type != NewMarchType.ASSEMBLY_MARCH && type != NewMarchType.DIRECT_MOVE_MARCH)
			{
				return type == NewMarchType.ALL_OUT;
			}
			return true;
		}
		return false;
	}

	public Vector3 GetTrainTailPos()
	{
		return position - moveDir * train.length;
	}

	public bool ShowTrain()
	{
		return type == NewMarchType.TRAIN;
	}

	public bool ShowFlowerTrain()
	{
		return type == NewMarchType.FLOWER_TRAIN;
	}

	public bool IsZombieBusTrain()
	{
		return type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN;
	}

	public Vector3 GetZombieBusTrainTailPos()
	{
		float zombieBusTrainLength = GetZombieBusTrainLength();
		return position - zombieBusTrainLength * moveDir;
	}

	public float GetZombieBusTrainLength()
	{
		int num = ((detectZombieBusTrain != null) ? detectZombieBusTrain.ZombieBusList.Count : 0);
		return 4.56f * (float)num;
	}

	public bool IsMummyMarch()
	{
		return isMummyMarch;
	}

	public bool IsDrillBase()
	{
		if (type == NewMarchType.BOSS && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result) && (result == 8 || result == 42 || result == 51))
		{
			return true;
		}
		return false;
	}

	public bool IsDrillBaseTank()
	{
		if (type == NewMarchType.BOSS && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result) && result == 8)
		{
			return true;
		}
		return false;
	}

	public bool IsDrillBaseNewBossHugeSandWorm()
	{
		if (type == NewMarchType.BOSS && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result) && result == 42)
		{
			return true;
		}
		return false;
	}

	public bool IsDrillBaseRoadHog()
	{
		if (type == NewMarchType.BOSS && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result) && result == 51)
		{
			return true;
		}
		return false;
	}

	public bool IsAisila()
	{
		if (type != NewMarchType.BOSS)
		{
			return false;
		}
		if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result))
		{
			return result == 27;
		}
		return false;
	}

	public bool IsAlChallengeKirov()
	{
		if (type == NewMarchType.BOSS && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result))
		{
			return result == 44;
		}
		return false;
	}

	public bool IsActBerserkBoss()
	{
		if (type != NewMarchType.ACT_BERSERK_BOSS)
		{
			return false;
		}
		if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result))
		{
			return result == 23;
		}
		return false;
	}

	public bool IsSandWorm()
	{
		if (type == NewMarchType.BOSS)
		{
			if (monsterSpecialType != 29)
			{
				return monsterSpecialType == 40;
			}
			return true;
		}
		return false;
	}

	public bool IsS4WanderBoss()
	{
		if (type == NewMarchType.DARKNESS_MONSTER)
		{
			return monsterType == 22;
		}
		return false;
	}

	public bool IsFlowerCar()
	{
		if (type == NewMarchType.DARKNESS_MONSTER)
		{
			return monsterType == 13;
		}
		return false;
	}

	public Vector3 GetFlowerCarTailPos()
	{
		int id = monsterId / 100;
		return position - moveDir * SceneManager.MarchDataMgr.GetFlowerCarLength(id);
	}

	public bool IsAllyBaseAttackCitySandWorm()
	{
		return type == NewMarchType.ALLIANCE_BOSS_SAND;
	}

	public bool HasFightMonster()
	{
		return fightMonster;
	}

	public bool IsVisibleMarch()
	{
		if (status == MarchStatus.IN_TEAM || status == MarchStatus.WAIT_RALLY || status == MarchStatus.COLLECTING || status == MarchStatus.ASSISTANCE || status == MarchStatus.IN_WORM_HOLE || status == MarchStatus.BUILD_WORM_HOLE || status == MarchStatus.TREASURE_DIGGING || status == MarchStatus.CROSS_SERVER)
		{
			return false;
		}
		int num = GameEntry.Data.Player.GetWorldId();
		if (worldId != num)
		{
			return false;
		}
		if (worldId == 0 && num == 0 && SeasonDataManager.Instance.InSeasonBigMapMode())
		{
			return SeasonDataManager.Instance.InNinePalacesList(serverId);
		}
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (curServerId > 0 && serverId > 0 && serverId != curServerId)
		{
			return false;
		}
		return true;
	}

	public bool IsLightMarch()
	{
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (curServerId > 0 && serverId > 0 && serverId != curServerId)
		{
			return false;
		}
		if (worldId != GameEntry.Data.Player.GetWorldId())
		{
			return false;
		}
		if (status != MarchStatus.IN_TEAM && status != MarchStatus.WAIT_RALLY && status != MarchStatus.COLLECTING && status != MarchStatus.ASSISTANCE && status != MarchStatus.IN_WORM_HOLE && status != MarchStatus.BUILD_WORM_HOLE && status != MarchStatus.TREASURE_DIGGING)
		{
			return status != MarchStatus.CROSS_SERVER;
		}
		return false;
	}

	public bool IsCityBattleS1Monster()
	{
		if (type == NewMarchType.BOSS && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result))
		{
			return result == 46;
		}
		return false;
	}

	public bool IsBloodyQueenMonster()
	{
		return type == NewMarchType.BLOODY_QUEEN;
	}

	public bool IsBloodyQueenQueenGunner()
	{
		if (type == NewMarchType.BLOODY_QUEEN && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result))
		{
			return result == 48;
		}
		return false;
	}

	public bool IsFlowerTrain()
	{
		return type == NewMarchType.FLOWER_TRAIN;
	}

	public Vector3 GetFlowerTrainTailPos()
	{
		return position - moveDir * flowerTrain.length;
	}

	public void UpdateWorldMarch(ISFSObject q, bool isPushAdd = false)
	{
		IsValid = true;
		if (q.TryGetBool("isProto"))
		{
			ByteArray byteArray = q.GetByteArray("_proto");
			if (byteArray?.Bytes != null)
			{
				Protobuf.WorldMarch worldMarch = Protobuf.WorldMarch.Parser.ParseFrom(byteArray.Bytes);
				if (worldMarch != null)
				{
					UpdateFromProto(q, worldMarch);
				}
			}
		}
		else
		{
			UpdateFromSFS(q);
		}
		if (isPushAdd && (type == NewMarchType.MUMMY || type == NewMarchType.RUNNING_MUMMY))
		{
			bornTime = GameEntry.Timer.GetServerTime();
		}
		if (catchZombieNum <= 0 || target != MarchTargetType.BACK_HOME)
		{
			return;
		}
		int value = 0;
		SceneInterface world = SceneManager.World;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		catchZombieAnim.TryGetValue(uuid, out value);
		if (world == null || curSkinMeta == null || !curSkinMeta.IsDarknessMode() || value == catchZombieNum)
		{
			return;
		}
		Vector3 marchCurPos = GetMarchCurPos();
		Vector3 v3PosStart = TileCoord.TileIndexToWorld(startPos, ForceChangeScene.World, serverId);
		if (!(Vector3.Distance(marchCurPos, v3PosStart) < 3f))
		{
			return;
		}
		world.CreateBattleVFX("Assets/Main/SeasonRes/S4/Prefabs/World/WorldTroopTaskOn.prefab", 3f, delegate(GameObject go)
		{
			if (go != null)
			{
				go.transform.SetParent(SceneManager.World.DynamicObjNode);
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = v3PosStart;
			}
		});
		catchZombieAnim[uuid] = catchZombieNum;
	}

	private void UpdateFromSFS(ISFSObject q)
	{
		IsValid = true;
		_uuid = q.TryGetLong("uuid");
		holdUuid = q.TryGetLong("holdUuid");
		teamUuid = q.TryGetLong("teamUuid");
		ownerUid = q.TryGetString("ownerUid");
		ownerName = q.TryGetString("ownerName");
		ownerServer = q.TryGetInt("ownerServer");
		ownerCurServerId = q.TryGetInt("ownerCurServerId");
		ownerFormationUuid = q.TryGetLong("ownerFormationUuid");
		string text = q.TryGetString("path");
		path = (from a in text.Split(new char[1] { ';' })
			select a.ToInt()).ToArray();
		targetPos = q.TryGetInt("targetPos");
		power = q.TryGetInt("power");
		startPos = q.TryGetInt("startPos");
		homePos = q.TryGetInt("mainPointId");
		target = SeasonDataManager.CheckCrossMarchTargetType((MarchTargetType)q.TryGetInt("target"), ref globalArmy);
		status = (MarchStatus)q.TryGetInt("status");
		targetUuid = q.TryGetLong("targetUuid");
		startTime = q.TryGetLong("startTime");
		endTime = q.TryGetLong("endTime");
		worldId = q.TryGetInt("worldId");
		worldType = q.TryGetInt("worldType");
		secretKey = q.TryGetInt("secretKey");
		if (worldId <= 0)
		{
			blackStartTime = q.TryGetLong("blackStartTime");
			blackEndTime = q.TryGetLong("blackEndTime");
		}
		else
		{
			blackStartTime = 0L;
			blackEndTime = 0L;
		}
		allianceUid = q.TryGetString("allianceUid");
		type = SeasonDataManager.CheckCrossMarchType((NewMarchType)q.TryGetInt("type"), ref globalArmy);
		speed = q.TryGetFloat("speed");
		oriSpeed = q.TryGetFloat("oriSpeed");
		blackSpeed = q.TryGetFloat("blackSpeed");
		plunderRes = q.TryGetString("plunderRes");
		collectSpd = q.TryGetFloat("collectSpd");
		armyWeight = q.TryGetLong("armyWeight");
		inBattle = q.TryGetBool("inBattle");
		eventId = q.TryGetString("eventId");
		belongUid = q.TryGetString("belongUid");
		actEndTime = q.TryGetLong("actEnd");
		actStartTime = q.TryGetLong("actStart");
		bossOwnerUid = q.TryGetString("bossOwnerUid");
		serverId = q.TryGetInt("server");
		targetServer = q.TryGetInt("targetServer");
		srcServer = q.TryGetInt("srcServer");
		pathStartServerId = q.TryGetInt("pathStartServerId");
		fightMonster = q.TryGetBool("fightMonster");
		isAnonymity = q.TryGetBool("isAnonymity");
		fixedSoldierType = q.TryGetInt("fixedSoldierType");
		isMummyMarch = fixedSoldierType == 4;
		if (pathStartServerId <= 0)
		{
			pathStartServerId = srcServer;
		}
		if (ownerCurServerId == 0)
		{
			ownerCurServerId = ownerServer;
		}
		homeWorldPos = TileCoord.TileIndexToWorld(homePos, ForceChangeScene.World, ownerCurServerId);
		targetWorldPos = TileCoord.TileIndexToWorld(targetPos, ForceChangeScene.World, targetServer);
		startWorldPos = TileCoord.TileIndexToWorld(startPos, ForceChangeScene.World, srcServer);
		ownerLightUuid = q.TryGetLong("ownerLightUuid");
		catchZombieNum = q.TryGetInt("catchZombieNum");
		monsterRallyNum = 0;
		if (type == NewMarchType.MONSTER || type == NewMarchType.BOSS || type == NewMarchType.ACT_BOSS || type == NewMarchType.PUZZLE_BOSS || type == NewMarchType.DARK_KNIGHT_CITY || type == NewMarchType.RUNNING_BOSS || type == NewMarchType.SANDFISH || type == NewMarchType.RUNNING_MUMMY || type == NewMarchType.ZOMBIE_RUSH || type == NewMarchType.MUMMY || type == NewMarchType.ACT_BERSERK_BOSS || type == NewMarchType.BEHEMOTH_BOSS || type == NewMarchType.BEHEMOTN_SKILL || type == NewMarchType.ZONE_MOBILIZATION_BOSS || type == NewMarchType.CAMEL || type == NewMarchType.BLOODY_QUEEN || type == NewMarchType.ALLIANCE_BOSS_SAND)
		{
			monsterId = q.TryGetInt("monsterId");
			refreshTime = q.TryGetLong("refreshTime");
			createTime = q.TryGetLong("createTime");
			monsterHpRatio = q.TryGetInt("running_hp");
			switch (type)
			{
			case NewMarchType.BOSS:
			{
				ISFSObject iSFSObject2 = q.TryGetObj("allianceBossInfo");
				if (iSFSObject2 != null)
				{
					allianceBoss = new AllianceBossInfo();
					allianceBoss.Update(iSFSObject2);
				}
				ISFSObject iSFSObject3 = q.TryGetObj("invasionBossInfo");
				if (iSFSObject3 != null)
				{
					invasionBossInfo = new InvasionBossInfo();
					invasionBossInfo.Update(iSFSObject3);
				}
				ISFSObject iSFSObject4 = q.TryGetObj("strongholdBoss");
				if (iSFSObject4 != null)
				{
					IsStrongholdBoss = true;
					strongholdBossNum = iSFSObject4.TryGetLong("curNum");
					strongholdBossMax = iSFSObject4.TryGetLong("maxNum");
					if (strongholdBossMax != 0L)
					{
						monsterHpRatio = (float)strongholdBossNum * 100f / (float)strongholdBossMax;
					}
				}
				ISFSObject iSFSObject5 = q.TryGetObj("ghostKing");
				if (iSFSObject5 != null)
				{
					IsCityBoss = true;
					cityBossNum = iSFSObject5.TryGetLong("curNum");
					cityBossMax = iSFSObject5.TryGetLong("maxNum");
					if (cityBossMax != 0L)
					{
						monsterHpRatio = (float)cityBossNum * 100f / (float)cityBossMax;
					}
				}
				break;
			}
			case NewMarchType.RUNNING_BOSS:
			case NewMarchType.RUNNING_MUMMY:
				monsterRallyNum = q.TryGetInt("rally_num");
				break;
			case NewMarchType.ZONE_MOBILIZATION_BOSS:
			{
				ISFSObject iSFSObject = q.TryGetObj("zMBossInfo");
				if (iSFSObject != null)
				{
					zMBossInfo = new ZMBossInfo();
					zMBossInfo.Update(iSFSObject);
				}
				break;
			}
			case NewMarchType.ZOMBIE_RUSH:
				zombieRushRound = q.TryGetInt("zombieRushRound");
				zombieRushId = q.TryGetInt("zombieRushId");
				allianceBuildingCfgId = q.TryGetInt("allianceBuildingCfgId");
				break;
			case NewMarchType.ACT_BERSERK_BOSS:
				_berserkBossMetaId = q.TryGetInt("bossId");
				break;
			case NewMarchType.BEHEMOTH_BOSS:
				armyUnit = q.TryGetInt("armyUnit");
				totalArmyUnit = q.TryGetInt("totalArmyUnit");
				monsterHpRatio = (float)armyUnit * 100f / (float)totalArmyUnit;
				nextSkillTime = q.TryGetLong("nextSkillTime");
				break;
			case NewMarchType.ACT_BOSS:
				actBossState = q.TryGetInt("actbossstate");
				break;
			}
			if (IsAlChallengeKirov())
			{
				ISFSObject iSFSObject6 = q.TryGetObj("allianceChallengeInfo");
				if (iSFSObject6 != null)
				{
					allianceChallengeInfo = new AllianceChallengeInfo();
					allianceChallengeInfo.Update(iSFSObject6);
				}
			}
			if (IsCityBattleS1Monster())
			{
				ISFSObject iSFSObject7 = q.TryGetObj("cityBattleS1MonsterInfo");
				if (iSFSObject7 != null)
				{
					cityBattleS1MonsterInfo = new CityBattleS1MonsterInfo();
					cityBattleS1MonsterInfo.Update(iSFSObject7);
				}
			}
			if (IsBloodyQueenMonster())
			{
				ISFSObject iSFSObject8 = q.TryGetObj("bloodyQueenMonster");
				if (iSFSObject8 != null)
				{
					bloodyQueenMonster = new BloodyQueenMonster();
					bloodyQueenMonster.Update(iSFSObject8);
				}
			}
			_curHp = q.TryGetLong("curHp");
			long num = q.TryGetLong("curHp");
			long num2 = q.TryGetLong("maxHp");
			if (num > 0)
			{
				monsterHpRatio = 100f * (float)num / (float)((num2 == 0L) ? 1 : num2);
				if (IsBloodyQueenMonster())
				{
					monsterHpRatio = (float)Math.Truncate(monsterHpRatio * 100f) / 100f;
				}
			}
			callHelp = 0;
		}
		else if (type == NewMarchType.CHALLENGE_BOSS)
		{
			monsterId = q.TryGetInt("monsterId");
			refreshTime = q.TryGetLong("refreshTime");
			callHelp = q.TryGetInt("callHelp");
			monsterHpRatio = 0f;
		}
		else if (type == NewMarchType.ZOMBIE_RETREAT)
		{
			cityId = q.TryGetInt("cityId");
			npcNum = q.TryGetInt("npcNum");
		}
		else if (type == NewMarchType.TRAIN)
		{
			train = new WorldTrain(q.TryGetObj("train"));
		}
		else if (type == NewMarchType.FLOWER_TRAIN)
		{
			flowerTrain = new WorldFlowerTrain(q);
		}
		else
		{
			monsterId = 0;
			refreshTime = 0L;
			createTime = 0L;
			callHelp = 0;
			monsterHpRatio = 0f;
		}
		if (q.ContainsKey("thermalConductor"))
		{
			thermalConductor = new ThermalConductor(q.TryGetObj("thermalConductor"));
		}
		if (type == NewMarchType.REBUILD_CAR)
		{
			userInfo = q.TryGetObj("userInfo");
		}
		UpdateArmy(q.TryGetArray("combatInfos"));
		isBroken = q.GetBool("isBroken");
		string text2 = q.TryGetString("diffPoint");
		if (!string.IsNullOrEmpty(text2))
		{
			string[] array = text2.Split(new char[1] { ';' });
			stationDir = new Vector2Int(int.Parse(array[0]), int.Parse(array[1]));
		}
		else
		{
			stationDir = Vector2Int.zero;
		}
		allianceAbbr = q.TryGetString("allianceAbbr");
		allianceName = q.TryGetString("allianceName");
		allianceIcon = q.TryGetString("allianceIcon");
		pic = q.TryGetString("pic");
		picVer = q.TryGetInt("picVer");
		headSkinId = q.TryGetInt("headSkinId");
		headSkinET = q.TryGetLong("headSkinET");
		pvpNum = q.TryGetInt("pvpNum");
		pveNum = q.TryGetInt("pveNum");
		baseVirusLayer = q.TryGetInt("baseVirusLayer");
		extraVirusLayer = q.TryGetInt("extraVirusLayer");
		if (GameEntry.Data.Player.IsInBattleField())
		{
			baseVirusLayer = 0;
			extraVirusLayer = 0;
		}
		_clientCreateGuid = q.TryGetString("clientCreateUuid");
		eventUuid = q.TryGetLong("eventUuid");
		if (type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN)
		{
			ISFSArray arr = q.TryGetArray("busList");
			detectZombieBusTrain = new DetectZombieBusTrain();
			detectZombieBusTrain.Update(arr);
		}
	}

	private void UpdateFromProto(ISFSObject q, Protobuf.WorldMarch p)
	{
		IsValid = true;
		_uuid = q.TryGetLong("uuid");
		teamUuid = p.TeamUuid;
		holdUuid = p.HoldUuid;
		ownerUid = q.TryGetString("ownerUid");
		ownerName = p.OwnerName;
		ownerServer = p.OwnerServer;
		ownerCurServerId = p.OwnerCurServerId;
		ownerFormationUuid = p.OwnerFormationUuid;
		string text = p.Path;
		path = (from a in text.Split(new char[1] { ';' })
			select a.ToInt()).ToArray();
		targetPos = p.TargetPos;
		power = p.Power;
		startPos = p.StartPos;
		homePos = p.MainPointId;
		target = SeasonDataManager.CheckCrossMarchTargetType((MarchTargetType)p.Target, ref globalArmy);
		status = (MarchStatus)p.Status;
		targetUuid = p.TargetUuid;
		startTime = p.StartTime;
		endTime = p.EndTime;
		worldId = p.WorldId;
		worldType = p.WorldType;
		secretKey = q.TryGetInt("secretKey");
		if (worldId <= 0)
		{
			blackStartTime = p.BlackStartTime;
			blackEndTime = p.BlackEndTime;
		}
		else
		{
			blackStartTime = 0L;
			blackEndTime = 0L;
		}
		allianceUid = p.AllianceUid;
		allianceAbbr = p.AllianceAbbr;
		type = SeasonDataManager.CheckCrossMarchType((NewMarchType)p.Type, ref globalArmy);
		_ = type;
		_ = 28;
		speed = p.Speed;
		oriSpeed = q.TryGetFloat("oriSpeed");
		blackSpeed = p.BlackSpeed;
		plunderRes = p.PlunderRes;
		collectSpd = p.CollectSpeed;
		armyWeight = p.ArmyWeight;
		inBattle = p.InBattle;
		eventId = q.TryGetString("eventId");
		belongUid = q.TryGetString("belongUid");
		actEndTime = q.TryGetLong("actEnd");
		actStartTime = q.TryGetLong("actStart");
		bossOwnerUid = q.TryGetString("bossOwnerUid");
		serverId = p.Server;
		targetServer = p.TargetServer;
		srcServer = p.SrcServer;
		pathStartServerId = p.PathStartServerId;
		fightMonster = p.FightMonster;
		isAnonymity = p.IsAnonymity;
		fixedSoldierType = p.FixedSoldierType;
		ownerLightUuid = p.OwnerLightUuid;
		catchZombieNum = p.CatchZombieNum;
		isMummyMarch = fixedSoldierType == 4;
		if (pathStartServerId <= 0)
		{
			pathStartServerId = srcServer;
		}
		if (ownerCurServerId == 0)
		{
			ownerCurServerId = ownerServer;
		}
		homeWorldPos = TileCoord.TileIndexToWorld(homePos, ForceChangeScene.World, ownerCurServerId);
		targetWorldPos = TileCoord.TileIndexToWorld(targetPos, ForceChangeScene.World, targetServer);
		startWorldPos = TileCoord.TileIndexToWorld(startPos, ForceChangeScene.World, srcServer);
		bloodyQueenMonsterUuid = p.BloodyQueenMonsterUuid;
		cityBattleS1RestBossUuid = p.CityBattleS1RestBossUuid;
		monsterRallyNum = 0;
		if (p.BankDeposit != null)
		{
			bankDeposit = p.BankDeposit.DepositAmount;
		}
		if (p.BankDeposit != null)
		{
			itemId = p.BankDeposit.ItemId;
		}
		if (type == NewMarchType.MONSTER || type == NewMarchType.BOSS || type == NewMarchType.ACT_BOSS || type == NewMarchType.PUZZLE_BOSS || type == NewMarchType.DARK_KNIGHT_CITY || type == NewMarchType.RUNNING_BOSS || type == NewMarchType.SANDFISH || type == NewMarchType.ZOMBIE_RUSH || type == NewMarchType.MUMMY || type == NewMarchType.ACT_BERSERK_BOSS || type == NewMarchType.BEHEMOTH_BOSS || type == NewMarchType.BEHEMOTN_SKILL || type == NewMarchType.ZONE_MOBILIZATION_BOSS || type == NewMarchType.ALLIANCE_BOSS_SAND)
		{
			monsterId = q.TryGetInt("monsterId");
			refreshTime = q.TryGetLong("refreshTime");
			createTime = q.TryGetLong("createTime");
			monsterHpRatio = q.TryGetInt("running_hp");
			switch (type)
			{
			case NewMarchType.BOSS:
			{
				monsterSpecialType = p.MonsterSpecialType;
				ISFSObject iSFSObject2 = q.TryGetObj("allianceBossInfo");
				if (iSFSObject2 != null)
				{
					allianceBoss = new AllianceBossInfo();
					allianceBoss.Update(iSFSObject2);
				}
				ISFSObject iSFSObject3 = q.TryGetObj("invasionBossInfo");
				if (iSFSObject3 != null)
				{
					invasionBossInfo = new InvasionBossInfo();
					invasionBossInfo.Update(iSFSObject3);
				}
				ISFSObject iSFSObject4 = q.TryGetObj("strongholdBoss");
				if (iSFSObject4 != null)
				{
					IsStrongholdBoss = true;
					strongholdBossNum = iSFSObject4.TryGetLong("curNum");
					strongholdBossMax = iSFSObject4.TryGetLong("maxNum");
					if (strongholdBossMax != 0L)
					{
						monsterHpRatio = (float)strongholdBossNum * 100f / (float)strongholdBossMax;
					}
				}
				ISFSObject iSFSObject5 = q.TryGetObj("ghostKing");
				if (iSFSObject5 != null)
				{
					IsCityBoss = true;
					cityBossNum = iSFSObject5.TryGetLong("curNum");
					cityBossMax = iSFSObject5.TryGetLong("maxNum");
					if (cityBossMax != 0L)
					{
						monsterHpRatio = (float)cityBossNum * 100f / (float)cityBossMax;
					}
				}
				break;
			}
			case NewMarchType.RUNNING_BOSS:
				monsterSpecialType = p.MonsterSpecialType;
				monsterRallyNum = q.TryGetInt("rally_num");
				break;
			case NewMarchType.ZONE_MOBILIZATION_BOSS:
			{
				ISFSObject iSFSObject = q.TryGetObj("zMBossInfo");
				if (iSFSObject != null)
				{
					zMBossInfo = new ZMBossInfo();
					zMBossInfo.Update(iSFSObject);
				}
				break;
			}
			case NewMarchType.ZOMBIE_RUSH:
				zombieRushRound = q.TryGetInt("zombieRushRound");
				zombieRushId = q.TryGetInt("zombieRushId");
				allianceBuildingCfgId = q.TryGetInt("allianceBuildingCfgId");
				break;
			case NewMarchType.ACT_BERSERK_BOSS:
				_berserkBossMetaId = q.TryGetInt("bossId");
				break;
			case NewMarchType.BEHEMOTH_BOSS:
				armyUnit = q.TryGetInt("armyUnit");
				totalArmyUnit = q.TryGetInt("totalArmyUnit");
				monsterHpRatio = (float)armyUnit * 100f / (float)totalArmyUnit;
				nextSkillTime = q.TryGetLong("nextSkillTime");
				break;
			case NewMarchType.MUMMY:
				monsterId = p.MonsterId;
				break;
			}
			if (IsAlChallengeKirov())
			{
				ISFSObject iSFSObject6 = q.TryGetObj("allianceChallengeInfo");
				if (iSFSObject6 != null)
				{
					allianceChallengeInfo = new AllianceChallengeInfo();
					allianceChallengeInfo.Update(iSFSObject6);
				}
			}
			_curHp = q.TryGetLong("curHp");
			long num = q.TryGetLong("curHp");
			long num2 = q.TryGetLong("maxHp");
			if (num > 0)
			{
				monsterHpRatio = (int)(100f * (float)num / (float)((num2 == 0L) ? 1 : num2));
			}
			callHelp = 0;
			if (IsSandWorm())
			{
				if (p.MonsterInfo == null)
				{
					Log.Error("后端下发的沙虫MonsterInfo=null,uuid=" + uuid);
				}
				else
				{
					sandWormData = new SandWormData();
					sandWormData.SetData(p.MonsterInfo, belongUid, ownerName, allianceAbbr, srcServer);
					monsterHpRatio = (float)sandWormData.curHp * 100f / (float)sandWormData.maxHp;
				}
			}
		}
		else if (type == NewMarchType.DARKNESS_MONSTER)
		{
			if (p.MonsterInfo == null)
			{
				Log.Error("后端下发的血夜怪物MonsterInfo=null,uuid=" + uuid);
			}
			else
			{
				monsterId = p.MonsterInfo.MonsterId;
				expireTime = p.MonsterInfo.ExpireTime;
				monsterType = p.MonsterInfo.Type;
				monsterSpecialType = p.MonsterInfo.SpecialType;
				darknessMonsterData = new DarknessMonsterData();
				darknessMonsterData.SetData(p.MonsterInfo);
				monsterHpRatio = darknessMonsterData.hpRatio;
			}
		}
		else if (type == NewMarchType.RUNNING_MUMMY)
		{
			monsterId = q.TryGetInt("monsterId");
			refreshTime = q.TryGetLong("refreshTime");
			createTime = q.TryGetLong("createTime");
			monsterHpRatio = q.TryGetInt("running_hp");
			monsterRallyNum = q.TryGetInt("rally_num");
			expireTime = p.MonsterInfo.ExpireTime;
			monsterId = p.MonsterId;
			_curHp = p.MonsterInfo.CurHp;
			long maxHp = p.MonsterInfo.MaxHp;
			if (_curHp > 0)
			{
				monsterHpRatio = (int)(100f * (float)_curHp / (float)((maxHp == 0L) ? 1 : maxHp));
			}
			callHelp = 0;
			if (status == MarchStatus.STATION)
			{
				speed = 0f;
			}
		}
		else if (type == NewMarchType.CHALLENGE_BOSS)
		{
			monsterId = q.TryGetInt("monsterId");
			refreshTime = q.TryGetLong("refreshTime");
			callHelp = q.TryGetInt("callHelp");
			monsterHpRatio = 0f;
		}
		else if (type == NewMarchType.ZOMBIE_RETREAT)
		{
			cityId = q.TryGetInt("cityId");
			npcNum = q.TryGetInt("npcNum");
		}
		else if (type == NewMarchType.TRAIN)
		{
			train = new WorldTrain(q.TryGetObj("train"));
		}
		else if (type == NewMarchType.FLOWER_TRAIN)
		{
			flowerTrain = new WorldFlowerTrain(q);
		}
		else
		{
			monsterId = 0;
			refreshTime = 0L;
			createTime = 0L;
			callHelp = 0;
			monsterHpRatio = 0f;
		}
		if (q.ContainsKey("thermalConductor"))
		{
			thermalConductor = new ThermalConductor(q.TryGetObj("thermalConductor"));
		}
		UpdateArmy(p.ArmyCombatUnits);
		isBroken = p.IsBroken;
		string diffPoint = p.DiffPoint;
		if (!string.IsNullOrEmpty(diffPoint))
		{
			string[] array = diffPoint.Split(new char[1] { ';' });
			stationDir = new Vector2Int(int.Parse(array[0]), int.Parse(array[1]));
		}
		else
		{
			stationDir = Vector2Int.zero;
		}
		allianceName = p.AllianceName;
		allianceIcon = p.AllianceIcon;
		pic = p.Pic;
		picVer = p.PicVer;
		headSkinId = p.HeadSkinId;
		headSkinET = p.HeadSkinETime;
		pvpNum = p.PvpNum;
		pveNum = p.PveNum;
		baseVirusLayer = p.BaseVirusLayer;
		extraVirusLayer = p.ExtraVirusLayer;
		if (GameEntry.Data.Player.IsInBattleField())
		{
			baseVirusLayer = 0;
			extraVirusLayer = 0;
		}
		_clientCreateGuid = q.TryGetString("clientCreateUuid");
		if (p.MeteoriteInfo != null)
		{
			crystal = p.MeteoriteInfo.Crystal;
			nucleus = p.MeteoriteInfo.Nucleus;
		}
		eventUuid = q.TryGetLong("eventUuid");
		if (type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN)
		{
			ISFSArray arr = q.TryGetArray("busList");
			detectZombieBusTrain = new DetectZombieBusTrain();
			detectZombieBusTrain.Update(arr);
		}
		if (p.StationPointInfo != null)
		{
			WorldPointInfo p2 = WorldPointInfo.Parser.ParseFrom(p.StationPointInfo.ToByteArray());
			StationPointInfo = WorldPointManager.NewPointInfo(p2, isCreate: false);
		}
	}

	public List<DetectZombieBus> GetZombieBusList()
	{
		if (detectZombieBusTrain == null)
		{
			return null;
		}
		return detectZombieBusTrain.ZombieBusList;
	}

	private void UpdateByNewMarchType(ISFSObject q)
	{
	}

	public bool IsFrozen()
	{
		if (thermalConductor != null)
		{
			return thermalConductor.hp > 0;
		}
		return false;
	}

	public bool IsMonster()
	{
		if (type != NewMarchType.MONSTER)
		{
			if (type == NewMarchType.DARKNESS_MONSTER)
			{
				if (monsterType != 14 && monsterType != 18 && monsterType != 15 && monsterType != 19 && monsterType != 16)
				{
					return monsterType == 20;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public bool IsOrdinaryBoss()
	{
		if (type != NewMarchType.BOSS)
		{
			if (type == NewMarchType.DARKNESS_MONSTER)
			{
				if (monsterType != 17)
				{
					return monsterType == 21;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public bool IsBoss()
	{
		if (type != NewMarchType.BOSS && type != NewMarchType.ACT_BOSS && type != NewMarchType.PUZZLE_BOSS && type != NewMarchType.CHALLENGE_BOSS && type != NewMarchType.DARK_KNIGHT_CITY && type != NewMarchType.RUNNING_BOSS && type != NewMarchType.RUNNING_MUMMY && type != NewMarchType.MUMMY && type != NewMarchType.SANDFISH && type != NewMarchType.ACT_BERSERK_BOSS && type != NewMarchType.BEHEMOTH_BOSS && type != NewMarchType.ZONE_MOBILIZATION_BOSS && type != NewMarchType.BLOODY_QUEEN)
		{
			if (type == NewMarchType.DARKNESS_MONSTER)
			{
				if (monsterType != 17 && monsterType != 21 && monsterType != 22 && monsterType != 12)
				{
					return monsterType == 13;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public bool IsMonsterOrBoss()
	{
		if (type != NewMarchType.MONSTER && type != NewMarchType.DARKNESS_MONSTER)
		{
			return IsBoss();
		}
		return true;
	}

	public bool IsMonsterOrOrdinaryBoss()
	{
		if (type != NewMarchType.MONSTER && type != NewMarchType.BOSS)
		{
			if (type == NewMarchType.DARKNESS_MONSTER)
			{
				if (monsterType != 17 && monsterType != 21 && monsterType != 14 && monsterType != 18 && monsterType != 15 && monsterType != 19 && monsterType != 16)
				{
					return monsterType == 20;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public bool IsWanderBoss()
	{
		if (type != NewMarchType.RUNNING_BOSS)
		{
			return type == NewMarchType.RUNNING_MUMMY;
		}
		return true;
	}

	public bool IsWanderMonster()
	{
		if (type == NewMarchType.DARKNESS_MONSTER)
		{
			return status == MarchStatus.MOVING;
		}
		return false;
	}

	public bool IsEVP()
	{
		if (type != NewMarchType.ZOMBIE_RUSH && type != NewMarchType.MUMMY)
		{
			return type == NewMarchType.RUNNING_MUMMY;
		}
		return true;
	}

	public bool IsZombieRushAltered()
	{
		if (type == NewMarchType.ZOMBIE_RUSH && int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result) && result == 45)
		{
			return true;
		}
		return false;
	}

	public bool IsGeneralAllyBoss()
	{
		if (type == NewMarchType.BOSS && monsterId > 0)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "type");
			if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result) && int.TryParse(templateData, out var result2))
			{
				if (result2 != 3 || result != 7)
				{
					if (result2 == 12)
					{
						return result == 34;
					}
					return false;
				}
				return true;
			}
		}
		return false;
	}

	public bool IsS4BNMonsterOrBoss()
	{
		if (type == NewMarchType.DARKNESS_MONSTER)
		{
			if (monsterType != 21 && monsterType != 18 && monsterType != 19)
			{
				return monsterType == 20;
			}
			return true;
		}
		return false;
	}

	public bool IsS4WNMonster()
	{
		if (type == NewMarchType.DARKNESS_MONSTER)
		{
			if (monsterType != 14 && monsterType != 15)
			{
				return monsterType == 16;
			}
			return true;
		}
		return false;
	}

	private void UpdateArmy(ISFSArray arr)
	{
		if (arr == null || arr.Size() == 0)
		{
			armyInfos.Clear();
			return;
		}
		armyInfos.Clear();
		for (int i = 0; i < arr.Count; i++)
		{
			ArmyInfo armyInfo = new ArmyInfo();
			byte[] data = Base64.Decode(arr.GetUtfString(i));
			ArmyCombatUnit armyCombatUnit = ArmyCombatUnit.Parser.ParseFrom(data);
			armyInfo.health = armyCombatUnit.SimpleCombatUnit.Health;
			armyInfo.initHealth = armyCombatUnit.SimpleCombatUnit.InitHealth;
			armyInfo.uuid = armyCombatUnit.SimpleCombatUnit.Uuid;
			armyInfo.uid = armyCombatUnit.SimpleCombatUnit.Uid;
			armyInfo.pic = armyCombatUnit.ArmyInfo.Pic;
			armyInfo.picVer = armyCombatUnit.ArmyInfo.PicVer;
			armyInfo.headSkinId = armyCombatUnit.ArmyInfo.HeadSkinId;
			armyInfo.UpdateArmyList(armyCombatUnit.ArmyInfo);
			armyInfos.Add(armyInfo);
			if (!isMummyMarch && armyInfo.IsMummy())
			{
				isMummyMarch = true;
			}
		}
	}

	private void UpdateArmy(RepeatedField<ArmyCombatUnit> unit)
	{
		if (unit == null || unit.Count == 0)
		{
			armyInfos.Clear();
			return;
		}
		armyInfos.Clear();
		for (int i = 0; i < unit.Count; i++)
		{
			ArmyInfo armyInfo = new ArmyInfo();
			ArmyCombatUnit armyCombatUnit = unit[i];
			armyInfo.health = armyCombatUnit.SimpleCombatUnit.Health;
			armyInfo.initHealth = armyCombatUnit.SimpleCombatUnit.InitHealth;
			armyInfo.uuid = armyCombatUnit.SimpleCombatUnit.Uuid;
			armyInfo.uid = armyCombatUnit.SimpleCombatUnit.Uid;
			armyInfo.pic = armyCombatUnit.ArmyInfo.Pic;
			armyInfo.picVer = armyCombatUnit.ArmyInfo.PicVer;
			armyInfo.headSkinId = armyCombatUnit.ArmyInfo.HeadSkinId;
			armyInfo.UpdateArmyList(armyCombatUnit.ArmyInfo);
			armyInfos.Add(armyInfo);
			if (!isMummyMarch && armyInfo.IsMummy())
			{
				isMummyMarch = true;
			}
		}
	}

	public bool GetIsBroken()
	{
		return isBroken;
	}

	public ArmyInfo GetArmyInfo(long uid)
	{
		foreach (ArmyInfo armyInfo in armyInfos)
		{
			if (armyInfo.uuid == uid)
			{
				return armyInfo;
			}
		}
		return null;
	}

	public ArmyInfo GetFirstArmyInfo()
	{
		foreach (ArmyInfo armyInfo in armyInfos)
		{
			if (armyInfo.uuid == uuid)
			{
				return armyInfo;
			}
		}
		return null;
	}

	public HeroInfo GetLeaderHero()
	{
		return GetFirstArmyInfo()?.GetLeaderHero();
	}

	public int GetSoliderNum()
	{
		int num = 0;
		foreach (ArmyInfo armyInfo in armyInfos)
		{
			foreach (ArmySoldierInfo soldier in armyInfo.Soldiers)
			{
				num += soldier.total - soldier.lost;
			}
		}
		return num;
	}

	public int GetHP()
	{
		int result = 1;
		List<ArmyInfo> list = armyInfos;
		if (list != null && list.Count > 0)
		{
			foreach (ArmyInfo armyInfo in armyInfos)
			{
				if (armyInfo.uid == GameEntry.Data.Player.Uid)
				{
					result = armyInfo.health;
				}
			}
		}
		return result;
	}

	public int GetMaxHP()
	{
		int result = 1;
		List<ArmyInfo> list = armyInfos;
		if (list != null && list.Count > 0)
		{
			foreach (ArmyInfo armyInfo in armyInfos)
			{
				if (armyInfo.uid == GameEntry.Data.Player.Uid)
				{
					result = armyInfo.initHealth;
				}
			}
		}
		return result;
	}

	public int GetMarchTargetType()
	{
		return (int)target;
	}

	public int GetMarchStatus()
	{
		return (int)status;
	}

	public int GetMarchType()
	{
		return (int)type;
	}

	public Vector3 GetMarchCurPos()
	{
		if ((status == MarchStatus.MOVING || status == MarchStatus.BACK_HOME || status == MarchStatus.CHASING || status == MarchStatus.IN_WORM_HOLE) && path.Length > 1)
		{
			float passedLen = GetPassedLen();
			WorldTroop.CalcMoveOnPath(CreatePathSegment(), 0, passedLen, out var _, out var _, out var pos);
			return pos;
		}
		if (status == MarchStatus.TRANSPORT_BACK_HOME)
		{
			return TileCoord.TileIndexToWorld(startPos, ForceChangeScene.World, srcServer);
		}
		return TileCoord.TileIndexToWorld(targetPos, ForceChangeScene.World, targetServer);
	}

	public int GetMarchCurPosIndex()
	{
		return TileCoord.WorldToTileIndex(GetMarchCurPos(), ForceChangeScene.World);
	}

	public WorldTroopPathSegment[] CreatePathSegment()
	{
		if (path.Length < 2)
		{
			return null;
		}
		WorldTroopPathSegment[] array = new WorldTroopPathSegment[path.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = default(WorldTroopPathSegment);
			if (i < array.Length - 1)
			{
				Vector3 vector = TileCoord.TileIndexToWorld(path[i], ForceChangeScene.World, pathStartServerId);
				Vector3 vector2 = TileCoord.TileIndexToWorld(path[i + 1], ForceChangeScene.World, targetServer) - vector;
				array[i].pos = vector;
				array[i].dir = vector2.normalized;
				array[i].dist = vector2.magnitude;
			}
			else
			{
				array[i].pos = TileCoord.TileIndexToWorld(path[i], ForceChangeScene.World, targetServer);
				array[i].dir = array[i - 1].dir;
				array[i].dist = float.MaxValue;
			}
		}
		return array;
	}

	public float GetPassedLen()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (blackEndTime > 0 && blackStartTime > 0)
		{
			if (serverTime <= blackStartTime)
			{
				return (float)(serverTime - startTime) * 0.001f * GetMoveSpeed();
			}
			float num;
			if (serverTime <= blackEndTime)
			{
				num = (float)(blackStartTime - startTime) * 0.001f * GetMoveSpeed();
				return num + (float)(serverTime - blackStartTime) * 0.001f * GetBlackSpeed();
			}
			num = (float)(blackStartTime - startTime) * 0.001f * GetMoveSpeed();
			num += (float)(blackEndTime - blackStartTime) * 0.001f * GetBlackSpeed();
			return num + (float)(serverTime - blackEndTime) * 0.001f * GetMoveSpeed();
		}
		return (float)(serverTime - startTime) * 0.001f * GetMoveSpeed();
	}

	public long GetCurArmyWeight()
	{
		int num = 0;
		if (plunderRes != null)
		{
			int num2 = 0;
			string[] array = plunderRes.Split(new char[1] { ';' });
			foreach (string obj in array)
			{
				int num3 = obj.IndexOf(',');
				num2 = obj.Substring(num3 + 1).ToInt() + num2;
			}
			num = num2;
		}
		return num;
	}

	public float GetResourcePercent()
	{
		double num = (double)(GameEntry.Timer.GetServerTime() - startTime) * 0.001 * (double)collectSpd;
		if (GetCurArmyWeight() == 0L)
		{
			return (float)num / (float)armyWeight;
		}
		return (float)(num + (double)GetCurArmyWeight()) / (float)armyWeight;
	}

	public bool IsMasstroops()
	{
		return false;
	}

	public void InitMove(WorldTroopPathSegment[] pathSegments)
	{
		if (pathSegments == null || pathSegments.Length < 2)
		{
			if (status == MarchStatus.TRANSPORT_BACK_HOME)
			{
				position = TileCoord.TileIndexToWorld(startPos, ForceChangeScene.World, srcServer);
			}
			else if (IsActBerserkBoss())
			{
				position = TileCoord.TileIndexToWorld(startPos, ForceChangeScene.World, srcServer);
			}
			else
			{
				position = TileCoord.TileIndexToWorld(targetPos, ForceChangeScene.World, targetServer);
			}
			curPathLen = 0f;
			pathList = null;
			return;
		}
		int pathIdx = 0;
		moveSpeed = speed * 2f;
		curPathLen = GetPassedLen();
		pathList = pathSegments;
		WorldTroop.CalcMoveOnPath(pathList, 0, curPathLen, out pathIdx, out var _, out position);
		if (pathIdx >= pathList.Length - 1)
		{
			if (type == NewMarchType.TRAIN)
			{
				moveDir = (pathList[pathList.Length - 1].pos - pathList[pathList.Length - 2].pos).normalized;
			}
			curPathLen = 0f;
		}
		else
		{
			Vector3 vector = pathList[pathIdx + 1].pos - position;
			moveDir = vector.normalized;
			curPathLen = vector.magnitude;
		}
	}

	public float GetMoveSpeed()
	{
		return speed * 2f;
	}

	public float GetBlackSpeed()
	{
		return blackSpeed * 2f;
	}

	public void UpdateMove(float deltaTime, long serverNow)
	{
		if (!(curPathLen <= 0f))
		{
			if (firstTick == 0L)
			{
				firstTick = serverNow;
				firstTick_Real = RealTimer.elapsedMilliseconds;
				firstTick_unity = Time.timeSinceLevelLoad * 1000f;
			}
			moveSpeed = speed * SceneManager.World.TileSize;
			if (blackEndTime > 0 && blackStartTime > 0 && blackStartTime <= serverNow && serverNow <= blackEndTime)
			{
				moveSpeed = GetBlackSpeed();
			}
			position += moveDir * (moveSpeed * deltaTime);
			curPathLen -= moveSpeed * deltaTime;
			clientSpend += deltaTime;
			if (curPathLen <= 0f && pathList != null && pathList.Length != 0)
			{
				position = pathList[pathList.Length - 1].pos;
			}
		}
	}

	public void UpdateViewRectState(bool isMarchInView)
	{
		IsInViewRect = isMarchInView;
	}

	public WorldCamp GetCamp()
	{
		if (ownerUid == GameEntry.Data.Player.Uid)
		{
			if (type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN)
			{
				return WorldCamp.Enemy;
			}
			return WorldCamp.Self;
		}
		int num = GameEntry.Data.Player.GetWorldType();
		switch (num)
		{
		case 2:
			return (WorldCamp)GameEntry.Lua.CallWithReturn<int, string, int>("CSharpCallLuaInterface.GetWorldCampInBattleField", ownerUid, num);
		case 3:
			return (WorldCamp)GameEntry.Lua.CallWithReturn<int, string, int>("CSharpCallLuaInterface.GetWorldCampInBattleField", allianceUid, num);
		default:
		{
			if (type == NewMarchType.BEHEMOTH_BOSS)
			{
				return WorldCamp.Enemy;
			}
			string allianceId = GameEntry.Data.Player.GetAllianceId();
			switch (target)
			{
			case MarchTargetType.SCOUT_TREAT:
			case MarchTargetType.SEASON_FARMER_SEND_RES:
			case MarchTargetType.LOTTO_RECEIVE_BASE_REWARD:
			case MarchTargetType.SCOUT_ZONE_MOBILIZATION_DONATE:
			case MarchTargetType.VALENTINE_RECEIVE_BASE_REWARD:
			case MarchTargetType.POWER_WORK_HELPER_CHARGE:
			case MarchTargetType.CHARGE_SUPPLIES:
			case MarchTargetType.ALLIANCE_MONSTER_CHALLENGE_NEW_DONATE:
				return WorldCamp.Ally;
			case MarchTargetType.BERSERK_BOSS_ATTACK_THRONE:
				return WorldCamp.Enemy;
			case MarchTargetType.ZOMBIE_BOSS_ATTACK_CITY:
			case MarchTargetType.DARK_KNIGHT_CITY:
			case MarchTargetType.ALLIANCE_BOSS_SAND_ATTACK_CITY:
			case MarchTargetType.BLOODY_QUEEN_ATTACK_CITY:
				if (allianceUid == allianceId)
				{
					return WorldCamp.Enemy;
				}
				return WorldCamp.Neutral;
			case MarchTargetType.ATTACK_CITY:
			case MarchTargetType.RALLY_FOR_CITY:
			case MarchTargetType.SCOUT_CITY:
			case MarchTargetType.RUNNING_BOSS_ATTACK_CITY:
			case MarchTargetType.SCOUT_WINTER_STORM_CITY:
			case MarchTargetType.ATTACK_WINTER_STORM_CITY:
			case MarchTargetType.ATTACK_EPIDEMIC_CITY:
			case MarchTargetType.SCOUT_EPIDEMIC_CITY:
			case MarchTargetType.RALLY_EPIDEMIC_CITY:
			case MarchTargetType.DARKNESS_MONSTER_ATTACK_CITY:
			case MarchTargetType.SCOUT_OUTPOST_BUILDING:
				if (GameEntry.Data.Building.CheckIsMyBuilding(targetUuid))
				{
					return WorldCamp.Enemy;
				}
				if (string.IsNullOrEmpty(allianceId))
				{
					return WorldCamp.Neutral;
				}
				if (target == MarchTargetType.ATTACK_EPIDEMIC_CITY || target == MarchTargetType.RALLY_EPIDEMIC_CITY)
				{
					int num2 = 3;
					if (worldType == num2 && !GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", allianceUid, num2))
					{
						return WorldCamp.Ally;
					}
				}
				else if (allianceId == allianceUid)
				{
					return WorldCamp.Ally;
				}
				if (SceneManager.MarchDataMgr.IsMemberByPointId(targetPos))
				{
					return WorldCamp.Enemy;
				}
				return WorldCamp.Neutral;
			case MarchTargetType.RUNNING_BOSS_MOVING:
				if (monsterSpecialType == 50 && allianceUid == allianceId)
				{
					return WorldCamp.Enemy;
				}
				if (string.IsNullOrEmpty(allianceId))
				{
					if (SceneManager.MarchDataMgr.IsTargetForMine(this))
					{
						return WorldCamp.Enemy;
					}
				}
				else
				{
					if (allianceId == allianceUid)
					{
						return WorldCamp.Ally;
					}
					if (SceneManager.MarchDataMgr.IsTargetForMine(this))
					{
						return WorldCamp.Enemy;
					}
					if (SceneManager.MarchDataMgr.IsTargetForAlly(this))
					{
						return WorldCamp.Enemy;
					}
				}
				return WorldCamp.Neutral;
			default:
				if (string.IsNullOrEmpty(allianceId))
				{
					if (SceneManager.MarchDataMgr.IsTargetForMine(this))
					{
						return WorldCamp.Enemy;
					}
				}
				else
				{
					if (allianceId == allianceUid)
					{
						return WorldCamp.Ally;
					}
					if (SceneManager.MarchDataMgr.IsTargetForMine(this))
					{
						return WorldCamp.Enemy;
					}
					if (SceneManager.MarchDataMgr.IsTargetForAlly(this))
					{
						return WorldCamp.Enemy;
					}
				}
				return WorldCamp.Neutral;
			}
		}
		}
	}

	public bool NeedCreateTroopLine()
	{
		if (GMSwitch.IsGM && GMSwitch.GetBool("DebugSandFishTroopLineEnable") && type == NewMarchType.SANDFISH)
		{
			return true;
		}
		if (type == NewMarchType.MONSTER || type == NewMarchType.BOSS || type == NewMarchType.TRAIN || type == NewMarchType.ZONE_TRAIN || type == NewMarchType.SANDFISH || type == NewMarchType.ACT_BOSS || type == NewMarchType.FLOWER_TRAIN)
		{
			return false;
		}
		if (type == NewMarchType.DARKNESS_MONSTER)
		{
			if (status == MarchStatus.MOVING)
			{
				return SceneManager.MarchDataMgr.IsTargetForMine(this);
			}
			return false;
		}
		if (type == NewMarchType.ZOMBIE_RETREAT && isFake)
		{
			return false;
		}
		if ((IsWanderBoss() || type == NewMarchType.ZONE_MOBILIZATION_BOSS) && (GameEntry.Timer.GetServerTime() / 1000 >= endTime / 1000 - 1 || pathList == null || pathList.Length == 0))
		{
			return false;
		}
		return true;
	}

	public bool SupportLittleSmartTroopLineMode()
	{
		NewMarchType newMarchType = type;
		if (newMarchType == NewMarchType.ALLIANCE_BOSS_SAND)
		{
			return false;
		}
		return true;
	}

	public bool SupportLittleSmartTroopMode()
	{
		switch (type)
		{
		case NewMarchType.NORMAL:
		case NewMarchType.MUMMY:
		case NewMarchType.FAKE_ATTACK:
		case NewMarchType.CROSS_NORMAL:
			return true;
		default:
			return false;
		}
	}

	public bool IsPerformanceMarch()
	{
		return ShowFiveHero();
	}

	public int GetMarchBlockSize()
	{
		if (blockRecordSize >= 0)
		{
			return blockRecordSize;
		}
		blockRecordSize = 0;
		bool flag = false;
		if (IsMonsterOrOrdinaryBoss())
		{
			if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "special"), out var result))
			{
				switch (result)
				{
				case 6:
				case 7:
				case 8:
				case 20:
				case 22:
				case 27:
				case 29:
				case 40:
				case 42:
				case 44:
				case 47:
				case 48:
				case 49:
				case 51:
					flag = true;
					break;
				}
			}
		}
		else if (type == NewMarchType.ACT_BOSS || IsCityBattleS1Monster() || IsBloodyQueenMonster())
		{
			flag = true;
		}
		if (flag)
		{
			int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "size"), out blockRecordSize);
		}
		return blockRecordSize;
	}

	public void RecordMarchBlock(Dictionary<int, int> blockSet)
	{
		int marchBlockSize = GetMarchBlockSize();
		if (marchBlockSize <= 0)
		{
			return;
		}
		int num = targetPos - 1;
		int num2 = num % 1000;
		int num3 = num / 1000;
		int num4 = marchBlockSize / 2;
		for (int i = -num4; i <= num4; i++)
		{
			for (int j = -num4; j <= num4; j++)
			{
				int num5 = num2 + i;
				int num6 = num3 + j;
				int key = num5 + num6 * 1000 + 1;
				blockSet[key] = srcServer;
			}
		}
	}

	public void RemoveBlockIndex(Dictionary<int, int> blockSet)
	{
		if (blockRecordSize <= 0)
		{
			return;
		}
		int num = targetPos - 1;
		int num2 = num % 1000;
		int num3 = num / 1000;
		int num4 = blockRecordSize / 2;
		for (int i = -num4; i <= num4; i++)
		{
			for (int j = -num4; j <= num4; j++)
			{
				int num5 = num2 + i;
				int num6 = num3 + j;
				int key = num5 + num6 * 1000 + 1;
				blockSet.Remove(key);
			}
		}
	}

	public string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<color=yellow>===WorldMarch===</color>");
		stringBuilder.AppendLine($"uuid:{uuid}");
		stringBuilder.AppendLine("ownerUid:" + (ownerUid ?? "NULL") + "，ownerName:" + (ownerName ?? "NULL") + "，allianceAbbr:" + (allianceAbbr ?? "NULL"));
		stringBuilder.AppendLine("NewMarchType:" + type);
		stringBuilder.AppendLine("MarchStatus:" + status);
		stringBuilder.AppendLine("MarchTargetType:" + target);
		stringBuilder.AppendLine($"targetUuid:{targetUuid}");
		stringBuilder.AppendLine("位置属性:");
		stringBuilder.AppendLine("当前PointId:" + TileCoord.WorldToTileIndex(position, ForceChangeScene.World) + ",当前position:" + position.ToString());
		stringBuilder.Append("path:");
		int[] array = path;
		foreach (int num in array)
		{
			stringBuilder.Append($"{num},");
		}
		stringBuilder.Append("\n");
		stringBuilder.AppendLine($"startPos:{startPos},targetPos:{targetPos},homePos:{homePos}");
		if (startPos != targetPos)
		{
			stringBuilder.AppendLine($"distance:{DistanceFromPointToLine(startWorldPos, targetWorldPos, position)}");
		}
		if (monsterId > 0)
		{
			stringBuilder.AppendLine("怪物属性:");
			stringBuilder.AppendLine($"MonsterId:{monsterId},MonsterType:{monsterType},MonsterSpecialType:{monsterSpecialType},BelongUid:{belongUid}");
		}
		if (crystal + nucleus > 0)
		{
			stringBuilder.AppendLine("陨铁争夺战属性:");
			stringBuilder.AppendLine($"crystal(结晶):{crystal}");
			stringBuilder.AppendLine($"nucleus(晶核):{nucleus}");
		}
		return stringBuilder.ToString();
	}

	public double DistanceFromPointToLine(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 vector = a;
		Vector3 vector2 = b;
		Vector3 vector3 = c;
		float num = vector2.x - vector.x;
		float num2 = vector2.z - vector.z;
		float num3 = vector3.x - vector.x;
		float num4 = vector3.z - vector.z;
		double num5 = Math.Abs(num * num4 - num2 * num3);
		double num6 = Math.Sqrt(num * num + num2 * num2);
		if (Math.Abs(num6) < double.Epsilon)
		{
			return 0.0;
		}
		return num5 / num6;
	}

	public bool UnContinuousMarch()
	{
		if (bloodyQueenMonsterUuid <= 0)
		{
			return cityBattleS1RestBossUuid > 0;
		}
		return true;
	}

	public bool NeedShowLoopAttackEffect()
	{
		return IsBloodyQueenMonster();
	}

	public float GetMonsterAttackRange()
	{
		if (IsBloodyQueenMonster() && float.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "attackRange"), out var result))
		{
			return result * 2f;
		}
		return 0f;
	}
}
