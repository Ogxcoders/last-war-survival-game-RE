using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class HeatSourceDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(HeatSourceDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 23, 2, 2);
		Utils.RegisterFunc(L, -3, "SetOnHeatSourceChanged", _m_SetOnHeatSourceChanged);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "OnEnterGame", _m_OnEnterGame);
		Utils.RegisterFunc(L, -3, "HandleHotSpotBaseInfo", _m_HandleHotSpotBaseInfo);
		Utils.RegisterFunc(L, -3, "HandleWorldGetBlock", _m_HandleWorldGetBlock);
		Utils.RegisterFunc(L, -3, "HandleHotSpotMayEffectMe", _m_HandleHotSpotMayEffectMe);
		Utils.RegisterFunc(L, -3, "HandlePushHotSpotPatch", _m_HandlePushHotSpotPatch);
		Utils.RegisterFunc(L, -3, "HandlePushConstHotSpotDel", _m_HandlePushConstHotSpotDel);
		Utils.RegisterFunc(L, -3, "CreateConstHeatSource", _m_CreateConstHeatSource);
		Utils.RegisterFunc(L, -3, "RemoveConstHeatSource", _m_RemoveConstHeatSource);
		Utils.RegisterFunc(L, -3, "GetTemperatureByIndex", _m_GetTemperatureByIndex);
		Utils.RegisterFunc(L, -3, "GetTemperatureByXY", _m_GetTemperatureByXY);
		Utils.RegisterFunc(L, -3, "GetCityHeatSourceTemperature", _m_GetCityHeatSourceTemperature);
		Utils.RegisterFunc(L, -3, "GetThermalConductorInfo", _m_GetThermalConductorInfo);
		Utils.RegisterFunc(L, -3, "HandleThermalConductorInfo", _m_HandleThermalConductorInfo);
		Utils.RegisterFunc(L, -3, "HandlePushThermalConductorInfo", _m_HandlePushThermalConductorInfo);
		Utils.RegisterFunc(L, -3, "RefreshMyBaseThermalConductor", _m_RefreshMyBaseThermalConductor);
		Utils.RegisterFunc(L, -3, "GetMyTileTemperature", _m_GetMyTileTemperature);
		Utils.RegisterFunc(L, -3, "GetHeatSourceAffectMe", _m_GetHeatSourceAffectMe);
		Utils.RegisterFunc(L, -3, "IsMyBaseFrozen", _m_IsMyBaseFrozen);
		Utils.RegisterFunc(L, -3, "GetMyBaseTemperature", _m_GetMyBaseTemperature);
		Utils.RegisterFunc(L, -3, "GetMyBaseConductor", _m_GetMyBaseConductor);
		Utils.RegisterFunc(L, -3, "GetTerrainStateByXY", _m_GetTerrainStateByXY);
		Utils.RegisterFunc(L, -2, "WorldCalSwitch", _g_get_WorldCalSwitch);
		Utils.RegisterFunc(L, -2, "CurWorldIsSnowSeason", _g_get_CurWorldIsSnowSeason);
		Utils.RegisterFunc(L, -1, "WorldCalSwitch", _s_set_WorldCalSwitch);
		Utils.RegisterFunc(L, -1, "CurWorldIsSnowSeason", _s_set_CurWorldIsSnowSeason);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "GetInstance", _m_GetInstance_xlua_st_);
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
				HeatSourceDataManager o = new HeatSourceDataManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to HeatSourceDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnHeatSourceChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			HeatSourceDataManager.OnHeatSourceChanged onHeatSourceChanged = objectTranslator.GetDelegate<HeatSourceDataManager.OnHeatSourceChanged>(L, 2);
			heatSourceDataManager.SetOnHeatSourceChanged(onHeatSourceChanged);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInstance_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager instance = HeatSourceDataManager.GetInstance();
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
			((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
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
			((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEnterGame();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleHotSpotBaseInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.HandleHotSpotBaseInfo(msg);
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
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			int lb = Lua.xlua_tointeger(L, 3);
			int rt = Lua.xlua_tointeger(L, 4);
			heatSourceDataManager.HandleWorldGetBlock(msg, lb, rt);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleHotSpotMayEffectMe(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.HandleHotSpotMayEffectMe(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushHotSpotPatch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.HandlePushHotSpotPatch(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushConstHotSpotDel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.HandlePushConstHotSpotDel(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateConstHeatSource(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cfgId = Lua.xlua_tointeger(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			float temperature = (float)Lua.lua_tonumber(L, 4);
			obj.CreateConstHeatSource(cfgId, type, temperature);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveConstHeatSource(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cfgId = Lua.xlua_tointeger(L, 2);
			obj.RemoveConstHeatSource(cfgId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTemperatureByIndex(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			float temperatureByIndex = obj.GetTemperatureByIndex(pointIndex);
			Lua.lua_pushnumber(L, temperatureByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTemperatureByXY(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int x = Lua.xlua_tointeger(L, 2);
			int y = Lua.xlua_tointeger(L, 3);
			float temperatureByXY = obj.GetTemperatureByXY(x, y);
			Lua.lua_pushnumber(L, temperatureByXY);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCityHeatSourceTemperature(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cityId = Lua.xlua_tointeger(L, 2);
			float cityHeatSourceTemperature = obj.GetCityHeatSourceTemperature(cityId);
			Lua.lua_pushnumber(L, cityHeatSourceTemperature);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetThermalConductorInfo(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uuid = Lua.lua_tostring(L, 2);
			obj.GetThermalConductorInfo(uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleThermalConductorInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.HandleThermalConductorInfo(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushThermalConductorInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.HandlePushThermalConductorInfo(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshMyBaseThermalConductor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			heatSourceDataManager.RefreshMyBaseThermalConductor(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyTileTemperature(IntPtr L)
	{
		try
		{
			float myTileTemperature = ((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMyTileTemperature();
			Lua.lua_pushnumber(L, myTileTemperature);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHeatSourceAffectMe(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<HeatSourceBase> heatSourceAffectMe = ((HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1)).GetHeatSourceAffectMe();
			objectTranslator.Push(L, heatSourceAffectMe);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMyBaseFrozen(IntPtr L)
	{
		try
		{
			bool value = ((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMyBaseFrozen();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyBaseTemperature(IntPtr L)
	{
		try
		{
			float myBaseTemperature = ((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMyBaseTemperature();
			Lua.lua_pushnumber(L, myBaseTemperature);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyBaseConductor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ThermalConductor myBaseConductor = ((HeatSourceDataManager)objectTranslator.FastGetCSObj(L, 1)).GetMyBaseConductor();
			objectTranslator.Push(L, myBaseConductor);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTerrainStateByXY(IntPtr L)
	{
		try
		{
			HeatSourceDataManager obj = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int x = Lua.xlua_tointeger(L, 2);
			int y = Lua.xlua_tointeger(L, 3);
			float terrainStateByXY = obj.GetTerrainStateByXY(x, y);
			Lua.lua_pushnumber(L, terrainStateByXY);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldCalSwitch(IntPtr L)
	{
		try
		{
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, heatSourceDataManager.WorldCalSwitch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurWorldIsSnowSeason(IntPtr L)
	{
		try
		{
			HeatSourceDataManager heatSourceDataManager = (HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, heatSourceDataManager.CurWorldIsSnowSeason);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_WorldCalSwitch(IntPtr L)
	{
		try
		{
			((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WorldCalSwitch = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CurWorldIsSnowSeason(IntPtr L)
	{
		try
		{
			((HeatSourceDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CurWorldIsSnowSeason = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
