using System;
using System.IO;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FileUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FileUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 13, 0, 0);
		Utils.RegisterFunc(L, -4, "ExistDirectory", _m_ExistDirectory_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetScript", _m_GetScript_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExistFile", _m_ExistFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteDirectoryIfExists", _m_DeleteDirectoryIfExists_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteFileIfExists", _m_DeleteFileIfExists_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateFileDirectoryIfNotExists", _m_CreateFileDirectoryIfNotExists_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateFile", _m_CreateFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateText", _m_CreateText_xlua_st_);
		Utils.RegisterFunc(L, -4, "WriteFile", _m_WriteFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "CopyFile", _m_CopyFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetFileSize", _m_GetFileSize_xlua_st_);
		Utils.RegisterFunc(L, -4, "CopyFilesRecursively", _m_CopyFilesRecursively_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "FileUtils does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExistDirectory_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = FileUtils.ExistDirectory(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetScript_xlua_st_(IntPtr L)
	{
		try
		{
			string script = FileUtils.GetScript(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, script);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExistFile_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = FileUtils.ExistFile(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteDirectoryIfExists_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				string path = Lua.lua_tostring(L, 1);
				bool recursive = Lua.lua_toboolean(L, 2);
				FileUtils.DeleteDirectoryIfExists(path, recursive);
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				FileUtils.DeleteDirectoryIfExists(Lua.lua_tostring(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FileUtils.DeleteDirectoryIfExists!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteFileIfExists_xlua_st_(IntPtr L)
	{
		try
		{
			FileUtils.DeleteFileIfExists(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateFileDirectoryIfNotExists_xlua_st_(IntPtr L)
	{
		try
		{
			FileUtils.CreateFileDirectoryIfNotExists(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateFile_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FileStream o = FileUtils.CreateFile(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateText_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			StreamWriter o = FileUtils.CreateText(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WriteFile_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string path = Lua.lua_tostring(L, 1);
				string content = Lua.lua_tostring(L, 2);
				FileUtils.WriteFile(path, content);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string path2 = Lua.lua_tostring(L, 1);
				byte[] bytedata = Lua.lua_tobytes(L, 2);
				bool overwrite = Lua.lua_toboolean(L, 3);
				FileUtils.WriteFile(path2, bytedata, overwrite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string path3 = Lua.lua_tostring(L, 1);
				byte[] bytedata2 = Lua.lua_tobytes(L, 2);
				FileUtils.WriteFile(path3, bytedata2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FileUtils.WriteFile!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyFile_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string srcPath = Lua.lua_tostring(L, 1);
				string dstPath = Lua.lua_tostring(L, 2);
				bool overwrite = Lua.lua_toboolean(L, 3);
				FileUtils.CopyFile(srcPath, dstPath, overwrite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string srcPath2 = Lua.lua_tostring(L, 1);
				string dstPath2 = Lua.lua_tostring(L, 2);
				FileUtils.CopyFile(srcPath2, dstPath2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FileUtils.CopyFile!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFileSize_xlua_st_(IntPtr L)
	{
		try
		{
			long fileSize = FileUtils.GetFileSize(Lua.lua_tostring(L, 1));
			Lua.lua_pushint64(L, fileSize);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyFilesRecursively_xlua_st_(IntPtr L)
	{
		try
		{
			string sourcePath = Lua.lua_tostring(L, 1);
			string targetPath = Lua.lua_tostring(L, 2);
			FileUtils.CopyFilesRecursively(sourcePath, targetPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
