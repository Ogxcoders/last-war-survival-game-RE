using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldTroopWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldTroop);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 149, 18, 9);
		Utils.RegisterFunc(L, -3, "UpdateLod", _m_UpdateLod);
		Utils.RegisterFunc(L, -3, "IsBattle", _m_IsBattle);
		Utils.RegisterFunc(L, -3, "SetIsBattle", _m_SetIsBattle);
		Utils.RegisterFunc(L, -3, "Create", _m_Create);
		Utils.RegisterFunc(L, -3, "DelayDestroy", _m_DelayDestroy);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "LogDebug", _m_LogDebug);
		Utils.RegisterFunc(L, -3, "UpdateSelfMarch", _m_UpdateSelfMarch);
		Utils.RegisterFunc(L, -3, "GetMarchInfo", _m_GetMarchInfo);
		Utils.RegisterFunc(L, -3, "WorldTroopObjectIsCreate", _m_WorldTroopObjectIsCreate);
		Utils.RegisterFunc(L, -3, "TryShowWorldBossTipByText", _m_TryShowWorldBossTipByText);
		Utils.RegisterFunc(L, -3, "InitLod", _m_InitLod);
		Utils.RegisterFunc(L, -3, "UnLoadFd", _m_UnLoadFd);
		Utils.RegisterFunc(L, -3, "MoveFd", _m_MoveFd);
		Utils.RegisterFunc(L, -3, "CreateFdBirthEffect", _m_CreateFdBirthEffect);
		Utils.RegisterFunc(L, -3, "CreateFdOutPutEffect", _m_CreateFdOutPutEffect);
		Utils.RegisterFunc(L, -3, "RefreshState", _m_RefreshState);
		Utils.RegisterFunc(L, -3, "IsBossTroop", _m_IsBossTroop);
		Utils.RegisterFunc(L, -3, "IsMonsterTroop", _m_IsMonsterTroop);
		Utils.RegisterFunc(L, -3, "GetMonsterSpecialType", _m_GetMonsterSpecialType);
		Utils.RegisterFunc(L, -3, "IsCityStrongholdMonsterTroop", _m_IsCityStrongholdMonsterTroop);
		Utils.RegisterFunc(L, -3, "IsPlayerTroop", _m_IsPlayerTroop);
		Utils.RegisterFunc(L, -3, "IsWorldBossTroop", _m_IsWorldBossTroop);
		Utils.RegisterFunc(L, -3, "IsAttackWorldBoss", _m_IsAttackWorldBoss);
		Utils.RegisterFunc(L, -3, "IsAttackAisilla", _m_IsAttackAisilla);
		Utils.RegisterFunc(L, -3, "IsAttackRadarPlayer", _m_IsAttackRadarPlayer);
		Utils.RegisterFunc(L, -3, "IsMarchTargetAttack", _m_IsMarchTargetAttack);
		Utils.RegisterFunc(L, -3, "CanAttack", _m_CanAttack);
		Utils.RegisterFunc(L, -3, "IsMarchTargetChangable", _m_IsMarchTargetChangable);
		Utils.RegisterFunc(L, -3, "IsFakeAttackMonsterMarch", _m_IsFakeAttackMonsterMarch);
		Utils.RegisterFunc(L, -3, "IsMummyMarch", _m_IsMummyMarch);
		Utils.RegisterFunc(L, -3, "UpdateFakeAttackMonsterMarch", _m_UpdateFakeAttackMonsterMarch);
		Utils.RegisterFunc(L, -3, "CreatePathSegment", _m_CreatePathSegment);
		Utils.RegisterFunc(L, -3, "Refresh", _m_Refresh);
		Utils.RegisterFunc(L, -3, "RefreshTrain", _m_RefreshTrain);
		Utils.RegisterFunc(L, -3, "RefreshFlowerTrain", _m_RefreshFlowerTrain);
		Utils.RegisterFunc(L, -3, "GetTransform", _m_GetTransform);
		Utils.RegisterFunc(L, -3, "GetCameraFollowTransform", _m_GetCameraFollowTransform);
		Utils.RegisterFunc(L, -3, "GetCameraFollowOffset", _m_GetCameraFollowOffset);
		Utils.RegisterFunc(L, -3, "GetModel", _m_GetModel);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "InitBossBornStage", _m_InitBossBornStage);
		Utils.RegisterFunc(L, -3, "PlayBornAni", _m_PlayBornAni);
		Utils.RegisterFunc(L, -3, "MarkInvasionMonsterDialogFlag", _m_MarkInvasionMonsterDialogFlag);
		Utils.RegisterFunc(L, -3, "AdjustIcon", _m_AdjustIcon);
		Utils.RegisterFunc(L, -3, "UpdatePerformance", _m_UpdatePerformance);
		Utils.RegisterFunc(L, -3, "Attack", _m_Attack);
		Utils.RegisterFunc(L, -3, "TryPlayS4IdleAnim", _m_TryPlayS4IdleAnim);
		Utils.RegisterFunc(L, -3, "ShowBattleHurt", _m_ShowBattleHurt);
		Utils.RegisterFunc(L, -3, "ClearEffect", _m_ClearEffect);
		Utils.RegisterFunc(L, -3, "ShowBattleSuccess", _m_ShowBattleSuccess);
		Utils.RegisterFunc(L, -3, "ShowBattleFailed", _m_ShowBattleFailed);
		Utils.RegisterFunc(L, -3, "ShowBattleDefeat", _m_ShowBattleDefeat);
		Utils.RegisterFunc(L, -3, "ShowScoutSuccess", _m_ShowScoutSuccess);
		Utils.RegisterFunc(L, -3, "IsAttackTargetInRange", _m_IsAttackTargetInRange);
		Utils.RegisterFunc(L, -3, "GetPosition", _m_GetPosition);
		Utils.RegisterFunc(L, -3, "GetTargetTroopPosition", _m_GetTargetTroopPosition);
		Utils.RegisterFunc(L, -3, "GetStationRotation", _m_GetStationRotation);
		Utils.RegisterFunc(L, -3, "GetMarchUUID", _m_GetMarchUUID);
		Utils.RegisterFunc(L, -3, "GetMarchTargetType", _m_GetMarchTargetType);
		Utils.RegisterFunc(L, -3, "GetMarchTargetPos", _m_GetMarchTargetPos);
		Utils.RegisterFunc(L, -3, "GetMarchTargetServer", _m_GetMarchTargetServer);
		Utils.RegisterFunc(L, -3, "NeedGetRealTargetPos", _m_NeedGetRealTargetPos);
		Utils.RegisterFunc(L, -3, "GetRealMarchTargetPos", _m_GetRealMarchTargetPos);
		Utils.RegisterFunc(L, -3, "GetMarchStatus", _m_GetMarchStatus);
		Utils.RegisterFunc(L, -3, "GetMarchStartTime", _m_GetMarchStartTime);
		Utils.RegisterFunc(L, -3, "GetMarchBlackStartTime", _m_GetMarchBlackStartTime);
		Utils.RegisterFunc(L, -3, "GetMarchBlackEndTime", _m_GetMarchBlackEndTime);
		Utils.RegisterFunc(L, -3, "GetMovePath", _m_GetMovePath);
		Utils.RegisterFunc(L, -3, "GetMovePathCount", _m_GetMovePathCount);
		Utils.RegisterFunc(L, -3, "GetSpeed", _m_GetSpeed);
		Utils.RegisterFunc(L, -3, "GetBlackSpeed", _m_GetBlackSpeed);
		Utils.RegisterFunc(L, -3, "SetPosition", _m_SetPosition);
		Utils.RegisterFunc(L, -3, "SetLocalPosition", _m_SetLocalPosition);
		Utils.RegisterFunc(L, -3, "GetRotation", _m_GetRotation);
		Utils.RegisterFunc(L, -3, "SetRotation", _m_SetRotation);
		Utils.RegisterFunc(L, -3, "CreateTroopDestination", _m_CreateTroopDestination);
		Utils.RegisterFunc(L, -3, "DestroyTroopDestination", _m_DestroyTroopDestination);
		Utils.RegisterFunc(L, -3, "UpdateTroopDestination", _m_UpdateTroopDestination);
		Utils.RegisterFunc(L, -3, "SetStateSpeed", _m_SetStateSpeed);
		Utils.RegisterFunc(L, -3, "PlayAnim", _m_PlayAnim);
		Utils.RegisterFunc(L, -3, "PlayQueued", _m_PlayQueued);
		Utils.RegisterFunc(L, -3, "PlayIdleQueued", _m_PlayIdleQueued);
		Utils.RegisterFunc(L, -3, "PlayLoopAttackAnim", _m_PlayLoopAttackAnim);
		Utils.RegisterFunc(L, -3, "GetDefenderMarchStatus", _m_GetDefenderMarchStatus);
		Utils.RegisterFunc(L, -3, "GetTargetTroop", _m_GetTargetTroop);
		Utils.RegisterFunc(L, -3, "GetTargetPointInfo", _m_GetTargetPointInfo);
		Utils.RegisterFunc(L, -3, "TryPlayBornAnim", _m_TryPlayBornAnim);
		Utils.RegisterFunc(L, -3, "TriggerDefenderToAfraidLight", _m_TriggerDefenderToAfraidLight);
		Utils.RegisterFunc(L, -3, "TriggerDefenderToDefend", _m_TriggerDefenderToDefend);
		Utils.RegisterFunc(L, -3, "TriggerDefenderToBeWhistled", _m_TriggerDefenderToBeWhistled);
		Utils.RegisterFunc(L, -3, "PlaySandWormAnim", _m_PlaySandWormAnim);
		Utils.RegisterFunc(L, -3, "PlayDefendAnim", _m_PlayDefendAnim);
		Utils.RegisterFunc(L, -3, "CleanAllQueuedStates", _m_CleanAllQueuedStates);
		Utils.RegisterFunc(L, -3, "TryPlay", _m_TryPlay);
		Utils.RegisterFunc(L, -3, "GetDefenderPositionList", _m_GetDefenderPositionList);
		Utils.RegisterFunc(L, -3, "GetDefenderPosition", _m_GetDefenderPosition);
		Utils.RegisterFunc(L, -3, "TroopUnitPickSuccess", _m_TroopUnitPickSuccess);
		Utils.RegisterFunc(L, -3, "TroopUnitPickBack", _m_TroopUnitPickBack);
		Utils.RegisterFunc(L, -3, "BackTroopUnits", _m_BackTroopUnits);
		Utils.RegisterFunc(L, -3, "ClearTroopUnits", _m_ClearTroopUnits);
		Utils.RegisterFunc(L, -3, "TroopUnitsBirthThenPickGarbage", _m_TroopUnitsBirthThenPickGarbage);
		Utils.RegisterFunc(L, -3, "GetPickPoint", _m_GetPickPoint);
		Utils.RegisterFunc(L, -3, "IsPickGarbageTroop", _m_IsPickGarbageTroop);
		Utils.RegisterFunc(L, -3, "IsGolloesExplore", _m_IsGolloesExplore);
		Utils.RegisterFunc(L, -3, "IsExplore", _m_IsExplore);
		Utils.RegisterFunc(L, -3, "IsScoutTroop", _m_IsScoutTroop);
		Utils.RegisterFunc(L, -3, "IsDetectRescue", _m_IsDetectRescue);
		Utils.RegisterFunc(L, -3, "ReSetEntityTarget", _m_ReSetEntityTarget);
		Utils.RegisterFunc(L, -3, "PlayAttackEffect", _m_PlayAttackEffect);
		Utils.RegisterFunc(L, -3, "LookAtTarget", _m_LookAtTarget);
		Utils.RegisterFunc(L, -3, "SetRotationRoot", _m_SetRotationRoot);
		Utils.RegisterFunc(L, -3, "TroopUnitsBirthThenAttack", _m_TroopUnitsBirthThenAttack);
		Utils.RegisterFunc(L, -3, "AddShield", _m_AddShield);
		Utils.RegisterFunc(L, -3, "DelShield", _m_DelShield);
		Utils.RegisterFunc(L, -3, "ShowRallyMarchAttack", _m_ShowRallyMarchAttack);
		Utils.RegisterFunc(L, -3, "ShowAttack", _m_ShowAttack);
		Utils.RegisterFunc(L, -3, "RemoveAttack", _m_RemoveAttack);
		Utils.RegisterFunc(L, -3, "DoSkill", _m_DoSkill);
		Utils.RegisterFunc(L, -3, "TroopUnitsAttack", _m_TroopUnitsAttack);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -3, "GetBoundingSphere", _m_GetBoundingSphere);
		Utils.RegisterFunc(L, -3, "OnCullingStateVisible", _m_OnCullingStateVisible);
		Utils.RegisterFunc(L, -3, "GetHeight", _m_GetHeight);
		Utils.RegisterFunc(L, -3, "ChangeFsmState", _m_ChangeFsmState);
		Utils.RegisterFunc(L, -3, "PlayHitEffect", _m_PlayHitEffect);
		Utils.RegisterFunc(L, -3, "ClearHitEffect", _m_ClearHitEffect);
		Utils.RegisterFunc(L, -3, "HideJunkMan", _m_HideJunkMan);
		Utils.RegisterFunc(L, -3, "ShowJunkMan", _m_ShowJunkMan);
		Utils.RegisterFunc(L, -3, "SetVisible", _m_SetVisible);
		Utils.RegisterFunc(L, -3, "AttackOnce", _m_AttackOnce);
		Utils.RegisterFunc(L, -3, "AttackOnceWithIndex", _m_AttackOnceWithIndex);
		Utils.RegisterFunc(L, -3, "OnMoveStateEnd", _m_OnMoveStateEnd);
		Utils.RegisterFunc(L, -3, "SetLookAt", _m_SetLookAt);
		Utils.RegisterFunc(L, -3, "RefreshPosition", _m_RefreshPosition);
		Utils.RegisterFunc(L, -3, "OnMonsterIceBroken", _m_OnMonsterIceBroken);
		Utils.RegisterFunc(L, -3, "IsS1SeasonPreBoss", _m_IsS1SeasonPreBoss);
		Utils.RegisterFunc(L, -3, "IsWeakS1SeasonPreBoss", _m_IsWeakS1SeasonPreBoss);
		Utils.RegisterFunc(L, -3, "TryGetAnimStateTimeLength", _m_TryGetAnimStateTimeLength);
		Utils.RegisterFunc(L, -3, "IsCanPlayAni", _m_IsCanPlayAni);
		Utils.RegisterFunc(L, -3, "UpdateWhenLodChange", _m_UpdateWhenLodChange);
		Utils.RegisterFunc(L, -3, "UpdateSoundByLod", _m_UpdateSoundByLod);
		Utils.RegisterFunc(L, -3, "UpdateSound", _m_UpdateSound);
		Utils.RegisterFunc(L, -3, "SetStunVFX", _m_SetStunVFX);
		Utils.RegisterFunc(L, -3, "ShowZombieRushDefendSuccess", _m_ShowZombieRushDefendSuccess);
		Utils.RegisterFunc(L, -3, "ShowZombieRushDefendFailed", _m_ShowZombieRushDefendFailed);
		Utils.RegisterFunc(L, -3, "ShowBloodyQueenDefendFailed", _m_ShowBloodyQueenDefendFailed);
		Utils.RegisterFunc(L, -3, "ShowZombieDead", _m_ShowZombieDead);
		Utils.RegisterFunc(L, -3, "OnPushBloodQueenGunnerAttack", _m_OnPushBloodQueenGunnerAttack);
		Utils.RegisterFunc(L, -2, "Uid", _g_get_Uid);
		Utils.RegisterFunc(L, -2, "CanShowLittleSmartMode", _g_get_CanShowLittleSmartMode);
		Utils.RegisterFunc(L, -2, "S4MonsterEffList", _g_get_S4MonsterEffList);
		Utils.RegisterFunc(L, -2, "isDestroy", _g_get_isDestroy);
		Utils.RegisterFunc(L, -2, "IsInstanced", _g_get_IsInstanced);
		Utils.RegisterFunc(L, -2, "IsInvalid", _g_get_IsInvalid);
		Utils.RegisterFunc(L, -2, "IsDelayDestroyed", _g_get_IsDelayDestroyed);
		Utils.RegisterFunc(L, -2, "IsDelayDestroy", _g_get_IsDelayDestroy);
		Utils.RegisterFunc(L, -2, "DelayApply", _g_get_DelayApply);
		Utils.RegisterFunc(L, -2, "DelayApplyTime", _g_get_DelayApplyTime);
		Utils.RegisterFunc(L, -2, "idlePlayAttack", _g_get_idlePlayAttack);
		Utils.RegisterFunc(L, -2, "oldMarchState", _g_get_oldMarchState);
		Utils.RegisterFunc(L, -2, "RelaseSkillTick", _g_get_RelaseSkillTick);
		Utils.RegisterFunc(L, -2, "CullingBoundsIndex", _g_get_CullingBoundsIndex);
		Utils.RegisterFunc(L, -2, "delStartTick", _g_get_delStartTick);
		Utils.RegisterFunc(L, -2, "monsterAttackRange", _g_get_monsterAttackRange);
		Utils.RegisterFunc(L, -2, "happyTroopCircile", _g_get_happyTroopCircile);
		Utils.RegisterFunc(L, -2, "defAtkUuid", _g_get_defAtkUuid);
		Utils.RegisterFunc(L, -1, "DelayApply", _s_set_DelayApply);
		Utils.RegisterFunc(L, -1, "DelayApplyTime", _s_set_DelayApplyTime);
		Utils.RegisterFunc(L, -1, "oldMarchState", _s_set_oldMarchState);
		Utils.RegisterFunc(L, -1, "RelaseSkillTick", _s_set_RelaseSkillTick);
		Utils.RegisterFunc(L, -1, "CullingBoundsIndex", _s_set_CullingBoundsIndex);
		Utils.RegisterFunc(L, -1, "delStartTick", _s_set_delStartTick);
		Utils.RegisterFunc(L, -1, "monsterAttackRange", _s_set_monsterAttackRange);
		Utils.RegisterFunc(L, -1, "happyTroopCircile", _s_set_happyTroopCircile);
		Utils.RegisterFunc(L, -1, "defAtkUuid", _s_set_defAtkUuid);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 60, 0, 0);
		Utils.RegisterFunc(L, -4, "CalcMoveOnPath", _m_CalcMoveOnPath_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "skillWordPath", "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab");
		Utils.RegisterObject(L, translator, -4, "normalWordPath", "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab");
		Utils.RegisterObject(L, translator, -4, "cureWordPath", "Assets/Main/Prefabs/UI/BattleWord/BattleCureBloodTip.prefab");
		Utils.RegisterObject(L, translator, -4, "battleVictoryPath", "Assets/Main/Prefabs/Effect/World/VFX_VictoryNoUI.prefab");
		Utils.RegisterObject(L, translator, -4, "battleFailurePath", "Assets/Main/Prefabs/Effect/World/VFX_FailureNoUI.prefab");
		Utils.RegisterObject(L, translator, -4, "battleDefeatPath", "Assets/Main/Prefabs/Effect/World/VFX_DefeatNoUI.prefab");
		Utils.RegisterObject(L, translator, -4, "battleBoomPath", "Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_duboom_002.prefab");
		Utils.RegisterObject(L, translator, -4, "clearEffectPath", "Assets/Main/Prefabs/World/Saiji/Eff_saiji_dsj_dikuai_fangzhi.prefab");
		Utils.RegisterObject(L, translator, -4, "virusPoisonedBubblePath", "Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_kuosan_du_01.prefab");
		Utils.RegisterObject(L, translator, -4, "strongholdMonsterBornPath", "Assets/Main/Prefabs/World/Saiji/Eff_Common_chusheng_01.prefab");
		Utils.RegisterObject(L, translator, -4, "bossBoomPath", "Assets/Main/Prefabs/World/Saiji/Eff_shiti_boom.prefab");
		Utils.RegisterObject(L, translator, -4, "scoutSuccessPath", "Assets/Main/Prefabs/Effect/World/VFX_ScoutNoUI.prefab");
		Utils.RegisterObject(L, translator, -4, "BigSandwormBornVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormBornVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "HugeSandwormBornVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormBornVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "BigSandwormDeadVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormDeadVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "HugeSandwormDeadVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormDeadVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "BigSandwormEscapeVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormEscapeVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "HugeSandwormEscapeVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormEscapeVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "BigSandwormAttackVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormAttackVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "HugeSandwormAttackVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormAttackVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "BigSandwormStunVFX", "Assets/Main/Prefabs/Effect/World/Sandworm/BigSandwormStunVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "HugeSandwormStunVFX", "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormStunVFX.prefab");
		Utils.RegisterObject(L, translator, -4, "Anim_Birth", "birth");
		Utils.RegisterObject(L, translator, -4, "Anim_Idle", "idle");
		Utils.RegisterObject(L, translator, -4, "Anim_Idle1", "idle01");
		Utils.RegisterObject(L, translator, -4, "Anim_Idle2", "idle02");
		Utils.RegisterObject(L, translator, -4, "Anim_Run", "run");
		Utils.RegisterObject(L, translator, -4, "Anim_Walk", "walk");
		Utils.RegisterObject(L, translator, -4, "Anim_Attack", "attack");
		Utils.RegisterObject(L, translator, -4, "Anim_Attack_Loop", "attack_move");
		Utils.RegisterObject(L, translator, -4, "Anim_Hit", "hit");
		Utils.RegisterObject(L, translator, -4, "Anim_Death", "death");
		Utils.RegisterObject(L, translator, -4, "Anim_Dead", "dead");
		Utils.RegisterObject(L, translator, -4, "Anim_Stop", "stop");
		Utils.RegisterObject(L, translator, -4, "Anim_Back", "back");
		Utils.RegisterObject(L, translator, -4, "Anim_Pick_Garbage", "xiaoren_work");
		Utils.RegisterObject(L, translator, -4, "Anim_Pick_Garbage_Run", "xiaoren_run");
		Utils.RegisterObject(L, translator, -4, "Anim_Pick_Garbage_Success", "xiaoren_show");
		Utils.RegisterObject(L, translator, -4, "Anim_Detect_Rescue_Start", "huangseyunshuji_01_show");
		Utils.RegisterObject(L, translator, -4, "Anim_Stun", "stun");
		Utils.RegisterObject(L, translator, -4, "Anim_Born", "born");
		Utils.RegisterObject(L, translator, -4, "Anim_Escape", "escape");
		Utils.RegisterObject(L, translator, -4, "Anim_Eachother", "fight_eachother");
		Utils.RegisterObject(L, translator, -4, "Anim_Beat", "beat");
		Utils.RegisterObject(L, translator, -4, "Anim_Get_Beat", "get_beat");
		Utils.RegisterObject(L, translator, -4, "Anim_Weak", "weak");
		Utils.RegisterObject(L, translator, -4, "HpBarPath", "HPBar");
		Utils.RegisterObject(L, translator, -4, "HpBarSliderPath", "HPBar/Slider");
		Utils.RegisterObject(L, translator, -4, "HpBarTextPath", "HPBar/Text");
		Utils.RegisterObject(L, translator, -4, "RallyTipPath", "HPBar/RallyTip");
		Utils.RegisterObject(L, translator, -4, "BerserkBossTombRootPath", "Model/Tomb");
		Utils.RegisterObject(L, translator, -4, "BerserkBossNormalRootPath", "Model/Normal");
		Utils.RegisterObject(L, translator, -4, "ModelTextContentPath", "ModelLabel");
		Utils.RegisterObject(L, translator, -4, "BerserkBossRewardPath", "rewardBubble");
		Utils.RegisterObject(L, translator, -4, "AttackRange", 6f);
		Utils.RegisterObject(L, translator, -4, "range", 4f);
		Utils.RegisterObject(L, translator, -4, "DummyAttackRange", 3.3f);
		Utils.RegisterObject(L, translator, -4, "DummyAttackCityRange", 8f);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldScene>(L, 2))
			{
				WorldTroop o = new WorldTroop((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLod(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			obj.UpdateLod(lod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBattle(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBattle();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsBattle(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isBattle = Lua.lua_toboolean(L, 2);
			obj.SetIsBattle(isBattle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			WorldTroopManager manager = (WorldTroopManager)objectTranslator.GetObject(L, 3, typeof(WorldTroopManager));
			worldTroop.Create(march, manager);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DelayDestroy(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float delaySec = (float)Lua.lua_tonumber(L, 2);
			obj.DelayDestroy(delaySec);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long marchuuid = Lua.lua_toint64(L, 2);
				worldTroop.Destroy(marchuuid);
				return 0;
			}
			if (num == 1)
			{
				worldTroop.Destroy(0L);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.Destroy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogDebug(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string msg = Lua.lua_tostring(L, 2);
			obj.LogDebug(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSelfMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			object o = objectTranslator.GetObject(L, 2, typeof(object));
			worldTroop.UpdateSelfMarch(o);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarch marchInfo = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetMarchInfo();
			objectTranslator.Push(L, marchInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldTroopObjectIsCreate(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WorldTroopObjectIsCreate();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryShowWorldBossTipByText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				string text = Lua.lua_tostring(L, 2);
				float showTime = (float)Lua.lua_tonumber(L, 3);
				Action callback = objectTranslator.GetDelegate<Action>(L, 4);
				worldTroop.TryShowWorldBossTipByText(text, showTime, callback);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string text2 = Lua.lua_tostring(L, 2);
				float showTime2 = (float)Lua.lua_tonumber(L, 3);
				worldTroop.TryShowWorldBossTipByText(text2, showTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.TryShowWorldBossTipByText!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitLod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			worldTroop.InitLod(gameObject);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnLoadFd(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnLoadFd();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveFd(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MoveFd();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateFdBirthEffect(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateFdBirthEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateFdOutPutEffect(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateFdOutPutEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshState(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshState();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBossTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBossTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMonsterTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMonsterTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMonsterSpecialType(IntPtr L)
	{
		try
		{
			int monsterSpecialType = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMonsterSpecialType();
			Lua.xlua_pushinteger(L, monsterSpecialType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCityStrongholdMonsterTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCityStrongholdMonsterTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPlayerTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPlayerTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWorldBossTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWorldBossTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAttackWorldBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAttackWorldBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAttackAisilla(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAttackAisilla();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAttackRadarPlayer(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAttackRadarPlayer();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMarchTargetAttack(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMarchTargetAttack();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanAttack(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float curPathLen = (float)Lua.lua_tonumber(L, 2);
			bool value = obj.CanAttack(curPathLen);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMarchTargetChangable(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMarchTargetChangable();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFakeAttackMonsterMarch(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFakeAttackMonsterMarch();
			Lua.lua_pushboolean(L, value);
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
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMummyMarch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFakeAttackMonsterMarch(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateFakeAttackMonsterMarch();
			return 0;
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
			WorldTroopPathSegment[] o = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).CreatePathSegment();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcMoveOnPath_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopPathSegment[] path = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 1, typeof(WorldTroopPathSegment[]));
			int startIndex = Lua.xlua_tointeger(L, 2);
			float startPathLen = (float)Lua.lua_tonumber(L, 3);
			WorldTroop.CalcMoveOnPath(path, startIndex, startPathLen, out var pathIdx, out var pathLen, out var pos);
			Lua.xlua_pushinteger(L, pathIdx);
			Lua.lua_pushnumber(L, pathLen);
			objectTranslator.PushUnityEngineVector3(L, pos);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Refresh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroop.Refresh(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshTrain(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshTrain();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshFlowerTrain(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshFlowerTrain();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetTransform();
			objectTranslator.Push(L, transform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraFollowTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform cameraFollowTransform = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetCameraFollowTransform();
			objectTranslator.Push(L, cameraFollowTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraFollowOffset(IntPtr L)
	{
		try
		{
			float cameraFollowOffset = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCameraFollowOffset();
			Lua.lua_pushnumber(L, cameraFollowOffset);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject model = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetModel();
			objectTranslator.Push(L, model);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBossBornStage(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			uint type = Lua.xlua_touint(L, 2);
			obj.InitBossBornStage(type);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBornAni(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayBornAni();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MarkInvasionMonsterDialogFlag(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			uint flag = Lua.xlua_touint(L, 2);
			obj.MarkInvasionMonsterDialogFlag(flag);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AdjustIcon(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AdjustIcon();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdatePerformance(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdatePerformance();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Attack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Attack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryPlayS4IdleAnim(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			obj.TryPlayS4IdleAnim(animName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBattleHurt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			int hurt = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out WorldMarchDataManager.BattleWordType val);
			worldTroop.ShowBattleHurt(hurt, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearEffect(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBattleSuccess(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowBattleSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBattleFailed(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowBattleFailed();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBattleDefeat(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowBattleDefeat();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowScoutSuccess(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowScoutSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAttackTargetInRange(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAttackTargetInRange();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 position = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetPosition();
			objectTranslator.PushUnityEngineVector3(L, position);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTargetTroopPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 targetTroopPosition = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetTargetTroopPosition();
			objectTranslator.PushUnityEngineVector3(L, targetTroopPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStationRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Quaternion stationRotation = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetStationRotation();
			objectTranslator.PushUnityEngineQuaternion(L, stationRotation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchUUID(IntPtr L)
	{
		try
		{
			long marchUUID = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchUUID();
			Lua.lua_pushint64(L, marchUUID);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MarchTargetType marchTargetType = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetMarchTargetType();
			objectTranslator.Push(L, marchTargetType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchTargetPos(IntPtr L)
	{
		try
		{
			int marchTargetPos = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchTargetPos();
			Lua.xlua_pushinteger(L, marchTargetPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchTargetServer(IntPtr L)
	{
		try
		{
			int marchTargetServer = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchTargetServer();
			Lua.xlua_pushinteger(L, marchTargetServer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NeedGetRealTargetPos(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NeedGetRealTargetPos();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRealMarchTargetPos(IntPtr L)
	{
		try
		{
			int realMarchTargetPos = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRealMarchTargetPos();
			Lua.xlua_pushinteger(L, realMarchTargetPos);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MarchStatus marchStatus = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetMarchStatus();
			objectTranslator.PushMarchStatus(L, marchStatus);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchStartTime(IntPtr L)
	{
		try
		{
			long marchStartTime = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchStartTime();
			Lua.lua_pushint64(L, marchStartTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchBlackStartTime(IntPtr L)
	{
		try
		{
			long marchBlackStartTime = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchBlackStartTime();
			Lua.lua_pushint64(L, marchBlackStartTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchBlackEndTime(IntPtr L)
	{
		try
		{
			long marchBlackEndTime = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMarchBlackEndTime();
			Lua.lua_pushint64(L, marchBlackEndTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMovePath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] movePath = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetMovePath();
			objectTranslator.Push(L, movePath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMovePathCount(IntPtr L)
	{
		try
		{
			int movePathCount = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMovePathCount();
			Lua.xlua_pushinteger(L, movePathCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpeed(IntPtr L)
	{
		try
		{
			float speed = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSpeed();
			Lua.lua_pushnumber(L, speed);
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
			float blackSpeed = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetBlackSpeed();
			Lua.lua_pushnumber(L, blackSpeed);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldTroop.SetPosition(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldTroop.SetLocalPosition(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Quaternion rotation = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetRotation();
			objectTranslator.PushUnityEngineQuaternion(L, rotation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Quaternion>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Quaternion val);
				bool includeAnims = Lua.lua_toboolean(L, 3);
				worldTroop.SetRotation(val, includeAnims);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Quaternion>(L, 2))
			{
				objectTranslator.Get(L, 2, out Quaternion val2);
				worldTroop.SetRotation(val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.SetRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTroopDestination(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateTroopDestination();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyTroopDestination(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyTroopDestination();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroopDestination(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int targetPos = Lua.xlua_tointeger(L, 2);
			obj.UpdateTroopDestination(targetPos);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStateSpeed(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			float speed = (float)Lua.lua_tonumber(L, 3);
			obj.SetStateSpeed(animName, speed);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnim(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName = Lua.lua_tostring(L, 2);
				worldTroop.PlayAnim(animName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				bool rewind = Lua.lua_toboolean(L, 3);
				worldTroop.PlayAnim(animName2, rewind);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.PlayAnim!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayQueued(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<QueueMode>(L, 3))
			{
				string animName = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out QueueMode v);
				worldTroop.PlayQueued(animName, v);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				worldTroop.PlayQueued(animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.PlayQueued!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayIdleQueued(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayIdleQueued();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayLoopAttackAnim(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayLoopAttackAnim();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDefenderMarchStatus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MarchStatus defenderMarchStatus = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetDefenderMarchStatus();
			objectTranslator.PushMarchStatus(L, defenderMarchStatus);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTargetTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop targetTroop = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetTargetTroop();
			objectTranslator.Push(L, targetTroop);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTargetPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject targetPointInfo = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetTargetPointInfo();
			objectTranslator.Push(L, targetPointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryPlayBornAnim(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TryPlayBornAnim();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TriggerDefenderToAfraidLight(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TriggerDefenderToAfraidLight();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TriggerDefenderToDefend(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			float attackDuration = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			worldTroop.TriggerDefenderToDefend(attackDuration, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TriggerDefenderToBeWhistled(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float whistleDuration = (float)Lua.lua_tonumber(L, 2);
			obj.TriggerDefenderToBeWhistled(whistleDuration);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySandWormAnim(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			worldTroop.PlaySandWormAnim(animName, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayDefendAnim(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			float attackDuration = (float)Lua.lua_tonumber(L, 4);
			float num = worldTroop.PlayDefendAnim(animName, val, attackDuration);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CleanAllQueuedStates(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CleanAllQueuedStates();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryPlay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			worldTroop.TryPlay(animName, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDefenderPositionList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<Vector3> defenderPositionList = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetDefenderPositionList();
			objectTranslator.Push(L, defenderPositionList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDefenderPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 defenderPosition = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetDefenderPosition();
			objectTranslator.PushUnityEngineVector3(L, defenderPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitPickSuccess(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TroopUnitPickSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitPickBack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TroopUnitPickBack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BackTroopUnits(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).BackTroopUnits();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearTroopUnits(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearTroopUnits();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitsBirthThenPickGarbage(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool appear = Lua.lua_toboolean(L, 2);
				worldTroop.TroopUnitsBirthThenPickGarbage(appear);
				return 0;
			}
			if (num == 1)
			{
				worldTroop.TroopUnitsBirthThenPickGarbage();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.TroopUnitsBirthThenPickGarbage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPickPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			int endP = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			List<Vector3> pickPoint = worldTroop.GetPickPoint(endP, val);
			objectTranslator.Push(L, pickPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPickGarbageTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPickGarbageTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGolloesExplore(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsGolloesExplore();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsExplore(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsExplore();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsScoutTroop(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsScoutTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDetectRescue(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDetectRescue();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReSetEntityTarget(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReSetEntityTarget();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAttackEffect(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayAttackEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LookAtTarget(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LookAtTarget();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRotationRoot(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetRotationRoot();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitsBirthThenAttack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TroopUnitsBirthThenAttack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddShield(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AddShield();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DelShield(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DelShield();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowRallyMarchAttack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowRallyMarchAttack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowAttack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowAttack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAttack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveAttack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoSkill(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			int useSkillID = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out DamageType v);
			int heroId = Lua.xlua_tointeger(L, 4);
			Dictionary<long, List<string>> useSkillList = (Dictionary<long, List<string>>)objectTranslator.GetObject(L, 5, typeof(Dictionary<long, List<string>>));
			long useSkillUid = Lua.lua_toint64(L, 6);
			worldTroop.DoSkill(useSkillID, v, heroId, useSkillList, useSkillUid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitsAttack(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool isBirth = Lua.lua_toboolean(L, 2);
				worldTroop.TroopUnitsAttack(isBirth);
				return 0;
			}
			if (num == 1)
			{
				worldTroop.TroopUnitsAttack();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroop.TroopUnitsAttack!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrawGizmos(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoundingSphere(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BoundingSphere boundingSphere = ((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).GetBoundingSphere();
			objectTranslator.Push(L, boundingSphere);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCullingStateVisible(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool visible = Lua.lua_toboolean(L, 2);
			obj.OnCullingStateVisible(visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHeight(IntPtr L)
	{
		try
		{
			float height = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHeight();
			Lua.lua_pushnumber(L, height);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeFsmState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldTroopState v);
			worldTroop.ChangeFsmState(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayHitEffect(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string effectPath = Lua.lua_tostring(L, 2);
			obj.PlayHitEffect(effectPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearHitEffect(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearHitEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideJunkMan(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideJunkMan();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowJunkMan(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowJunkMan();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisible(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool visible = Lua.lua_toboolean(L, 2);
			obj.SetVisible(visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AttackOnce(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldTroop.AttackOnce(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AttackOnceWithIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			int index = Lua.xlua_tointeger(L, 3);
			worldTroop.AttackOnceWithIndex(val, index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMoveStateEnd(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnMoveStateEnd();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLookAt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool ignoreState = Lua.lua_toboolean(L, 3);
			worldTroop.SetLookAt(val, ignoreState);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshPosition(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshPosition();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMonsterIceBroken(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnMonsterIceBroken();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsS1SeasonPreBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsS1SeasonPreBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWeakS1SeasonPreBoss(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWeakS1SeasonPreBoss();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetAnimStateTimeLength(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			float num = obj.TryGetAnimStateTimeLength(animName);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCanPlayAni(IntPtr L)
	{
		try
		{
			bool value = ((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCanPlayAni();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateWhenLodChange(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int curLod = Lua.xlua_tointeger(L, 2);
			obj.UpdateWhenLodChange(curLod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSoundByLod(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int curLod = Lua.xlua_tointeger(L, 2);
			obj.UpdateSoundByLod(curLod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSound(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateSound();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStunVFX(IntPtr L)
	{
		try
		{
			WorldTroop obj = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool stunVFX = Lua.lua_toboolean(L, 2);
			obj.SetStunVFX(stunVFX);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowZombieRushDefendSuccess(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowZombieRushDefendSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowZombieRushDefendFailed(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowZombieRushDefendFailed();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBloodyQueenDefendFailed(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowBloodyQueenDefendFailed();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowZombieDead(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowZombieDead();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPushBloodQueenGunnerAttack(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnPushBloodQueenGunnerAttack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Uid(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldTroop.Uid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CanShowLittleSmartMode(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.CanShowLittleSmartMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_S4MonsterEffList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTroop.S4MonsterEffList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDestroy(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.isDestroy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInstanced(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.IsInstanced);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInvalid(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.IsInvalid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsDelayDestroyed(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.IsDelayDestroyed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsDelayDestroy(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.IsDelayDestroy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DelayApply(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.DelayApply);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DelayApplyTime(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTroop.DelayApplyTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_idlePlayAttack(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroop.idlePlayAttack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_oldMarchState(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroop.oldMarchState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RelaseSkillTick(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTroop.RelaseSkillTick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CullingBoundsIndex(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroop.CullingBoundsIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_delStartTick(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTroop.delStartTick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterAttackRange(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTroop.monsterAttackRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_happyTroopCircile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroop worldTroop = (WorldTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTroop.happyTroopCircile);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defAtkUuid(IntPtr L)
	{
		try
		{
			WorldTroop worldTroop = (WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldTroop.defAtkUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DelayApply(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DelayApply = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DelayApplyTime(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DelayApplyTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_oldMarchState(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).oldMarchState = (byte)Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RelaseSkillTick(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RelaseSkillTick = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CullingBoundsIndex(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CullingBoundsIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_delStartTick(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).delStartTick = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterAttackRange(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).monsterAttackRange = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_happyTroopCircile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldTroop)objectTranslator.FastGetCSObj(L, 1)).happyTroopCircile = (WorldIconRendererFacade.HappyIcon)objectTranslator.GetObject(L, 2, typeof(WorldIconRendererFacade.HappyIcon));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defAtkUuid(IntPtr L)
	{
		try
		{
			((WorldTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).defAtkUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
