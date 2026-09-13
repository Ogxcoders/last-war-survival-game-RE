using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderTextureWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RenderTexture);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 21, 18);
		Utils.RegisterFunc(L, -3, "GetNativeDepthBufferPtr", _m_GetNativeDepthBufferPtr);
		Utils.RegisterFunc(L, -3, "DiscardContents", _m_DiscardContents);
		Utils.RegisterFunc(L, -3, "MarkRestoreExpected", _m_MarkRestoreExpected);
		Utils.RegisterFunc(L, -3, "ResolveAntiAliasedSurface", _m_ResolveAntiAliasedSurface);
		Utils.RegisterFunc(L, -3, "SetGlobalShaderProperty", _m_SetGlobalShaderProperty);
		Utils.RegisterFunc(L, -3, "Create", _m_Create);
		Utils.RegisterFunc(L, -3, "Release", _m_Release);
		Utils.RegisterFunc(L, -3, "IsCreated", _m_IsCreated);
		Utils.RegisterFunc(L, -3, "GenerateMips", _m_GenerateMips);
		Utils.RegisterFunc(L, -3, "ConvertToEquirect", _m_ConvertToEquirect);
		Utils.RegisterFunc(L, -2, "width", _g_get_width);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "dimension", _g_get_dimension);
		Utils.RegisterFunc(L, -2, "graphicsFormat", _g_get_graphicsFormat);
		Utils.RegisterFunc(L, -2, "useMipMap", _g_get_useMipMap);
		Utils.RegisterFunc(L, -2, "sRGB", _g_get_sRGB);
		Utils.RegisterFunc(L, -2, "vrUsage", _g_get_vrUsage);
		Utils.RegisterFunc(L, -2, "memorylessMode", _g_get_memorylessMode);
		Utils.RegisterFunc(L, -2, "format", _g_get_format);
		Utils.RegisterFunc(L, -2, "stencilFormat", _g_get_stencilFormat);
		Utils.RegisterFunc(L, -2, "autoGenerateMips", _g_get_autoGenerateMips);
		Utils.RegisterFunc(L, -2, "volumeDepth", _g_get_volumeDepth);
		Utils.RegisterFunc(L, -2, "antiAliasing", _g_get_antiAliasing);
		Utils.RegisterFunc(L, -2, "bindTextureMS", _g_get_bindTextureMS);
		Utils.RegisterFunc(L, -2, "enableRandomWrite", _g_get_enableRandomWrite);
		Utils.RegisterFunc(L, -2, "useDynamicScale", _g_get_useDynamicScale);
		Utils.RegisterFunc(L, -2, "isPowerOfTwo", _g_get_isPowerOfTwo);
		Utils.RegisterFunc(L, -2, "colorBuffer", _g_get_colorBuffer);
		Utils.RegisterFunc(L, -2, "depthBuffer", _g_get_depthBuffer);
		Utils.RegisterFunc(L, -2, "depth", _g_get_depth);
		Utils.RegisterFunc(L, -2, "descriptor", _g_get_descriptor);
		Utils.RegisterFunc(L, -1, "width", _s_set_width);
		Utils.RegisterFunc(L, -1, "height", _s_set_height);
		Utils.RegisterFunc(L, -1, "dimension", _s_set_dimension);
		Utils.RegisterFunc(L, -1, "graphicsFormat", _s_set_graphicsFormat);
		Utils.RegisterFunc(L, -1, "useMipMap", _s_set_useMipMap);
		Utils.RegisterFunc(L, -1, "vrUsage", _s_set_vrUsage);
		Utils.RegisterFunc(L, -1, "memorylessMode", _s_set_memorylessMode);
		Utils.RegisterFunc(L, -1, "format", _s_set_format);
		Utils.RegisterFunc(L, -1, "stencilFormat", _s_set_stencilFormat);
		Utils.RegisterFunc(L, -1, "autoGenerateMips", _s_set_autoGenerateMips);
		Utils.RegisterFunc(L, -1, "volumeDepth", _s_set_volumeDepth);
		Utils.RegisterFunc(L, -1, "antiAliasing", _s_set_antiAliasing);
		Utils.RegisterFunc(L, -1, "bindTextureMS", _s_set_bindTextureMS);
		Utils.RegisterFunc(L, -1, "enableRandomWrite", _s_set_enableRandomWrite);
		Utils.RegisterFunc(L, -1, "useDynamicScale", _s_set_useDynamicScale);
		Utils.RegisterFunc(L, -1, "isPowerOfTwo", _s_set_isPowerOfTwo);
		Utils.RegisterFunc(L, -1, "depth", _s_set_depth);
		Utils.RegisterFunc(L, -1, "descriptor", _s_set_descriptor);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 1, 1);
		Utils.RegisterFunc(L, -4, "SupportsStencil", _m_SupportsStencil_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReleaseTemporary", _m_ReleaseTemporary_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTemporary", _m_GetTemporary_xlua_st_);
		Utils.RegisterFunc(L, -2, "active", _g_get_active);
		Utils.RegisterFunc(L, -1, "active", _s_set_active);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<RenderTextureDescriptor>(L, 2))
			{
				objectTranslator.Get(L, 2, out RenderTextureDescriptor v);
				RenderTexture o = new RenderTexture(v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<RenderTexture>(L, 2))
			{
				RenderTexture o2 = new RenderTexture((RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture)));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<DefaultFormat>(L, 5))
			{
				int width = Lua.xlua_tointeger(L, 2);
				int height = Lua.xlua_tointeger(L, 3);
				int depth = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out DefaultFormat v2);
				RenderTexture o3 = new RenderTexture(width, height, depth, v2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<GraphicsFormat>(L, 5))
			{
				int width2 = Lua.xlua_tointeger(L, 2);
				int height2 = Lua.xlua_tointeger(L, 3);
				int depth2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out GraphicsFormat v3);
				RenderTexture o4 = new RenderTexture(width2, height2, depth2, v3);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (Lua.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<GraphicsFormat>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int width3 = Lua.xlua_tointeger(L, 2);
				int height3 = Lua.xlua_tointeger(L, 3);
				int depth3 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out GraphicsFormat v4);
				RenderTexture o5 = new RenderTexture(mipCount: Lua.xlua_tointeger(L, 6), width: width3, height: height3, depth: depth3, format: v4);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (Lua.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<RenderTextureFormat>(L, 5) && objectTranslator.Assignable<RenderTextureReadWrite>(L, 6))
			{
				int width4 = Lua.xlua_tointeger(L, 2);
				int height4 = Lua.xlua_tointeger(L, 3);
				int depth4 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out RenderTextureFormat val);
				objectTranslator.Get(L, 6, out RenderTextureReadWrite v5);
				RenderTexture o6 = new RenderTexture(width4, height4, depth4, val, v5);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<RenderTextureFormat>(L, 5))
			{
				int width5 = Lua.xlua_tointeger(L, 2);
				int height5 = Lua.xlua_tointeger(L, 3);
				int depth5 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out RenderTextureFormat val2);
				RenderTexture o7 = new RenderTexture(width5, height5, depth5, val2);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int width6 = Lua.xlua_tointeger(L, 2);
				int height6 = Lua.xlua_tointeger(L, 3);
				int depth6 = Lua.xlua_tointeger(L, 4);
				RenderTexture o8 = new RenderTexture(width6, height6, depth6);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (Lua.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<RenderTextureFormat>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int width7 = Lua.xlua_tointeger(L, 2);
				int height7 = Lua.xlua_tointeger(L, 3);
				int depth7 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out RenderTextureFormat val3);
				RenderTexture o9 = new RenderTexture(mipCount: Lua.xlua_tointeger(L, 6), width: width7, height: height7, depth: depth7, format: val3);
				objectTranslator.Push(L, o9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RenderTexture constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNativeDepthBufferPtr(IntPtr L)
	{
		try
		{
			IntPtr nativeDepthBufferPtr = ((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetNativeDepthBufferPtr();
			Lua.lua_pushlightuserdata(L, nativeDepthBufferPtr);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DiscardContents(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				renderTexture.DiscardContents();
				return 0;
			case 3:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
				{
					bool discardColor = Lua.lua_toboolean(L, 2);
					bool discardDepth = Lua.lua_toboolean(L, 3);
					renderTexture.DiscardContents(discardColor, discardDepth);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RenderTexture.DiscardContents!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MarkRestoreExpected(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MarkRestoreExpected();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResolveAntiAliasedSurface(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				renderTexture.ResolveAntiAliasedSurface();
				return 0;
			case 2:
				if (objectTranslator.Assignable<RenderTexture>(L, 2))
				{
					RenderTexture target = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
					renderTexture.ResolveAntiAliasedSurface(target);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RenderTexture.ResolveAntiAliasedSurface!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalShaderProperty(IntPtr L)
	{
		try
		{
			RenderTexture obj = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string globalShaderProperty = Lua.lua_tostring(L, 2);
			obj.SetGlobalShaderProperty(globalShaderProperty);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create(IntPtr L)
	{
		try
		{
			bool value = ((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Create();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Release(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Release();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCreated(IntPtr L)
	{
		try
		{
			bool value = ((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCreated();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GenerateMips(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GenerateMips();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConvertToEquirect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<RenderTexture>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				RenderTexture equirect = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val);
				renderTexture.ConvertToEquirect(equirect, val);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<RenderTexture>(L, 2))
			{
				RenderTexture equirect2 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				renderTexture.ConvertToEquirect(equirect2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RenderTexture.ConvertToEquirect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportsStencil_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = RenderTexture.SupportsStencil((RenderTexture)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RenderTexture)));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReleaseTemporary_xlua_st_(IntPtr L)
	{
		try
		{
			RenderTexture.ReleaseTemporary((RenderTexture)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RenderTexture)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTemporary_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int width = Lua.xlua_tointeger(L, 1);
				int height = Lua.xlua_tointeger(L, 2);
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				objectTranslator.Push(L, temporary);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int width2 = Lua.xlua_tointeger(L, 1);
				int height2 = Lua.xlua_tointeger(L, 2);
				int depthBuffer = Lua.xlua_tointeger(L, 3);
				RenderTexture temporary2 = RenderTexture.GetTemporary(width2, height2, depthBuffer);
				objectTranslator.Push(L, temporary2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<RenderTextureDescriptor>(L, 1))
			{
				objectTranslator.Get(L, 1, out RenderTextureDescriptor v);
				RenderTexture temporary3 = RenderTexture.GetTemporary(v);
				objectTranslator.Push(L, temporary3);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GraphicsFormat>(L, 4))
			{
				int width3 = Lua.xlua_tointeger(L, 1);
				int height3 = Lua.xlua_tointeger(L, 2);
				int depthBuffer2 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out GraphicsFormat v2);
				RenderTexture temporary4 = RenderTexture.GetTemporary(width3, height3, depthBuffer2, v2);
				objectTranslator.Push(L, temporary4);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RenderTextureFormat>(L, 4))
			{
				int width4 = Lua.xlua_tointeger(L, 1);
				int height4 = Lua.xlua_tointeger(L, 2);
				int depthBuffer3 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out RenderTextureFormat val);
				RenderTexture temporary5 = RenderTexture.GetTemporary(width4, height4, depthBuffer3, val);
				objectTranslator.Push(L, temporary5);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GraphicsFormat>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int width5 = Lua.xlua_tointeger(L, 1);
				int height5 = Lua.xlua_tointeger(L, 2);
				int depthBuffer4 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out GraphicsFormat v3);
				RenderTexture o = RenderTexture.GetTemporary(antiAliasing: Lua.xlua_tointeger(L, 5), width: width5, height: height5, depthBuffer: depthBuffer4, format: v3);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RenderTextureFormat>(L, 4) && objectTranslator.Assignable<RenderTextureReadWrite>(L, 5))
			{
				int width6 = Lua.xlua_tointeger(L, 1);
				int height6 = Lua.xlua_tointeger(L, 2);
				int depthBuffer5 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out RenderTextureFormat val2);
				objectTranslator.Get(L, 5, out RenderTextureReadWrite v4);
				RenderTexture temporary6 = RenderTexture.GetTemporary(width6, height6, depthBuffer5, val2, v4);
				objectTranslator.Push(L, temporary6);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GraphicsFormat>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<RenderTextureMemoryless>(L, 6))
			{
				int width7 = Lua.xlua_tointeger(L, 1);
				int height7 = Lua.xlua_tointeger(L, 2);
				int depthBuffer6 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out GraphicsFormat v5);
				int antiAliasing = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out RenderTextureMemoryless v6);
				RenderTexture temporary7 = RenderTexture.GetTemporary(width7, height7, depthBuffer6, v5, antiAliasing, v6);
				objectTranslator.Push(L, temporary7);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RenderTextureFormat>(L, 4) && objectTranslator.Assignable<RenderTextureReadWrite>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int width8 = Lua.xlua_tointeger(L, 1);
				int height8 = Lua.xlua_tointeger(L, 2);
				int depthBuffer7 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out RenderTextureFormat val3);
				objectTranslator.Get(L, 5, out RenderTextureReadWrite v7);
				RenderTexture o2 = RenderTexture.GetTemporary(antiAliasing: Lua.xlua_tointeger(L, 6), width: width8, height: height8, depthBuffer: depthBuffer7, format: val3, readWrite: v7);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GraphicsFormat>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<RenderTextureMemoryless>(L, 6) && objectTranslator.Assignable<VRTextureUsage>(L, 7))
			{
				int width9 = Lua.xlua_tointeger(L, 1);
				int height9 = Lua.xlua_tointeger(L, 2);
				int depthBuffer8 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out GraphicsFormat v8);
				int antiAliasing2 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out RenderTextureMemoryless v9);
				objectTranslator.Get(L, 7, out VRTextureUsage v10);
				RenderTexture temporary8 = RenderTexture.GetTemporary(width9, height9, depthBuffer8, v8, antiAliasing2, v9, v10);
				objectTranslator.Push(L, temporary8);
				return 1;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RenderTextureFormat>(L, 4) && objectTranslator.Assignable<RenderTextureReadWrite>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<RenderTextureMemoryless>(L, 7))
			{
				int width10 = Lua.xlua_tointeger(L, 1);
				int height10 = Lua.xlua_tointeger(L, 2);
				int depthBuffer9 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out RenderTextureFormat val4);
				objectTranslator.Get(L, 5, out RenderTextureReadWrite v11);
				int antiAliasing3 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out RenderTextureMemoryless v12);
				RenderTexture temporary9 = RenderTexture.GetTemporary(width10, height10, depthBuffer9, val4, v11, antiAliasing3, v12);
				objectTranslator.Push(L, temporary9);
				return 1;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GraphicsFormat>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<RenderTextureMemoryless>(L, 6) && objectTranslator.Assignable<VRTextureUsage>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				int width11 = Lua.xlua_tointeger(L, 1);
				int height11 = Lua.xlua_tointeger(L, 2);
				int depthBuffer10 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out GraphicsFormat v13);
				int antiAliasing4 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out RenderTextureMemoryless v14);
				objectTranslator.Get(L, 7, out VRTextureUsage v15);
				RenderTexture o3 = RenderTexture.GetTemporary(useDynamicScale: Lua.lua_toboolean(L, 8), width: width11, height: height11, depthBuffer: depthBuffer10, format: v13, antiAliasing: antiAliasing4, memorylessMode: v14, vrUsage: v15);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RenderTextureFormat>(L, 4) && objectTranslator.Assignable<RenderTextureReadWrite>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<RenderTextureMemoryless>(L, 7) && objectTranslator.Assignable<VRTextureUsage>(L, 8))
			{
				int width12 = Lua.xlua_tointeger(L, 1);
				int height12 = Lua.xlua_tointeger(L, 2);
				int depthBuffer11 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out RenderTextureFormat val5);
				objectTranslator.Get(L, 5, out RenderTextureReadWrite v16);
				int antiAliasing5 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out RenderTextureMemoryless v17);
				objectTranslator.Get(L, 8, out VRTextureUsage v18);
				RenderTexture temporary10 = RenderTexture.GetTemporary(width12, height12, depthBuffer11, val5, v16, antiAliasing5, v17, v18);
				objectTranslator.Push(L, temporary10);
				return 1;
			}
			if (num == 9 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RenderTextureFormat>(L, 4) && objectTranslator.Assignable<RenderTextureReadWrite>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<RenderTextureMemoryless>(L, 7) && objectTranslator.Assignable<VRTextureUsage>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				int width13 = Lua.xlua_tointeger(L, 1);
				int height13 = Lua.xlua_tointeger(L, 2);
				int depthBuffer12 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out RenderTextureFormat val6);
				objectTranslator.Get(L, 5, out RenderTextureReadWrite v19);
				int antiAliasing6 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out RenderTextureMemoryless v20);
				objectTranslator.Get(L, 8, out VRTextureUsage v21);
				RenderTexture o4 = RenderTexture.GetTemporary(useDynamicScale: Lua.lua_toboolean(L, 9), width: width13, height: height13, depthBuffer: depthBuffer12, format: val6, readWrite: v19, antiAliasing: antiAliasing6, memorylessMode: v20, vrUsage: v21);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.RenderTexture.GetTemporary!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_width(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderTexture.width);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderTexture.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dimension(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.dimension);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.graphicsFormat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useMipMap(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.useMipMap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sRGB(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.sRGB);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vrUsage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.vrUsage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_memorylessMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.memorylessMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_format(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineRenderTextureFormat(L, renderTexture.format);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stencilFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.stencilFormat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoGenerateMips(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.autoGenerateMips);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_volumeDepth(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderTexture.volumeDepth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_antiAliasing(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderTexture.antiAliasing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bindTextureMS(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.bindTextureMS);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableRandomWrite(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.enableRandomWrite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useDynamicScale(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.useDynamicScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPowerOfTwo(IntPtr L)
	{
		try
		{
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTexture.isPowerOfTwo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_active(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, RenderTexture.active);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.colorBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_depthBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.depthBuffer);
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
			RenderTexture renderTexture = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderTexture.depth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_descriptor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTexture.descriptor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_width(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).width = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_height(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).height = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dimension(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextureDimension v);
			renderTexture.dimension = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_graphicsFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GraphicsFormat v);
			renderTexture.graphicsFormat = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useMipMap(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useMipMap = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_vrUsage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VRTextureUsage v);
			renderTexture.vrUsage = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_memorylessMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderTextureMemoryless v);
			renderTexture.memorylessMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_format(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderTextureFormat val);
			renderTexture.format = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stencilFormat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GraphicsFormat v);
			renderTexture.stencilFormat = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoGenerateMips(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoGenerateMips = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_volumeDepth(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).volumeDepth = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_antiAliasing(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).antiAliasing = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bindTextureMS(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).bindTextureMS = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableRandomWrite(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableRandomWrite = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useDynamicScale(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useDynamicScale = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isPowerOfTwo(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isPowerOfTwo = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_active(IntPtr L)
	{
		try
		{
			RenderTexture.active = (RenderTexture)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RenderTexture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_depth(IntPtr L)
	{
		try
		{
			((RenderTexture)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).depth = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_descriptor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderTexture renderTexture = (RenderTexture)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderTextureDescriptor v);
			renderTexture.descriptor = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
