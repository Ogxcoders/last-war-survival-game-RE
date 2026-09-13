using System;
using System.Collections.Generic;
using Protobuf;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LightDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LightDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 20, 15, 15);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "OnEnterGame", _m_OnEnterGame);
		Utils.RegisterFunc(L, -3, "IsDawn", _m_IsDawn);
		Utils.RegisterFunc(L, -3, "OnBloodyNightActivityRefresh", _m_OnBloodyNightActivityRefresh);
		Utils.RegisterFunc(L, -3, "GetS4LayerStateValue", _m_GetS4LayerStateValue);
		Utils.RegisterFunc(L, -3, "ClearAllLightData", _m_ClearAllLightData);
		Utils.RegisterFunc(L, -3, "UpdateAllLightDataBrushSize", _m_UpdateAllLightDataBrushSize);
		Utils.RegisterFunc(L, -3, "TryAddLightMarch", _m_TryAddLightMarch);
		Utils.RegisterFunc(L, -3, "TryRemoveLightMarch", _m_TryRemoveLightMarch);
		Utils.RegisterFunc(L, -3, "AddDiscoLight", _m_AddDiscoLight);
		Utils.RegisterFunc(L, -3, "RemoveDiscoLight", _m_RemoveDiscoLight);
		Utils.RegisterFunc(L, -3, "HandleLightDataChange", _m_HandleLightDataChange);
		Utils.RegisterFunc(L, -3, "HandleWorldGetBlock", _m_HandleWorldGetBlock);
		Utils.RegisterFunc(L, -3, "UpdateTerrain", _m_UpdateTerrain);
		Utils.RegisterFunc(L, -3, "GetOnLighdataChange", _m_GetOnLighdataChange);
		Utils.RegisterFunc(L, -3, "GetMaxLightLevelInPointId", _m_GetMaxLightLevelInPointId);
		Utils.RegisterFunc(L, -3, "IsLightUpInPointId", _m_IsLightUpInPointId);
		Utils.RegisterFunc(L, -3, "GetEvColorInDarknessSeason", _m_GetEvColorInDarknessSeason);
		Utils.RegisterFunc(L, -3, "GetCurrentEvColorInDarknessSeason", _m_GetCurrentEvColorInDarknessSeason);
		Utils.RegisterFunc(L, -3, "GetCurrentFogColorInDarknessSeason", _m_GetCurrentFogColorInDarknessSeason);
		Utils.RegisterFunc(L, -2, "mMarchSize", _g_get_mMarchSize);
		Utils.RegisterFunc(L, -2, "mMarchBrushSize", _g_get_mMarchBrushSize);
		Utils.RegisterFunc(L, -2, "mMarchBrushSize_Bloody", _g_get_mMarchBrushSize_Bloody);
		Utils.RegisterFunc(L, -2, "mMarchSizeStation", _g_get_mMarchSizeStation);
		Utils.RegisterFunc(L, -2, "mFogBrushSize", _g_get_mFogBrushSize);
		Utils.RegisterFunc(L, -2, "mFogBrushSize_Bloody", _g_get_mFogBrushSize_Bloody);
		Utils.RegisterFunc(L, -2, "mMonsterMarchBrushSizeDic", _g_get_mMonsterMarchBrushSizeDic);
		Utils.RegisterFunc(L, -2, "mLightSourceDataCache", _g_get_mLightSourceDataCache);
		Utils.RegisterFunc(L, -2, "mLightMarchDict", _g_get_mLightMarchDict);
		Utils.RegisterFunc(L, -2, "mDiscoLightDataCache", _g_get_mDiscoLightDataCache);
		Utils.RegisterFunc(L, -2, "mLightInstanceDirty", _g_get_mLightInstanceDirty);
		Utils.RegisterFunc(L, -2, "mMarchLightInstanceDirty", _g_get_mMarchLightInstanceDirty);
		Utils.RegisterFunc(L, -2, "mDiscoLightDataDirty", _g_get_mDiscoLightDataDirty);
		Utils.RegisterFunc(L, -2, "mShowMarchLight", _g_get_mShowMarchLight);
		Utils.RegisterFunc(L, -2, "_onLighdataChanged", _g_get__onLighdataChanged);
		Utils.RegisterFunc(L, -1, "mMarchSize", _s_set_mMarchSize);
		Utils.RegisterFunc(L, -1, "mMarchBrushSize", _s_set_mMarchBrushSize);
		Utils.RegisterFunc(L, -1, "mMarchBrushSize_Bloody", _s_set_mMarchBrushSize_Bloody);
		Utils.RegisterFunc(L, -1, "mMarchSizeStation", _s_set_mMarchSizeStation);
		Utils.RegisterFunc(L, -1, "mFogBrushSize", _s_set_mFogBrushSize);
		Utils.RegisterFunc(L, -1, "mFogBrushSize_Bloody", _s_set_mFogBrushSize_Bloody);
		Utils.RegisterFunc(L, -1, "mMonsterMarchBrushSizeDic", _s_set_mMonsterMarchBrushSizeDic);
		Utils.RegisterFunc(L, -1, "mLightSourceDataCache", _s_set_mLightSourceDataCache);
		Utils.RegisterFunc(L, -1, "mLightMarchDict", _s_set_mLightMarchDict);
		Utils.RegisterFunc(L, -1, "mDiscoLightDataCache", _s_set_mDiscoLightDataCache);
		Utils.RegisterFunc(L, -1, "mLightInstanceDirty", _s_set_mLightInstanceDirty);
		Utils.RegisterFunc(L, -1, "mMarchLightInstanceDirty", _s_set_mMarchLightInstanceDirty);
		Utils.RegisterFunc(L, -1, "mDiscoLightDataDirty", _s_set_mDiscoLightDataDirty);
		Utils.RegisterFunc(L, -1, "mShowMarchLight", _s_set_mShowMarchLight);
		Utils.RegisterFunc(L, -1, "_onLighdataChanged", _s_set__onLighdataChanged);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 6, 0, 0);
		Utils.RegisterFunc(L, -4, "GetInstance", _m_GetInstance_xlua_st_);
		Utils.RegisterFunc(L, -4, "CloseEvColorInDarknessSeason", _m_CloseEvColorInDarknessSeason_xlua_st_);
		Utils.RegisterFunc(L, -4, "StringToColor", _m_StringToColor_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "mEvColorOnId", LightDataManager.mEvColorOnId);
		Utils.RegisterObject(L, translator, -4, "mEvColorId", LightDataManager.mEvColorId);
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
				LightDataManager o = new LightDataManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LightDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInstance_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager instance = LightDataManager.GetInstance();
			objectTranslator.Push(L, instance);
			return 1;
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
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEnterGame(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEnterGame();
			return 0;
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
			LightDataManager obj = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_OnBloodyNightActivityRefresh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			object serverId = objectTranslator.GetObject(L, 2, typeof(object));
			lightDataManager.OnBloodyNightActivityRefresh(serverId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetS4LayerStateValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager obj = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			int x = Lua.xlua_tointeger(L, 2);
			int y = Lua.xlua_tointeger(L, 3);
			QuadCellS3 s4LayerStateValue = obj.GetS4LayerStateValue(x, y);
			objectTranslator.Push(L, s4LayerStateValue);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllLightData(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllLightData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAllLightDataBrushSize(IntPtr L)
	{
		try
		{
			LightDataManager obj = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isBloodyNight = Lua.lua_toboolean(L, 2);
			obj.UpdateAllLightDataBrushSize(isBloodyNight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryAddLightMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			WorldScene world = (WorldScene)objectTranslator.GetObject(L, 3, typeof(WorldScene));
			lightDataManager.TryAddLightMarch(march, world);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryRemoveLightMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			lightDataManager.TryRemoveLightMarch(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddDiscoLight(IntPtr L)
	{
		try
		{
			LightDataManager obj = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			obj.AddDiscoLight(pointId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveDiscoLight(IntPtr L)
	{
		try
		{
			LightDataManager obj = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			obj.RemoveDiscoLight(pointId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleLightDataChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			PushLightChange data = (PushLightChange)objectTranslator.GetObject(L, 2, typeof(PushLightChange));
			WorldScene world = (WorldScene)objectTranslator.GetObject(L, 3, typeof(WorldScene));
			lightDataManager.HandleLightDataChange(data, world);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleWorldGetBlock(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			RangeLightData msg = (RangeLightData)objectTranslator.GetObject(L, 2, typeof(RangeLightData));
			WorldScene world = (WorldScene)objectTranslator.GetObject(L, 3, typeof(WorldScene));
			lightDataManager.HandleWorldGetBlock(msg, world);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTerrain(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int tileWidth = Lua.xlua_tointeger(L, 3);
			int tileHeight = Lua.xlua_tointeger(L, 4);
			lightDataManager.UpdateTerrain(v, tileWidth, tileHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOnLighdataChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			LightDataManager.OnLighdataChangeFun func = objectTranslator.GetDelegate<LightDataManager.OnLighdataChangeFun>(L, 2);
			lightDataManager.GetOnLighdataChange(func);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMaxLightLevelInPointId(IntPtr L)
	{
		try
		{
			LightDataManager obj = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			int maxLightLevelInPointId = obj.GetMaxLightLevelInPointId(pointId);
			Lua.xlua_pushinteger(L, maxLightLevelInPointId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLightUpInPointId(IntPtr L)
	{
		try
		{
			LightDataManager obj = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsLightUpInPointId(pointId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CloseEvColorInDarknessSeason_xlua_st_(IntPtr L)
	{
		try
		{
			LightDataManager.CloseEvColorInDarknessSeason();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEvColorInDarknessSeason(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager obj = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			int lightLevel = Lua.xlua_tointeger(L, 2);
			bool isBloody = Lua.lua_toboolean(L, 3);
			Color evColorInDarknessSeason = obj.GetEvColorInDarknessSeason(lightLevel, isBloody);
			objectTranslator.PushUnityEngineColor(L, evColorInDarknessSeason);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentEvColorInDarknessSeason(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Color currentEvColorInDarknessSeason = ((LightDataManager)objectTranslator.FastGetCSObj(L, 1)).GetCurrentEvColorInDarknessSeason();
			objectTranslator.PushUnityEngineColor(L, currentEvColorInDarknessSeason);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentFogColorInDarknessSeason(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Color currentFogColorInDarknessSeason = ((LightDataManager)objectTranslator.FastGetCSObj(L, 1)).GetCurrentFogColorInDarknessSeason();
			objectTranslator.PushUnityEngineColor(L, currentFogColorInDarknessSeason);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringToColor_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Color val = LightDataManager.StringToColor(Lua.lua_tostring(L, 1));
			objectTranslator.PushUnityEngineColor(L, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mMarchSize(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lightDataManager.mMarchSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mMarchBrushSize(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lightDataManager.mMarchBrushSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mMarchBrushSize_Bloody(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lightDataManager.mMarchBrushSize_Bloody);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mMarchSizeStation(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lightDataManager.mMarchSizeStation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mFogBrushSize(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lightDataManager.mFogBrushSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mFogBrushSize_Bloody(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lightDataManager.mFogBrushSize_Bloody);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mMonsterMarchBrushSizeDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lightDataManager.mMonsterMarchBrushSizeDic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mLightSourceDataCache(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lightDataManager.mLightSourceDataCache);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mLightMarchDict(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lightDataManager.mLightMarchDict);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mDiscoLightDataCache(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lightDataManager.mDiscoLightDataCache);
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
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lightDataManager.mLightInstanceDirty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mMarchLightInstanceDirty(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lightDataManager.mMarchLightInstanceDirty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mDiscoLightDataDirty(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lightDataManager.mDiscoLightDataDirty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mShowMarchLight(IntPtr L)
	{
		try
		{
			LightDataManager lightDataManager = (LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lightDataManager.mShowMarchLight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__onLighdataChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LightDataManager lightDataManager = (LightDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lightDataManager._onLighdataChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mMarchSize(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mMarchSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mMarchBrushSize(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mMarchBrushSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mMarchBrushSize_Bloody(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mMarchBrushSize_Bloody = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mMarchSizeStation(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mMarchSizeStation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mFogBrushSize(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mFogBrushSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mFogBrushSize_Bloody(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mFogBrushSize_Bloody = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mMonsterMarchBrushSizeDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LightDataManager)objectTranslator.FastGetCSObj(L, 1)).mMonsterMarchBrushSizeDic = (Dictionary<int, float>)objectTranslator.GetObject(L, 2, typeof(Dictionary<int, float>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mLightSourceDataCache(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LightDataManager)objectTranslator.FastGetCSObj(L, 1)).mLightSourceDataCache = (Dictionary<long, LightDataManager.LightSourceData>)objectTranslator.GetObject(L, 2, typeof(Dictionary<long, LightDataManager.LightSourceData>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mLightMarchDict(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LightDataManager)objectTranslator.FastGetCSObj(L, 1)).mLightMarchDict = (Dictionary<long, LightDataManager.LightSourceData>)objectTranslator.GetObject(L, 2, typeof(Dictionary<long, LightDataManager.LightSourceData>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mDiscoLightDataCache(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LightDataManager)objectTranslator.FastGetCSObj(L, 1)).mDiscoLightDataCache = (Dictionary<long, LightDataManager.LightSourceData>)objectTranslator.GetObject(L, 2, typeof(Dictionary<long, LightDataManager.LightSourceData>));
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
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mLightInstanceDirty = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mMarchLightInstanceDirty(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mMarchLightInstanceDirty = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mDiscoLightDataDirty(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mDiscoLightDataDirty = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mShowMarchLight(IntPtr L)
	{
		try
		{
			((LightDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mShowMarchLight = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__onLighdataChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LightDataManager)objectTranslator.FastGetCSObj(L, 1))._onLighdataChanged = objectTranslator.GetDelegate<LightDataManager.OnLighdataChangeFun>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
