using System;
using DG.Tweening;
using GameKit.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIScrollRectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ScrollRect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 23, 28, 21);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "IsActive", _m_IsActive);
		Utils.RegisterFunc(L, -3, "StopMovement", _m_StopMovement);
		Utils.RegisterFunc(L, -3, "OnScroll", _m_OnScroll);
		Utils.RegisterFunc(L, -3, "OnInitializePotentialDrag", _m_OnInitializePotentialDrag);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "GetPrevPosition", _m_GetPrevPosition);
		Utils.RegisterFunc(L, -3, "SetPrevPosition", _m_SetPrevPosition);
		Utils.RegisterFunc(L, -3, "OffsetPrevPosition", _m_OffsetPrevPosition);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetLayoutHorizontal", _m_SetLayoutHorizontal);
		Utils.RegisterFunc(L, -3, "SetLayoutVertical", _m_SetLayoutVertical);
		Utils.RegisterFunc(L, -3, "DONormalizedPos", _m_DONormalizedPos);
		Utils.RegisterFunc(L, -3, "DOHorizontalNormalizedPos", _m_DOHorizontalNormalizedPos);
		Utils.RegisterFunc(L, -3, "DOVerticalNormalizedPos", _m_DOVerticalNormalizedPos);
		Utils.RegisterFunc(L, -3, "ScrollRect_EndDrag", _m_ScrollRect_EndDrag);
		Utils.RegisterFunc(L, -3, "SetHorizontalNormalizedPosition", _m_SetHorizontalNormalizedPosition);
		Utils.RegisterFunc(L, -3, "GetHorizontalNormalizedPosition", _m_GetHorizontalNormalizedPosition);
		Utils.RegisterFunc(L, -2, "content", _g_get_content);
		Utils.RegisterFunc(L, -2, "horizontal", _g_get_horizontal);
		Utils.RegisterFunc(L, -2, "vertical", _g_get_vertical);
		Utils.RegisterFunc(L, -2, "movementType", _g_get_movementType);
		Utils.RegisterFunc(L, -2, "elasticity", _g_get_elasticity);
		Utils.RegisterFunc(L, -2, "inertia", _g_get_inertia);
		Utils.RegisterFunc(L, -2, "decelerationRate", _g_get_decelerationRate);
		Utils.RegisterFunc(L, -2, "scrollSensitivity", _g_get_scrollSensitivity);
		Utils.RegisterFunc(L, -2, "viewport", _g_get_viewport);
		Utils.RegisterFunc(L, -2, "horizontalScrollbar", _g_get_horizontalScrollbar);
		Utils.RegisterFunc(L, -2, "verticalScrollbar", _g_get_verticalScrollbar);
		Utils.RegisterFunc(L, -2, "horizontalScrollbarVisibility", _g_get_horizontalScrollbarVisibility);
		Utils.RegisterFunc(L, -2, "verticalScrollbarVisibility", _g_get_verticalScrollbarVisibility);
		Utils.RegisterFunc(L, -2, "horizontalScrollbarSpacing", _g_get_horizontalScrollbarSpacing);
		Utils.RegisterFunc(L, -2, "verticalScrollbarSpacing", _g_get_verticalScrollbarSpacing);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -2, "velocity", _g_get_velocity);
		Utils.RegisterFunc(L, -2, "InertiaSkipNormalizePosition", _g_get_InertiaSkipNormalizePosition);
		Utils.RegisterFunc(L, -2, "normalizedPosition", _g_get_normalizedPosition);
		Utils.RegisterFunc(L, -2, "horizontalNormalizedPosition", _g_get_horizontalNormalizedPosition);
		Utils.RegisterFunc(L, -2, "verticalNormalizedPosition", _g_get_verticalNormalizedPosition);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -1, "content", _s_set_content);
		Utils.RegisterFunc(L, -1, "horizontal", _s_set_horizontal);
		Utils.RegisterFunc(L, -1, "vertical", _s_set_vertical);
		Utils.RegisterFunc(L, -1, "movementType", _s_set_movementType);
		Utils.RegisterFunc(L, -1, "elasticity", _s_set_elasticity);
		Utils.RegisterFunc(L, -1, "inertia", _s_set_inertia);
		Utils.RegisterFunc(L, -1, "decelerationRate", _s_set_decelerationRate);
		Utils.RegisterFunc(L, -1, "scrollSensitivity", _s_set_scrollSensitivity);
		Utils.RegisterFunc(L, -1, "viewport", _s_set_viewport);
		Utils.RegisterFunc(L, -1, "horizontalScrollbar", _s_set_horizontalScrollbar);
		Utils.RegisterFunc(L, -1, "verticalScrollbar", _s_set_verticalScrollbar);
		Utils.RegisterFunc(L, -1, "horizontalScrollbarVisibility", _s_set_horizontalScrollbarVisibility);
		Utils.RegisterFunc(L, -1, "verticalScrollbarVisibility", _s_set_verticalScrollbarVisibility);
		Utils.RegisterFunc(L, -1, "horizontalScrollbarSpacing", _s_set_horizontalScrollbarSpacing);
		Utils.RegisterFunc(L, -1, "verticalScrollbarSpacing", _s_set_verticalScrollbarSpacing);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.RegisterFunc(L, -1, "velocity", _s_set_velocity);
		Utils.RegisterFunc(L, -1, "InertiaSkipNormalizePosition", _s_set_InertiaSkipNormalizePosition);
		Utils.RegisterFunc(L, -1, "normalizedPosition", _s_set_normalizedPosition);
		Utils.RegisterFunc(L, -1, "horizontalNormalizedPosition", _s_set_horizontalNormalizedPosition);
		Utils.RegisterFunc(L, -1, "verticalNormalizedPosition", _s_set_verticalNormalizedPosition);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.ScrollRect does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			scrollRect.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LayoutComplete(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GraphicUpdateComplete(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsActive(IntPtr L)
	{
		try
		{
			bool value = ((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsActive();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopMovement(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopMovement();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnScroll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollRect.OnScroll(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnInitializePotentialDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollRect.OnInitializePotentialDrag(eventData);
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
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollRect.OnBeginDrag(eventData);
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
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollRect.OnEndDrag(eventData);
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
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollRect.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPrevPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2 prevPosition = ((ScrollRect)objectTranslator.FastGetCSObj(L, 1)).GetPrevPosition();
			objectTranslator.PushUnityEngineVector2(L, prevPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPrevPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollRect.SetPrevPosition(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OffsetPrevPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollRect.OffsetPrevPosition(val);
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
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
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
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutHorizontal();
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
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DONormalizedPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect target = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				Tweener o = target.DONormalizedPos(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DONormalizedPos(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.ScrollRect.DONormalizedPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOHorizontalNormalizedPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect target = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				Tweener o = target.DOHorizontalNormalizedPos(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DOHorizontalNormalizedPos(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.ScrollRect.DOHorizontalNormalizedPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOVerticalNormalizedPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect target = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				Tweener o = target.DOVerticalNormalizedPos(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DOVerticalNormalizedPos(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.ScrollRect.DOVerticalNormalizedPos!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScrollRect_EndDrag(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ScrollRect_EndDrag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetHorizontalNormalizedPosition(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float ratio = (float)Lua.lua_tonumber(L, 2);
			scrollRect.SetHorizontalNormalizedPosition(ratio);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHorizontalNormalizedPosition(IntPtr L)
	{
		try
		{
			float horizontalNormalizedPosition = ((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHorizontalNormalizedPosition();
			Lua.lua_pushnumber(L, horizontalNormalizedPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_content(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollRect.content);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontal(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollRect.horizontal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertical(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollRect.vertical);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_movementType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIScrollRectMovementType(L, scrollRect.movementType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_elasticity(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.elasticity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inertia(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollRect.inertia);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_decelerationRate(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.decelerationRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scrollSensitivity(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.scrollSensitivity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_viewport(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollRect.viewport);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalScrollbar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollRect.horizontalScrollbar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalScrollbar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollRect.verticalScrollbar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalScrollbarVisibility(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIScrollRectScrollbarVisibility(L, scrollRect.horizontalScrollbarVisibility);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalScrollbarVisibility(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIScrollRectScrollbarVisibility(L, scrollRect.verticalScrollbarVisibility);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalScrollbarSpacing(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.horizontalScrollbarSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalScrollbarSpacing(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.verticalScrollbarSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollRect.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, scrollRect.velocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_InertiaSkipNormalizePosition(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollRect.InertiaSkipNormalizePosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalizedPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, scrollRect.normalizedPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalNormalizedPosition(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.horizontalNormalizedPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalNormalizedPosition(IntPtr L)
	{
		try
		{
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.verticalNormalizedPosition);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.minWidth);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.preferredWidth);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.flexibleWidth);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.minHeight);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.preferredHeight);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollRect.flexibleHeight);
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
			ScrollRect scrollRect = (ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, scrollRect.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_content(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollRect)objectTranslator.FastGetCSObj(L, 1)).content = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontal(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).horizontal = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_vertical(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).vertical = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_movementType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrollRect.MovementType val);
			scrollRect.movementType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_elasticity(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).elasticity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inertia(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).inertia = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_decelerationRate(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).decelerationRate = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scrollSensitivity(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scrollSensitivity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_viewport(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollRect)objectTranslator.FastGetCSObj(L, 1)).viewport = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalScrollbar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollRect)objectTranslator.FastGetCSObj(L, 1)).horizontalScrollbar = (Scrollbar)objectTranslator.GetObject(L, 2, typeof(Scrollbar));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalScrollbar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollRect)objectTranslator.FastGetCSObj(L, 1)).verticalScrollbar = (Scrollbar)objectTranslator.GetObject(L, 2, typeof(Scrollbar));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalScrollbarVisibility(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrollRect.ScrollbarVisibility val);
			scrollRect.horizontalScrollbarVisibility = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalScrollbarVisibility(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrollRect.ScrollbarVisibility val);
			scrollRect.verticalScrollbarVisibility = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalScrollbarSpacing(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).horizontalScrollbarSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalScrollbarSpacing(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).verticalScrollbarSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollRect)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (ScrollRect.ScrollRectEvent)objectTranslator.GetObject(L, 2, typeof(ScrollRect.ScrollRectEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollRect.velocity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_InertiaSkipNormalizePosition(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InertiaSkipNormalizePosition = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normalizedPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollRect scrollRect = (ScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollRect.normalizedPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalNormalizedPosition(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).horizontalNormalizedPosition = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalNormalizedPosition(IntPtr L)
	{
		try
		{
			((ScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).verticalNormalizedPosition = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
