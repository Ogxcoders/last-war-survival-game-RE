using System;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PageViewComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PageViewComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 7, 7);
		Utils.RegisterFunc(L, -3, "Refresh", _m_Refresh);
		Utils.RegisterFunc(L, -3, "RefreshByCell", _m_RefreshByCell);
		Utils.RegisterFunc(L, -3, "pageTo", _m_pageTo);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -2, "onPageChanged", _g_get_onPageChanged);
		Utils.RegisterFunc(L, -2, "item", _g_get_item);
		Utils.RegisterFunc(L, -2, "Content", _g_get_Content);
		Utils.RegisterFunc(L, -2, "smooting", _g_get_smooting);
		Utils.RegisterFunc(L, -2, "sensitivity", _g_get_sensitivity);
		Utils.RegisterFunc(L, -2, "_forceOneByOne", _g_get__forceOneByOne);
		Utils.RegisterFunc(L, -2, "_emptyEnds", _g_get__emptyEnds);
		Utils.RegisterFunc(L, -1, "onPageChanged", _s_set_onPageChanged);
		Utils.RegisterFunc(L, -1, "item", _s_set_item);
		Utils.RegisterFunc(L, -1, "Content", _s_set_Content);
		Utils.RegisterFunc(L, -1, "smooting", _s_set_smooting);
		Utils.RegisterFunc(L, -1, "sensitivity", _s_set_sensitivity);
		Utils.RegisterFunc(L, -1, "_forceOneByOne", _s_set__forceOneByOne);
		Utils.RegisterFunc(L, -1, "_emptyEnds", _s_set__emptyEnds);
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
				PageViewComponent o = new PageViewComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PageViewComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Refresh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			Action<int> onUpdate = objectTranslator.GetDelegate<Action<int>>(L, 2);
			pageViewComponent.Refresh(onUpdate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshByCell(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			Action<int> onUpdate = objectTranslator.GetDelegate<Action<int>>(L, 2);
			pageViewComponent.RefreshByCell(onUpdate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_pageTo(IntPtr L)
	{
		try
		{
			PageViewComponent obj = (PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.pageTo(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			pageViewComponent.OnBeginDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			pageViewComponent.OnEndDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPageChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pageViewComponent.onPageChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_item(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pageViewComponent.item);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Content(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PageViewComponent pageViewComponent = (PageViewComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pageViewComponent.Content);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_smooting(IntPtr L)
	{
		try
		{
			PageViewComponent pageViewComponent = (PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, pageViewComponent.smooting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sensitivity(IntPtr L)
	{
		try
		{
			PageViewComponent pageViewComponent = (PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, pageViewComponent.sensitivity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__forceOneByOne(IntPtr L)
	{
		try
		{
			PageViewComponent pageViewComponent = (PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, pageViewComponent._forceOneByOne);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__emptyEnds(IntPtr L)
	{
		try
		{
			PageViewComponent pageViewComponent = (PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, pageViewComponent._emptyEnds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPageChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PageViewComponent)objectTranslator.FastGetCSObj(L, 1)).onPageChanged = objectTranslator.GetDelegate<Action<int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_item(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PageViewComponent)objectTranslator.FastGetCSObj(L, 1)).item = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Content(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PageViewComponent)objectTranslator.FastGetCSObj(L, 1)).Content = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_smooting(IntPtr L)
	{
		try
		{
			((PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).smooting = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sensitivity(IntPtr L)
	{
		try
		{
			((PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sensitivity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__forceOneByOne(IntPtr L)
	{
		try
		{
			((PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._forceOneByOne = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__emptyEnds(IntPtr L)
	{
		try
		{
			((PageViewComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._emptyEnds = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
