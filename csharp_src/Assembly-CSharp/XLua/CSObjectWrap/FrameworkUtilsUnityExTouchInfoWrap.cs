using System;
using Framework.Utils.UnityEx;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FrameworkUtilsUnityExTouchInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TouchInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 3, 3);
		Utils.RegisterFunc(L, -2, "pointerId", _g_get_pointerId);
		Utils.RegisterFunc(L, -2, "pointerPos", _g_get_pointerPos);
		Utils.RegisterFunc(L, -2, "deltaPos", _g_get_deltaPos);
		Utils.RegisterFunc(L, -1, "pointerId", _s_set_pointerId);
		Utils.RegisterFunc(L, -1, "pointerPos", _s_set_pointerPos);
		Utils.RegisterFunc(L, -1, "deltaPos", _s_set_deltaPos);
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
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && objectTranslator.Assignable<Vector2>(L, 4))
			{
				int pointerId = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val);
				objectTranslator.Get(L, 4, out Vector2 val2);
				TouchInfo val3 = new TouchInfo(pointerId, val, val2);
				objectTranslator.PushFrameworkUtilsUnityExTouchInfo(L, val3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.PushFrameworkUtilsUnityExTouchInfo(L, default(TouchInfo));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Framework.Utils.UnityEx.TouchInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerId(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TouchInfo val);
			Lua.xlua_pushinteger(L, val.pointerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointerPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TouchInfo val);
			objectTranslator.PushUnityEngineVector2(L, val.pointerPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deltaPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TouchInfo val);
			objectTranslator.PushUnityEngineVector2(L, val.deltaPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TouchInfo val);
			val.pointerId = Lua.xlua_tointeger(L, 2);
			objectTranslator.UpdateFrameworkUtilsUnityExTouchInfo(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointerPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TouchInfo val);
			objectTranslator.Get(L, 2, out Vector2 val2);
			val.pointerPos = val2;
			objectTranslator.UpdateFrameworkUtilsUnityExTouchInfo(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_deltaPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TouchInfo val);
			objectTranslator.Get(L, 2, out Vector2 val2);
			val.deltaPos = val2;
			objectTranslator.UpdateFrameworkUtilsUnityExTouchInfo(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
