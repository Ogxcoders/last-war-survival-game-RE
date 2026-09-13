using System;
using PVEBattleLogic.Unit;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PVEBattleLogicUnitUnitViewFacadeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnitViewFacade);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 85, 0, 0);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitUnitView", _m_InitUnitView_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitUnitView", _m_UnInitUnitView_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateUnitViewList", _m_CreateUnitViewList_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateUnitView", _m_CreateUnitView_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddAgent", _m_AddAgent_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadUnitView", _m_PreloadUnitView_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitFirePoints", _m_InitFirePoints_xlua_st_);
		Utils.RegisterFunc(L, -4, "AppendFirePoint", _m_AppendFirePoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetFirePointById", _m_GetFirePointById_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitCompPoints", _m_InitCompPoints_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddCompPoint", _m_AddCompPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCmpPointByIndex", _m_GetCmpPointByIndex_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTransformPoint", _m_GetTransformPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitBuffPoints", _m_InitBuffPoints_xlua_st_);
		Utils.RegisterFunc(L, -4, "AppendBuffPoint", _m_AppendBuffPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitUIPoint", _m_InitUIPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetUIPoint", _m_GetUIPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRotateRoot", _m_GetRotateRoot_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitCannon", _m_InitCannon_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetCannon", _m_ResetCannon_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGameObject", _m_GetGameObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTransform", _m_GetTransform_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitTxtNumberText", _m_InitTxtNumberText_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitHpText", _m_InitHpText_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowHpTweenScale", _m_ShowHpTweenScale_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetNumberHpText", _m_SetNumberHpText_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetNumberText", _m_SetNumberText_xlua_st_);
		Utils.RegisterFunc(L, -4, "NumberHpTextActive", _m_NumberHpTextActive_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetLocalPosition", _m_SetLocalPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetPosition", _m_SetPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "MoveToLocalPos", _m_MoveToLocalPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPositionXYZ", _m_GetPositionXYZ_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetLocalScaleX", _m_GetLocalScaleX_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetLocalScaleX", _m_SetLocalScaleX_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlaySimpleAnim", _m_PlaySimpleAnim_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSimpleAnimation", _m_GetSimpleAnimation_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSimpleAnimState", _m_GetSimpleAnimState_xlua_st_);
		Utils.RegisterFunc(L, -4, "RewindAndPlaySimpleAnim", _m_RewindAndPlaySimpleAnim_xlua_st_);
		Utils.RegisterFunc(L, -4, "CrossFadeSimpleAnim", _m_CrossFadeSimpleAnim_xlua_st_);
		Utils.RegisterFunc(L, -4, "RewindSimpleAnim", _m_RewindSimpleAnim_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCurAnimName", _m_GetCurAnimName_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAnimLength", _m_GetAnimLength_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyUnitView", _m_DestroyUnitView_xlua_st_);
		Utils.RegisterFunc(L, -4, "UseGPUSkinMaterial", _m_UseGPUSkinMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRedMaterial", _m_GetRedMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetWhiteMaterial", _m_GetWhiteMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceMaterialFlashRed", _m_ReplaceMaterialFlashRed_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceMaterialFlashWhite", _m_ReplaceMaterialFlashWhite_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceMaterialReset", _m_ReplaceMaterialReset_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceMaterial", _m_ReplaceMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBFlashRed", _m_MPBFlashRed_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBResetGray", _m_MPBResetGray_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBFlashGray", _m_MPBFlashGray_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBModelScale", _m_MPBModelScale_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBBornEffect", _m_MPBBornEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBShieldEffect", _m_MPBShieldEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "MPBReset", _m_MPBReset_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCollider", _m_GetCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "EnableCollider", _m_EnableCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetVisible", _m_SetVisible_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckLoaded", _m_CheckLoaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "RegisterUpdater", _m_RegisterUpdater_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnregisterUpdater", _m_UnregisterUpdater_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveUpdater", _m_RemoveUpdater_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateHpBarListWithHandle", _m_CreateHpBarListWithHandle_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateSelfHpBar", _m_CreateSelfHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateSelfHpBarWithHandle", _m_CreateSelfHpBarWithHandle_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateEnemyHpBar", _m_CreateEnemyHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateEnemyHpBarWithHandle", _m_CreateEnemyHpBarWithHandle_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetHpBar", _m_SetHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetHpBarOffsetX", _m_SetHpBarOffsetX_xlua_st_);
		Utils.RegisterFunc(L, -4, "EnableHpBar", _m_EnableHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceHpBarTarget", _m_ReplaceHpBarTarget_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceHpBarTargetWithHandle", _m_ReplaceHpBarTargetWithHandle_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadHpBar", _m_PreloadHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyHpBar", _m_DestroyHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateHpBar", _m_UpdateHpBar_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitGlassAndWaterRender", _m_InitGlassAndWaterRender_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGlassCrackEffect", _m_SetGlassCrackEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetWaveIntensityEffect", _m_SetWaveIntensityEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAll", _m_ClearAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dispose", _m_Dispose_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "INVALID_HANDLE", -1);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PVEBattleLogic.Unit.UnitViewFacade does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitUnitView_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LuaArrAccess luaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 1, typeof(LuaArrAccess));
			LuaArrAccess unitViewListLuaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 2, typeof(LuaArrAccess));
			LuaArrAccess hpBarLuaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 3, typeof(LuaArrAccess));
			LuaArrAccess hpBarListLuaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 4, typeof(LuaArrAccess));
			UnitViewFacade.InitUnitView(luaArrAccess, unitViewListLuaArrAccess, hpBarLuaArrAccess, hpBarListLuaArrAccess);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitUnitView_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.UnInitUnitView();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateUnitViewList_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			int count = Lua.xlua_tointeger(L, 3);
			bool[] loaded;
			int[] o = UnitViewFacade.CreateUnitViewList(path, parent, count, out loaded);
			objectTranslator.Push(L, o);
			objectTranslator.Push(L, loaded);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateUnitView_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			bool loaded;
			int value = UnitViewFacade.CreateUnitView(path, parent, out loaded);
			Lua.xlua_pushinteger(L, value);
			Lua.lua_pushboolean(L, loaded);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddAgent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LWBattleRVOAgent o = UnitViewFacade.AddAgent((LWBattleRVOManager)objectTranslator.GetObject(L, 1, typeof(LWBattleRVOManager)));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadUnitView_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			UnitViewFacade.PreloadUnitView(path, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitFirePoints_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int appearanceId = Lua.xlua_tointeger(L, 2);
			int count = Lua.xlua_tointeger(L, 3);
			bool value = UnitViewFacade.InitFirePoints(handle, appearanceId, count);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AppendFirePoint_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			string firePointPath = Lua.lua_tostring(L, 2);
			UnitViewFacade.AppendFirePoint(handle, firePointPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFirePointById_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			Transform firePointById = UnitViewFacade.GetFirePointById(handle, id);
			objectTranslator.Push(L, firePointById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitCompPoints_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int appearanceId = Lua.xlua_tointeger(L, 2);
			int count = Lua.xlua_tointeger(L, 3);
			bool value = UnitViewFacade.InitCompPoints(handle, appearanceId, count);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCompPoint_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			string cmpAttachedPath = Lua.lua_tostring(L, 3);
			bool value = UnitViewFacade.AddCompPoint(handle, index, cmpAttachedPath);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCmpPointByIndex_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			Transform cmpPointByIndex = UnitViewFacade.GetCmpPointByIndex(handle, index);
			objectTranslator.Push(L, cmpPointByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTransformPoint_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float posX = (float)Lua.lua_tonumber(L, 2);
			float posY = (float)Lua.lua_tonumber(L, 3);
			float posZ = (float)Lua.lua_tonumber(L, 4);
			UnitViewFacade.GetTransformPoint(handle, posX, posY, posZ, out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBuffPoints_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			UnitViewFacade.InitBuffPoints(handle, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AppendBuffPoint_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			string buffPointPath = Lua.lua_tostring(L, 2);
			UnitViewFacade.AppendBuffPoint(handle, buffPointPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitUIPoint_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			string uiPointPath = Lua.lua_tostring(L, 2);
			UnitViewFacade.InitUIPoint(handle, uiPointPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUIPoint_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform uIPoint = UnitViewFacade.GetUIPoint(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, uIPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRotateRoot_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform rotateRoot = UnitViewFacade.GetRotateRoot(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, rotateRoot);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitCannon_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			string cannonPath = Lua.lua_tostring(L, 2);
			Transform o = UnitViewFacade.InitCannon(handle, cannonPath);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCannon_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ResetCannon(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = UnitViewFacade.GetGameObject(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, gameObject);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTransform_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = UnitViewFacade.GetTransform(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, transform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitTxtNumberText_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			int number = Lua.xlua_tointeger(L, 2);
			Transform o = UnitViewFacade.InitTxtNumberText(handle, number);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitHpText_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			int hp = Lua.xlua_tointeger(L, 2);
			Transform o = UnitViewFacade.InitHpText(handle, hp);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowHpTweenScale_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ShowHpTweenScale(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNumberHpText_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int number = Lua.xlua_tointeger(L, 2);
			UnitViewFacade.SetNumberHpText(handle, number);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNumberText_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int number = Lua.xlua_tointeger(L, 2);
			UnitViewFacade.SetNumberText(handle, number);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NumberHpTextActive_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			bool active = Lua.lua_toboolean(L, 2);
			UnitViewFacade.NumberHpTextActive(handle, active);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalPosition_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			UnitViewFacade.SetLocalPosition(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosition_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			UnitViewFacade.SetPosition(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveToLocalPos_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			float time = (float)Lua.lua_tonumber(L, 5);
			UnitViewFacade.MoveToLocalPos(handle, x, y, z, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPositionXYZ_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.GetPositionXYZ(Lua.xlua_tointeger(L, 1), out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLocalScaleX_xlua_st_(IntPtr L)
	{
		try
		{
			float localScaleX = UnitViewFacade.GetLocalScaleX(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushnumber(L, localScaleX);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalScaleX_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float newScale = (float)Lua.lua_tonumber(L, 2);
			UnitViewFacade.SetLocalScaleX(handle, newScale);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySimpleAnim_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int handle = Lua.xlua_tointeger(L, 1);
				string animName = Lua.lua_tostring(L, 2);
				float speed = (float)Lua.lua_tonumber(L, 3);
				UnitViewFacade.PlaySimpleAnim(handle, animName, speed);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				int handle2 = Lua.xlua_tointeger(L, 1);
				string animName2 = Lua.lua_tostring(L, 2);
				UnitViewFacade.PlaySimpleAnim(handle2, animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Unit.UnitViewFacade.PlaySimpleAnim!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSimpleAnimation_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = UnitViewFacade.GetSimpleAnimation(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, simpleAnimation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSimpleAnimState_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			SimpleAnimation.State simpleAnimState = UnitViewFacade.GetSimpleAnimState(handle, animName);
			objectTranslator.PushAny(L, simpleAnimState);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RewindAndPlaySimpleAnim_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int handle = Lua.xlua_tointeger(L, 1);
				string animName = Lua.lua_tostring(L, 2);
				float speed = (float)Lua.lua_tonumber(L, 3);
				UnitViewFacade.RewindAndPlaySimpleAnim(handle, animName, speed);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				int handle2 = Lua.xlua_tointeger(L, 1);
				string animName2 = Lua.lua_tostring(L, 2);
				UnitViewFacade.RewindAndPlaySimpleAnim(handle2, animName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Unit.UnitViewFacade.RewindAndPlaySimpleAnim!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeSimpleAnim_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int handle = Lua.xlua_tointeger(L, 1);
				string animName = Lua.lua_tostring(L, 2);
				float speed = (float)Lua.lua_tonumber(L, 3);
				float fadeTime = (float)Lua.lua_tonumber(L, 4);
				UnitViewFacade.CrossFadeSimpleAnim(handle, animName, speed, fadeTime);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int handle2 = Lua.xlua_tointeger(L, 1);
				string animName2 = Lua.lua_tostring(L, 2);
				float speed2 = (float)Lua.lua_tonumber(L, 3);
				UnitViewFacade.CrossFadeSimpleAnim(handle2, animName2, speed2);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				int handle3 = Lua.xlua_tointeger(L, 1);
				string animName3 = Lua.lua_tostring(L, 2);
				UnitViewFacade.CrossFadeSimpleAnim(handle3, animName3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Unit.UnitViewFacade.CrossFadeSimpleAnim!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RewindSimpleAnim_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			UnitViewFacade.RewindSimpleAnim(handle, animName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurAnimName_xlua_st_(IntPtr L)
	{
		try
		{
			string curAnimName = UnitViewFacade.GetCurAnimName(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushstring(L, curAnimName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAnimLength_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			float animLength = UnitViewFacade.GetAnimLength(handle, animName);
			Lua.lua_pushnumber(L, animLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyUnitView_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			UnitViewFacade.DestroyUnitView(ref handle);
			Lua.xlua_pushinteger(L, handle);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UseGPUSkinMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.UseGPUSkinMaterial(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRedMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material redMaterial = UnitViewFacade.GetRedMaterial();
			objectTranslator.Push(L, redMaterial);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWhiteMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material whiteMaterial = UnitViewFacade.GetWhiteMaterial();
			objectTranslator.Push(L, whiteMaterial);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceMaterialFlashRed_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ReplaceMaterialFlashRed(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceMaterialFlashWhite_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ReplaceMaterialFlashWhite(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceMaterialReset_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				UnitViewFacade.ReplaceMaterialReset(Lua.xlua_tointeger(L, 1));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Renderer>(L, 1) && objectTranslator.Assignable<Material>(L, 2))
			{
				Renderer renderer = (Renderer)objectTranslator.GetObject(L, 1, typeof(Renderer));
				Material material = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
				UnitViewFacade.ReplaceMaterialReset(renderer, material);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Unit.UnitViewFacade.ReplaceMaterialReset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.GetObject(L, 1, typeof(Renderer));
			Material material = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			UnitViewFacade.ReplaceMaterial(renderer, material);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBFlashRed_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBFlashRed(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBResetGray_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBResetGray(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBFlashGray_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBFlashGray(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBModelScale_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBModelScale(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBBornEffect_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBBornEffect(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBShieldEffect_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBShieldEffect(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MPBReset_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.MPBReset(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollider_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = UnitViewFacade.GetCollider(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, collider);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableCollider_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			bool enable = Lua.lua_toboolean(L, 2);
			UnitViewFacade.EnableCollider(handle, enable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisible_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			bool visible = Lua.lua_toboolean(L, 2);
			UnitViewFacade.SetVisible(handle, visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckLoaded_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.CheckLoaded();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterUpdater_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float updateZ = (float)Lua.lua_tonumber(L, 2);
			UnitViewFacade.RegisterUpdater(handle, updateZ);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterUpdater_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.UnregisterUpdater(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveUpdater_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.RemoveUpdater();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateHpBarListWithHandle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] o = UnitViewFacade.CreateHpBarListWithHandle(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateSelfHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			int value = UnitViewFacade.CreateSelfHpBar((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateSelfHpBarWithHandle_xlua_st_(IntPtr L)
	{
		try
		{
			int value = UnitViewFacade.CreateSelfHpBarWithHandle();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateEnemyHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			int value = UnitViewFacade.CreateEnemyHpBar((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateEnemyHpBarWithHandle_xlua_st_(IntPtr L)
	{
		try
		{
			int value = UnitViewFacade.CreateEnemyHpBarWithHandle();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.SetHpBar();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetHpBarOffsetX_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.SetHpBarOffsetX();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.EnableHpBar();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceHpBarTarget_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ReplaceHpBarTarget((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceHpBarTargetWithHandle_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ReplaceHpBarTargetWithHandle();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.PreloadHpBar(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			UnitViewFacade.DestroyHpBar(ref handle);
			Lua.xlua_pushinteger(L, handle);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateHpBar_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.UpdateHpBar();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitGlassAndWaterRender_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			string glassRenderName = Lua.lua_tostring(L, 2);
			string waterRenderName = Lua.lua_tostring(L, 3);
			UnitViewFacade.InitGlassAndWaterRender(handle, glassRenderName, waterRenderName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlassCrackEffect_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			int crackValue = Lua.xlua_tointeger(L, 2);
			UnitViewFacade.SetGlassCrackEffect(handle, crackValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWaveIntensityEffect_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float heightValue = (float)Lua.lua_tonumber(L, 2);
			UnitViewFacade.SetWaveIntensityEffect(handle, heightValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAll_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.ClearAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose_xlua_st_(IntPtr L)
	{
		try
		{
			UnitViewFacade.Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
