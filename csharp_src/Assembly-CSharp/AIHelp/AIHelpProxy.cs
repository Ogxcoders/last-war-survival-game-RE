using GameFramework;
using UnityEngine;
using Zendesk;

namespace AIHelp;

public class AIHelpProxy
{
	public class AIHelpDataTable
	{
		public string Game_Version;

		public string Device_ID;

		public string AirKey;

		public string Mail_Address;

		public string Distinct_ID;
	}

	private static int unreadMsgCount = 0;

	private static bool IsAIHelpHaveConversation = true;

	public static bool IsUsingZendesk = false;

	private const string AIHELP_HAVE_CONVERSATION = "aihelp_have_conversation";

	public static AIHelpDataTable aIHelpDataTable;

	public static int UnreadMsgCount
	{
		get
		{
			if (IsUsingZendesk)
			{
				return ZendeskInit.unreadMsgCount;
			}
			return unreadMsgCount;
		}
		set
		{
			if (IsUsingZendesk)
			{
				ZendeskInit.unreadMsgCount = value;
			}
			unreadMsgCount = value;
		}
	}

	public static void ResetIsUsingZendesk(bool switchOn)
	{
		bool flag = StringUtils.VersionCompare(Application.version, "1.0.284") >= 0;
		Log.Info($"IsUsingZendesk: {flag}");
		IsUsingZendesk = flag;
	}

	public static bool CheckZendeskSwitch()
	{
		if (!GameEntry.Lua.HasGameStart)
		{
			return false;
		}
		return GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.CheckSwitch", "zendesk_sdk");
	}

	public static void CacheAIHelpHaveConversation(int status = -1)
	{
		if (status == -1)
		{
			IsAIHelpHaveConversation = PlayerPrefs.GetInt("aihelp_have_conversation", 1) == 1;
			return;
		}
		PlayerPrefs.SetInt("aihelp_have_conversation", status);
		IsAIHelpHaveConversation = status == 1;
	}

	public static void Init()
	{
		AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
		AIHelpSupport.Init("LASTWARDEV_app_3a8bd35be72e471abfaaa5f30971c54d", "lastwardev.aihelp.net", "lastwardev_platform_342905211d470af49c7bd03dbb6bb026");
		CacheAIHelpHaveConversation();
	}

	private static void OnAIHelpInitializedCallback(bool isSuccess, string message)
	{
		if (isSuccess)
		{
			Log.Info("AIHelp init success");
			AIHelpSupport.StartUnreadMessageCountPolling(OnUnreadMessageCountPolling);
		}
		else
		{
			Log.Error("AIHelp init failed: " + message);
		}
	}

	private static void OnUnreadMessageCountPolling(int msgCount)
	{
		unreadMsgCount = msgCount;
		Log.Info("AIHelp OnUnreadMessageCountPolling: " + msgCount);
	}

	public static void Show(string entranceId, string welcomeMessage)
	{
		if (!IsUsingZendesk)
		{
			AIHelpSupport.Show(new ApiConfig.Builder().SetEntranceId(entranceId).SetWelcomeMessage(welcomeMessage).build());
			unreadMsgCount = 0;
			Log.Info("AIHelp Show Reset UnreadMsgCount: " + unreadMsgCount);
		}
		else
		{
			ZendeskCore.ZendeskSupport(entranceId);
		}
	}

	public static void SetAIHelpDataList()
	{
		if (aIHelpDataTable == null)
		{
			aIHelpDataTable = new AIHelpDataTable();
		}
		aIHelpDataTable.Game_Version = GameEntry.Sdk.Version;
		aIHelpDataTable.Device_ID = GameEntry.Device.GetDeviceUid();
		aIHelpDataTable.AirKey = GameEntry.Device.GetDeviceUid_Transcoding();
		aIHelpDataTable.Mail_Address = GameEntry.Setting.GetString("Setting.CUSTOM_UID", "");
		aIHelpDataTable.Distinct_ID = GameEntry.Sdk.DistinctId;
	}

	public static void UpdateUserInfo(string uid, string uname, string tag, string serverId, string json)
	{
		ResetIsUsingZendesk(CheckZendeskSwitch());
		ZendeskDefine.UserConfig.Set(uid, uname, serverId, tag, json);
		ZendeskCore.SetFields(ZendeskDefine.UserConfig.ToFields());
		AIHelpSupport.UpdateUserInfo(new UserConfig.Builder().SetUserId(uid).SetUserName(uname).SetUserTags(tag)
			.SetCustomData(json)
			.SetServerId(serverId)
			.build());
	}

	public static void Logout()
	{
		if (IsUsingZendesk)
		{
			ZendeskCore.Logout();
			ZendeskCore.ClearFields();
		}
	}
}
