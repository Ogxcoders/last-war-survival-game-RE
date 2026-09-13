using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Image);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 15, 23, 13);
		Utils.RegisterFunc(L, -3, "DisableSpriteOptimizations", _m_DisableSpriteOptimizations);
		Utils.RegisterFunc(L, -3, "OnBeforeSerialize", _m_OnBeforeSerialize);
		Utils.RegisterFunc(L, -3, "OnAfterDeserialize", _m_OnAfterDeserialize);
		Utils.RegisterFunc(L, -3, "SetNativeSize", _m_SetNativeSize);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
		Utils.RegisterFunc(L, -3, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
		Utils.RegisterFunc(L, -3, "IsRaycastLocationValid", _m_IsRaycastLocationValid);
		Utils.RegisterFunc(L, -3, "DOColor", _m_DOColor);
		Utils.RegisterFunc(L, -3, "DOFade", _m_DOFade);
		Utils.RegisterFunc(L, -3, "DOFillAmount", _m_DOFillAmount);
		Utils.RegisterFunc(L, -3, "DOGradientColor", _m_DOGradientColor);
		Utils.RegisterFunc(L, -3, "DOBlendableColor", _m_DOBlendableColor);
		Utils.RegisterFunc(L, -3, "LoadSprite", _m_LoadSprite);
		Utils.RegisterFunc(L, -3, "LoadSpriteAsync", _m_LoadSpriteAsync);
		Utils.RegisterFunc(L, -3, "SetAlpha", _m_SetAlpha);
		Utils.RegisterFunc(L, -2, "sprite", _g_get_sprite);
		Utils.RegisterFunc(L, -2, "overrideSprite", _g_get_overrideSprite);
		Utils.RegisterFunc(L, -2, "type", _g_get_type);
		Utils.RegisterFunc(L, -2, "preserveAspect", _g_get_preserveAspect);
		Utils.RegisterFunc(L, -2, "fillCenter", _g_get_fillCenter);
		Utils.RegisterFunc(L, -2, "fillMethod", _g_get_fillMethod);
		Utils.RegisterFunc(L, -2, "fillAmount", _g_get_fillAmount);
		Utils.RegisterFunc(L, -2, "fillClockwise", _g_get_fillClockwise);
		Utils.RegisterFunc(L, -2, "fillOrigin", _g_get_fillOrigin);
		Utils.RegisterFunc(L, -2, "alphaHitTestMinimumThreshold", _g_get_alphaHitTestMinimumThreshold);
		Utils.RegisterFunc(L, -2, "useSpriteMesh", _g_get_useSpriteMesh);
		Utils.RegisterFunc(L, -2, "mainTexture", _g_get_mainTexture);
		Utils.RegisterFunc(L, -2, "hasBorder", _g_get_hasBorder);
		Utils.RegisterFunc(L, -2, "pixelsPerUnitMultiplier", _g_get_pixelsPerUnitMultiplier);
		Utils.RegisterFunc(L, -2, "pixelsPerUnit", _g_get_pixelsPerUnit);
		Utils.RegisterFunc(L, -2, "material", _g_get_material);
		Utils.RegisterFunc(L, -2, "minWidth", _g_get_minWidth);
		Utils.RegisterFunc(L, -2, "preferredWidth", _g_get_preferredWidth);
		Utils.RegisterFunc(L, -2, "flexibleWidth", _g_get_flexibleWidth);
		Utils.RegisterFunc(L, -2, "minHeight", _g_get_minHeight);
		Utils.RegisterFunc(L, -2, "preferredHeight", _g_get_preferredHeight);
		Utils.RegisterFunc(L, -2, "flexibleHeight", _g_get_flexibleHeight);
		Utils.RegisterFunc(L, -2, "layoutPriority", _g_get_layoutPriority);
		Utils.RegisterFunc(L, -1, "sprite", _s_set_sprite);
		Utils.RegisterFunc(L, -1, "overrideSprite", _s_set_overrideSprite);
		Utils.RegisterFunc(L, -1, "type", _s_set_type);
		Utils.RegisterFunc(L, -1, "preserveAspect", _s_set_preserveAspect);
		Utils.RegisterFunc(L, -1, "fillCenter", _s_set_fillCenter);
		Utils.RegisterFunc(L, -1, "fillMethod", _s_set_fillMethod);
		Utils.RegisterFunc(L, -1, "fillAmount", _s_set_fillAmount);
		Utils.RegisterFunc(L, -1, "fillClockwise", _s_set_fillClockwise);
		Utils.RegisterFunc(L, -1, "fillOrigin", _s_set_fillOrigin);
		Utils.RegisterFunc(L, -1, "alphaHitTestMinimumThreshold", _s_set_alphaHitTestMinimumThreshold);
		Utils.RegisterFunc(L, -1, "useSpriteMesh", _s_set_useSpriteMesh);
		Utils.RegisterFunc(L, -1, "pixelsPerUnitMultiplier", _s_set_pixelsPerUnitMultiplier);
		Utils.RegisterFunc(L, -1, "material", _s_set_material);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "defaultETC1GraphicMaterial", _g_get_defaultETC1GraphicMaterial);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Image does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisableSpriteOptimizations(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisableSpriteOptimizations();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeforeSerialize(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeforeSerialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAfterDeserialize(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAfterDeserialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNativeSize(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetNativeSize();
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
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputHorizontal();
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
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalculateLayoutInputVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRaycastLocationValid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Camera eventCamera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = image.IsRaycastLocationValid(val, eventCamera);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			Image target = (Image)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_DOFade(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image target = (Image)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_DOFillAmount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image target = (Image)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOFillAmount(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOGradientColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image target = (Image)objectTranslator.FastGetCSObj(L, 1);
			UnityEngine.Gradient gradient = (UnityEngine.Gradient)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Gradient));
			float duration = (float)Lua.lua_tonumber(L, 3);
			Sequence o = target.DOGradientColor(gradient, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image target = (Image)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_LoadSprite(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				image.LoadSprite(spritePath, defaultSprite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				image.LoadSprite(spritePath2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Image.LoadSprite!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSpriteAsync(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				image.LoadSpriteAsync(spritePath, defaultSprite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				image.LoadSpriteAsync(spritePath2);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<Sprite>>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string spritePath3 = Lua.lua_tostring(L, 2);
				Action<Sprite> completeCallback = objectTranslator.GetDelegate<Action<Sprite>>(L, 3);
				string defaultSprite2 = Lua.lua_tostring(L, 4);
				image.LoadSpriteAsync(spritePath3, completeCallback, defaultSprite2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<Sprite>>(L, 3))
			{
				string spritePath4 = Lua.lua_tostring(L, 2);
				Action<Sprite> completeCallback2 = objectTranslator.GetDelegate<Action<Sprite>>(L, 3);
				image.LoadSpriteAsync(spritePath4, completeCallback2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Image.LoadSpriteAsync!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAlpha(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float alpha = (float)Lua.lua_tonumber(L, 2);
			image.SetAlpha(alpha);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, image.sprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, image.overrideSprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIImageType(L, image.type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preserveAspect(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, image.preserveAspect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillCenter(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, image.fillCenter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillMethod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIImageFillMethod(L, image.fillMethod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillAmount(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.fillAmount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillClockwise(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, image.fillClockwise);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillOrigin(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, image.fillOrigin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alphaHitTestMinimumThreshold(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.alphaHitTestMinimumThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useSpriteMesh(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, image.useSpriteMesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultETC1GraphicMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Image.defaultETC1GraphicMaterial);
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
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, image.mainTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasBorder(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, image.hasBorder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelsPerUnitMultiplier(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.pixelsPerUnitMultiplier);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.pixelsPerUnit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_material(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, image.material);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.minWidth);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.preferredWidth);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.flexibleWidth);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.minHeight);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.preferredHeight);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, image.flexibleHeight);
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
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, image.layoutPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Image)objectTranslator.FastGetCSObj(L, 1)).sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Image)objectTranslator.FastGetCSObj(L, 1)).overrideSprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Image.Type val);
			image.type = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_preserveAspect(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).preserveAspect = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillCenter(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fillCenter = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillMethod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Image.FillMethod val);
			image.fillMethod = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillAmount(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fillAmount = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillClockwise(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fillClockwise = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillOrigin(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fillOrigin = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alphaHitTestMinimumThreshold(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alphaHitTestMinimumThreshold = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useSpriteMesh(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useSpriteMesh = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pixelsPerUnitMultiplier(IntPtr L)
	{
		try
		{
			((Image)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pixelsPerUnitMultiplier = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_material(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Image)objectTranslator.FastGetCSObj(L, 1)).material = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
