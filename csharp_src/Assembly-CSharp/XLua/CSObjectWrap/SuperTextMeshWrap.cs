using System;
using ArabicSupport;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SuperTextMeshWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SuperTextMesh);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 25, 24);
		Utils.RegisterFunc(L, -3, "FontTextureChanged", _m_FontTextureChanged);
		Utils.RegisterFunc(L, -3, "SetOrderInLayer", _m_SetOrderInLayer);
		Utils.RegisterFunc(L, -3, "SetCallBack", _m_SetCallBack);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "GetHeight", _m_GetHeight);
		Utils.RegisterFunc(L, -3, "GetWidth", _m_GetWidth);
		Utils.RegisterFunc(L, -3, "SetColorAlpha", _m_SetColorAlpha);
		Utils.RegisterFunc(L, -3, "PopulateWithErrors", _m_PopulateWithErrors);
		Utils.RegisterFunc(L, -3, "SetLocalText", _m_SetLocalText);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "color32", _g_get_color32);
		Utils.RegisterFunc(L, -2, "EffectGradient", _g_get_EffectGradient);
		Utils.RegisterFunc(L, -2, "MaterialKey", _g_get_MaterialKey);
		Utils.RegisterFunc(L, -2, "LineCount", _g_get_LineCount);
		Utils.RegisterFunc(L, -2, "alpha", _g_get_alpha);
		Utils.RegisterFunc(L, -2, "_text", _g_get__text);
		Utils.RegisterFunc(L, -2, "font", _g_get_font);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "size", _g_get_size);
		Utils.RegisterFunc(L, -2, "quality", _g_get_quality);
		Utils.RegisterFunc(L, -2, "style", _g_get_style);
		Utils.RegisterFunc(L, -2, "lineSpacing", _g_get_lineSpacing);
		Utils.RegisterFunc(L, -2, "characterSpacing", _g_get_characterSpacing);
		Utils.RegisterFunc(L, -2, "tabSize", _g_get_tabSize);
		Utils.RegisterFunc(L, -2, "autoWrap", _g_get_autoWrap);
		Utils.RegisterFunc(L, -2, "breakText", _g_get_breakText);
		Utils.RegisterFunc(L, -2, "insertHyphens", _g_get_insertHyphens);
		Utils.RegisterFunc(L, -2, "textMat", _g_get_textMat);
		Utils.RegisterFunc(L, -2, "gradient", _g_get_gradient);
		Utils.RegisterFunc(L, -2, "ztest", _g_get_ztest);
		Utils.RegisterFunc(L, -2, "outline", _g_get_outline);
		Utils.RegisterFunc(L, -2, "outlineColor", _g_get_outlineColor);
		Utils.RegisterFunc(L, -2, "outlineDistance", _g_get_outlineDistance);
		Utils.RegisterFunc(L, -2, "alignment", _g_get_alignment);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "color32", _s_set_color32);
		Utils.RegisterFunc(L, -1, "EffectGradient", _s_set_EffectGradient);
		Utils.RegisterFunc(L, -1, "MaterialKey", _s_set_MaterialKey);
		Utils.RegisterFunc(L, -1, "alpha", _s_set_alpha);
		Utils.RegisterFunc(L, -1, "_text", _s_set__text);
		Utils.RegisterFunc(L, -1, "font", _s_set_font);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "size", _s_set_size);
		Utils.RegisterFunc(L, -1, "quality", _s_set_quality);
		Utils.RegisterFunc(L, -1, "style", _s_set_style);
		Utils.RegisterFunc(L, -1, "lineSpacing", _s_set_lineSpacing);
		Utils.RegisterFunc(L, -1, "characterSpacing", _s_set_characterSpacing);
		Utils.RegisterFunc(L, -1, "tabSize", _s_set_tabSize);
		Utils.RegisterFunc(L, -1, "autoWrap", _s_set_autoWrap);
		Utils.RegisterFunc(L, -1, "breakText", _s_set_breakText);
		Utils.RegisterFunc(L, -1, "insertHyphens", _s_set_insertHyphens);
		Utils.RegisterFunc(L, -1, "textMat", _s_set_textMat);
		Utils.RegisterFunc(L, -1, "gradient", _s_set_gradient);
		Utils.RegisterFunc(L, -1, "ztest", _s_set_ztest);
		Utils.RegisterFunc(L, -1, "outline", _s_set_outline);
		Utils.RegisterFunc(L, -1, "outlineColor", _s_set_outlineColor);
		Utils.RegisterFunc(L, -1, "outlineDistance", _s_set_outlineDistance);
		Utils.RegisterFunc(L, -1, "alignment", _s_set_alignment);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 1);
		Utils.RegisterFunc(L, -2, "IsArabicLanguage", _g_get_IsArabicLanguage);
		Utils.RegisterFunc(L, -1, "IsArabicLanguage", _s_set_IsArabicLanguage);
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
				SuperTextMesh o = new SuperTextMesh();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperTextMesh constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FontTextureChanged(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FontTextureChanged();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOrderInLayer(IntPtr L)
	{
		try
		{
			SuperTextMesh obj = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int orderInLayer = Lua.xlua_tointeger(L, 2);
			obj.SetOrderInLayer(orderInLayer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			Action callBack = objectTranslator.GetDelegate<Action>(L, 2);
			superTextMesh.SetCallBack(callBack);
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
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Rebuild();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			SuperTextMesh obj = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float alpha = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(alpha);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHeight(IntPtr L)
	{
		try
		{
			float height = ((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHeight();
			Lua.lua_pushnumber(L, height);
			return 1;
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
			float width = ((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetWidth();
			Lua.lua_pushnumber(L, width);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColorAlpha(IntPtr L)
	{
		try
		{
			SuperTextMesh obj = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float colorAlpha = (float)Lua.lua_tonumber(L, 2);
			obj.SetColorAlpha(colorAlpha);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PopulateWithErrors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh obj = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			string finalTxt = Lua.lua_tostring(L, 2);
			UGUITextLines[] o = obj.PopulateWithErrors(finalTxt);
			objectTranslator.Push(L, o);
			return 1;
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
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLocalText();
			return 0;
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
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, superTextMesh.text);
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
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, superTextMesh.color32);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EffectGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, superTextMesh.EffectGradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MaterialKey(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, superTextMesh.MaterialKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LineCount(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, superTextMesh.LineCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alpha(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, superTextMesh.alpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsArabicLanguage(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SuperTextMesh.IsArabicLanguage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__text(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, superTextMesh._text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_font(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, superTextMesh.font);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, superTextMesh.color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_size(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, superTextMesh.size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quality(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, superTextMesh.quality);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_style(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, superTextMesh.style);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineSpacing(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, superTextMesh.lineSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_characterSpacing(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, superTextMesh.characterSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tabSize(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, superTextMesh.tabSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoWrap(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, superTextMesh.autoWrap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_breakText(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, superTextMesh.breakText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_insertHyphens(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, superTextMesh.insertHyphens);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textMat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, superTextMesh.textMat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gradient(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, superTextMesh.gradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ztest(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, superTextMesh.ztest);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outline(IntPtr L)
	{
		try
		{
			SuperTextMesh superTextMesh = (SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, superTextMesh.outline);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outlineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, superTextMesh.outlineColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outlineDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, superTextMesh.outlineDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushSuperTextMeshAlignment(L, superTextMesh.alignment);
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
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
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
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color32 v);
			superTextMesh.color32 = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EffectGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SuperTextMesh)objectTranslator.FastGetCSObj(L, 1)).EffectGradient = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MaterialKey(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MaterialKey = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alpha(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alpha = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsArabicLanguage(IntPtr L)
	{
		try
		{
			SuperTextMesh.IsArabicLanguage = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__text(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_font(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SuperTextMesh)objectTranslator.FastGetCSObj(L, 1)).font = (Font)objectTranslator.GetObject(L, 2, typeof(Font));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color32 v);
			superTextMesh.color = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_size(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).size = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quality(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quality = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_style(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FontStyle v);
			superTextMesh.style = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineSpacing(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_characterSpacing(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).characterSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tabSize(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tabSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoWrap(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoWrap = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_breakText(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).breakText = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_insertHyphens(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).insertHyphens = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textMat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SuperTextMesh)objectTranslator.FastGetCSObj(L, 1)).textMat = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gradient(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gradient = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ztest(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ztest = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outline(IntPtr L)
	{
		try
		{
			((SuperTextMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).outline = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outlineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			superTextMesh.outlineColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outlineDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			superTextMesh.outlineDistance = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SuperTextMesh superTextMesh = (SuperTextMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SuperTextMesh.Alignment val);
			superTextMesh.alignment = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
