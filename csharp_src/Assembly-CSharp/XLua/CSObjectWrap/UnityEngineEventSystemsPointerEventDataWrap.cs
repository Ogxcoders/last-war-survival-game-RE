using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineEventSystemsPointerEventDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PointerEventData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 21, 18);
		Utils.RegisterFunc(L, -3, "IsPointerMoving", _m_IsPointerMoving);
		Utils.RegisterFunc(L, -3, "IsScrolling", _m_IsScrolling);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -2, "pointerEnter", _g_get_pointerEnter);
		Utils.RegisterFunc(L, -2, "lastPress", _g_get_lastPress);
		Utils.RegisterFunc(L, -2, "rawPointerPress", _g_get_rawPointerPress);
		Utils.RegisterFunc(L, -2, "pointerDrag", _g_get_pointerDrag);
		Utils.RegisterFunc(L, -2, "pointerCurrentRaycast", _g_get_pointerCurrentRaycast);
		Utils.RegisterFunc(L, -2, "pointerPressRaycast", _g_get_pointerPressRaycast);
		Utils.RegisterFunc(L, -2, "eligibleForClick", _g_get_eligibleForClick);
		Utils.RegisterFunc(L, -2, "pointerId", _g_get_pointerId);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "delta", _g_get_delta);
		Utils.RegisterFunc(L, -2, "pressPosition", _g_get_pressPosition);
		Utils.RegisterFunc(L, -2, "clickTime", _g_get_clickTime);
		Utils.RegisterFunc(L, -2, "clickCount", _g_get_clickCount);
		Utils.RegisterFunc(L, -2, "scrollDelta", _g_get_scrollDelta);
		Utils.RegisterFunc(L, -2, "useDragThreshold", _g_get_useDragThreshold);
		Utils.RegisterFunc(L, -2, "dragging", _g_get_dragging);
		Utils.RegisterFunc(L, -2, "button", _g_get_button);
		Utils.RegisterFunc(L, -2, "enterEventCamera", _g_get_enterEventCamera);
		Utils.RegisterFunc(L, -2, "pressEventCamera", _g_get_pressEventCamera);
		Utils.RegisterFunc(L, -2, "pointerPress", _g_get_pointerPress);
		Utils.RegisterFunc(L, -2, "hovered", _g_get_hovered);
		Utils.RegisterFunc(L, -1, "pointerEnter", _s_set_pointerEnter);
		Utils.RegisterFunc(L, -1, "rawPointerPress", _s_set_rawPointerPress);
		Utils.RegisterFunc(L, -1, "pointerDrag", _s_set_pointerDrag);
		Utils.RegisterFunc(L, -1, "pointerCurrentRaycast", _s_set_pointerCurrentRaycast);
		Utils.RegisterFunc(L, -1, "pointerPressRaycast", _s_set_pointerPressRaycast);
		Utils.RegisterFunc(L, -1, "eligibleForClick", _s_set_eligibleForClick);
		Utils.RegisterFunc(L, -1, "pointerId", _s_set_pointerId);
		Utils.RegisterFunc(L, -1, "position", _s_set_position);
		Utils.RegisterFunc(L, -1, "delta", _s_set_delta);
		Utils.RegisterFunc(L, -1, "pressPosition", _s_set_pressPosition);
		Utils.RegisterFunc(L, -1, "clickTime", _s_set_clickTime);
		Utils.RegisterFunc(L, -1, "clickCount", _s_set_clickCount);
		Utils.RegisterFunc(L, -1, "scrollDelta", _s_set_scrollDelta);
		Utils.RegisterFunc(L, -1, "useDragThreshold", _s_set_useDragThreshold);
		Utils.RegisterFunc(L, -1, "dragging", _s_set_dragging);
		Utils.RegisterFunc(L, -1, "button", _s_set_button);
		Utils.RegisterFunc(L, -1, "pointerPress", _s_set_pointerPress);
		Utils.RegisterFunc(L, -1, "hovered", _s_set_hovered);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<EventSystem>(L, 2))
			{
				PointerEventData o = new PointerEventData((EventSystem)objectTranslator.GetObject(L, 2, typeof(EventSystem)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.EventSystems.PointerEventData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPointerMoving(IntPtr L)
	{
		try
		{
			bool value = ((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPointerMoving();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsScrolling(IntPtr L)
	{
		try
		{
			bool value = ((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsScrolling();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			string str = ((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToString();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.pointerEnter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastPress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.lastPress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rawPointerPress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.rawPointerPress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.pointerDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerCurrentRaycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.pointerCurrentRaycast);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerPressRaycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.pointerPressRaycast);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eligibleForClick(IntPtr L)
	{
		try
		{
			PointerEventData pointerEventData = (PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, pointerEventData.eligibleForClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerId(IntPtr L)
	{
		try
		{
			PointerEventData pointerEventData = (PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointerEventData.pointerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, pointerEventData.position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_delta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, pointerEventData.delta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pressPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, pointerEventData.pressPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clickTime(IntPtr L)
	{
		try
		{
			PointerEventData pointerEventData = (PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, pointerEventData.clickTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clickCount(IntPtr L)
	{
		try
		{
			PointerEventData pointerEventData = (PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointerEventData.clickCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scrollDelta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, pointerEventData.scrollDelta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useDragThreshold(IntPtr L)
	{
		try
		{
			PointerEventData pointerEventData = (PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, pointerEventData.useDragThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dragging(IntPtr L)
	{
		try
		{
			PointerEventData pointerEventData = (PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, pointerEventData.dragging);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_button(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineEventSystemsPointerEventDataInputButton(L, pointerEventData.button);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enterEventCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.enterEventCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pressEventCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.pressEventCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerPress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.pointerPress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hovered(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointerEventData.hovered);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointerEventData)objectTranslator.FastGetCSObj(L, 1)).pointerEnter = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rawPointerPress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointerEventData)objectTranslator.FastGetCSObj(L, 1)).rawPointerPress = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointerEventData)objectTranslator.FastGetCSObj(L, 1)).pointerDrag = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerCurrentRaycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RaycastResult v);
			pointerEventData.pointerCurrentRaycast = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerPressRaycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RaycastResult v);
			pointerEventData.pointerPressRaycast = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eligibleForClick(IntPtr L)
	{
		try
		{
			((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).eligibleForClick = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerId(IntPtr L)
	{
		try
		{
			((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointerId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			pointerEventData.position = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_delta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			pointerEventData.delta = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pressPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			pointerEventData.pressPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clickTime(IntPtr L)
	{
		try
		{
			((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).clickTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clickCount(IntPtr L)
	{
		try
		{
			((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).clickCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scrollDelta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			pointerEventData.scrollDelta = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useDragThreshold(IntPtr L)
	{
		try
		{
			((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useDragThreshold = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dragging(IntPtr L)
	{
		try
		{
			((PointerEventData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).dragging = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_button(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointerEventData pointerEventData = (PointerEventData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out PointerEventData.InputButton val);
			pointerEventData.button = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerPress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointerEventData)objectTranslator.FastGetCSObj(L, 1)).pointerPress = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hovered(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointerEventData)objectTranslator.FastGetCSObj(L, 1)).hovered = (List<GameObject>)objectTranslator.GetObject(L, 2, typeof(List<GameObject>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
