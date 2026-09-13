using System;
using BitBenderGames;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BitBenderGamesTouchInputControllerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TouchInputController);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 15, 3, 2);
		Utils.RegisterFunc(L, -3, "OnEventTriggerPointerDown", _m_OnEventTriggerPointerDown);
		Utils.RegisterFunc(L, -3, "GetFingerDownPosition", _m_GetFingerDownPosition);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "RestartDrag", _m_RestartDrag);
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
		Utils.RegisterFunc(L, -2, "LongTapStartsDrag", _g_get_LongTapStartsDrag);
		Utils.RegisterFunc(L, -2, "IsInputOnLockedArea", _g_get_IsInputOnLockedArea);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -1, "IsInputOnLockedArea", _s_set_IsInputOnLockedArea);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
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
				TouchInputController o = new TouchInputController();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEventTriggerPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnEventTriggerPointerDown!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFingerDownPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 fingerDownPosition = ((TouchInputController)objectTranslator.FastGetCSObj(L, 1)).GetFingerDownPosition();
			objectTranslator.PushUnityEngineVector3(L, fingerDownPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			((TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnUpdate();
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
			((TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RestartDrag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LongTapStartsDrag(IntPtr L)
	{
		try
		{
			TouchInputController touchInputController = (TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			TouchInputController touchInputController = (TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.IsInputOnLockedArea);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			TouchInputController touchInputController = (TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchInputController.enabled);
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
			((TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInputOnLockedArea = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enabled(IntPtr L)
	{
		try
		{
			((TouchInputController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enabled = Lua.lua_toboolean(L, 2);
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
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.InputDragStartDelegate inputDragStartDelegate = objectTranslator.GetDelegate<TouchInputController.InputDragStartDelegate>(L, 3);
			if (inputDragStartDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.InputDragStartDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnDragStart!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnFingerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.Input1PositionDelegate input1PositionDelegate = objectTranslator.GetDelegate<TouchInputController.Input1PositionDelegate>(L, 3);
			if (input1PositionDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.Input1PositionDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnFingerDown!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnFingerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					touchInputController.OnFingerUp += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					touchInputController.OnFingerUp -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnFingerUp!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnDragUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.DragUpdateDelegate dragUpdateDelegate = objectTranslator.GetDelegate<TouchInputController.DragUpdateDelegate>(L, 3);
			if (dragUpdateDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.DragUpdateDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnDragUpdate!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnDragStop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.DragStopDelegate dragStopDelegate = objectTranslator.GetDelegate<TouchInputController.DragStopDelegate>(L, 3);
			if (dragStopDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.DragStopDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnDragStop!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.PinchStartDelegate pinchStartDelegate = objectTranslator.GetDelegate<TouchInputController.PinchStartDelegate>(L, 3);
			if (pinchStartDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.PinchStartDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchStart!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.PinchUpdateDelegate pinchUpdateDelegate = objectTranslator.GetDelegate<TouchInputController.PinchUpdateDelegate>(L, 3);
			if (pinchUpdateDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.PinchUpdateDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchUpdate!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchUpdateExtended(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.PinchUpdateExtendedDelegate pinchUpdateExtendedDelegate = objectTranslator.GetDelegate<TouchInputController.PinchUpdateExtendedDelegate>(L, 3);
			if (pinchUpdateExtendedDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.PinchUpdateExtendedDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchUpdateExtended!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPinchStop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchStop!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnLongTapProgress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.InputLongTapProgress inputLongTapProgress = objectTranslator.GetDelegate<TouchInputController.InputLongTapProgress>(L, 3);
			if (inputLongTapProgress == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.InputLongTapProgress!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnLongTapProgress!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnInputClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TouchInputController touchInputController = (TouchInputController)objectTranslator.FastGetCSObj(L, 1);
			TouchInputController.InputClickDelegate inputClickDelegate = objectTranslator.GetDelegate<TouchInputController.InputClickDelegate>(L, 3);
			if (inputClickDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.TouchInputController.InputClickDelegate!");
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
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnInputClick!");
		return 0;
	}
}
