using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DynamicResourceManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DynamicResourceManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 18, 0, 0);
		Utils.RegisterFunc(L, -3, "CreateTexture2DTask", _m_CreateTexture2DTask);
		Utils.RegisterFunc(L, -3, "CreateAudioClipTask", _m_CreateAudioClipTask);
		Utils.RegisterFunc(L, -3, "IsTaskExist", _m_IsTaskExist);
		Utils.RegisterFunc(L, -3, "ReleaseAsset", _m_ReleaseAsset);
		Utils.RegisterFunc(L, -3, "GetCacheCount", _m_GetCacheCount);
		Utils.RegisterFunc(L, -3, "DumpCache", _m_DumpCache);
		Utils.RegisterFunc(L, -3, "CancelTask", _m_CancelTask);
		Utils.RegisterFunc(L, -3, "AbortTask", _m_AbortTask);
		Utils.RegisterFunc(L, -3, "GetBigPhotoPathBySmall", _m_GetBigPhotoPathBySmall);
		Utils.RegisterFunc(L, -3, "DeleteCacheCompressedPhoto", _m_DeleteCacheCompressedPhoto);
		Utils.RegisterFunc(L, -3, "DeleteFileByPath", _m_DeleteFileByPath);
		Utils.RegisterFunc(L, -3, "DeleteCacheCompressedPhotoFolder", _m_DeleteCacheCompressedPhotoFolder);
		Utils.RegisterFunc(L, -3, "MoveCompressedPhotoToTargetPath", _m_MoveCompressedPhotoToTargetPath);
		Utils.RegisterFunc(L, -3, "MovePhotoToTargetPath", _m_MovePhotoToTargetPath);
		Utils.RegisterFunc(L, -3, "CopyCompressedPhotoToTargetPath", _m_CopyCompressedPhotoToTargetPath);
		Utils.RegisterFunc(L, -3, "CopyPhotoToTargetPath", _m_CopyPhotoToTargetPath);
		Utils.RegisterFunc(L, -3, "OnUpdate", _e_OnUpdate);
		Utils.RegisterFunc(L, -3, "OnLowMemory", _e_OnLowMemory);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 2, 1);
		Utils.RegisterFunc(L, -4, "GetSaveAssetPath", _m_GetSaveAssetPath_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.RegisterFunc(L, -2, "downloadPathForUrl", _g_get_downloadPathForUrl);
		Utils.RegisterFunc(L, -1, "downloadPathForUrl", _s_set_downloadPathForUrl);
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
				DynamicResourceManager o = new DynamicResourceManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicResourceManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTexture2DTask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicResourceManager dynamicResourceManager = (DynamicResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 8 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DynamicResourceManager.OnLoadComplete>(L, 3) && objectTranslator.Assignable<object>(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				string assetKey = Lua.lua_tostring(L, 2);
				DynamicResourceManager.OnLoadComplete callback = objectTranslator.GetDelegate<DynamicResourceManager.OnLoadComplete>(L, 3);
				object userdata = objectTranslator.GetObject(L, 4, typeof(object));
				string cdnPath = Lua.lua_tostring(L, 5);
				string cacheFolder = Lua.lua_tostring(L, 6);
				bool nonReadable = Lua.lua_toboolean(L, 7);
				bool ignoreCache = Lua.lua_toboolean(L, 8);
				dynamicResourceManager.CreateTexture2DTask(assetKey, callback, userdata, cdnPath, cacheFolder, nonReadable, ignoreCache);
				return 0;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DynamicResourceManager.OnLoadComplete>(L, 3) && objectTranslator.Assignable<object>(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				string assetKey2 = Lua.lua_tostring(L, 2);
				DynamicResourceManager.OnLoadComplete callback2 = objectTranslator.GetDelegate<DynamicResourceManager.OnLoadComplete>(L, 3);
				object userdata2 = objectTranslator.GetObject(L, 4, typeof(object));
				string cdnPath2 = Lua.lua_tostring(L, 5);
				string cacheFolder2 = Lua.lua_tostring(L, 6);
				bool nonReadable2 = Lua.lua_toboolean(L, 7);
				dynamicResourceManager.CreateTexture2DTask(assetKey2, callback2, userdata2, cdnPath2, cacheFolder2, nonReadable2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicResourceManager.CreateTexture2DTask!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateAudioClipTask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicResourceManager dynamicResourceManager = (DynamicResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 8 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DynamicResourceManager.OnLoadComplete>(L, 3) && objectTranslator.Assignable<object>(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<AudioType>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				string assetKey = Lua.lua_tostring(L, 2);
				DynamicResourceManager.OnLoadComplete callback = objectTranslator.GetDelegate<DynamicResourceManager.OnLoadComplete>(L, 3);
				object userdata = objectTranslator.GetObject(L, 4, typeof(object));
				string cdnPath = Lua.lua_tostring(L, 5);
				string cacheFolder = Lua.lua_tostring(L, 6);
				objectTranslator.Get(L, 7, out AudioType v);
				bool ignoreCache = Lua.lua_toboolean(L, 8);
				dynamicResourceManager.CreateAudioClipTask(assetKey, callback, userdata, cdnPath, cacheFolder, v, ignoreCache);
				return 0;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DynamicResourceManager.OnLoadComplete>(L, 3) && objectTranslator.Assignable<object>(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<AudioType>(L, 7))
			{
				string assetKey2 = Lua.lua_tostring(L, 2);
				DynamicResourceManager.OnLoadComplete callback2 = objectTranslator.GetDelegate<DynamicResourceManager.OnLoadComplete>(L, 3);
				object userdata2 = objectTranslator.GetObject(L, 4, typeof(object));
				string cdnPath2 = Lua.lua_tostring(L, 5);
				string cacheFolder2 = Lua.lua_tostring(L, 6);
				objectTranslator.Get(L, 7, out AudioType v2);
				dynamicResourceManager.CreateAudioClipTask(assetKey2, callback2, userdata2, cdnPath2, cacheFolder2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicResourceManager.CreateAudioClipTask!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTaskExist(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string assetKey = Lua.lua_tostring(L, 2);
			bool value = obj.IsTaskExist(assetKey);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReleaseAsset(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string assetKey = Lua.lua_tostring(L, 2);
			obj.ReleaseAsset(assetKey);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCacheCount(IntPtr L)
	{
		try
		{
			int cacheCount = ((DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCacheCount();
			Lua.xlua_pushinteger(L, cacheCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DumpCache(IntPtr L)
	{
		try
		{
			string str = ((DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DumpCache();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelTask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicResourceManager dynamicResourceManager = (DynamicResourceManager)objectTranslator.FastGetCSObj(L, 1);
			string assetKey = Lua.lua_tostring(L, 2);
			DynamicResourceManager.OnLoadComplete callback = objectTranslator.GetDelegate<DynamicResourceManager.OnLoadComplete>(L, 3);
			dynamicResourceManager.CancelTask(assetKey, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AbortTask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicResourceManager dynamicResourceManager = (DynamicResourceManager)objectTranslator.FastGetCSObj(L, 1);
			string assetKey = Lua.lua_tostring(L, 2);
			DynamicResourceManager.OnLoadComplete callback = objectTranslator.GetDelegate<DynamicResourceManager.OnLoadComplete>(L, 3);
			dynamicResourceManager.AbortTask(assetKey, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSaveAssetPath_xlua_st_(IntPtr L)
	{
		try
		{
			string cacheFolder = Lua.lua_tostring(L, 1);
			string assetKey = Lua.lua_tostring(L, 2);
			string saveAssetPath = DynamicResourceManager.GetSaveAssetPath(cacheFolder, assetKey);
			Lua.lua_pushstring(L, saveAssetPath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBigPhotoPathBySmall(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string filePathSmall = Lua.lua_tostring(L, 2);
			string bigPhotoPathBySmall = obj.GetBigPhotoPathBySmall(filePathSmall);
			Lua.lua_pushstring(L, bigPhotoPathBySmall);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteCacheCompressedPhoto(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string filePathSmall = Lua.lua_tostring(L, 2);
			obj.DeleteCacheCompressedPhoto(filePathSmall);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteFileByPath(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string filePath = Lua.lua_tostring(L, 2);
			obj.DeleteFileByPath(filePath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteCacheCompressedPhotoFolder(IntPtr L)
	{
		try
		{
			((DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DeleteCacheCompressedPhotoFolder();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveCompressedPhotoToTargetPath(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string filePathSmall = Lua.lua_tostring(L, 2);
			string assetKey = Lua.lua_tostring(L, 3);
			string assetKeyBig = Lua.lua_tostring(L, 4);
			obj.MoveCompressedPhotoToTargetPath(filePathSmall, assetKey, assetKeyBig);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePhotoToTargetPath(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string sourcePath = Lua.lua_tostring(L, 2);
			string destinationPath = Lua.lua_tostring(L, 3);
			obj.MovePhotoToTargetPath(sourcePath, destinationPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyCompressedPhotoToTargetPath(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string filePathSmall = Lua.lua_tostring(L, 2);
			string assetKey = Lua.lua_tostring(L, 3);
			string assetKeyBig = Lua.lua_tostring(L, 4);
			obj.CopyCompressedPhotoToTargetPath(filePathSmall, assetKey, assetKeyBig);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyPhotoToTargetPath(IntPtr L)
	{
		try
		{
			DynamicResourceManager obj = (DynamicResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string sourcePath = Lua.lua_tostring(L, 2);
			string destinationPath = Lua.lua_tostring(L, 3);
			obj.CopyPhotoToTargetPath(sourcePath, destinationPath);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, DynamicResourceManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadPathForUrl(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, DynamicResourceManager.downloadPathForUrl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_downloadPathForUrl(IntPtr L)
	{
		try
		{
			DynamicResourceManager.downloadPathForUrl = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicResourceManager dynamicResourceManager = (DynamicResourceManager)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicResourceManager.OnUpdate += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicResourceManager.OnUpdate -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicResourceManager.OnUpdate!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnLowMemory(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicResourceManager dynamicResourceManager = (DynamicResourceManager)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicResourceManager.OnLowMemory += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicResourceManager.OnLowMemory -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicResourceManager.OnLowMemory!");
		return 0;
	}
}
