using System;
using System.Collections.Generic;
using AFMiniJSON;
using UnityEngine;

namespace AppsFlyerSDK;

public class AppsFlyer : MonoBehaviour
{
	public static readonly string kAppsFlyerPluginVersion = "6.4.0";

	public static string CallBackObjectName = null;

	private static EventHandler onRequestResponse;

	private static EventHandler onInAppResponse;

	private static EventHandler onDeepLinkReceived;

	public static event EventHandler OnRequestResponse
	{
		add
		{
			onRequestResponse = (EventHandler)Delegate.Combine(onRequestResponse, value);
		}
		remove
		{
			onRequestResponse = (EventHandler)Delegate.Remove(onRequestResponse, value);
		}
	}

	public static event EventHandler OnInAppResponse
	{
		add
		{
			onInAppResponse = (EventHandler)Delegate.Combine(onInAppResponse, value);
		}
		remove
		{
			onInAppResponse = (EventHandler)Delegate.Remove(onInAppResponse, value);
		}
	}

	public static event EventHandler OnDeepLinkReceived
	{
		add
		{
			onDeepLinkReceived = (EventHandler)Delegate.Combine(onDeepLinkReceived, value);
			subscribeForDeepLink();
		}
		remove
		{
			onDeepLinkReceived = (EventHandler)Delegate.Remove(onDeepLinkReceived, value);
		}
	}

	public static void initSDK(string devKey, string appID)
	{
	}

	public static void initSDK(string devKey, string appID, MonoBehaviour gameObject)
	{
	}

	public static void startSDK()
	{
	}

	public static void sendEvent(string eventName, Dictionary<string, string> eventValues)
	{
	}

	public static void stopSDK(bool isSDKStopped)
	{
	}

	public static bool isSDKStopped()
	{
		return false;
	}

	public static string getSdkVersion()
	{
		return "";
	}

	public static void setIsDebug(bool shouldEnable)
	{
	}

	public static void setCustomerUserId(string id)
	{
	}

	public static void setAppInviteOneLinkID(string oneLinkId)
	{
	}

	public static void setAdditionalData(Dictionary<string, string> customData)
	{
	}

	public static void setResolveDeepLinkURLs(params string[] urls)
	{
	}

	public static void setOneLinkCustomDomain(params string[] domains)
	{
	}

	public static void setCurrencyCode(string currencyCode)
	{
	}

	public static void recordLocation(double latitude, double longitude)
	{
	}

	public static void anonymizeUser(bool shouldAnonymizeUser)
	{
	}

	public static string getAppsFlyerId()
	{
		return "";
	}

	public static void setMinTimeBetweenSessions(int seconds)
	{
	}

	public static void setHost(string hostPrefixName, string hostName)
	{
	}

	public static void setUserEmails(EmailCryptType cryptMethod, params string[] emails)
	{
	}

	public static void setPhoneNumber(string phoneNumber)
	{
	}

	[Obsolete("Please use setSharingFilterForPartners api")]
	public static void setSharingFilterForAllPartners()
	{
	}

	[Obsolete("Please use setSharingFilterForPartners api")]
	public static void setSharingFilter(params string[] partners)
	{
	}

	public static void setSharingFilterForPartners(params string[] partners)
	{
	}

	public static void getConversionData(string objectName)
	{
	}

	public static void attributeAndOpenStore(string appID, string campaign, Dictionary<string, string> userParams, MonoBehaviour gameObject)
	{
	}

	public static void recordCrossPromoteImpression(string appID, string campaign, Dictionary<string, string> parameters)
	{
	}

	public static void generateUserInviteLink(Dictionary<string, string> parameters, MonoBehaviour gameObject)
	{
	}

	public static void addPushNotificationDeepLinkPath(params string[] paths)
	{
	}

	public static void subscribeForDeepLink()
	{
	}

	public void inAppResponseReceived(string response)
	{
		if (onInAppResponse != null)
		{
			onInAppResponse(null, parseRequestCallback(response));
		}
	}

	public void requestResponseReceived(string response)
	{
		if (onRequestResponse != null)
		{
			onRequestResponse(null, parseRequestCallback(response));
		}
	}

	public void onDeepLinking(string response)
	{
		DeepLinkEventsArgs e = new DeepLinkEventsArgs(response);
		if (onDeepLinkReceived != null)
		{
			onDeepLinkReceived(null, e);
		}
	}

	private static AppsFlyerRequestEventArgs parseRequestCallback(string response)
	{
		int code = 0;
		string description = "";
		try
		{
			Dictionary<string, object> dictionary = CallbackStringToDictionary(response);
			description = (string)(dictionary.ContainsKey("errorDescription") ? dictionary["errorDescription"] : "");
			code = (int)(long)dictionary["statusCode"];
		}
		catch (Exception arg)
		{
			AFLog("parseRequestCallback", $"{arg} Exception caught.");
		}
		return new AppsFlyerRequestEventArgs(code, description);
	}

	public static Dictionary<string, object> CallbackStringToDictionary(string str)
	{
		return Json.Deserialize(str) as Dictionary<string, object>;
	}

	public static void AFLog(string methodName, string str)
	{
		Debug.Log($"AppsFlyer_Unity_v{kAppsFlyerPluginVersion} {methodName} called with {str}");
	}
}
