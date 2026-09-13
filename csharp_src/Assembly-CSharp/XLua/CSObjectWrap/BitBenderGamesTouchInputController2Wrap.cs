using System;
using BitBenderGames;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BitBenderGamesTouchInputController2Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TouchInputController2);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 24, 5, 2);
		Utils.RegisterFunc(L, -3, "Awake", _m_Awake);
		Utils.RegisterFunc(L, -3, "OnEventTriggerPointerDown", _m_OnEventTriggerPointerDown);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "RestartDrag", _m_RestartDrag);
		Utils.RegisterFunc(L, -3, "ClearAllEvent", _m_ClearAllEvent);
		Utils.RegisterFunc(L, -3, "ClearEventOnInputClick", _m_ClearEventOnInputClick);
		Utils.RegisterFunc(L, -3, "SetOnInputClick", _m_SetOnInputClick);
		Utils.RegisterFunc(L, -3, "SetOnDragStart", _m_SetOnDragStart);
		Utils.RegisterFunc(L, -3, "SetOnDragUpdate", _m_SetOnDragUpdate);
		Utils.RegisterFunc(L, -3, "SetOnDragStop", _m_SetOnDragStop);
		Utils.RegisterFunc(L, -3, "SetOnPinchStart", _m_SetOnPinchStart);
		Utils.RegisterFunc(L, -3, "SetOnPinchUpdate", _m_SetOnPinchUpdate);
		Utils.RegisterFunc(L, -3, "SetOnPinchStop", _m_SetOnPinchStop);
		Utils.RegisterFunc(L, -3, "OnDragStart", _e_OnDragStart);
		Utils.RegisterFunc(L, -3, "OnFingerDown", _e_OnFingerDown);
		Utils.RegisterFunc(L, -3, "OnFingerUp", _e_OnFingerUp);
		Utils.RegisterFunc(L, -3, "OnDragUpdate", _e_OnDragUpdate);
		Utils.RegisterFunc(L, -3, "OnDragStop", _e_OnDragStop);
		Utils.RegisterFunc(L, -3, "OnPinchStart", _e_OnPinchStart);
		Utils.RegisterFunc(L, -3, "OnPinchUpdate", _e_OnPinchUpdate);
		Utils.RegisterFunc(L, -3, "OnPinchUpdateExtended", _e_OnPinchUpdateExtended);
		Utils.RegisterFunc(L, -3, "OnPinchStop", _e_OnPinchStop);
		Utils.RegisterFunc(L, -3, "OnLongTapProgress", _e_OnLongTapProgress);
		Utils.RegisterFunc(L, -3, "OnInputClick", _e_OnInputClick);
		Utils.RegisterFunc(L, -2, "IsDragging", _g_get_IsDragging);
		Utils.RegisterFunc(L, -2, "IsPinching", _g_get_IsPinching);
		Utils.RegisterFunc(L, -2, "LongTapStartsDrag", _g_get_LongTapStartsDrag);
		Utils.RegisterFunc(L, -2, "IsInputOnLockedArea", _g_get_IsInputOnLockedArea);
		Utils.RegisterFunc(L, -2, "stopUpdate", _g_get_stopUpdate);
		Utils.RegisterFunc(L, -1, "IsInputOnLockedArea", _s_set_IsInputOnLockedArea);
		Utils.RegisterFunc(L, -1, "stopUpdate", _s_set_stopUpdate);
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
				TouchInputController2 o = new TouchInputController2();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2 constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Awake(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Awake();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEventTriggerPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<GameObject>(L, 2))
			{
				GameObject go = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
				touchInputController.OnEventTriggerPointerDown(go);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<BaseEventData>(L, 2))
			{
				BaseEventData baseEventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
				touchInputController.OnEventTriggerPointerDown(baseEventData);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnEventTriggerPointerDown!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RestartDrag(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RestartDrag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllEvent(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllEvent();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearEventOnInputClick(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearEventOnInputClick();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnInputClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.InputClickDelegate onInputClick = objectTranslator.GetDelegate<TouchInputController2.InputClickDelegate>(L, 2);
			touchInputController.SetOnInputClick(onInputClick);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnDragStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.InputDragStartDelegate onDragStart = objectTranslator.GetDelegate<TouchInputController2.InputDragStartDelegate>(L, 2);
			touchInputController.SetOnDragStart(onDragStart);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnDragUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.DragUpdateDelegate onDragUpdate = objectTranslator.GetDelegate<TouchInputController2.DragUpdateDelegate>(L, 2);
			touchInputController.SetOnDragUpdate(onDragUpdate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnDragStop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.DragStopDelegate onDragStop = objectTranslator.GetDelegate<TouchInputController2.DragStopDelegate>(L, 2);
			touchInputController.SetOnDragStop(onDragStop);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnPinchStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.PinchStartDelegate onPinchStart = objectTranslator.GetDelegate<TouchInputController2.PinchStartDelegate>(L, 2);
			touchInputController.SetOnPinchStart(onPinchStart);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnPinchUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.PinchUpdateDelegate onPinchUpdate = objectTranslator.GetDelegate<TouchInputController2.PinchUpdateDelegate>(L, 2);
			touchInputController.SetOnPinchUpdate(onPinchUpdate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnPinchStop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			Action onPinchStop = objectTranslator.GetDelegate<Action>(L, 2);
			touchInputController.SetOnPinchStop(onPinchStop);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsDragging(IntPtr L)
	{
		try
		{
			TouchInputController2 touchInputController = (TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.IsDragging);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsPinching(IntPtr L)
	{
		try
		{
			TouchInputController2 touchInputController = (TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.IsPinching);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LongTapStartsDrag(IntPtr L)
	{
		try
		{
			TouchInputController2 touchInputController = (TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.LongTapStartsDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInputOnLockedArea(IntPtr L)
	{
		try
		{
			TouchInputController2 touchInputController = (TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.IsInputOnLockedArea);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stopUpdate(IntPtr L)
	{
		try
		{
			TouchInputController2 touchInputController = (TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.stopUpdate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsInputOnLockedArea(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInputOnLockedArea = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stopUpdate(IntPtr L)
	{
		try
		{
			((TouchInputController2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stopUpdate = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnDragStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.InputDragStartDelegate inputDragStartDelegate = objectTranslator.GetDelegate<TouchInputController2.InputDragStartDelegate>(L, 3);
			if (inputDragStartDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.InputDragStartDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnDragStart += inputDragStartDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnDragStart -= inputDragStartDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnDragStart!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnFingerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.Input1PositionDelegate input1PositionDelegate = objectTranslator.GetDelegate<TouchInputController2.Input1PositionDelegate>(L, 3);
			if (input1PositionDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.Input1PositionDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnFingerDown += input1PositionDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnFingerDown -= input1PositionDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnFingerDown!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnFingerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.Input1PositionDelegate input1PositionDelegate = objectTranslator.GetDelegate<TouchInputController2.Input1PositionDelegate>(L, 3);
			if (input1PositionDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.Input1PositionDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnFingerUp += input1PositionDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnFingerUp -= input1PositionDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnFingerUp!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnDragUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.DragUpdateDelegate dragUpdateDelegate = objectTranslator.GetDelegate<TouchInputController2.DragUpdateDelegate>(L, 3);
			if (dragUpdateDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.DragUpdateDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnDragUpdate += dragUpdateDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnDragUpdate -= dragUpdateDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnDragUpdate!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnDragStop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.DragStopDelegate dragStopDelegate = objectTranslator.GetDelegate<TouchInputController2.DragStopDelegate>(L, 3);
			if (dragStopDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.DragStopDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnDragStop += dragStopDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnDragStop -= dragStopDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnDragStop!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.PinchStartDelegate pinchStartDelegate = objectTranslator.GetDelegate<TouchInputController2.PinchStartDelegate>(L, 3);
			if (pinchStartDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.PinchStartDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnPinchStart += pinchStartDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnPinchStart -= pinchStartDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnPinchStart!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.PinchUpdateDelegate pinchUpdateDelegate = objectTranslator.GetDelegate<TouchInputController2.PinchUpdateDelegate>(L, 3);
			if (pinchUpdateDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.PinchUpdateDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnPinchUpdate += pinchUpdateDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnPinchUpdate -= pinchUpdateDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnPinchUpdate!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchUpdateExtended(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.PinchUpdateExtendedDelegate pinchUpdateExtendedDelegate = objectTranslator.GetDelegate<TouchInputController2.PinchUpdateExtendedDelegate>(L, 3);
			if (pinchUpdateExtendedDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.PinchUpdateExtendedDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnPinchUpdateExtended += pinchUpdateExtendedDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnPinchUpdateExtended -= pinchUpdateExtendedDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnPinchUpdateExtended!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchStop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnPinchStop += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnPinchStop -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnPinchStop!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnLongTapProgress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.InputLongTapProgress inputLongTapProgress = objectTranslator.GetDelegate<TouchInputController2.InputLongTapProgress>(L, 3);
			if (inputLongTapProgress == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.InputLongTapProgress!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnLongTapProgress += inputLongTapProgress;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnLongTapProgress -= inputLongTapProgress;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnLongTapProgress!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnInputClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController2 touchInputController = (TouchInputController2)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController2.InputClickDelegate inputClickDelegate = objectTranslator.GetDelegate<TouchInputController2.InputClickDelegate>(L, 3);
			if (inputClickDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController2.InputClickDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnInputClick += inputClickDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnInputClick -= inputClickDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController2.OnInputClick!");
		return 0;
	}
}
