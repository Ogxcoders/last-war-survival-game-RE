using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppsFlyerSDK;

public class AppsFlyerAndroid
{
	private static AndroidJavaClass appsFlyerAndroid = new AndroidJavaClass("com.appsflyer.unity.AppsFlyerAndroidWrapper");

	public static void initSDK(string devkey, MonoBehaviour gameObject)
	{
		appsFlyerAndroid.CallStatic("initSDK", devkey, gameObject ? gameObject.name : null);
	}

	public static void startSDK()
	{
		startSDK(shouldCallback: false, AppsFlyer.CallBackObjectName);
	}

	public static void startSDK(bool shouldCallback, string callBackObjectName)
	{
		appsFlyerAndroid.CallStatic("startTracking", shouldCallback, callBackObjectName);
	}

	public static void stopSDK(bool isSDKStopped)
	{
		appsFlyerAndroid.CallStatic("stopTracking", isSDKStopped);
	}

	public static string getSdkVersion()
	{
		return appsFlyerAndroid.CallStatic<string>("getSdkVersion", Array.Empty<object>());
	}

	public static void updateServerUninstallToken(string token)
	{
		appsFlyerAndroid.CallStatic("updateServerUninstallToken", token);
	}

	public static void setIsDebug(bool shouldEnable)
	{
		appsFlyerAndroid.CallStatic("setIsDebug", shouldEnable);
	}

	public static void setImeiData(string aImei)
	{
		appsFlyerAndroid.CallStatic("setImeiData", aImei);
	}

	public static void setAndroidIdData(string aAndroidId)
	{
		appsFlyerAndroid.CallStatic("setAndroidIdData", aAndroidId);
	}

	public static void setCustomerUserId(string id)
	{
		appsFlyerAndroid.CallStatic("setCustomerUserId", id);
	}

	public static void waitForCustomerUserId(bool wait)
	{
		appsFlyerAndroid.CallStatic("waitForCustomerUserId", wait);
	}

	public static void setCustomerIdAndStartSDK(string id)
	{
		appsFlyerAndroid.CallStatic("setCustomerIdAndTrack", id);
	}

	public static string getOutOfStore()
	{
		return appsFlyerAndroid.CallStatic<string>("getOutOfStore", Array.Empty<object>());
	}

	public static void setOutOfStore(string sourceName)
	{
		appsFlyerAndroid.CallStatic("setOutOfStore", sourceName);
	}

	public static void setAppInviteOneLinkID(string oneLinkId)
	{
		appsFlyerAndroid.CallStatic("setAppInviteOneLinkID", oneLinkId);
	}

	public static void setAdditionalData(Dictionary<string, string> customData)
	{
		appsFlyerAndroid.CallStatic("setAdditionalData", convertDictionaryToJavaMap(customData));
	}

	public static void setUserEmails(params string[] emails)
	{
		appsFlyerAndroid.CallStatic("setUserEmails", new object[1] { emails });
	}

	public static void setPhoneNumber(string phoneNumber)
	{
		appsFlyerAndroid.CallStatic("setPhoneNumber", phoneNumber);
	}

	public static void setUserEmails(EmailCryptType cryptMethod, params string[] emails)
	{
		appsFlyerAndroid.CallStatic("setUserEmails", getEmailType(cryptMethod), emails);
	}

	public static void setCollectAndroidID(bool isCollect)
	{
		appsFlyerAndroid.CallStatic("setCollectAndroidID", isCollect);
	}

	public static void setCollectIMEI(bool isCollect)
	{
		appsFlyerAndroid.CallStatic("setCollectIMEI", isCollect);
	}

	public static void setResolveDeepLinkURLs(params string[] urls)
	{
		appsFlyerAndroid.CallStatic("setResolveDeepLinkURLs", new object[1] { urls });
	}

	public static void setOneLinkCustomDomain(params string[] domains)
	{
		appsFlyerAndroid.CallStatic("setOneLinkCustomDomain", new object[1] { domains });
	}

	public static void setIsUpdate(bool isUpdate)
	{
		appsFlyerAndroid.CallStatic("setIsUpdate", isUpdate);
	}

	public static void setCurrencyCode(string currencyCode)
	{
		appsFlyerAndroid.CallStatic("setCurrencyCode", currencyCode);
	}

	public static void recordLocation(double latitude, double longitude)
	{
		appsFlyerAndroid.CallStatic("trackLocation", latitude, longitude);
	}

	public static void sendEvent(string eventName, Dictionary<string, string> eventValues)
	{
		sendEvent(eventName, eventValues, shouldCallback: false, AppsFlyer.CallBackObjectName);
	}

	public static void sendEvent(string eventName, Dictionary<string, string> eventValues, bool shouldCallback, string callBackObjectName)
	{
		appsFlyerAndroid.CallStatic("trackEvent", eventName, convertDictionaryToJavaMap(eventValues), shouldCallback, callBackObjectName);
	}

	public static void anonymizeUser(bool isDisabled)
	{
		appsFlyerAndroid.CallStatic("setDeviceTrackingDisabled", isDisabled);
	}

	public static void enableFacebookDeferredApplinks(bool isEnabled)
	{
		appsFlyerAndroid.CallStatic("enableFacebookDeferredApplinks", isEnabled);
	}

