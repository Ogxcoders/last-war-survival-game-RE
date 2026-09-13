using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIHorizontalOrVerticalLayoutGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(HorizontalOrVerticalLayoutGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 7, 7);
		Utils.RegisterFunc(L, -2, "spacing", _g_get_spacing);
		Utils.RegisterFunc(L, -2, "childForceExpandWidth", _g_get_childForceExpandWidth);
		Utils.RegisterFunc(L, -2, "childForceExpandHeight", _g_get_childForceExpandHeight);
		Utils.RegisterFunc(L, -2, "childControlWidth", _g_get_childControlWidth);
		Utils.RegisterFunc(L, -2, "childControlHeight", _g_get_childControlHeight);
		Utils.RegisterFunc(L, -2, "childScaleWidth", _g_get_childScaleWidth);
		Utils.RegisterFunc(L, -2, "childScaleHeight", _g_get_childScaleHeight);
		Utils.RegisterFunc(L, -1, "spacing", _s_set_spacing);
		Utils.RegisterFunc(L, -1, "childForceExpandWidth", _s_set_childForceExpandWidth);
		Utils.RegisterFunc(L, -1, "childForceExpandHeight", _s_set_childForceExpandHeight);
		Utils.RegisterFunc(L, -1, "childControlWidth", _s_set_childControlWidth);
		Utils.RegisterFunc(L, -1, "childControlHeight", _s_set_childControlHeight);
		Utils.RegisterFunc(L, -1, "childScaleWidth", _s_set_childScaleWidth);
		Utils.RegisterFunc(L, -1, "childScaleHeight", _s_set_childScaleHeight);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.HorizontalOrVerticalLayoutGroup does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spacing(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, horizontalOrVerticalLayoutGroup.spacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childForceExpandWidth(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, horizontalOrVerticalLayoutGroup.childForceExpandWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childForceExpandHeight(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, horizontalOrVerticalLayoutGroup.childForceExpandHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childControlWidth(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, horizontalOrVerticalLayoutGroup.childControlWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childControlHeight(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, horizontalOrVerticalLayoutGroup.childControlHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childScaleWidth(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, horizontalOrVerticalLayoutGroup.childScaleWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childScaleHeight(IntPtr L)
	{
		try
		{
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, horizontalOrVerticalLayoutGroup.childScaleHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spacing(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).spacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childForceExpandWidth(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).childForceExpandWidth = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childForceExpandHeight(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).childForceExpandHeight = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childControlWidth(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).childControlWidth = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childControlHeight(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).childControlHeight = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childScaleWidth(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).childScaleWidth = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childScaleHeight(IntPtr L)
	{
		try
		{
			((HorizontalOrVerticalLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).childScaleHeight = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
