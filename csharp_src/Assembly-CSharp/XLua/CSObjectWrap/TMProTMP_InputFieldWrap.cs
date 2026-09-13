using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_InputField);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 30, 63, 52);
		Utils.RegisterFunc(L, -3, "GetCompositionLength", _m_GetCompositionLength);
		Utils.RegisterFunc(L, -3, "SetTextWithoutNotify", _m_SetTextWithoutNotify);
		Utils.RegisterFunc(L, -3, "MoveTextEnd", _m_MoveTextEnd);
		Utils.RegisterFunc(L, -3, "MoveTextStart", _m_MoveTextStart);
		Utils.RegisterFunc(L, -3, "MoveToEndOfLine", _m_MoveToEndOfLine);
		Utils.RegisterFunc(L, -3, "MoveToStartOfLine", _m_MoveToStartOfLine);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "ProcessEvent", _m_ProcessEvent);
		Utils.RegisterFunc(L, -3, "OnUpdateSelected", _m_OnUpdateSelected);
		Utils.RegisterFunc(L, -3, "OnScroll", _m_OnScroll);
		Utils.RegisterFunc(L, -3, "ForceLabelUpdate", _m_ForceLabelUpdate);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "ActivateInputField", _m_ActivateInputField);
		Utils.RegisterFunc(L, -3, "OnSelect", _m_OnSelect);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnControlClick", _m_OnControlClick);
		Utils.RegisterFunc(L, -3, "ReleaseSelection", _m_ReleaseSelection);
		Utils.RegisterFunc(L, -3, "DeactivateInputField", _m_DeactivateInputField);
		Utils.RegisterFunc(L, -3, "OnDeselect", _m_OnDeselect);
		Utils.RegisterFunc(L, -3, "OnSubmit", _m_OnSubmit);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetGlobalPointSize", _m_SetGlobalPointSize);
		Utils.RegisterFunc(L, -3, "SetGlobalFontAsset", _m_SetGlobalFontAsset);
		Utils.RegisterFunc(L, -3, "SetLocalText", _m_SetLocalText);
		Utils.RegisterFunc(L, -2, "OnPaste", _g_get_OnPaste);
		Utils.RegisterFunc(L, -2, "UseNativeKeyboard", _g_get_UseNativeKeyboard);
		Utils.RegisterFunc(L, -2, "NativeSelection", _g_get_NativeSelection);
		Utils.RegisterFunc(L, -2, "shouldHideMobileInput", _g_get_shouldHideMobileInput);
		Utils.RegisterFunc(L, -2, "shouldHideSoftKeyboard", _g_get_shouldHideSoftKeyboard);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "isFocused", _g_get_isFocused);
		Utils.RegisterFunc(L, -2, "caretBlinkRate", _g_get_caretBlinkRate);
		Utils.RegisterFunc(L, -2, "caretWidth", _g_get_caretWidth);
		Utils.RegisterFunc(L, -2, "textViewport", _g_get_textViewport);
		Utils.RegisterFunc(L, -2, "textComponent", _g_get_textComponent);
		Utils.RegisterFunc(L, -2, "placeholder", _g_get_placeholder);
		Utils.RegisterFunc(L, -2, "verticalScrollbar", _g_get_verticalScrollbar);
		Utils.RegisterFunc(L, -2, "scrollSensitivity", _g_get_scrollSensitivity);
		Utils.RegisterFunc(L, -2, "caretColor", _g_get_caretColor);
		Utils.RegisterFunc(L, -2, "customCaretColor", _g_get_customCaretColor);
		Utils.RegisterFunc(L, -2, "selectionColor", _g_get_selectionColor);
		Utils.RegisterFunc(L, -2, "onEndEdit", _g_get_onEndEdit);
		Utils.RegisterFunc(L, -2, "onSubmit", _g_get_onSubmit);
		Utils.RegisterFunc(L, -2, "onSelect", _g_get_onSelect);
		Utils.RegisterFunc(L, -2, "onDeselect", _g_get_onDeselect);
		Utils.RegisterFunc(L, -2, "onTextSelection", _g_get_onTextSelection);
		Utils.RegisterFunc(L, -2, "onEndTextSelection", _g_get_onEndTextSelection);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -2, "onPressEnter", _g_get_onPressEnter);
		Utils.RegisterFunc(L, -2, "onTouchScreenKeyboardStatusChanged", _g_get_onTouchScreenKeyboardStatusChanged);
		Utils.RegisterFunc(L, -2, "onValidateInput", _g_get_onValidateInput);
		Utils.RegisterFunc(L, -2, "characterLimit", _g_get_characterLimit);
		Utils.RegisterFunc(L, -2, "pointSize", _g_get_pointSize);
		Utils.RegisterFunc(L, -2, "fontAsset", _g_get_fontAsset);
		Utils.RegisterFunc(L, -2, "onFocusSelectAll", _g_get_onFocusSelectAll);
		Utils.RegisterFunc(L, -2, "resetOnDeActivation", _g_get_resetOnDeActivation);
		Utils.RegisterFunc(L, -2, "restoreOriginalTextOnEscape", _g_get_restoreOriginalTextOnEscape);
		Utils.RegisterFunc(L, -2, "isRichTextEditingAllowed", _g_get_isRichTextEditingAllowed);
		Utils.RegisterFunc(L, -2, "contentType", _g_get_contentType);
		Utils.RegisterFunc(L, -2, "lineType", _g_get_lineType);
		Utils.RegisterFunc(L, -2, "lineLimit", _g_get_lineLimit);
		Utils.RegisterFunc(L, -2, "inputType", _g_get_inputType);
		Utils.RegisterFunc(L, -2, "keyboardType", _g_get_keyboardType);
		Utils.RegisterFunc(L, -2, "characterValidation", _g_get_characterValidation);
		Utils.RegisterFunc(L, -2, "inputValidator", _g_get_inputValidator);
		Utils.RegisterFunc(L, -2, "readOnly", _g_get_readOnly);
		Utils.RegisterFunc(L, -2, "richText", _g_get_richText);
		Utils.RegisterFunc(L, -2, "multiLine", _g_get_multiLine);
		Utils.RegisterFunc(L, -2, "asteriskChar", _g_get_asteriskChar);
		Utils.RegisterFunc(L, -2, "wasCanceled", _g_get_wasCanceled);
		Utils.RegisterFunc(L, -2, "caretPosition", _g_get_caretPosition);
		Utils.RegisterFunc(L, -2, "selectionAnchorPosition", _g_get_selectionAnchorPosition);
		Utils.RegisterFunc(L, -2, "selectionFocusPosition", _g_get_selectionFocusPosition);
		Utils.RegisterFunc(L, -2, "stringPosition", _g_get_stringPosition);
		Utils.RegisterFunc(L, -2, "selectionStringAnchorPosition", _g_get_selectionStringAnchorPosition);
		Utils.RegisterFunc(L, -2, "selectionStringFocusPosition", _g_get_selectionStringFocusPosition);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -2, "OpenNativeKeyboard", _g_get_OpenNativeKeyboard);
		Utils.RegisterFunc(L, -2, "SetNativeKeyboardSelection", _g_get_SetNativeKeyboardSelection);
		Utils.RegisterFunc(L, -2, "GetNativeKeyboardSelection", _g_get_GetNativeKeyboardSelection);
		Utils.RegisterFunc(L, -2, "isOnPressEnterRegistered", _g_get_isOnPressEnterRegistered);
		Utils.RegisterFunc(L, -1, "OnPaste", _s_set_OnPaste);
		Utils.RegisterFunc(L, -1, "NativeSelection", _s_set_NativeSelection);
		Utils.RegisterFunc(L, -1, "shouldHideMobileInput", _s_set_shouldHideMobileInput);
		Utils.RegisterFunc(L, -1, "shouldHideSoftKeyboard", _s_set_shouldHideSoftKeyboard);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "caretBlinkRate", _s_set_caretBlinkRate);
		Utils.RegisterFunc(L, -1, "caretWidth", _s_set_caretWidth);
		Utils.RegisterFunc(L, -1, "textViewport", _s_set_textViewport);
		Utils.RegisterFunc(L, -1, "textComponent", _s_set_textComponent);
		Utils.RegisterFunc(L, -1, "placeholder", _s_set_placeholder);
		Utils.RegisterFunc(L, -1, "verticalScrollbar", _s_set_verticalScrollbar);
		Utils.RegisterFunc(L, -1, "scrollSensitivity", _s_set_scrollSensitivity);
		Utils.RegisterFunc(L, -1, "caretColor", _s_set_caretColor);
		Utils.RegisterFunc(L, -1, "customCaretColor", _s_set_customCaretColor);
		Utils.RegisterFunc(L, -1, "selectionColor", _s_set_selectionColor);
		Utils.RegisterFunc(L, -1, "onEndEdit", _s_set_onEndEdit);
		Utils.RegisterFunc(L, -1, "onSubmit", _s_set_onSubmit);
		Utils.RegisterFunc(L, -1, "onSelect", _s_set_onSelect);
		Utils.RegisterFunc(L, -1, "onDeselect", _s_set_onDeselect);
		Utils.RegisterFunc(L, -1, "onTextSelection", _s_set_onTextSelection);
		Utils.RegisterFunc(L, -1, "onEndTextSelection", _s_set_onEndTextSelection);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.RegisterFunc(L, -1, "onPressEnter", _s_set_onPressEnter);
		Utils.RegisterFunc(L, -1, "onTouchScreenKeyboardStatusChanged", _s_set_onTouchScreenKeyboardStatusChanged);
		Utils.RegisterFunc(L, -1, "onValidateInput", _s_set_onValidateInput);
		Utils.RegisterFunc(L, -1, "characterLimit", _s_set_characterLimit);
		Utils.RegisterFunc(L, -1, "pointSize", _s_set_pointSize);
		Utils.RegisterFunc(L, -1, "fontAsset", _s_set_fontAsset);
		Utils.RegisterFunc(L, -1, "onFocusSelectAll", _s_set_onFocusSelectAll);
		Utils.RegisterFunc(L, -1, "resetOnDeActivation", _s_set_resetOnDeActivation);
		Utils.RegisterFunc(L, -1, "restoreOriginalTextOnEscape", _s_set_restoreOriginalTextOnEscape);
		Utils.RegisterFunc(L, -1, "isRichTextEditingAllowed", _s_set_isRichTextEditingAllowed);
		Utils.RegisterFunc(L, -1, "contentType", _s_set_contentType);
		Utils.RegisterFunc(L, -1, "lineType", _s_set_lineType);
		Utils.RegisterFunc(L, -1, "lineLimit", _s_set_lineLimit);
		Utils.RegisterFunc(L, -1, "inputType", _s_set_inputType);
		Utils.RegisterFunc(L, -1, "keyboardType", _s_set_keyboardType);
		Utils.RegisterFunc(L, -1, "characterValidation", _s_set_characterValidation);
		Utils.RegisterFunc(L, -1, "inputValidator", _s_set_inputValidator);
		Utils.RegisterFunc(L, -1, "readOnly", _s_set_readOnly);
		Utils.RegisterFunc(L, -1, "richText", _s_set_richText);
		Utils.RegisterFunc(L, -1, "asteriskChar", _s_set_asteriskChar);
		Utils.RegisterFunc(L, -1, "caretPosition", _s_set_caretPosition);
		Utils.RegisterFunc(L, -1, "selectionAnchorPosition", _s_set_selectionAnchorPosition);
		Utils.RegisterFunc(L, -1, "selectionFocusPosition", _s_set_selectionFocusPosition);
		Utils.RegisterFunc(L, -1, "stringPosition", _s_set_stringPosition);
		Utils.RegisterFunc(L, -1, "selectionStringAnchorPosition", _s_set_selectionStringAnchorPosition);
		Utils.RegisterFunc(L, -1, "selectionStringFocusPosition", _s_set_selectionStringFocusPosition);
		Utils.RegisterFunc(L, -1, "OpenNativeKeyboard", _s_set_OpenNativeKeyboard);
		Utils.RegisterFunc(L, -1, "SetNativeKeyboardSelection", _s_set_SetNativeKeyboardSelection);
		Utils.RegisterFunc(L, -1, "GetNativeKeyboardSelection", _s_set_GetNativeKeyboardSelection);
		Utils.RegisterFunc(L, -1, "isOnPressEnterRegistered", _s_set_isOnPressEnterRegistered);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "TMPro.TMP_InputField does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCompositionLength(IntPtr L)
	{
		try
		{
			int compositionLength = ((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCompositionLength();
			Lua.xlua_pushinteger(L, compositionLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTextWithoutNotify(IntPtr L)
	{
		try
		{
			TMP_InputField obj = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string textWithoutNotify = Lua.lua_tostring(L, 2);
			obj.SetTextWithoutNotify(textWithoutNotify);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveTextEnd(IntPtr L)
	{
		try
		{
			TMP_InputField obj = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool shift = Lua.lua_toboolean(L, 2);
			obj.MoveTextEnd(shift);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveTextStart(IntPtr L)
	{
		try
		{
			TMP_InputField obj = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool shift = Lua.lua_toboolean(L, 2);
			obj.MoveTextStart(shift);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveToEndOfLine(IntPtr L)
	{
		try
		{
			TMP_InputField obj = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool shift = Lua.lua_toboolean(L, 2);
			bool ctrl = Lua.lua_toboolean(L, 3);
			obj.MoveToEndOfLine(shift, ctrl);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveToStartOfLine(IntPtr L)
	{
		try
		{
			TMP_InputField obj = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool shift = Lua.lua_toboolean(L, 2);
			bool ctrl = Lua.lua_toboolean(L, 3);
			obj.MoveToStartOfLine(shift, ctrl);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_InputField.OnBeginDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_InputField.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_InputField.OnEndDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_InputField.OnPointerDown(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProcessEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			Event e = (Event)objectTranslator.GetObject(L, 2, typeof(Event));
			tMP_InputField.ProcessEvent(e);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdateSelected(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			tMP_InputField.OnUpdateSelected(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnScroll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_InputField.OnScroll(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceLabelUpdate(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceLabelUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			tMP_InputField.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LayoutComplete(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GraphicUpdateComplete(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ActivateInputField(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ActivateInputField();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSelect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			tMP_InputField.OnSelect(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_InputField.OnPointerClick(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnControlClick(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnControlClick();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReleaseSelection(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReleaseSelection();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeactivateInputField(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool clearSelection = Lua.lua_toboolean(L, 2);
				tMP_InputField.DeactivateInputField(clearSelection);
				return 0;
			}
			if (num == 1)
			{
				tMP_InputField.DeactivateInputField();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_InputField.DeactivateInputField!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDeselect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			tMP_InputField.OnDeselect(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSubmit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			tMP_InputField.OnSubmit(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputHorizontal(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLayoutInputVertical(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalPointSize(IntPtr L)
	{
		try
		{
			TMP_InputField obj = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float globalPointSize = (float)Lua.lua_tonumber(L, 2);
			obj.SetGlobalPointSize(globalPointSize);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalFontAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			TMP_FontAsset globalFontAsset = (TMP_FontAsset)objectTranslator.GetObject(L, 2, typeof(TMP_FontAsset));
			tMP_InputField.SetGlobalFontAsset(globalFontAsset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalText(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLocalText();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnPaste(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.OnPaste);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UseNativeKeyboard(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.UseNativeKeyboard);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NativeSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.NativeSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shouldHideMobileInput(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.shouldHideMobileInput);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shouldHideSoftKeyboard(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.shouldHideSoftKeyboard);
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
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, tMP_InputField.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isFocused(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.isFocused);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_caretBlinkRate(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.caretBlinkRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_caretWidth(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.caretWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textViewport(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.textViewport);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.textComponent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_placeholder(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.placeholder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalScrollbar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.verticalScrollbar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scrollSensitivity(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.scrollSensitivity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_caretColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, tMP_InputField.caretColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_customCaretColor(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.customCaretColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_selectionColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, tMP_InputField.selectionColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndEdit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onEndEdit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onSubmit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onSubmit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onSelect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onSelect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onDeselect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onDeselect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onTextSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onTextSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndTextSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onEndTextSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPressEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onPressEnter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onTouchScreenKeyboardStatusChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onTouchScreenKeyboardStatusChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onValidateInput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.onValidateInput);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_characterLimit(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.characterLimit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointSize(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.pointSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.fontAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onFocusSelectAll(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.onFocusSelectAll);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resetOnDeActivation(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.resetOnDeActivation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_restoreOriginalTextOnEscape(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.restoreOriginalTextOnEscape);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRichTextEditingAllowed(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.isRichTextEditingAllowed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_contentType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushTMProTMP_InputFieldContentType(L, tMP_InputField.contentType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushTMProTMP_InputFieldLineType(L, tMP_InputField.lineType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineLimit(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.lineLimit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inputType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushTMProTMP_InputFieldInputType(L, tMP_InputField.inputType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_keyboardType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.keyboardType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_characterValidation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, tMP_InputField.characterValidation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inputValidator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.inputValidator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_readOnly(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.readOnly);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_richText(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.richText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_multiLine(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.multiLine);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asteriskChar(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.asteriskChar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wasCanceled(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.wasCanceled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_caretPosition(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.caretPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_selectionAnchorPosition(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.selectionAnchorPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_selectionFocusPosition(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.selectionFocusPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stringPosition(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.stringPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_selectionStringAnchorPosition(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.selectionStringAnchorPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_selectionStringFocusPosition(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.selectionStringFocusPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minWidth(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.minWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preferredWidth(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.preferredWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flexibleWidth(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.flexibleWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minHeight(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.minHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preferredHeight(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.preferredHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flexibleHeight(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_InputField.flexibleHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layoutPriority(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_InputField.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OpenNativeKeyboard(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.OpenNativeKeyboard);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SetNativeKeyboardSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.SetNativeKeyboardSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GetNativeKeyboardSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_InputField.GetNativeKeyboardSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isOnPressEnterRegistered(IntPtr L)
	{
		try
		{
			TMP_InputField tMP_InputField = (TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_InputField.isOnPressEnterRegistered);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnPaste(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).OnPaste = objectTranslator.GetDelegate<Action<string>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_NativeSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RangeInt v);
			tMP_InputField.NativeSelection = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shouldHideMobileInput(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shouldHideMobileInput = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shouldHideSoftKeyboard(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shouldHideSoftKeyboard = Lua.lua_toboolean(L, 2);
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
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_caretBlinkRate(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).caretBlinkRate = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_caretWidth(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).caretWidth = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textViewport(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).textViewport = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).textComponent = (TMP_Text)objectTranslator.GetObject(L, 2, typeof(TMP_Text));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_placeholder(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).placeholder = (Graphic)objectTranslator.GetObject(L, 2, typeof(Graphic));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalScrollbar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).verticalScrollbar = (Scrollbar)objectTranslator.GetObject(L, 2, typeof(Scrollbar));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scrollSensitivity(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scrollSensitivity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_caretColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			tMP_InputField.caretColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_customCaretColor(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).customCaretColor = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_selectionColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			tMP_InputField.selectionColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndEdit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onEndEdit = (TMP_InputField.SubmitEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.SubmitEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onSubmit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onSubmit = (TMP_InputField.SubmitEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.SubmitEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onSelect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onSelect = (TMP_InputField.SelectionEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.SelectionEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onDeselect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onDeselect = (TMP_InputField.SelectionEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.SelectionEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onTextSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onTextSelection = (TMP_InputField.TextSelectionEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.TextSelectionEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndTextSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onEndTextSelection = (TMP_InputField.TextSelectionEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.TextSelectionEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (TMP_InputField.OnChangeEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.OnChangeEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPressEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onPressEnter = (TMP_InputField.OnPressEnterEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.OnPressEnterEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onTouchScreenKeyboardStatusChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onTouchScreenKeyboardStatusChanged = (TMP_InputField.TouchScreenKeyboardEvent)objectTranslator.GetObject(L, 2, typeof(TMP_InputField.TouchScreenKeyboardEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onValidateInput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).onValidateInput = objectTranslator.GetDelegate<TMP_InputField.OnValidateInput>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_characterLimit(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).characterLimit = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointSize(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).fontAsset = (TMP_FontAsset)objectTranslator.GetObject(L, 2, typeof(TMP_FontAsset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onFocusSelectAll(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).onFocusSelectAll = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resetOnDeActivation(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).resetOnDeActivation = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_restoreOriginalTextOnEscape(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).restoreOriginalTextOnEscape = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isRichTextEditingAllowed(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isRichTextEditingAllowed = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_contentType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TMP_InputField.ContentType val);
			tMP_InputField.contentType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TMP_InputField.LineType val);
			tMP_InputField.lineType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineLimit(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineLimit = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inputType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TMP_InputField.InputType val);
			tMP_InputField.inputType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_keyboardType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TouchScreenKeyboardType v);
			tMP_InputField.keyboardType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_characterValidation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_InputField tMP_InputField = (TMP_InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TMP_InputField.CharacterValidation val);
			tMP_InputField.characterValidation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inputValidator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).inputValidator = (TMP_InputValidator)objectTranslator.GetObject(L, 2, typeof(TMP_InputValidator));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_readOnly(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).readOnly = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_richText(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).richText = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asteriskChar(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).asteriskChar = (char)Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_caretPosition(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).caretPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_selectionAnchorPosition(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).selectionAnchorPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_selectionFocusPosition(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).selectionFocusPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stringPosition(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stringPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_selectionStringAnchorPosition(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).selectionStringAnchorPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_selectionStringFocusPosition(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).selectionStringFocusPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OpenNativeKeyboard(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).OpenNativeKeyboard = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SetNativeKeyboardSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).SetNativeKeyboardSelection = objectTranslator.GetDelegate<Action<RangeInt>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GetNativeKeyboardSelection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_InputField)objectTranslator.FastGetCSObj(L, 1)).GetNativeKeyboardSelection = objectTranslator.GetDelegate<Func<RangeInt>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isOnPressEnterRegistered(IntPtr L)
	{
		try
		{
			((TMP_InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isOnPressEnterRegistered = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
