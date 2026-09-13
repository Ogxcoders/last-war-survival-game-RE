using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MeteoriteWorldEffectPlayerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MeteoriteWorldEffectPlayer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 1, 0);
		Utils.RegisterFunc(L, -3, "CreateMeteoriteDropPlayer", _m_CreateMeteoriteDropPlayer);
		Utils.RegisterFunc(L, -3, "RandomCreateMeteoriteSonPlayer", _m_RandomCreateMeteoriteSonPlayer);
		Utils.RegisterFunc(L, -3, "SetFragmentDensity", _m_SetFragmentDensity);
		Utils.RegisterFunc(L, -3, "CreateSmallFragmentPlayer", _m_CreateSmallFragmentPlayer);
		Utils.RegisterFunc(L, -3, "CreateMiddleFragmentPlayer", _m_CreateMiddleFragmentPlayer);
		Utils.RegisterFunc(L, -3, "CreateMeteoriteDaddyPlayer", _m_CreateMeteoriteDaddyPlayer);
		Utils.RegisterFunc(L, -3, "PlayLastKillNotice", _m_PlayLastKillNotice);
		Utils.RegisterFunc(L, -3, "UpdateInfo", _m_UpdateInfo);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "Description", _m_Description);
		Utils.RegisterFunc(L, -2, "CurrentState", _g_get_CurrentState);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
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
				MeteoriteWorldEffectPlayer o = new MeteoriteWorldEffectPlayer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MeteoriteWorldEffectPlayer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateMeteoriteDropPlayer(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer obj = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fromPoint = Lua.xlua_tointeger(L, 2);
			int toPoint = Lua.xlua_tointeger(L, 3);
			int infoOpenTime = Lua.xlua_tointeger(L, 4);
			obj.CreateMeteoriteDropPlayer(fromPoint, toPoint, infoOpenTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RandomCreateMeteoriteSonPlayer(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer obj = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			obj.RandomCreateMeteoriteSonPlayer(pointIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFragmentDensity(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer meteoriteWorldEffectPlayer = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int fragmentDensity = Lua.xlua_tointeger(L, 2);
				meteoriteWorldEffectPlayer.SetFragmentDensity(fragmentDensity);
				return 0;
			}
			if (num == 1)
			{
				meteoriteWorldEffectPlayer.SetFragmentDensity();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MeteoriteWorldEffectPlayer.SetFragmentDensity!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateSmallFragmentPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer meteoriteWorldEffectPlayer = (MeteoriteWorldEffectPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float height = (float)Lua.lua_tonumber(L, 3);
			float degree = (float)Lua.lua_tonumber(L, 4);
			float dropTimeSec = (float)Lua.lua_tonumber(L, 5);
			objectTranslator.Get(L, 6, out Vector3 val2);
			float rotateSpeed = (float)Lua.lua_tonumber(L, 7);
			meteoriteWorldEffectPlayer.CreateSmallFragmentPlayer(val, height, degree, dropTimeSec, val2, rotateSpeed);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateMiddleFragmentPlayer(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer obj = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			int openTime = Lua.xlua_tointeger(L, 3);
			obj.CreateMiddleFragmentPlayer(pointIndex, openTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateMeteoriteDaddyPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer meteoriteWorldEffectPlayer = (MeteoriteWorldEffectPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float height = (float)Lua.lua_tonumber(L, 3);
			float degree = (float)Lua.lua_tonumber(L, 4);
			float dropTimeSec = (float)Lua.lua_tonumber(L, 5);
			float exploreTimeSec = (float)Lua.lua_tonumber(L, 6);
			float scale = (float)Lua.lua_tonumber(L, 7);
			float rotateSpeed = (float)Lua.lua_tonumber(L, 8);
			long startTime = Lua.lua_toint64(L, 9);
			meteoriteWorldEffectPlayer.CreateMeteoriteDaddyPlayer(val, height, degree, dropTimeSec, exploreTimeSec, scale, rotateSpeed, startTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayLastKillNotice(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer obj = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			string uuid = Lua.lua_tostring(L, 3);
			string headPic = Lua.lua_tostring(L, 4);
			int picVer = Lua.xlua_tointeger(L, 5);
			string iconPic = Lua.lua_tostring(L, 6);
			int scoreAdd = Lua.xlua_tointeger(L, 7);
			string iconPic2 = Lua.lua_tostring(L, 8);
			int scoreAdd2 = Lua.xlua_tointeger(L, 9);
			obj.PlayLastKillNotice(pointIndex, uuid, headPic, picVer, iconPic, scoreAdd, iconPic2, scoreAdd2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateInfo(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer obj = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			int pointIndex = Lua.xlua_tointeger(L, 3);
			int hRadius = Lua.xlua_tointeger(L, 4);
			int lRadius = Lua.xlua_tointeger(L, 5);
			int startTime = Lua.xlua_tointeger(L, 6);
			int endTime = Lua.xlua_tointeger(L, 7);
			obj.UpdateInfo(serverId, pointIndex, hRadius, lRadius, startTime, endTime);
			return 0;
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
			((MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
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
			string str = ((MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Description();
			Lua.lua_pushstring(L, str);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, MeteoriteWorldEffectPlayer.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentState(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer meteoriteWorldEffectPlayer = (MeteoriteWorldEffectPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, meteoriteWorldEffectPlayer.CurrentState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
