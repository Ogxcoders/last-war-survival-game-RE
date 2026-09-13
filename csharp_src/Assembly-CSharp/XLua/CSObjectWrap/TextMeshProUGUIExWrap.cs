using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TextMeshProUGUIExWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TextMeshProUGUIEx);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 3, 3);
		Utils.RegisterFunc(L, -3, "SetFontFillingUint", _m_SetFontFillingUint);
		Utils.RegisterFunc(L, -3, "GetOriginalText", _m_GetOriginalText);
		Utils.RegisterFunc(L, -3, "ChangeAutoSetting", _m_ChangeAutoSetting);
		Utils.RegisterFunc(L, -3, "HasArabic", _m_HasArabic);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "ThroughPointerClickHandler", _m_ThroughPointerClickHandler);
		Utils.RegisterFunc(L, -3, "OnCharacterPointClick", _m_OnCharacterPointClick);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnPointerUp", _m_OnPointerUp);
		Utils.RegisterFunc(L, -3, "SetNewMaterial", _m_SetNewMaterial);
		Utils.RegisterFunc(L, -3, "SetTextColorRange", _m_SetTextColorRange);
		Utils.RegisterFunc(L, -3, "FixSpaces", _m_FixSpaces);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "onPointerClick", _g_get_onPointerClick);
		Utils.RegisterFunc(L, -2, "onCharacterPointerClick", _g_get_onCharacterPointerClick);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "onPointerClick", _s_set_onPointerClick);
		Utils.RegisterFunc(L, -1, "onCharacterPointerClick", _s_set_onCharacterPointerClick);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "ConvertAlignFormat", _m_ConvertAlignFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckArabicByChar", _m_CheckArabicByChar_xlua_st_);
		Utils.RegisterFunc(L, -4, "isArabic", _m_isArabic_xlua_st_);
		Utils.RegisterFunc(L, -4, "FixAllah", _m_FixAllah_xlua_st_);
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
				TextMeshProUGUIEx o = new TextMeshProUGUIEx();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TextMeshProUGUIEx constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFontFillingUint(IntPtr L)
	{
		try
		{
			TextMeshProUGUIEx obj = (TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fontFillingUint = Lua.xlua_tointeger(L, 2);
			obj.SetFontFillingUint(fontFillingUint);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOriginalText(IntPtr L)
	{
		try
		{
			string originalText = ((TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetOriginalText();
			Lua.lua_pushstring(L, originalText);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeAutoSetting(IntPtr L)
	{
		try
		{
			TextMeshProUGUIEx obj = (TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool value = Lua.lua_toboolean(L, 2);
			obj.ChangeAutoSetting(value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConvertAlignFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TextAnchor val);
			TextAlignmentOptions val2 = TextMeshProUGUIEx.ConvertAlignFormat(alignByGeometry: Lua.lua_toboolean(L, 2), anchor: val);
			objectTranslator.PushTMProTextAlignmentOptions(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasArabic(IntPtr L)
	{
		try
		{
			TextMeshProUGUIEx obj = (TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string str = Lua.lua_tostring(L, 2);
			int arabicCount;
			bool value = obj.HasArabic(str, out arabicCount);
			Lua.lua_pushboolean(L, value);
			Lua.xlua_pushinteger(L, arabicCount);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckArabicByChar_xlua_st_(IntPtr L)
	{
		try
		{
			int arabicCount;
			bool value = TextMeshProUGUIEx.CheckArabicByChar(Lua.lua_tostring(L, 1), out arabicCount);
			Lua.lua_pushboolean(L, value);
			Lua.xlua_pushinteger(L, arabicCount);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isArabic_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = TextMeshProUGUIEx.isArabic((char)Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
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
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProUGUIEx.OnPointerClick(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ThroughPointerClickHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 3, typeof(PointerEventData));
			bool value = textMeshProUGUIEx.ThroughPointerClickHandler(gameObject, eventData);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCharacterPointClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProUGUIEx.OnCharacterPointClick(eventData);
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
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProUGUIEx.OnPointerDown(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProUGUIEx.OnPointerUp(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FixAllah_xlua_st_(IntPtr L)
	{
		try
		{
			string str = TextMeshProUGUIEx.FixAllah(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNewMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			Material newMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			textMeshProUGUIEx.SetNewMaterial(newMaterial);
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
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			List<int> indexList = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
			objectTranslator.Get(L, 3, out Color val);
			textMeshProUGUIEx.SetTextColorRange(indexList, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FixSpaces(IntPtr L)
	{
		try
		{
			TextMeshProUGUIEx obj = (TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string val = Lua.lua_tostring(L, 2);
			string str = obj.FixSpaces(val);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_text(IntPtr L)
	{
		try
		{
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, textMeshProUGUIEx.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProUGUIEx.onPointerClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onCharacterPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProUGUIEx textMeshProUGUIEx = (TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProUGUIEx.onCharacterPointerClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_text(IntPtr L)
	{
		try
		{
			((TextMeshProUGUIEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1)).onPointerClick = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onCharacterPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TextMeshProUGUIEx)objectTranslator.FastGetCSObj(L, 1)).onCharacterPointerClick = objectTranslator.GetDelegate<Action<int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
