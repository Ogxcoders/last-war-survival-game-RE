using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class InputFieldTextListenerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(InputFieldTextListener);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 2, 0);
		Utils.RegisterFunc(L, -3, "CheckReplace", _m_CheckReplace);
		Utils.RegisterFunc(L, -3, "SetCustomSetting", _m_SetCustomSetting);
		Utils.RegisterFunc(L, -2, "OnTextInsertEvent", _g_get_OnTextInsertEvent);
		Utils.RegisterFunc(L, -2, "OnTextDeleteEvent", _g_get_OnTextDeleteEvent);
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
				InputFieldTextListener o = new InputFieldTextListener();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to InputFieldTextListener constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckReplace(IntPtr L)
	{
		try
		{
			InputFieldTextListener obj = (InputFieldTextListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string newText = Lua.lua_tostring(L, 2);
			int currentCharLength = Lua.xlua_tointeger(L, 3);
			obj.CheckReplace(newText, currentCharLength);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCustomSetting(IntPtr L)
	{
		try
		{
			InputFieldTextListener obj = (InputFieldTextListener)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool useCompositionLength = Lua.lua_toboolean(L, 2);
			bool useUnicodeLength = Lua.lua_toboolean(L, 3);
			obj.SetCustomSetting(useCompositionLength, useUnicodeLength);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnTextInsertEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputFieldTextListener inputFieldTextListener = (InputFieldTextListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputFieldTextListener.OnTextInsertEvent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnTextDeleteEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputFieldTextListener inputFieldTextListener = (InputFieldTextListener)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputFieldTextListener.OnTextDeleteEvent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
