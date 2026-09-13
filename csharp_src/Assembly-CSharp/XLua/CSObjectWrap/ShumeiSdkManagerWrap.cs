using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ShumeiSdkManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ShumeiSdkManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 3, 1);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "Logout", _m_Logout);
		Utils.RegisterFunc(L, -3, "CallCreate", _m_CallCreate);
		Utils.RegisterFunc(L, -3, "OnShumeiSdkInitSuccess", _m_OnShumeiSdkInitSuccess);
		Utils.RegisterFunc(L, -3, "OnShumeiSdkInitError", _m_OnShumeiSdkInitError);
		Utils.RegisterFunc(L, -3, "SendDeviceIdToServer", _m_SendDeviceIdToServer);
		Utils.RegisterFunc(L, -3, "GetLoginSmsdkId", _m_GetLoginSmsdkId);
		Utils.RegisterFunc(L, -3, "SetMostNewVersion", _m_SetMostNewVersion);
		Utils.RegisterFunc(L, -3, "IsForbidCreateRole", _m_IsForbidCreateRole);
		Utils.RegisterFunc(L, -3, "GotoStoreToUpgrade", _m_GotoStoreToUpgrade);
		Utils.RegisterFunc(L, -3, "PrintInfoLog", _m_PrintInfoLog);
		Utils.RegisterFunc(L, -2, "Platform", _g_get_Platform);
		Utils.RegisterFunc(L, -2, "IsFunctionOpen", _g_get_IsFunctionOpen);
		Utils.RegisterFunc(L, -2, "_platform", _g_get__platform);
		Utils.RegisterFunc(L, -1, "_platform", _s_set__platform);
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
				ShumeiSdkManager o = new ShumeiSdkManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ShumeiSdkManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Logout(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Logout();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CallCreate(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CallCreate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnShumeiSdkInitSuccess(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnShumeiSdkInitSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnShumeiSdkInitError(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnShumeiSdkInitError();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendDeviceIdToServer(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SendDeviceIdToServer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLoginSmsdkId(IntPtr L)
	{
		try
		{
			string loginSmsdkId = ((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLoginSmsdkId();
			Lua.lua_pushstring(L, loginSmsdkId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMostNewVersion(IntPtr L)
	{
		try
		{
			ShumeiSdkManager obj = (ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string mostNewVersion = Lua.lua_tostring(L, 2);
			obj.SetMostNewVersion(mostNewVersion);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsForbidCreateRole(IntPtr L)
	{
		try
		{
			bool value = ((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsForbidCreateRole();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GotoStoreToUpgrade(IntPtr L)
	{
		try
		{
			((ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GotoStoreToUpgrade();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrintInfoLog(IntPtr L)
	{
		try
		{
			ShumeiSdkManager obj = (ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string msg = Lua.lua_tostring(L, 2);
			obj.PrintInfoLog(msg);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, ShumeiSdkManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Platform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShumeiSdkManager shumeiSdkManager = (ShumeiSdkManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, shumeiSdkManager.Platform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsFunctionOpen(IntPtr L)
	{
		try
		{
			ShumeiSdkManager shumeiSdkManager = (ShumeiSdkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, shumeiSdkManager.IsFunctionOpen);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__platform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShumeiSdkManager shumeiSdkManager = (ShumeiSdkManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, shumeiSdkManager._platform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__platform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ShumeiSdkManager)objectTranslator.FastGetCSObj(L, 1))._platform = (IShumeiSdkPlatform)objectTranslator.GetObject(L, 2, typeof(IShumeiSdkPlatform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
