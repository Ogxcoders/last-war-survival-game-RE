using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PushNoticeManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PushNoticeManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 0, 0);
		Utils.RegisterFunc(L, -4, "PushNotice", _m_PushNotice_xlua_st_);
		Utils.RegisterFunc(L, -4, "CancelNotice", _m_CancelNotice_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAllNotice", _m_ClearAllNotice_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPushCountById", _m_GetPushCountById_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPushSecondTimeById", _m_GetPushSecondTimeById_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetIsNotifyOpen", _m_GetIsNotifyOpen_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCurrentTimeUnix", _m_GetCurrentTimeUnix_xlua_st_);
		Utils.RegisterFunc(L, -4, "pushDataToHttpServer", _m_pushDataToHttpServer_xlua_st_);
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
				PushNoticeManager o = new PushNoticeManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PushNoticeManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PushNotice_xlua_st_(IntPtr L)
	{
		try
		{
			PushNoticeManager.PushNotice(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelNotice_xlua_st_(IntPtr L)
	{
		try
		{
			PushNoticeManager.CancelNotice(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllNotice_xlua_st_(IntPtr L)
	{
		try
		{
			PushNoticeManager.ClearAllNotice();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPushCountById_xlua_st_(IntPtr L)
	{
		try
		{
			int pushCountById = PushNoticeManager.GetPushCountById(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, pushCountById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPushSecondTimeById_xlua_st_(IntPtr L)
	{
		try
		{
			int pushSecondTimeById = PushNoticeManager.GetPushSecondTimeById(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, pushSecondTimeById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsNotifyOpen_xlua_st_(IntPtr L)
	{
		try
		{
			bool isNotifyOpen = PushNoticeManager.GetIsNotifyOpen();
			Lua.lua_pushboolean(L, isNotifyOpen);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentTimeUnix_xlua_st_(IntPtr L)
	{
		try
		{
			long currentTimeUnix = PushNoticeManager.GetCurrentTimeUnix();
			Lua.lua_pushint64(L, currentTimeUnix);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_pushDataToHttpServer_xlua_st_(IntPtr L)
	{
		try
		{
			PushNoticeManager.pushDataToHttpServer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
