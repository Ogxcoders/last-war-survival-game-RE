using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LocalizationManagerDialogEntryWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LocalizationManager.DialogEntry);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 2, 2);
		Utils.RegisterFunc(L, -2, "originDlg", _g_get_originDlg);
		Utils.RegisterFunc(L, -2, "hasCRLF", _g_get_hasCRLF);
		Utils.RegisterFunc(L, -1, "originDlg", _s_set_originDlg);
		Utils.RegisterFunc(L, -1, "hasCRLF", _s_set_hasCRLF);
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
				LocalizationManager.DialogEntry o = new LocalizationManager.DialogEntry();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LocalizationManager.DialogEntry constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_originDlg(IntPtr L)
	{
		try
		{
			LocalizationManager.DialogEntry dialogEntry = (LocalizationManager.DialogEntry)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, dialogEntry.originDlg);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasCRLF(IntPtr L)
	{
		try
		{
			LocalizationManager.DialogEntry dialogEntry = (LocalizationManager.DialogEntry)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dialogEntry.hasCRLF);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_originDlg(IntPtr L)
	{
		try
		{
			((LocalizationManager.DialogEntry)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).originDlg = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hasCRLF(IntPtr L)
	{
		try
		{
			((LocalizationManager.DialogEntry)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).hasCRLF = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
