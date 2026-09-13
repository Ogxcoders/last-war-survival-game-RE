using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUILayoutElementWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LayoutElement);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 8, 8);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "DOFlexibleSize", _m_DOFlexibleSize);
		Utils.RegisterFunc(L, -3, "DOMinSize", _m_DOMinSize);
		Utils.RegisterFunc(L, -3, "DOPreferredSize", _m_DOPreferredSize);
		Utils.RegisterFunc(L, -2, "ignoreLayout", _g_get_ignoreLayout);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -1, "ignoreLayout", _s_set_ignoreLayout);
		Utils.RegisterFunc(L, -1, "minWidth", _s_set_minWidth);
		Utils.RegisterFunc(L, -1, "minHeight", _s_set_minHeight);
		Utils.RegisterFunc(L, -1, "preferredWidth", _s_set_preferredWidth);
		Utils.RegisterFunc(L, -1, "preferredHeight", _s_set_preferredHeight);
		Utils.RegisterFunc(L, -1, "flexibleWidth", _s_set_flexibleWidth);
		Utils.RegisterFunc(L, -1, "flexibleHeight", _s_set_flexibleHeight);
		Utils.RegisterFunc(L, -1, "layoutPriority", _s_set_layoutPriority);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.LayoutElement does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputHorizontal(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFlexibleSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LayoutElement target = (LayoutElement)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOFlexibleSize(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOFlexibleSize(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.LayoutElement.DOFlexibleSize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOMinSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LayoutElement target = (LayoutElement)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOMinSize(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOMinSize(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.LayoutElement.DOMinSize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPreferredSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LayoutElement target = (LayoutElement)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector2, Vector2, VectorOptions> o = target.DOPreferredSize(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector2, Vector2, VectorOptions> o2 = target.DOPreferredSize(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.LayoutElement.DOPreferredSize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ignoreLayout(IntPtr L)
	{
		try
		{
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, layoutElement.ignoreLayout);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutElement.minWidth);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutElement.minHeight);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutElement.preferredWidth);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutElement.preferredHeight);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutElement.flexibleWidth);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, layoutElement.flexibleHeight);
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
			LayoutElement layoutElement = (LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, layoutElement.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ignoreLayout(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ignoreLayout = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minWidth(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).minWidth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minHeight(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).minHeight = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_preferredWidth(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).preferredWidth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_preferredHeight(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).preferredHeight = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flexibleWidth(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).flexibleWidth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flexibleHeight(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).flexibleHeight = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_layoutPriority(IntPtr L)
	{
		try
		{
			((LayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).layoutPriority = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
