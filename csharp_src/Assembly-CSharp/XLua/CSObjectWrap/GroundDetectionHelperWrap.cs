using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GroundDetectionHelperWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GroundDetectionHelper);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "Uninit", _m_Uninit);
		Utils.RegisterFunc(L, -3, "DetectGround", _m_DetectGround);
		Utils.RegisterFunc(L, -3, "GetDebugLog", _m_GetDebugLog);
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
				GroundDetectionHelper o = new GroundDetectionHelper();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GroundDetectionHelper constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GroundDetectionHelper groundDetectionHelper = (GroundDetectionHelper)objectTranslator.FastGetCSObj(L, 1);
			Transform transform = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			float skinWidth = (float)Lua.lua_tonumber(L, 3);
			float maxHeight = (float)Lua.lua_tonumber(L, 4);
			groundDetectionHelper.Init(transform, skinWidth, maxHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Uninit(IntPtr L)
	{
		try
		{
			((GroundDetectionHelper)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uninit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DetectGround(IntPtr L)
	{
		try
		{
			GroundDetectionHelper obj = (GroundDetectionHelper)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newX = (float)Lua.lua_tonumber(L, 2);
			float newY = (float)Lua.lua_tonumber(L, 3);
			float newZ = (float)Lua.lua_tonumber(L, 4);
			obj.DetectGround(newX, newY, newZ, out var isGrounded, out var contactPointY);
			Lua.lua_pushboolean(L, isGrounded);
			Lua.lua_pushnumber(L, contactPointY);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDebugLog(IntPtr L)
	{
		try
		{
			string debugLog = ((GroundDetectionHelper)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDebugLog();
			Lua.lua_pushstring(L, debugLog);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
