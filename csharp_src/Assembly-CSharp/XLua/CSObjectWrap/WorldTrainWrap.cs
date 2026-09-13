using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldTrainWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldTrain);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 7, 7);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "cfgId", _g_get_cfgId);
		Utils.RegisterFunc(L, -2, "length", _g_get_length);
		Utils.RegisterFunc(L, -2, "type", _g_get_type);
		Utils.RegisterFunc(L, -2, "config", _g_get_config);
		Utils.RegisterFunc(L, -2, "trainData", _g_get_trainData);
		Utils.RegisterFunc(L, -2, "stationList", _g_get_stationList);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "cfgId", _s_set_cfgId);
		Utils.RegisterFunc(L, -1, "length", _s_set_length);
		Utils.RegisterFunc(L, -1, "type", _s_set_type);
		Utils.RegisterFunc(L, -1, "config", _s_set_config);
		Utils.RegisterFunc(L, -1, "trainData", _s_set_trainData);
		Utils.RegisterFunc(L, -1, "stationList", _s_set_stationList);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 1);
		Utils.RegisterFunc(L, -2, "Carriage_Length", _g_get_Carriage_Length);
		Utils.RegisterFunc(L, -1, "Carriage_Length", _s_set_Carriage_Length);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<ISFSObject>(L, 2))
			{
				WorldTrain o = new WorldTrain((ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTrain constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Carriage_Length(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldTrain.Carriage_Length);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			WorldTrain worldTrain = (WorldTrain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldTrain.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cfgId(IntPtr L)
	{
		try
		{
			WorldTrain worldTrain = (WorldTrain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTrain.cfgId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_length(IntPtr L)
	{
		try
		{
			WorldTrain worldTrain = (WorldTrain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTrain.length);
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
			WorldTrain worldTrain = (WorldTrain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTrain.type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_config(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTrain worldTrain = (WorldTrain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTrain.config);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_trainData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTrain worldTrain = (WorldTrain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, worldTrain.trainData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stationList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTrain worldTrain = (WorldTrain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTrain.stationList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Carriage_Length(IntPtr L)
	{
		try
		{
			WorldTrain.Carriage_Length = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((WorldTrain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cfgId(IntPtr L)
	{
		try
		{
			((WorldTrain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cfgId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_length(IntPtr L)
	{
		try
		{
			((WorldTrain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).length = (float)Lua.lua_tonumber(L, 2);
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
			WorldTrain worldTrain = (WorldTrain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TrainType v);
			worldTrain.type = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_config(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldTrain)objectTranslator.FastGetCSObj(L, 1)).config = (WorldTrainConfig)objectTranslator.GetObject(L, 2, typeof(WorldTrainConfig));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_trainData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldTrain)objectTranslator.FastGetCSObj(L, 1)).trainData = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stationList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldTrain)objectTranslator.FastGetCSObj(L, 1)).stationList = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
