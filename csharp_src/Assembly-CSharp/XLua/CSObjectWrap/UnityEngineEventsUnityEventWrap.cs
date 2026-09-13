using System;
using UnityEngine.Events;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineEventsUnityEventWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityEvent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 0, 0);
		Utils.RegisterFunc(L, -3, "AddListener", _m_AddListener);
		Utils.RegisterFunc(L, -3, "RemoveListener", _m_RemoveListener);
		Utils.RegisterFunc(L, -3, "Invoke", _m_Invoke);
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
				UnityEvent o = new UnityEvent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Events.UnityEvent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEvent unityEvent = (UnityEvent)objectTranslator.FastGetCSObj(L, 1);
			UnityAction call = objectTranslator.GetDelegate<UnityAction>(L, 2);
			unityEvent.AddListener(call);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEvent unityEvent = (UnityEvent)objectTranslator.FastGetCSObj(L, 1);
			UnityAction call = objectTranslator.GetDelegate<UnityAction>(L, 2);
			unityEvent.RemoveListener(call);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Invoke(IntPtr L)
	{
		try
		{
			((UnityEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Invoke();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
