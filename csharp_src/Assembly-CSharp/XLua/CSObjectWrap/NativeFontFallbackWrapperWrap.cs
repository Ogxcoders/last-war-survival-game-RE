using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NativeFontFallbackWrapperWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(NativeFontFallbackWrapper);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				NativeFontFallbackWrapper o = new NativeFontFallbackWrapper();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NativeFontFallbackWrapper constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((NativeFontFallbackWrapper)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, NativeFontFallbackWrapper.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
