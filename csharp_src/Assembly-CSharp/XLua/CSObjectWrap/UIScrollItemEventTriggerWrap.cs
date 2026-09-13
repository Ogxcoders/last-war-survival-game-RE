using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIScrollItemEventTriggerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIScrollItemEventTrigger);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 10, 10);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnDrop", _m_OnDrop);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnPointerUp", _m_OnPointerUp);
		Utils.RegisterFunc(L, -2, "scrollRect", _g_get_scrollRect);
		Utils.RegisterFunc(L, -2, "autoFindScrollRect", _g_get_autoFindScrollRect);
		Utils.RegisterFunc(L, -2, "pixelClickThreshold", _g_get_pixelClickThreshold);
		Utils.RegisterFunc(L, -2, "onBeginDrag", _g_get_onBeginDrag);
		Utils.RegisterFunc(L, -2, "onDrag", _g_get_onDrag);
		Utils.RegisterFunc(L, -2, "onDrop", _g_get_onDrop);
		Utils.RegisterFunc(L, -2, "onEndDrag", _g_get_onEndDrag);
		Utils.RegisterFunc(L, -2, "onPointerClick", _g_get_onPointerClick);
		Utils.RegisterFunc(L, -2, "onPointerDown", _g_get_onPointerDown);
		Utils.RegisterFunc(L, -2, "onPointerUp", _g_get_onPointerUp);
		Utils.RegisterFunc(L, -1, "scrollRect", _s_set_scrollRect);
		Utils.RegisterFunc(L, -1, "autoFindScrollRect", _s_set_autoFindScrollRect);
		Utils.RegisterFunc(L, -1, "pixelClickThreshold", _s_set_pixelClickThreshold);
		Utils.RegisterFunc(L, -1, "onBeginDrag", _s_set_onBeginDrag);
		Utils.RegisterFunc(L, -1, "onDrag", _s_set_onDrag);
		Utils.RegisterFunc(L, -1, "onDrop", _s_set_onDrop);
		Utils.RegisterFunc(L, -1, "onEndDrag", _s_set_onEndDrag);
		Utils.RegisterFunc(L, -1, "onPointerClick", _s_set_onPointerClick);
		Utils.RegisterFunc(L, -1, "onPointerDown", _s_set_onPointerDown);
		Utils.RegisterFunc(L, -1, "onPointerUp", _s_set_onPointerUp);
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
				UIScrollItemEventTrigger o = new UIScrollItemEventTrigger();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIScrollItemEventTrigger constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnBeginDrag(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnDrag(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnDrop(data);
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
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnEndDrag(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnPointerClick(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnPointerDown(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIScrollItemEventTrigger.OnPointerUp(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scrollRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.scrollRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoFindScrollRect(IntPtr L)
	{
		try
		{
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIScrollItemEventTrigger.autoFindScrollRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelClickThreshold(IntPtr L)
	{
		try
		{
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, uIScrollItemEventTrigger.pixelClickThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onBeginDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onDrop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onDrop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onEndDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onPointerClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onPointerDown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIScrollItemEventTrigger uIScrollItemEventTrigger = (UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIScrollItemEventTrigger.onPointerUp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scrollRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).scrollRect = (ScrollRect)objectTranslator.GetObject(L, 2, typeof(ScrollRect));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoFindScrollRect(IntPtr L)
	{
		try
		{
			((UIScrollItemEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoFindScrollRect = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pixelClickThreshold(IntPtr L)
	{
		try
		{
			((UIScrollItemEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pixelClickThreshold = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onBeginDrag = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onDrag = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onDrop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onDrop = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onEndDrag = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerClick = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerDown = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIScrollItemEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerUp = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
