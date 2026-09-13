using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GridInfinityScrollViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GridInfinityScrollView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 1, 1);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "ForceUpdate", _m_ForceUpdate);
		Utils.RegisterFunc(L, -3, "ForceUpdateCell", _m_ForceUpdateCell);
		Utils.RegisterFunc(L, -3, "SetItemCount", _m_SetItemCount);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "MoveItemByIndex", _m_MoveItemByIndex);
		Utils.RegisterFunc(L, -3, "LaterItemByIndex", _m_LaterItemByIndex);
		Utils.RegisterFunc(L, -3, "GetColumnCount", _m_GetColumnCount);
		Utils.RegisterFunc(L, -3, "GetRenderItemSizeY", _m_GetRenderItemSizeY);
		Utils.RegisterFunc(L, -3, "RefreshMaskSize", _m_RefreshMaskSize);
		Utils.RegisterFunc(L, -2, "isRTLInArabic", _g_get_isRTLInArabic);
		Utils.RegisterFunc(L, -1, "isRTLInArabic", _s_set_isRTLInArabic);
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
				GridInfinityScrollView o = new GridInfinityScrollView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GridInfinityScrollView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GridInfinityScrollView gridInfinityScrollView = (GridInfinityScrollView)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject, int> onInit = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 2);
			Action<GameObject, int, int> onUpdate = objectTranslator.GetDelegate<Action<GameObject, int, int>>(L, 3);
			Action<GameObject, int> onDestroy = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 4);
			gridInfinityScrollView.Init(onInit, onUpdate, onDestroy);
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
			((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceUpdateCell(IntPtr L)
	{
		try
		{
			((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdateCell();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetItemCount(IntPtr L)
	{
		try
		{
			GridInfinityScrollView obj = (GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemCount = Lua.xlua_tointeger(L, 2);
			obj.SetItemCount(itemCount);
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
			((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveItemByIndex(IntPtr L)
	{
		try
		{
			GridInfinityScrollView obj = (GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			float delay = (float)Lua.lua_tonumber(L, 3);
			obj.MoveItemByIndex(index, delay);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LaterItemByIndex(IntPtr L)
	{
		try
		{
			GridInfinityScrollView obj = (GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			float delay = (float)Lua.lua_tonumber(L, 3);
			obj.LaterItemByIndex(index, delay);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColumnCount(IntPtr L)
	{
		try
		{
			int columnCount = ((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetColumnCount();
			Lua.xlua_pushinteger(L, columnCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderItemSizeY(IntPtr L)
	{
		try
		{
			float renderItemSizeY = ((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRenderItemSizeY();
			Lua.lua_pushnumber(L, renderItemSizeY);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshMaskSize(IntPtr L)
	{
		try
		{
			((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshMaskSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRTLInArabic(IntPtr L)
	{
		try
		{
			GridInfinityScrollView gridInfinityScrollView = (GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gridInfinityScrollView.isRTLInArabic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isRTLInArabic(IntPtr L)
	{
		try
		{
			((GridInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isRTLInArabic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
