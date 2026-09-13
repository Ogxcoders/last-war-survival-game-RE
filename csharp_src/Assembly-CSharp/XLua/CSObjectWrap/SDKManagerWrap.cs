using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SDKManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SDKManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 79, 13, 6);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "ApplicationWillEnterForeground", _m_ApplicationWillEnterForeground);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.RegisterFunc(L, -3, "SetPayMangerCallback", _m_SetPayMangerCallback);
		Utils.RegisterFunc(L, -3, "GetPublishRegion", _m_GetPublishRegion);
		Utils.RegisterFunc(L, -3, "GetPackageName", _m_GetPackageName);
		Utils.RegisterFunc(L, -3, "GetPackageSign", _m_GetPackageSign);
		Utils.RegisterFunc(L, -3, "getChannel", _m_getChannel);
		Utils.RegisterFunc(L, -3, "saveDataToSdcard", _m_saveDataToSdcard);
		Utils.RegisterFunc(L, -3, "RequestSdCardPermission", _m_RequestSdCardPermission);
		Utils.RegisterFunc(L, -3, "DoInitGooglePay", _m_DoInitGooglePay);
		Utils.RegisterFunc(L, -3, "DoInitShop", _m_DoInitShop);
		Utils.RegisterFunc(L, -3, "IsSimulator", _m_IsSimulator);
		Utils.RegisterFunc(L, -3, "CheckDownloadGoogleApk", _m_CheckDownloadGoogleApk);
		Utils.RegisterFunc(L, -3, "GetDeviceUDID", _m_GetDeviceUDID);
		Utils.RegisterFunc(L, -3, "GetSerialID", _m_GetSerialID);
		Utils.RegisterFunc(L, -3, "GetDeviceInfo", _m_GetDeviceInfo);
		Utils.RegisterFunc(L, -3, "SetAndroidScreenNotch", _m_SetAndroidScreenNotch);
		Utils.RegisterFunc(L, -3, "GetHandSetInfo", _m_GetHandSetInfo);
		Utils.RegisterFunc(L, -3, "GenerateHighVersionUUID", _m_GenerateHighVersionUUID);
		Utils.RegisterFunc(L, -3, "GetSimOperator", _m_GetSimOperator);
		Utils.RegisterFunc(L, -3, "GetSimOperatorName", _m_GetSimOperatorName);
		Utils.RegisterFunc(L, -3, "getClickPushTag", _m_getClickPushTag);
		Utils.RegisterFunc(L, -3, "getClickPushId", _m_getClickPushId);
		Utils.RegisterFunc(L, -3, "getClickPushTime", _m_getClickPushTime);
		Utils.RegisterFunc(L, -3, "ClearAllPushData", _m_ClearAllPushData);
		Utils.RegisterFunc(L, -3, "CopyTextToClipboard", _m_CopyTextToClipboard);
		Utils.RegisterFunc(L, -3, "GetIsNotifyOpen", _m_GetIsNotifyOpen);
		Utils.RegisterFunc(L, -3, "AskForNotifyPermission", _m_AskForNotifyPermission);
		Utils.RegisterFunc(L, -3, "IsTrackingEnabled", _m_IsTrackingEnabled);
		Utils.RegisterFunc(L, -3, "OpenSettings", _m_OpenSettings);
		Utils.RegisterFunc(L, -3, "LogEvent", _m_LogEvent);
		Utils.RegisterFunc(L, -3, "GetBuildInfo", _m_GetBuildInfo);
		Utils.RegisterFunc(L, -3, "GetPermissionByType", _m_GetPermissionByType);
		Utils.RegisterFunc(L, -3, "LogEventLevelUp", _m_LogEventLevelUp);
		Utils.RegisterFunc(L, -3, "Login", _m_Login);
		Utils.RegisterFunc(L, -3, "SetAccountFunc", _m_SetAccountFunc);
		Utils.RegisterFunc(L, -3, "Logout", _m_Logout);
		Utils.RegisterFunc(L, -3, "Pay", _m_Pay);
		Utils.RegisterFunc(L, -3, "CheckPayEnv", _m_CheckPayEnv);
		Utils.RegisterFunc(L, -3, "Get_PF_DisplayName", _m_Get_PF_DisplayName);
		Utils.RegisterFunc(L, -3, "ConsumeProduct", _m_ConsumeProduct);
		Utils.RegisterFunc(L, -3, "GotoMarket", _m_GotoMarket);
		Utils.RegisterFunc(L, -3, "SendDataToGame", _m_SendDataToGame);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "HandleEvent", _m_HandleEvent);
		Utils.RegisterFunc(L, -3, "SendDataToNative", _m_SendDataToNative);
		Utils.RegisterFunc(L, -3, "GetDataFromNative", _m_GetDataFromNative);
		Utils.RegisterFunc(L, -3, "HideSplash", _m_HideSplash);
		Utils.RegisterFunc(L, -3, "IsShowLogoOk", _m_IsShowLogoOk);
		Utils.RegisterFunc(L, -3, "IsKoreaRegion", _m_IsKoreaRegion);
		Utils.RegisterFunc(L, -3, "requestFCMToken", _m_requestFCMToken);
		Utils.RegisterFunc(L, -3, "CrashlyticsSetCustomValue", _m_CrashlyticsSetCustomValue);
		Utils.RegisterFunc(L, -3, "CrashlyticsAddLog", _m_CrashlyticsAddLog);
		Utils.RegisterFunc(L, -3, "CrashlyticsSetUserId", _m_CrashlyticsSetUserId);
		Utils.RegisterFunc(L, -3, "AnalyticsAccountEmail", _m_AnalyticsAccountEmail);
		Utils.RegisterFunc(L, -3, "initAppsFlyer", _m_initAppsFlyer);
		Utils.RegisterFunc(L, -3, "InitTrackingData", _m_InitTrackingData);
		Utils.RegisterFunc(L, -3, "SetDeviceIdSuperProperty", _m_SetDeviceIdSuperProperty);
		Utils.RegisterFunc(L, -3, "SetDeviceLevelSuperProperty", _m_SetDeviceLevelSuperProperty);
		Utils.RegisterFunc(L, -3, "LoginTracking", _m_LoginTracking);
		Utils.RegisterFunc(L, -3, "Restart", _m_Restart);
		Utils.RegisterFunc(L, -3, "GetRestartData", _m_GetRestartData);
		Utils.RegisterFunc(L, -3, "RecordAppsflyer", _m_RecordAppsflyer);
		Utils.RegisterFunc(L, -3, "SetAppsFlyerPurchase", _m_SetAppsFlyerPurchase);
		Utils.RegisterFunc(L, -3, "SetFacebookPurchaseEvent", _m_SetFacebookPurchaseEvent);
		Utils.RegisterFunc(L, -3, "GetAppsFlyerUid", _m_GetAppsFlyerUid);
		Utils.RegisterFunc(L, -3, "SetUserId", _m_SetUserId);
		Utils.RegisterFunc(L, -3, "OnUploadPhoto", _m_OnUploadPhoto);
		Utils.RegisterFunc(L, -3, "OnUploadPhoto_Chat", _m_OnUploadPhoto_Chat);
		Utils.RegisterFunc(L, -3, "Odm2_PostAggregateConversionInfo", _m_Odm2_PostAggregateConversionInfo);
		Utils.RegisterFunc(L, -3, "GetPlatformNameForBI", _m_GetPlatformNameForBI);
		Utils.RegisterFunc(L, -3, "IsVNPlatform", _m_IsVNPlatform);
		Utils.RegisterFunc(L, -3, "DMAPrivacyAllowed", _m_DMAPrivacyAllowed);
		Utils.RegisterFunc(L, -3, "RequestInAppReview", _m_RequestInAppReview);
		Utils.RegisterFunc(L, -3, "ReportIOSCustomDylibNames", _m_ReportIOSCustomDylibNames);
		Utils.RegisterFunc(L, -3, "GetRealtime", _m_GetRealtime);
		Utils.RegisterFunc(L, -3, "GetFormattedPrice", _m_GetFormattedPrice);
		Utils.RegisterFunc(L, -3, "AddCalendarEvent", _m_AddCalendarEvent);
		Utils.RegisterFunc(L, -2, "IPCountry", _g_get_IPCountry);
		Utils.RegisterFunc(L, -2, "RegCountry", _g_get_RegCountry);
		Utils.RegisterFunc(L, -2, "Platform", _g_get_Platform);
		Utils.RegisterFunc(L, -2, "DistinctId", _g_get_DistinctId);
		Utils.RegisterFunc(L, -2, "Version", _g_get_Version);
		Utils.RegisterFunc(L, -2, "VersionCode", _g_get_VersionCode);
		Utils.RegisterFunc(L, -2, "IsGoogleAvailable", _g_get_IsGoogleAvailable);
		Utils.RegisterFunc(L, -2, "AndroidScreenNotch", _g_get_AndroidScreenNotch);
		Utils.RegisterFunc(L, -2, "LaunchPushId", _g_get_LaunchPushId);
		Utils.RegisterFunc(L, -2, "CurrentThermalState", _g_get_CurrentThermalState);
		Utils.RegisterFunc(L, -2, "pf_displayname", _g_get_pf_displayname);
		Utils.RegisterFunc(L, -2, "alreadyStartAF", _g_get_alreadyStartAF);
		Utils.RegisterFunc(L, -2, "runtimeInfo", _g_get_runtimeInfo);
		Utils.RegisterFunc(L, -1, "IPCountry", _s_set_IPCountry);
		Utils.RegisterFunc(L, -1, "RegCountry", _s_set_RegCountry);
		Utils.RegisterFunc(L, -1, "LaunchPushId", _s_set_LaunchPushId);
		Utils.RegisterFunc(L, -1, "pf_displayname", _s_set_pf_displayname);
		Utils.RegisterFunc(L, -1, "alreadyStartAF", _s_set_alreadyStartAF);
		Utils.RegisterFunc(L, -1, "runtimeInfo", _s_set_runtimeInfo);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 20, 5, 5);
		Utils.RegisterFunc(L, -4, "IS_UNITY_ANDROID", _m_IS_UNITY_ANDROID_xlua_st_);
		Utils.RegisterFunc(L, -4, "IS_UNITY_IOS", _m_IS_UNITY_IOS_xlua_st_);
		Utils.RegisterFunc(L, -4, "IS_UNITY_IPHONE", _m_IS_UNITY_IPHONE_xlua_st_);
		Utils.RegisterFunc(L, -4, "IS_UNITY_EDITOR", _m_IS_UNITY_EDITOR_xlua_st_);
		Utils.RegisterFunc(L, -4, "IS_UNITY_STANDALONE", _m_IS_UNITY_STANDALONE_xlua_st_);
		Utils.RegisterFunc(L, -4, "IS_IPhonePlayer", _m_IS_IPhonePlayer_xlua_st_);
		Utils.RegisterFunc(L, -4, "IS_Android", _m_IS_Android_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsNativeAppsFlyerAvailable", _m_IsNativeAppsFlyerAvailable_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddBILaunchTimeProperty", _m_AddBILaunchTimeProperty_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTaPresetProp", _m_GetTaPresetProp_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTaDistinctId", _m_GetTaDistinctId_xlua_st_);
		Utils.RegisterFunc(L, -4, "SaveToAlbum", _m_SaveToAlbum_xlua_st_);
		Utils.RegisterFunc(L, -4, "SaveImageToPhotoIOS", _m_SaveImageToPhotoIOS_xlua_st_);
		Utils.RegisterFunc(L, -4, "SaveImageToPhotoAndroid", _m_SaveImageToPhotoAndroid_xlua_st_);
		Utils.RegisterFunc(L, -4, "SaveRTToAlbum", _m_SaveRTToAlbum_xlua_st_);
		Utils.RegisterFunc(L, -4, "OpenURL", _m_OpenURL_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitUWAGotOnline", _m_InitUWAGotOnline_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "writeAlbumCode", 1001);
		Utils.RegisterObject(L, translator, -4, "calendarEventCode", 1002);
		Utils.RegisterFunc(L, -2, "writeAlbumFileName", _g_get_writeAlbumFileName);
		Utils.RegisterFunc(L, -2, "writeAlbumFilePath", _g_get_writeAlbumFilePath);
		Utils.RegisterFunc(L, -2, "isUploadImageFix", _g_get_isUploadImageFix);
		Utils.RegisterFunc(L, -2, "hadCallHideSplash", _g_get_hadCallHideSplash);
		Utils.RegisterFunc(L, -2, "UWAInitialized", _g_get_UWAInitialized);
		Utils.RegisterFunc(L, -1, "writeAlbumFileName", _s_set_writeAlbumFileName);
		Utils.RegisterFunc(L, -1, "writeAlbumFilePath", _s_set_writeAlbumFilePath);
		Utils.RegisterFunc(L, -1, "isUploadImageFix", _s_set_isUploadImageFix);
		Utils.RegisterFunc(L, -1, "hadCallHideSplash", _s_set_hadCallHideSplash);
		Utils.RegisterFunc(L, -1, "UWAInitialized", _s_set_UWAInitialized);
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
				SDKManager o = new SDKManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SDKManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Initialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ApplicationWillEnterForeground(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ApplicationWillEnterForeground();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPayMangerCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SDKManager sDKManager = (SDKManager)objectTranslator.FastGetCSObj(L, 1);
			Action<string, string> payMangerCallback = objectTranslator.GetDelegate<Action<string, string>>(L, 2);
			sDKManager.SetPayMangerCallback(payMangerCallback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPublishRegion(IntPtr L)
	{
		try
		{
			string publishRegion = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPublishRegion();
			Lua.lua_pushstring(L, publishRegion);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPackageName(IntPtr L)
	{
		try
		{
			string packageName = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPackageName();
			Lua.lua_pushstring(L, packageName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPackageSign(IntPtr L)
	{
		try
		{
			string packageSign = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPackageSign();
			Lua.lua_pushstring(L, packageSign);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getChannel(IntPtr L)
	{
		try
		{
			string channel = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).getChannel();
			Lua.lua_pushstring(L, channel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_saveDataToSdcard(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			string path = Lua.lua_tostring(L, 3);
			obj.saveDataToSdcard(data, path);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestSdCardPermission(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RequestSdCardPermission();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoInitGooglePay(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoInitGooglePay();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoInitShop(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoInitShop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSimulator(IntPtr L)
	{
		try
		{
			bool value = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSimulator();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckDownloadGoogleApk(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckDownloadGoogleApk();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDeviceUDID(IntPtr L)
	{
		try
		{
			string deviceUDID = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDeviceUDID();
			Lua.lua_pushstring(L, deviceUDID);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSerialID(IntPtr L)
	{
		try
		{
			string serialID = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSerialID();
			Lua.lua_pushstring(L, serialID);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDeviceInfo(IntPtr L)
	{
		try
		{
			string deviceInfo = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDeviceInfo();
			Lua.lua_pushstring(L, deviceInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAndroidScreenNotch(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAndroidScreenNotch();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHandSetInfo(IntPtr L)
	{
		try
		{
			string handSetInfo = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHandSetInfo();
			Lua.lua_pushstring(L, handSetInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GenerateHighVersionUUID(IntPtr L)
	{
		try
		{
			string str = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GenerateHighVersionUUID();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSimOperator(IntPtr L)
	{
		try
		{
			string simOperator = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSimOperator();
			Lua.lua_pushstring(L, simOperator);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSimOperatorName(IntPtr L)
	{
		try
		{
			string simOperatorName = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSimOperatorName();
			Lua.lua_pushstring(L, simOperatorName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getClickPushTag(IntPtr L)
	{
		try
		{
			string clickPushTag = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).getClickPushTag();
			Lua.lua_pushstring(L, clickPushTag);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getClickPushId(IntPtr L)
	{
		try
		{
			string clickPushId = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).getClickPushId();
			Lua.lua_pushstring(L, clickPushId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getClickPushTime(IntPtr L)
	{
		try
		{
			string clickPushTime = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).getClickPushTime();
			Lua.lua_pushstring(L, clickPushTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllPushData(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllPushData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyTextToClipboard(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string content = Lua.lua_tostring(L, 2);
			obj.CopyTextToClipboard(content);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsNotifyOpen(IntPtr L)
	{
		try
		{
			bool isNotifyOpen = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetIsNotifyOpen();
			Lua.lua_pushboolean(L, isNotifyOpen);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AskForNotifyPermission(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AskForNotifyPermission();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTrackingEnabled(IntPtr L)
	{
		try
		{
			bool value = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsTrackingEnabled();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OpenSettings(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OpenSettings();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SDKManager sDKManager = (SDKManager)objectTranslator.FastGetCSObj(L, 1);
			string eventName = Lua.lua_tostring(L, 2);
			object[] datas = objectTranslator.GetParams<object>(L, 3);
			sDKManager.LogEvent(eventName, datas);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildInfo(IntPtr L)
	{
		try
		{
			string buildInfo = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetBuildInfo();
			Lua.lua_pushstring(L, buildInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPermissionByType(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			string permissionByType = obj.GetPermissionByType(data);
			Lua.lua_pushstring(L, permissionByType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogEventLevelUp(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lv = Lua.xlua_tointeger(L, 2);
			obj.LogEventLevelUp(lv);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Login(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SDKManager sDKManager = (SDKManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<LoginPlatform>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out LoginPlatform v);
				bool changeAccount = Lua.lua_toboolean(L, 3);
				bool isBind = Lua.lua_toboolean(L, 4);
				sDKManager.Login(v, changeAccount, isBind);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<LoginPlatform>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out LoginPlatform v2);
				bool changeAccount2 = Lua.lua_toboolean(L, 3);
				sDKManager.Login(v2, changeAccount2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<LoginPlatform>(L, 2))
			{
				objectTranslator.Get(L, 2, out LoginPlatform v3);
				sDKManager.Login(v3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SDKManager.Login!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAccountFunc(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SDKManager sDKManager = (SDKManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LoginPlatform v);
			int accountFuncType = Lua.xlua_tointeger(L, 3);
			sDKManager.SetAccountFunc(v, accountFuncType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Logout(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Logout();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pay(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			obj.Pay(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckPayEnv(IntPtr L)
	{
		try
		{
			int value = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckPayEnv();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_PF_DisplayName(IntPtr L)
	{
		try
		{
			string pF_DisplayName = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_PF_DisplayName();
			Lua.lua_pushstring(L, pF_DisplayName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConsumeProduct(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string orderId = Lua.lua_tostring(L, 2);
			int state = Lua.xlua_tointeger(L, 3);
			obj.ConsumeProduct(orderId, state);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GotoMarket(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string url = Lua.lua_tostring(L, 2);
			string urlCDN = Lua.lua_tostring(L, 3);
			obj.GotoMarket(url, urlCDN);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendDataToGame(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string funcName = Lua.lua_tostring(L, 2);
			string data = Lua.lua_tostring(L, 3);
			obj.SendDataToGame(funcName, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float elapseSeconds = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(elapseSeconds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleEvent(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string funcName = Lua.lua_tostring(L, 2);
			string data = Lua.lua_tostring(L, 3);
			obj.HandleEvent(funcName, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendDataToNative(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string funcName = Lua.lua_tostring(L, 2);
			string data = Lua.lua_tostring(L, 3);
			obj.SendDataToNative(funcName, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDataFromNative(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string funcName = Lua.lua_tostring(L, 2);
			string data = Lua.lua_tostring(L, 3);
			string dataFromNative = obj.GetDataFromNative(funcName, data);
			Lua.lua_pushstring(L, dataFromNative);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideSplash(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideSplash();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsShowLogoOk(IntPtr L)
	{
		try
		{
			bool value = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsShowLogoOk();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_UNITY_ANDROID_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_UNITY_ANDROID();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_UNITY_IOS_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_UNITY_IOS();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_UNITY_IPHONE_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_UNITY_IPHONE();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_UNITY_EDITOR_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_UNITY_EDITOR();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_UNITY_STANDALONE_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_UNITY_STANDALONE();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_IPhonePlayer_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_IPhonePlayer();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IS_Android_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IS_Android();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsKoreaRegion(IntPtr L)
	{
		try
		{
			bool value = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsKoreaRegion();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_requestFCMToken(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).requestFCMToken();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrashlyticsSetCustomValue(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.CrashlyticsSetCustomValue(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrashlyticsAddLog(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string log = Lua.lua_tostring(L, 2);
			obj.CrashlyticsAddLog(log);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrashlyticsSetUserId(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string userId = Lua.lua_tostring(L, 2);
			obj.CrashlyticsSetUserId(userId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AnalyticsAccountEmail(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string accountEmail = Lua.lua_tostring(L, 2);
			obj.AnalyticsAccountEmail(accountEmail);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_initAppsFlyer(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string gameUid = Lua.lua_tostring(L, 2);
			obj.initAppsFlyer(gameUid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitTrackingData(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string discard = Lua.lua_tostring(L, 2);
			obj.InitTrackingData(discard);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDeviceIdSuperProperty(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetDeviceIdSuperProperty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDeviceLevelSuperProperty(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetDeviceLevelSuperProperty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoginTracking(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string gameUid = Lua.lua_tostring(L, 2);
			obj.LoginTracking(gameUid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Restart(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string restartData = Lua.lua_tostring(L, 2);
			obj.Restart(restartData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRestartData(IntPtr L)
	{
		try
		{
			string restartData = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRestartData();
			Lua.lua_pushstring(L, restartData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNativeAppsFlyerAvailable_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SDKManager.IsNativeAppsFlyerAvailable();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddBILaunchTimeProperty_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Dictionary<string, object>>(L, 1))
			{
				Dictionary<string, object> o = SDKManager.AddBILaunchTimeProperty((Dictionary<string, object>)objectTranslator.GetObject(L, 1, typeof(Dictionary<string, object>)));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 0)
			{
				Dictionary<string, object> o2 = SDKManager.AddBILaunchTimeProperty();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SDKManager.AddBILaunchTimeProperty!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTaPresetProp_xlua_st_(IntPtr L)
	{
		try
		{
			string taPresetProp = SDKManager.GetTaPresetProp();
			Lua.lua_pushstring(L, taPresetProp);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTaDistinctId_xlua_st_(IntPtr L)
	{
		try
		{
			string taDistinctId = SDKManager.GetTaDistinctId();
			Lua.lua_pushstring(L, taDistinctId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordAppsflyer(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			obj.RecordAppsflyer(key);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAppsFlyerPurchase(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string cost = Lua.lua_tostring(L, 2);
			string itemId = Lua.lua_tostring(L, 3);
			obj.SetAppsFlyerPurchase(cost, itemId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFacebookPurchaseEvent(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string cost = Lua.lua_tostring(L, 2);
			string itemId = Lua.lua_tostring(L, 3);
			obj.SetFacebookPurchaseEvent(cost, itemId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAppsFlyerUid(IntPtr L)
	{
		try
		{
			string appsFlyerUid = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAppsFlyerUid();
			Lua.lua_pushstring(L, appsFlyerUid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUserId(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string userId = Lua.lua_tostring(L, 2);
			obj.SetUserId(userId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUploadPhoto(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uid = Lua.lua_tostring(L, 2);
			int code = Lua.xlua_tointeger(L, 3);
			int idx = Lua.xlua_tointeger(L, 4);
			obj.OnUploadPhoto(uid, code, idx);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUploadPhoto_Chat(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int code = Lua.xlua_tointeger(L, 2);
			int photoResolutionLimit = Lua.xlua_tointeger(L, 3);
			int photoFileSizeLimit = Lua.xlua_tointeger(L, 4);
			int suitableResolutionSizeBig = Lua.xlua_tointeger(L, 5);
			int suitableResolutionSizeSmall = Lua.xlua_tointeger(L, 6);
			obj.OnUploadPhoto_Chat(code, photoResolutionLimit, photoFileSizeLimit, suitableResolutionSizeBig, suitableResolutionSizeSmall);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Odm2_PostAggregateConversionInfo(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Odm2_PostAggregateConversionInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPlatformNameForBI(IntPtr L)
	{
		try
		{
			string platformNameForBI = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPlatformNameForBI();
			Lua.lua_pushstring(L, platformNameForBI);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsVNPlatform(IntPtr L)
	{
		try
		{
			bool value = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsVNPlatform();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DMAPrivacyAllowed(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DMAPrivacyAllowed();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestInAppReview(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RequestInAppReview();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReportIOSCustomDylibNames(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReportIOSCustomDylibNames();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRealtime(IntPtr L)
	{
		try
		{
			long realtime = ((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRealtime();
			Lua.lua_pushint64(L, realtime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFormattedPrice(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			double price = Lua.lua_tonumber(L, 2);
			obj.GetFormattedPrice(price);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveToAlbum_xlua_st_(IntPtr L)
	{
		try
		{
			string filePath = Lua.lua_tostring(L, 1);
			string fileName = Lua.lua_tostring(L, 2);
			SDKManager.SaveToAlbum(filePath, fileName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCalendarEvent(IntPtr L)
	{
		try
		{
			SDKManager obj = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string title = Lua.lua_tostring(L, 2);
			string location = Lua.lua_tostring(L, 3);
			long startTime = Lua.lua_toint64(L, 4);
			long endTime = Lua.lua_toint64(L, 5);
			int haveAlarm = Lua.xlua_tointeger(L, 6);
			int alarmTime = Lua.xlua_tointeger(L, 7);
			string calendarItemExternalIdentifier = Lua.lua_tostring(L, 8);
			int cfgId = Lua.xlua_tointeger(L, 9);
			string sourcePath = Lua.lua_tostring(L, 10);
			obj.AddCalendarEvent(title, location, startTime, endTime, haveAlarm, alarmTime, calendarItemExternalIdentifier, cfgId, sourcePath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveImageToPhotoIOS_xlua_st_(IntPtr L)
	{
		try
		{
			SDKManager.SaveImageToPhotoIOS(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveImageToPhotoAndroid_xlua_st_(IntPtr L)
	{
		try
		{
			string filePath = Lua.lua_tostring(L, 1);
			string fileName = Lua.lua_tostring(L, 2);
			SDKManager.SaveImageToPhotoAndroid(filePath, fileName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveRTToAlbum_xlua_st_(IntPtr L)
	{
		try
		{
			SDKManager.SaveRTToAlbum((RenderTexture)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RenderTexture)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OpenURL_xlua_st_(IntPtr L)
	{
		try
		{
			SDKManager.OpenURL(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitUWAGotOnline_xlua_st_(IntPtr L)
	{
		try
		{
			SDKManager.InitUWAGotOnline();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IPCountry(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.IPCountry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RegCountry(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.RegCountry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Platform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SDKManager sDKManager = (SDKManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, sDKManager.Platform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DistinctId(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.DistinctId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Version(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.Version);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VersionCode(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.VersionCode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsGoogleAvailable(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, sDKManager.IsGoogleAvailable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AndroidScreenNotch(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.AndroidScreenNotch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LaunchPushId(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.LaunchPushId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentThermalState(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sDKManager.CurrentThermalState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pf_displayname(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sDKManager.pf_displayname);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alreadyStartAF(IntPtr L)
	{
		try
		{
			SDKManager sDKManager = (SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, sDKManager.alreadyStartAF);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_runtimeInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SDKManager sDKManager = (SDKManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sDKManager.runtimeInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_writeAlbumFileName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SDKManager.writeAlbumFileName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_writeAlbumFilePath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SDKManager.writeAlbumFilePath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isUploadImageFix(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SDKManager.isUploadImageFix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hadCallHideSplash(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SDKManager.hadCallHideSplash);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UWAInitialized(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SDKManager.UWAInitialized);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IPCountry(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IPCountry = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RegCountry(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RegCountry = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LaunchPushId(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LaunchPushId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pf_displayname(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pf_displayname = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alreadyStartAF(IntPtr L)
	{
		try
		{
			((SDKManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alreadyStartAF = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_runtimeInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SDKManager)objectTranslator.FastGetCSObj(L, 1)).runtimeInfo = (RuntimeInfoManager)objectTranslator.GetObject(L, 2, typeof(RuntimeInfoManager));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_writeAlbumFileName(IntPtr L)
	{
		try
		{
			SDKManager.writeAlbumFileName = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_writeAlbumFilePath(IntPtr L)
	{
		try
		{
			SDKManager.writeAlbumFilePath = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isUploadImageFix(IntPtr L)
	{
		try
		{
			SDKManager.isUploadImageFix = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hadCallHideSplash(IntPtr L)
	{
		try
		{
			SDKManager.hadCallHideSplash = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UWAInitialized(IntPtr L)
	{
		try
		{
			SDKManager.UWAInitialized = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
