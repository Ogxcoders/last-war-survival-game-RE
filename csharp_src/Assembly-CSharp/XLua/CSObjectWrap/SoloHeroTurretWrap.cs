using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SoloHeroTurretWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SoloHeroTurret);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 3, 3);
		Utils.RegisterFunc(L, -2, "rotateSpeed", _g_get_rotateSpeed);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -2, "OnComplete", _g_get_OnComplete);
		Utils.RegisterFunc(L, -1, "rotateSpeed", _s_set_rotateSpeed);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
		Utils.RegisterFunc(L, -1, "OnComplete", _s_set_OnComplete);
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
				SoloHeroTurret o = new SoloHeroTurret();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoloHeroTurret constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rotateSpeed(IntPtr L)
	{
		try
		{
			SoloHeroTurret soloHeroTurret = (SoloHeroTurret)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soloHeroTurret.rotateSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoloHeroTurret soloHeroTurret = (SoloHeroTurret)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, soloHeroTurret.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoloHeroTurret soloHeroTurret = (SoloHeroTurret)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, soloHeroTurret.OnComplete);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rotateSpeed(IntPtr L)
	{
		try
		{
			((SoloHeroTurret)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rotateSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SoloHeroTurret)objectTranslator.FastGetCSObj(L, 1)).target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SoloHeroTurret)objectTranslator.FastGetCSObj(L, 1)).OnComplete = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
