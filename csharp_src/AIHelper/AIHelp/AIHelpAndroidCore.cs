using System;
using UnityEngine;

namespace AIHelp;

public class AIHelpAndroidCore : IAIHelpCore
{
	private class ListenerAdapter : AndroidJavaProxy
	{
		private readonly AIHelpDefine.OnAIHelpInitializedCallback initCallback;

		private readonly AIHelpDefine.OnMessageCountArrivedCallback msgCountCallback;

		private readonly AIHelpDefine.OnNetworkCheckResultCallback netCheckCallback;

		private readonly AIHelpDefine.OnSpecificFormSubmittedCallback submittedCallback;

		private readonly AIHelpDefine.OnAIHelpSessionOpenCallback sessionOpenCallback;

		private readonly AIHelpDefine.OnAIHelpSessionCloseCallback sessionCloseCallback;

		private readonly AIHelpDefine.OnSpecificUrlClickedCallback urlClickedCallback;

		public ListenerAdapter(AIHelpDefine.OnAIHelpInitializedCallback callback)
			: base("net.aihelp.ui.listener.OnAIHelpInitializedCallback")
		{
			initCallback = callback;
		}

		public ListenerAdapter(AIHelpDefine.OnMessageCountArrivedCallback callback)
			: base("net.aihelp.ui.listener.OnMessageCountArrivedCallback")
		{
			msgCountCallback = callback;
		}

		public ListenerAdapter(AIHelpDefine.OnNetworkCheckResultCallback callback)
			: base("net.aihelp.ui.listener.OnNetworkCheckResultCallback")
		{
			netCheckCallback = callback;
		}

		public ListenerAdapter(AIHelpDefine.OnSpecificFormSubmittedCallback callback)
			: base("net.aihelp.ui.listener.OnSpecificFormSubmittedCallback")
		{
			submittedCallback = callback;
		}

		public ListenerAdapter(AIHelpDefine.OnAIHelpSessionOpenCallback callback)
			: base("net.aihelp.ui.listener.OnAIHelpSessionOpenCallback")
		{
			sessionOpenCallback = callback;
		}

		public ListenerAdapter(AIHelpDefine.OnAIHelpSessionCloseCallback callback)
			: base("net.aihelp.ui.listener.OnAIHelpSessionCloseCallback")
		{
			sessionCloseCallback = callback;
		}

		public ListenerAdapter(AIHelpDefine.OnSpecificUrlClickedCallback callback)
			: base("net.aihelp.ui.listener.OnSpecificUrlClickedCallback")
		{
			urlClickedCallback = callback;
		}

		private void onAIHelpInitialized(bool isSuccess, string message)
		{
			initCallback(isSuccess, message);
		}

		private void onMessageCountArrived(int msgCount)
		{
			msgCountCallback(msgCount);
		}

		private void onNetworkCheckResult(string netLog)
		{
			netCheckCallback(netLog);
		}

		private void onFormSubmitted()
		{
			submittedCallback();
		}

		private void onAIHelpSessionOpened()
		{
			sessionOpenCallback();
		}

		private void onAIHelpSessionClosed()
		{
			sessionCloseCallback();
		}

		private void onSpecificUrlClicked(string url)
		{
			urlClickedCallback(url);
		}
	}

	private AndroidJavaClass javaSupport;

	private AndroidJavaObject currentActivity;

	public AIHelpAndroidCore()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		javaSupport = new AndroidJavaClass("net.aihelp.init.AIHelpSupport");
		currentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public void Init(string appKey, string domain, string appId, string language)
	{
		if (javaSupport != null && currentActivity != null)
		{
			javaSupport.CallStatic("init", currentActivity, appKey, domain, appId, language);
		}
	}

	private AndroidJavaObject getApiConfig(ApiConfig config)
	{
		return new AndroidJavaObject("net.aihelp.config.ApiConfig$Builder").Call<AndroidJavaObject>("build", new object[2]
		{
			config.GetEntranceId(),
			config.GetWelcomeMessage()
		});
	}

