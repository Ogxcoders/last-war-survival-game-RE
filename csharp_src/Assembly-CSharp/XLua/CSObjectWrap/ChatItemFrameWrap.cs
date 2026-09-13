using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ChatItemFrameWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ChatItemFrame);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 2, 2);
		Utils.RegisterFunc(L, -3, "SwapAlign", _m_SwapAlign);
		Utils.RegisterFunc(L, -3, "ClearAdditionOffset", _m_ClearAdditionOffset);
		Utils.RegisterFunc(L, -3, "AddAdditionOffset", _m_AddAdditionOffset);
		Utils.RegisterFunc(L, -2, "alignToLeft", _g_get_alignToLeft);
		Utils.RegisterFunc(L, -2, "alignItems", _g_get_alignItems);
		Utils.RegisterFunc(L, -1, "alignToLeft", _s_set_alignToLeft);
		Utils.RegisterFunc(L, -1, "alignItems", _s_set_alignItems);
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
				ChatItemFrame o = new ChatItemFrame();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChatItemFrame constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SwapAlign(IntPtr L)
	{
		try
		{
			ChatItemFrame chatItemFrame = (ChatItemFrame)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				chatItemFrame.SwapAlign();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool toLeft = Lua.lua_toboolean(L, 2);
					chatItemFrame.SwapAlign(toLeft);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChatItemFrame.SwapAlign!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAdditionOffset(IntPtr L)
	{
		try
		{
			((ChatItemFrame)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAdditionOffset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddAdditionOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatItemFrame chatItemFrame = (ChatItemFrame)objectTranslator.FastGetCSObj(L, 1);
			RectTransform rect = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
			int offset = Lua.xlua_tointeger(L, 3);
			chatItemFrame.AddAdditionOffset(rect, offset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alignToLeft(IntPtr L)
	{
		try
		{
			ChatItemFrame chatItemFrame = (ChatItemFrame)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, chatItemFrame.alignToLeft);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alignItems(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatItemFrame chatItemFrame = (ChatItemFrame)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, chatItemFrame.alignItems);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alignToLeft(IntPtr L)
	{
		try
		{
			((ChatItemFrame)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alignToLeft = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alignItems(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ChatItemFrame)objectTranslator.FastGetCSObj(L, 1)).alignItems = (ChatItemFrame.AlignItem[])objectTranslator.GetObject(L, 2, typeof(ChatItemFrame.AlignItem[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
