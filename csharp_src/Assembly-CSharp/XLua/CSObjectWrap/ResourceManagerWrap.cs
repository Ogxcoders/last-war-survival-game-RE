using System;
using System.Collections.Generic;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourceManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ResourceManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 52, 14, 4);
		Utils.RegisterFunc(L, -3, "EditorRecordCreate", _m_EditorRecordCreate);
		Utils.RegisterFunc(L, -3, "EditorRecordSetupCost", _m_EditorRecordSetupCost);
		Utils.RegisterFunc(L, -3, "EditorDescription", _m_EditorDescription);
		Utils.RegisterFunc(L, -3, "EditorGetPoolInfo", _m_EditorGetPoolInfo);
		Utils.RegisterFunc(L, -3, "EditorStartSample", _m_EditorStartSample);
		Utils.RegisterFunc(L, -3, "EditorEndSample", _m_EditorEndSample);
		Utils.RegisterFunc(L, -3, "EditorStartSampleSetupCost", _m_EditorStartSampleSetupCost);
		Utils.RegisterFunc(L, -3, "EditorEndSampleSetupCost", _m_EditorEndSampleSetupCost);
		Utils.RegisterFunc(L, -3, "RecordRequest", _m_RecordRequest);
		Utils.RegisterFunc(L, -3, "EditorStartSampleInstCost", _m_EditorStartSampleInstCost);
		Utils.RegisterFunc(L, -3, "EditorEndSampleInstCost", _m_EditorEndSampleInstCost);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "GetManifestNames", _m_GetManifestNames);
		Utils.RegisterFunc(L, -3, "GetManifestNamesInUse", _m_GetManifestNamesInUse);
		Utils.RegisterFunc(L, -3, "GetBkgroundManifestName", _m_GetBkgroundManifestName);
		Utils.RegisterFunc(L, -3, "GetPackageResManifestName", _m_GetPackageResManifestName);
		Utils.RegisterFunc(L, -3, "GetTempDownloadPath", _m_GetTempDownloadPath);
		Utils.RegisterFunc(L, -3, "OverrideManifest", _m_OverrideManifest);
		Utils.RegisterFunc(L, -3, "UpdateManifests", _m_UpdateManifests);
		Utils.RegisterFunc(L, -3, "GetDownloadSize", _m_GetDownloadSize);
		Utils.RegisterFunc(L, -3, "DeleteBundle", _m_DeleteBundle);
		Utils.RegisterFunc(L, -3, "DownloadUpdates", _m_DownloadUpdates);
		Utils.RegisterFunc(L, -3, "StartBkgroundDownload", _m_StartBkgroundDownload);
		Utils.RegisterFunc(L, -3, "BeginWhiteListCheck", _m_BeginWhiteListCheck);
		Utils.RegisterFunc(L, -3, "EndWhiteListCheck", _m_EndWhiteListCheck);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "ReadAllBytes", _m_ReadAllBytes);
		Utils.RegisterFunc(L, -3, "LoadAsset", _m_LoadAsset);
		Utils.RegisterFunc(L, -3, "LoadAssetAsync", _m_LoadAssetAsync);
		Utils.RegisterFunc(L, -3, "PreloadAsset", _m_PreloadAsset);
		Utils.RegisterFunc(L, -3, "UnloadAsset", _m_UnloadAsset);
		Utils.RegisterFunc(L, -3, "HasAsset", _m_HasAsset);
		Utils.RegisterFunc(L, -3, "IsAssetDownloaded", _m_IsAssetDownloaded);
		Utils.RegisterFunc(L, -3, "UnloadUnusedAssets", _m_UnloadUnusedAssets);
		Utils.RegisterFunc(L, -3, "UnloadUnusedAssetsSceneChange", _m_UnloadUnusedAssetsSceneChange);
		Utils.RegisterFunc(L, -3, "CollectGarbage", _m_CollectGarbage);
		Utils.RegisterFunc(L, -3, "DebugOutput", _m_DebugOutput);
		Utils.RegisterFunc(L, -3, "DebugLoadCount", _m_DebugLoadCount);
		Utils.RegisterFunc(L, -3, "RemoveCachedUnusedAssets", _m_RemoveCachedUnusedAssets);
		Utils.RegisterFunc(L, -3, "GetRawFilePath", _m_GetRawFilePath);
		Utils.RegisterFunc(L, -3, "GetResVersion", _m_GetResVersion);
		Utils.RegisterFunc(L, -3, "SyncGameLogicInfo", _m_SyncGameLogicInfo);
		Utils.RegisterFunc(L, -3, "ClearGameLogicInfo", _m_ClearGameLogicInfo);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "GetObjectPool", _m_GetObjectPool);
		Utils.RegisterFunc(L, -3, "PrefabHasCache", _m_PrefabHasCache);
		Utils.RegisterFunc(L, -3, "PrefabPoolIsReady", _m_PrefabPoolIsReady);
		Utils.RegisterFunc(L, -3, "PrefabAssetsDownloaded", _m_PrefabAssetsDownloaded);
		Utils.RegisterFunc(L, -3, "ClearPoolByTag", _m_ClearPoolByTag);
		Utils.RegisterFunc(L, -3, "ClearPoolByTagGroup", _m_ClearPoolByTagGroup);
		Utils.RegisterFunc(L, -3, "InstantiateAsync", _m_InstantiateAsync);
		Utils.RegisterFunc(L, -3, "InstantiateAsyncImmediately", _m_InstantiateAsyncImmediately);
		Utils.RegisterFunc(L, -2, "EditorSampling", _g_get_EditorSampling);
		Utils.RegisterFunc(L, -2, "asyncDelayPath", _g_get_asyncDelayPath);
		Utils.RegisterFunc(L, -2, "AccCheckVersionURL", _g_get_AccCheckVersionURL);
		Utils.RegisterFunc(L, -2, "CheckVersionURL", _g_get_CheckVersionURL);
		Utils.RegisterFunc(L, -2, "Loggable", _g_get_Loggable);
		Utils.RegisterFunc(L, -2, "IsSimulation", _g_get_IsSimulation);
		Utils.RegisterFunc(L, -2, "SkipUpdateBundle", _g_get_SkipUpdateBundle);
		Utils.RegisterFunc(L, -2, "ObjectPoolRootTrans", _g_get_ObjectPoolRootTrans);
		Utils.RegisterFunc(L, -2, "GameResManifestName", _g_get_GameResManifestName);
		Utils.RegisterFunc(L, -2, "DataTableManifestName", _g_get_DataTableManifestName);
		Utils.RegisterFunc(L, -2, "LuaManifestName", _g_get_LuaManifestName);
		Utils.RegisterFunc(L, -2, "DllResManifestName", _g_get_DllResManifestName);
		Utils.RegisterFunc(L, -2, "asyncDelayTime", _g_get_asyncDelayTime);
		Utils.RegisterFunc(L, -2, "asyncDelayPathSet", _g_get_asyncDelayPathSet);
		Utils.RegisterFunc(L, -1, "asyncDelayPath", _s_set_asyncDelayPath);
		Utils.RegisterFunc(L, -1, "Loggable", _s_set_Loggable);
		Utils.RegisterFunc(L, -1, "asyncDelayTime", _s_set_asyncDelayTime);
		Utils.RegisterFunc(L, -1, "asyncDelayPathSet", _s_set_asyncDelayPathSet);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "LoadAssetStatic", _m_LoadAssetStatic_xlua_st_);
		Utils.RegisterFunc(L, -4, "LoadAssetAsyncStatic", _m_LoadAssetAsyncStatic_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "BUNDLE_OFFSET_TABLE_FILE", "BundleOffsetTable.bytes");
		Utils.RegisterObject(L, translator, -4, "k_GenGPUAnimAssetPath", "Assets/_Art_LastWar/GenGPUAnim/");
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
				ResourceManager o = new ResourceManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorRecordCreate(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabName = Lua.lua_tostring(L, 2);
			long msCost = Lua.lua_toint64(L, 3);
			bool fromPool = Lua.lua_toboolean(L, 4);
			obj.EditorRecordCreate(prefabName, msCost, fromPool);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorRecordSetupCost(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabName = Lua.lua_tostring(L, 2);
			long msCost = Lua.lua_toint64(L, 3);
			obj.EditorRecordSetupCost(prefabName, msCost);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorDescription(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string desc = Lua.lua_tostring(L, 2);
			obj.EditorDescription(ref desc);
			Lua.lua_pushstring(L, desc);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorGetPoolInfo(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string desc = Lua.lua_tostring(L, 2);
			obj.EditorGetPoolInfo(ref desc);
			Lua.lua_pushstring(L, desc);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorStartSample(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EditorStartSample();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorEndSample(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EditorEndSample();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorStartSampleSetupCost(IntPtr L)
	{
		try
		{
			_ = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_tostring(L, 2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorEndSampleSetupCost(IntPtr L)
	{
		try
		{
			_ = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordRequest(IntPtr L)
	{
		try
		{
			_ = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_tostring(L, 2);
			Lua.xlua_tointeger(L, 3);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorStartSampleInstCost(IntPtr L)
	{
		try
		{
			_ = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_tostring(L, 2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorEndSampleInstCost(IntPtr L)
	{
		try
		{
			_ = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_toboolean(L, 2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			Action<bool> onComplete = objectTranslator.GetDelegate<Action<bool>>(L, 2);
			resourceManager.Initialize(onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetManifestNames(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string[] manifestNames = ((ResourceManager)objectTranslator.FastGetCSObj(L, 1)).GetManifestNames();
			objectTranslator.Push(L, manifestNames);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetManifestNamesInUse(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<string> manifestNamesInUse = ((ResourceManager)objectTranslator.FastGetCSObj(L, 1)).GetManifestNamesInUse();
			objectTranslator.Push(L, manifestNamesInUse);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBkgroundManifestName(IntPtr L)
	{
		try
		{
			string bkgroundManifestName = ((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetBkgroundManifestName();
			Lua.lua_pushstring(L, bkgroundManifestName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPackageResManifestName(IntPtr L)
	{
		try
		{
			string packageResManifestName = ((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPackageResManifestName();
			Lua.lua_pushstring(L, packageResManifestName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTempDownloadPath(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string file = Lua.lua_tostring(L, 2);
			string tempDownloadPath = obj.GetTempDownloadPath(file);
			Lua.lua_pushstring(L, tempDownloadPath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverrideManifest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Manifest>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Manifest manifest = (Manifest)objectTranslator.GetObject(L, 2, typeof(Manifest));
				bool refreshMemory = Lua.lua_toboolean(L, 3);
				resourceManager.OverrideManifest(manifest, refreshMemory);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Manifest>(L, 2))
			{
				Manifest manifest2 = (Manifest)objectTranslator.GetObject(L, 2, typeof(Manifest));
				resourceManager.OverrideManifest(manifest2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.OverrideManifest!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateManifests(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UpdateVersions o = ((ResourceManager)objectTranslator.FastGetCSObj(L, 1)).UpdateManifests();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDownloadSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<List<Manifest>>(L, 2) && objectTranslator.Assignable<List<DownloadInfo>>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DownloadChecker>(L, 5))
			{
				List<Manifest> manifests = (List<Manifest>)objectTranslator.GetObject(L, 2, typeof(List<Manifest>));
				List<DownloadInfo> downloadInfos = (List<DownloadInfo>)objectTranslator.GetObject(L, 3, typeof(List<DownloadInfo>));
				string targetPath = Lua.lua_tostring(L, 4);
				DownloadChecker downloadChecker = (DownloadChecker)objectTranslator.GetObject(L, 5, typeof(DownloadChecker));
				ulong downloadSize = resourceManager.GetDownloadSize(manifests, downloadInfos, targetPath, downloadChecker);
				Lua.lua_pushuint64(L, downloadSize);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<List<Manifest>>(L, 2) && objectTranslator.Assignable<List<DownloadInfo>>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				List<Manifest> manifests2 = (List<Manifest>)objectTranslator.GetObject(L, 2, typeof(List<Manifest>));
				List<DownloadInfo> downloadInfos2 = (List<DownloadInfo>)objectTranslator.GetObject(L, 3, typeof(List<DownloadInfo>));
				string targetPath2 = Lua.lua_tostring(L, 4);
				ulong downloadSize2 = resourceManager.GetDownloadSize(manifests2, downloadInfos2, targetPath2);
				Lua.lua_pushuint64(L, downloadSize2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<List<Manifest>>(L, 2) && objectTranslator.Assignable<List<DownloadInfo>>(L, 3))
			{
				List<Manifest> manifests3 = (List<Manifest>)objectTranslator.GetObject(L, 2, typeof(List<Manifest>));
				List<DownloadInfo> downloadInfos3 = (List<DownloadInfo>)objectTranslator.GetObject(L, 3, typeof(List<DownloadInfo>));
				ulong downloadSize3 = resourceManager.GetDownloadSize(manifests3, downloadInfos3);
				Lua.lua_pushuint64(L, downloadSize3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.GetDownloadSize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteBundle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			BundleInfo bundle = (BundleInfo)objectTranslator.GetObject(L, 2, typeof(BundleInfo));
			resourceManager.DeleteBundle(bundle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DownloadUpdates(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<List<DownloadInfo>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				List<DownloadInfo> downloadInfos = (List<DownloadInfo>)objectTranslator.GetObject(L, 2, typeof(List<DownloadInfo>));
				int queueID = Lua.xlua_tointeger(L, 3);
				DownloadVersions o = resourceManager.DownloadUpdates(downloadInfos, queueID);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<List<DownloadInfo>>(L, 2))
			{
				List<DownloadInfo> downloadInfos2 = (List<DownloadInfo>)objectTranslator.GetObject(L, 2, typeof(List<DownloadInfo>));
				DownloadVersions o2 = resourceManager.DownloadUpdates(downloadInfos2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.DownloadUpdates!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartBkgroundDownload(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			List<string> manifestNames = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
			resourceManager.StartBkgroundDownload(manifestNames);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BeginWhiteListCheck(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).BeginWhiteListCheck();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndWhiteListCheck(IntPtr L)
	{
		try
		{
			bool value = ((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndWhiteListCheck();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReadAllBytes(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string assetPath = Lua.lua_tostring(L, 2);
			byte[] str = obj.ReadAllBytes(assetPath);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			Type type = (Type)objectTranslator.GetObject(L, 3, typeof(Type));
			Asset o = resourceManager.LoadAsset(path, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadAssetStatic_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Asset o = ResourceManager.LoadAssetStatic(path, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadAssetAsync(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			Type type = (Type)objectTranslator.GetObject(L, 3, typeof(Type));
			Asset o = resourceManager.LoadAssetAsync(path, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadAssetAsyncStatic_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Asset o = ResourceManager.LoadAssetAsyncStatic(path, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Type>(L, 3) && objectTranslator.Assignable<ResourceManager.PreloadType>(L, 4))
			{
				string path = Lua.lua_tostring(L, 2);
				Type type = (Type)objectTranslator.GetObject(L, 3, typeof(Type));
				objectTranslator.Get(L, 4, out ResourceManager.PreloadType val);
				resourceManager.PreloadAsset(path, type, val);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Type>(L, 3))
			{
				string path2 = Lua.lua_tostring(L, 2);
				Type type2 = (Type)objectTranslator.GetObject(L, 3, typeof(Type));
				resourceManager.PreloadAsset(path2, type2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.PreloadAsset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			Asset asset = (Asset)objectTranslator.GetObject(L, 2, typeof(Asset));
			resourceManager.UnloadAsset(asset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasAsset(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			bool value = obj.HasAsset(path);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAssetDownloaded(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			bool value = obj.IsAssetDownloaded(path);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadUnusedAssets(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadUnusedAssets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadUnusedAssetsSceneChange(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadUnusedAssetsSceneChange();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CollectGarbage(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CollectGarbage();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugOutput(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DebugOutput();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugLoadCount(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DebugLoadCount();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCachedUnusedAssets(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveCachedUnusedAssets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRawFilePath(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			string rawFilePath = obj.GetRawFilePath(path);
			Lua.lua_pushstring(L, rawFilePath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResVersion(IntPtr L)
	{
		try
		{
			string resVersion = ((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetResVersion();
			Lua.lua_pushstring(L, resVersion);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncGameLogicInfo(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string paramStr = Lua.lua_tostring(L, 2);
			obj.SyncGameLogicInfo(paramStr);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearGameLogicInfo(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearGameLogicInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectPool(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ObjectPoolTag>(L, 3))
			{
				string prefabPath = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out ObjectPoolTag val);
				global::ObjectPool objectPool = resourceManager.GetObjectPool(prefabPath, val);
				objectTranslator.Push(L, objectPool);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string prefabPath2 = Lua.lua_tostring(L, 2);
				global::ObjectPool objectPool2 = resourceManager.GetObjectPool(prefabPath2);
				objectTranslator.Push(L, objectPool2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.GetObjectPool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrefabHasCache(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			bool value = obj.PrefabHasCache(prefabPath);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrefabPoolIsReady(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			bool value = obj.PrefabPoolIsReady(prefabPath);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrefabAssetsDownloaded(IntPtr L)
	{
		try
		{
			ResourceManager obj = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			bool value = obj.PrefabAssetsDownloaded(prefabPath);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPoolByTag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ObjectPoolTag val);
			resourceManager.ClearPoolByTag(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPoolByTagGroup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ObjectPoolTagGroup val);
			resourceManager.ClearPoolByTagGroup(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InstantiateAsync(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Func<string, BasePool>>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string prefabPath = Lua.lua_tostring(L, 2);
				Func<string, BasePool> poolFunc = objectTranslator.GetDelegate<Func<string, BasePool>>(L, 3);
				int property = Lua.xlua_tointeger(L, 4);
				InstanceRequest o = resourceManager.InstantiateAsync(prefabPath, poolFunc, property);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Func<string, BasePool>>(L, 3))
			{
				string prefabPath2 = Lua.lua_tostring(L, 2);
				Func<string, BasePool> poolFunc2 = objectTranslator.GetDelegate<Func<string, BasePool>>(L, 3);
				InstanceRequest o2 = resourceManager.InstantiateAsync(prefabPath2, poolFunc2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ObjectPoolTag>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string prefabPath3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out ObjectPoolTag val);
				int property2 = Lua.xlua_tointeger(L, 4);
				InstanceRequest o3 = resourceManager.InstantiateAsync(prefabPath3, val, property2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ObjectPoolTag>(L, 3))
			{
				string prefabPath4 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out ObjectPoolTag val2);
				InstanceRequest o4 = resourceManager.InstantiateAsync(prefabPath4, val2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string prefabPath5 = Lua.lua_tostring(L, 2);
				InstanceRequest o5 = resourceManager.InstantiateAsync(prefabPath5);
				objectTranslator.Push(L, o5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.InstantiateAsync!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InstantiateAsyncImmediately(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string prefabPath = Lua.lua_tostring(L, 2);
				InstanceRequest o = resourceManager.InstantiateAsyncImmediately(prefabPath);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<InstanceRequest>>(L, 3))
			{
				string prefabPath2 = Lua.lua_tostring(L, 2);
				Action<InstanceRequest> cb = objectTranslator.GetDelegate<Action<InstanceRequest>>(L, 3);
				InstanceRequest o2 = resourceManager.InstantiateAsyncImmediately(prefabPath2, cb);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.InstantiateAsyncImmediately!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EditorSampling(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, resourceManager.EditorSampling);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncDelayPath(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.asyncDelayPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AccCheckVersionURL(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.AccCheckVersionURL);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CheckVersionURL(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.CheckVersionURL);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Loggable(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, resourceManager.Loggable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsSimulation(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, resourceManager.IsSimulation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkipUpdateBundle(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, resourceManager.SkipUpdateBundle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ObjectPoolRootTrans(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, resourceManager.ObjectPoolRootTrans);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameResManifestName(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.GameResManifestName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DataTableManifestName(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.DataTableManifestName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LuaManifestName(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.LuaManifestName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DllResManifestName(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, resourceManager.DllResManifestName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncDelayTime(IntPtr L)
	{
		try
		{
			ResourceManager resourceManager = (ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, resourceManager.asyncDelayTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncDelayPathSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager resourceManager = (ResourceManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, resourceManager.asyncDelayPathSet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncDelayPath(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).asyncDelayPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Loggable(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Loggable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncDelayTime(IntPtr L)
	{
		try
		{
			((ResourceManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).asyncDelayTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncDelayPathSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ResourceManager)objectTranslator.FastGetCSObj(L, 1)).asyncDelayPathSet = (HashSet<string>)objectTranslator.GetObject(L, 2, typeof(HashSet<string>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
