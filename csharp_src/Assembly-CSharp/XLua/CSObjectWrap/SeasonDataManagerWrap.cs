using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SeasonDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SeasonDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 17, 1, 0);
		Utils.RegisterFunc(L, -3, "UpdateUISkin", _m_UpdateUISkin);
		Utils.RegisterFunc(L, -3, "SetData", _m_SetData);
		Utils.RegisterFunc(L, -3, "GetData", _m_GetData);
		Utils.RegisterFunc(L, -3, "InSeasonBigMapMode", _m_InSeasonBigMapMode);
		Utils.RegisterFunc(L, -3, "UpdateNinePalacesSkin", _m_UpdateNinePalacesSkin);
		Utils.RegisterFunc(L, -3, "GetWorldSkinByIndex", _m_GetWorldSkinByIndex);
		Utils.RegisterFunc(L, -3, "GetWorldSkinByServerId", _m_GetWorldSkinByServerId);
		Utils.RegisterFunc(L, -3, "UpdateNinePalacesData", _m_UpdateNinePalacesData);
		Utils.RegisterFunc(L, -3, "GetNinePalacesServer", _m_GetNinePalacesServer);
		Utils.RegisterFunc(L, -3, "InNinePalacesList", _m_InNinePalacesList);
		Utils.RegisterFunc(L, -3, "GetNinePalacesIndex", _m_GetNinePalacesIndex);
		Utils.RegisterFunc(L, -3, "CleanNinePalacesData", _m_CleanNinePalacesData);
		Utils.RegisterFunc(L, -3, "NormalizedWorldPos", _m_NormalizedWorldPos);
		Utils.RegisterFunc(L, -3, "GetServerIdFromWorldPos", _m_GetServerIdFromWorldPos);
		Utils.RegisterFunc(L, -3, "GetNinePalacesIndexFromWorldPos", _m_GetNinePalacesIndexFromWorldPos);
		Utils.RegisterFunc(L, -3, "GetWorldBasePos", _m_GetWorldBasePos);
		Utils.RegisterFunc(L, -3, "GetWorldBasePosByIndex", _m_GetWorldBasePosByIndex);
		Utils.RegisterFunc(L, -2, "SkinSetting", _g_get_SkinSetting);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 1, 0);
		Utils.RegisterFunc(L, -4, "Purge", _m_Purge_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckCrossMarchType", _m_CheckCrossMarchType_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckCrossMarchTargetType", _m_CheckCrossMarchTargetType_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				SeasonDataManager o = new SeasonDataManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SeasonDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Purge_xlua_st_(IntPtr L)
	{
		try
		{
			SeasonDataManager.Purge();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateUISkin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			string bgPath = Lua.lua_tostring(L, 3);
			string titlePath = Lua.lua_tostring(L, 4);
			string backPath = Lua.lua_tostring(L, 5);
			seasonDataManager.UpdateUISkin(val, bgPath, titlePath, backPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetData(IntPtr L)
	{
		try
		{
			SeasonDataManager obj = (SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string k = Lua.lua_tostring(L, 2);
			string k2 = Lua.lua_tostring(L, 3);
			string data = Lua.lua_tostring(L, 4);
			obj.SetData(k, k2, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetData(IntPtr L)
	{
		try
		{
			SeasonDataManager obj = (SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string k = Lua.lua_tostring(L, 2);
			string k2 = Lua.lua_tostring(L, 3);
			string defaultData = Lua.lua_tostring(L, 4);
			string data = obj.GetData(k, k2, defaultData);
			Lua.lua_pushstring(L, data);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckCrossMarchType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out NewMarchType val);
			bool globalArmy = Lua.lua_toboolean(L, 2);
			NewMarchType val2 = SeasonDataManager.CheckCrossMarchType(val, ref globalArmy);
			objectTranslator.PushNewMarchType(L, val2);
			Lua.lua_pushboolean(L, globalArmy);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckCrossMarchTargetType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out MarchTargetType v);
			bool globalArmy = Lua.lua_toboolean(L, 2);
			MarchTargetType marchTargetType = SeasonDataManager.CheckCrossMarchTargetType(v, ref globalArmy);
			objectTranslator.Push(L, marchTargetType);
			Lua.lua_pushboolean(L, globalArmy);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InSeasonBigMapMode(IntPtr L)
	{
		try
		{
			bool value = ((SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InSeasonBigMapMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateNinePalacesSkin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			int mapIndex = Lua.xlua_tointeger(L, 3);
			SceneSkinMeta meta = (SceneSkinMeta)objectTranslator.GetObject(L, 4, typeof(SceneSkinMeta));
			seasonDataManager.UpdateNinePalacesSkin(serverId, mapIndex, meta);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldSkinByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager obj = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			int mapIndex = Lua.xlua_tointeger(L, 2);
			SceneSkinMeta worldSkinByIndex = obj.GetWorldSkinByIndex(mapIndex);
			objectTranslator.Push(L, worldSkinByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldSkinByServerId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager obj = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			SceneSkinMeta worldSkinByServerId = obj.GetWorldSkinByServerId(serverId);
			objectTranslator.Push(L, worldSkinByServerId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateNinePalacesData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			LuaTable info = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			seasonDataManager.UpdateNinePalacesData(info);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNinePalacesServer(IntPtr L)
	{
		try
		{
			SeasonDataManager obj = (SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int ninePalacesServer = obj.GetNinePalacesServer(index);
			Lua.xlua_pushinteger(L, ninePalacesServer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InNinePalacesList(IntPtr L)
	{
		try
		{
			SeasonDataManager obj = (SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			bool value = obj.InNinePalacesList(serverId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNinePalacesIndex(IntPtr L)
	{
		try
		{
			SeasonDataManager obj = (SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			int ninePalacesIndex = obj.GetNinePalacesIndex(serverId);
			Lua.xlua_pushinteger(L, ninePalacesIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CleanNinePalacesData(IntPtr L)
	{
		try
		{
			((SeasonDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CleanNinePalacesData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NormalizedWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = seasonDataManager.NormalizedWorldPos(serverId: Lua.xlua_tointeger(L, 3), realWorldPos: val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerIdFromWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			int serverIdFromWorldPos = seasonDataManager.GetServerIdFromWorldPos(val);
			Lua.xlua_pushinteger(L, serverIdFromWorldPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNinePalacesIndexFromWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			int ninePalacesIndexFromWorldPos = seasonDataManager.GetNinePalacesIndexFromWorldPos(val);
			Lua.xlua_pushinteger(L, ninePalacesIndexFromWorldPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldBasePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int serverId = Lua.xlua_tointeger(L, 2);
				Vector3 worldBasePos = seasonDataManager.GetWorldBasePos(serverId);
				objectTranslator.PushUnityEngineVector3(L, worldBasePos);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int tileX = Lua.xlua_tointeger(L, 2);
				int tileY = Lua.xlua_tointeger(L, 3);
				Vector3 worldBasePos2 = seasonDataManager.GetWorldBasePos(tileX, tileY);
				objectTranslator.PushUnityEngineVector3(L, worldBasePos2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SeasonDataManager.GetWorldBasePos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldBasePosByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager obj = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			int mapIndex = Lua.xlua_tointeger(L, 2);
			Vector3 worldBasePosByIndex = obj.GetWorldBasePosByIndex(mapIndex);
			objectTranslator.PushUnityEngineVector3(L, worldBasePosByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SeasonDataManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkinSetting(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonDataManager seasonDataManager = (SeasonDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, seasonDataManager.SkinSetting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
