using System;
using AIHelp;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AIHelpAIHelpProxyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AIHelpProxy);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 3, 3);
		Utils.RegisterFunc(L, -4, "ResetIsUsingZendesk", _m_ResetIsUsingZendesk_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckZendeskSwitch", _m_CheckZendeskSwitch_xlua_st_);
		Utils.RegisterFunc(L, -4, "CacheAIHelpHaveConversation", _m_CacheAIHelpHaveConversation_xlua_st_);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "Show", _m_Show_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetAIHelpDataList", _m_SetAIHelpDataList_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateUserInfo", _m_UpdateUserInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "Logout", _m_Logout_xlua_st_);
		Utils.RegisterFunc(L, -2, "UnreadMsgCount", _g_get_UnreadMsgCount);
		Utils.RegisterFunc(L, -2, "IsUsingZendesk", _g_get_IsUsingZendesk);
		Utils.RegisterFunc(L, -2, "aIHelpDataTable", _g_get_aIHelpDataTable);
		Utils.RegisterFunc(L, -1, "UnreadMsgCount", _s_set_UnreadMsgCount);
		Utils.RegisterFunc(L, -1, "IsUsingZendesk", _s_set_IsUsingZendesk);
		Utils.RegisterFunc(L, -1, "aIHelpDataTable", _s_set_aIHelpDataTable);
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
				AIHelpProxy o = new AIHelpProxy();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AIHelp.AIHelpProxy constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetIsUsingZendesk_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.ResetIsUsingZendesk(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckZendeskSwitch_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = AIHelpProxy.CheckZendeskSwitch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CacheAIHelpHaveConversation_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				AIHelpProxy.CacheAIHelpHaveConversation(Lua.xlua_tointeger(L, 1));
				return 0;
			}
			if (num == 0)
			{
				AIHelpProxy.CacheAIHelpHaveConversation();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AIHelp.AIHelpProxy.CacheAIHelpHaveConversation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Show_xlua_st_(IntPtr L)
	{
		try
		{
			string entranceId = Lua.lua_tostring(L, 1);
			string welcomeMessage = Lua.lua_tostring(L, 2);
			AIHelpProxy.Show(entranceId, welcomeMessage);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAIHelpDataList_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.SetAIHelpDataList();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateUserInfo_xlua_st_(IntPtr L)
	{
		try
		{
			string uid = Lua.lua_tostring(L, 1);
			string uname = Lua.lua_tostring(L, 2);
			string tag = Lua.lua_tostring(L, 3);
			string serverId = Lua.lua_tostring(L, 4);
			string json = Lua.lua_tostring(L, 5);
			AIHelpProxy.UpdateUserInfo(uid, uname, tag, serverId, json);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Logout_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.Logout();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UnreadMsgCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, AIHelpProxy.UnreadMsgCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsUsingZendesk(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, AIHelpProxy.IsUsingZendesk);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aIHelpDataTable(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, AIHelpProxy.aIHelpDataTable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UnreadMsgCount(IntPtr L)
	{
		try
		{
			AIHelpProxy.UnreadMsgCount = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsUsingZendesk(IntPtr L)
	{
		try
		{
			AIHelpProxy.IsUsingZendesk = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aIHelpDataTable(IntPtr L)
	{
		try
		{
			AIHelpProxy.aIHelpDataTable = (AIHelpProxy.AIHelpDataTable)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(AIHelpProxy.AIHelpDataTable));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