	public static void setConsumeAFDeepLinks(bool doConsume)
	{
		appsFlyerAndroid.CallStatic("setConsumeAFDeepLinks", doConsume);
	}

	public static void setPreinstallAttribution(string mediaSource, string campaign, string siteId)
	{
		appsFlyerAndroid.CallStatic("setPreinstallAttribution", mediaSource, campaign, siteId);
	}

	public static bool isPreInstalledApp()
	{
		return appsFlyerAndroid.CallStatic<bool>("isPreInstalledApp", Array.Empty<object>());
	}

	public static string getAttributionId()
	{
		return appsFlyerAndroid.CallStatic<string>("getAttributionId", Array.Empty<object>());
	}

	public static string getAppsFlyerId()
	{
		return appsFlyerAndroid.CallStatic<string>("getAppsFlyerId", Array.Empty<object>());
	}

	public static void validateAndSendInAppPurchase(string publicKey, string signature, string purchaseData, string price, string currency, Dictionary<string, string> additionalParameters, MonoBehaviour gameObject)
	{
		appsFlyerAndroid.CallStatic("validateAndTrackInAppPurchase", publicKey, signature, purchaseData, price, currency, convertDictionaryToJavaMap(additionalParameters), gameObject ? gameObject.name : null);
	}

	public static bool isSDKStopped()
	{
		return appsFlyerAndroid.CallStatic<bool>("isTrackingStopped", Array.Empty<object>());
	}

	public static void setMinTimeBetweenSessions(int seconds)
	{
		appsFlyerAndroid.CallStatic("setMinTimeBetweenSessions", seconds);
	}

	public static void setHost(string hostPrefixName, string hostName)
	{
		appsFlyerAndroid.CallStatic("setHost", hostPrefixName, hostName);
	}

	public static string getHostName()
	{
		return appsFlyerAndroid.CallStatic<string>("getHostName", Array.Empty<object>());
	}

	public static string getHostPrefix()
	{
		return appsFlyerAndroid.CallStatic<string>("getHostPrefix", Array.Empty<object>());
	}

	public static void setSharingFilterForAllPartners()
	{
		appsFlyerAndroid.CallStatic("setSharingFilterForAllPartners");
	}

	public static void setSharingFilter(params string[] partners)
	{
		appsFlyerAndroid.CallStatic("setSharingFilter", new object[1] { partners });
	}

	public static void setSharingFilterForPartners(params string[] partners)
	{
		appsFlyerAndroid.CallStatic("setSharingFilterForPartners", new object[1] { partners });
	}

	public static void getConversionData(string objectName)
	{
		appsFlyerAndroid.CallStatic("getConversionData", objectName);
	}

	public static void initInAppPurchaseValidatorListener(MonoBehaviour gameObject)
	{
		appsFlyerAndroid.CallStatic("initInAppPurchaseValidatorListener", gameObject ? gameObject.name : null);
	}

	public static void setCollectOaid(bool isCollect)
	{
		appsFlyerAndroid.CallStatic("setCollectOaid", isCollect);
	}

	public static void attributeAndOpenStore(string promoted_app_id, string campaign, Dictionary<string, string> userParams)
	{
		appsFlyerAndroid.CallStatic("attributeAndOpenStore", promoted_app_id, campaign, convertDictionaryToJavaMap(userParams));
	}

	public static void recordCrossPromoteImpression(string appID, string campaign, Dictionary<string, string> parameters)
	{
		appsFlyerAndroid.CallStatic("recordCrossPromoteImpression", appID, campaign, convertDictionaryToJavaMap(parameters));
	}

	public static void generateUserInviteLink(Dictionary<string, string> parameters, MonoBehaviour gameObject)
	{
		appsFlyerAndroid.CallStatic("createOneLinkInviteListener", convertDictionaryToJavaMap(parameters), gameObject ? gameObject.name : null);
	}

	public static void handlePushNotifications()
	{
		appsFlyerAndroid.CallStatic("handlePushNotifications");
	}

	public static void addPushNotificationDeepLinkPath(params string[] paths)
	{
		appsFlyerAndroid.CallStatic("addPushNotificationDeepLinkPath", new object[1] { paths });
	}

	public static void subscribeForDeepLink(string objectName)
	{
		appsFlyerAndroid.CallStatic("subscribeForDeepLink", objectName);
	}

	public static void setDisableAdvertisingIdentifiers(bool disable)
	{
		appsFlyerAndroid.CallStatic("setDisableAdvertisingIdentifiers", disable);
	}

	private static AndroidJavaObject getEmailType(EmailCryptType cryptType)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.appsflyer.AppsFlyerProperties$EmailsCryptType");
		if (cryptType == EmailCryptType.EmailCryptTypeSHA256)
		{
			return androidJavaClass.GetStatic<AndroidJavaObject>("SHA256");
		}
		return androidJavaClass.GetStatic<AndroidJavaObject>("NONE");
	}

	private static AndroidJavaObject convertDictionaryToJavaMap(Dictionary<string, string> dictionary)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		if (dictionary != null)
		{
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(new object[2] { item.Key, item.Value });
				AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, array);
				AndroidJNI.DeleteLocalRef(array[0].l);
				AndroidJNI.DeleteLocalRef(array[1].l);
			}
		}
		return androidJavaObject;
	}
}
