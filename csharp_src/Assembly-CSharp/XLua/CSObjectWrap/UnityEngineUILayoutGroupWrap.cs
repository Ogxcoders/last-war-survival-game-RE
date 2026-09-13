using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUILayoutGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LayoutGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 9, 2);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetLayoutHorizontal", _m_SetLayoutHorizontal);
		Utils.RegisterFunc(L, -3, "SetLayoutVertical", _m_SetLayoutVertical);
		Utils.RegisterFunc(L, -2, "padding", _g_get_padding);
		Utils.RegisterFunc(L, -2, "childAlignment", _g_get_childAlignment);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -1, "padding", _s_set_padding);
		Utils.RegisterFunc(L, -1, "childAlignment", _s_set_childAlignment);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.LayoutGroup does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputHorizontal(IntPtr L)
	{
		try
		{
			((LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputVertical(IntPtr L)
	{
		try
		{
			((LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutHorizontal(IntPtr L)
	{
		try
		{
			((LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutHorizontal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutVertical(IntPtr L)
	{
		try
		{
			((LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_padding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LayoutGroup layoutGroup = (LayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, layoutGroup.padding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LayoutGroup layoutGroup = (LayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineTextAnchor(L, layoutGroup.childAlignment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minWidth(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutGroup.minWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preferredWidth(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutGroup.preferredWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flexibleWidth(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutGroup.flexibleWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minHeight(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutGroup.minHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preferredHeight(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutGroup.preferredHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flexibleHeight(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutGroup.flexibleHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layoutPriority(IntPtr L)
	{
		try
		{
			LayoutGroup layoutGroup = (LayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, layoutGroup.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_padding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LayoutGroup)objectTranslator.FastGetCSObj(L, 1)).padding = (RectOffset)objectTranslator.GetObject(L, 2, typeof(RectOffset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LayoutGroup layoutGroup = (LayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextAnchor val);
			layoutGroup.childAlignment = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
