using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIGraphicWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Graphic);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 33, 10, 3);
		Utils.RegisterFunc(L, -3, "SetAllDirty", _m_SetAllDirty);
		Utils.RegisterFunc(L, -3, "SetLayoutDirty", _m_SetLayoutDirty);
		Utils.RegisterFunc(L, -3, "SetVerticesDirty", _m_SetVerticesDirty);
		Utils.RegisterFunc(L, -3, "SetMaterialDirty", _m_SetMaterialDirty);
		Utils.RegisterFunc(L, -3, "OnCullingChanged", _m_OnCullingChanged);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "SetNativeSize", _m_SetNativeSize);
		Utils.RegisterFunc(L, -3, "Raycast", _m_Raycast);
		Utils.RegisterFunc(L, -3, "PixelAdjustPoint", _m_PixelAdjustPoint);
		Utils.RegisterFunc(L, -3, "GetPixelAdjustedRect", _m_GetPixelAdjustedRect);
		Utils.RegisterFunc(L, -3, "CrossFadeColor", _m_CrossFadeColor);
		Utils.RegisterFunc(L, -3, "CrossFadeAlpha", _m_CrossFadeAlpha);
		Utils.RegisterFunc(L, -3, "RegisterDirtyLayoutCallback", _m_RegisterDirtyLayoutCallback);
		Utils.RegisterFunc(L, -3, "UnregisterDirtyLayoutCallback", _m_UnregisterDirtyLayoutCallback);
		Utils.RegisterFunc(L, -3, "RegisterDirtyVerticesCallback", _m_RegisterDirtyVerticesCallback);
		Utils.RegisterFunc(L, -3, "UnregisterDirtyVerticesCallback", _m_UnregisterDirtyVerticesCallback);
		Utils.RegisterFunc(L, -3, "RegisterDirtyMaterialCallback", _m_RegisterDirtyMaterialCallback);
		Utils.RegisterFunc(L, -3, "UnregisterDirtyMaterialCallback", _m_UnregisterDirtyMaterialCallback);
		Utils.RegisterFunc(L, -3, "DOColor", _m_DOColor);
		Utils.RegisterFunc(L, -3, "DOFade", _m_DOFade);
		Utils.RegisterFunc(L, -3, "DOBlendableColor", _m_DOBlendableColor);
		Utils.RegisterFunc(L, -3, "Set_color", _m_Set_color);
		Utils.RegisterFunc(L, -3, "Set_color_r", _m_Set_color_r);
		Utils.RegisterFunc(L, -3, "Set_color_g", _m_Set_color_g);
		Utils.RegisterFunc(L, -3, "Set_color_b", _m_Set_color_b);
		Utils.RegisterFunc(L, -3, "Set_color_a", _m_Set_color_a);
		Utils.RegisterFunc(L, -3, "Get_color", _m_Get_color);
		Utils.RegisterFunc(L, -3, "Get_color_r", _m_Get_color_r);
		Utils.RegisterFunc(L, -3, "Get_color_g", _m_Get_color_g);
		Utils.RegisterFunc(L, -3, "Get_color_b", _m_Get_color_b);
		Utils.RegisterFunc(L, -3, "Get_color_a", _m_Get_color_a);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "raycastTarget", _g_get_raycastTarget);
		Utils.RegisterFunc(L, -2, "depth", _g_get_depth);
		Utils.RegisterFunc(L, -2, "rectTransform", _g_get_rectTransform);
		Utils.RegisterFunc(L, -2, "canvas", _g_get_canvas);
		Utils.RegisterFunc(L, -2, "canvasRenderer", _g_get_canvasRenderer);
		Utils.RegisterFunc(L, -2, "defaultMaterial", _g_get_defaultMaterial);
		Utils.RegisterFunc(L, -2, "material", _g_get_material);
		Utils.RegisterFunc(L, -2, "materialForRendering", _g_get_materialForRendering);
		Utils.RegisterFunc(L, -2, "mainTexture", _g_get_mainTexture);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "raycastTarget", _s_set_raycastTarget);
		Utils.RegisterFunc(L, -1, "material", _s_set_material);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "defaultGraphicMaterial", _g_get_defaultGraphicMaterial);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Graphic does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAllDirty(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAllDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutDirty(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVerticesDirty(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetVerticesDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaterialDirty(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetMaterialDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCullingChanged(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnCullingChanged();
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
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			graphic.Rebuild(v);
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
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
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
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
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
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetNativeSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Raycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Camera eventCamera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = graphic.Raycast(val, eventCamera);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PixelAdjustPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Vector2 val2 = graphic.PixelAdjustPoint(val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPixelAdjustedRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rect pixelAdjustedRect = ((Graphic)objectTranslator.FastGetCSObj(L, 1)).GetPixelAdjustedRect();
			objectTranslator.Push(L, pixelAdjustedRect);
			return 1;
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
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Color val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool ignoreTimeScale = Lua.lua_toboolean(L, 4);
				bool useAlpha = Lua.lua_toboolean(L, 5);
				graphic.CrossFadeColor(val, duration, ignoreTimeScale, useAlpha);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Color>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Color val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				bool ignoreTimeScale2 = Lua.lua_toboolean(L, 4);
				bool useAlpha2 = Lua.lua_toboolean(L, 5);
				bool useRGB = Lua.lua_toboolean(L, 6);
				graphic.CrossFadeColor(val2, duration2, ignoreTimeScale2, useAlpha2, useRGB);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Graphic.CrossFadeColor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeAlpha(IntPtr L)
	{
		try
		{
			Graphic obj = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_RegisterDirtyLayoutCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			graphic.RegisterDirtyLayoutCallback(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterDirtyLayoutCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			graphic.UnregisterDirtyLayoutCallback(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterDirtyVerticesCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			graphic.RegisterDirtyVerticesCallback(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterDirtyVerticesCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			graphic.UnregisterDirtyVerticesCallback(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterDirtyMaterialCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			graphic.RegisterDirtyMaterialCallback(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterDirtyMaterialCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			graphic.UnregisterDirtyMaterialCallback(action);
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
			Graphic target = (Graphic)objectTranslator.FastGetCSObj(L, 1);
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
			Graphic target = (Graphic)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_DOBlendableColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic target = (Graphic)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_Set_color(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			graphic.Set_color(r, g, b, a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_r(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			graphic.Set_color_r(r);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_g(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float g = (float)Lua.lua_tonumber(L, 2);
			graphic.Set_color_g(g);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_b(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float b = (float)Lua.lua_tonumber(L, 2);
			graphic.Set_color_b(b);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_a(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float a = (float)Lua.lua_tonumber(L, 2);
			graphic.Set_color_a(a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color(out var r, out var g, out var b, out var a);
			Lua.lua_pushnumber(L, r);
			Lua.lua_pushnumber(L, g);
			Lua.lua_pushnumber(L, b);
			Lua.lua_pushnumber(L, a);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_r(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_r(out var r);
			Lua.lua_pushnumber(L, r);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_g(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_g(out var g);
			Lua.lua_pushnumber(L, g);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_b(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_b(out var b);
			Lua.lua_pushnumber(L, b);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_a(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_a(out var a);
			Lua.lua_pushnumber(L, a);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultGraphicMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Graphic.defaultGraphicMaterial);
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
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, graphic.color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_raycastTarget(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, graphic.raycastTarget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_depth(IntPtr L)
	{
		try
		{
			Graphic graphic = (Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, graphic.depth);
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
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.rectTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canvas(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.canvas);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canvasRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.canvasRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.defaultMaterial);
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
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.material);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_materialForRendering(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.materialForRendering);
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
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, graphic.mainTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Graphic graphic = (Graphic)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			graphic.color = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_raycastTarget(IntPtr L)
	{
		try
		{
			((Graphic)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).raycastTarget = Lua.lua_toboolean(L, 2);
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
			((Graphic)objectTranslator.FastGetCSObj(L, 1)).material = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
