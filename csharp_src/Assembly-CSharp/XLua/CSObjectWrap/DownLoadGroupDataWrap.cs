using System;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DownLoadGroupDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DownLoadGroupData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 11, 8);
		Utils.RegisterFunc(L, -3, "CalcDownloadInfo", _m_CalcDownloadInfo);
		Utils.RegisterFunc(L, -3, "SendRequest", _m_SendRequest);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "CheckFinish", _m_CheckFinish);
		Utils.RegisterFunc(L, -2, "downloadSize", _g_get_downloadSize);
		Utils.RegisterFunc(L, -2, "TotalProgress", _g_get_TotalProgress);
		Utils.RegisterFunc(L, -2, "IsPaused", _g_get_IsPaused);
		Utils.RegisterFunc(L, -2, "configId", _g_get_configId);
		Utils.RegisterFunc(L, -2, "packageIds", _g_get_packageIds);
		Utils.RegisterFunc(L, -2, "manifest", _g_get_manifest);
		Utils.RegisterFunc(L, -2, "isDone", _g_get_isDone);
		Utils.RegisterFunc(L, -2, "errMsg", _g_get_errMsg);
		Utils.RegisterFunc(L, -2, "totalSize", _g_get_totalSize);
		Utils.RegisterFunc(L, -2, "totalSizeMBStr", _g_get_totalSizeMBStr);
		Utils.RegisterFunc(L, -2, "alreadyDownloadSize", _g_get_alreadyDownloadSize);
		Utils.RegisterFunc(L, -1, "configId", _s_set_configId);
		Utils.RegisterFunc(L, -1, "packageIds", _s_set_packageIds);
		Utils.RegisterFunc(L, -1, "manifest", _s_set_manifest);
		Utils.RegisterFunc(L, -1, "isDone", _s_set_isDone);
		Utils.RegisterFunc(L, -1, "errMsg", _s_set_errMsg);
		Utils.RegisterFunc(L, -1, "totalSize", _s_set_totalSize);
		Utils.RegisterFunc(L, -1, "totalSizeMBStr", _s_set_totalSizeMBStr);
		Utils.RegisterFunc(L, -1, "alreadyDownloadSize", _s_set_alreadyDownloadSize);
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
				DownLoadGroupData o = new DownLoadGroupData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DownLoadGroupData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ByteToMegaByte_xlua_st_(IntPtr L)
	{
		try
		{
			float num = DownLoadGroupData.ByteToMegaByte((float)Lua.lua_tonumber(L, 1));
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcDownloadInfo(IntPtr L)
	{
		try
		{
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CalcDownloadInfo();
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SendRequest();
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Stop();
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
			bool value = ((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckFinish();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadSize(IntPtr L)
	{
		try
		{
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, downLoadGroupData.downloadSize);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, downLoadGroupData.TotalProgress);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, downLoadGroupData.IsPaused);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, downLoadGroupData.configId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_packageIds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, downLoadGroupData.packageIds);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, downLoadGroupData.manifest);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDone(IntPtr L)
	{
		try
		{
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, downLoadGroupData.isDone);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, downLoadGroupData.errMsg);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, downLoadGroupData.totalSize);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, downLoadGroupData.totalSizeMBStr);
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
			DownLoadGroupData downLoadGroupData = (DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, downLoadGroupData.alreadyDownloadSize);
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).configId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_packageIds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DownLoadGroupData)objectTranslator.FastGetCSObj(L, 1)).packageIds = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
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
			((DownLoadGroupData)objectTranslator.FastGetCSObj(L, 1)).manifest = (Manifest)objectTranslator.GetObject(L, 2, typeof(Manifest));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isDone(IntPtr L)
	{
		try
		{
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isDone = Lua.lua_toboolean(L, 2);
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).errMsg = Lua.lua_tostring(L, 2);
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalSize = Lua.lua_touint64(L, 2);
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalSizeMBStr = Lua.lua_tostring(L, 2);
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
			((DownLoadGroupData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alreadyDownloadSize = Lua.lua_touint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
