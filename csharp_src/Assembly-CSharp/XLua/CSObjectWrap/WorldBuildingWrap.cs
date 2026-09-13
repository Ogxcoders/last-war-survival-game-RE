using System;
using UnityEngine;
using UnityEngine.Playables;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldBuildingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldBuilding);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 66, 14, 9);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "GetPointInfo", _m_GetPointInfo);
		Utils.RegisterFunc(L, -3, "GetBuildInfo", _m_GetBuildInfo);
		Utils.RegisterFunc(L, -3, "CheckFakeAutoDestroy", _m_CheckFakeAutoDestroy);
		Utils.RegisterFunc(L, -3, "InitLod", _m_InitLod);
		Utils.RegisterFunc(L, -3, "DoFoldUpAnim", _m_DoFoldUpAnim);
		Utils.RegisterFunc(L, -3, "ClearPlayIdleAniManager", _m_ClearPlayIdleAniManager);
		Utils.RegisterFunc(L, -3, "DoExtendDome", _m_DoExtendDome);
		Utils.RegisterFunc(L, -3, "DoUpgradeDome", _m_DoUpgradeDome);
		Utils.RegisterFunc(L, -3, "refeshDate", _m_refeshDate);
		Utils.RegisterFunc(L, -3, "EnterMoveCityState", _m_EnterMoveCityState);
		Utils.RegisterFunc(L, -3, "GetTransform", _m_GetTransform);
		Utils.RegisterFunc(L, -3, "PointInPick", _m_PointInPick);
		Utils.RegisterFunc(L, -3, "Drag", _m_Drag);
		Utils.RegisterFunc(L, -3, "Select", _m_Select);
		Utils.RegisterFunc(L, -3, "CanLongTap", _m_CanLongTap);
		Utils.RegisterFunc(L, -3, "DestroyDomeInstance", _m_DestroyDomeInstance);
		Utils.RegisterFunc(L, -3, "IsOutRange", _m_IsOutRange);
		Utils.RegisterFunc(L, -3, "ChangeTouchPos", _m_ChangeTouchPos);
		Utils.RegisterFunc(L, -3, "OnBattleDefUpdate", _m_OnBattleDefUpdate);
		Utils.RegisterFunc(L, -3, "ResetParam", _m_ResetParam);
		Utils.RegisterFunc(L, -3, "ChangeMove", _m_ChangeMove);
		Utils.RegisterFunc(L, -3, "GetClosestPoint", _m_GetClosestPoint);
		Utils.RegisterFunc(L, -3, "HasAnimClip", _m_HasAnimClip);
		Utils.RegisterFunc(L, -3, "PlayAnim", _m_PlayAnim);
		Utils.RegisterFunc(L, -3, "DoBuildClickAnim", _m_DoBuildClickAnim);
		Utils.RegisterFunc(L, -3, "DoBuildPlaceAnim", _m_DoBuildPlaceAnim);
		Utils.RegisterFunc(L, -3, "GetPlayBuildingAnim", _m_GetPlayBuildingAnim);
		Utils.RegisterFunc(L, -3, "OnClick", _m_OnClick);
		Utils.RegisterFunc(L, -3, "OnBeginLongTap", _m_OnBeginLongTap);
		Utils.RegisterFunc(L, -3, "OnEndLongTap", _m_OnEndLongTap);
		Utils.RegisterFunc(L, -3, "GetHeight", _m_GetHeight);
		Utils.RegisterFunc(L, -3, "ShowDome", _m_ShowDome);
		Utils.RegisterFunc(L, -3, "HideDome", _m_HideDome);
		Utils.RegisterFunc(L, -3, "IsSelf", _m_IsSelf);
		Utils.RegisterFunc(L, -3, "UpdateCityLabel", _m_UpdateCityLabel);
		Utils.RegisterFunc(L, -3, "DoGuideStartShow", _m_DoGuideStartShow);
		Utils.RegisterFunc(L, -3, "ChangeToBox", _m_ChangeToBox);
		Utils.RegisterFunc(L, -3, "OnBattleAtkUpdate", _m_OnBattleAtkUpdate);
		Utils.RegisterFunc(L, -3, "OnBattleAtkEnd", _m_OnBattleAtkEnd);
		Utils.RegisterFunc(L, -3, "ContainsPos", _m_ContainsPos);
		Utils.RegisterFunc(L, -3, "ProfileToggleGlass", _m_ProfileToggleGlass);
		Utils.RegisterFunc(L, -3, "IsRuins", _m_IsRuins);
		Utils.RegisterFunc(L, -3, "SetMoveState", _m_SetMoveState);
		Utils.RegisterFunc(L, -3, "CheckPlayAnimation", _m_CheckPlayAnimation);
		Utils.RegisterFunc(L, -3, "PlayAnimation", _m_PlayAnimation);
		Utils.RegisterFunc(L, -3, "PlayCrossFadeAnimation", _m_PlayCrossFadeAnimation);
		Utils.RegisterFunc(L, -3, "PlayAnimationNormalizeAndCrossFade", _m_PlayAnimationNormalizeAndCrossFade);
		Utils.RegisterFunc(L, -3, "PlayAnimationEffect", _m_PlayAnimationEffect);
		Utils.RegisterFunc(L, -3, "PlayAnimationEffectAni", _m_PlayAnimationEffectAni);
		Utils.RegisterFunc(L, -3, "PlayTimeline", _m_PlayTimeline);
		Utils.RegisterFunc(L, -3, "OnEnable", _m_OnEnable);
		Utils.RegisterFunc(L, -3, "OnLodChangeFunc", _m_OnLodChangeFunc);
		Utils.RegisterFunc(L, -3, "PlayAnimationAndEffectReturnTime", _m_PlayAnimationAndEffectReturnTime);
		Utils.RegisterFunc(L, -3, "PlayCrossFadeAnim", _m_PlayCrossFadeAnim);
		Utils.RegisterFunc(L, -3, "GetAnimationLength", _m_GetAnimationLength);
		Utils.RegisterFunc(L, -3, "UpdateStatus", _m_UpdateStatus);
		Utils.RegisterFunc(L, -3, "RefreshMeteorite", _m_RefreshMeteorite);
		Utils.RegisterFunc(L, -3, "FakeEpidemicSkill", _m_FakeEpidemicSkill);
		Utils.RegisterFunc(L, -3, "RefreshEpidemicSkill", _m_RefreshEpidemicSkill);
		Utils.RegisterFunc(L, -3, "InitDynamicModel", _m_InitDynamicModel);
		Utils.RegisterFunc(L, -3, "UnloadDynamicModel", _m_UnloadDynamicModel);
		Utils.RegisterFunc(L, -3, "OnDynamicModelLoad", _m_OnDynamicModelLoad);
		Utils.RegisterFunc(L, -3, "GetCurIdleAniTIme", _m_GetCurIdleAniTIme);
		Utils.RegisterFunc(L, -3, "SetIdleAniManagerTryToNextAni", _m_SetIdleAniManagerTryToNextAni);
		Utils.RegisterFunc(L, -3, "OnDynamicModelUnload", _m_OnDynamicModelUnload);
		Utils.RegisterFunc(L, -2, "PreviewType", _g_get_PreviewType);
		Utils.RegisterFunc(L, -2, "TilePos", _g_get_TilePos);
		Utils.RegisterFunc(L, -2, "Uuid", _g_get_Uuid);
		Utils.RegisterFunc(L, -2, "AdjustLod", _g_get_AdjustLod);
		Utils.RegisterFunc(L, -2, "OpenFakeAutoDestroy", _g_get_OpenFakeAutoDestroy);
		Utils.RegisterFunc(L, -2, "AutoAdjustLod", _g_get_AutoAdjustLod);
		Utils.RegisterFunc(L, -2, "previewIconPath", _g_get_previewIconPath);
		Utils.RegisterFunc(L, -2, "previewName", _g_get_previewName);
		Utils.RegisterFunc(L, -2, "previewType", _g_get_previewType);
		Utils.RegisterFunc(L, -2, "build_Id", _g_get_build_Id);
		Utils.RegisterFunc(L, -2, "tileX", _g_get_tileX);
		Utils.RegisterFunc(L, -2, "tileY", _g_get_tileY);
		Utils.RegisterFunc(L, -2, "scan", _g_get_scan);
		Utils.RegisterFunc(L, -2, "build_type", _g_get_build_type);
		Utils.RegisterFunc(L, -1, "Uuid", _s_set_Uuid);
		Utils.RegisterFunc(L, -1, "previewIconPath", _s_set_previewIconPath);
		Utils.RegisterFunc(L, -1, "previewName", _s_set_previewName);
		Utils.RegisterFunc(L, -1, "previewType", _s_set_previewType);
		Utils.RegisterFunc(L, -1, "build_Id", _s_set_build_Id);
		Utils.RegisterFunc(L, -1, "tileX", _s_set_tileX);
		Utils.RegisterFunc(L, -1, "tileY", _s_set_tileY);
		Utils.RegisterFunc(L, -1, "scan", _s_set_scan);
		Utils.RegisterFunc(L, -1, "build_type", _s_set_build_type);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 4, 4);
		Utils.RegisterFunc(L, -2, "ScreenRangeLeft", _g_get_ScreenRangeLeft);
		Utils.RegisterFunc(L, -2, "ScreenRangeRight", _g_get_ScreenRangeRight);
		Utils.RegisterFunc(L, -2, "ScreenRangeTop", _g_get_ScreenRangeTop);
		Utils.RegisterFunc(L, -2, "ScreenRangeDown", _g_get_ScreenRangeDown);
		Utils.RegisterFunc(L, -1, "ScreenRangeLeft", _s_set_ScreenRangeLeft);
		Utils.RegisterFunc(L, -1, "ScreenRangeRight", _s_set_ScreenRangeRight);
		Utils.RegisterFunc(L, -1, "ScreenRangeTop", _s_set_ScreenRangeTop);
		Utils.RegisterFunc(L, -1, "ScreenRangeDown", _s_set_ScreenRangeDown);
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
				WorldBuilding o = new WorldBuilding();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerDown(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnPointerDown();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = ((WorldBuilding)objectTranslator.FastGetCSObj(L, 1)).GetPointInfo();
			objectTranslator.Push(L, pointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildInfo = ((WorldBuilding)objectTranslator.FastGetCSObj(L, 1)).GetBuildInfo();
			objectTranslator.Push(L, buildInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckFakeAutoDestroy(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckFakeAutoDestroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitLod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			worldBuilding.InitLod(gameObject);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoFoldUpAnim(IntPtr L)
	{
		try
		{
			float num = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoFoldUpAnim();
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPlayIdleAniManager(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearPlayIdleAniManager();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoExtendDome(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoExtendDome();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoUpgradeDome(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoUpgradeDome();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_refeshDate(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refeshDate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterMoveCityState(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int moveCityType = Lua.xlua_tointeger(L, 2);
			obj.EnterMoveCityState(moveCityType);
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
			Transform transform = ((WorldBuilding)objectTranslator.FastGetCSObj(L, 1)).GetTransform();
			objectTranslator.Push(L, transform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PointInPick(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PointInPick();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Drag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldBuilding.Drag(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Select(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Select();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanLongTap(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyDomeInstance(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyDomeInstance();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOutRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = worldBuilding.IsOutRange(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeTouchPos(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.ChangeTouchPos(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBattleDefUpdate(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int damage = Lua.xlua_tointeger(L, 2);
			obj.OnBattleDefUpdate(damage);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetParam(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int posIndex = Lua.xlua_tointeger(L, 2);
				worldBuilding.ResetParam(posIndex);
				return 0;
			}
			if (num == 1)
			{
				worldBuilding.ResetParam();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.ResetParam!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeMove(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeMove();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClosestPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 closestPoint = worldBuilding.GetClosestPoint(val);
			objectTranslator.PushUnityEngineVector3(L, closestPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasAnimClip(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			bool value = obj.HasAnimClip(animName);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			float num = obj.PlayAnim(animName);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoBuildClickAnim(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoBuildClickAnim();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoBuildPlaceAnim(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoBuildPlaceAnim();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPlayBuildingAnim(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation playBuildingAnim = ((WorldBuilding)objectTranslator.FastGetCSObj(L, 1)).GetPlayBuildingAnim();
			objectTranslator.Push(L, playBuildingAnim);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnClick(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClick();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginLongTap(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeginLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndLongTap(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEndLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
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
			float height = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHeight();
			Lua.lua_pushnumber(L, height);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowDome(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowDome();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideDome(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideDome();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSelf(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSelf();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateCityLabel(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long obj2 = Lua.lua_toint64(L, 2);
			obj.UpdateCityLabel(obj2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoGuideStartShow(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int time = Lua.xlua_tointeger(L, 2);
			obj.DoGuideStartShow(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeToBox(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeToBox();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBattleAtkUpdate(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long targetUuid = Lua.lua_toint64(L, 2);
			obj.OnBattleAtkUpdate(targetUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBattleAtkEnd(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBattleAtkEnd();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ContainsPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			bool value = worldBuilding.ContainsPos(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggleGlass(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleGlass();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRuins(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsRuins();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMoveState(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool moveState = Lua.lua_toboolean(L, 2);
			obj.SetMoveState(moveState);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckPlayAnimation(IntPtr L)
	{
		try
		{
			bool value = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckPlayAnimation();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimation(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName = Lua.lua_tostring(L, 2);
				float normalize = (float)Lua.lua_tonumber(L, 3);
				worldBuilding.PlayAnimation(animName, normalize);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayAnimation(animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayAnimation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayCrossFadeAnimation(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName = Lua.lua_tostring(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				worldBuilding.PlayCrossFadeAnimation(animName, time);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayCrossFadeAnimation(animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayCrossFadeAnimation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimationNormalizeAndCrossFade(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string animName = Lua.lua_tostring(L, 2);
				float normalize = (float)Lua.lua_tonumber(L, 3);
				float time = (float)Lua.lua_tonumber(L, 4);
				worldBuilding.PlayAnimationNormalizeAndCrossFade(animName, normalize, time);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				float normalize2 = (float)Lua.lua_tonumber(L, 3);
				worldBuilding.PlayAnimationNormalizeAndCrossFade(animName2, normalize2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName3 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayAnimationNormalizeAndCrossFade(animName3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayAnimationNormalizeAndCrossFade!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimationEffect(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName = Lua.lua_tostring(L, 2);
				float startTime = (float)Lua.lua_tonumber(L, 3);
				worldBuilding.PlayAnimationEffect(animName, startTime);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayAnimationEffect(animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayAnimationEffect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimationEffectAni(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName = Lua.lua_tostring(L, 2);
				float startTime = (float)Lua.lua_tonumber(L, 3);
				worldBuilding.PlayAnimationEffectAni(animName, startTime);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayAnimationEffectAni(animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayAnimationEffectAni!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayTimeline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<DirectorWrapMode>(L, 4))
			{
				string animName = Lua.lua_tostring(L, 2);
				bool toIdle = Lua.lua_toboolean(L, 3);
				objectTranslator.Get(L, 4, out DirectorWrapMode v);
				worldBuilding.PlayTimeline(animName, toIdle, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				bool toIdle2 = Lua.lua_toboolean(L, 3);
				worldBuilding.PlayTimeline(animName2, toIdle2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName3 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayTimeline(animName3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayTimeline!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEnable(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEnable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLodChangeFunc(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			obj.OnLodChangeFunc(lod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimationAndEffectReturnTime(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string animName = Lua.lua_tostring(L, 2);
				float normalize = (float)Lua.lua_tonumber(L, 3);
				float startTime = (float)Lua.lua_tonumber(L, 4);
				float crossTime = (float)Lua.lua_tonumber(L, 5);
				worldBuilding.PlayAnimationAndEffectReturnTime(animName, normalize, startTime, crossTime);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				float normalize2 = (float)Lua.lua_tonumber(L, 3);
				float startTime2 = (float)Lua.lua_tonumber(L, 4);
				worldBuilding.PlayAnimationAndEffectReturnTime(animName2, normalize2, startTime2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName3 = Lua.lua_tostring(L, 2);
				float normalize3 = (float)Lua.lua_tonumber(L, 3);
				worldBuilding.PlayAnimationAndEffectReturnTime(animName3, normalize3);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName4 = Lua.lua_tostring(L, 2);
				worldBuilding.PlayAnimationAndEffectReturnTime(animName4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.PlayAnimationAndEffectReturnTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayCrossFadeAnim(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			float mixTIme = (float)Lua.lua_tonumber(L, 3);
			float num = obj.PlayCrossFadeAnim(animName, mixTIme);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAnimationLength(IntPtr L)
	{
		try
		{
			WorldBuilding obj = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			float animationLength = obj.GetAnimationLength(animName);
			Lua.lua_pushnumber(L, animationLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateStatus(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateStatus();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshMeteorite(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshMeteorite();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FakeEpidemicSkill(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FakeEpidemicSkill();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshEpidemicSkill(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshEpidemicSkill();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitDynamicModel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Action>(L, 3))
			{
				int skinId = Lua.xlua_tointeger(L, 2);
				Action onLoaded = objectTranslator.GetDelegate<Action>(L, 3);
				worldBuilding.InitDynamicModel(skinId, onLoaded);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int skinId2 = Lua.xlua_tointeger(L, 2);
				worldBuilding.InitDynamicModel(skinId2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuilding.InitDynamicModel!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadDynamicModel(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadDynamicModel();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDynamicModelLoad(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			int skinId = Lua.xlua_tointeger(L, 2);
			SimpleAnimation buildingAnim = (SimpleAnimation)objectTranslator.GetObject(L, 3, typeof(SimpleAnimation));
			WorldBuildingAniEffect buildingAniEffect = (WorldBuildingAniEffect)objectTranslator.GetObject(L, 4, typeof(WorldBuildingAniEffect));
			WorldBuildingAniEffectAni buildingAniEffectAni = (WorldBuildingAniEffectAni)objectTranslator.GetObject(L, 5, typeof(WorldBuildingAniEffectAni));
			worldBuilding.OnDynamicModelLoad(skinId, buildingAnim, buildingAniEffect, buildingAniEffectAni);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurIdleAniTIme(IntPtr L)
	{
		try
		{
			float curIdleAniTIme = ((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurIdleAniTIme();
			Lua.lua_pushnumber(L, curIdleAniTIme);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIdleAniManagerTryToNextAni(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetIdleAniManagerTryToNextAni();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDynamicModelUnload(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDynamicModelUnload();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PreviewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuilding.PreviewType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuilding.TilePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Uuid(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldBuilding.Uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AdjustLod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuilding.AdjustLod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OpenFakeAutoDestroy(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldBuilding.OpenFakeAutoDestroy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoAdjustLod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuilding.AutoAdjustLod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ScreenRangeLeft(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldBuilding.ScreenRangeLeft);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ScreenRangeRight(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldBuilding.ScreenRangeRight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ScreenRangeTop(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldBuilding.ScreenRangeTop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ScreenRangeDown(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldBuilding.ScreenRangeDown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewIconPath(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldBuilding.previewIconPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewName(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldBuilding.previewName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuilding.previewType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_build_Id(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldBuilding.build_Id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tileX(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldBuilding.tileX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tileY(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldBuilding.tileY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scan(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldBuilding.scan);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_build_type(IntPtr L)
	{
		try
		{
			WorldBuilding worldBuilding = (WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldBuilding.build_type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Uuid(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ScreenRangeLeft(IntPtr L)
	{
		try
		{
			WorldBuilding.ScreenRangeLeft = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ScreenRangeRight(IntPtr L)
	{
		try
		{
			WorldBuilding.ScreenRangeRight = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ScreenRangeTop(IntPtr L)
	{
		try
		{
			WorldBuilding.ScreenRangeTop = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ScreenRangeDown(IntPtr L)
	{
		try
		{
			WorldBuilding.ScreenRangeDown = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewIconPath(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).previewIconPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewName(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).previewName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuilding worldBuilding = (WorldBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldPreviewType v);
			worldBuilding.previewType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_build_Id(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).build_Id = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tileX(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tileX = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tileY(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tileY = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scan(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scan = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_build_type(IntPtr L)
	{
		try
		{
			((WorldBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).build_type = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
