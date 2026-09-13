using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DownloadManifestManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DownloadManifestManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 0, 0);
		Utils.RegisterFunc(L, -3, "GetCurrentDownloadConfigId", _m_GetCurrentDownloadConfigId);
		Utils.RegisterFunc(L, -3, "CreateDownloadData", _m_CreateDownloadData);
		Utils.RegisterFunc(L, -3, "AddNewDownload", _m_AddNewDownload);
		Utils.RegisterFunc(L, -3, "StopAllDownload", _m_StopAllDownload);
		Utils.RegisterFunc(L, -3, "GetLoadManifestData", _m_GetLoadManifestData);
		Utils.RegisterFunc(L, -3, "StopDownload", _m_StopDownload);
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
				DownloadManifestManager o = new DownloadManifestManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DownloadManifestManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentDownloadConfigId(IntPtr L)
	{
		try
		{
			int currentDownloadConfigId = ((DownloadManifestManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurrentDownloadConfigId();
			Lua.xlua_pushinteger(L, currentDownloadConfigId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateDownloadData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadManifestManager obj = (DownloadManifestManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownLoadGroupData o = obj.CreateDownloadData(configId);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddNewDownload(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadManifestManager downloadManifestManager = (DownloadManifestManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int configId = Lua.xlua_tointeger(L, 2);
				bool isStart = Lua.lua_toboolean(L, 3);
				DownLoadGroupData o = downloadManifestManager.AddNewDownload(configId, isStart);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<DownLoadGroupData>(L, 2))
			{
				DownLoadGroupData loader = (DownLoadGroupData)objectTranslator.GetObject(L, 2, typeof(DownLoadGroupData));
				DownLoadGroupData o2 = downloadManifestManager.AddNewDownload(loader);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DownloadManifestManager.AddNewDownload!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAllDownload(IntPtr L)
	{
		try
		{
			((DownloadManifestManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAllDownload();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLoadManifestData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadManifestManager obj = (DownloadManifestManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownLoadGroupData loadManifestData = obj.GetLoadManifestData(configId);
			objectTranslator.Push(L, loadManifestData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopDownload(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadManifestManager obj = (DownloadManifestManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownLoadGroupData o = obj.StopDownload(configId);
			objectTranslator.Push(L, o);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, DownloadManifestManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
