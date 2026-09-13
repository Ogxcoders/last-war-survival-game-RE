using System;
using SuperScrollView;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UITestInputEventWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UITestInputEvent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 1, 1);
		Utils.RegisterFunc(L, -3, "SetEnable", _m_SetEnable);
		Utils.RegisterFunc(L, -3, "IsRaycastLocationValid", _m_IsRaycastLocationValid);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "LateUpdate", _m_LateUpdate);
		Utils.RegisterFunc(L, -2, "loopListView2", _g_get_loopListView2);
		Utils.RegisterFunc(L, -1, "loopListView2", _s_set_loopListView2);
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
				UITestInputEvent o = new UITestInputEvent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UITestInputEvent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetEnable(IntPtr L)
	{
		try
		{
			UITestInputEvent obj = (UITestInputEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool enable = Lua.lua_toboolean(L, 2);
			obj.SetEnable(enable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRaycastLocationValid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UITestInputEvent uITestInputEvent = (UITestInputEvent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Camera eventCamera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = uITestInputEvent.IsRaycastLocationValid(val, eventCamera);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((UITestInputEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LateUpdate(IntPtr L)
	{
		try
		{
			((UITestInputEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LateUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loopListView2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UITestInputEvent uITestInputEvent = (UITestInputEvent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uITestInputEvent.loopListView2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loopListView2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UITestInputEvent)objectTranslator.FastGetCSObj(L, 1)).loopListView2 = (LoopListView2)objectTranslator.GetObject(L, 2, typeof(LoopListView2));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
