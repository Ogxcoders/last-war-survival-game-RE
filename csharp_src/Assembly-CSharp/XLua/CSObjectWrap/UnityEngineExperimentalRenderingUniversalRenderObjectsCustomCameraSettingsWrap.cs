using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineExperimentalRenderingUniversalRenderObjectsCustomCameraSettingsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RenderObjects.CustomCameraSettings);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "overrideCamera", _g_get_overrideCamera);
		Utils.RegisterFunc(L, -2, "restoreCamera", _g_get_restoreCamera);
		Utils.RegisterFunc(L, -2, "offset", _g_get_offset);
		Utils.RegisterFunc(L, -2, "cameraFieldOfView", _g_get_cameraFieldOfView);
		Utils.RegisterFunc(L, -1, "overrideCamera", _s_set_overrideCamera);
		Utils.RegisterFunc(L, -1, "restoreCamera", _s_set_restoreCamera);
		Utils.RegisterFunc(L, -1, "offset", _s_set_offset);
		Utils.RegisterFunc(L, -1, "cameraFieldOfView", _s_set_cameraFieldOfView);
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
				RenderObjects.CustomCameraSettings o = new RenderObjects.CustomCameraSettings();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Experimental.Rendering.Universal.RenderObjects.CustomCameraSettings constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideCamera(IntPtr L)
	{
		try
		{
			RenderObjects.CustomCameraSettings customCameraSettings = (RenderObjects.CustomCameraSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, customCameraSettings.overrideCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_restoreCamera(IntPtr L)
	{
		try
		{
			RenderObjects.CustomCameraSettings customCameraSettings = (RenderObjects.CustomCameraSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, customCameraSettings.restoreCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_offset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.CustomCameraSettings customCameraSettings = (RenderObjects.CustomCameraSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, customCameraSettings.offset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraFieldOfView(IntPtr L)
	{
		try
		{
			RenderObjects.CustomCameraSettings customCameraSettings = (RenderObjects.CustomCameraSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, customCameraSettings.cameraFieldOfView);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideCamera(IntPtr L)
	{
		try
		{
			((RenderObjects.CustomCameraSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideCamera = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_restoreCamera(IntPtr L)
	{
		try
		{
			((RenderObjects.CustomCameraSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).restoreCamera = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_offset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.CustomCameraSettings customCameraSettings = (RenderObjects.CustomCameraSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			customCameraSettings.offset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cameraFieldOfView(IntPtr L)
	{
		try
		{
			((RenderObjects.CustomCameraSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cameraFieldOfView = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
