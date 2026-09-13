using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ShowFPSTextWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ShowFPSText);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 1);
		Utils.RegisterFunc(L, -2, "FpsString", _g_get_FpsString);
		Utils.RegisterFunc(L, -2, "MemString", _g_get_MemString);
		Utils.RegisterFunc(L, -2, "GpuSkinString", _g_get_GpuSkinString);
		Utils.RegisterFunc(L, -2, "txt", _g_get_txt);
		Utils.RegisterFunc(L, -1, "txt", _s_set_txt);
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
				ShowFPSText o = new ShowFPSText();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ShowFPSText constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, ShowFPSText.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FpsString(IntPtr L)
	{
		try
		{
			ShowFPSText showFPSText = (ShowFPSText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, showFPSText.FpsString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MemString(IntPtr L)
	{
		try
		{
			ShowFPSText showFPSText = (ShowFPSText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, showFPSText.MemString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GpuSkinString(IntPtr L)
	{
		try
		{
			ShowFPSText showFPSText = (ShowFPSText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, showFPSText.GpuSkinString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShowFPSText showFPSText = (ShowFPSText)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, showFPSText.txt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ShowFPSText)objectTranslator.FastGetCSObj(L, 1)).txt = (TextMeshProUGUI)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUI));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
