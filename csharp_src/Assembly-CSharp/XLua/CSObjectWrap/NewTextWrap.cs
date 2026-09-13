using System;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NewTextWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(NewText);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 5, 4);
		Utils.RegisterFunc(L, -2, "oringinalText", _g_get_oringinalText);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "VisibleLines", _g_get_VisibleLines);
		Utils.RegisterFunc(L, -2, "_useTextWithEllipsis", _g_get__useTextWithEllipsis);
		Utils.RegisterFunc(L, -2, "_useTextBestFit", _g_get__useTextBestFit);
		Utils.RegisterFunc(L, -1, "oringinalText", _s_set_oringinalText);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "_useTextWithEllipsis", _s_set__useTextWithEllipsis);
		Utils.RegisterFunc(L, -1, "_useTextBestFit", _s_set__useTextBestFit);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "GetArabicSupportLinesInfo", _m_GetArabicSupportLinesInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "PopulateWithErrors", _m_PopulateWithErrors_xlua_st_);
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
				NewText o = new NewText();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NewText constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetArabicSupportLinesInfo_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextGenerator generator = (TextGenerator)objectTranslator.GetObject(L, 1, typeof(TextGenerator));
			string input = Lua.lua_tostring(L, 2);
			UGUITextLines[] arabicSupportLinesInfo = NewText.GetArabicSupportLinesInfo(generator, input);
			objectTranslator.Push(L, arabicSupportLinesInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PopulateWithErrors_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.GetObject(L, 1, typeof(Text));
			string finalTxt = Lua.lua_tostring(L, 2);
			UGUITextLines[] o = NewText.PopulateWithErrors(text, finalTxt);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_oringinalText(IntPtr L)
	{
		try
		{
			NewText newText = (NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, newText.oringinalText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_text(IntPtr L)
	{
		try
		{
			NewText newText = (NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, newText.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VisibleLines(IntPtr L)
	{
		try
		{
			NewText newText = (NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, newText.VisibleLines);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__useTextWithEllipsis(IntPtr L)
	{
		try
		{
			NewText newText = (NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, newText._useTextWithEllipsis);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__useTextBestFit(IntPtr L)
	{
		try
		{
			NewText newText = (NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, newText._useTextBestFit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_oringinalText(IntPtr L)
	{
		try
		{
			((NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).oringinalText = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_text(IntPtr L)
	{
		try
		{
			((NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__useTextWithEllipsis(IntPtr L)
	{
		try
		{
			((NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._useTextWithEllipsis = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__useTextBestFit(IntPtr L)
	{
		try
		{
			((NewText)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._useTextBestFit = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
