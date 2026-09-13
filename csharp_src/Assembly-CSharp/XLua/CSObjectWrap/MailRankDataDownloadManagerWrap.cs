using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MailRankDataDownloadManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MailRankDataDownloadManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 0, 0);
		Utils.RegisterFunc(L, -3, "TryDownloadMailRankData", _m_TryDownloadMailRankData);
		Utils.RegisterFunc(L, -3, "GetSavePath", _m_GetSavePath);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "instance", _g_get_instance);
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
				MailRankDataDownloadManager o = new MailRankDataDownloadManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MailRankDataDownloadManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryDownloadMailRankData(IntPtr L)
	{
		try
		{
			MailRankDataDownloadManager obj = (MailRankDataDownloadManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uuid = Lua.lua_tostring(L, 2);
			ulong size = Lua.lua_touint64(L, 3);
			uint crc = Lua.xlua_touint(L, 4);
			string address = Lua.lua_tostring(L, 5);
			obj.TryDownloadMailRankData(uuid, size, crc, address);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSavePath(IntPtr L)
	{
		try
		{
			MailRankDataDownloadManager obj = (MailRankDataDownloadManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uuid = Lua.lua_tostring(L, 2);
			string savePath = obj.GetSavePath(uuid);
			Lua.lua_pushstring(L, savePath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, MailRankDataDownloadManager.instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
