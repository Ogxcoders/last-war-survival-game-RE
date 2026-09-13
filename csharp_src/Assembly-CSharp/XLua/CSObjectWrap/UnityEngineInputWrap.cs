using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineInputWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Input);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 16, 25, 6);
		Utils.RegisterFunc(L, -4, "GetAxis", _m_GetAxis_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAxisRaw", _m_GetAxisRaw_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetButton", _m_GetButton_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetButtonDown", _m_GetButtonDown_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetButtonUp", _m_GetButtonUp_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetMouseButton", _m_GetMouseButton_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetMouseButtonDown", _m_GetMouseButtonDown_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetMouseButtonUp", _m_GetMouseButtonUp_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetInputAxes", _m_ResetInputAxes_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetJoystickNames", _m_GetJoystickNames_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTouch", _m_GetTouch_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAccelerationEvent", _m_GetAccelerationEvent_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetKey", _m_GetKey_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetKeyUp", _m_GetKeyUp_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetKeyDown", _m_GetKeyDown_xlua_st_);
		Utils.RegisterFunc(L, -2, "simulateMouseWithTouches", _g_get_simulateMouseWithTouches);
		Utils.RegisterFunc(L, -2, "anyKey", _g_get_anyKey);
		Utils.RegisterFunc(L, -2, "anyKeyDown", _g_get_anyKeyDown);
		Utils.RegisterFunc(L, -2, "inputString", _g_get_inputString);
		Utils.RegisterFunc(L, -2, "mousePosition", _g_get_mousePosition);
		Utils.RegisterFunc(L, -2, "mouseScrollDelta", _g_get_mouseScrollDelta);
		Utils.RegisterFunc(L, -2, "imeCompositionMode", _g_get_imeCompositionMode);
		Utils.RegisterFunc(L, -2, "compositionString", _g_get_compositionString);
		Utils.RegisterFunc(L, -2, "imeIsSelected", _g_get_imeIsSelected);
		Utils.RegisterFunc(L, -2, "compositionCursorPos", _g_get_compositionCursorPos);
		Utils.RegisterFunc(L, -2, "mousePresent", _g_get_mousePresent);
		Utils.RegisterFunc(L, -2, "touchCount", _g_get_touchCount);
		Utils.RegisterFunc(L, -2, "touchPressureSupported", _g_get_touchPressureSupported);
		Utils.RegisterFunc(L, -2, "stylusTouchSupported", _g_get_stylusTouchSupported);
		Utils.RegisterFunc(L, -2, "touchSupported", _g_get_touchSupported);
		Utils.RegisterFunc(L, -2, "multiTouchEnabled", _g_get_multiTouchEnabled);
		Utils.RegisterFunc(L, -2, "deviceOrientation", _g_get_deviceOrientation);
		Utils.RegisterFunc(L, -2, "acceleration", _g_get_acceleration);
		Utils.RegisterFunc(L, -2, "compensateSensors", _g_get_compensateSensors);
		Utils.RegisterFunc(L, -2, "accelerationEventCount", _g_get_accelerationEventCount);
		Utils.RegisterFunc(L, -2, "backButtonLeavesApp", _g_get_backButtonLeavesApp);
		Utils.RegisterFunc(L, -2, "compass", _g_get_compass);
		Utils.RegisterFunc(L, -2, "gyro", _g_get_gyro);
		Utils.RegisterFunc(L, -2, "touches", _g_get_touches);
		Utils.RegisterFunc(L, -2, "accelerationEvents", _g_get_accelerationEvents);
		Utils.RegisterFunc(L, -1, "simulateMouseWithTouches", _s_set_simulateMouseWithTouches);
		Utils.RegisterFunc(L, -1, "imeCompositionMode", _s_set_imeCompositionMode);
		Utils.RegisterFunc(L, -1, "compositionCursorPos", _s_set_compositionCursorPos);
		Utils.RegisterFunc(L, -1, "multiTouchEnabled", _s_set_multiTouchEnabled);
		Utils.RegisterFunc(L, -1, "compensateSensors", _s_set_compensateSensors);
		Utils.RegisterFunc(L, -1, "backButtonLeavesApp", _s_set_backButtonLeavesApp);
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
				Input o = new Input();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Input constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAxis_xlua_st_(IntPtr L)
	{
		try
		{
			float axis = Input.GetAxis(Lua.lua_tostring(L, 1));
			Lua.lua_pushnumber(L, axis);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAxisRaw_xlua_st_(IntPtr L)
	{
		try
		{
			float axisRaw = Input.GetAxisRaw(Lua.lua_tostring(L, 1));
			Lua.lua_pushnumber(L, axisRaw);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetButton_xlua_st_(IntPtr L)
	{
		try
		{
			bool button = Input.GetButton(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, button);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetButtonDown_xlua_st_(IntPtr L)
	{
		try
		{
			bool buttonDown = Input.GetButtonDown(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, buttonDown);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetButtonUp_xlua_st_(IntPtr L)
	{
		try
		{
			bool buttonUp = Input.GetButtonUp(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, buttonUp);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMouseButton_xlua_st_(IntPtr L)
	{
		try
		{
			bool mouseButton = Input.GetMouseButton(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, mouseButton);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMouseButtonDown_xlua_st_(IntPtr L)
	{
		try
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, mouseButtonDown);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMouseButtonUp_xlua_st_(IntPtr L)
	{
		try
		{
			bool mouseButtonUp = Input.GetMouseButtonUp(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, mouseButtonUp);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetInputAxes_xlua_st_(IntPtr L)
	{
		try
		{
			Input.ResetInputAxes();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetJoystickNames_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string[] joystickNames = Input.GetJoystickNames();
			objectTranslator.Push(L, joystickNames);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTouch_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Touch touch = Input.GetTouch(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, touch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAccelerationEvent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AccelerationEvent accelerationEvent = Input.GetAccelerationEvent(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, accelerationEvent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetKey_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<KeyCode>(L, 1))
			{
				objectTranslator.Get(L, 1, out KeyCode val);
				bool key = Input.GetKey(val);
				Lua.lua_pushboolean(L, key);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				bool key2 = Input.GetKey(Lua.lua_tostring(L, 1));
				Lua.lua_pushboolean(L, key2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Input.GetKey!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetKeyUp_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<KeyCode>(L, 1))
			{
				objectTranslator.Get(L, 1, out KeyCode val);
				bool keyUp = Input.GetKeyUp(val);
				Lua.lua_pushboolean(L, keyUp);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				bool keyUp2 = Input.GetKeyUp(Lua.lua_tostring(L, 1));
				Lua.lua_pushboolean(L, keyUp2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Input.GetKeyUp!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetKeyDown_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<KeyCode>(L, 1))
			{
				objectTranslator.Get(L, 1, out KeyCode val);
				bool keyDown = Input.GetKeyDown(val);
				Lua.lua_pushboolean(L, keyDown);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				bool keyDown2 = Input.GetKeyDown(Lua.lua_tostring(L, 1));
				Lua.lua_pushboolean(L, keyDown2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Input.GetKeyDown!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_simulateMouseWithTouches(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.simulateMouseWithTouches);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anyKey(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.anyKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anyKeyDown(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.anyKeyDown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inputString(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Input.inputString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mousePosition(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Input.mousePosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mouseScrollDelta(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector2(L, Input.mouseScrollDelta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_imeCompositionMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Input.imeCompositionMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_compositionString(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Input.compositionString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_imeIsSelected(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.imeIsSelected);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_compositionCursorPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector2(L, Input.compositionCursorPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mousePresent(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.mousePresent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touchCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Input.touchCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touchPressureSupported(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.touchPressureSupported);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stylusTouchSupported(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.stylusTouchSupported);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touchSupported(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.touchSupported);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_multiTouchEnabled(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.multiTouchEnabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deviceOrientation(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Input.deviceOrientation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_acceleration(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Input.acceleration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_compensateSensors(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.compensateSensors);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_accelerationEventCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Input.accelerationEventCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_backButtonLeavesApp(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Input.backButtonLeavesApp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_compass(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Input.compass);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gyro(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Input.gyro);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touches(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Input.touches);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_accelerationEvents(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Input.accelerationEvents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_simulateMouseWithTouches(IntPtr L)
	{
		try
		{
			Input.simulateMouseWithTouches = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_imeCompositionMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out IMECompositionMode v);
			Input.imeCompositionMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_compositionCursorPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2 val);
			Input.compositionCursorPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_multiTouchEnabled(IntPtr L)
	{
		try
		{
			Input.multiTouchEnabled = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_compensateSensors(IntPtr L)
	{
		try
		{
			Input.compensateSensors = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_backButtonLeavesApp(IntPtr L)
	{
		try
		{
			Input.backButtonLeavesApp = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
