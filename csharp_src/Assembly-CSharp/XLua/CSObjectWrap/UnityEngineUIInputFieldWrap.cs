using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIInputFieldWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(InputField);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 22, 35, 24);
		Utils.RegisterFunc(L, -3, "SetTextWithoutNotify", _m_SetTextWithoutNotify);
		Utils.RegisterFunc(L, -3, "MoveTextEnd", _m_MoveTextEnd);
		Utils.RegisterFunc(L, -3, "MoveTextStart", _m_MoveTextStart);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "ProcessEvent", _m_ProcessEvent);
		Utils.RegisterFunc(L, -3, "OnUpdateSelected", _m_OnUpdateSelected);
		Utils.RegisterFunc(L, -3, "ForceLabelUpdate", _m_ForceLabelUpdate);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "ActivateInputField", _m_ActivateInputField);
		Utils.RegisterFunc(L, -3, "OnSelect", _m_OnSelect);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "DeactivateInputField", _m_DeactivateInputField);
		Utils.RegisterFunc(L, -3, "OnDeselect", _m_OnDeselect);
		Utils.RegisterFunc(L, -3, "OnSubmit", _m_OnSubmit);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "SetLocalText", _m_SetLocalText);
		Utils.RegisterFunc(L, -2, "shouldHideMobileInput", _g_get_shouldHideMobileInput);
		Utils.RegisterFunc(L, -2, "shouldActivateOnSelect", _g_get_shouldActivateOnSelect);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "isFocused", _g_get_isFocused);
		Utils.RegisterFunc(L, -2, "caretBlinkRate", _g_get_caretBlinkRate);
		Utils.RegisterFunc(L, -2, "caretWidth", _g_get_caretWidth);
		Utils.RegisterFunc(L, -2, "textComponent", _g_get_textComponent);
		Utils.RegisterFunc(L, -2, "placeholder", _g_get_placeholder);
		Utils.RegisterFunc(L, -2, "caretColor", _g_get_caretColor);
		Utils.RegisterFunc(L, -2, "customCaretColor", _g_get_customCaretColor);
		Utils.RegisterFunc(L, -2, "selectionColor", _g_get_selectionColor);
		Utils.RegisterFunc(L, -2, "onEndEdit", _g_get_onEndEdit);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -2, "onValidateInput", _g_get_onValidateInput);
		Utils.RegisterFunc(L, -2, "characterLimit", _g_get_characterLimit);
		Utils.RegisterFunc(L, -2, "contentType", _g_get_contentType);
		Utils.RegisterFunc(L, -2, "lineType", _g_get_lineType);
		Utils.RegisterFunc(L, -2, "inputType", _g_get_inputType);
		Utils.RegisterFunc(L, -2, "touchScreenKeyboard", _g_get_touchScreenKeyboard);
		Utils.RegisterFunc(L, -2, "keyboardType", _g_get_keyboardType);
		Utils.RegisterFunc(L, -2, "characterValidation", _g_get_characterValidation);
		Utils.RegisterFunc(L, -2, "readOnly", _g_get_readOnly);
		Utils.RegisterFunc(L, -2, "multiLine", _g_get_multiLine);
		Utils.RegisterFunc(L, -2, "asteriskChar", _g_get_asteriskChar);
		Utils.RegisterFunc(L, -2, "wasCanceled", _g_get_wasCanceled);
		Utils.RegisterFunc(L, -2, "caretPosition", _g_get_caretPosition);
		Utils.RegisterFunc(L, -2, "selectionAnchorPosition", _g_get_selectionAnchorPosition);
		Utils.RegisterFunc(L, -2, "selectionFocusPosition", _g_get_selectionFocusPosition);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -1, "shouldHideMobileInput", _s_set_shouldHideMobileInput);
		Utils.RegisterFunc(L, -1, "shouldActivateOnSelect", _s_set_shouldActivateOnSelect);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "caretBlinkRate", _s_set_caretBlinkRate);
		Utils.RegisterFunc(L, -1, "caretWidth", _s_set_caretWidth);
		Utils.RegisterFunc(L, -1, "textComponent", _s_set_textComponent);
		Utils.RegisterFunc(L, -1, "placeholder", _s_set_placeholder);
		Utils.RegisterFunc(L, -1, "caretColor", _s_set_caretColor);
		Utils.RegisterFunc(L, -1, "customCaretColor", _s_set_customCaretColor);
		Utils.RegisterFunc(L, -1, "selectionColor", _s_set_selectionColor);
		Utils.RegisterFunc(L, -1, "onEndEdit", _s_set_onEndEdit);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.RegisterFunc(L, -1, "onValidateInput", _s_set_onValidateInput);
		Utils.RegisterFunc(L, -1, "characterLimit", _s_set_characterLimit);
		Utils.RegisterFunc(L, -1, "contentType", _s_set_contentType);
		Utils.RegisterFunc(L, -1, "lineType", _s_set_lineType);
		Utils.RegisterFunc(L, -1, "inputType", _s_set_inputType);
		Utils.RegisterFunc(L, -1, "keyboardType", _s_set_keyboardType);
		Utils.RegisterFunc(L, -1, "characterValidation", _s_set_characterValidation);
		Utils.RegisterFunc(L, -1, "readOnly", _s_set_readOnly);
		Utils.RegisterFunc(L, -1, "asteriskChar", _s_set_asteriskChar);
		Utils.RegisterFunc(L, -1, "caretPosition", _s_set_caretPosition);
		Utils.RegisterFunc(L, -1, "selectionAnchorPosition", _s_set_selectionAnchorPosition);
		Utils.RegisterFunc(L, -1, "selectionFocusPosition", _s_set_selectionFocusPosition);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.InputField does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTextWithoutNotify(IntPtr L)
	{
		try
		{
			InputField obj = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			InputField obj = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			InputField obj = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			inputField.OnBeginDrag(eventData);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			inputField.OnDrag(eventData);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			inputField.OnEndDrag(eventData);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			inputField.OnPointerDown(eventData);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			Event e = (Event)objectTranslator.GetObject(L, 2, typeof(Event));
			inputField.ProcessEvent(e);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			inputField.OnUpdateSelected(eventData);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceLabelUpdate();
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			inputField.Rebuild(v);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ActivateInputField();
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			inputField.OnSelect(eventData);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			inputField.OnPointerClick(eventData);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DeactivateInputField();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDeselect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			inputField.OnDeselect(eventData);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			inputField.OnSubmit(eventData);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLocalText();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shouldHideMobileInput(IntPtr L)
	{
		try
		{
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.shouldHideMobileInput);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shouldActivateOnSelect(IntPtr L)
	{
		try
		{
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.shouldActivateOnSelect);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, inputField.text);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.isFocused);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.caretBlinkRate);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.caretWidth);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.textComponent);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.placeholder);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, inputField.caretColor);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.customCaretColor);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, inputField.selectionColor);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.onEndEdit);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.onValueChanged);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.onValidateInput);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.characterLimit);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIInputFieldContentType(L, inputField.contentType);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIInputFieldLineType(L, inputField.lineType);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIInputFieldInputType(L, inputField.inputType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touchScreenKeyboard(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.touchScreenKeyboard);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, inputField.keyboardType);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, inputField.characterValidation);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.readOnly);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.multiLine);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.asteriskChar);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, inputField.wasCanceled);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.caretPosition);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.selectionAnchorPosition);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.selectionFocusPosition);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.minWidth);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.preferredWidth);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.flexibleWidth);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.minHeight);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.preferredHeight);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, inputField.flexibleHeight);
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
			InputField inputField = (InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, inputField.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shouldHideMobileInput(IntPtr L)
	{
		try
		{
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shouldHideMobileInput = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shouldActivateOnSelect(IntPtr L)
	{
		try
		{
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shouldActivateOnSelect = Lua.lua_toboolean(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).caretBlinkRate = (float)Lua.lua_tonumber(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).caretWidth = Lua.xlua_tointeger(L, 2);
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
			((InputField)objectTranslator.FastGetCSObj(L, 1)).textComponent = (Text)objectTranslator.GetObject(L, 2, typeof(Text));
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
			((InputField)objectTranslator.FastGetCSObj(L, 1)).placeholder = (Graphic)objectTranslator.GetObject(L, 2, typeof(Graphic));
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			inputField.caretColor = val;
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).customCaretColor = Lua.lua_toboolean(L, 2);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			inputField.selectionColor = val;
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
			((InputField)objectTranslator.FastGetCSObj(L, 1)).onEndEdit = (InputField.SubmitEvent)objectTranslator.GetObject(L, 2, typeof(InputField.SubmitEvent));
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
			((InputField)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (InputField.OnChangeEvent)objectTranslator.GetObject(L, 2, typeof(InputField.OnChangeEvent));
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
			((InputField)objectTranslator.FastGetCSObj(L, 1)).onValidateInput = objectTranslator.GetDelegate<InputField.OnValidateInput>(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).characterLimit = Lua.xlua_tointeger(L, 2);
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out InputField.ContentType val);
			inputField.contentType = val;
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out InputField.LineType val);
			inputField.lineType = val;
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out InputField.InputType val);
			inputField.inputType = val;
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TouchScreenKeyboardType v);
			inputField.keyboardType = v;
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
			InputField inputField = (InputField)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out InputField.CharacterValidation val);
			inputField.characterValidation = val;
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).readOnly = Lua.lua_toboolean(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).asteriskChar = (char)Lua.xlua_tointeger(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).caretPosition = Lua.xlua_tointeger(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).selectionAnchorPosition = Lua.xlua_tointeger(L, 2);
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
			((InputField)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).selectionFocusPosition = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
