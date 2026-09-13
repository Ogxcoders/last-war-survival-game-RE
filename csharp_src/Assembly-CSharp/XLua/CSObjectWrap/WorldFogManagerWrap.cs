using System;
using System.Collections.Generic;
using Main.Scripts.Scene.LightAndDark;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldFogManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldFogManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 20, 13, 13);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "GetCurrentLightSourcesDic", _m_GetCurrentLightSourcesDic);
		Utils.RegisterFunc(L, -3, "ClearAllRevealer", _m_ClearAllRevealer);
		Utils.RegisterFunc(L, -3, "lightSourceAndFogRangeIntersectType", _m_lightSourceAndFogRangeIntersectType);
		Utils.RegisterFunc(L, -3, "UninitWorldFog", _m_UninitWorldFog);
		Utils.RegisterFunc(L, -3, "OnChangeToDawn", _m_OnChangeToDawn);
		Utils.RegisterFunc(L, -3, "OnChangeToBloodyNight", _m_OnChangeToBloodyNight);
		Utils.RegisterFunc(L, -3, "OnChangeFogMat", _m_OnChangeFogMat);
		Utils.RegisterFunc(L, -3, "OnBloodyNightActivityRefresh", _m_OnBloodyNightActivityRefresh);
		Utils.RegisterFunc(L, -3, "IsInBloodyNight", _m_IsInBloodyNight);
		Utils.RegisterFunc(L, -3, "IsDawn", _m_IsDawn);
		Utils.RegisterFunc(L, -3, "InitWorldFog", _m_InitWorldFog);
		Utils.RegisterFunc(L, -3, "HideWorldFogObj", _m_HideWorldFogObj);
		Utils.RegisterFunc(L, -3, "NeedUpdateAllFog", _m_NeedUpdateAllFog);
		Utils.RegisterFunc(L, -3, "UpdateCurrentFogRect", _m_UpdateCurrentFogRect);
		Utils.RegisterFunc(L, -3, "UpdateAllFogOnViewChange", _m_UpdateAllFogOnViewChange);
		Utils.RegisterFunc(L, -3, "OnFogDataChangedAll", _m_OnFogDataChangedAll);
		Utils.RegisterFunc(L, -3, "TestClearFogInPos", _m_TestClearFogInPos);
		Utils.RegisterFunc(L, -2, "mUseMeshInstanced", _g_get_mUseMeshInstanced);
		Utils.RegisterFunc(L, -2, "mIsDebugMode", _g_get_mIsDebugMode);
		Utils.RegisterFunc(L, -2, "mIsBloodyNight", _g_get_mIsBloodyNight);
		Utils.RegisterFunc(L, -2, "mIsDawn", _g_get_mIsDawn);
		Utils.RegisterFunc(L, -2, "FogMaxRange", _g_get_FogMaxRange);
		Utils.RegisterFunc(L, -2, "FogUpdateRange", _g_get_FogUpdateRange);
		Utils.RegisterFunc(L, -2, "mCurrentFogRect", _g_get_mCurrentFogRect);
		Utils.RegisterFunc(L, -2, "mFowObj", _g_get_mFowObj);
		Utils.RegisterFunc(L, -2, "mFowSystem", _g_get_mFowSystem);
		Utils.RegisterFunc(L, -2, "mIsFogLoaded", _g_get_mIsFogLoaded);
		Utils.RegisterFunc(L, -2, "mFogMesh", _g_get_mFogMesh);
		Utils.RegisterFunc(L, -2, "mLightSourcesDic", _g_get_mLightSourcesDic);
		Utils.RegisterFunc(L, -2, "mLightInstanceDirty", _g_get_mLightInstanceDirty);
		Utils.RegisterFunc(L, -1, "mUseMeshInstanced", _s_set_mUseMeshInstanced);
		Utils.RegisterFunc(L, -1, "mIsDebugMode", _s_set_mIsDebugMode);
		Utils.RegisterFunc(L, -1, "mIsBloodyNight", _s_set_mIsBloodyNight);
		Utils.RegisterFunc(L, -1, "mIsDawn", _s_set_mIsDawn);
		Utils.RegisterFunc(L, -1, "FogMaxRange", _s_set_FogMaxRange);
		Utils.RegisterFunc(L, -1, "FogUpdateRange", _s_set_FogUpdateRange);
		Utils.RegisterFunc(L, -1, "mCurrentFogRect", _s_set_mCurrentFogRect);
		Utils.RegisterFunc(L, -1, "mFowObj", _s_set_mFowObj);
		Utils.RegisterFunc(L, -1, "mFowSystem", _s_set_mFowSystem);
		Utils.RegisterFunc(L, -1, "mIsFogLoaded", _s_set_mIsFogLoaded);
		Utils.RegisterFunc(L, -1, "mFogMesh", _s_set_mFogMesh);
		Utils.RegisterFunc(L, -1, "mLightSourcesDic", _s_set_mLightSourcesDic);
		Utils.RegisterFunc(L, -1, "mLightInstanceDirty", _s_set_mLightInstanceDirty);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldScene>(L, 2))
			{
				WorldFogManager o = new WorldFogManager((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldFogManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
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
			WorldFogManager obj = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetCurrentLightSourcesDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, LightDataManager.LightSourceData> currentLightSourcesDic = ((WorldFogManager)objectTranslator.FastGetCSObj(L, 1)).GetCurrentLightSourcesDic();
			objectTranslator.Push(L, currentLightSourcesDic);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllRevealer(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllRevealer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_lightSourceAndFogRangeIntersectType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out LightDataManager.LightSourceData v);
			int value = worldFogManager.lightSourceAndFogRangeIntersectType(val, v);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UninitWorldFog(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UninitWorldFog();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnChangeToDawn(IntPtr L)
	{
		try
		{
			WorldFogManager obj = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isDawn = Lua.lua_toboolean(L, 2);
			obj.OnChangeToDawn(isDawn);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnChangeToBloodyNight(IntPtr L)
	{
		try
		{
			WorldFogManager obj = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isBloodyNight = Lua.lua_toboolean(L, 2);
			obj.OnChangeToBloodyNight(isBloodyNight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnChangeFogMat(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnChangeFogMat();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBloodyNightActivityRefresh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			object serverId = objectTranslator.GetObject(L, 2, typeof(object));
			worldFogManager.OnBloodyNightActivityRefresh(serverId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInBloodyNight(IntPtr L)
	{
		try
		{
			WorldFogManager obj = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int targetServerId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsInBloodyNight(targetServerId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDawn(IntPtr L)
	{
		try
		{
			WorldFogManager obj = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int targetServerId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsDawn(targetServerId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitWorldFog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			WorldFogRendererRenderer fogRenderer = (WorldFogRendererRenderer)objectTranslator.GetObject(L, 2, typeof(WorldFogRendererRenderer));
			WorldFogInstanceRenderer fogInstanceRenderer = (WorldFogInstanceRenderer)objectTranslator.GetObject(L, 3, typeof(WorldFogInstanceRenderer));
			WorldCamera camera = (WorldCamera)objectTranslator.GetObject(L, 4, typeof(WorldCamera));
			worldFogManager.InitWorldFog(fogRenderer, fogInstanceRenderer, camera);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideWorldFogObj(IntPtr L)
	{
		try
		{
			WorldFogManager obj = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool hide = Lua.lua_toboolean(L, 2);
			obj.HideWorldFogObj(hide);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NeedUpdateAllFog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			WorldCamera camera = (WorldCamera)objectTranslator.GetObject(L, 2, typeof(WorldCamera));
			bool value = worldFogManager.NeedUpdateAllFog(camera);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateCurrentFogRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldFogManager.UpdateCurrentFogRect(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAllFogOnViewChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			WorldCamera camera = (WorldCamera)objectTranslator.GetObject(L, 2, typeof(WorldCamera));
			worldFogManager.UpdateAllFogOnViewChange(camera);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnFogDataChangedAll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			Dictionary<long, LightDataManager.LightSourceData> lightSourceDataCache = (Dictionary<long, LightDataManager.LightSourceData>)objectTranslator.GetObject(L, 2, typeof(Dictionary<long, LightDataManager.LightSourceData>));
			worldFogManager.OnFogDataChangedAll(lightSourceDataCache);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TestClearFogInPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldFogManager.TestClearFogInPos(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mUseMeshInstanced(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldFogManager.mUseMeshInstanced);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mIsDebugMode(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldFogManager.mIsDebugMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mIsBloodyNight(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldFogManager.mIsBloodyNight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mIsDawn(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldFogManager.mIsDawn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FogMaxRange(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldFogManager.FogMaxRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FogUpdateRange(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldFogManager.FogUpdateRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mCurrentFogRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldFogManager.mCurrentFogRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mFowObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldFogManager.mFowObj);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mFowSystem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldFogManager.mFowSystem);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mIsFogLoaded(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldFogManager.mIsFogLoaded);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mFogMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldFogManager.mFogMesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mLightSourcesDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldFogManager.mLightSourcesDic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mLightInstanceDirty(IntPtr L)
	{
		try
		{
			WorldFogManager worldFogManager = (WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldFogManager.mLightInstanceDirty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mUseMeshInstanced(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mUseMeshInstanced = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mIsDebugMode(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mIsDebugMode = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mIsBloodyNight(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mIsBloodyNight = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mIsDawn(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mIsDawn = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FogMaxRange(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FogMaxRange = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FogUpdateRange(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FogUpdateRange = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mCurrentFogRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldFogManager worldFogManager = (WorldFogManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			worldFogManager.mCurrentFogRect = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mFowObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldFogManager)objectTranslator.FastGetCSObj(L, 1)).mFowObj = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mFowSystem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldFogManager)objectTranslator.FastGetCSObj(L, 1)).mFowSystem = (FOWSystem)objectTranslator.GetObject(L, 2, typeof(FOWSystem));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mIsFogLoaded(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mIsFogLoaded = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mFogMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldFogManager)objectTranslator.FastGetCSObj(L, 1)).mFogMesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mLightSourcesDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldFogManager)objectTranslator.FastGetCSObj(L, 1)).mLightSourcesDic = (Dictionary<long, LightDataManager.LightSourceData>)objectTranslator.GetObject(L, 2, typeof(Dictionary<long, LightDataManager.LightSourceData>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mLightInstanceDirty(IntPtr L)
	{
		try
		{
			((WorldFogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mLightInstanceDirty = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
