using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRectOffsetWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RectOffset);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 6, 4);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "Add", _m_Add);
		Utils.RegisterFunc(L, -3, "Remove", _m_Remove);
		Utils.RegisterFunc(L, -2, "left", _g_get_left);
		Utils.RegisterFunc(L, -2, "right", _g_get_right);
		Utils.RegisterFunc(L, -2, "top", _g_get_top);
		Utils.RegisterFunc(L, -2, "bottom", _g_get_bottom);
		Utils.RegisterFunc(L, -2, "horizontal", _g_get_horizontal);
		Utils.RegisterFunc(L, -2, "vertical", _g_get_vertical);
		Utils.RegisterFunc(L, -1, "left", _s_set_left);
		Utils.RegisterFunc(L, -1, "right", _s_set_right);
		Utils.RegisterFunc(L, -1, "top", _s_set_top);
		Utils.RegisterFunc(L, -1, "bottom", _s_set_bottom);
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
				RectOffset o = new RectOffset();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int left = Lua.xlua_tointeger(L, 2);
				int right = Lua.xlua_tointeger(L, 3);
				int top = Lua.xlua_tointeger(L, 4);
				int bottom = Lua.xlua_tointeger(L, 5);
				RectOffset o2 = new RectOffset(left, right, top, bottom);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RectOffset constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			string str = ((RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToString();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Add(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectOffset rectOffset = (RectOffset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			Rect rect = rectOffset.Add(v);
			objectTranslator.Push(L, rect);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Remove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectOffset rectOffset = (RectOffset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			Rect rect = rectOffset.Remove(v);
			objectTranslator.Push(L, rect);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_left(IntPtr L)
	{
		try
		{
			RectOffset rectOffset = (RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rectOffset.left);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_right(IntPtr L)
	{
		try
		{
			RectOffset rectOffset = (RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rectOffset.right);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_top(IntPtr L)
	{
		try
		{
			RectOffset rectOffset = (RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rectOffset.top);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bottom(IntPtr L)
	{
		try
		{
			RectOffset rectOffset = (RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rectOffset.bottom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontal(IntPtr L)
	{
		try
		{
			RectOffset rectOffset = (RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rectOffset.horizontal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertical(IntPtr L)
	{
		try
		{
			RectOffset rectOffset = (RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rectOffset.vertical);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_left(IntPtr L)
	{
		try
		{
			((RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).left = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_right(IntPtr L)
	{
		try
		{
			((RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).right = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_top(IntPtr L)
	{
		try
		{
			((RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).top = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bottom(IntPtr L)
	{
		try
		{
			((RectOffset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).bottom = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
