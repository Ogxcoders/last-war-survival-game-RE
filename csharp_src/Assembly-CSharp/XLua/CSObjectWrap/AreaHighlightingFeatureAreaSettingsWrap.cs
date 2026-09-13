using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AreaHighlightingFeatureAreaSettingsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AreaHighlightingFeature.AreaSettings);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 15, 15);
		Utils.RegisterFunc(L, -2, "centerPos", _g_get_centerPos);
		Utils.RegisterFunc(L, -2, "worldSize", _g_get_worldSize);
		Utils.RegisterFunc(L, -2, "mat", _g_get_mat);
		Utils.RegisterFunc(L, -2, "inSideColor", _g_get_inSideColor);
		Utils.RegisterFunc(L, -2, "addSideColor", _g_get_addSideColor);
		Utils.RegisterFunc(L, -2, "drakColor", _g_get_drakColor);
		Utils.RegisterFunc(L, -2, "jibanColor", _g_get_jibanColor);
		Utils.RegisterFunc(L, -2, "brightness", _g_get_brightness);
		Utils.RegisterFunc(L, -2, "saturation", _g_get_saturation);
		Utils.RegisterFunc(L, -2, "contrast", _g_get_contrast);
		Utils.RegisterFunc(L, -2, "diffusePower", _g_get_diffusePower);
		Utils.RegisterFunc(L, -2, "diffuseScale", _g_get_diffuseScale);
		Utils.RegisterFunc(L, -2, "_cutThreshold", _g_get__cutThreshold);
		Utils.RegisterFunc(L, -2, "renderPassEvent", _g_get_renderPassEvent);
		Utils.RegisterFunc(L, -2, "isDebug", _g_get_isDebug);
		Utils.RegisterFunc(L, -1, "centerPos", _s_set_centerPos);
		Utils.RegisterFunc(L, -1, "worldSize", _s_set_worldSize);
		Utils.RegisterFunc(L, -1, "mat", _s_set_mat);
		Utils.RegisterFunc(L, -1, "inSideColor", _s_set_inSideColor);
		Utils.RegisterFunc(L, -1, "addSideColor", _s_set_addSideColor);
		Utils.RegisterFunc(L, -1, "drakColor", _s_set_drakColor);
		Utils.RegisterFunc(L, -1, "jibanColor", _s_set_jibanColor);
		Utils.RegisterFunc(L, -1, "brightness", _s_set_brightness);
		Utils.RegisterFunc(L, -1, "saturation", _s_set_saturation);
		Utils.RegisterFunc(L, -1, "contrast", _s_set_contrast);
		Utils.RegisterFunc(L, -1, "diffusePower", _s_set_diffusePower);
		Utils.RegisterFunc(L, -1, "diffuseScale", _s_set_diffuseScale);
		Utils.RegisterFunc(L, -1, "_cutThreshold", _s_set__cutThreshold);
		Utils.RegisterFunc(L, -1, "renderPassEvent", _s_set_renderPassEvent);
		Utils.RegisterFunc(L, -1, "isDebug", _s_set_isDebug);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				AreaHighlightingFeature.AreaSettings o = new AreaHighlightingFeature.AreaSettings();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AreaHighlightingFeature.AreaSettings constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_centerPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, areaSettings.centerPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldSize(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, areaSettings.worldSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, areaSettings.mat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inSideColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, areaSettings.inSideColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_addSideColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, areaSettings.addSideColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_drakColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, areaSettings.drakColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_jibanColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, areaSettings.jibanColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_brightness(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, areaSettings.brightness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_saturation(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, areaSettings.saturation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_contrast(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, areaSettings.contrast);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_diffusePower(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, areaSettings.diffusePower);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_diffuseScale(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, areaSettings.diffuseScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__cutThreshold(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, areaSettings._cutThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderPassEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, areaSettings.renderPassEvent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDebug(IntPtr L)
	{
		try
		{
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, areaSettings.isDebug);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_centerPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			areaSettings.centerPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldSize(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).worldSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1)).mat = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inSideColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			areaSettings.inSideColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_addSideColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			areaSettings.addSideColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_drakColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			areaSettings.drakColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_jibanColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			areaSettings.jibanColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_brightness(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).brightness = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_saturation(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).saturation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_contrast(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).contrast = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_diffusePower(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).diffusePower = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_diffuseScale(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).diffuseScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__cutThreshold(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._cutThreshold = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderPassEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AreaHighlightingFeature.AreaSettings areaSettings = (AreaHighlightingFeature.AreaSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderPassEvent v);
			areaSettings.renderPassEvent = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isDebug(IntPtr L)
	{
		try
		{
			((AreaHighlightingFeature.AreaSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isDebug = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
