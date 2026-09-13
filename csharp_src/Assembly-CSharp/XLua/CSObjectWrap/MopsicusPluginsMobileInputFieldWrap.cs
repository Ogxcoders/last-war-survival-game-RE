using System;
using System.Collections.Generic;
using Mopsicus.Plugins;
using NiceJson;
using UnityEngine;
using UnityEngine.Events;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MopsicusPluginsMobileInputFieldWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MobileInputField);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 27, 17, 13);
		Utils.RegisterFunc(L, -3, "GetMobilId", _m_GetMobilId);
		Utils.RegisterFunc(L, -3, "InsertTextAndScroll", _m_InsertTextAndScroll);
		Utils.RegisterFunc(L, -3, "DeleteButtonClick", _m_DeleteButtonClick);
		Utils.RegisterFunc(L, -3, "SetUnityInputEnabled", _m_SetUnityInputEnabled);
		Utils.RegisterFunc(L, -3, "SetMaxLine", _m_SetMaxLine);
		Utils.RegisterFunc(L, -3, "Send", _m_Send);
		Utils.RegisterFunc(L, -3, "Hide", _m_Hide);
		Utils.RegisterFunc(L, -3, "SetIngoreFocus", _m_SetIngoreFocus);
		Utils.RegisterFunc(L, -3, "SetRectNative", _m_SetRectNative);
		Utils.RegisterFunc(L, -3, "SetFocus", _m_SetFocus);
		Utils.RegisterFunc(L, -3, "SetVisible", _m_SetVisible);
		Utils.RegisterFunc(L, -3, "SetMobilTextColor", _m_SetMobilTextColor);
		Utils.RegisterFunc(L, -3, "SetMobilBackGroundColor", _m_SetMobilBackGroundColor);
		Utils.RegisterFunc(L, -3, "SetMobilPlaceholderColor", _m_SetMobilPlaceholderColor);
		Utils.RegisterFunc(L, -3, "SetColor", _m_SetColor);
		Utils.RegisterFunc(L, -3, "IsUnlockAt", _m_IsUnlockAt);
		Utils.RegisterFunc(L, -3, "IsUnlockPredict", _m_IsUnlockPredict);
		Utils.RegisterFunc(L, -3, "GetFunctionFlags", _m_GetFunctionFlags);
		Utils.RegisterFunc(L, -3, "IsUnlockEmojiInput", _m_IsUnlockEmojiInput);
		Utils.RegisterFunc(L, -3, "IsUnSupportMultiple", _m_IsUnSupportMultiple);
		Utils.RegisterFunc(L, -3, "IsUnCustomMobilColor", _m_IsUnCustomMobilColor);
		Utils.RegisterFunc(L, -3, "IsUnlockSuitableInputField", _m_IsUnlockSuitableInputField);
		Utils.RegisterFunc(L, -3, "SetSelection", _m_SetSelection);
		Utils.RegisterFunc(L, -3, "GetSelection", _m_GetSelection);
		Utils.RegisterFunc(L, -3, "InsertMention", _m_InsertMention);
		Utils.RegisterFunc(L, -3, "ResetMentions", _m_ResetMentions);
		Utils.RegisterFunc(L, -3, "SetIsDefaultVisibleOnEnable", _m_SetIsDefaultVisibleOnEnable);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -2, "InputField", _g_get_InputField);
		Utils.RegisterFunc(L, -2, "TmproInputField", _g_get_TmproInputField);
		Utils.RegisterFunc(L, -2, "Visible", _g_get_Visible);
		Utils.RegisterFunc(L, -2, "Text", _g_get_Text);
		Utils.RegisterFunc(L, -2, "OnTextChangeFromPlatform", _g_get_OnTextChangeFromPlatform);
		Utils.RegisterFunc(L, -2, "CustomFont", _g_get_CustomFont);
		Utils.RegisterFunc(L, -2, "CustomFontDir", _g_get_CustomFontDir);
		Utils.RegisterFunc(L, -2, "IsManualHideControl", _g_get_IsManualHideControl);
		Utils.RegisterFunc(L, -2, "IsWithDoneButton", _g_get_IsWithDoneButton);
		Utils.RegisterFunc(L, -2, "IsWithClearButton", _g_get_IsWithClearButton);
		Utils.RegisterFunc(L, -2, "ReturnKey", _g_get_ReturnKey);
		Utils.RegisterFunc(L, -2, "OnReturnPressed", _g_get_OnReturnPressed);
		Utils.RegisterFunc(L, -2, "OnFocusChanged", _g_get_OnFocusChanged);
		Utils.RegisterFunc(L, -2, "OnReturnPressedEvent", _g_get_OnReturnPressedEvent);
		Utils.RegisterFunc(L, -2, "lineCount", _g_get_lineCount);
		Utils.RegisterFunc(L, -2, "charHeight", _g_get_charHeight);
		Utils.RegisterFunc(L, -1, "Text", _s_set_Text);
		Utils.RegisterFunc(L, -1, "OnTextChangeFromPlatform", _s_set_OnTextChangeFromPlatform);
		Utils.RegisterFunc(L, -1, "CustomFont", _s_set_CustomFont);
		Utils.RegisterFunc(L, -1, "CustomFontDir", _s_set_CustomFontDir);
		Utils.RegisterFunc(L, -1, "IsManualHideControl", _s_set_IsManualHideControl);
		Utils.RegisterFunc(L, -1, "IsWithDoneButton", _s_set_IsWithDoneButton);
		Utils.RegisterFunc(L, -1, "IsWithClearButton", _s_set_IsWithClearButton);
		Utils.RegisterFunc(L, -1, "ReturnKey", _s_set_ReturnKey);
		Utils.RegisterFunc(L, -1, "OnReturnPressed", _s_set_OnReturnPressed);
		Utils.RegisterFunc(L, -1, "OnFocusChanged", _s_set_OnFocusChanged);
		Utils.RegisterFunc(L, -1, "OnReturnPressedEvent", _s_set_OnReturnPressedEvent);
		Utils.RegisterFunc(L, -1, "lineCount", _s_set_lineCount);
		Utils.RegisterFunc(L, -1, "charHeight", _s_set_charHeight);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "GetScreenRectFromRectTransform", _m_GetScreenRectFromRectTransform_xlua_st_);
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
				MobileInputField o = new MobileInputField();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Mopsicus.Plugins.MobileInputField constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMobilId(IntPtr L)
	{
		try
		{
			int mobilId = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMobilId();
			Lua.xlua_pushinteger(L, mobilId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertTextAndScroll(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string text = Lua.lua_tostring(L, 2);
				bool isUnescape = Lua.lua_toboolean(L, 3);
				mobileInputField.InsertTextAndScroll(text, isUnescape);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string text2 = Lua.lua_tostring(L, 2);
				mobileInputField.InsertTextAndScroll(text2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Mopsicus.Plugins.MobileInputField.InsertTextAndScroll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteButtonClick(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DeleteButtonClick();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUnityInputEnabled(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool unityInputEnabled = Lua.lua_toboolean(L, 2);
			obj.SetUnityInputEnabled(unityInputEnabled);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaxLine(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int maxLine = Lua.xlua_tointeger(L, 2);
			obj.SetMaxLine(maxLine);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetScreenRectFromRectTransform_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rect screenRectFromRectTransform = MobileInputField.GetScreenRectFromRectTransform((RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform)));
			objectTranslator.Push(L, screenRectFromRectTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Send(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			JsonObject data = (JsonObject)objectTranslator.GetObject(L, 2, typeof(JsonObject));
			mobileInputField.Send(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Hide(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Hide();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIngoreFocus(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool ingoreFocus = Lua.lua_toboolean(L, 2);
			obj.SetIngoreFocus(ingoreFocus);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRectNative(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			RectTransform rectNative = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
			mobileInputField.SetRectNative(rectNative);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFocus(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool focus = Lua.lua_toboolean(L, 2);
			obj.SetFocus(focus);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisible(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool visible = Lua.lua_toboolean(L, 2);
			obj.SetVisible(visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMobilTextColor(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			obj.SetMobilTextColor(r, g, b, a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMobilBackGroundColor(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			obj.SetMobilBackGroundColor(r, g, b, a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMobilPlaceholderColor(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			obj.SetMobilPlaceholderColor(r, g, b, a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColor(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			string componentType = Lua.lua_tostring(L, 6);
			obj.SetColor(r, g, b, a, componentType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnlockAt(IntPtr L)
	{
		try
		{
			bool value = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUnlockAt();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnlockPredict(IntPtr L)
	{
		try
		{
			bool value = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUnlockPredict();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFunctionFlags(IntPtr L)
	{
		try
		{
			int functionFlags = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFunctionFlags();
			Lua.xlua_pushinteger(L, functionFlags);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnlockEmojiInput(IntPtr L)
	{
		try
		{
			bool value = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUnlockEmojiInput();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnSupportMultiple(IntPtr L)
	{
		try
		{
			bool value = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUnSupportMultiple();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnCustomMobilColor(IntPtr L)
	{
		try
		{
			bool value = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUnCustomMobilColor();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnlockSuitableInputField(IntPtr L)
	{
		try
		{
			bool value = ((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUnlockSuitableInputField();
			Lua.lua_pushboolean(L, value);
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
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RangeInt v);
			mobileInputField.SetSelection(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RangeInt selection = ((MobileInputField)objectTranslator.FastGetCSObj(L, 1)).GetSelection();
			objectTranslator.Push(L, selection);
			return 1;
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
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			List<MobileInputField.Mention> list = (List<MobileInputField.Mention>)objectTranslator.GetObject(L, 2, typeof(List<MobileInputField.Mention>));
			mobileInputField.ResetMentions(list);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsDefaultVisibleOnEnable(IntPtr L)
	{
		try
		{
			MobileInputField obj = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isDefaultVisibleOnEnable = Lua.lua_toboolean(L, 2);
			obj.SetIsDefaultVisibleOnEnable(isDefaultVisibleOnEnable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_InputField(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.InputField);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TmproInputField(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.TmproInputField);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Visible(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileInputField.Visible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Text(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, mobileInputField.Text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnTextChangeFromPlatform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.OnTextChangeFromPlatform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CustomFont(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, mobileInputField.CustomFont);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CustomFontDir(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, mobileInputField.CustomFontDir);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsManualHideControl(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileInputField.IsManualHideControl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsWithDoneButton(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileInputField.IsWithDoneButton);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsWithClearButton(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileInputField.IsWithClearButton);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ReturnKey(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.ReturnKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnReturnPressed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.OnReturnPressed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnFocusChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.OnFocusChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnReturnPressedEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileInputField.OnReturnPressedEvent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineCount(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mobileInputField.lineCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_charHeight(IntPtr L)
	{
		try
		{
			MobileInputField mobileInputField = (MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileInputField.charHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Text(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnTextChangeFromPlatform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileInputField)objectTranslator.FastGetCSObj(L, 1)).OnTextChangeFromPlatform = objectTranslator.GetDelegate<Action<string>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CustomFont(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CustomFont = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CustomFontDir(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CustomFontDir = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsManualHideControl(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsManualHideControl = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsWithDoneButton(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWithDoneButton = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsWithClearButton(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWithClearButton = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ReturnKey(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileInputField mobileInputField = (MobileInputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MobileInputField.ReturnKeyType v);
			mobileInputField.ReturnKey = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnReturnPressed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileInputField)objectTranslator.FastGetCSObj(L, 1)).OnReturnPressed = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnFocusChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileInputField)objectTranslator.FastGetCSObj(L, 1)).OnFocusChanged = objectTranslator.GetDelegate<Action<bool>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnReturnPressedEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileInputField)objectTranslator.FastGetCSObj(L, 1)).OnReturnPressedEvent = (UnityEvent)objectTranslator.GetObject(L, 2, typeof(UnityEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineCount(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_charHeight(IntPtr L)
	{
		try
		{
			((MobileInputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).charHeight = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
