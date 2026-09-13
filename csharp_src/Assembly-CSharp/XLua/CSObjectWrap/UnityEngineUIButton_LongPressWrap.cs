using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIButton_LongPressWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Button_LongPress);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 1, 1);
		Utils.RegisterFunc(L, -3, "SetTouchBgGray", _m_SetTouchBgGray);
		Utils.RegisterFunc(L, -3, "SetLongPressAction", _m_SetLongPressAction);
		Utils.RegisterFunc(L, -3, "SetClickAction", _m_SetClickAction);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnPointerUp", _m_OnPointerUp);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -2, "pressDurationTime", _g_get_pressDurationTime);
		Utils.RegisterFunc(L, -1, "pressDurationTime", _s_set_pressDurationTime);
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
				Button_LongPress o = new Button_LongPress();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Button_LongPress constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTouchBgGray(IntPtr L)
	{
		try
		{
			Button_LongPress obj = (Button_LongPress)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool touchBgGray = Lua.lua_toboolean(L, 2);
			obj.SetTouchBgGray(touchBgGray);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLongPressAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Button_LongPress button_LongPress = (Button_LongPress)objectTranslator.FastGetCSObj(L, 1);
			Action longPressAction = objectTranslator.GetDelegate<Action>(L, 2);
			button_LongPress.SetLongPressAction(longPressAction);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetClickAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Button_LongPress button_LongPress = (Button_LongPress)objectTranslator.FastGetCSObj(L, 1);
			Action clickAction = objectTranslator.GetDelegate<Action>(L, 2);
			button_LongPress.SetClickAction(clickAction);
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
			Button_LongPress button_LongPress = (Button_LongPress)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			button_LongPress.OnPointerDown(eventData);
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
			Button_LongPress button_LongPress = (Button_LongPress)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			button_LongPress.OnPointerUp(eventData);
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
			Button_LongPress button_LongPress = (Button_LongPress)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			button_LongPress.OnPointerClick(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pressDurationTime(IntPtr L)
	{
		try
		{
			Button_LongPress button_LongPress = (Button_LongPress)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, button_LongPress.pressDurationTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pressDurationTime(IntPtr L)
	{
		try
		{
			((Button_LongPress)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pressDurationTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
