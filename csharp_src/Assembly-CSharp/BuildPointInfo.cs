using System.Collections.Generic;
using System.Text;
using GameFramework;
using Google.Protobuf.Collections;
using Protobuf;
using UnityEngine;

public class BuildPointInfo : PointInfo
{
	public class WolfCoveredInfo
	{
		public int skinId;

		public int colourId;

		public int titleNameSkinId;

		public int effectId;
	}

	public int itemId;

	public int level;

	public int state;

	public int buildState;

	public string allianceId;

	public int startTime;

	public int endTime;

	public int inside;

	public GameDefines.BuildConnectRoadDirection roadDir;

	public int curHp;

	public int lastHpTime;

	public int protectEndTime;

	public string playerName;

	public string alAbbr;

	public long lastCollectTime;

	public long unavailableTime;

	public int queueItemId;

	public long queueStartTime;

	public long queueUpdateTime;

	public long destroyStartTime;

	public long destroyEndTime;

	public int appearanceId;

	public Protobuf.SpecialType specialType;

	public string positionId;

	public int virusLayer;

	public long virusEndTime;

	public bool refuseTreadVirus;

	public bool refusePowerHelper;

	public LightHouseInfo lightHouseInfo;

	public float recoverSpeed;

	public float fireSpeed;

	public int assistanceCount;

	public int maxAssistanceCount;

	internal AllianceOfficialSkillType aosType;

	private bool isWerewolf;

	public byte wolfDecrHp;

	public RepeatedField<FireWorksInfo> fireworksInfoList;

	public RepeatedField<FireWorksGift> fireworksGiftList;

	public long aosEndTime;

	public int skillId;

	public SandWormData sandWorm;

	public int mummyConvertId;

	public int mummyConvertCount;

	public int skinId;

	public int colourId;

	public float colourTime;

	public int titleNameSkinId;

	public int effectId;

	public WolfCoveredInfo wolfCoveredInfo;

	public int crystal;

	public int nucleus;

	public string countryFlag;

	public MonsterInvasion monsterInvasion;

	public ZoneMobilization zoneMobilization;

	public CommonMonsterSkillInfo commonMonsterSkillInfo;

	public MonsterChallengeNewDonate challangeNewDonate;

	public int seasonRole;

	public int quarantineSkillId;

	public long quarantineSkillSTime;

	public long quarantineSkillETime;

	public int quarantineSkilTargetPId;

	public int quarantineRole;

	public int quarantineLeave;

	public int curMaxHp;

	public long mummyCurseExpireTime;

	public AllianceOfficialSkillType AOSType
	{
		get
		{
			if (aosType == AllianceOfficialSkillType.None)
			{
				return AllianceOfficialSkillType.None;
			}
			if (GameEntry.Timer.GetServerTime() > aosEndTime)
			{
				Log.Info($"[TimeOver] SkillChantInfo : {ownerUid} , {mainIndex} , {aosEndTime} , {aosType}");
				aosType = AllianceOfficialSkillType.None;
				return AllianceOfficialSkillType.None;
			}
			return aosType;
		}
	}

	public bool IsWerewolf => isWerewolf;

	public int GetAOSType()
	{
		return (int)AOSType;
	}

	public BuildPointInfo()
	{
	}

