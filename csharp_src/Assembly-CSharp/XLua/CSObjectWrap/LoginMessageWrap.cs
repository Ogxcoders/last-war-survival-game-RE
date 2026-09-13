using System;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LoginMessageWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LoginMessage);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 1, 1);
		Utils.RegisterFunc(L, -3, "GetMsgId", _m_GetMsgId);
		Utils.RegisterFunc(L, -3, "SendDelAccountMsg", _m_SendDelAccountMsg);
		Utils.RegisterFunc(L, -3, "SendDelAccountApplyCancelMsg", _m_SendDelAccountApplyCancelMsg);
		Utils.RegisterFunc(L, -2, "onLoginResponse", _g_get_onLoginResponse);
		Utils.RegisterFunc(L, -1, "onLoginResponse", _s_set_onLoginResponse);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				LoginMessage o = new LoginMessage();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LoginMessage constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMsgId(IntPtr L)
	{
		try
		{
			string msgId = ((LoginMessage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMsgId();
			Lua.lua_pushstring(L, msgId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendDelAccountMsg(IntPtr L)
	{
		try
		{
			((LoginMessage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SendDelAccountMsg();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendDelAccountApplyCancelMsg(IntPtr L)
	{
		try
		{
			((LoginMessage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SendDelAccountApplyCancelMsg();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, LoginMessage.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onLoginResponse(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoginMessage loginMessage = (LoginMessage)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loginMessage.onLoginResponse);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onLoginResponse(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoginMessage)objectTranslator.FastGetCSObj(L, 1)).onLoginResponse = objectTranslator.GetDelegate<Action<ISFSObject>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
