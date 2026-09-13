using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CommonUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CommonUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 3, 3);
		Utils.RegisterFunc(L, -4, "IsDebug", _m_IsDebug_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsWriteLog", _m_IsWriteLog_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckIsOverridePackage", _m_CheckIsOverridePackage_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteCache", _m_DeleteCache_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteOffsetFile", _m_DeleteOffsetFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "WriteVersion", _m_WriteVersion_xlua_st_);
		Utils.RegisterFunc(L, -4, "Lua_File_Read", _m_Lua_File_Read_xlua_st_);
		Utils.RegisterFunc(L, -4, "Lua_File_Write", _m_Lua_File_Write_xlua_st_);
		Utils.RegisterFunc(L, -4, "Lua_File_Delete", _m_Lua_File_Delete_xlua_st_);
		Utils.RegisterFunc(L, -2, "VEngine_Utility_BuildPath", _g_get_VEngine_Utility_BuildPath);
		Utils.RegisterFunc(L, -2, "BUNDLE_OFFSET_TABLE_FILE", _g_get_BUNDLE_OFFSET_TABLE_FILE);
		Utils.RegisterFunc(L, -2, "BUNDLE_ALIAS_OFFSET_TABLE_FILE", _g_get_BUNDLE_ALIAS_OFFSET_TABLE_FILE);
		Utils.RegisterFunc(L, -1, "VEngine_Utility_BuildPath", _s_set_VEngine_Utility_BuildPath);
		Utils.RegisterFunc(L, -1, "BUNDLE_OFFSET_TABLE_FILE", _s_set_BUNDLE_OFFSET_TABLE_FILE);
		Utils.RegisterFunc(L, -1, "BUNDLE_ALIAS_OFFSET_TABLE_FILE", _s_set_BUNDLE_ALIAS_OFFSET_TABLE_FILE);
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
				CommonUtils o = new CommonUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CommonUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDebug_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = CommonUtils.IsDebug();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWriteLog_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = CommonUtils.IsWriteLog();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckIsOverridePackage_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = CommonUtils.CheckIsOverridePackage();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteCache_xlua_st_(IntPtr L)
	{
		try
		{
			CommonUtils.DeleteCache(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteOffsetFile_xlua_st_(IntPtr L)
	{
		try
		{
			CommonUtils.DeleteOffsetFile();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WriteVersion_xlua_st_(IntPtr L)
	{
		try
		{
			CommonUtils.WriteVersion();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lua_File_Read_xlua_st_(IntPtr L)
	{
		try
		{
			byte[] data;
			string error;
			bool value = CommonUtils.Lua_File_Read(Lua.lua_tostring(L, 1), out data, out error);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushstring(L, data);
			Lua.lua_pushstring(L, error);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lua_File_Write_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			byte[] data = Lua.lua_tobytes(L, 2);
			string error;
			bool value = CommonUtils.Lua_File_Write(path, data, out error);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushstring(L, error);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lua_File_Delete_xlua_st_(IntPtr L)
	{
		try
		{
			string error;
			bool value = CommonUtils.Lua_File_Delete(Lua.lua_tostring(L, 1), out error);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushstring(L, error);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VEngine_Utility_BuildPath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, CommonUtils.VEngine_Utility_BuildPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BUNDLE_OFFSET_TABLE_FILE(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, CommonUtils.BUNDLE_OFFSET_TABLE_FILE);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BUNDLE_ALIAS_OFFSET_TABLE_FILE(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, CommonUtils.BUNDLE_ALIAS_OFFSET_TABLE_FILE);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_VEngine_Utility_BuildPath(IntPtr L)
	{
		try
		{
			CommonUtils.VEngine_Utility_BuildPath = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BUNDLE_OFFSET_TABLE_FILE(IntPtr L)
	{
		try
		{
			CommonUtils.BUNDLE_OFFSET_TABLE_FILE = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BUNDLE_ALIAS_OFFSET_TABLE_FILE(IntPtr L)
	{
		try
		{
			CommonUtils.BUNDLE_ALIAS_OFFSET_TABLE_FILE = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
