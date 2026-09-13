using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CommonCompressorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CommonCompressor);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "CompressFileGZip", _m_CompressFileGZip_xlua_st_);
		Utils.RegisterFunc(L, -4, "DecompressFileGZip", _m_DecompressFileGZip_xlua_st_);
		Utils.RegisterFunc(L, -4, "CompressFile", _m_CompressFile_xlua_st_);
		Utils.RegisterFunc(L, -4, "DecompressFile", _m_DecompressFile_xlua_st_);
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
				CommonCompressor o = new CommonCompressor();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CommonCompressor constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompressFileGZip_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			string desPath = Lua.lua_tostring(L, 2);
			CommonCompressor.CompressFileGZip(path, desPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DecompressFileGZip_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			string desPath = Lua.lua_tostring(L, 2);
			CommonCompressor.DecompressFileGZip(path, desPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompressFile_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			string desPath = Lua.lua_tostring(L, 2);
			CommonCompressor.CompressFile(path, desPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DecompressFile_xlua_st_(IntPtr L)
	{
		try
		{
			string path = Lua.lua_tostring(L, 1);
			string desPath = Lua.lua_tostring(L, 2);
			CommonCompressor.DecompressFile(path, desPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
