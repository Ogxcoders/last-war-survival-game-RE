using System;
using MiniGame.Biubiu.Client;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientUIBiuBiuPvpBootWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIBiuBiuPvpBoot);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 0, 0);
		Utils.RegisterFunc(L, -3, "BindCallback", _m_BindCallback);
		Utils.RegisterFunc(L, -3, "UnBindCallback", _m_UnBindCallback);
		Utils.RegisterFunc(L, -3, "BindAdapt", _m_BindAdapt);
		Utils.RegisterFunc(L, -3, "StartGame", _m_StartGame);
		Utils.RegisterFunc(L, -3, "StartGameByPvpConnect", _m_StartGameByPvpConnect);
		Utils.RegisterFunc(L, -3, "EndGame", _m_EndGame);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "IsDone", _m_IsDone);
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
				UIBiuBiuPvpBoot o = new UIBiuBiuPvpBoot();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.UIBiuBiuPvpBoot constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BindCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuPvpBoot uIBiuBiuPvpBoot = (UIBiuBiuPvpBoot)objectTranslator.FastGetCSObj(L, 1);
			Action<IRender> handle = objectTranslator.GetDelegate<Action<IRender>>(L, 2);
			uIBiuBiuPvpBoot.BindCallback(handle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnBindCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuPvpBoot uIBiuBiuPvpBoot = (UIBiuBiuPvpBoot)objectTranslator.FastGetCSObj(L, 1);
			Action<IRender> handle = objectTranslator.GetDelegate<Action<IRender>>(L, 2);
			uIBiuBiuPvpBoot.UnBindCallback(handle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BindAdapt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuPvpBoot uIBiuBiuPvpBoot = (UIBiuBiuPvpBoot)objectTranslator.FastGetCSObj(L, 1);
			DataUIAdapt dataUIAdapt = (DataUIAdapt)objectTranslator.GetObject(L, 2, typeof(DataUIAdapt));
			uIBiuBiuPvpBoot.BindAdapt(dataUIAdapt);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartGame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuPvpBoot uIBiuBiuPvpBoot = (UIBiuBiuPvpBoot)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Transform>(L, 5) && objectTranslator.Assignable<Action<string>>(L, 6))
			{
				int serverId = Lua.xlua_tointeger(L, 2);
				string levelPath = Lua.lua_tostring(L, 3);
				LuaTable data = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Transform gameRoot = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
				Action<string> onError = objectTranslator.GetDelegate<Action<string>>(L, 6);
				uIBiuBiuPvpBoot.StartGame(serverId, levelPath, data, gameRoot, onError);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Transform>(L, 5))
			{
				int serverId2 = Lua.xlua_tointeger(L, 2);
				string levelPath2 = Lua.lua_tostring(L, 3);
				LuaTable data2 = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Transform gameRoot2 = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
				uIBiuBiuPvpBoot.StartGame(serverId2, levelPath2, data2, gameRoot2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.UIBiuBiuPvpBoot.StartGame!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartGameByPvpConnect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuPvpBoot uIBiuBiuPvpBoot = (UIBiuBiuPvpBoot)objectTranslator.FastGetCSObj(L, 1);
			UIBootPvpConnect pvpConnect = (UIBootPvpConnect)objectTranslator.GetObject(L, 2, typeof(UIBootPvpConnect));
			Transform gameRoot = (Transform)objectTranslator.GetObject(L, 3, typeof(Transform));
			uIBiuBiuPvpBoot.StartGameByPvpConnect(pvpConnect, gameRoot);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndGame(IntPtr L)
	{
		try
		{
			((UIBiuBiuPvpBoot)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndGame();
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
			((UIBiuBiuPvpBoot)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDone(IntPtr L)
	{
		try
		{
			bool value = ((UIBiuBiuPvpBoot)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDone();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
