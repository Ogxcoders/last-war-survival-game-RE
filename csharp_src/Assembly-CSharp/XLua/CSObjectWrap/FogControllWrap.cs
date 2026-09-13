using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FogControllWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FogControll);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 6, 6);
		Utils.RegisterFunc(L, -3, "Open", _m_Open);
		Utils.RegisterFunc(L, -3, "Close", _m_Close);
		Utils.RegisterFunc(L, -2, "DistanceFog", _g_get_DistanceFog);
		Utils.RegisterFunc(L, -2, "DistanceFogStart", _g_get_DistanceFogStart);
		Utils.RegisterFunc(L, -2, "DistanceFogEnd", _g_get_DistanceFogEnd);
		Utils.RegisterFunc(L, -2, "HeightFogStart", _g_get_HeightFogStart);
		Utils.RegisterFunc(L, -2, "HeightFogEnd", _g_get_HeightFogEnd);
		Utils.RegisterFunc(L, -2, "EnableFog", _g_get_EnableFog);
		Utils.RegisterFunc(L, -1, "DistanceFog", _s_set_DistanceFog);
		Utils.RegisterFunc(L, -1, "DistanceFogStart", _s_set_DistanceFogStart);
		Utils.RegisterFunc(L, -1, "DistanceFogEnd", _s_set_DistanceFogEnd);
		Utils.RegisterFunc(L, -1, "HeightFogStart", _s_set_HeightFogStart);
		Utils.RegisterFunc(L, -1, "HeightFogEnd", _s_set_HeightFogEnd);
		Utils.RegisterFunc(L, -1, "EnableFog", _s_set_EnableFog);
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
				FogControll o = new FogControll();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FogControll constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Open(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Open();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Close(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Close();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DistanceFog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FogControll fogControll = (FogControll)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, fogControll.DistanceFog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DistanceFogStart(IntPtr L)
	{
		try
		{
			FogControll fogControll = (FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fogControll.DistanceFogStart);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DistanceFogEnd(IntPtr L)
	{
		try
		{
			FogControll fogControll = (FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fogControll.DistanceFogEnd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeightFogStart(IntPtr L)
	{
		try
		{
			FogControll fogControll = (FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fogControll.HeightFogStart);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeightFogEnd(IntPtr L)
	{
		try
		{
			FogControll fogControll = (FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fogControll.HeightFogEnd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EnableFog(IntPtr L)
	{
		try
		{
			FogControll fogControll = (FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fogControll.EnableFog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DistanceFog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FogControll fogControll = (FogControll)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			fogControll.DistanceFog = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DistanceFogStart(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DistanceFogStart = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DistanceFogEnd(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DistanceFogEnd = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HeightFogStart(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HeightFogStart = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HeightFogEnd(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HeightFogEnd = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EnableFog(IntPtr L)
	{
		try
		{
			((FogControll)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnableFog = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
