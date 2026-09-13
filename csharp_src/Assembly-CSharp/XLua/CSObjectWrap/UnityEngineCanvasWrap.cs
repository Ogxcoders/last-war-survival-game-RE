using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCanvasWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Canvas);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 20, 14);
		Utils.RegisterFunc(L, -2, "renderMode", _g_get_renderMode);
		Utils.RegisterFunc(L, -2, "isRootCanvas", _g_get_isRootCanvas);
		Utils.RegisterFunc(L, -2, "pixelRect", _g_get_pixelRect);
		Utils.RegisterFunc(L, -2, "scaleFactor", _g_get_scaleFactor);
		Utils.RegisterFunc(L, -2, "referencePixelsPerUnit", _g_get_referencePixelsPerUnit);
		Utils.RegisterFunc(L, -2, "overridePixelPerfect", _g_get_overridePixelPerfect);
		Utils.RegisterFunc(L, -2, "pixelPerfect", _g_get_pixelPerfect);
		Utils.RegisterFunc(L, -2, "planeDistance", _g_get_planeDistance);
		Utils.RegisterFunc(L, -2, "renderOrder", _g_get_renderOrder);
		Utils.RegisterFunc(L, -2, "overrideSorting", _g_get_overrideSorting);
		Utils.RegisterFunc(L, -2, "sortingOrder", _g_get_sortingOrder);
		Utils.RegisterFunc(L, -2, "targetDisplay", _g_get_targetDisplay);
		Utils.RegisterFunc(L, -2, "sortingLayerID", _g_get_sortingLayerID);
		Utils.RegisterFunc(L, -2, "cachedSortingLayerValue", _g_get_cachedSortingLayerValue);
		Utils.RegisterFunc(L, -2, "additionalShaderChannels", _g_get_additionalShaderChannels);
		Utils.RegisterFunc(L, -2, "sortingLayerName", _g_get_sortingLayerName);
		Utils.RegisterFunc(L, -2, "rootCanvas", _g_get_rootCanvas);
		Utils.RegisterFunc(L, -2, "renderingDisplaySize", _g_get_renderingDisplaySize);
		Utils.RegisterFunc(L, -2, "worldCamera", _g_get_worldCamera);
		Utils.RegisterFunc(L, -2, "normalizedSortingGridSize", _g_get_normalizedSortingGridSize);
		Utils.RegisterFunc(L, -1, "renderMode", _s_set_renderMode);
		Utils.RegisterFunc(L, -1, "scaleFactor", _s_set_scaleFactor);
		Utils.RegisterFunc(L, -1, "referencePixelsPerUnit", _s_set_referencePixelsPerUnit);
		Utils.RegisterFunc(L, -1, "overridePixelPerfect", _s_set_overridePixelPerfect);
		Utils.RegisterFunc(L, -1, "pixelPerfect", _s_set_pixelPerfect);
		Utils.RegisterFunc(L, -1, "planeDistance", _s_set_planeDistance);
		Utils.RegisterFunc(L, -1, "overrideSorting", _s_set_overrideSorting);
		Utils.RegisterFunc(L, -1, "sortingOrder", _s_set_sortingOrder);
		Utils.RegisterFunc(L, -1, "targetDisplay", _s_set_targetDisplay);
		Utils.RegisterFunc(L, -1, "sortingLayerID", _s_set_sortingLayerID);
		Utils.RegisterFunc(L, -1, "additionalShaderChannels", _s_set_additionalShaderChannels);
		Utils.RegisterFunc(L, -1, "sortingLayerName", _s_set_sortingLayerName);
		Utils.RegisterFunc(L, -1, "worldCamera", _s_set_worldCamera);
		Utils.RegisterFunc(L, -1, "normalizedSortingGridSize", _s_set_normalizedSortingGridSize);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 6, 0, 0);
		Utils.RegisterFunc(L, -4, "GetDefaultCanvasMaterial", _m_GetDefaultCanvasMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetETC1SupportedCanvasMaterial", _m_GetETC1SupportedCanvasMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "ForceUpdateCanvases", _m_ForceUpdateCanvases_xlua_st_);
		Utils.RegisterFunc(L, -4, "preWillRenderCanvases", _e_preWillRenderCanvases);
		Utils.RegisterFunc(L, -4, "willRenderCanvases", _e_willRenderCanvases);
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
				Canvas o = new Canvas();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Canvas constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDefaultCanvasMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material defaultCanvasMaterial = Canvas.GetDefaultCanvasMaterial();
			objectTranslator.Push(L, defaultCanvasMaterial);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetETC1SupportedCanvasMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material eTC1SupportedCanvasMaterial = Canvas.GetETC1SupportedCanvasMaterial();
			objectTranslator.Push(L, eTC1SupportedCanvasMaterial);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceUpdateCanvases_xlua_st_(IntPtr L)
	{
		try
		{
			Canvas.ForceUpdateCanvases();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, canvas.renderMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRootCanvas(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvas.isRootCanvas);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, canvas.pixelRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scaleFactor(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, canvas.scaleFactor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_referencePixelsPerUnit(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, canvas.referencePixelsPerUnit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overridePixelPerfect(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvas.overridePixelPerfect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelPerfect(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvas.pixelPerfect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_planeDistance(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, canvas.planeDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderOrder(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, canvas.renderOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideSorting(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvas.overrideSorting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingOrder(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, canvas.sortingOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetDisplay(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, canvas.targetDisplay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingLayerID(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, canvas.sortingLayerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedSortingLayerValue(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, canvas.cachedSortingLayerValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_additionalShaderChannels(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, canvas.additionalShaderChannels);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingLayerName(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, canvas.sortingLayerName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rootCanvas(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, canvas.rootCanvas);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderingDisplaySize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, canvas.renderingDisplaySize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, canvas.worldCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalizedSortingGridSize(IntPtr L)
	{
		try
		{
			Canvas canvas = (Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, canvas.normalizedSortingGridSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderMode v);
			canvas.renderMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scaleFactor(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scaleFactor = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_referencePixelsPerUnit(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).referencePixelsPerUnit = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overridePixelPerfect(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overridePixelPerfect = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pixelPerfect(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pixelPerfect = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_planeDistance(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).planeDistance = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideSorting(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideSorting = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingOrder(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingOrder = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetDisplay(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetDisplay = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingLayerID(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingLayerID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_additionalShaderChannels(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Canvas canvas = (Canvas)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AdditionalCanvasShaderChannels v);
			canvas.additionalShaderChannels = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingLayerName(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingLayerName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Canvas)objectTranslator.FastGetCSObj(L, 1)).worldCamera = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normalizedSortingGridSize(IntPtr L)
	{
		try
		{
			((Canvas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).normalizedSortingGridSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_preWillRenderCanvases(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Canvas.WillRenderCanvases willRenderCanvases = objectTranslator.GetDelegate<Canvas.WillRenderCanvases>(L, 2);
			if (willRenderCanvases == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Canvas.WillRenderCanvases!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Canvas.preWillRenderCanvases += willRenderCanvases;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Canvas.preWillRenderCanvases -= willRenderCanvases;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Canvas.preWillRenderCanvases!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_willRenderCanvases(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Canvas.WillRenderCanvases willRenderCanvases = objectTranslator.GetDelegate<Canvas.WillRenderCanvases>(L, 2);
			if (willRenderCanvases == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Canvas.WillRenderCanvases!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Canvas.willRenderCanvases += willRenderCanvases;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Canvas.willRenderCanvases -= willRenderCanvases;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Canvas.willRenderCanvases!");
	}
}
