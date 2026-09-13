using System;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourcePackageManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ResourcePackageManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 0, 0);
		Utils.RegisterFunc(L, -4, "RemoveDownloadCompleteCallback", _m_RemoveDownloadCompleteCallback_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetupDownloadChecker", _m_SetupDownloadChecker_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsPackageHasBundle", _m_IsPackageHasBundle_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsPackageDownloaded", _m_IsPackageDownloaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPackageTotalSize", _m_GetPackageTotalSize_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearPackageDownloadedCache", _m_ClearPackageDownloadedCache_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsDownloaded", _m_IsDownloaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveBundle", _m_RemoveBundle_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsSeasonResDownloadedByPackageId", _m_IsSeasonResDownloadedByPackageId_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsSeasonResDownloaded", _m_IsSeasonResDownloaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRequiredPackagesWithoutLog", _m_GetRequiredPackagesWithoutLog_xlua_st_);
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
				ResourcePackageManager o = new ResourcePackageManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourcePackageManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveDownloadCompleteCallback_xlua_st_(IntPtr L)
	{
		try
		{
			ResourcePackageManager.RemoveDownloadCompleteCallback();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupDownloadChecker_xlua_st_(IntPtr L)
	{
		try
		{
			ResourcePackageManager.SetupDownloadChecker();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPackageHasBundle_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ResourcePackageManager.IsPackageHasBundle(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPackageDownloaded_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<IDownloadChecker>(L, 2))
			{
				int packageId = Lua.xlua_tointeger(L, 1);
				IDownloadChecker downloadChecker = (IDownloadChecker)objectTranslator.GetObject(L, 2, typeof(IDownloadChecker));
				bool value = ResourcePackageManager.IsPackageDownloaded(packageId, downloadChecker);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				bool value2 = ResourcePackageManager.IsPackageDownloaded(Lua.xlua_tointeger(L, 1));
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourcePackageManager.IsPackageDownloaded!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPackageTotalSize_xlua_st_(IntPtr L)
	{
		try
		{
			ulong packageTotalSize = ResourcePackageManager.GetPackageTotalSize(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushuint64(L, packageTotalSize);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPackageDownloadedCache_xlua_st_(IntPtr L)
	{
		try
		{
			ResourcePackageManager.ClearPackageDownloadedCache(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDownloaded_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ResourcePackageManager.IsDownloaded((BundleInfo)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(BundleInfo)));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveBundle_xlua_st_(IntPtr L)
	{
		try
		{
			ResourcePackageManager.RemoveBundle((BundleInfo)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(BundleInfo)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSeasonResDownloadedByPackageId_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<IDownloadChecker>(L, 2))
			{
				int packageId = Lua.xlua_tointeger(L, 1);
				IDownloadChecker downloadChecker = (IDownloadChecker)objectTranslator.GetObject(L, 2, typeof(IDownloadChecker));
				bool value = ResourcePackageManager.IsSeasonResDownloadedByPackageId(packageId, downloadChecker);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				bool value2 = ResourcePackageManager.IsSeasonResDownloadedByPackageId(Lua.xlua_tointeger(L, 1));
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourcePackageManager.IsSeasonResDownloadedByPackageId!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSeasonResDownloaded_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ResourcePackageManager.IsSeasonResDownloaded(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRequiredPackagesWithoutLog_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] requiredPackagesWithoutLog = ResourcePackageManager.GetRequiredPackagesWithoutLog();
			objectTranslator.Push(L, requiredPackagesWithoutLog);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
