using System;
using BitBenderGames;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BitBenderGamesTouchWrapperWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TouchWrapper);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 5, 0);
		Utils.RegisterFunc(L, -4, "GetFirstWrappedTouch", _m_GetFirstWrappedTouch_xlua_st_);
		Utils.RegisterFunc(L, -2, "TouchCount", _g_get_TouchCount);
		Utils.RegisterFunc(L, -2, "Touch0", _g_get_Touch0);
		Utils.RegisterFunc(L, -2, "IsFingerDown", _g_get_IsFingerDown);
		Utils.RegisterFunc(L, -2, "Touches", _g_get_Touches);
		Utils.RegisterFunc(L, -2, "AverageTouchPos", _g_get_AverageTouchPos);
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
				TouchWrapper o = new TouchWrapper();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.TouchWrapper constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFirstWrappedTouch_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WrappedTouch firstWrappedTouch = TouchWrapper.GetFirstWrappedTouch();
			objectTranslator.Push(L, firstWrappedTouch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TouchCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, TouchWrapper.TouchCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Touch0(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, TouchWrapper.Touch0);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsFingerDown(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, TouchWrapper.IsFingerDown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Touches(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, TouchWrapper.Touches);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AverageTouchPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector2(L, TouchWrapper.AverageTouchPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
