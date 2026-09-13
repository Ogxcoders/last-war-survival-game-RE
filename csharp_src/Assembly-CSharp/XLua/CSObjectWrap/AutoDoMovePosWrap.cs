using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AutoDoMovePosWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AutoDoMovePos);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "GetDownTime", _m_GetDownTime);
		Utils.RegisterFunc(L, -3, "GetUpTime", _m_GetUpTime);
		Utils.RegisterFunc(L, -3, "GetMoveSpeed", _m_GetMoveSpeed);
		Utils.RegisterFunc(L, -3, "ChangeStartPos", _m_ChangeStartPos);
		Utils.RegisterFunc(L, -3, "ChangeEndPos", _m_ChangeEndPos);
		Utils.RegisterFunc(L, -3, "GetMovePosListCount", _m_GetMovePosListCount);
		Utils.RegisterFunc(L, -3, "GetScreenPos", _m_GetScreenPos);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 3, 3);
		Utils.RegisterFunc(L, -2, "MoveSpeed", _g_get_MoveSpeed);
		Utils.RegisterFunc(L, -2, "DownTime", _g_get_DownTime);
		Utils.RegisterFunc(L, -2, "UpTime", _g_get_UpTime);
		Utils.RegisterFunc(L, -1, "MoveSpeed", _s_set_MoveSpeed);
		Utils.RegisterFunc(L, -1, "DownTime", _s_set_DownTime);
		Utils.RegisterFunc(L, -1, "UpTime", _s_set_UpTime);
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
				AutoDoMovePos o = new AutoDoMovePos();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoDoMovePos constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			AutoDoMovePos obj = (AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string posLists = Lua.lua_tostring(L, 2);
			obj.Init(posLists);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDownTime(IntPtr L)
	{
		try
		{
			float downTime = ((AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDownTime();
			Lua.lua_pushnumber(L, downTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUpTime(IntPtr L)
	{
		try
		{
			float upTime = ((AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetUpTime();
			Lua.lua_pushnumber(L, upTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMoveSpeed(IntPtr L)
	{
		try
		{
			float moveSpeed = ((AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMoveSpeed();
			Lua.lua_pushnumber(L, moveSpeed);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeStartPos(IntPtr L)
	{
		try
		{
			((AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeStartPos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeEndPos(IntPtr L)
	{
		try
		{
			((AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeEndPos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMovePosListCount(IntPtr L)
	{
		try
		{
			int movePosListCount = ((AutoDoMovePos)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMovePosListCount();
			Lua.xlua_pushinteger(L, movePosListCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetScreenPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoDoMovePos obj = (AutoDoMovePos)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			Vector3 screenPos = obj.GetScreenPos(index);
			objectTranslator.PushUnityEngineVector3(L, screenPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MoveSpeed(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, AutoDoMovePos.MoveSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DownTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, AutoDoMovePos.DownTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UpTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, AutoDoMovePos.UpTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MoveSpeed(IntPtr L)
	{
		try
		{
			AutoDoMovePos.MoveSpeed = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DownTime(IntPtr L)
	{
		try
		{
			AutoDoMovePos.DownTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UpTime(IntPtr L)
	{
		try
		{
			AutoDoMovePos.UpTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
