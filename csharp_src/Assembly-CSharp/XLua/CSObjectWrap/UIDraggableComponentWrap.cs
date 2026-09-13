using System;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIDraggableComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIDraggableComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 1, 1);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "ClampToScreenBounds", _m_ClampToScreenBounds);
		Utils.RegisterFunc(L, -2, "onEndDragCallback", _g_get_onEndDragCallback);
		Utils.RegisterFunc(L, -1, "onEndDragCallback", _s_set_onEndDragCallback);
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
				UIDraggableComponent o = new UIDraggableComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIDraggableComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIDraggableComponent uIDraggableComponent = (UIDraggableComponent)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIDraggableComponent.OnBeginDrag(eventData);
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
			UIDraggableComponent uIDraggableComponent = (UIDraggableComponent)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIDraggableComponent.OnDrag(eventData);
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
			UIDraggableComponent uIDraggableComponent = (UIDraggableComponent)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIDraggableComponent.OnEndDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClampToScreenBounds(IntPtr L)
	{
		try
		{
			((UIDraggableComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClampToScreenBounds();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndDragCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIDraggableComponent uIDraggableComponent = (UIDraggableComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIDraggableComponent.onEndDragCallback);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndDragCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIDraggableComponent)objectTranslator.FastGetCSObj(L, 1)).onEndDragCallback = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
