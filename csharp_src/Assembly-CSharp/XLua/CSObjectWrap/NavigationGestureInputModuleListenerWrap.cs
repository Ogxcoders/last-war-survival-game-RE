using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NavigationGestureInputModuleListenerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(NavigationGestureInputModuleListener);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 1, 0);
		Utils.RegisterFunc(L, -2, "inputModule", _g_get_inputModule);
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
				NavigationGestureInputModuleListener o = new NavigationGestureInputModuleListener();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NavigationGestureInputModuleListener constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inputModule(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NavigationGestureInputModuleListener navigationGestureInputModuleListener = (NavigationGestureInputModuleListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, navigationGestureInputModuleListener.inputModule);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
