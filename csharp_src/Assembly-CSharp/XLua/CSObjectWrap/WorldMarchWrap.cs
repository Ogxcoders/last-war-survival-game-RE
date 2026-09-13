using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldMarchWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldMarch);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 85, 130, 123);
		Utils.RegisterFunc(L, -3, "IsBirthing", _m_IsBirthing);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "IsMine", _m_IsMine);
		Utils.RegisterFunc(L, -3, "IsMineOrAllyAssembly", _m_IsMineOrAllyAssembly);
		Utils.RegisterFunc(L, -3, "IsScoutMarch", _m_IsScoutMarch);
		Utils.RegisterFunc(L, -3, "GetArmyInfoIndexOne", _m_GetArmyInfoIndexOne);
		Utils.RegisterFunc(L, -3, "GetWorldType", _m_GetWorldType);
		Utils.RegisterFunc(L, -3, "ShowFiveHero", _m_ShowFiveHero);
		Utils.RegisterFunc(L, -3, "GetTrainTailPos", _m_GetTrainTailPos);
		Utils.RegisterFunc(L, -3, "ShowTrain", _m_ShowTrain);
		Utils.RegisterFunc(L, -3, "ShowFlowerTrain", _m_ShowFlowerTrain);
		Utils.RegisterFunc(L, -3, "IsZombieBusTrain", _m_IsZombieBusTrain);
		Utils.RegisterFunc(L, -3, "GetZombieBusTrainTailPos", _m_GetZombieBusTrainTailPos);
		Utils.RegisterFunc(L, -3, "GetZombieBusTrainLength", _m_GetZombieBusTrainLength);
		Utils.RegisterFunc(L, -3, "IsMummyMarch", _m_IsMummyMarch);
		Utils.RegisterFunc(L, -3, "IsDrillBase", _m_IsDrillBase);
		Utils.RegisterFunc(L, -3, "IsDrillBaseTank", _m_IsDrillBaseTank);
		Utils.RegisterFunc(L, -3, "IsDrillBaseNewBossHugeSandWorm", _m_IsDrillBaseNewBossHugeSandWorm);
		Utils.RegisterFunc(L, -3, "IsDrillBaseRoadHog", _m_IsDrillBaseRoadHog);
		Utils.RegisterFunc(L, -3, "IsAisila", _m_IsAisila);
		Utils.RegisterFunc(L, -3, "IsAlChallengeKirov", _m_IsAlChallengeKirov);
		Utils.RegisterFunc(L, -3, "IsActBerserkBoss", _m_IsActBerserkBoss);
		Utils.RegisterFunc(L, -3, "IsSandWorm", _m_IsSandWorm);
		Utils.RegisterFunc(L, -3, "IsS4WanderBoss", _m_IsS4WanderBoss);
		Utils.RegisterFunc(L, -3, "IsFlowerCar", _m_IsFlowerCar);
		Utils.RegisterFunc(L, -3, "GetFlowerCarTailPos", _m_GetFlowerCarTailPos);
		Utils.RegisterFunc(L, -3, "IsAllyBaseAttackCitySandWorm", _m_IsAllyBaseAttackCitySandWorm);
		Utils.RegisterFunc(L, -3, "HasFightMonster", _m_HasFightMonster);
		Utils.RegisterFunc(L, -3, "IsVisibleMarch", _m_IsVisibleMarch);
		Utils.RegisterFunc(L, -3, "IsLightMarch", _m_IsLightMarch);
		Utils.RegisterFunc(L, -3, "IsCityBattleS1Monster", _m_IsCityBattleS1Monster);
		Utils.RegisterFunc(L, -3, "IsBloodyQueenMonster", _m_IsBloodyQueenMonster);
		Utils.RegisterFunc(L, -3, "IsBloodyQueenQueenGunner", _m_IsBloodyQueenQueenGunner);
		Utils.RegisterFunc(L, -3, "IsFlowerTrain", _m_IsFlowerTrain);
		Utils.RegisterFunc(L, -3, "GetFlowerTrainTailPos", _m_GetFlowerTrainTailPos);
		Utils.RegisterFunc(L, -3, "UpdateWorldMarch", _m_UpdateWorldMarch);
		Utils.RegisterFunc(L, -3, "GetZombieBusList", _m_GetZombieBusList);
		Utils.RegisterFunc(L, -3, "IsFrozen", _m_IsFrozen);
		Utils.RegisterFunc(L, -3, "IsMonster", _m_IsMonster);
		Utils.RegisterFunc(L, -3, "IsOrdinaryBoss", _m_IsOrdinaryBoss);
		Utils.RegisterFunc(L, -3, "IsBoss", _m_IsBoss);
		Utils.RegisterFunc(L, -3, "IsMonsterOrBoss", _m_IsMonsterOrBoss);
		Utils.RegisterFunc(L, -3, "IsMonsterOrOrdinaryBoss", _m_IsMonsterOrOrdinaryBoss);
		Utils.RegisterFunc(L, -3, "IsWanderBoss", _m_IsWanderBoss);
		Utils.RegisterFunc(L, -3, "IsWanderMonster", _m_IsWanderMonster);
		Utils.RegisterFunc(L, -3, "IsEVP", _m_IsEVP);
		Utils.RegisterFunc(L, -3, "IsZombieRushAltered", _m_IsZombieRushAltered);
		Utils.RegisterFunc(L, -3, "IsGeneralAllyBoss", _m_IsGeneralAllyBoss);
		Utils.RegisterFunc(L, -3, "IsS4BNMonsterOrBoss", _m_IsS4BNMonsterOrBoss);
		Utils.RegisterFunc(L, -3, "IsS4WNMonster", _m_IsS4WNMonster);
		Utils.RegisterFunc(L, -3, "GetIsBroken", _m_GetIsBroken);
		Utils.RegisterFunc(L, -3, "GetArmyInfo", _m_GetArmyInfo);
		Utils.RegisterFunc(L, -3, "GetFirstArmyInfo", _m_GetFirstArmyInfo);
		Utils.RegisterFunc(L, -3, "GetLeaderHero", _m_GetLeaderHero);
		Utils.RegisterFunc(L, -3, "GetSoliderNum", _m_GetSoliderNum);
		Utils.RegisterFunc(L, -3, "GetHP", _m_GetHP);
		Utils.RegisterFunc(L, -3, "GetMaxHP", _m_GetMaxHP);
		Utils.RegisterFunc(L, -3, "GetMarchTargetType", _m_GetMarchTargetType);
		Utils.RegisterFunc(L, -3, "GetMarchStatus", _m_GetMarchStatus);
		Utils.RegisterFunc(L, -3, "GetMarchType", _m_GetMarchType);
		Utils.RegisterFunc(L, -3, "GetMarchCurPos", _m_GetMarchCurPos);
		Utils.RegisterFunc(L, -3, "GetMarchCurPosIndex", _m_GetMarchCurPosIndex);
		Utils.RegisterFunc(L, -3, "CreatePathSegment", _m_CreatePathSegment);
		Utils.RegisterFunc(L, -3, "GetPassedLen", _m_GetPassedLen);
		Utils.RegisterFunc(L, -3, "GetCurArmyWeight", _m_GetCurArmyWeight);
		Utils.RegisterFunc(L, -3, "GetResourcePercent", _m_GetResourcePercent);
		Utils.RegisterFunc(L, -3, "IsMasstroops", _m_IsMasstroops);
		Utils.RegisterFunc(L, -3, "InitMove", _m_InitMove);
		Utils.RegisterFunc(L, -3, "GetMoveSpeed", _m_GetMoveSpeed);
		Utils.RegisterFunc(L, -3, "GetBlackSpeed", _m_GetBlackSpeed);
		Utils.RegisterFunc(L, -3, "UpdateMove", _m_UpdateMove);
		Utils.RegisterFunc(L, -3, "UpdateViewRectState", _m_UpdateViewRectState);
		Utils.RegisterFunc(L, -3, "GetCamp", _m_GetCamp);
		Utils.RegisterFunc(L, -3, "NeedCreateTroopLine", _m_NeedCreateTroopLine);
		Utils.RegisterFunc(L, -3, "SupportLittleSmartTroopLineMode", _m_SupportLittleSmartTroopLineMode);
		Utils.RegisterFunc(L, -3, "SupportLittleSmartTroopMode", _m_SupportLittleSmartTroopMode);
		Utils.RegisterFunc(L, -3, "IsPerformanceMarch", _m_IsPerformanceMarch);
		Utils.RegisterFunc(L, -3, "GetMarchBlockSize", _m_GetMarchBlockSize);
		Utils.RegisterFunc(L, -3, "RecordMarchBlock", _m_RecordMarchBlock);
		Utils.RegisterFunc(L, -3, "RemoveBlockIndex", _m_RemoveBlockIndex);
		Utils.RegisterFunc(L, -3, "Description", _m_Description);
		Utils.RegisterFunc(L, -3, "DistanceFromPointToLine", _m_DistanceFromPointToLine);
		Utils.RegisterFunc(L, -3, "UnContinuousMarch", _m_UnContinuousMarch);
		Utils.RegisterFunc(L, -3, "NeedShowLoopAttackEffect", _m_NeedShowLoopAttackEffect);
		Utils.RegisterFunc(L, -3, "GetMonsterAttackRange", _m_GetMonsterAttackRange);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "StationPointInfo", _g_get_StationPointInfo);
		Utils.RegisterFunc(L, -2, "MoveDir", _g_get_MoveDir);
		Utils.RegisterFunc(L, -2, "curHp", _g_get_curHp);
		Utils.RegisterFunc(L, -2, "berserkBossMetaId", _g_get_berserkBossMetaId);
		Utils.RegisterFunc(L, -2, "clientCreateGuid", _g_get_clientCreateGuid);
		Utils.RegisterFunc(L, -2, "IsValid", _g_get_IsValid);
		Utils.RegisterFunc(L, -2, "HasMeteorite", _g_get_HasMeteorite);
		Utils.RegisterFunc(L, -2, "IsInViewRect", _g_get_IsInViewRect);
		Utils.RegisterFunc(L, -2, "updateVisibleID", _g_get_updateVisibleID);
		Utils.RegisterFunc(L, -2, "updateTroopID", _g_get_updateTroopID);
		Utils.RegisterFunc(L, -2, "train", _g_get_train);
		Utils.RegisterFunc(L, -2, "flowerTrain", _g_get_flowerTrain);
		Utils.RegisterFunc(L, -2, "allianceBoss", _g_get_allianceBoss);
		Utils.RegisterFunc(L, -2, "invasionBossInfo", _g_get_invasionBossInfo);
		Utils.RegisterFunc(L, -2, "thermalConductor", _g_get_thermalConductor);
		Utils.RegisterFunc(L, -2, "sandWormData", _g_get_sandWormData);
		Utils.RegisterFunc(L, -2, "darknessMonsterData", _g_get_darknessMonsterData);
		Utils.RegisterFunc(L, -2, "zMBossInfo", _g_get_zMBossInfo);
		Utils.RegisterFunc(L, -2, "detectZombieBusTrain", _g_get_detectZombieBusTrain);
		Utils.RegisterFunc(L, -2, "allianceChallengeInfo", _g_get_allianceChallengeInfo);
		Utils.RegisterFunc(L, -2, "cityBattleS1MonsterInfo", _g_get_cityBattleS1MonsterInfo);
		Utils.RegisterFunc(L, -2, "bloodyQueenMonster", _g_get_bloodyQueenMonster);
		Utils.RegisterFunc(L, -2, "ownerUid", _g_get_ownerUid);
		Utils.RegisterFunc(L, -2, "ownerName", _g_get_ownerName);
		Utils.RegisterFunc(L, -2, "ownerServer", _g_get_ownerServer);
		Utils.RegisterFunc(L, -2, "ownerCurServerId", _g_get_ownerCurServerId);
		Utils.RegisterFunc(L, -2, "teamUuid", _g_get_teamUuid);
		Utils.RegisterFunc(L, -2, "allianceUid", _g_get_allianceUid);
		Utils.RegisterFunc(L, -2, "ownerFormationUuid", _g_get_ownerFormationUuid);
		Utils.RegisterFunc(L, -2, "path", _g_get_path);
		Utils.RegisterFunc(L, -2, "targetPos", _g_get_targetPos);
		Utils.RegisterFunc(L, -2, "startPos", _g_get_startPos);
		Utils.RegisterFunc(L, -2, "homePos", _g_get_homePos);
		Utils.RegisterFunc(L, -2, "power", _g_get_power);
		Utils.RegisterFunc(L, -2, "startWorldPos", _g_get_startWorldPos);
		Utils.RegisterFunc(L, -2, "targetWorldPos", _g_get_targetWorldPos);
		Utils.RegisterFunc(L, -2, "homeWorldPos", _g_get_homeWorldPos);
		Utils.RegisterFunc(L, -2, "startTime", _g_get_startTime);
		Utils.RegisterFunc(L, -2, "endTime", _g_get_endTime);
		Utils.RegisterFunc(L, -2, "blackStartTime", _g_get_blackStartTime);
		Utils.RegisterFunc(L, -2, "blackEndTime", _g_get_blackEndTime);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -2, "status", _g_get_status);
		Utils.RegisterFunc(L, -2, "type", _g_get_type);
		Utils.RegisterFunc(L, -2, "isAnonymity", _g_get_isAnonymity);
		Utils.RegisterFunc(L, -2, "targetUuid", _g_get_targetUuid);
		Utils.RegisterFunc(L, -2, "speed", _g_get_speed);
		Utils.RegisterFunc(L, -2, "oriSpeed", _g_get_oriSpeed);
		Utils.RegisterFunc(L, -2, "blackSpeed", _g_get_blackSpeed);
		Utils.RegisterFunc(L, -2, "plunderRes", _g_get_plunderRes);
		Utils.RegisterFunc(L, -2, "worldId", _g_get_worldId);
		Utils.RegisterFunc(L, -2, "worldType", _g_get_worldType);
		Utils.RegisterFunc(L, -2, "collectSpd", _g_get_collectSpd);
		Utils.RegisterFunc(L, -2, "armyWeight", _g_get_armyWeight);
		Utils.RegisterFunc(L, -2, "inBattle", _g_get_inBattle);
		Utils.RegisterFunc(L, -2, "armyInfos", _g_get_armyInfos);
		Utils.RegisterFunc(L, -2, "monsterId", _g_get_monsterId);
		Utils.RegisterFunc(L, -2, "monsterSpecialType", _g_get_monsterSpecialType);
		Utils.RegisterFunc(L, -2, "monsterType", _g_get_monsterType);
		Utils.RegisterFunc(L, -2, "monsterHpRatio", _g_get_monsterHpRatio);
		Utils.RegisterFunc(L, -2, "monsterRallyNum", _g_get_monsterRallyNum);
		Utils.RegisterFunc(L, -2, "zombieRushRound", _g_get_zombieRushRound);
		Utils.RegisterFunc(L, -2, "zombieRushId", _g_get_zombieRushId);
		Utils.RegisterFunc(L, -2, "allianceBuildingCfgId", _g_get_allianceBuildingCfgId);
		Utils.RegisterFunc(L, -2, "refreshTime", _g_get_refreshTime);
		Utils.RegisterFunc(L, -2, "createTime", _g_get_createTime);
		Utils.RegisterFunc(L, -2, "expireTime", _g_get_expireTime);
		Utils.RegisterFunc(L, -2, "actStartTime", _g_get_actStartTime);
		Utils.RegisterFunc(L, -2, "actEndTime", _g_get_actEndTime);
		Utils.RegisterFunc(L, -2, "cityId", _g_get_cityId);
		Utils.RegisterFunc(L, -2, "npcNum", _g_get_npcNum);
		Utils.RegisterFunc(L, -2, "isBroken", _g_get_isBroken);
		Utils.RegisterFunc(L, -2, "stationDir", _g_get_stationDir);
		Utils.RegisterFunc(L, -2, "allianceAbbr", _g_get_allianceAbbr);
		Utils.RegisterFunc(L, -2, "allianceName", _g_get_allianceName);
		Utils.RegisterFunc(L, -2, "allianceIcon", _g_get_allianceIcon);
		Utils.RegisterFunc(L, -2, "eventId", _g_get_eventId);
		Utils.RegisterFunc(L, -2, "eventUuid", _g_get_eventUuid);
		Utils.RegisterFunc(L, -2, "belongUid", _g_get_belongUid);
		Utils.RegisterFunc(L, -2, "pic", _g_get_pic);
		Utils.RegisterFunc(L, -2, "picVer", _g_get_picVer);
		Utils.RegisterFunc(L, -2, "headSkinId", _g_get_headSkinId);
		Utils.RegisterFunc(L, -2, "headSkinET", _g_get_headSkinET);
		Utils.RegisterFunc(L, -2, "pvpNum", _g_get_pvpNum);
		Utils.RegisterFunc(L, -2, "pveNum", _g_get_pveNum);
		Utils.RegisterFunc(L, -2, "baseVirusLayer", _g_get_baseVirusLayer);
		Utils.RegisterFunc(L, -2, "extraVirusLayer", _g_get_extraVirusLayer);
		Utils.RegisterFunc(L, -2, "serverId", _g_get_serverId);
		Utils.RegisterFunc(L, -2, "targetServer", _g_get_targetServer);
		Utils.RegisterFunc(L, -2, "srcServer", _g_get_srcServer);
		Utils.RegisterFunc(L, -2, "pathStartServerId", _g_get_pathStartServerId);
		Utils.RegisterFunc(L, -2, "realTargetPos", _g_get_realTargetPos);
		Utils.RegisterFunc(L, -2, "isSelect", _g_get_isSelect);
		Utils.RegisterFunc(L, -2, "isCameraFollow", _g_get_isCameraFollow);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "pathList", _g_get_pathList);
		Utils.RegisterFunc(L, -2, "bossOwnerUid", _g_get_bossOwnerUid);
		Utils.RegisterFunc(L, -2, "callHelp", _g_get_callHelp);
		Utils.RegisterFunc(L, -2, "secretKey", _g_get_secretKey);
		Utils.RegisterFunc(L, -2, "isFake", _g_get_isFake);
		Utils.RegisterFunc(L, -2, "isFakeAttack", _g_get_isFakeAttack);
		Utils.RegisterFunc(L, -2, "fakeMarchTime", _g_get_fakeMarchTime);
		Utils.RegisterFunc(L, -2, "parentUuid", _g_get_parentUuid);
		Utils.RegisterFunc(L, -2, "delayApply", _g_get_delayApply);
		Utils.RegisterFunc(L, -2, "delayApplyTime", _g_get_delayApplyTime);
		Utils.RegisterFunc(L, -2, "IsStrongholdBoss", _g_get_IsStrongholdBoss);
		Utils.RegisterFunc(L, -2, "strongholdBossNum", _g_get_strongholdBossNum);
		Utils.RegisterFunc(L, -2, "strongholdBossMax", _g_get_strongholdBossMax);
		Utils.RegisterFunc(L, -2, "IsCityBoss", _g_get_IsCityBoss);
		Utils.RegisterFunc(L, -2, "cityBossNum", _g_get_cityBossNum);
		Utils.RegisterFunc(L, -2, "cityBossMax", _g_get_cityBossMax);
		Utils.RegisterFunc(L, -2, "isMummyMarch", _g_get_isMummyMarch);
		Utils.RegisterFunc(L, -2, "fixedSoldierType", _g_get_fixedSoldierType);
		Utils.RegisterFunc(L, -2, "ownerLightUuid", _g_get_ownerLightUuid);
		Utils.RegisterFunc(L, -2, "catchZombieNum", _g_get_catchZombieNum);
		Utils.RegisterFunc(L, -2, "bankDeposit", _g_get_bankDeposit);
		Utils.RegisterFunc(L, -2, "itemId", _g_get_itemId);
		Utils.RegisterFunc(L, -2, "globalArmy", _g_get_globalArmy);
		Utils.RegisterFunc(L, -2, "LightLength", _g_get_LightLength);
		Utils.RegisterFunc(L, -2, "actBossState", _g_get_actBossState);
		Utils.RegisterFunc(L, -2, "userInfo", _g_get_userInfo);
		Utils.RegisterFunc(L, -2, "armyUnit", _g_get_armyUnit);
		Utils.RegisterFunc(L, -2, "totalArmyUnit", _g_get_totalArmyUnit);
		Utils.RegisterFunc(L, -2, "nextSkillTime", _g_get_nextSkillTime);
		Utils.RegisterFunc(L, -2, "crystal", _g_get_crystal);
		Utils.RegisterFunc(L, -2, "nucleus", _g_get_nucleus);
		Utils.RegisterFunc(L, -2, "holdUuid", _g_get_holdUuid);
		Utils.RegisterFunc(L, -2, "bloodyQueenMonsterUuid", _g_get_bloodyQueenMonsterUuid);
		Utils.RegisterFunc(L, -2, "cityBattleS1RestBossUuid", _g_get_cityBattleS1RestBossUuid);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "IsValid", _s_set_IsValid);
		Utils.RegisterFunc(L, -1, "updateVisibleID", _s_set_updateVisibleID);
		Utils.RegisterFunc(L, -1, "updateTroopID", _s_set_updateTroopID);
		Utils.RegisterFunc(L, -1, "train", _s_set_train);
		Utils.RegisterFunc(L, -1, "flowerTrain", _s_set_flowerTrain);
		Utils.RegisterFunc(L, -1, "allianceBoss", _s_set_allianceBoss);
		Utils.RegisterFunc(L, -1, "invasionBossInfo", _s_set_invasionBossInfo);
		Utils.RegisterFunc(L, -1, "thermalConductor", _s_set_thermalConductor);
		Utils.RegisterFunc(L, -1, "sandWormData", _s_set_sandWormData);
		Utils.RegisterFunc(L, -1, "darknessMonsterData", _s_set_darknessMonsterData);
		Utils.RegisterFunc(L, -1, "zMBossInfo", _s_set_zMBossInfo);
		Utils.RegisterFunc(L, -1, "detectZombieBusTrain", _s_set_detectZombieBusTrain);
		Utils.RegisterFunc(L, -1, "allianceChallengeInfo", _s_set_allianceChallengeInfo);
		Utils.RegisterFunc(L, -1, "cityBattleS1MonsterInfo", _s_set_cityBattleS1MonsterInfo);
		Utils.RegisterFunc(L, -1, "bloodyQueenMonster", _s_set_bloodyQueenMonster);
		Utils.RegisterFunc(L, -1, "ownerUid", _s_set_ownerUid);
		Utils.RegisterFunc(L, -1, "ownerName", _s_set_ownerName);
		Utils.RegisterFunc(L, -1, "ownerServer", _s_set_ownerServer);
		Utils.RegisterFunc(L, -1, "ownerCurServerId", _s_set_ownerCurServerId);
		Utils.RegisterFunc(L, -1, "teamUuid", _s_set_teamUuid);
		Utils.RegisterFunc(L, -1, "allianceUid", _s_set_allianceUid);
		Utils.RegisterFunc(L, -1, "ownerFormationUuid", _s_set_ownerFormationUuid);
		Utils.RegisterFunc(L, -1, "path", _s_set_path);
		Utils.RegisterFunc(L, -1, "targetPos", _s_set_targetPos);
		Utils.RegisterFunc(L, -1, "startPos", _s_set_startPos);
		Utils.RegisterFunc(L, -1, "homePos", _s_set_homePos);
		Utils.RegisterFunc(L, -1, "power", _s_set_power);
		Utils.RegisterFunc(L, -1, "startWorldPos", _s_set_startWorldPos);
		Utils.RegisterFunc(L, -1, "targetWorldPos", _s_set_targetWorldPos);
		Utils.RegisterFunc(L, -1, "homeWorldPos", _s_set_homeWorldPos);
		Utils.RegisterFunc(L, -1, "startTime", _s_set_startTime);
		Utils.RegisterFunc(L, -1, "endTime", _s_set_endTime);
		Utils.RegisterFunc(L, -1, "blackStartTime", _s_set_blackStartTime);
		Utils.RegisterFunc(L, -1, "blackEndTime", _s_set_blackEndTime);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
		Utils.RegisterFunc(L, -1, "status", _s_set_status);
		Utils.RegisterFunc(L, -1, "type", _s_set_type);
		Utils.RegisterFunc(L, -1, "isAnonymity", _s_set_isAnonymity);
		Utils.RegisterFunc(L, -1, "targetUuid", _s_set_targetUuid);
		Utils.RegisterFunc(L, -1, "speed", _s_set_speed);
		Utils.RegisterFunc(L, -1, "oriSpeed", _s_set_oriSpeed);
		Utils.RegisterFunc(L, -1, "blackSpeed", _s_set_blackSpeed);
		Utils.RegisterFunc(L, -1, "plunderRes", _s_set_plunderRes);
		Utils.RegisterFunc(L, -1, "worldId", _s_set_worldId);
		Utils.RegisterFunc(L, -1, "worldType", _s_set_worldType);
		Utils.RegisterFunc(L, -1, "collectSpd", _s_set_collectSpd);
		Utils.RegisterFunc(L, -1, "armyWeight", _s_set_armyWeight);
		Utils.RegisterFunc(L, -1, "inBattle", _s_set_inBattle);
		Utils.RegisterFunc(L, -1, "armyInfos", _s_set_armyInfos);
		Utils.RegisterFunc(L, -1, "monsterId", _s_set_monsterId);
		Utils.RegisterFunc(L, -1, "monsterSpecialType", _s_set_monsterSpecialType);
		Utils.RegisterFunc(L, -1, "monsterType", _s_set_monsterType);
		Utils.RegisterFunc(L, -1, "monsterHpRatio", _s_set_monsterHpRatio);
		Utils.RegisterFunc(L, -1, "monsterRallyNum", _s_set_monsterRallyNum);
		Utils.RegisterFunc(L, -1, "zombieRushRound", _s_set_zombieRushRound);
		Utils.RegisterFunc(L, -1, "zombieRushId", _s_set_zombieRushId);
		Utils.RegisterFunc(L, -1, "allianceBuildingCfgId", _s_set_allianceBuildingCfgId);
		Utils.RegisterFunc(L, -1, "refreshTime", _s_set_refreshTime);
		Utils.RegisterFunc(L, -1, "createTime", _s_set_createTime);
		Utils.RegisterFunc(L, -1, "expireTime", _s_set_expireTime);
		Utils.RegisterFunc(L, -1, "actStartTime", _s_set_actStartTime);
		Utils.RegisterFunc(L, -1, "actEndTime", _s_set_actEndTime);
		Utils.RegisterFunc(L, -1, "cityId", _s_set_cityId);
		Utils.RegisterFunc(L, -1, "npcNum", _s_set_npcNum);
		Utils.RegisterFunc(L, -1, "isBroken", _s_set_isBroken);
		Utils.RegisterFunc(L, -1, "stationDir", _s_set_stationDir);
		Utils.RegisterFunc(L, -1, "allianceAbbr", _s_set_allianceAbbr);
		Utils.RegisterFunc(L, -1, "allianceName", _s_set_allianceName);
		Utils.RegisterFunc(L, -1, "allianceIcon", _s_set_allianceIcon);
		Utils.RegisterFunc(L, -1, "eventId", _s_set_eventId);
		Utils.RegisterFunc(L, -1, "eventUuid", _s_set_eventUuid);
		Utils.RegisterFunc(L, -1, "belongUid", _s_set_belongUid);
		Utils.RegisterFunc(L, -1, "pic", _s_set_pic);
		Utils.RegisterFunc(L, -1, "picVer", _s_set_picVer);
		Utils.RegisterFunc(L, -1, "headSkinId", _s_set_headSkinId);
		Utils.RegisterFunc(L, -1, "headSkinET", _s_set_headSkinET);
		Utils.RegisterFunc(L, -1, "pvpNum", _s_set_pvpNum);
		Utils.RegisterFunc(L, -1, "pveNum", _s_set_pveNum);
		Utils.RegisterFunc(L, -1, "baseVirusLayer", _s_set_baseVirusLayer);
		Utils.RegisterFunc(L, -1, "extraVirusLayer", _s_set_extraVirusLayer);
		Utils.RegisterFunc(L, -1, "serverId", _s_set_serverId);
		Utils.RegisterFunc(L, -1, "targetServer", _s_set_targetServer);
		Utils.RegisterFunc(L, -1, "srcServer", _s_set_srcServer);
		Utils.RegisterFunc(L, -1, "pathStartServerId", _s_set_pathStartServerId);
		Utils.RegisterFunc(L, -1, "realTargetPos", _s_set_realTargetPos);
		Utils.RegisterFunc(L, -1, "isSelect", _s_set_isSelect);
		Utils.RegisterFunc(L, -1, "isCameraFollow", _s_set_isCameraFollow);
		Utils.RegisterFunc(L, -1, "position", _s_set_position);
		Utils.RegisterFunc(L, -1, "pathList", _s_set_pathList);
		Utils.RegisterFunc(L, -1, "bossOwnerUid", _s_set_bossOwnerUid);
		Utils.RegisterFunc(L, -1, "callHelp", _s_set_callHelp);
		Utils.RegisterFunc(L, -1, "secretKey", _s_set_secretKey);
		Utils.RegisterFunc(L, -1, "isFake", _s_set_isFake);
		Utils.RegisterFunc(L, -1, "isFakeAttack", _s_set_isFakeAttack);
		Utils.RegisterFunc(L, -1, "fakeMarchTime", _s_set_fakeMarchTime);
		Utils.RegisterFunc(L, -1, "parentUuid", _s_set_parentUuid);
		Utils.RegisterFunc(L, -1, "delayApply", _s_set_delayApply);
		Utils.RegisterFunc(L, -1, "delayApplyTime", _s_set_delayApplyTime);
		Utils.RegisterFunc(L, -1, "IsStrongholdBoss", _s_set_IsStrongholdBoss);
		Utils.RegisterFunc(L, -1, "strongholdBossNum", _s_set_strongholdBossNum);
		Utils.RegisterFunc(L, -1, "strongholdBossMax", _s_set_strongholdBossMax);
		Utils.RegisterFunc(L, -1, "IsCityBoss", _s_set_IsCityBoss);
		Utils.RegisterFunc(L, -1, "cityBossNum", _s_set_cityBossNum);
		Utils.RegisterFunc(L, -1, "cityBossMax", _s_set_cityBossMax);
		Utils.RegisterFunc(L, -1, "isMummyMarch", _s_set_isMummyMarch);
		Utils.RegisterFunc(L, -1, "fixedSoldierType", _s_set_fixedSoldierType);
		Utils.RegisterFunc(L, -1, "ownerLightUuid", _s_set_ownerLightUuid);
		Utils.RegisterFunc(L, -1, "catchZombieNum", _s_set_catchZombieNum);
		Utils.RegisterFunc(L, -1, "bankDeposit", _s_set_bankDeposit);
		Utils.RegisterFunc(L, -1, "itemId", _s_set_itemId);
		Utils.RegisterFunc(L, -1, "globalArmy", _s_set_globalArmy);
		Utils.RegisterFunc(L, -1, "LightLength", _s_set_LightLength);
		Utils.RegisterFunc(L, -1, "actBossState", _s_set_actBossState);
		Utils.RegisterFunc(L, -1, "userInfo", _s_set_userInfo);
		Utils.RegisterFunc(L, -1, "armyUnit", _s_set_armyUnit);
		Utils.RegisterFunc(L, -1, "totalArmyUnit", _s_set_totalArmyUnit);
		Utils.RegisterFunc(L, -1, "nextSkillTime", _s_set_nextSkillTime);
		Utils.RegisterFunc(L, -1, "crystal", _s_set_crystal);
		Utils.RegisterFunc(L, -1, "nucleus", _s_set_nucleus);
		Utils.RegisterFunc(L, -1, "holdUuid", _s_set_holdUuid);
		Utils.RegisterFunc(L, -1, "bloodyQueenMonsterUuid", _s_set_bloodyQueenMonsterUuid);
		Utils.RegisterFunc(L, -1, "cityBattleS1RestBossUuid", _s_set_cityBattleS1RestBossUuid);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				WorldMarch o = new WorldMarch();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarch constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBirthing(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBirthing();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMine(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMine();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMineOrAllyAssembly(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMineOrAllyAssembly();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsScoutMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsScoutMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetArmyInfoIndexOne(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArmyInfo armyInfoIndexOne = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetArmyInfoIndexOne();
			objectTranslator.Push(L, armyInfoIndexOne);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldType(IntPtr L)
	{
		try
		{
			int worldType = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetWorldType();
			Lua.xlua_pushinteger(L, worldType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowFiveHero(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowFiveHero();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTrainTailPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 trainTailPos = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetTrainTailPos();
			objectTranslator.PushUnityEngineVector3(L, trainTailPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowTrain(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowTrain();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowFlowerTrain(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowFlowerTrain();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsZombieBusTrain(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsZombieBusTrain();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZombieBusTrainTailPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 zombieBusTrainTailPos = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetZombieBusTrainTailPos();
			objectTranslator.PushUnityEngineVector3(L, zombieBusTrainTailPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZombieBusTrainLength(IntPtr L)
	{
		try
		{
			float zombieBusTrainLength = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetZombieBusTrainLength();
			Lua.lua_pushnumber(L, zombieBusTrainLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMummyMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMummyMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDrillBase(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDrillBase();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDrillBaseTank(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDrillBaseTank();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDrillBaseNewBossHugeSandWorm(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDrillBaseNewBossHugeSandWorm();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDrillBaseRoadHog(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDrillBaseRoadHog();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAisila(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAisila();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAlChallengeKirov(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAlChallengeKirov();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsActBerserkBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsActBerserkBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSandWorm(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSandWorm();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsS4WanderBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsS4WanderBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFlowerCar(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFlowerCar();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFlowerCarTailPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 flowerCarTailPos = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetFlowerCarTailPos();
			objectTranslator.PushUnityEngineVector3(L, flowerCarTailPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAllyBaseAttackCitySandWorm(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAllyBaseAttackCitySandWorm();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasFightMonster(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HasFightMonster();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsVisibleMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsVisibleMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLightMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsLightMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCityBattleS1Monster(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCityBattleS1Monster();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBloodyQueenMonster(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBloodyQueenMonster();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBloodyQueenQueenGunner(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBloodyQueenQueenGunner();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFlowerTrain(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFlowerTrain();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFlowerTrainTailPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 flowerTrainTailPos = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetFlowerTrainTailPos();
			objectTranslator.PushUnityEngineVector3(L, flowerTrainTailPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateWorldMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				ISFSObject q = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				bool isPushAdd = Lua.lua_toboolean(L, 3);
				worldMarch.UpdateWorldMarch(q, isPushAdd);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<ISFSObject>(L, 2))
			{
				ISFSObject q2 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				worldMarch.UpdateWorldMarch(q2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarch.UpdateWorldMarch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZombieBusList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<DetectZombieBus> zombieBusList = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetZombieBusList();
			objectTranslator.Push(L, zombieBusList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFrozen(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFrozen();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMonster(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMonster();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOrdinaryBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsOrdinaryBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMonsterOrBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMonsterOrBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMonsterOrOrdinaryBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMonsterOrOrdinaryBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWanderBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWanderBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWanderMonster(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWanderMonster();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsEVP(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsEVP();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsZombieRushAltered(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsZombieRushAltered();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGeneralAllyBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsGeneralAllyBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsS4BNMonsterOrBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsS4BNMonsterOrBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsS4WNMonster(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsS4WNMonster();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsBroken(IntPtr L)
	{
		try
		{
			bool isBroken = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetIsBroken();
			Lua.lua_pushboolean(L, isBroken);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetArmyInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch obj = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			long uid = Lua.lua_toint64(L, 2);
			ArmyInfo armyInfo = obj.GetArmyInfo(uid);
			objectTranslator.Push(L, armyInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFirstArmyInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArmyInfo firstArmyInfo = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetFirstArmyInfo();
			objectTranslator.Push(L, firstArmyInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLeaderHero(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeroInfo leaderHero = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetLeaderHero();
			objectTranslator.Push(L, leaderHero);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSoliderNum(IntPtr L)
	{
		try
		{
			int soliderNum = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSoliderNum();
			Lua.xlua_pushinteger(L, soliderNum);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHP(IntPtr L)
	{
		try
		{
			int hP = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHP();
			Lua.xlua_pushinteger(L, hP);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMaxHP(IntPtr L)
	{
		try
		{
			int maxHP = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMaxHP();
			Lua.xlua_pushinteger(L, maxHP);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchTargetType(IntPtr L)
	{
		try
		{
			int marchTargetType = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchTargetType();
			Lua.xlua_pushinteger(L, marchTargetType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchStatus(IntPtr L)
	{
		try
		{
			int marchStatus = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchStatus();
			Lua.xlua_pushinteger(L, marchStatus);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchType(IntPtr L)
	{
		try
		{
			int marchType = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchType();
			Lua.xlua_pushinteger(L, marchType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchCurPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 marchCurPos = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetMarchCurPos();
			objectTranslator.PushUnityEngineVector3(L, marchCurPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchCurPosIndex(IntPtr L)
	{
		try
		{
			int marchCurPosIndex = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchCurPosIndex();
			Lua.xlua_pushinteger(L, marchCurPosIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreatePathSegment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopPathSegment[] o = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).CreatePathSegment();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPassedLen(IntPtr L)
	{
		try
		{
			float passedLen = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPassedLen();
			Lua.lua_pushnumber(L, passedLen);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurArmyWeight(IntPtr L)
	{
		try
		{
			long curArmyWeight = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurArmyWeight();
			Lua.lua_pushint64(L, curArmyWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourcePercent(IntPtr L)
	{
		try
		{
			float resourcePercent = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetResourcePercent();
			Lua.lua_pushnumber(L, resourcePercent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMasstroops(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMasstroops();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitMove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			WorldTroopPathSegment[] pathSegments = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 2, typeof(WorldTroopPathSegment[]));
			worldMarch.InitMove(pathSegments);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMoveSpeed(IntPtr L)
	{
		try
		{
			float moveSpeed = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMoveSpeed();
			Lua.lua_pushnumber(L, moveSpeed);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlackSpeed(IntPtr L)
	{
		try
		{
			float blackSpeed = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetBlackSpeed();
			Lua.lua_pushnumber(L, blackSpeed);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMove(IntPtr L)
	{
		try
		{
			WorldMarch obj = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			long serverNow = Lua.lua_toint64(L, 3);
			obj.UpdateMove(deltaTime, serverNow);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateViewRectState(IntPtr L)
	{
		try
		{
			WorldMarch obj = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isMarchInView = Lua.lua_toboolean(L, 2);
			obj.UpdateViewRectState(isMarchInView);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCamp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamp camp = ((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).GetCamp();
			objectTranslator.Push(L, camp);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NeedCreateTroopLine(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NeedCreateTroopLine();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportLittleSmartTroopLineMode(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SupportLittleSmartTroopLineMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportLittleSmartTroopMode(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SupportLittleSmartTroopMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPerformanceMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPerformanceMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchBlockSize(IntPtr L)
	{
		try
		{
			int marchBlockSize = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchBlockSize();
			Lua.xlua_pushinteger(L, marchBlockSize);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordMarchBlock(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			Dictionary<int, int> blockSet = (Dictionary<int, int>)objectTranslator.GetObject(L, 2, typeof(Dictionary<int, int>));
			worldMarch.RecordMarchBlock(blockSet);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveBlockIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			Dictionary<int, int> blockSet = (Dictionary<int, int>)objectTranslator.GetObject(L, 2, typeof(Dictionary<int, int>));
			worldMarch.RemoveBlockIndex(blockSet);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Description(IntPtr L)
	{
		try
		{
			string str = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Description();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DistanceFromPointToLine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			objectTranslator.Get(L, 4, out Vector3 val3);
			double number = worldMarch.DistanceFromPointToLine(val, val2, val3);
			Lua.lua_pushnumber(L, number);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnContinuousMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnContinuousMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NeedShowLoopAttackEffect(IntPtr L)
	{
		try
		{
			bool value = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NeedShowLoopAttackEffect();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMonsterAttackRange(IntPtr L)
	{
		try
		{
			float monsterAttackRange = ((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMonsterAttackRange();
			Lua.lua_pushnumber(L, monsterAttackRange);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_StationPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.StationPointInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MoveDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldMarch.MoveDir);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curHp(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.curHp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_berserkBossMetaId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.berserkBossMetaId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clientCreateGuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.clientCreateGuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsValid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.IsValid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HasMeteorite(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.HasMeteorite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInViewRect(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.IsInViewRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateVisibleID(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.updateVisibleID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateTroopID(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.updateTroopID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_train(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.train);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flowerTrain(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.flowerTrain);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceBoss(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.allianceBoss);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_invasionBossInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.invasionBossInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_thermalConductor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.thermalConductor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sandWormData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.sandWormData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_darknessMonsterData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.darknessMonsterData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zMBossInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.zMBossInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_detectZombieBusTrain(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.detectZombieBusTrain);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceChallengeInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.allianceChallengeInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityBattleS1MonsterInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.cityBattleS1MonsterInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bloodyQueenMonster(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.bloodyQueenMonster);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerUid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.ownerUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerName(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.ownerName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerServer(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.ownerServer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerCurServerId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.ownerCurServerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_teamUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.teamUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceUid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.allianceUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerFormationUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.ownerFormationUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_path(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.path);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPos(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.targetPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startPos(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.startPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_homePos(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.homePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_power(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.power);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldMarch.startWorldPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldMarch.targetWorldPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_homeWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldMarch.homeWorldPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.startTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.endTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blackStartTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.blackStartTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blackEndTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.blackEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushMarchStatus(L, worldMarch.status);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushNewMarchType(L, worldMarch.type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isAnonymity(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isAnonymity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.targetUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speed(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.speed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_oriSpeed(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.oriSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blackSpeed(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.blackSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_plunderRes(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.plunderRes);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.worldId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldType(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.worldType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_collectSpd(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.collectSpd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_armyWeight(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.armyWeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inBattle(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.inBattle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_armyInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.armyInfos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.monsterId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterSpecialType(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.monsterSpecialType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterType(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.monsterType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterHpRatio(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.monsterHpRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterRallyNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.monsterRallyNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zombieRushRound(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.zombieRushRound);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zombieRushId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.zombieRushId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceBuildingCfgId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.allianceBuildingCfgId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_refreshTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.refreshTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_createTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.createTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_expireTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.expireTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_actStartTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.actStartTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_actEndTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.actEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.cityId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_npcNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.npcNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isBroken(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isBroken);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stationDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.stationDir);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceAbbr(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.allianceAbbr);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceName(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.allianceName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceIcon(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.allianceIcon);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eventId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.eventId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eventUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.eventUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_belongUid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.belongUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pic(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.pic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_picVer(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.picVer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_headSkinId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.headSkinId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_headSkinET(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.headSkinET);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pvpNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.pvpNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pveNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.pveNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_baseVirusLayer(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.baseVirusLayer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extraVirusLayer(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.extraVirusLayer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.serverId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetServer(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.targetServer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_srcServer(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.srcServer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pathStartServerId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.pathStartServerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_realTargetPos(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.realTargetPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isSelect(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isSelect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isCameraFollow(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isCameraFollow);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldMarch.position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pathList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarch.pathList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bossOwnerUid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.bossOwnerUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_callHelp(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.callHelp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_secretKey(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.secretKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isFake(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isFake);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isFakeAttack(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isFakeAttack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fakeMarchTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.fakeMarchTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parentUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.parentUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_delayApply(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.delayApply);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_delayApplyTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.delayApplyTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsStrongholdBoss(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.IsStrongholdBoss);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_strongholdBossNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.strongholdBossNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_strongholdBossMax(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.strongholdBossMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsCityBoss(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.IsCityBoss);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityBossNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.cityBossNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityBossMax(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.cityBossMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isMummyMarch(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.isMummyMarch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fixedSoldierType(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.fixedSoldierType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerLightUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.ownerLightUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_catchZombieNum(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.catchZombieNum);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bankDeposit(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.bankDeposit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemId(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMarch.itemId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_globalArmy(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarch.globalArmy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LightLength(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMarch.LightLength);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_actBossState(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.actBossState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_userInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, worldMarch.userInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_armyUnit(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.armyUnit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalArmyUnit(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.totalArmyUnit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nextSkillTime(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.nextSkillTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_crystal(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.crystal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nucleus(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarch.nucleus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_holdUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.holdUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bloodyQueenMonsterUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.bloodyQueenMonsterUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityBattleS1RestBossUuid(IntPtr L)
	{
		try
		{
			WorldMarch worldMarch = (WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMarch.cityBattleS1RestBossUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsValid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsValid = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateVisibleID(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).updateVisibleID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateTroopID(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).updateTroopID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_train(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).train = (WorldTrain)objectTranslator.GetObject(L, 2, typeof(WorldTrain));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flowerTrain(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).flowerTrain = (WorldFlowerTrain)objectTranslator.GetObject(L, 2, typeof(WorldFlowerTrain));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceBoss(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).allianceBoss = (AllianceBossInfo)objectTranslator.GetObject(L, 2, typeof(AllianceBossInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_invasionBossInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).invasionBossInfo = (InvasionBossInfo)objectTranslator.GetObject(L, 2, typeof(InvasionBossInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_thermalConductor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).thermalConductor = (ThermalConductor)objectTranslator.GetObject(L, 2, typeof(ThermalConductor));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sandWormData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).sandWormData = (SandWormData)objectTranslator.GetObject(L, 2, typeof(SandWormData));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_darknessMonsterData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).darknessMonsterData = (DarknessMonsterData)objectTranslator.GetObject(L, 2, typeof(DarknessMonsterData));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zMBossInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).zMBossInfo = (ZMBossInfo)objectTranslator.GetObject(L, 2, typeof(ZMBossInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_detectZombieBusTrain(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).detectZombieBusTrain = (DetectZombieBusTrain)objectTranslator.GetObject(L, 2, typeof(DetectZombieBusTrain));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceChallengeInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).allianceChallengeInfo = (AllianceChallengeInfo)objectTranslator.GetObject(L, 2, typeof(AllianceChallengeInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityBattleS1MonsterInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).cityBattleS1MonsterInfo = (CityBattleS1MonsterInfo)objectTranslator.GetObject(L, 2, typeof(CityBattleS1MonsterInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bloodyQueenMonster(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).bloodyQueenMonster = (BloodyQueenMonster)objectTranslator.GetObject(L, 2, typeof(BloodyQueenMonster));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerUid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerName(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerServer(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerServer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerCurServerId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerCurServerId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_teamUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).teamUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceUid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerFormationUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerFormationUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_path(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).path = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetPos(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetPos = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startPos(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startPos = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_homePos(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).homePos = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_power(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).power = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldMarch.startWorldPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldMarch.targetWorldPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_homeWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldMarch.homeWorldPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).endTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_blackStartTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).blackStartTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_blackEndTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).blackEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MarchTargetType v);
			worldMarch.target = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MarchStatus val);
			worldMarch.status = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out NewMarchType val);
			worldMarch.type = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isAnonymity(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isAnonymity = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speed(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).speed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_oriSpeed(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).oriSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_blackSpeed(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).blackSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_plunderRes(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).plunderRes = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).worldId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldType(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).worldType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_collectSpd(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).collectSpd = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_armyWeight(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).armyWeight = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inBattle(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).inBattle = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_armyInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).armyInfos = (List<ArmyInfo>)objectTranslator.GetObject(L, 2, typeof(List<ArmyInfo>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).monsterId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterSpecialType(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).monsterSpecialType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterType(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).monsterType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterHpRatio(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).monsterHpRatio = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterRallyNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).monsterRallyNum = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zombieRushRound(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).zombieRushRound = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zombieRushId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).zombieRushId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceBuildingCfgId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceBuildingCfgId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_refreshTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refreshTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_createTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).createTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_expireTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).expireTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_actStartTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).actStartTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_actEndTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).actEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cityId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_npcNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).npcNum = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isBroken(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isBroken = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stationDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			worldMarch.stationDir = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceAbbr(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceAbbr = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceName(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceIcon(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceIcon = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eventId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).eventId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eventUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).eventUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_belongUid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).belongUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pic(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pic = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_picVer(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).picVer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_headSkinId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).headSkinId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_headSkinET(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).headSkinET = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pvpNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pvpNum = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pveNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pveNum = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_baseVirusLayer(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).baseVirusLayer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extraVirusLayer(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).extraVirusLayer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetServer(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetServer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_srcServer(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).srcServer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pathStartServerId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pathStartServerId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_realTargetPos(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).realTargetPos = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isSelect(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isSelect = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isCameraFollow(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isCameraFollow = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldMarch.position = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pathList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).pathList = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 2, typeof(WorldTroopPathSegment[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bossOwnerUid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).bossOwnerUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_callHelp(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).callHelp = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_secretKey(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).secretKey = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isFake(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isFake = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isFakeAttack(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isFakeAttack = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fakeMarchTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fakeMarchTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_parentUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).parentUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_delayApply(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).delayApply = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_delayApplyTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).delayApplyTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsStrongholdBoss(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsStrongholdBoss = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_strongholdBossNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).strongholdBossNum = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_strongholdBossMax(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).strongholdBossMax = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsCityBoss(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCityBoss = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityBossNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cityBossNum = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityBossMax(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cityBossMax = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isMummyMarch(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isMummyMarch = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fixedSoldierType(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fixedSoldierType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerLightUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerLightUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_catchZombieNum(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).catchZombieNum = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bankDeposit(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).bankDeposit = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemId(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).itemId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_globalArmy(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).globalArmy = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LightLength(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LightLength = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_actBossState(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).actBossState = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_userInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMarch)objectTranslator.FastGetCSObj(L, 1)).userInfo = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_armyUnit(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).armyUnit = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalArmyUnit(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalArmyUnit = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nextSkillTime(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).nextSkillTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_crystal(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).crystal = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nucleus(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).nucleus = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_holdUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).holdUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bloodyQueenMonsterUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).bloodyQueenMonsterUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityBattleS1RestBossUuid(IntPtr L)
	{
		try
		{
			((WorldMarch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cityBattleS1RestBossUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
