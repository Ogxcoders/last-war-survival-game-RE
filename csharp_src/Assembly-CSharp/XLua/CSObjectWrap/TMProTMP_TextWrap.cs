using System;
using System.Text;
using TMPro;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_TextWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_Text);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 18, 90, 68);
		Utils.RegisterFunc(L, -3, "ForceMeshUpdate", _m_ForceMeshUpdate);
		Utils.RegisterFunc(L, -3, "UpdateGeometry", _m_UpdateGeometry);
		Utils.RegisterFunc(L, -3, "UpdateVertexData", _m_UpdateVertexData);
		Utils.RegisterFunc(L, -3, "SetVertices", _m_SetVertices);
		Utils.RegisterFunc(L, -3, "UpdateMeshPadding", _m_UpdateMeshPadding);
		Utils.RegisterFunc(L, -3, "CrossFadeColor", _m_CrossFadeColor);
		Utils.RegisterFunc(L, -3, "CrossFadeAlpha", _m_CrossFadeAlpha);
		Utils.RegisterFunc(L, -3, "SetText", _m_SetText);
		Utils.RegisterFunc(L, -3, "SetCharArray", _m_SetCharArray);
		Utils.RegisterFunc(L, -3, "GetPreferredValues", _m_GetPreferredValues);
		Utils.RegisterFunc(L, -3, "GetRenderedValues", _m_GetRenderedValues);
		Utils.RegisterFunc(L, -3, "GetTextInfo", _m_GetTextInfo);
		Utils.RegisterFunc(L, -3, "ComputeMarginSize", _m_ComputeMarginSize);
		Utils.RegisterFunc(L, -3, "ClearMesh", _m_ClearMesh);
		Utils.RegisterFunc(L, -3, "GetParsedText", _m_GetParsedText);
		Utils.RegisterFunc(L, -3, "AddInputHtmlTag", _m_AddInputHtmlTag);
		Utils.RegisterFunc(L, -3, "SetInputHtmlTagEmpty", _m_SetInputHtmlTagEmpty);
		Utils.RegisterFunc(L, -3, "OnPreRenderText", _e_OnPreRenderText);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "textPreprocessor", _g_get_textPreprocessor);
		Utils.RegisterFunc(L, -2, "isRightToLeftText", _g_get_isRightToLeftText);
		Utils.RegisterFunc(L, -2, "font", _g_get_font);
		Utils.RegisterFunc(L, -2, "fontSharedMaterial", _g_get_fontSharedMaterial);
		Utils.RegisterFunc(L, -2, "fontSharedMaterials", _g_get_fontSharedMaterials);
		Utils.RegisterFunc(L, -2, "fontMaterial", _g_get_fontMaterial);
		Utils.RegisterFunc(L, -2, "fontMaterials", _g_get_fontMaterials);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "alpha", _g_get_alpha);
		Utils.RegisterFunc(L, -2, "enableVertexGradient", _g_get_enableVertexGradient);
		Utils.RegisterFunc(L, -2, "colorGradient", _g_get_colorGradient);
		Utils.RegisterFunc(L, -2, "colorGradientPreset", _g_get_colorGradientPreset);
		Utils.RegisterFunc(L, -2, "spriteAsset", _g_get_spriteAsset);
		Utils.RegisterFunc(L, -2, "tintAllSprites", _g_get_tintAllSprites);
		Utils.RegisterFunc(L, -2, "styleSheet", _g_get_styleSheet);
		Utils.RegisterFunc(L, -2, "textStyle", _g_get_textStyle);
		Utils.RegisterFunc(L, -2, "overrideColorTags", _g_get_overrideColorTags);
		Utils.RegisterFunc(L, -2, "faceColor", _g_get_faceColor);
		Utils.RegisterFunc(L, -2, "outlineColor", _g_get_outlineColor);
		Utils.RegisterFunc(L, -2, "outlineWidth", _g_get_outlineWidth);
		Utils.RegisterFunc(L, -2, "fontSize", _g_get_fontSize);
		Utils.RegisterFunc(L, -2, "fontWeight", _g_get_fontWeight);
		Utils.RegisterFunc(L, -2, "pixelsPerUnit", _g_get_pixelsPerUnit);
		Utils.RegisterFunc(L, -2, "enableAutoSizing", _g_get_enableAutoSizing);
		Utils.RegisterFunc(L, -2, "fontSizeMin", _g_get_fontSizeMin);
		Utils.RegisterFunc(L, -2, "fontSizeMax", _g_get_fontSizeMax);
		Utils.RegisterFunc(L, -2, "fontStyle", _g_get_fontStyle);
		Utils.RegisterFunc(L, -2, "isUsingBold", _g_get_isUsingBold);
		Utils.RegisterFunc(L, -2, "horizontalAlignment", _g_get_horizontalAlignment);
		Utils.RegisterFunc(L, -2, "verticalAlignment", _g_get_verticalAlignment);
		Utils.RegisterFunc(L, -2, "alignment", _g_get_alignment);
		Utils.RegisterFunc(L, -2, "characterSpacing", _g_get_characterSpacing);
		Utils.RegisterFunc(L, -2, "wordSpacing", _g_get_wordSpacing);
		Utils.RegisterFunc(L, -2, "lineSpacing", _g_get_lineSpacing);
		Utils.RegisterFunc(L, -2, "lineSpacingAdjustment", _g_get_lineSpacingAdjustment);
		Utils.RegisterFunc(L, -2, "paragraphSpacing", _g_get_paragraphSpacing);
		Utils.RegisterFunc(L, -2, "characterWidthAdjustment", _g_get_characterWidthAdjustment);
		Utils.RegisterFunc(L, -2, "enableWordWrapping", _g_get_enableWordWrapping);
		Utils.RegisterFunc(L, -2, "wordWrappingRatios", _g_get_wordWrappingRatios);
		Utils.RegisterFunc(L, -2, "overflowMode", _g_get_overflowMode);
		Utils.RegisterFunc(L, -2, "isTextOverflowing", _g_get_isTextOverflowing);
		Utils.RegisterFunc(L, -2, "firstOverflowCharacterIndex", _g_get_firstOverflowCharacterIndex);
		Utils.RegisterFunc(L, -2, "linkedTextComponent", _g_get_linkedTextComponent);
		Utils.RegisterFunc(L, -2, "isTextTruncated", _g_get_isTextTruncated);
		Utils.RegisterFunc(L, -2, "enableKerning", _g_get_enableKerning);
		Utils.RegisterFunc(L, -2, "extraPadding", _g_get_extraPadding);
		Utils.RegisterFunc(L, -2, "richText", _g_get_richText);
		Utils.RegisterFunc(L, -2, "parseCtrlCharacters", _g_get_parseCtrlCharacters);
		Utils.RegisterFunc(L, -2, "isOverlay", _g_get_isOverlay);
		Utils.RegisterFunc(L, -2, "isOrthographic", _g_get_isOrthographic);
		Utils.RegisterFunc(L, -2, "enableCulling", _g_get_enableCulling);
		Utils.RegisterFunc(L, -2, "ignoreVisibility", _g_get_ignoreVisibility);
		Utils.RegisterFunc(L, -2, "horizontalMapping", _g_get_horizontalMapping);
		Utils.RegisterFunc(L, -2, "verticalMapping", _g_get_verticalMapping);
		Utils.RegisterFunc(L, -2, "mappingUvLineOffset", _g_get_mappingUvLineOffset);
		Utils.RegisterFunc(L, -2, "renderMode", _g_get_renderMode);
		Utils.RegisterFunc(L, -2, "geometrySortingOrder", _g_get_geometrySortingOrder);
		Utils.RegisterFunc(L, -2, "isTextObjectScaleStatic", _g_get_isTextObjectScaleStatic);
		Utils.RegisterFunc(L, -2, "vertexBufferAutoSizeReduction", _g_get_vertexBufferAutoSizeReduction);
		Utils.RegisterFunc(L, -2, "firstVisibleCharacter", _g_get_firstVisibleCharacter);
		Utils.RegisterFunc(L, -2, "maxVisibleCharacters", _g_get_maxVisibleCharacters);
		Utils.RegisterFunc(L, -2, "maxVisibleWords", _g_get_maxVisibleWords);
		Utils.RegisterFunc(L, -2, "maxVisibleLines", _g_get_maxVisibleLines);
		Utils.RegisterFunc(L, -2, "useMaxVisibleDescender", _g_get_useMaxVisibleDescender);
		Utils.RegisterFunc(L, -2, "pageToDisplay", _g_get_pageToDisplay);
		Utils.RegisterFunc(L, -2, "margin", _g_get_margin);
		Utils.RegisterFunc(L, -2, "textInfo", _g_get_textInfo);
		Utils.RegisterFunc(L, -2, "havePropertiesChanged", _g_get_havePropertiesChanged);
		Utils.RegisterFunc(L, -2, "isUsingLegacyAnimationComponent", _g_get_isUsingLegacyAnimationComponent);
		Utils.RegisterFunc(L, -2, "transform", _g_get_transform);
		Utils.RegisterFunc(L, -2, "rectTransform", _g_get_rectTransform);
		Utils.RegisterFunc(L, -2, "autoSizeTextContainer", _g_get_autoSizeTextContainer);
		Utils.RegisterFunc(L, -2, "mesh", _g_get_mesh);
		Utils.RegisterFunc(L, -2, "isVolumetricText", _g_get_isVolumetricText);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "textBounds", _g_get_textBounds);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "maxWidth", _g_get_maxWidth);
		Utils.RegisterFunc(L, -2, "maxHeight", _g_get_maxHeight);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "renderedWidth", _g_get_renderedWidth);
		Utils.RegisterFunc(L, -2, "renderedHeight", _g_get_renderedHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -2, "supportSpriteEmoji", _g_get_supportSpriteEmoji);
		Utils.RegisterFunc(L, -2, "supportSpriteTagOnly", _g_get_supportSpriteTagOnly);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "textPreprocessor", _s_set_textPreprocessor);
		Utils.RegisterFunc(L, -1, "isRightToLeftText", _s_set_isRightToLeftText);
		Utils.RegisterFunc(L, -1, "font", _s_set_font);
		Utils.RegisterFunc(L, -1, "fontSharedMaterial", _s_set_fontSharedMaterial);
		Utils.RegisterFunc(L, -1, "fontSharedMaterials", _s_set_fontSharedMaterials);
		Utils.RegisterFunc(L, -1, "fontMaterial", _s_set_fontMaterial);
		Utils.RegisterFunc(L, -1, "fontMaterials", _s_set_fontMaterials);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "alpha", _s_set_alpha);
		Utils.RegisterFunc(L, -1, "enableVertexGradient", _s_set_enableVertexGradient);
		Utils.RegisterFunc(L, -1, "colorGradient", _s_set_colorGradient);
		Utils.RegisterFunc(L, -1, "colorGradientPreset", _s_set_colorGradientPreset);
		Utils.RegisterFunc(L, -1, "spriteAsset", _s_set_spriteAsset);
		Utils.RegisterFunc(L, -1, "tintAllSprites", _s_set_tintAllSprites);
		Utils.RegisterFunc(L, -1, "styleSheet", _s_set_styleSheet);
		Utils.RegisterFunc(L, -1, "textStyle", _s_set_textStyle);
		Utils.RegisterFunc(L, -1, "overrideColorTags", _s_set_overrideColorTags);
		Utils.RegisterFunc(L, -1, "faceColor", _s_set_faceColor);
		Utils.RegisterFunc(L, -1, "outlineColor", _s_set_outlineColor);
		Utils.RegisterFunc(L, -1, "outlineWidth", _s_set_outlineWidth);
		Utils.RegisterFunc(L, -1, "fontSize", _s_set_fontSize);
		Utils.RegisterFunc(L, -1, "fontWeight", _s_set_fontWeight);
		Utils.RegisterFunc(L, -1, "enableAutoSizing", _s_set_enableAutoSizing);
		Utils.RegisterFunc(L, -1, "fontSizeMin", _s_set_fontSizeMin);
		Utils.RegisterFunc(L, -1, "fontSizeMax", _s_set_fontSizeMax);
		Utils.RegisterFunc(L, -1, "fontStyle", _s_set_fontStyle);
		Utils.RegisterFunc(L, -1, "horizontalAlignment", _s_set_horizontalAlignment);
		Utils.RegisterFunc(L, -1, "verticalAlignment", _s_set_verticalAlignment);
		Utils.RegisterFunc(L, -1, "alignment", _s_set_alignment);
		Utils.RegisterFunc(L, -1, "characterSpacing", _s_set_characterSpacing);
		Utils.RegisterFunc(L, -1, "wordSpacing", _s_set_wordSpacing);
		Utils.RegisterFunc(L, -1, "lineSpacing", _s_set_lineSpacing);
		Utils.RegisterFunc(L, -1, "lineSpacingAdjustment", _s_set_lineSpacingAdjustment);
		Utils.RegisterFunc(L, -1, "paragraphSpacing", _s_set_paragraphSpacing);
		Utils.RegisterFunc(L, -1, "characterWidthAdjustment", _s_set_characterWidthAdjustment);
		Utils.RegisterFunc(L, -1, "enableWordWrapping", _s_set_enableWordWrapping);
		Utils.RegisterFunc(L, -1, "wordWrappingRatios", _s_set_wordWrappingRatios);
		Utils.RegisterFunc(L, -1, "overflowMode", _s_set_overflowMode);
		Utils.RegisterFunc(L, -1, "linkedTextComponent", _s_set_linkedTextComponent);
		Utils.RegisterFunc(L, -1, "enableKerning", _s_set_enableKerning);
		Utils.RegisterFunc(L, -1, "extraPadding", _s_set_extraPadding);
		Utils.RegisterFunc(L, -1, "richText", _s_set_richText);
		Utils.RegisterFunc(L, -1, "parseCtrlCharacters", _s_set_parseCtrlCharacters);
		Utils.RegisterFunc(L, -1, "isOverlay", _s_set_isOverlay);
		Utils.RegisterFunc(L, -1, "isOrthographic", _s_set_isOrthographic);
		Utils.RegisterFunc(L, -1, "enableCulling", _s_set_enableCulling);
		Utils.RegisterFunc(L, -1, "ignoreVisibility", _s_set_ignoreVisibility);
		Utils.RegisterFunc(L, -1, "horizontalMapping", _s_set_horizontalMapping);
		Utils.RegisterFunc(L, -1, "verticalMapping", _s_set_verticalMapping);
		Utils.RegisterFunc(L, -1, "mappingUvLineOffset", _s_set_mappingUvLineOffset);
		Utils.RegisterFunc(L, -1, "renderMode", _s_set_renderMode);
		Utils.RegisterFunc(L, -1, "geometrySortingOrder", _s_set_geometrySortingOrder);
		Utils.RegisterFunc(L, -1, "isTextObjectScaleStatic", _s_set_isTextObjectScaleStatic);
		Utils.RegisterFunc(L, -1, "vertexBufferAutoSizeReduction", _s_set_vertexBufferAutoSizeReduction);
		Utils.RegisterFunc(L, -1, "firstVisibleCharacter", _s_set_firstVisibleCharacter);
		Utils.RegisterFunc(L, -1, "maxVisibleCharacters", _s_set_maxVisibleCharacters);
		Utils.RegisterFunc(L, -1, "maxVisibleWords", _s_set_maxVisibleWords);
		Utils.RegisterFunc(L, -1, "maxVisibleLines", _s_set_maxVisibleLines);
		Utils.RegisterFunc(L, -1, "useMaxVisibleDescender", _s_set_useMaxVisibleDescender);
		Utils.RegisterFunc(L, -1, "pageToDisplay", _s_set_pageToDisplay);
		Utils.RegisterFunc(L, -1, "margin", _s_set_margin);
		Utils.RegisterFunc(L, -1, "havePropertiesChanged", _s_set_havePropertiesChanged);
		Utils.RegisterFunc(L, -1, "isUsingLegacyAnimationComponent", _s_set_isUsingLegacyAnimationComponent);
		Utils.RegisterFunc(L, -1, "autoSizeTextContainer", _s_set_autoSizeTextContainer);
		Utils.RegisterFunc(L, -1, "isVolumetricText", _s_set_isVolumetricText);
		Utils.RegisterFunc(L, -1, "supportSpriteEmoji", _s_set_supportSpriteEmoji);
		Utils.RegisterFunc(L, -1, "supportSpriteTagOnly", _s_set_supportSpriteTagOnly);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "isArabic", _m_isArabic_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnFontAssetRequest", _e_OnFontAssetRequest);
		Utils.RegisterFunc(L, -4, "OnSpriteAssetRequest", _e_OnSpriteAssetRequest);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "TMPro.TMP_Text does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceMeshUpdate(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				bool ignoreActiveState = Lua.lua_toboolean(L, 2);
				bool forceTextReparsing = Lua.lua_toboolean(L, 3);
				tMP_Text.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool ignoreActiveState2 = Lua.lua_toboolean(L, 2);
				tMP_Text.ForceMeshUpdate(ignoreActiveState2);
				return 0;
			}
			if (num == 1)
			{
				tMP_Text.ForceMeshUpdate();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.ForceMeshUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGeometry(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
			int index = Lua.xlua_tointeger(L, 3);
			tMP_Text.UpdateGeometry(mesh, index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateVertexData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				tMP_Text.UpdateVertexData();
				return 0;
			case 2:
				if (objectTranslator.Assignable<TMP_VertexDataUpdateFlags>(L, 2))
				{
					objectTranslator.Get(L, 2, out TMP_VertexDataUpdateFlags v);
					tMP_Text.UpdateVertexData(v);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.UpdateVertexData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			Vector3[] vertices = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
			tMP_Text.SetVertices(vertices);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMeshPadding(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateMeshPadding();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			float duration = (float)Lua.lua_tonumber(L, 3);
			bool ignoreTimeScale = Lua.lua_toboolean(L, 4);
			bool useAlpha = Lua.lua_toboolean(L, 5);
			tMP_Text.CrossFadeColor(val, duration, ignoreTimeScale, useAlpha);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeAlpha(IntPtr L)
	{
		try
		{
			TMP_Text obj = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float alpha = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			bool ignoreTimeScale = Lua.lua_toboolean(L, 4);
			obj.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			return 0;
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
			bool value = TMP_Text.isArabic(Lua.xlua_touint(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<StringBuilder>(L, 2))
			{
				StringBuilder text = (StringBuilder)objectTranslator.GetObject(L, 2, typeof(StringBuilder));
				tMP_Text.SetText(text);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<char[]>(L, 2))
			{
				char[] text2 = (char[])objectTranslator.GetObject(L, 2, typeof(char[]));
				tMP_Text.SetText(text2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string sourceText = Lua.lua_tostring(L, 2);
				bool syncTextInputBox = Lua.lua_toboolean(L, 3);
				tMP_Text.SetText(sourceText, syncTextInputBox);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string sourceText2 = Lua.lua_tostring(L, 2);
				tMP_Text.SetText(sourceText2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string sourceText3 = Lua.lua_tostring(L, 2);
				float arg = (float)Lua.lua_tonumber(L, 3);
				tMP_Text.SetText(sourceText3, arg);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string sourceText4 = Lua.lua_tostring(L, 2);
				float arg2 = (float)Lua.lua_tonumber(L, 3);
				float arg3 = (float)Lua.lua_tonumber(L, 4);
				tMP_Text.SetText(sourceText4, arg2, arg3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<char[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				char[] sourceText5 = (char[])objectTranslator.GetObject(L, 2, typeof(char[]));
				int start = Lua.xlua_tointeger(L, 3);
				int length = Lua.xlua_tointeger(L, 4);
				tMP_Text.SetText(sourceText5, start, length);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string sourceText6 = Lua.lua_tostring(L, 2);
				float arg4 = (float)Lua.lua_tonumber(L, 3);
				float arg5 = (float)Lua.lua_tonumber(L, 4);
				float arg6 = (float)Lua.lua_tonumber(L, 5);
				tMP_Text.SetText(sourceText6, arg4, arg5, arg6);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string sourceText7 = Lua.lua_tostring(L, 2);
				float arg7 = (float)Lua.lua_tonumber(L, 3);
				float arg8 = (float)Lua.lua_tonumber(L, 4);
				float arg9 = (float)Lua.lua_tonumber(L, 5);
				float arg10 = (float)Lua.lua_tonumber(L, 6);
				tMP_Text.SetText(sourceText7, arg7, arg8, arg9, arg10);
				return 0;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				string sourceText8 = Lua.lua_tostring(L, 2);
				float arg11 = (float)Lua.lua_tonumber(L, 3);
				float arg12 = (float)Lua.lua_tonumber(L, 4);
				float arg13 = (float)Lua.lua_tonumber(L, 5);
				float arg14 = (float)Lua.lua_tonumber(L, 6);
				float arg15 = (float)Lua.lua_tonumber(L, 7);
				tMP_Text.SetText(sourceText8, arg11, arg12, arg13, arg14, arg15);
				return 0;
			}
			if (num == 8 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				string sourceText9 = Lua.lua_tostring(L, 2);
				float arg16 = (float)Lua.lua_tonumber(L, 3);
				float arg17 = (float)Lua.lua_tonumber(L, 4);
				float arg18 = (float)Lua.lua_tonumber(L, 5);
				float arg19 = (float)Lua.lua_tonumber(L, 6);
				float arg20 = (float)Lua.lua_tonumber(L, 7);
				float arg21 = (float)Lua.lua_tonumber(L, 8);
				tMP_Text.SetText(sourceText9, arg16, arg17, arg18, arg19, arg20, arg21);
				return 0;
			}
			if (num == 9 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				string sourceText10 = Lua.lua_tostring(L, 2);
				float arg22 = (float)Lua.lua_tonumber(L, 3);
				float arg23 = (float)Lua.lua_tonumber(L, 4);
				float arg24 = (float)Lua.lua_tonumber(L, 5);
				float arg25 = (float)Lua.lua_tonumber(L, 6);
				float arg26 = (float)Lua.lua_tonumber(L, 7);
				float arg27 = (float)Lua.lua_tonumber(L, 8);
				float arg28 = (float)Lua.lua_tonumber(L, 9);
				tMP_Text.SetText(sourceText10, arg22, arg23, arg24, arg25, arg26, arg27, arg28);
				return 0;
			}
			if (num == 10 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				string sourceText11 = Lua.lua_tostring(L, 2);
				float arg29 = (float)Lua.lua_tonumber(L, 3);
				float arg30 = (float)Lua.lua_tonumber(L, 4);
				float arg31 = (float)Lua.lua_tonumber(L, 5);
				float arg32 = (float)Lua.lua_tonumber(L, 6);
				float arg33 = (float)Lua.lua_tonumber(L, 7);
				float arg34 = (float)Lua.lua_tonumber(L, 8);
				float arg35 = (float)Lua.lua_tonumber(L, 9);
				float arg36 = (float)Lua.lua_tonumber(L, 10);
				tMP_Text.SetText(sourceText11, arg29, arg30, arg31, arg32, arg33, arg34, arg35, arg36);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.SetText!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCharArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<char[]>(L, 2))
			{
				char[] charArray = (char[])objectTranslator.GetObject(L, 2, typeof(char[]));
				tMP_Text.SetCharArray(charArray);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<char[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				char[] sourceText = (char[])objectTranslator.GetObject(L, 2, typeof(char[]));
				int start = Lua.xlua_tointeger(L, 3);
				int length = Lua.xlua_tointeger(L, 4);
				tMP_Text.SetCharArray(sourceText, start, length);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.SetCharArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPreferredValues(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
			{
				Vector2 preferredValues2 = tMP_Text.GetPreferredValues();
				objectTranslator.PushUnityEngineVector2(L, preferredValues2);
				return 1;
			}
			case 3:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
				{
					float width = (float)Lua.lua_tonumber(L, 2);
					float height = (float)Lua.lua_tonumber(L, 3);
					Vector2 preferredValues = tMP_Text.GetPreferredValues(width, height);
					objectTranslator.PushUnityEngineVector2(L, preferredValues);
					return 1;
				}
				break;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string text = Lua.lua_tostring(L, 2);
				Vector2 preferredValues3 = tMP_Text.GetPreferredValues(text);
				objectTranslator.PushUnityEngineVector2(L, preferredValues3);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string text2 = Lua.lua_tostring(L, 2);
				float width2 = (float)Lua.lua_tonumber(L, 3);
				float height2 = (float)Lua.lua_tonumber(L, 4);
				Vector2 preferredValues4 = tMP_Text.GetPreferredValues(text2, width2, height2);
				objectTranslator.PushUnityEngineVector2(L, preferredValues4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.GetPreferredValues!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderedValues(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Vector2 renderedValues2 = tMP_Text.GetRenderedValues();
				objectTranslator.PushUnityEngineVector2(L, renderedValues2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool onlyVisibleCharacters = Lua.lua_toboolean(L, 2);
					Vector2 renderedValues = tMP_Text.GetRenderedValues(onlyVisibleCharacters);
					objectTranslator.PushUnityEngineVector2(L, renderedValues);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.GetRenderedValues!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTextInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text obj = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			string text = Lua.lua_tostring(L, 2);
			TMP_TextInfo textInfo = obj.GetTextInfo(text);
			objectTranslator.Push(L, textInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ComputeMarginSize(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ComputeMarginSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearMesh(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				tMP_Text.ClearMesh();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool uploadGeometry = Lua.lua_toboolean(L, 2);
					tMP_Text.ClearMesh(uploadGeometry);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.ClearMesh!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetParsedText(IntPtr L)
	{
		try
		{
			string parsedText = ((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetParsedText();
			Lua.lua_pushstring(L, parsedText);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddInputHtmlTag(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				string tagStrStart = Lua.lua_tostring(L, 2);
				string tagStrEnd = Lua.lua_tostring(L, 3);
				int tagIndexStart = Lua.xlua_tointeger(L, 4);
				int tagIndexEnd = Lua.xlua_tointeger(L, 5);
				bool isArabicFixIngore = Lua.lua_toboolean(L, 6);
				tMP_Text.AddInputHtmlTag(tagStrStart, tagStrEnd, tagIndexStart, tagIndexEnd, isArabicFixIngore);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string tagStrStart2 = Lua.lua_tostring(L, 2);
				string tagStrEnd2 = Lua.lua_tostring(L, 3);
				int tagIndexStart2 = Lua.xlua_tointeger(L, 4);
				int tagIndexEnd2 = Lua.xlua_tointeger(L, 5);
				tMP_Text.AddInputHtmlTag(tagStrStart2, tagStrEnd2, tagIndexStart2, tagIndexEnd2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.AddInputHtmlTag!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInputHtmlTagEmpty(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetInputHtmlTagEmpty();
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, tMP_Text.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textPreprocessor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, tMP_Text.textPreprocessor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRightToLeftText(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isRightToLeftText);
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.font);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontSharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.fontSharedMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontSharedMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.fontSharedMaterials);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.fontMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.fontMaterials);
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, tMP_Text.color);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.alpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableVertexGradient(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.enableVertexGradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.colorGradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorGradientPreset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.colorGradientPreset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.spriteAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tintAllSprites(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.tintAllSprites);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_styleSheet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.styleSheet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textStyle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.textStyle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideColorTags(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.overrideColorTags);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_faceColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.faceColor);
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.outlineColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outlineWidth(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.outlineWidth);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.fontSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.fontWeight);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.pixelsPerUnit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableAutoSizing(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.enableAutoSizing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontSizeMin(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.fontSizeMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fontSizeMax(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.fontSizeMax);
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.fontStyle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isUsingBold(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isUsingBold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.horizontalAlignment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.verticalAlignment);
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushTMProTextAlignmentOptions(L, tMP_Text.alignment);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.characterSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wordSpacing(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.wordSpacing);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.lineSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineSpacingAdjustment(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.lineSpacingAdjustment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_paragraphSpacing(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.paragraphSpacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_characterWidthAdjustment(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.characterWidthAdjustment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableWordWrapping(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.enableWordWrapping);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wordWrappingRatios(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.wordWrappingRatios);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overflowMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.overflowMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isTextOverflowing(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isTextOverflowing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_firstOverflowCharacterIndex(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.firstOverflowCharacterIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_linkedTextComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.linkedTextComponent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isTextTruncated(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isTextTruncated);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableKerning(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.enableKerning);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extraPadding(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.extraPadding);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.richText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parseCtrlCharacters(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.parseCtrlCharacters);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isOverlay(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isOverlay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isOrthographic(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isOrthographic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableCulling(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.enableCulling);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ignoreVisibility(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.ignoreVisibility);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalMapping(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.horizontalMapping);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalMapping(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.verticalMapping);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mappingUvLineOffset(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.mappingUvLineOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.renderMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_geometrySortingOrder(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.geometrySortingOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isTextObjectScaleStatic(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isTextObjectScaleStatic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertexBufferAutoSizeReduction(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.vertexBufferAutoSizeReduction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_firstVisibleCharacter(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.firstVisibleCharacter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxVisibleCharacters(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.maxVisibleCharacters);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxVisibleWords(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.maxVisibleWords);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxVisibleLines(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.maxVisibleLines);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useMaxVisibleDescender(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.useMaxVisibleDescender);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pageToDisplay(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.pageToDisplay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_margin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, tMP_Text.margin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.textInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_havePropertiesChanged(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.havePropertiesChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isUsingLegacyAnimationComponent(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isUsingLegacyAnimationComponent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.transform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rectTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.rectTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoSizeTextContainer(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.autoSizeTextContainer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Text.mesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isVolumetricText(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.isVolumetricText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, tMP_Text.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, tMP_Text.textBounds);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.flexibleHeight);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.flexibleWidth);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.minWidth);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.minHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxWidth(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.maxWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxHeight(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.maxHeight);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.preferredWidth);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.preferredHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderedWidth(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.renderedWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderedHeight(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Text.renderedHeight);
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
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Text.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportSpriteEmoji(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.supportSpriteEmoji);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportSpriteTagOnly(IntPtr L)
	{
		try
		{
			TMP_Text tMP_Text = (TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Text.supportSpriteTagOnly);
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
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textPreprocessor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).textPreprocessor = (ITextPreprocessor)objectTranslator.GetObject(L, 2, typeof(ITextPreprocessor));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isRightToLeftText(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isRightToLeftText = Lua.lua_toboolean(L, 2);
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
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).font = (TMP_FontAsset)objectTranslator.GetObject(L, 2, typeof(TMP_FontAsset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontSharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).fontSharedMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontSharedMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).fontSharedMaterials = (Material[])objectTranslator.GetObject(L, 2, typeof(Material[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).fontMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).fontMaterials = (Material[])objectTranslator.GetObject(L, 2, typeof(Material[]));
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			tMP_Text.color = val;
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
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alpha = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableVertexGradient(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableVertexGradient = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VertexGradient v);
			tMP_Text.colorGradient = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorGradientPreset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).colorGradientPreset = (TMP_ColorGradient)objectTranslator.GetObject(L, 2, typeof(TMP_ColorGradient));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).spriteAsset = (TMP_SpriteAsset)objectTranslator.GetObject(L, 2, typeof(TMP_SpriteAsset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tintAllSprites(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tintAllSprites = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_styleSheet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).styleSheet = (TMP_StyleSheet)objectTranslator.GetObject(L, 2, typeof(TMP_StyleSheet));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textStyle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).textStyle = (TMP_Style)objectTranslator.GetObject(L, 2, typeof(TMP_Style));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideColorTags(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideColorTags = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_faceColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color32 v);
			tMP_Text.faceColor = v;
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color32 v);
			tMP_Text.outlineColor = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outlineWidth(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).outlineWidth = (float)Lua.lua_tonumber(L, 2);
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
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fontSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FontWeight v);
			tMP_Text.fontWeight = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableAutoSizing(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableAutoSizing = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontSizeMin(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fontSizeMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fontSizeMax(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fontSizeMax = (float)Lua.lua_tonumber(L, 2);
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FontStyles v);
			tMP_Text.fontStyle = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out HorizontalAlignmentOptions v);
			tMP_Text.horizontalAlignment = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VerticalAlignmentOptions v);
			tMP_Text.verticalAlignment = v;
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
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextAlignmentOptions val);
			tMP_Text.alignment = val;
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
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).characterSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wordSpacing(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).wordSpacing = (float)Lua.lua_tonumber(L, 2);
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
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineSpacingAdjustment(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineSpacingAdjustment = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_paragraphSpacing(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).paragraphSpacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_characterWidthAdjustment(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).characterWidthAdjustment = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableWordWrapping(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableWordWrapping = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wordWrappingRatios(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).wordWrappingRatios = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overflowMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextOverflowModes v);
			tMP_Text.overflowMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_linkedTextComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Text)objectTranslator.FastGetCSObj(L, 1)).linkedTextComponent = (TMP_Text)objectTranslator.GetObject(L, 2, typeof(TMP_Text));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableKerning(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableKerning = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extraPadding(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).extraPadding = Lua.lua_toboolean(L, 2);
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
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).richText = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_parseCtrlCharacters(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).parseCtrlCharacters = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isOverlay(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isOverlay = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isOrthographic(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isOrthographic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableCulling(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableCulling = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ignoreVisibility(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ignoreVisibility = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalMapping(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextureMappingOptions v);
			tMP_Text.horizontalMapping = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalMapping(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextureMappingOptions v);
			tMP_Text.verticalMapping = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mappingUvLineOffset(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mappingUvLineOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextRenderFlags v);
			tMP_Text.renderMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_geometrySortingOrder(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VertexSortingOrder v);
			tMP_Text.geometrySortingOrder = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isTextObjectScaleStatic(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isTextObjectScaleStatic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_vertexBufferAutoSizeReduction(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).vertexBufferAutoSizeReduction = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_firstVisibleCharacter(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).firstVisibleCharacter = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxVisibleCharacters(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxVisibleCharacters = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxVisibleWords(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxVisibleWords = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxVisibleLines(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxVisibleLines = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useMaxVisibleDescender(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useMaxVisibleDescender = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pageToDisplay(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pageToDisplay = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_margin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			tMP_Text.margin = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_havePropertiesChanged(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).havePropertiesChanged = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isUsingLegacyAnimationComponent(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isUsingLegacyAnimationComponent = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoSizeTextContainer(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoSizeTextContainer = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isVolumetricText(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isVolumetricText = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportSpriteEmoji(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportSpriteEmoji = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportSpriteTagOnly(IntPtr L)
	{
		try
		{
			((TMP_Text)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportSpriteTagOnly = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnPreRenderText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			TMP_Text tMP_Text = (TMP_Text)objectTranslator.FastGetCSObj(L, 1);
			Action<TMP_TextInfo> action = objectTranslator.GetDelegate<Action<TMP_TextInfo>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<TMPro.TMP_TextInfo>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					tMP_Text.OnPreRenderText += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					tMP_Text.OnPreRenderText -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.OnPreRenderText!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnFontAssetRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Func<int, string, TMP_FontAsset> func = objectTranslator.GetDelegate<Func<int, string, TMP_FontAsset>>(L, 2);
			if (func == null)
			{
				return Lua.luaL_error(L, "#2 need System.Func<int, string, TMPro.TMP_FontAsset>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				TMP_Text.OnFontAssetRequest += func;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				TMP_Text.OnFontAssetRequest -= func;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.OnFontAssetRequest!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnSpriteAssetRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Func<int, string, TMP_SpriteAsset> func = objectTranslator.GetDelegate<Func<int, string, TMP_SpriteAsset>>(L, 2);
			if (func == null)
			{
				return Lua.luaL_error(L, "#2 need System.Func<int, string, TMPro.TMP_SpriteAsset>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				TMP_Text.OnSpriteAssetRequest += func;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				TMP_Text.OnSpriteAssetRequest -= func;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Text.OnSpriteAssetRequest!");
	}
}