	private AndroidJavaObject getUserConfig(UserConfig config)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("net.aihelp.config.UserConfig$Builder");
		androidJavaObject.Call<AndroidJavaObject>("setUserId", new object[1] { config.GetUserId() });
		androidJavaObject.Call<AndroidJavaObject>("setUserName", new object[1] { config.GetUserName() });
		androidJavaObject.Call<AndroidJavaObject>("setServerId", new object[1] { config.GetServerId() });
		androidJavaObject.Call<AndroidJavaObject>("setUserTags", new object[1] { config.GetUserTags() });
		androidJavaObject.Call<AndroidJavaObject>("setCustomData", new object[1] { config.GetCustomData() });
		androidJavaObject.Call<AndroidJavaObject>("setSyncCrmInfo", new object[1] { config.GetWhetherSyncCrmInfo() });
		return androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>());
	}

	private AndroidJavaObject getPublishCountryOrRegion(PublishCountryOrRegion countryOrRegion)
	{
		return new AndroidJavaClass("net.aihelp.config.enums.PublishCountryOrRegion").CallStatic<AndroidJavaObject>("fromValue", new object[1] { (int)countryOrRegion });
	}

	private AndroidJavaObject getPushPlatform(PushPlatform platform)
	{
		return new AndroidJavaClass("net.aihelp.config.enums.PushPlatform").CallStatic<AndroidJavaObject>("fromValue", new object[1] { (int)platform });
	}

	private AndroidJavaObject getShowConversationMoment(ConversationMoment conversationMoment)
	{
		return new AndroidJavaClass("net.aihelp.config.enums.ShowConversationMoment").CallStatic<AndroidJavaObject>("fromValue", new object[1] { (int)conversationMoment });
	}

	public bool Show(string entranceId)
	{
		if (javaSupport != null && currentActivity != null)
		{
			return javaSupport.CallStatic<bool>("show", new object[1] { entranceId });
		}
		return false;
	}

	public bool Show(ApiConfig apiConfig)
	{
		if (javaSupport != null && currentActivity != null)
		{
			return javaSupport.CallStatic<bool>("show", new object[1] { getApiConfig(apiConfig) });
		}
		return false;
	}

	public void ShowSingleFAQ(string faqId, ConversationMoment conversationMoment)
	{
		if (javaSupport != null && currentActivity != null)
		{
			javaSupport.CallStatic("showSingleFAQ", faqId, getShowConversationMoment(conversationMoment));
		}
	}

	public void UpdateUserInfo(UserConfig config)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("updateUserInfo", getUserConfig(config));
		}
	}

	public void ResetUserInfo()
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("resetUserInfo");
		}
	}

	public void UpdateSDKLanguage(string language)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("updateSDKLanguage", language);
		}
	}

	public void SetUploadLogPath(string logPath)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("setUploadLogPath", logPath);
		}
	}

	public void SetPushTokenAndPlatform(string logPath, PushPlatform pushPlatform)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("setPushTokenAndPlatform", logPath, getPushPlatform(pushPlatform));
		}
	}

	public void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback listener)
	{
		javaSupport.CallStatic("setOnAIHelpInitializedCallback", (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback listener)
	{
		javaSupport.CallStatic("setNetworkCheckHostAddress", address, (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback listener)
	{
		javaSupport.CallStatic("startUnreadMessageCountPolling", (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback listener)
	{
		javaSupport.CallStatic("setOnSpecificFormSubmittedCallback", (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback listener)
	{
		javaSupport.CallStatic("setOnAIHelpSessionOpenCallback", (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback listener)
	{
		javaSupport.CallStatic("setOnAIHelpSessionCloseCallback", (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void SetOnSpecificUrlClickedCallback(AIHelpDefine.OnSpecificUrlClickedCallback listener)
	{
		javaSupport.CallStatic("setOnSpecificUrlClickedCallback", (listener == null) ? null : new ListenerAdapter(listener));
	}

	public void ShowUrl(string url)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("showUrl", url);
		}
	}

	public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("additionalSupportFor", getPublishCountryOrRegion(countryOrRegion));
		}
	}

	public string GetSDKVersion()
	{
		if (javaSupport != null)
		{
			return javaSupport.CallStatic<string>("getSDKVersion", Array.Empty<object>());
		}
		return "";
	}

	public bool IsAIHelpShowing()
	{
		if (javaSupport != null)
		{
			return javaSupport.CallStatic<bool>("isAIHelpShowing", Array.Empty<object>());
		}
		return false;
	}

	public void EnableLogging(bool isOpen)
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("enableLogging", isOpen);
		}
	}

	public void Close()
	{
		if (javaSupport != null)
		{
			javaSupport.CallStatic("close");
		}
	}
}
