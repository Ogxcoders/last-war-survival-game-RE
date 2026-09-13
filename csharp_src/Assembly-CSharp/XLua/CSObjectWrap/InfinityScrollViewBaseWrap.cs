using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class InfinityScrollViewBaseWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(InfinityScrollViewBase);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 2, 3);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "SetItemCount", _m_SetItemCount);
		Utils.RegisterFunc(L, -3, "ForceUpdate", _m_ForceUpdate);
		Utils.RegisterFunc(L, -3, "ForceUpdateCell", _m_ForceUpdateCell);
		Utils.RegisterFunc(L, -3, "GetColumnCount", _m_GetColumnCount);
		Utils.RegisterFunc(L, -3, "GetRenderCount", _m_GetRenderCount);
		Utils.RegisterFunc(L, -3, "GetInfinityItemByIndex", _m_GetInfinityItemByIndex);
		Utils.RegisterFunc(L, -3, "MoveItemByIndex", _m_MoveItemByIndex);
		Utils.RegisterFunc(L, -3, "StopLocateCoroutine", _m_StopLocateCoroutine);
		Utils.RegisterFunc(L, -2, "onUpdate", _g_get_onUpdate);
		Utils.RegisterFunc(L, -2, "onDestroy", _g_get_onDestroy);
		Utils.RegisterFunc(L, -1, "MaxCount", _s_set_MaxCount);
		Utils.RegisterFunc(L, -1, "onUpdate", _s_set_onUpdate);
		Utils.RegisterFunc(L, -1, "onDestroy", _s_set_onDestroy);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "InfinityScrollViewBase does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InfinityScrollViewBase infinityScrollViewBase = (InfinityScrollViewBase)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject, int> onInit = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 2);
			Action<GameObject, int, int> onUpdate = objectTranslator.GetDelegate<Action<GameObject, int, int>>(L, 3);
			Action<GameObject, int> onDestroy = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 4);
			infinityScrollViewBase.Init(onInit, onUpdate, onDestroy);
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
			((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
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
			InfinityScrollViewBase obj = (InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_ForceUpdate(IntPtr L)
	{
		try
		{
			((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdate();
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
			((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdateCell();
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
			int columnCount = ((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetColumnCount();
			Lua.xlua_pushinteger(L, columnCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderCount(IntPtr L)
	{
		try
		{
			int renderCount = ((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRenderCount();
			Lua.xlua_pushinteger(L, renderCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInfinityItemByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InfinityScrollViewBase obj = (InfinityScrollViewBase)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			InfinityItem infinityItemByIndex = obj.GetInfinityItemByIndex(index);
			objectTranslator.Push(L, infinityItemByIndex);
			return 1;
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
			InfinityScrollViewBase obj = (InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_StopLocateCoroutine(IntPtr L)
	{
		try
		{
			((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopLocateCoroutine();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InfinityScrollViewBase infinityScrollViewBase = (InfinityScrollViewBase)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, infinityScrollViewBase.onUpdate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onDestroy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InfinityScrollViewBase infinityScrollViewBase = (InfinityScrollViewBase)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, infinityScrollViewBase.onDestroy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MaxCount(IntPtr L)
	{
		try
		{
			((InfinityScrollViewBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MaxCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((InfinityScrollViewBase)objectTranslator.FastGetCSObj(L, 1)).onUpdate = objectTranslator.GetDelegate<Action<GameObject, int, int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onDestroy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((InfinityScrollViewBase)objectTranslator.FastGetCSObj(L, 1)).onDestroy = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
