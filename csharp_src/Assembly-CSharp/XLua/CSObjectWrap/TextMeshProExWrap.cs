using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TextMeshProExWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TextMeshProEx);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 3, 3);
		Utils.RegisterFunc(L, -3, "SetText", _m_SetText);
		Utils.RegisterFunc(L, -3, "HasArabic", _m_HasArabic);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnPointerUp", _m_OnPointerUp);
		Utils.RegisterFunc(L, -3, "SetNewMaterial", _m_SetNewMaterial);
		Utils.RegisterFunc(L, -3, "GetWidth", _m_GetWidth);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "color32", _g_get_color32);
		Utils.RegisterFunc(L, -2, "onPointerClick", _g_get_onPointerClick);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "color32", _s_set_color32);
		Utils.RegisterFunc(L, -1, "onPointerClick", _s_set_onPointerClick);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "ConvertAlignFormat", _m_ConvertAlignFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckArabicByChar", _m_CheckArabicByChar_xlua_st_);
		Utils.RegisterFunc(L, -4, "isArabic", _m_isArabic_xlua_st_);
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
				TextMeshProEx o = new TextMeshProEx();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TextMeshProEx constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetText(IntPtr L)
	{
		try
		{
			TextMeshProEx obj = (TextMeshProEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string text = Lua.lua_tostring(L, 2);
			obj.SetText(text);
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
			TextAlignmentOptions val2 = TextMeshProEx.ConvertAlignFormat(alignByGeometry: Lua.lua_toboolean(L, 2), anchor: val);
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
			TextMeshProEx obj = (TextMeshProEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			bool value = TextMeshProEx.CheckArabicByChar(Lua.lua_tostring(L, 1), out arabicCount);
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
			bool value = TextMeshProEx.isArabic((char)Lua.xlua_tointeger(L, 1));
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
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProEx.OnPointerClick(eventData);
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
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProEx.OnPointerDown(eventData);
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
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			textMeshProEx.OnPointerUp(eventData);
			return 0;
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
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			Material newMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			textMeshProEx.SetNewMaterial(newMaterial);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWidth(IntPtr L)
	{
		try
		{
			float width = ((TextMeshProEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetWidth();
			Lua.lua_pushnumber(L, width);
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
			TextMeshProEx textMeshProEx = (TextMeshProEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, textMeshProEx.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_color32(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProEx.color32);
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
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, textMeshProEx.onPointerClick);
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
			((TextMeshProEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_color32(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TextMeshProEx textMeshProEx = (TextMeshProEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color32 v);
			textMeshProEx.color32 = v;
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
			((TextMeshProEx)objectTranslator.FastGetCSObj(L, 1)).onPointerClick = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