	public BuildPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		BuildInfo buildInfo = pi.BuildInfo;
		uuid = buildInfo.Uuid;
		itemId = buildInfo.BuildId;
		level = buildInfo.Level;
		state = buildInfo.QueueState;
		buildState = buildInfo.BuildState;
		allianceId = buildInfo.AllianceId;
		startTime = buildInfo.UpdateStartTime;
		endTime = buildInfo.UpdateEndTime;
		inside = buildInfo.Inside;
		ownerUid = buildInfo.OwnerUid;
		roadDir = GameDefines.BuildConnectRoadDirection.None;
		curHp = buildInfo.CurrentHp;
		recoverSpeed = buildInfo.RecoverSpeed;
		fireSpeed = buildInfo.FireSpeed;
		lastHpTime = buildInfo.LastHpTime;
		protectEndTime = buildInfo.ProtectEndTime;
		appearanceId = buildInfo.AppearanceId;
		playerName = buildInfo.Name;
		alAbbr = buildInfo.AlAbbr;
		if (buildInfo.ThermalConductor != null)
		{
			thermalConductor = new ThermalConductor(buildInfo.ThermalConductor);
		}
		if (buildInfo.Sandworm != null)
		{
			sandWorm = new SandWormData();
			sandWorm.SetData(buildInfo.Sandworm, buildInfo.OwnerUid, buildInfo.Name, buildInfo.AlAbbr, pi.SrcServerId);
		}
		lastCollectTime = (long)buildInfo.LastCollectTime * 1000L;
		unavailableTime = (long)buildInfo.UnavailableTime * 1000L;
		queueItemId = buildInfo.QueueItemId;
		queueStartTime = (long)buildInfo.QueueStartTime * 1000L;
		queueUpdateTime = (long)buildInfo.QueueUpdateTime * 1000L;
		destroyStartTime = (long)buildInfo.DestroyStartTime * 1000L;
		destroyEndTime = buildInfo.DestroyEndTimeMs;
		tileSize = SceneManager.World.GetBuildTileByItemId(itemId);
		if (base.isMainPoint)
		{
			RefreshRoadDir();
		}
		if (itemId == 10100000 || itemId == 735000)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			foreach (Skin skin in buildInfo.Skins)
			{
				if (skin.Type == 1 && (skin.SkinET > serverTime || skin.SkinET == 0L))
				{
					skinId = skin.SkinId;
					colourId = skin.ColourId;
					colourTime = skin.ColourTime;
				}
				else if (skin.Type == 3 && (skin.SkinET > serverTime || skin.SkinET == 0L))
				{
					titleNameSkinId = skin.SkinId;
				}
				else if (skin.Type == 4 && (skin.SkinET > serverTime || skin.SkinET == 0L))
				{
					effectId = skin.SkinId;
				}
			}
		}
		positionId = buildInfo.PositionId;
		specialType = buildInfo.SpecialType;
		virusLayer = buildInfo.VirusLayer;
		virusEndTime = buildInfo.VirusEndTime;
		refuseTreadVirus = buildInfo.RefuseTreadVirus;
		refusePowerHelper = buildInfo.RefusePowerHelper;
		lightHouseInfo = buildInfo.LightHouseInfo;
		seasonRole = buildInfo.SeasonRole;
		status = buildInfo.Status;
		countryFlag = buildInfo.Country;
		if (buildInfo.MonsterInvasion != null)
		{
			monsterInvasion = new MonsterInvasion(buildInfo.MonsterInvasion);
		}
		if (buildInfo.ZoneMobilization != null)
		{
			zoneMobilization = new ZoneMobilization(buildInfo.ZoneMobilization);
		}
		if (buildInfo.CommonMonsterSkillInfo != null)
		{
			commonMonsterSkillInfo = new CommonMonsterSkillInfo(buildInfo.CommonMonsterSkillInfo);
		}
		if (buildInfo.ChallengeNewDonate != null)
		{
			challangeNewDonate = new MonsterChallengeNewDonate(buildInfo.ChallengeNewDonate);
		}
		if (GameEntry.Data.Player.IsInBattleField())
		{
			virusLayer = 0;
			virusEndTime = 0L;
			refuseTreadVirus = true;
			refusePowerHelper = true;
		}
		aosType = AllianceOfficialSkillType.None;
		if (buildInfo.SkillChantInfo != null)
		{
			long serverTime2 = GameEntry.Timer.GetServerTime();
			aosEndTime = buildInfo.SkillChantInfo.OverTime;
			if (serverTime2 < aosEndTime)
			{
				string tabName = "alliance_government_skill";
				int num = (skillId = buildInfo.SkillChantInfo.SkillId);
				string text = GameEntry.ConfigCache.TryGetTemplateData(tabName, num, "skill_flag");
				if (num == 10001 || num == 10004 || text == "1" || num == 20001)
				{
					aosType = AllianceOfficialSkillType.AresMissile;
				}
				else if (num == 10002 || text == "2")
				{
					aosType = AllianceOfficialSkillType.GoddessMummy;
				}
				else if (num == 10003 || text == "3")
				{
					aosType = AllianceOfficialSkillType.MissileFactory;
				}
				Log.Info($"[Active] SkillChantInfo : {ownerUid} , {mainIndex} , {serverTime2} , {aosEndTime} , {num} , {aosType}");
			}
			else
			{
				int num2 = buildInfo.SkillChantInfo.SkillId;
				Log.Info($"[TimeOver] SkillChantInfo : {ownerUid} , {mainIndex} , {serverTime2} , {aosEndTime} , {num2} , {aosType}");
			}
		}
		if (buildInfo.MummyChangeInfo != null)
		{
			mummyConvertId = buildInfo.MummyChangeInfo.Id;
			mummyConvertCount = buildInfo.MummyChangeInfo.Num;
		}
		if (specialType == Protobuf.SpecialType.DetectEvent && ownerUid != GameEntry.Data.Player.Uid)
		{
			tileSize = 1;
		}
		if (buildInfo.MeteoriteInfo != null)
		{
			crystal = buildInfo.MeteoriteInfo.Crystal;
			nucleus = buildInfo.MeteoriteInfo.Nucleus;
		}
		if (buildInfo.QuarantineSkill != null)
		{
			quarantineSkillId = buildInfo.QuarantineSkill.SkilId;
			quarantineSkillSTime = buildInfo.QuarantineSkill.ActiveStartTime;
			quarantineSkillETime = buildInfo.QuarantineSkill.ActiveEndTime;
			quarantineSkilTargetPId = buildInfo.QuarantineSkill.TagetPointId;
		}
		else
		{
			quarantineSkillId = 0;
			quarantineSkillSTime = 0L;
			quarantineSkillETime = 0L;
			quarantineSkilTargetPId = 0;
		}
		quarantineRole = buildInfo.QuarantineRole;
		quarantineLeave = buildInfo.QuarantineLeave;
		curMaxHp = buildInfo.CurrentMaxHp;
		isWerewolf = false;
		wolfCoveredInfo = null;
		mummyCurseExpireTime = 0L;
		if (status != null && status.Count > 0)
		{
			foreach (Status item in status)
			{
				if (item.Id == 704102)
				{
					if (GameEntry.Timer.GetServerTime() < item.ExpireTime)
					{
						isWerewolf = true;
						wolfCoveredInfo = new WolfCoveredInfo
						{
							colourId = colourId,
							skinId = skinId,
							titleNameSkinId = titleNameSkinId,
							effectId = effectId
						};
						skinId = -89757;
						colourId = 0;
						titleNameSkinId = 0;
						effectId = 0;
					}
					break;
				}
				if (item.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1)
				{
					mummyCurseExpireTime = item.ExpireTime;
				}
			}
		}
		wolfDecrHp = (byte)buildInfo.WolfDecrHp;
		UpdateAssistanceCount();
		UpdateFireworksInfoList(buildInfo);
		UpdateFireworksGiftList(buildInfo);
	}

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	private void UpdateFireworksInfoList(BuildInfo buildInfo)
	{
		fireworksInfoList = buildInfo.Fireworks;
		GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateFireworkList", buildInfo.OwnerUid, buildInfo.Fireworks);
	}

	private void UpdateFireworksGiftList(BuildInfo buildInfo)
	{
		fireworksGiftList = buildInfo.FireWorksGiftList;
		GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateFireworkGiftList", buildInfo.OwnerUid, buildInfo.FireWorksGiftList);
	}

	public override PointInfo Clone()
	{
		BuildPointInfo buildPointInfo = new BuildPointInfo();
		BaseClone(buildPointInfo);
		buildPointInfo.uuid = uuid;
		buildPointInfo.itemId = itemId;
		buildPointInfo.buildState = buildState;
		buildPointInfo.level = level;
		buildPointInfo.state = state;
		buildPointInfo.allianceId = allianceId;
		buildPointInfo.startTime = startTime;
		buildPointInfo.endTime = endTime;
		buildPointInfo.inside = inside;
		buildPointInfo.roadDir = roadDir;
		buildPointInfo.ownerUid = ownerUid;
		buildPointInfo.curHp = curHp;
		buildPointInfo.lastHpTime = lastHpTime;
		buildPointInfo.protectEndTime = protectEndTime;
		buildPointInfo.alAbbr = alAbbr;
		buildPointInfo.playerName = playerName;
		buildPointInfo.lastCollectTime = lastCollectTime;
		buildPointInfo.unavailableTime = unavailableTime;
		buildPointInfo.queueItemId = queueItemId;
		buildPointInfo.queueStartTime = queueStartTime;
		buildPointInfo.queueUpdateTime = queueUpdateTime;
		buildPointInfo.destroyStartTime = destroyStartTime;
		buildPointInfo.specialType = specialType;
		buildPointInfo.positionId = positionId;
		buildPointInfo.skinId = skinId;
		buildPointInfo.titleNameSkinId = titleNameSkinId;
		buildPointInfo.countryFlag = countryFlag;
		buildPointInfo.effectId = effectId;
		buildPointInfo.virusLayer = virusLayer;
		buildPointInfo.virusEndTime = virusEndTime;
		buildPointInfo.refuseTreadVirus = refuseTreadVirus;
		buildPointInfo.refusePowerHelper = refusePowerHelper;
		buildPointInfo.lightHouseInfo = lightHouseInfo;
		buildPointInfo.status = status;
		buildPointInfo.isWerewolf = isWerewolf;
		buildPointInfo.wolfDecrHp = wolfDecrHp;
		buildPointInfo.wolfCoveredInfo = wolfCoveredInfo;
		buildPointInfo.monsterInvasion = monsterInvasion;
		buildPointInfo.zoneMobilization = zoneMobilization;
		buildPointInfo.commonMonsterSkillInfo = commonMonsterSkillInfo;
		buildPointInfo.challangeNewDonate = challangeNewDonate;
		buildPointInfo.seasonRole = seasonRole;
		buildPointInfo.mummyConvertId = mummyConvertId;
		buildPointInfo.mummyConvertCount = mummyConvertCount;
		buildPointInfo.crystal = crystal;
		buildPointInfo.nucleus = nucleus;
		buildPointInfo.aosType = aosType;
		buildPointInfo.aosEndTime = aosEndTime;
		buildPointInfo.quarantineSkillId = quarantineSkillId;
		buildPointInfo.quarantineSkillSTime = quarantineSkillSTime;
		buildPointInfo.quarantineSkillETime = quarantineSkillETime;
		buildPointInfo.quarantineSkilTargetPId = quarantineSkilTargetPId;
		buildPointInfo.quarantineRole = quarantineRole;
		buildPointInfo.quarantineLeave = quarantineLeave;
		buildPointInfo.curMaxHp = curMaxHp;
		buildPointInfo.skillId = skillId;
		buildPointInfo.colourId = colourId;
		buildPointInfo.colourTime = colourTime;
		buildPointInfo.skillId = skillId;
		buildPointInfo.assistanceCount = assistanceCount;
		buildPointInfo.maxAssistanceCount = maxAssistanceCount;
		buildPointInfo.fireworksInfoList = fireworksInfoList;
		buildPointInfo.fireworksGiftList = fireworksGiftList;
		buildPointInfo.mummyCurseExpireTime = mummyCurseExpireTime;
		return buildPointInfo;
	}

	public QueueState GetShowState()
	{
		return GetQueueStateFromMixValue(state, 0);
	}

	public QueueState GetShowStateByIndex(int index)
	{
		return GetQueueStateFromMixValue(state, index);
	}

	public bool IsThisState(QueueState needState)
	{
		if (needState == QueueState.DEFAULT)
		{
			return state == 0;
		}
		return ((state << (int)(31 - needState)) & int.MinValue) == int.MinValue;
	}

	private QueueState GetQueueStateFromMixValue(int queueMixState, int stateLevel)
	{
		int num = 0;
		int num2 = 32;
		for (int i = 0; i < 32; i++)
		{
			if (((queueMixState << i) & int.MinValue) == int.MinValue)
			{
				if (num >= stateLevel)
				{
					num2 = 31 - i;
					break;
				}
				num++;
			}
		}
		if (num2 < 32)
		{
			return (QueueState)num2;
		}
		return QueueState.DEFAULT;
	}

	public int GetSkinId()
	{
		return GetSkinId(skinId, colourId, colourTime);
	}

	public static int GetSkinId(int skinId, int colourId, float colourTime)
	{
		if (colourId > 0 && (colourTime <= 0f || (float)GameEntry.Timer.GetServerTime() < colourTime))
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("decoration_colorful_skin", colourId, "decoration_id_new");
			if (!string.IsNullOrEmpty(templateData) && int.TryParse(templateData, out var result))
			{
				return result;
			}
		}
		return skinId;
	}

	public static string GetModelPathById(int skinId, int pointType, bool isNormal, int pointIndex, int levelId, long uuid)
	{
		if (skinId == -89757)
		{
			return "Assets/Main/SeasonRes/S4/Prefabs/Building/A_build_werewolf_world.prefab";
		}
		string text = ((skinId > 0) ? GetSkinModelNameById(skinId, pointType) : (isNormal ? GetDefaultModelNameByLevel(levelId, pointType) : GameEntry.Lua.CallWithReturn<string, long>("CSharpCallLuaInterface.GetDetectEventModelByUuid", uuid)));
		if (string.IsNullOrEmpty(text))
		{
			Log.Info($"[WorldException]Missing build model path, skinId = {skinId}");
			return "Assets/Main/Prefabs/Building/building_10100035_world.prefab";
		}
		if (!text.StartsWith("Assets/"))
		{
			return $"Assets/Main/Prefabs/Building/{text}.prefab";
		}
		return text;
	}

	public string GetModelPath()
	{
		return GetModelPathById(skinId, base.PointType, IsNormalType(), pointIndex, itemId + level, uuid);
	}

	public static string GetSkinModelNameById(int skinId, int pointType)
	{
		string text = "";
		if (WorldScene.ModelPathDic.TryGetValue(skinId, out var value) && value.TryGetValue(pointType, out var value2))
		{
			text = value2;
		}
		if (text.IsNullOrEmpty())
		{
			if (text.IsNullOrEmpty())
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "model_world_path");
			}
			if (text.IsNullOrEmpty())
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "model_world");
			}
			if (!WorldScene.ModelPathDic.ContainsKey(skinId))
			{
				WorldScene.ModelPathDic[skinId] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[skinId][pointType] = text;
		}
		return text;
	}

	public string GetSkinModelName()
	{
		return GetSkinModelNameById(skinId, base.PointType);
	}

	public static string GetDefaultModelNameByLevel(int levelId, int pointType)
	{
		string text = "";
		if (WorldScene.ModelPathDic.TryGetValue(levelId, out var value) && value.TryGetValue(pointType, out var value2))
		{
			text = value2;
		}
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.ConfigCache.GetTemplateData("building", levelId, "model_world_path");
			if (text.IsNullOrEmpty())
			{
				text = GameEntry.ConfigCache.GetTemplateData("building", levelId, "model_world");
			}
			if (!text.IsNullOrEmpty())
			{
				if (!WorldScene.ModelPathDic.ContainsKey(levelId))
				{
					WorldScene.ModelPathDic[levelId] = new Dictionary<int, string>();
				}
				WorldScene.ModelPathDic[levelId][pointType] = text;
			}
		}
		return text;
	}

	public string GetDefaultModelName()
	{
		return GetDefaultModelNameByLevel(itemId + level, base.PointType);
	}

	public void RefreshRoadDir()
	{
		roadDir = GameDefines.BuildConnectRoadDirection.None;
		bool flag = false;
		for (int i = 0; i < GameDefines.BuildConnectList.Count; i++)
		{
			if (flag)
			{
				continue;
			}
			GameDefines.BuildConnectRoadDirection dir = GameDefines.BuildConnectList[i];
			List<int> posIndexListByDir = GetPosIndexListByDir(dir);
			if (posIndexListByDir == null)
			{
				continue;
			}
			for (int j = 0; j < posIndexListByDir.Count; j++)
			{
				PointInfo pointInfo = SceneManager.World.GetPointInfo(posIndexListByDir[j]);
				if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerRoad && pointInfo.ownerUid == ownerUid && itemId != 10100000)
				{
					flag = true;
					roadDir = dir;
					UpdateAllRoadDir();
				}
			}
		}
	}

	public List<int> GetPosIndexListByDir(GameDefines.BuildConnectRoadDirection dir)
	{
		List<int> list = new List<int>();
		switch (tileSize)
		{
		case 1:
			switch (dir)
			{
			case GameDefines.BuildConnectRoadDirection.Down:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 0, -1));
				break;
			case GameDefines.BuildConnectRoadDirection.Left:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -1));
				break;
			case GameDefines.BuildConnectRoadDirection.Right:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 1));
				break;
			case GameDefines.BuildConnectRoadDirection.Top:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 0, 1));
				break;
			}
			break;
		case 2:
			switch (dir)
			{
			case GameDefines.BuildConnectRoadDirection.Down:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 0, -2));
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -1, -2));
				break;
			case GameDefines.BuildConnectRoadDirection.Left:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -2));
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -2, -1));
				break;
			case GameDefines.BuildConnectRoadDirection.Right:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 1));
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 1, -1));
				break;
			case GameDefines.BuildConnectRoadDirection.Top:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 0, 1));
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -1, 1));
				break;
			}
			break;
		case 3:
			switch (dir)
			{
			case GameDefines.BuildConnectRoadDirection.Down:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -1, -3));
				break;
			case GameDefines.BuildConnectRoadDirection.Left:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -3, -1));
				break;
			case GameDefines.BuildConnectRoadDirection.Right:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, 1, -1));
				break;
			case GameDefines.BuildConnectRoadDirection.Top:
				list.Add(SceneManager.World.GetIndexByOffset(mainIndex, -1, 1));
				break;
			}
			break;
		}
		return list;
	}

	public void ChangeRoadDir(GameDefines.BuildConnectRoadDirection dir)
	{
		if (dir > roadDir && itemId != 10100000)
		{
			roadDir = dir;
			UpdateAllRoadDir();
		}
	}

	private void UpdateAllRoadDir()
	{
		for (int i = 0; i < tileSize; i++)
		{
			for (int j = 0; j < tileSize; j++)
			{
				int indexByOffset = SceneManager.World.GetIndexByOffset(mainIndex, -i, -j);
				PointInfo pointInfo = SceneManager.World.GetPointInfo(indexByOffset);
				if (pointInfo != null && pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo.ownerUid == ownerUid && pointInfo is BuildPointInfo buildPointInfo)
				{
					buildPointInfo.roadDir = roadDir;
				}
			}
		}
	}

	public bool IsNormalType()
	{
		return specialType == Protobuf.SpecialType.None;
	}

	public bool IsWormWrap()
	{
		return sandWorm != null;
	}

	public override PlayerType GetPlayerType()
	{
		if (GameEntry.Data.Player.GetUid() == ownerUid)
		{
			return PlayerType.PlayerSelf;
		}
		string text = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(text))
		{
			return PlayerType.PlayerOther;
		}
		if (text == allianceId)
		{
			string allianceLeaderID = GameEntry.Data.Player.GetAllianceLeaderID();
			if (ownerUid != allianceLeaderID)
			{
				return PlayerType.PlayerAlliance;
			}
			return PlayerType.PlayerAllianceLeader;
		}
		return PlayerType.PlayerOther;
	}

	public float GetResourcePercent()
	{
		return GameEntry.Lua.CallWithReturn<float, int, int, long, long>("CSharpCallLuaInterface.GetResourcePercent", itemId, level, unavailableTime, lastCollectTime);
	}

	public bool IsWerewolfBirthing()
	{
		if (!isWerewolf)
		{
			return false;
		}
		foreach (Status item in status)
		{
			if (item.Id == 704102)
			{
				long serverTime = GameEntry.Timer.GetServerTime();
				if (Mathf.Abs(item.BeginTime - serverTime) < 2000f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void OnDescription(StringBuilder sb)
	{
		sb.AppendLine("---BuildPointInfo---");
		sb.AppendLine("Alliance ID: " + allianceId);
		sb.AppendLine("陨铁争夺战属性:");
		sb.AppendLine($"crystal(结晶):{crystal}");
		sb.AppendLine($"nucleus(结晶):{nucleus}");
		sb.AppendLine($"AOSType:{aosType}");
		sb.AppendLine($"proTime:{protectEndTime}");
		sb.AppendLine($"assistance:{assistanceCount}");
		sb.AppendLine($"maxAssistanceCount:{maxAssistanceCount}");
	}

	public void UncoverWerewolf(long now)
	{
		if (status == null || status.Count <= 0)
		{
			return;
		}
		foreach (Status item in status)
		{
			if (item.Id == 704102)
			{
				if (now > item.ExpireTime)
				{
					isWerewolf = false;
					skinId = wolfCoveredInfo.skinId;
					colourId = wolfCoveredInfo.colourId;
					titleNameSkinId = wolfCoveredInfo.titleNameSkinId;
					effectId = wolfCoveredInfo.effectId;
					wolfCoveredInfo = null;
				}
				break;
			}
		}
	}

	protected override void BeforeSetPointStatus()
	{
		mummyCurseExpireTime = 0L;
	}

	protected override void OnAddPointState(Status status)
	{
		if (status != null && status.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1)
		{
			mummyCurseExpireTime = status.ExpireTime;
		}
	}
}
