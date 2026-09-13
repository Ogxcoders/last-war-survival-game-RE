using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ChatItemPostLayoutWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ChatItemPostLayout);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 9, 2);
		Utils.RegisterFunc(L, -3, "SetDirty", _m_SetDirty);
		Utils.RegisterFunc(L, -3, "ForceUpdate", _m_ForceUpdate);
		Utils.RegisterFunc(L, -3, "CalculateSize", _m_CalculateSize);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetLayoutHorizontal", _m_SetLayoutHorizontal);
		Utils.RegisterFunc(L, -3, "SetLayoutVertical", _m_SetLayoutVertical);
		Utils.RegisterFunc(L, -3, "SetPreferredSize", _m_SetPreferredSize);
		Utils.RegisterFunc(L, -3, "SetPreferredWidth", _m_SetPreferredWidth);
		Utils.RegisterFunc(L, -3, "SetPreferredHeight", _m_SetPreferredHeight);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -2, "minSize", _g_get_minSize);
		Utils.RegisterFunc(L, -2, "padding", _g_get_padding);
		Utils.RegisterFunc(L, -1, "minSize", _s_set_minSize);
		Utils.RegisterFunc(L, -1, "padding", _s_set_padding);
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
				ChatItemPostLayout o = new ChatItemPostLayout();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChatItemPostLayout constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDirty(IntPtr L)
	{
		try
		{
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceUpdate(IntPtr L)
	{
		try
		{
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateSize(IntPtr L)
	{
		try
		{
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputHorizontal(IntPtr L)
	{
		try
		{
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
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
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutHorizontal();
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
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPreferredSize(IntPtr L)
	{
		try
		{
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				chatItemPostLayout.SetPreferredSize();
				return 0;
			case 3:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
				{
					float width = (float)Lua.lua_tonumber(L, 2);
					float height = (float)Lua.lua_tonumber(L, 3);
					chatItemPostLayout.SetPreferredSize(width, height);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChatItemPostLayout.SetPreferredSize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPreferredWidth(IntPtr L)
	{
		try
		{
			ChatItemPostLayout obj = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float preferredWidth = (float)Lua.lua_tonumber(L, 2);
			obj.SetPreferredWidth(preferredWidth);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPreferredHeight(IntPtr L)
	{
		try
		{
			((ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetPreferredHeight();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minWidth(IntPtr L)
	{
		try
		{
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, chatItemPostLayout.minWidth);
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
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, chatItemPostLayout.preferredWidth);
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
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, chatItemPostLayout.flexibleWidth);
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
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, chatItemPostLayout.minHeight);
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
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, chatItemPostLayout.preferredHeight);
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
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, chatItemPostLayout.flexibleHeight);
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
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, chatItemPostLayout.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, chatItemPostLayout.minSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_padding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, chatItemPostLayout.padding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			chatItemPostLayout.minSize = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_padding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatItemPostLayout chatItemPostLayout = (ChatItemPostLayout)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			chatItemPostLayout.padding = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
