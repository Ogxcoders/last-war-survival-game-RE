using System;
using MiniGame.Biubiu.Client;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientUIBootPvpConnectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIBootPvpConnect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 2, 2);
		Utils.RegisterFunc(L, -3, "ConnectGameLift", _m_ConnectGameLift);
		Utils.RegisterFunc(L, -3, "ReEnter", _m_ReEnter);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -2, "ServerID", _g_get_ServerID);
		Utils.RegisterFunc(L, -2, "PvpPlayerRuntime", _g_get_PvpPlayerRuntime);
		Utils.RegisterFunc(L, -1, "ServerID", _s_set_ServerID);
		Utils.RegisterFunc(L, -1, "PvpPlayerRuntime", _s_set_PvpPlayerRuntime);
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
			if (Lua.lua_gettop(L) == 1)
			{
				UIBootPvpConnect o = new UIBootPvpConnect();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.UIBootPvpConnect constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConnectGameLift(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBootPvpConnect uIBootPvpConnect = (UIBootPvpConnect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TTABLE) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<string>>(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				int serverId = Lua.xlua_tointeger(L, 2);
				LuaTable data = (LuaTable)objectTranslator.GetObject(L, 3, typeof(LuaTable));
				int maxPlayers = Lua.xlua_tointeger(L, 4);
				string levelPath = Lua.lua_tostring(L, 5);
				Action<string> onError = objectTranslator.GetDelegate<Action<string>>(L, 6);
				Action onFinish = objectTranslator.GetDelegate<Action>(L, 7);
				uIBootPvpConnect.ConnectGameLift(serverId, data, maxPlayers, levelPath, onError, onFinish);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TTABLE) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<string>>(L, 6))
			{
				int serverId2 = Lua.xlua_tointeger(L, 2);
				LuaTable data2 = (LuaTable)objectTranslator.GetObject(L, 3, typeof(LuaTable));
				int maxPlayers2 = Lua.xlua_tointeger(L, 4);
				string levelPath2 = Lua.lua_tostring(L, 5);
				Action<string> onError2 = objectTranslator.GetDelegate<Action<string>>(L, 6);
				uIBootPvpConnect.ConnectGameLift(serverId2, data2, maxPlayers2, levelPath2, onError2);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TTABLE) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING))
			{
				int serverId3 = Lua.xlua_tointeger(L, 2);
				LuaTable data3 = (LuaTable)objectTranslator.GetObject(L, 3, typeof(LuaTable));
				int maxPlayers3 = Lua.xlua_tointeger(L, 4);
				string levelPath3 = Lua.lua_tostring(L, 5);
				uIBootPvpConnect.ConnectGameLift(serverId3, data3, maxPlayers3, levelPath3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.UIBootPvpConnect.ConnectGameLift!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReEnter(IntPtr L)
	{
		try
		{
			int value = ((UIBootPvpConnect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReEnter();
			Lua.xlua_pushinteger(L, value);
			return 1;
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
			((UIBootPvpConnect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ServerID(IntPtr L)
	{
		try
		{
			UIBootPvpConnect uIBootPvpConnect = (UIBootPvpConnect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, uIBootPvpConnect.ServerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PvpPlayerRuntime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBootPvpConnect uIBootPvpConnect = (UIBootPvpConnect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIBootPvpConnect.PvpPlayerRuntime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ServerID(IntPtr L)
	{
		try
		{
			((UIBootPvpConnect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ServerID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PvpPlayerRuntime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIBootPvpConnect)objectTranslator.FastGetCSObj(L, 1)).PvpPlayerRuntime = (PvpPlayerRuntime)objectTranslator.GetObject(L, 2, typeof(PvpPlayerRuntime));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
