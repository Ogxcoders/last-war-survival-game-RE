using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MailFileContentManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MailFileContentManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 8, 0, 0);
		Utils.RegisterFunc(L, -4, "InitFileManager", _m_InitFileManager_xlua_st_);
		Utils.RegisterFunc(L, -4, "UninitFileManager", _m_UninitFileManager_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsFileExist", _m_IsFileExist_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteFile", _m_DeleteFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateFile", _m_CreateFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReadFile", _m_ReadFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteFileList", _m_DeleteFileList_xlua_st_);
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
				MailFileContentManager o = new MailFileContentManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MailFileContentManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitFileManager_xlua_st_(IntPtr L)
	{
		try
		{
			MailFileContentManager.InitFileManager(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UninitFileManager_xlua_st_(IntPtr L)
	{
		try
		{
			MailFileContentManager.UninitFileManager();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFileExist_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = MailFileContentManager.IsFileExist(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteFile_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string id = Lua.lua_tostring(L, 1);
			Action callback = objectTranslator.GetDelegate<Action>(L, 2);
			MailFileContentManager.DeleteFile(id, callback);
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
			string id = Lua.lua_tostring(L, 1);
			string content = Lua.lua_tostring(L, 2);
			Action callback = objectTranslator.GetDelegate<Action>(L, 3);
			MailFileContentManager.CreateFile(id, content, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReadFile_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string id = Lua.lua_tostring(L, 1);
			Action<string> callback = objectTranslator.GetDelegate<Action<string>>(L, 2);
			MailFileContentManager.ReadFile(id, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteFileList_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<string> ids = (List<string>)objectTranslator.GetObject(L, 1, typeof(List<string>));
			Action callback = objectTranslator.GetDelegate<Action>(L, 2);
			MailFileContentManager.DeleteFileList(ids, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
