using System;
using System.Collections.Generic;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MessageFactoryWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MessageFactory);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 3, 3);
		Utils.RegisterFunc(L, -3, "InitMessageHandlers", _m_InitMessageHandlers);
		Utils.RegisterFunc(L, -3, "DispatchResponse", _m_DispatchResponse);
		Utils.RegisterFunc(L, -3, "OnLogin", _m_OnLogin);
		Utils.RegisterFunc(L, -2, "asyncDelayCmd", _g_get_asyncDelayCmd);
		Utils.RegisterFunc(L, -2, "asyncDelayTime", _g_get_asyncDelayTime);
		Utils.RegisterFunc(L, -2, "asyncDelayCmdSet", _g_get_asyncDelayCmdSet);
		Utils.RegisterFunc(L, -1, "asyncDelayCmd", _s_set_asyncDelayCmd);
		Utils.RegisterFunc(L, -1, "asyncDelayTime", _s_set_asyncDelayTime);
		Utils.RegisterFunc(L, -1, "asyncDelayCmdSet", _s_set_asyncDelayCmdSet);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 1, 0);
		Utils.RegisterFunc(L, -4, "GetMessageByCmd", _m_GetMessageByCmd_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetMessage", _m_GetMessage_xlua_st_);
		Utils.RegisterFunc(L, -4, "ContainsMessage", _m_ContainsMessage_xlua_st_);
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
				MessageFactory o = new MessageFactory();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MessageFactory constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitMessageHandlers(IntPtr L)
	{
		try
		{
			((MessageFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitMessageHandlers();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DispatchResponse(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MessageFactory messageFactory = (MessageFactory)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<BaseEvent>(L, 2))
			{
				BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
				messageFactory.DispatchResponse(e);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SFSObject>(L, 3))
			{
				string cmd = Lua.lua_tostring(L, 2);
				SFSObject so = (SFSObject)objectTranslator.GetObject(L, 3, typeof(SFSObject));
				messageFactory.DispatchResponse(cmd, so);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MessageFactory.DispatchResponse!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMessageByCmd_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BaseMessage messageByCmd = MessageFactory.GetMessageByCmd(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, messageByCmd);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMessage_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BaseMessage message = MessageFactory.GetMessage((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.Push(L, message);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ContainsMessage_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = MessageFactory.ContainsMessage((Type)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Type)));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLogin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MessageFactory messageFactory = (MessageFactory)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			bool value = messageFactory.OnLogin(e);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, MessageFactory.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncDelayCmd(IntPtr L)
	{
		try
		{
			MessageFactory messageFactory = (MessageFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, messageFactory.asyncDelayCmd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncDelayTime(IntPtr L)
	{
		try
		{
			MessageFactory messageFactory = (MessageFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, messageFactory.asyncDelayTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncDelayCmdSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MessageFactory messageFactory = (MessageFactory)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, messageFactory.asyncDelayCmdSet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncDelayCmd(IntPtr L)
	{
		try
		{
			((MessageFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).asyncDelayCmd = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncDelayTime(IntPtr L)
	{
		try
		{
			((MessageFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).asyncDelayTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncDelayCmdSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MessageFactory)objectTranslator.FastGetCSObj(L, 1)).asyncDelayCmdSet = (HashSet<string>)objectTranslator.GetObject(L, 2, typeof(HashSet<string>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
