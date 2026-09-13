using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DownloadResGroupCommonManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DownloadResGroupCommonManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 13, 0, 0);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "GetCurrentDownloadConfigId", _m_GetCurrentDownloadConfigId);
		Utils.RegisterFunc(L, -3, "CreateDownloadData", _m_CreateDownloadData);
		Utils.RegisterFunc(L, -3, "IsDownload", _m_IsDownload);
		Utils.RegisterFunc(L, -3, "StartDownload", _m_StartDownload);
		Utils.RegisterFunc(L, -3, "AddDownload", _m_AddDownload);
		Utils.RegisterFunc(L, -3, "StopDownload", _m_StopDownload);
		Utils.RegisterFunc(L, -3, "DeleteDownloadPackageList", _m_DeleteDownloadPackageList);
		Utils.RegisterFunc(L, -3, "IsPackageDeleteTotallyCompleted", _m_IsPackageDeleteTotallyCompleted);
		Utils.RegisterFunc(L, -3, "StopDeletingPackage", _m_StopDeletingPackage);
		Utils.RegisterFunc(L, -3, "IsAnyPackageDeleting", _m_IsAnyPackageDeleting);
		Utils.RegisterFunc(L, -3, "StopAllDownload", _m_StopAllDownload);
		Utils.RegisterFunc(L, -3, "GetLoadManifestData", _m_GetLoadManifestData);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 11, 1, 0);
		Utils.RegisterFunc(L, -4, "SetClearedAllBundleCacheFlag", _m_SetClearedAllBundleCacheFlag_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetClearedAllBundleCacheFlag", _m_GetClearedAllBundleCacheFlag_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetClearedAllBundleCacheFlag", _m_ResetClearedAllBundleCacheFlag_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddWaitDeletePackageId", _m_AddWaitDeletePackageId_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveWaitDeletePackageId", _m_RemoveWaitDeletePackageId_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetWaitDeletePackageIds", _m_GetWaitDeletePackageIds_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetPackageDeleteTimes", _m_SetPackageDeleteTimes_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPackageDeleteTimes", _m_GetPackageDeleteTimes_xlua_st_);
		Utils.RegisterFunc(L, -4, "PrintLogError", _m_PrintLogError_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetVersionsSkipUpdateFlag", _m_GetVersionsSkipUpdateFlag_xlua_st_);
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
				DownloadResGroupCommonManager o = new DownloadResGroupCommonManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DownloadResGroupCommonManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentDownloadConfigId(IntPtr L)
	{
		try
		{
			int currentDownloadConfigId = ((DownloadResGroupCommonManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurrentDownloadConfigId();
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
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownloadResGroupCommonData o = obj.CreateDownloadData(configId);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDownload(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsDownload(configId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartDownload(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownloadResGroupCommonData o = obj.StartDownload(configId);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddDownload(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownloadResGroupCommonData o = obj.AddDownload(configId);
			objectTranslator.Push(L, o);
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
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownloadResGroupCommonData o = obj.StopDownload(configId);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteDownloadPackageList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonManager downloadResGroupCommonManager = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int[] configIdList = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			DownloadResGroupCommonData[] o = downloadResGroupCommonManager.DeleteDownloadPackageList(configIdList);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPackageDeleteTotallyCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonManager downloadResGroupCommonManager = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int[] packageIdList = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			int configId = Lua.xlua_tointeger(L, 3);
			bool value = downloadResGroupCommonManager.IsPackageDeleteTotallyCompleted(packageIdList, configId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopDeletingPackage(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			obj.StopDeletingPackage(configId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAnyPackageDeleting(IntPtr L)
	{
		try
		{
			bool value = ((DownloadResGroupCommonManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAnyPackageDeleting();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAllDownload(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAllDownload();
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
			DownloadResGroupCommonManager obj = (DownloadResGroupCommonManager)objectTranslator.FastGetCSObj(L, 1);
			int configId = Lua.xlua_tointeger(L, 2);
			DownloadResGroupCommonData loadManifestData = obj.GetLoadManifestData(configId);
			objectTranslator.Push(L, loadManifestData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetClearedAllBundleCacheFlag_xlua_st_(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager.SetClearedAllBundleCacheFlag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClearedAllBundleCacheFlag_xlua_st_(IntPtr L)
	{
		try
		{
			bool clearedAllBundleCacheFlag = DownloadResGroupCommonManager.GetClearedAllBundleCacheFlag();
			Lua.lua_pushboolean(L, clearedAllBundleCacheFlag);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetClearedAllBundleCacheFlag_xlua_st_(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager.ResetClearedAllBundleCacheFlag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddWaitDeletePackageId_xlua_st_(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager.AddWaitDeletePackageId(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveWaitDeletePackageId_xlua_st_(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager.RemoveWaitDeletePackageId(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWaitDeletePackageIds_xlua_st_(IntPtr L)
	{
		try
		{
			string waitDeletePackageIds = DownloadResGroupCommonManager.GetWaitDeletePackageIds();
			Lua.lua_pushstring(L, waitDeletePackageIds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPackageDeleteTimes_xlua_st_(IntPtr L)
	{
		try
		{
			int packageId = Lua.xlua_tointeger(L, 1);
			int times = Lua.xlua_tointeger(L, 2);
			DownloadResGroupCommonManager.SetPackageDeleteTimes(packageId, times);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPackageDeleteTimes_xlua_st_(IntPtr L)
	{
		try
		{
			int packageDeleteTimes = DownloadResGroupCommonManager.GetPackageDeleteTimes(Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, packageDeleteTimes);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrintLogError_xlua_st_(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonManager.PrintLogError(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVersionsSkipUpdateFlag_xlua_st_(IntPtr L)
	{
		try
		{
			string versionsSkipUpdateFlag = DownloadResGroupCommonManager.GetVersionsSkipUpdateFlag();
			Lua.lua_pushstring(L, versionsSkipUpdateFlag);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, DownloadResGroupCommonManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
