using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PVELogManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PVELogManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 0, 0);
		Utils.RegisterFunc(L, -3, "UploadPVESurfingLog", _m_UploadPVESurfingLog);
		Utils.RegisterFunc(L, -3, "UploadPVESurfingLogFile", _m_UploadPVESurfingLogFile);
		Utils.RegisterFunc(L, -3, "UploadPVELog", _m_UploadPVELog);
		Utils.RegisterFunc(L, -3, "DownloadPVELog", _m_DownloadPVELog);
		Utils.RegisterFunc(L, -3, "DownloadPVESurfingLog", _m_DownloadPVESurfingLog);
		Utils.RegisterFunc(L, -3, "ClearAll", _m_ClearAll);
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
				PVELogManager o = new PVELogManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVELogManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UploadPVESurfingLog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PVELogManager pVELogManager = (PVELogManager)objectTranslator.FastGetCSObj(L, 1);
			string uid = Lua.lua_tostring(L, 2);
			string uuid = Lua.lua_tostring(L, 3);
			byte[] data = Lua.lua_tobytes(L, 4);
			Action<bool> callback = objectTranslator.GetDelegate<Action<bool>>(L, 5);
			pVELogManager.UploadPVESurfingLog(uid, uuid, data, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UploadPVESurfingLogFile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PVELogManager pVELogManager = (PVELogManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<bool>>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && (Lua.lua_isnil(L, 7) || Lua.lua_type(L, 7) == LuaTypes.LUA_TSTRING))
			{
				string uid = Lua.lua_tostring(L, 2);
				string uuid = Lua.lua_tostring(L, 3);
				string filePath = Lua.lua_tostring(L, 4);
				Action<bool> callback = objectTranslator.GetDelegate<Action<bool>>(L, 5);
				int funcType = Lua.xlua_tointeger(L, 6);
				string beginTime = Lua.lua_tostring(L, 7);
				pVELogManager.UploadPVESurfingLogFile(uid, uuid, filePath, callback, funcType, beginTime);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<bool>>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string uid2 = Lua.lua_tostring(L, 2);
				string uuid2 = Lua.lua_tostring(L, 3);
				string filePath2 = Lua.lua_tostring(L, 4);
				Action<bool> callback2 = objectTranslator.GetDelegate<Action<bool>>(L, 5);
				int funcType2 = Lua.xlua_tointeger(L, 6);
				pVELogManager.UploadPVESurfingLogFile(uid2, uuid2, filePath2, callback2, funcType2);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<bool>>(L, 5))
			{
				string uid3 = Lua.lua_tostring(L, 2);
				string uuid3 = Lua.lua_tostring(L, 3);
				string filePath3 = Lua.lua_tostring(L, 4);
				Action<bool> callback3 = objectTranslator.GetDelegate<Action<bool>>(L, 5);
				pVELogManager.UploadPVESurfingLogFile(uid3, uuid3, filePath3, callback3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVELogManager.UploadPVESurfingLogFile!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UploadPVELog(IntPtr L)
	{
		try
		{
			PVELogManager obj = (PVELogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uid = Lua.lua_tostring(L, 2);
			string uuid = Lua.lua_tostring(L, 3);
			obj.UploadPVELog(uid, uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DownloadPVELog(IntPtr L)
	{
		try
		{
			PVELogManager obj = (PVELogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uid = Lua.lua_tostring(L, 2);
			string uuid = Lua.lua_tostring(L, 3);
			obj.DownloadPVELog(uid, uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DownloadPVESurfingLog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PVELogManager pVELogManager = (PVELogManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<bool>>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string uid = Lua.lua_tostring(L, 2);
				string uuid = Lua.lua_tostring(L, 3);
				string downloadFilePath = Lua.lua_tostring(L, 4);
				Action<bool> callback = objectTranslator.GetDelegate<Action<bool>>(L, 5);
				int funcType = Lua.xlua_tointeger(L, 6);
				pVELogManager.DownloadPVESurfingLog(uid, uuid, downloadFilePath, callback, funcType);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<bool>>(L, 5))
			{
				string uid2 = Lua.lua_tostring(L, 2);
				string uuid2 = Lua.lua_tostring(L, 3);
				string downloadFilePath2 = Lua.lua_tostring(L, 4);
				Action<bool> callback2 = objectTranslator.GetDelegate<Action<bool>>(L, 5);
				pVELogManager.DownloadPVESurfingLog(uid2, uuid2, downloadFilePath2, callback2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVELogManager.DownloadPVESurfingLog!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAll(IntPtr L)
	{
		try
		{
			((PVELogManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAll();
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, PVELogManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
