using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CityBuildingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CityBuilding);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 42, 8, 7);
		Utils.RegisterFunc(L, -3, "SetBuildUpLevelTipActive", _m_SetBuildUpLevelTipActive);
		Utils.RegisterFunc(L, -3, "GetBuildInfo", _m_GetBuildInfo);
		Utils.RegisterFunc(L, -3, "GetPointInfo", _m_GetPointInfo);
		Utils.RegisterFunc(L, -3, "DoFoldUpAnim", _m_DoFoldUpAnim);
		Utils.RegisterFunc(L, -3, "OnEnable", _m_OnEnable);
		Utils.RegisterFunc(L, -3, "refeshDate", _m_refeshDate);
		Utils.RegisterFunc(L, -3, "GetTransform", _m_GetTransform);
		Utils.RegisterFunc(L, -3, "PointInPick", _m_PointInPick);
		Utils.RegisterFunc(L, -3, "Drag", _m_Drag);
		Utils.RegisterFunc(L, -3, "Select", _m_Select);
		Utils.RegisterFunc(L, -3, "CanLongTap", _m_CanLongTap);
		Utils.RegisterFunc(L, -3, "IsOutRange", _m_IsOutRange);
		Utils.RegisterFunc(L, -3, "ChangeTouchPos", _m_ChangeTouchPos);
		Utils.RegisterFunc(L, -3, "ResetParam", _m_ResetParam);
		Utils.RegisterFunc(L, -3, "ChangeMove", _m_ChangeMove);
		Utils.RegisterFunc(L, -3, "GetClosestPoint", _m_GetClosestPoint);
		Utils.RegisterFunc(L, -3, "HasAnimClip", _m_HasAnimClip);
		Utils.RegisterFunc(L, -3, "PlayAnim", _m_PlayAnim);
		Utils.RegisterFunc(L, -3, "PlayAnimationEffect", _m_PlayAnimationEffect);
		Utils.RegisterFunc(L, -3, "PlayCrossFadeAnim", _m_PlayCrossFadeAnim);
		Utils.RegisterFunc(L, -3, "DoBuildClickAnim", _m_DoBuildClickAnim);
		Utils.RegisterFunc(L, -3, "DoBuildPlaceAnim", _m_DoBuildPlaceAnim);
		Utils.RegisterFunc(L, -3, "OnClick", _m_OnClick);
		Utils.RegisterFunc(L, -3, "OnBeginLongTap", _m_OnBeginLongTap);
		Utils.RegisterFunc(L, -3, "OnEndLongTap", _m_OnEndLongTap);
		Utils.RegisterFunc(L, -3, "GetHeight", _m_GetHeight);
		Utils.RegisterFunc(L, -3, "IsSelf", _m_IsSelf);
		Utils.RegisterFunc(L, -3, "ChangeToBox", _m_ChangeToBox);
		Utils.RegisterFunc(L, -3, "OnBattleAtkUpdate", _m_OnBattleAtkUpdate);
		Utils.RegisterFunc(L, -3, "IsRuins", _m_IsRuins);
		Utils.RegisterFunc(L, -3, "SetCanDoAnim", _m_SetCanDoAnim);
		Utils.RegisterFunc(L, -3, "SetVisible", _m_SetVisible);
		Utils.RegisterFunc(L, -3, "AddActivityAlarmClockEffect", _m_AddActivityAlarmClockEffect);
		Utils.RegisterFunc(L, -3, "UpdateActivityAlarmClockTimeShow", _m_UpdateActivityAlarmClockTimeShow);
		Utils.RegisterFunc(L, -3, "PlayBoxShake", _m_PlayBoxShake);
		Utils.RegisterFunc(L, -3, "GetUpgradeObj", _m_GetUpgradeObj);
		Utils.RegisterFunc(L, -3, "InitDynamicModel", _m_InitDynamicModel);
		Utils.RegisterFunc(L, -3, "UnloadDynamicModel", _m_UnloadDynamicModel);
		Utils.RegisterFunc(L, -3, "GetAnimationLength", _m_GetAnimationLength);
		Utils.RegisterFunc(L, -3, "CheckPlayAnimation", _m_CheckPlayAnimation);
		Utils.RegisterFunc(L, -3, "OnDynamicModelLoad", _m_OnDynamicModelLoad);
		Utils.RegisterFunc(L, -3, "OnDynamicModelUnload", _m_OnDynamicModelUnload);
		Utils.RegisterFunc(L, -2, "PreviewType", _g_get_PreviewType);
		Utils.RegisterFunc(L, -2, "TilePos", _g_get_TilePos);
		Utils.RegisterFunc(L, -2, "Uuid", _g_get_Uuid);
		Utils.RegisterFunc(L, -2, "build_Id", _g_get_build_Id);
		Utils.RegisterFunc(L, -2, "tileX", _g_get_tileX);
		Utils.RegisterFunc(L, -2, "tileY", _g_get_tileY);
		Utils.RegisterFunc(L, -2, "scan", _g_get_scan);
		Utils.RegisterFunc(L, -2, "build_type", _g_get_build_type);
		Utils.RegisterFunc(L, -1, "TilePos", _s_set_TilePos);
		Utils.RegisterFunc(L, -1, "Uuid", _s_set_Uuid);
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
				CityBuilding o = new CityBuilding();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityBuilding constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBuildUpLevelTipActive(IntPtr L)
	{
		try
		{
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool buildUpLevelTipActive = Lua.lua_toboolean(L, 2);
			obj.SetBuildUpLevelTipActive(buildUpLevelTipActive);
			return 0;
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
			BuildPointInfo buildInfo = ((CityBuilding)objectTranslator.FastGetCSObj(L, 1)).GetBuildInfo();
			objectTranslator.Push(L, buildInfo);
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
			PointInfo pointInfo = ((CityBuilding)objectTranslator.FastGetCSObj(L, 1)).GetPointInfo();
			objectTranslator.Push(L, pointInfo);
			return 1;
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
			float num = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoFoldUpAnim();
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEnable(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEnable();
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refeshDate();
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
			Transform transform = ((CityBuilding)objectTranslator.FastGetCSObj(L, 1)).GetTransform();
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PointInPick();
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityBuilding.Drag(val);
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Select();
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = cityBuilding.IsOutRange(val);
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
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_ResetParam(IntPtr L)
	{
		try
		{
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int posIndex = Lua.xlua_tointeger(L, 2);
				cityBuilding.ResetParam(posIndex);
				return 0;
			}
			if (num == 1)
			{
				cityBuilding.ResetParam();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityBuilding.ResetParam!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeMove(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeMove();
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 closestPoint = cityBuilding.GetClosestPoint(val);
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
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_PlayAnimationEffect(IntPtr L)
	{
		try
		{
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animName = Lua.lua_tostring(L, 2);
				float startTime = (float)Lua.lua_tonumber(L, 3);
				cityBuilding.PlayAnimationEffect(animName, startTime);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animName2 = Lua.lua_tostring(L, 2);
				cityBuilding.PlayAnimationEffect(animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityBuilding.PlayAnimationEffect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayCrossFadeAnim(IntPtr L)
	{
		try
		{
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_DoBuildClickAnim(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoBuildClickAnim();
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoBuildPlaceAnim();
			return 0;
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClick();
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeginLongTap();
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEndLongTap();
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
			float height = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHeight();
			Lua.lua_pushnumber(L, height);
			return 1;
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
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSelf();
			Lua.lua_pushboolean(L, value);
			return 1;
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeToBox();
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
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsRuins(IntPtr L)
	{
		try
		{
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsRuins();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCanDoAnim(IntPtr L)
	{
		try
		{
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool canDoAnim = Lua.lua_toboolean(L, 2);
			obj.SetCanDoAnim(canDoAnim);
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
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_AddActivityAlarmClockEffect(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AddActivityAlarmClockEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateActivityAlarmClockTimeShow(IntPtr L)
	{
		try
		{
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isShowServerTime = Lua.lua_toboolean(L, 2);
			obj.UpdateActivityAlarmClockTimeShow(isShowServerTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBoxShake(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayBoxShake();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUpgradeObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject upgradeObj = ((CityBuilding)objectTranslator.FastGetCSObj(L, 1)).GetUpgradeObj();
			objectTranslator.Push(L, upgradeObj);
			return 1;
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Action>(L, 3))
			{
				int skinId = Lua.xlua_tointeger(L, 2);
				Action onLoaded = objectTranslator.GetDelegate<Action>(L, 3);
				cityBuilding.InitDynamicModel(skinId, onLoaded);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int skinId2 = Lua.xlua_tointeger(L, 2);
				cityBuilding.InitDynamicModel(skinId2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityBuilding.InitDynamicModel!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadDynamicModel(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadDynamicModel();
			return 0;
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
			CityBuilding obj = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_CheckPlayAnimation(IntPtr L)
	{
		try
		{
			bool value = ((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckPlayAnimation();
			Lua.lua_pushboolean(L, value);
			return 1;
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			int skinId = Lua.xlua_tointeger(L, 2);
			SimpleAnimation buildingAnim = (SimpleAnimation)objectTranslator.GetObject(L, 3, typeof(SimpleAnimation));
			WorldBuildingAniEffect buildingAniEffect = (WorldBuildingAniEffect)objectTranslator.GetObject(L, 4, typeof(WorldBuildingAniEffect));
			WorldBuildingAniEffectAni buildingAniEffectAni = (WorldBuildingAniEffectAni)objectTranslator.GetObject(L, 5, typeof(WorldBuildingAniEffectAni));
			cityBuilding.OnDynamicModelLoad(skinId, buildingAnim, buildingAniEffect, buildingAniEffectAni);
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDynamicModelUnload();
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityBuilding.PreviewType);
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
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityBuilding.TilePos);
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
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, cityBuilding.Uuid);
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
			Lua.lua_pushnumber(L, CityBuilding.ScreenRangeLeft);
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
			Lua.lua_pushnumber(L, CityBuilding.ScreenRangeRight);
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
			Lua.lua_pushnumber(L, CityBuilding.ScreenRangeTop);
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
			Lua.lua_pushnumber(L, CityBuilding.ScreenRangeDown);
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
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityBuilding.build_Id);
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
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityBuilding.tileX);
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
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityBuilding.tileY);
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
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityBuilding.scan);
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
			CityBuilding cityBuilding = (CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityBuilding.build_type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityBuilding cityBuilding = (CityBuilding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			cityBuilding.TilePos = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Uuid(IntPtr L)
	{
		try
		{
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uuid = Lua.lua_toint64(L, 2);
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
			CityBuilding.ScreenRangeLeft = (float)Lua.lua_tonumber(L, 1);
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
			CityBuilding.ScreenRangeRight = (float)Lua.lua_tonumber(L, 1);
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
			CityBuilding.ScreenRangeTop = (float)Lua.lua_tonumber(L, 1);
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
			CityBuilding.ScreenRangeDown = (float)Lua.lua_tonumber(L, 1);
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).build_Id = Lua.xlua_tointeger(L, 2);
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tileX = Lua.xlua_tointeger(L, 2);
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tileY = Lua.xlua_tointeger(L, 2);
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scan = Lua.xlua_tointeger(L, 2);
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
			((CityBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).build_type = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
