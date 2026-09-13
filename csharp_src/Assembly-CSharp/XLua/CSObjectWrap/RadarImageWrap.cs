using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RadarImageWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RadarImage);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 2, 2);
		Utils.RegisterFunc(L, -3, "SetValues", _m_SetValues);
		Utils.RegisterFunc(L, -3, "SetDemensions", _m_SetDemensions);
		Utils.RegisterFunc(L, -2, "demensions", _g_get_demensions);
		Utils.RegisterFunc(L, -2, "values", _g_get_values);
		Utils.RegisterFunc(L, -1, "demensions", _s_set_demensions);
		Utils.RegisterFunc(L, -1, "values", _s_set_values);
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
				RadarImage o = new RadarImage();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RadarImage constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetValues(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RadarImage radarImage = (RadarImage)objectTranslator.FastGetCSObj(L, 1);
			List<float> values = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
			radarImage.SetValues(values);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDemensions(IntPtr L)
	{
		try
		{
			RadarImage obj = (RadarImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int demensions = Lua.xlua_tointeger(L, 2);
			obj.SetDemensions(demensions);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_demensions(IntPtr L)
	{
		try
		{
			RadarImage radarImage = (RadarImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, radarImage.demensions);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_values(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RadarImage radarImage = (RadarImage)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, radarImage.values);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_demensions(IntPtr L)
	{
		try
		{
			((RadarImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).demensions = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_values(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((RadarImage)objectTranslator.FastGetCSObj(L, 1)).values = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
