using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AutoBuildConnectEffectBallMoveWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AutoBuildConnectEffectBallMove);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 1);
		Utils.RegisterFunc(L, -2, "HeightDelta", _g_get_HeightDelta);
		Utils.RegisterFunc(L, -1, "HeightDelta", _s_set_HeightDelta);
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
				AutoBuildConnectEffectBallMove o = new AutoBuildConnectEffectBallMove();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoBuildConnectEffectBallMove constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoBuildConnectEffectBallMove autoBuildConnectEffectBallMove = (AutoBuildConnectEffectBallMove)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<List<Vector2Int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				List<Vector2Int> list = (List<Vector2Int>)objectTranslator.GetObject(L, 2, typeof(List<Vector2Int>));
				float movePerTime = (float)Lua.lua_tonumber(L, 3);
				int startIndex = Lua.xlua_tointeger(L, 4);
				autoBuildConnectEffectBallMove.Init(list, movePerTime, startIndex);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<List<Vector2Int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<Vector2Int> list2 = (List<Vector2Int>)objectTranslator.GetObject(L, 2, typeof(List<Vector2Int>));
				float movePerTime2 = (float)Lua.lua_tonumber(L, 3);
				autoBuildConnectEffectBallMove.Init(list2, movePerTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoBuildConnectEffectBallMove.Init!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeightDelta(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, AutoBuildConnectEffectBallMove.HeightDelta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HeightDelta(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			AutoBuildConnectEffectBallMove.HeightDelta = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
