using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ChangeSceneCircleSliderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ChangeSceneCircleSlider);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 1, 1);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "SetValue", _m_SetValue);
		Utils.RegisterFunc(L, -3, "GetSpriteRenderer", _m_GetSpriteRenderer);
		Utils.RegisterFunc(L, -3, "ClearData", _m_ClearData);
		Utils.RegisterFunc(L, -2, "_waitRefresh", _g_get__waitRefresh);
		Utils.RegisterFunc(L, -1, "_waitRefresh", _s_set__waitRefresh);
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
				ChangeSceneCircleSlider o = new ChangeSceneCircleSlider();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChangeSceneCircleSlider constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ChangeSceneCircleSlider obj = (ChangeSceneCircleSlider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long startTime = Lua.lua_toint64(L, 2);
			long endTime = Lua.lua_toint64(L, 3);
			obj.Init(startTime, endTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetValue(IntPtr L)
	{
		try
		{
			ChangeSceneCircleSlider obj = (ChangeSceneCircleSlider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float value = (float)Lua.lua_tonumber(L, 2);
			obj.SetValue(value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpriteRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = ((ChangeSceneCircleSlider)objectTranslator.FastGetCSObj(L, 1)).GetSpriteRenderer();
			objectTranslator.Push(L, spriteRenderer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearData(IntPtr L)
	{
		try
		{
			((ChangeSceneCircleSlider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__waitRefresh(IntPtr L)
	{
		try
		{
			ChangeSceneCircleSlider changeSceneCircleSlider = (ChangeSceneCircleSlider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, changeSceneCircleSlider._waitRefresh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__waitRefresh(IntPtr L)
	{
		try
		{
			((ChangeSceneCircleSlider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1))._waitRefresh = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
