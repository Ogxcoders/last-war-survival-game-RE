using System;
using System.Collections.Generic;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DownloadResGroupCommonDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DownloadResGroupCommonData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 13, 13, 9);
		Utils.RegisterFunc(L, -3, "ClearCompleted", _m_ClearCompleted);
		Utils.RegisterFunc(L, -3, "ClearDeletedCompleted", _m_ClearDeletedCompleted);
		Utils.RegisterFunc(L, -3, "InitBundleInfo", _m_InitBundleInfo);
		Utils.RegisterFunc(L, -3, "CalcCanDeleteBundleInfoList", _m_CalcCanDeleteBundleInfoList);
		Utils.RegisterFunc(L, -3, "IsDeleteTotallyCompleted", _m_IsDeleteTotallyCompleted);
		Utils.RegisterFunc(L, -3, "RecalcDownloadInfo", _m_RecalcDownloadInfo);
		Utils.RegisterFunc(L, -3, "SendRequest", _m_SendRequest);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "CheckFinish", _m_CheckFinish);
		Utils.RegisterFunc(L, -3, "InvokeDeletedCompleted", _m_InvokeDeletedCompleted);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "completed", _e_completed);
		Utils.RegisterFunc(L, -3, "deletedCompleted", _e_deletedCompleted);
		Utils.RegisterFunc(L, -2, "isDownloadSuccess", _g_get_isDownloadSuccess);
		Utils.RegisterFunc(L, -2, "downloadSize", _g_get_downloadSize);
		Utils.RegisterFunc(L, -2, "TotalProgress", _g_get_TotalProgress);
		Utils.RegisterFunc(L, -2, "IsPaused", _g_get_IsPaused);
		Utils.RegisterFunc(L, -2, "configId", _g_get_configId);
		Utils.RegisterFunc(L, -2, "packageId", _g_get_packageId);
		Utils.RegisterFunc(L, -2, "manifest", _g_get_manifest);
		Utils.RegisterFunc(L, -2, "status", _g_get_status);
		Utils.RegisterFunc(L, -2, "errMsg", _g_get_errMsg);
		Utils.RegisterFunc(L, -2, "totalSize", _g_get_totalSize);
		Utils.RegisterFunc(L, -2, "totalSizeMBStr", _g_get_totalSizeMBStr);
		Utils.RegisterFunc(L, -2, "alreadyDownloadSize", _g_get_alreadyDownloadSize);
		Utils.RegisterFunc(L, -2, "bundles", _g_get_bundles);
		Utils.RegisterFunc(L, -1, "configId", _s_set_configId);
		Utils.RegisterFunc(L, -1, "packageId", _s_set_packageId);
		Utils.RegisterFunc(L, -1, "manifest", _s_set_manifest);
		Utils.RegisterFunc(L, -1, "status", _s_set_status);
		Utils.RegisterFunc(L, -1, "errMsg", _s_set_errMsg);
		Utils.RegisterFunc(L, -1, "totalSize", _s_set_totalSize);
		Utils.RegisterFunc(L, -1, "totalSizeMBStr", _s_set_totalSizeMBStr);
		Utils.RegisterFunc(L, -1, "alreadyDownloadSize", _s_set_alreadyDownloadSize);
		Utils.RegisterFunc(L, -1, "bundles", _s_set_bundles);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "ByteToMegaByte", _m_ByteToMegaByte_xlua_st_);
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
				DownloadResGroupCommonData o = new DownloadResGroupCommonData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DownloadResGroupCommonData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearCompleted(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearCompleted();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearDeletedCompleted(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearDeletedCompleted();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBundleInfo(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitBundleInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcCanDeleteBundleInfoList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			HashSet<int> tmpDeletePackageIdSet = (HashSet<int>)objectTranslator.GetObject(L, 2, typeof(HashSet<int>));
			List<BundleInfo> o = downloadResGroupCommonData.CalcCanDeleteBundleInfoList(tmpDeletePackageIdSet);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDeleteTotallyCompleted(IntPtr L)
	{
		try
		{
			bool value = ((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDeleteTotallyCompleted();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ByteToMegaByte_xlua_st_(IntPtr L)
	{
		try
		{
			float num = DownloadResGroupCommonData.ByteToMegaByte((float)Lua.lua_tonumber(L, 1));
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecalcDownloadInfo(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecalcDownloadInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendRequest(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SendRequest();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Stop(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Stop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckFinish(IntPtr L)
	{
		try
		{
			bool value = ((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckFinish();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InvokeDeletedCompleted(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InvokeDeletedCompleted();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDownloadSuccess(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, downloadResGroupCommonData.isDownloadSuccess);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadSize(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, downloadResGroupCommonData.downloadSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TotalProgress(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, downloadResGroupCommonData.TotalProgress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsPaused(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, downloadResGroupCommonData.IsPaused);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_configId(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, downloadResGroupCommonData.configId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_packageId(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, downloadResGroupCommonData.packageId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_manifest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, downloadResGroupCommonData.manifest);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, downloadResGroupCommonData.status);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_errMsg(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, downloadResGroupCommonData.errMsg);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalSize(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, downloadResGroupCommonData.totalSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalSizeMBStr(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, downloadResGroupCommonData.totalSizeMBStr);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alreadyDownloadSize(IntPtr L)
	{
		try
		{
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, downloadResGroupCommonData.alreadyDownloadSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bundles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, downloadResGroupCommonData.bundles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_configId(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).configId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_packageId(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).packageId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_manifest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1)).manifest = (Manifest)objectTranslator.GetObject(L, 2, typeof(Manifest));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out OperationStatus v);
			downloadResGroupCommonData.status = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_errMsg(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).errMsg = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalSize(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalSize = Lua.lua_touint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalSizeMBStr(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalSizeMBStr = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alreadyDownloadSize(IntPtr L)
	{
		try
		{
			((DownloadResGroupCommonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alreadyDownloadSize = Lua.lua_touint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bundles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1)).bundles = (List<BundleInfo>)objectTranslator.GetObject(L, 2, typeof(List<BundleInfo>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_completed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			Action<DownloadResGroupCommonData> action = objectTranslator.GetDelegate<Action<DownloadResGroupCommonData>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<DownloadResGroupCommonData>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					downloadResGroupCommonData.completed += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					downloadResGroupCommonData.completed -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DownloadResGroupCommonData.completed!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_deletedCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DownloadResGroupCommonData downloadResGroupCommonData = (DownloadResGroupCommonData)objectTranslator.FastGetCSObj(L, 1);
			Action<DownloadResGroupCommonData> action = objectTranslator.GetDelegate<Action<DownloadResGroupCommonData>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<DownloadResGroupCommonData>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					downloadResGroupCommonData.deletedCompleted += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					downloadResGroupCommonData.deletedCompleted -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DownloadResGroupCommonData.deletedCompleted!");
		return 0;
	}
}
