using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class HorizontalInfinityScrollViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(HorizontalInfinityScrollView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "MoveItemByIndex", _m_MoveItemByIndex);
		Utils.RegisterFunc(L, -3, "ForceUpdate", _m_ForceUpdate);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "SetItemCount", _m_SetItemCount);
		Utils.RegisterFunc(L, -3, "GetInfinityItemByIndex", _m_GetInfinityItemByIndex);
		Utils.RegisterFunc(L, -3, "SetScrollRectHorizontal", _m_SetScrollRectHorizontal);
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
				HorizontalInfinityScrollView o = new HorizontalInfinityScrollView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to HorizontalInfinityScrollView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HorizontalInfinityScrollView horizontalInfinityScrollView = (HorizontalInfinityScrollView)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject, int> onInit = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 2);
			Action<GameObject, int, int> onUpdate = objectTranslator.GetDelegate<Action<GameObject, int, int>>(L, 3);
			Action<GameObject, int> onDestroy = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 4);
			horizontalInfinityScrollView.Init(onInit, onUpdate, onDestroy);
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
			HorizontalInfinityScrollView obj = (HorizontalInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_ForceUpdate(IntPtr L)
	{
		try
		{
			((HorizontalInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdate();
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
			((HorizontalInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
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
			HorizontalInfinityScrollView obj = (HorizontalInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetInfinityItemByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HorizontalInfinityScrollView obj = (HorizontalInfinityScrollView)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_SetScrollRectHorizontal(IntPtr L)
	{
		try
		{
			HorizontalInfinityScrollView obj = (HorizontalInfinityScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool scrollRectHorizontal = Lua.lua_toboolean(L, 2);
			obj.SetScrollRectHorizontal(scrollRectHorizontal);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
