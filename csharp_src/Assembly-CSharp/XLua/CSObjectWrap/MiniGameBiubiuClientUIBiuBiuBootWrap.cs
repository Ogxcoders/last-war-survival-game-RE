using System;
using MiniGame.Biubiu.Client;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientUIBiuBiuBootWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIBiuBiuBoot);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 1, 1);
		Utils.RegisterFunc(L, -3, "BindCallback", _m_BindCallback);
		Utils.RegisterFunc(L, -3, "UnBindCallback", _m_UnBindCallback);
		Utils.RegisterFunc(L, -3, "BindAdapt", _m_BindAdapt);
		Utils.RegisterFunc(L, -3, "StartGame", _m_StartGame);
		Utils.RegisterFunc(L, -3, "EndGame", _m_EndGame);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "IsDone", _m_IsDone);
		Utils.RegisterFunc(L, -2, "LoaderEnv", _g_get_LoaderEnv);
		Utils.RegisterFunc(L, -1, "LoaderEnv", _s_set_LoaderEnv);
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
				UIBiuBiuBoot o = new UIBiuBiuBoot();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.UIBiuBiuBoot constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BindCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuBoot uIBiuBiuBoot = (UIBiuBiuBoot)objectTranslator.FastGetCSObj(L, 1);
			Action<IRender> handle = objectTranslator.GetDelegate<Action<IRender>>(L, 2);
			uIBiuBiuBoot.BindCallback(handle);
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
			UIBiuBiuBoot uIBiuBiuBoot = (UIBiuBiuBoot)objectTranslator.FastGetCSObj(L, 1);
			Action<IRender> handle = objectTranslator.GetDelegate<Action<IRender>>(L, 2);
			uIBiuBiuBoot.UnBindCallback(handle);
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
			UIBiuBiuBoot uIBiuBiuBoot = (UIBiuBiuBoot)objectTranslator.FastGetCSObj(L, 1);
			DataUIAdapt dataUIAdapt = (DataUIAdapt)objectTranslator.GetObject(L, 2, typeof(DataUIAdapt));
			uIBiuBiuBoot.BindAdapt(dataUIAdapt);
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
			UIBiuBiuBoot uIBiuBiuBoot = (UIBiuBiuBoot)objectTranslator.FastGetCSObj(L, 1);
			string levelPath = Lua.lua_tostring(L, 2);
			Transform gameRoot = (Transform)objectTranslator.GetObject(L, 3, typeof(Transform));
			uIBiuBiuBoot.StartGame(levelPath, gameRoot);
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
			((UIBiuBiuBoot)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndGame();
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
			((UIBiuBiuBoot)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
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
			bool value = ((UIBiuBiuBoot)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDone();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LoaderEnv(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIBiuBiuBoot uIBiuBiuBoot = (UIBiuBiuBoot)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIBiuBiuBoot.LoaderEnv);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LoaderEnv(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIBiuBiuBoot)objectTranslator.FastGetCSObj(L, 1)).LoaderEnv = (DataResourceLoaderEnv)objectTranslator.GetObject(L, 2, typeof(DataResourceLoaderEnv));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
