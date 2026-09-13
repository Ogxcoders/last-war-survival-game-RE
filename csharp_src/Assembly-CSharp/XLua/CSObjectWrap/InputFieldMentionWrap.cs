using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class InputFieldMentionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(InputFieldMention);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 0, 0);
		Utils.RegisterFunc(L, -3, "GetSelection", _m_GetSelection);
		Utils.RegisterFunc(L, -3, "SetSelection", _m_SetSelection);
		Utils.RegisterFunc(L, -3, "SetTextColorRange", _m_SetTextColorRange);
		Utils.RegisterFunc(L, -3, "InsertMention", _m_InsertMention);
		Utils.RegisterFunc(L, -3, "ResetMentions", _m_ResetMentions);
		Utils.RegisterFunc(L, -3, "InsertTextAtCaret", _m_InsertTextAtCaret);
		Utils.RegisterFunc(L, -3, "ClearMentions", _m_ClearMentions);
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
				InputFieldMention o = new InputFieldMention();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to InputFieldMention constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RangeInt selection = ((InputFieldMention)objectTranslator.FastGetCSObj(L, 1)).GetSelection();
			objectTranslator.Push(L, selection);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputFieldMention inputFieldMention = (InputFieldMention)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RangeInt v);
			inputFieldMention.SetSelection(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTextColorRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputFieldMention inputFieldMention = (InputFieldMention)objectTranslator.FastGetCSObj(L, 1);
			List<MentionInfo> textColorRange = (List<MentionInfo>)objectTranslator.GetObject(L, 2, typeof(List<MentionInfo>));
			inputFieldMention.SetTextColorRange(textColorRange);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertMention(IntPtr L)
	{
		try
		{
			InputFieldMention obj = (InputFieldMention)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string text = Lua.lua_tostring(L, 2);
			int offset = Lua.xlua_tointeger(L, 3);
			string colorStr = Lua.lua_tostring(L, 4);
			int index = Lua.xlua_tointeger(L, 5);
			obj.InsertMention(text, offset, colorStr, index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetMentions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputFieldMention inputFieldMention = (InputFieldMention)objectTranslator.FastGetCSObj(L, 1);
			List<MentionInfo> list = (List<MentionInfo>)objectTranslator.GetObject(L, 2, typeof(List<MentionInfo>));
			inputFieldMention.ResetMentions(list);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertTextAtCaret(IntPtr L)
	{
		try
		{
			InputFieldMention obj = (InputFieldMention)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string textToInsert = Lua.lua_tostring(L, 2);
			int offset = Lua.xlua_tointeger(L, 3);
			obj.InsertTextAtCaret(textToInsert, offset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearMentions(IntPtr L)
	{
		try
		{
			((InputFieldMention)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearMentions();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
