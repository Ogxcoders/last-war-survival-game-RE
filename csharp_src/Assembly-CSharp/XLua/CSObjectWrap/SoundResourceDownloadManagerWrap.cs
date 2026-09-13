using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SoundResourceDownloadManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SoundResourceDownloadManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "CheckLangResourceAutoDownload", _m_CheckLangResourceAutoDownload);
		Utils.RegisterFunc(L, -3, "OnDownloadFinish", _m_OnDownloadFinish);
		Utils.RegisterFunc(L, -3, "IsCanAsync", _m_IsCanAsync);
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
				SoundResourceDownloadManager o = new SoundResourceDownloadManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundResourceDownloadManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((SoundResourceDownloadManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit(IntPtr L)
	{
		try
		{
			((SoundResourceDownloadManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckLangResourceAutoDownload(IntPtr L)
	{
		try
		{
			SoundResourceDownloadManager obj = (SoundResourceDownloadManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int langIndex = Lua.xlua_tointeger(L, 2);
			string langName = Lua.lua_tostring(L, 3);
			obj.CheckLangResourceAutoDownload(langIndex, langName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDownloadFinish(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundResourceDownloadManager soundResourceDownloadManager = (SoundResourceDownloadManager)objectTranslator.FastGetCSObj(L, 1);
			object key = objectTranslator.GetObject(L, 2, typeof(object));
			soundResourceDownloadManager.OnDownloadFinish(key);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCanAsync(IntPtr L)
	{
		try
		{
			SoundResourceDownloadManager obj = (SoundResourceDownloadManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string soundAssetName = Lua.lua_tostring(L, 2);
			string soundGroupName = Lua.lua_tostring(L, 3);
			bool value = obj.IsCanAsync(soundAssetName, soundGroupName);
			Lua.lua_pushboolean(L, value);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, SoundResourceDownloadManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
