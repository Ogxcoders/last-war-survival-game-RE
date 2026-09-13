namespace AIHelp;

public interface IAIHelpCore
{
	void Init(string appKey, string domain, string appId, string language);

	void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback);

	bool Show(string entranceId);

	bool Show(ApiConfig apiConfig);

	void ShowSingleFAQ(string faqId, ConversationMoment moment);

	void UpdateUserInfo(UserConfig userConfig);

	void ResetUserInfo();

	void UpdateSDKLanguage(string language);

	void SetUploadLogPath(string path);

	void SetPushTokenAndPlatform(string pushToken, PushPlatform platform);

	void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback callback);

	void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback callback);

	string GetSDKVersion();

	bool IsAIHelpShowing();

	void EnableLogging(bool enable);

	void ShowUrl(string url);

	void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion);

	void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback callback);

	void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback callback);

	void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback callback);

	void SetOnSpecificUrlClickedCallback(AIHelpDefine.OnSpecificUrlClickedCallback callback);

	void Close();
}
