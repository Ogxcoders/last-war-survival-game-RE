using System;
using System.Globalization;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUITextWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Text);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 24, 13);
		Utils.RegisterFunc(L, -3, "FontTextureChanged", _m_FontTextureChanged);
		Utils.RegisterFunc(L, -3, "GetGenerationSettings", _m_GetGenerationSettings);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "DOColor", _m_DOColor);
		Utils.RegisterFunc(L, -3, "DOCounter", _m_DOCounter);
		Utils.RegisterFunc(L, -3, "DOFade", _m_DOFade);
		Utils.RegisterFunc(L, -3, "DOText", _m_DOText);
		Utils.RegisterFunc(L, -3, "DOBlendableColor", _m_DOBlendableColor);
		Utils.RegisterFunc(L, -3, "SetTimeStamp", _m_SetTimeStamp);
		Utils.RegisterFunc(L, -3, "SetLocalText", _m_SetLocalText);
		Utils.RegisterFunc(L, -2, "cachedTextGenerator", _g_get_cachedTextGenerator);
		Utils.RegisterFunc(L, -2, "cachedTextGeneratorForLayout", _g_get_cachedTextGeneratorForLayout);
		Utils.RegisterFunc(L, -2, "mainTexture", _g_get_mainTexture);
		Utils.RegisterFunc(L, -2, "font", _g_get_font);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "supportRichText", _g_get_supportRichText);
		Utils.RegisterFunc(L, -2, "resizeTextForBestFit", _g_get_resizeTextForBestFit);
		Utils.RegisterFunc(L, -2, "resizeTextMinSize", _g_get_resizeTextMinSize);
		Utils.RegisterFunc(L, -2, "resizeTextMaxSize", _g_get_resizeTextMaxSize);
		Utils.RegisterFunc(L, -2, "alignment", _g_get_alignment);
		Utils.RegisterFunc(L, -2, "alignByGeometry", _g_get_alignByGeometry);
		Utils.RegisterFunc(L, -2, "fontSize", _g_get_fontSize);
		Utils.RegisterFunc(L, -2, "horizontalOverflow", _g_get_horizontalOverflow);
		Utils.RegisterFunc(L, -2, "verticalOverflow", _g_get_verticalOverflow);
		Utils.RegisterFunc(L, -2, "lineSpacing", _g_get_lineSpacing);
		Utils.RegisterFunc(L, -2, "fontStyle", _g_get_fontStyle);
		Utils.RegisterFunc(L, -2, "pixelsPerUnit", _g_get_pixelsPerUnit);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -1, "font", _s_set_font);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "supportRichText", _s_set_supportRichText);
		Utils.RegisterFunc(L, -1, "resizeTextForBestFit", _s_set_resizeTextForBestFit);
		Utils.RegisterFunc(L, -1, "resizeTextMinSize", _s_set_resizeTextMinSize);
		Utils.RegisterFunc(L, -1, "resizeTextMaxSize", _s_set_resizeTextMaxSize);
		Utils.RegisterFunc(L, -1, "alignment", _s_set_alignment);
		Utils.RegisterFunc(L, -1, "alignByGeometry", _s_set_alignByGeometry);
		Utils.RegisterFunc(L, -1, "fontSize", _s_set_fontSize);
		Utils.RegisterFunc(L, -1, "horizontalOverflow", _s_set_horizontalOverflow);
		Utils.RegisterFunc(L, -1, "verticalOverflow", _s_set_verticalOverflow);
		Utils.RegisterFunc(L, -1, "lineSpacing", _s_set_lineSpacing);
		Utils.RegisterFunc(L, -1, "fontStyle", _s_set_fontStyle);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "GetTextAnchorPivot", _m_GetTextAnchorPivot_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Text does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FontTextureChanged(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FontTextureChanged();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGenerationSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			TextGenerationSettings generationSettings = text.GetGenerationSettings(val);
			objectTranslator.Push(L, generationSettings);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTextAnchorPivot_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TextAnchor val);
			Vector2 textAnchorPivot = Text.GetTextAnchorPivot(val);
			objectTranslator.PushUnityEngineVector2(L, textAnchorPivot);
			return 1;
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
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text target = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			TweenerCore<Color, Color, ColorOptions> o = DOTweenModuleUI.DOColor(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOCounter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text target = (Text)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && objectTranslator.Assignable<CultureInfo>(L, 6))
			{
				int fromValue = Lua.xlua_tointeger(L, 2);
				int endValue = Lua.xlua_tointeger(L, 3);
				float duration = (float)Lua.lua_tonumber(L, 4);
				bool addThousandsSeparator = Lua.lua_toboolean(L, 5);
				CultureInfo culture = (CultureInfo)objectTranslator.GetObject(L, 6, typeof(CultureInfo));
				TweenerCore<int, int, NoOptions> o = target.DOCounter(fromValue, endValue, duration, addThousandsSeparator, culture);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				int fromValue2 = Lua.xlua_tointeger(L, 2);
				int endValue2 = Lua.xlua_tointeger(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				bool addThousandsSeparator2 = Lua.lua_toboolean(L, 5);
				TweenerCore<int, int, NoOptions> o2 = target.DOCounter(fromValue2, endValue2, duration2, addThousandsSeparator2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int fromValue3 = Lua.xlua_tointeger(L, 2);
				int endValue3 = Lua.xlua_tointeger(L, 3);
				float duration3 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<int, int, NoOptions> o3 = target.DOCounter(fromValue3, endValue3, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Text.DOCounter!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFade(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text target = (Text)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<Color, Color, ColorOptions> o = target.DOFade(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text target = (Text)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && objectTranslator.Assignable<ScrambleMode>(L, 5) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TSTRING))
			{
				string endValue = Lua.lua_tostring(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool richTextEnabled = Lua.lua_toboolean(L, 4);
				objectTranslator.Get(L, 5, out ScrambleMode v);
				string scrambleChars = Lua.lua_tostring(L, 6);
				TweenerCore<string, string, StringOptions> o = target.DOText(endValue, duration, richTextEnabled, v, scrambleChars);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && objectTranslator.Assignable<ScrambleMode>(L, 5))
			{
				string endValue2 = Lua.lua_tostring(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				bool richTextEnabled2 = Lua.lua_toboolean(L, 4);
				objectTranslator.Get(L, 5, out ScrambleMode v2);
				TweenerCore<string, string, StringOptions> o2 = target.DOText(endValue2, duration2, richTextEnabled2, v2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string endValue3 = Lua.lua_tostring(L, 2);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				bool richTextEnabled3 = Lua.lua_toboolean(L, 4);
				TweenerCore<string, string, StringOptions> o3 = target.DOText(endValue3, duration3, richTextEnabled3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string endValue4 = Lua.lua_tostring(L, 2);
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<string, string, StringOptions> o4 = target.DOText(endValue4, duration4);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Text.DOText!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text target = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			Tweener o = DOTweenModuleUI.DOBlendableColor(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTimeStamp(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long leftMilliSecond = Lua.lua_toint64(L, 2);
			text.SetTimeStamp(leftMilliSecond);
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
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLocalText();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedTextGenerator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.cachedTextGenerator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedTextGeneratorForLayout(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.cachedTextGeneratorForLayout);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.mainTexture);
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
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.font);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, text.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportRichText(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, text.supportRichText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resizeTextForBestFit(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, text.resizeTextForBestFit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resizeTextMinSize(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, text.resizeTextMinSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resizeTextMaxSize(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, text.resizeTextMaxSize);
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
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineTextAnchor(L, text.alignment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alignByGeometry(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, text.alignByGeometry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontSize(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, text.fontSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalOverflow(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.horizontalOverflow);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalOverflow(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.verticalOverflow);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.lineSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontStyle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, text.fontStyle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelsPerUnit(IntPtr L)
	{
		try
		{
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.pixelsPerUnit);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.minWidth);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.preferredWidth);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.flexibleWidth);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.minHeight);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.preferredHeight);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, text.flexibleHeight);
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
			Text text = (Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, text.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_font(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Text)objectTranslator.FastGetCSObj(L, 1)).font = (Font)objectTranslator.GetObject(L, 2, typeof(Font));
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
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportRichText(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportRichText = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resizeTextForBestFit(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).resizeTextForBestFit = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resizeTextMinSize(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).resizeTextMinSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resizeTextMaxSize(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).resizeTextMaxSize = Lua.xlua_tointeger(L, 2);
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
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextAnchor val);
			text.alignment = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alignByGeometry(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alignByGeometry = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontSize(IntPtr L)
	{
		try
		{
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fontSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalOverflow(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out HorizontalWrapMode v);
			text.horizontalOverflow = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalOverflow(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VerticalWrapMode v);
			text.verticalOverflow = v;
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
			((Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontStyle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Text text = (Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FontStyle v);
			text.fontStyle = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
