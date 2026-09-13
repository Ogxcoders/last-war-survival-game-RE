using System;
using GameKit.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ScrollViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ScrollView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 23, 36, 30);
		Utils.RegisterFunc(L, -3, "ClearCells", _m_ClearCells);
		Utils.RegisterFunc(L, -3, "ScrollToCell", _m_ScrollToCell);
		Utils.RegisterFunc(L, -3, "StopScrollToCell", _m_StopScrollToCell);
		Utils.RegisterFunc(L, -3, "RefreshCells", _m_RefreshCells);
		Utils.RegisterFunc(L, -3, "RefillCellsFromEnd", _m_RefillCellsFromEnd);
		Utils.RegisterFunc(L, -3, "SetExtraFillSize", _m_SetExtraFillSize);
		Utils.RegisterFunc(L, -3, "RefillCells", _m_RefillCells);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "IsActive", _m_IsActive);
		Utils.RegisterFunc(L, -3, "StopMovement", _m_StopMovement);
		Utils.RegisterFunc(L, -3, "OnScroll", _m_OnScroll);
		Utils.RegisterFunc(L, -3, "OnInitializePotentialDrag", _m_OnInitializePotentialDrag);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetLayoutHorizontal", _m_SetLayoutHorizontal);
		Utils.RegisterFunc(L, -3, "SetLayoutVertical", _m_SetLayoutVertical);
		Utils.RegisterFunc(L, -3, "ScrollView_EndDrag", _m_ScrollView_EndDrag);
		Utils.RegisterFunc(L, -2, "content", _g_get_content);
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
		Utils.RegisterFunc(L, -2, "totalCount", _g_get_totalCount);
		Utils.RegisterFunc(L, -2, "reverseDirection", _g_get_reverseDirection);
		Utils.RegisterFunc(L, -2, "arabicMirrorDirection", _g_get_arabicMirrorDirection);
		Utils.RegisterFunc(L, -2, "rubberScale", _g_get_rubberScale);
		Utils.RegisterFunc(L, -2, "m_ContentConstraintCount", _g_get_m_ContentConstraintCount);
		Utils.RegisterFunc(L, -2, "onItemMoveIn", _g_get_onItemMoveIn);
		Utils.RegisterFunc(L, -2, "onItemMoveOut", _g_get_onItemMoveOut);
		Utils.RegisterFunc(L, -2, "onBeginDragCallBack", _g_get_onBeginDragCallBack);
		Utils.RegisterFunc(L, -2, "onEndDragCallBack", _g_get_onEndDragCallBack);
		Utils.RegisterFunc(L, -2, "onDragCallBack", _g_get_onDragCallBack);
		Utils.RegisterFunc(L, -2, "onScrollToCellEndCallBack", _g_get_onScrollToCellEndCallBack);
		Utils.RegisterFunc(L, -1, "content", _s_set_content);
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
		Utils.RegisterFunc(L, -1, "FixedItemSize", _s_set_FixedItemSize);
		Utils.RegisterFunc(L, -1, "normalizedPosition", _s_set_normalizedPosition);
		Utils.RegisterFunc(L, -1, "horizontalNormalizedPosition", _s_set_horizontalNormalizedPosition);
		Utils.RegisterFunc(L, -1, "verticalNormalizedPosition", _s_set_verticalNormalizedPosition);
		Utils.RegisterFunc(L, -1, "totalCount", _s_set_totalCount);
		Utils.RegisterFunc(L, -1, "reverseDirection", _s_set_reverseDirection);
		Utils.RegisterFunc(L, -1, "arabicMirrorDirection", _s_set_arabicMirrorDirection);
		Utils.RegisterFunc(L, -1, "rubberScale", _s_set_rubberScale);
		Utils.RegisterFunc(L, -1, "m_ContentConstraintCount", _s_set_m_ContentConstraintCount);
		Utils.RegisterFunc(L, -1, "onItemMoveIn", _s_set_onItemMoveIn);
		Utils.RegisterFunc(L, -1, "onItemMoveOut", _s_set_onItemMoveOut);
		Utils.RegisterFunc(L, -1, "onBeginDragCallBack", _s_set_onBeginDragCallBack);
		Utils.RegisterFunc(L, -1, "onEndDragCallBack", _s_set_onEndDragCallBack);
		Utils.RegisterFunc(L, -1, "onDragCallBack", _s_set_onDragCallBack);
		Utils.RegisterFunc(L, -1, "onScrollToCellEndCallBack", _s_set_onScrollToCellEndCallBack);
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
				ScrollView o = new ScrollView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ScrollView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearCells(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearCells();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScrollToCell(IntPtr L)
	{
		try
		{
			ScrollView obj = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			float speed = (float)Lua.lua_tonumber(L, 3);
			obj.ScrollToCell(index, speed);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopScrollToCell(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			object obj = objectTranslator.GetObject(L, 2, typeof(object));
			scrollView.StopScrollToCell(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshCells(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshCells();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefillCellsFromEnd(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int offset = Lua.xlua_tointeger(L, 2);
				scrollView.RefillCellsFromEnd(offset);
				return 0;
			}
			if (num == 1)
			{
				scrollView.RefillCellsFromEnd();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ScrollView.RefillCellsFromEnd!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetExtraFillSize(IntPtr L)
	{
		try
		{
			ScrollView obj = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float extraSizeX = (float)Lua.lua_tonumber(L, 2);
			float extraSizeY = (float)Lua.lua_tonumber(L, 3);
			obj.SetExtraFillSize(extraSizeX, extraSizeY);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefillCells(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int offset = Lua.xlua_tointeger(L, 2);
				bool fillViewRect = Lua.lua_toboolean(L, 3);
				scrollView.RefillCells(offset, fillViewRect);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int offset2 = Lua.xlua_tointeger(L, 2);
				scrollView.RefillCells(offset2);
				return 0;
			}
			if (num == 1)
			{
				scrollView.RefillCells();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ScrollView.RefillCells!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			scrollView.Rebuild(v);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
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
			bool value = ((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsActive();
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopMovement();
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollView.OnScroll(data);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollView.OnInitializePotentialDrag(eventData);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollView.OnPointerDown(eventData);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollView.OnBeginDrag(eventData);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollView.OnEndDrag(eventData);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			scrollView.OnDrag(eventData);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutHorizontal();
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScrollView_EndDrag(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ScrollView_EndDrag();
			return 0;
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.content);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushScrollViewMovementType(L, scrollView.movementType);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.elasticity);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollView.inertia);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.decelerationRate);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.scrollSensitivity);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.viewport);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.horizontalScrollbar);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.verticalScrollbar);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushScrollViewScrollbarVisibility(L, scrollView.horizontalScrollbarVisibility);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushScrollViewScrollbarVisibility(L, scrollView.verticalScrollbarVisibility);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.horizontalScrollbarSpacing);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.verticalScrollbarSpacing);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onValueChanged);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, scrollView.velocity);
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, scrollView.normalizedPosition);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.horizontalNormalizedPosition);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.verticalNormalizedPosition);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.minWidth);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.preferredWidth);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.flexibleWidth);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.minHeight);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.preferredHeight);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.flexibleHeight);
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
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, scrollView.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalCount(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, scrollView.totalCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_reverseDirection(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollView.reverseDirection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_arabicMirrorDirection(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scrollView.arabicMirrorDirection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rubberScale(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, scrollView.rubberScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_m_ContentConstraintCount(IntPtr L)
	{
		try
		{
			ScrollView scrollView = (ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, scrollView.m_ContentConstraintCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onItemMoveIn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onItemMoveIn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onItemMoveOut(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onItemMoveOut);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onBeginDragCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onBeginDragCallBack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndDragCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onEndDragCallBack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onDragCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onDragCallBack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onScrollToCellEndCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scrollView.onScrollToCellEndCallBack);
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
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).content = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrollView.MovementType val);
			scrollView.movementType = val;
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).elasticity = (float)Lua.lua_tonumber(L, 2);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).inertia = Lua.lua_toboolean(L, 2);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).decelerationRate = (float)Lua.lua_tonumber(L, 2);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scrollSensitivity = (float)Lua.lua_tonumber(L, 2);
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
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).viewport = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
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
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).horizontalScrollbar = (Scrollbar)objectTranslator.GetObject(L, 2, typeof(Scrollbar));
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
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).verticalScrollbar = (Scrollbar)objectTranslator.GetObject(L, 2, typeof(Scrollbar));
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrollView.ScrollbarVisibility val);
			scrollView.horizontalScrollbarVisibility = val;
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrollView.ScrollbarVisibility val);
			scrollView.verticalScrollbarVisibility = val;
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).horizontalScrollbarSpacing = (float)Lua.lua_tonumber(L, 2);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).verticalScrollbarSpacing = (float)Lua.lua_tonumber(L, 2);
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
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (ScrollView.ScrollRectEvent)objectTranslator.GetObject(L, 2, typeof(ScrollView.ScrollRectEvent));
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollView.velocity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FixedItemSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollView.FixedItemSize = val;
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
			ScrollView scrollView = (ScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			scrollView.normalizedPosition = val;
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).horizontalNormalizedPosition = (float)Lua.lua_tonumber(L, 2);
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
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).verticalNormalizedPosition = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalCount(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_reverseDirection(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).reverseDirection = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_arabicMirrorDirection(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).arabicMirrorDirection = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rubberScale(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rubberScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_m_ContentConstraintCount(IntPtr L)
	{
		try
		{
			((ScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).m_ContentConstraintCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onItemMoveIn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onItemMoveIn = objectTranslator.GetDelegate<ScrollView.MoveItemDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onItemMoveOut(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onItemMoveOut = objectTranslator.GetDelegate<ScrollView.MoveItemDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onBeginDragCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onBeginDragCallBack = objectTranslator.GetDelegate<ScrollView.PointerEventDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndDragCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onEndDragCallBack = objectTranslator.GetDelegate<ScrollView.PointerEventDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onDragCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onDragCallBack = objectTranslator.GetDelegate<ScrollView.PointerEventDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onScrollToCellEndCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScrollView)objectTranslator.FastGetCSObj(L, 1)).onScrollToCellEndCallBack = objectTranslator.GetDelegate<ScrollView.PointerEventDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
