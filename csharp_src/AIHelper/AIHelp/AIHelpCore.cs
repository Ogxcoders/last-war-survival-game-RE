using UnityEngine;

namespace AIHelp;

public class AIHelpCore
{
	private IAIHelpCore helpCore;

	private static AIHelpCore sInstance;

	private AIHelpCore()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			helpCore = new AIHelpAndroidCore();
		}
	}

	public static AIHelpCore getInstance()
	{
		if (sInstance == null)
		{
			sInstance = new AIHelpCore();
		}
		return sInstance;
	}

	public void Init(string appKey, string domain, string appId, string language)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.Init(appKey, domain, appId, language);
		}
	}

	public void Init(string appKey, string domain, string appId)
	{
		Init(appKey, domain, appId, "");
	}

	public void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetOnAIHelpInitializedCallback(callback);
		}
	}

	public bool Show(string entranceId)
	{
		if (!IsHelpCorePrepared())
		{
			return false;
		}
		return helpCore.Show(entranceId);
	}

	public bool Show(ApiConfig apiConfig)
	{
		if (!IsHelpCorePrepared())
		{
			return false;
		}
		return helpCore.Show(apiConfig);
	}

	public void ShowSingleFAQ(string faqId, ConversationMoment moment)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.ShowSingleFAQ(faqId, moment);
		}
	}

	public void UpdateUserInfo(UserConfig userConfig)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.UpdateUserInfo(userConfig);
		}
	}

	public void ResetUserInfo()
	{
		if (IsHelpCorePrepared())
		{
			helpCore.ResetUserInfo();
		}
	}

	public void UpdateSDKLanguage(string language)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.UpdateSDKLanguage(language);
		}
	}

	public void SetUploadLogPath(string path)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetUploadLogPath(path);
		}
	}

	public void SetPushTokenAndPlatform(string pushToken, PushPlatform platform)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetPushTokenAndPlatform(pushToken, platform);
		}
	}

	public void SetNetworkCheckHostAddress(string address)
	{
		SetNetworkCheckHostAddress(address, null);
	}

	public void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetNetworkCheckHostAddress(address, callback);
		}
	}

	public void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.StartUnreadMessageCountPolling(callback);
		}
	}

	public string GetSDKVersion()
	{
		if (!IsHelpCorePrepared())
		{
			return "";
		}
		return helpCore.GetSDKVersion();
	}

	public bool IsAIHelpShowing()
	{
		if (!IsHelpCorePrepared())
		{
			return false;
		}
		return helpCore.IsAIHelpShowing();
	}

	public void EnableLogging(bool enable)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.EnableLogging(enable);
		}
	}

	public void ShowUrl(string url)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.ShowUrl(url);
		}
	}

	public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.AdditionalSupportFor(countryOrRegion);
		}
	}

	public void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetOnSpecificFormSubmittedCallback(callback);
		}
	}

	public void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetOnAIHelpSessionOpenCallback(callback);
		}
	}

	public void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetOnAIHelpSessionCloseCallback(callback);
		}
	}

	public void SetOnSpecificUrlClickedCallback(AIHelpDefine.OnSpecificUrlClickedCallback callback)
	{
		if (IsHelpCorePrepared())
		{
			helpCore.SetOnSpecificUrlClickedCallback(callback);
		}
	}

	public void Close()
	{
		if (IsHelpCorePrepared())
		{
			helpCore.Close();
		}
	}

	private bool IsHelpCorePrepared()
	{
		return helpCore != null;
	}
}
