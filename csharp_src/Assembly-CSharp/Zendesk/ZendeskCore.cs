using System;
using System.Collections;
using System.Text;
using GameFramework;
using SFSLitJson;
using ThinkingAnalytics;

namespace Zendesk;

public class ZendeskCore
{
	private static bool isInit = false;

	private static bool isLogin = false;

	private static string jwtToken = "";

	private static Action waitLoginAction = null;

	private static bool isInRequest = false;

	private static string temp_entranceId = "";

	private static string temp_entranceName = "";

	private static string ComposeData(params object[] data)
	{
		if (data == null || data.Length == 0 || data.Length % 2 != 0)
		{
			Log.Error("ZendeskCore ComposeData error");
			return "";
		}
		JsonData jsonData = new JsonData();
		for (int i = 0; i < data.Length; i += 2)
		{
			jsonData[data[i].ToString()] = data[i + 1].ToString();
		}
		return jsonData.ToJson();
	}

	public static void Init(string channelKey)
	{
		string data = ComposeData("channelKey", channelKey);
		GameEntry.Sdk.SendDataToNative("Zendesk_Init", data);
	}

	private static void Show(string entranceId, string entranceName)
	{
		if (isInit)
		{
			string data = ComposeData("id", entranceId, "name", entranceName);
			GameEntry.Sdk.SendDataToNative("Zendesk_Show", data);
		}
	}

	public static void Login(string jwtToken)
	{
		if (isInit)
		{
			string data = ComposeData("jwt", jwtToken);
			GameEntry.Sdk.SendDataToNative("Zendesk_Login", data);
		}
	}

	public static void Logout()
	{
		if (isInit)
		{
			GameEntry.Sdk.SendDataToNative("Zendesk_Logout", "{}");
		}
	}

	public static void SetFields(string fields)
	{
		if (isInit)
		{
			string data = ComposeData("fields", fields);
			GameEntry.Sdk.SendDataToNative("Zendesk_Fields", data);
		}
	}

	public static void ClearFields()
	{
		if (isInit)
		{
			GameEntry.Sdk.SendDataToNative("Zendesk_Clear_Fields", "{}");
		}
	}

	public static void SetTags(string tags)
	{
		if (isInit)
		{
			string data = ComposeData("tags", tags);
			GameEntry.Sdk.SendDataToNative("Zendesk_Tags", data);
		}
	}

	public static void ClearTags()
	{
		if (isInit)
		{
			GameEntry.Sdk.SendDataToNative("Zendesk_Clear_Tags", "{}");
		}
	}

	private static bool GetGameIsLogin()
	{
		if (GameEntry.Network != null)
		{
			return GameEntry.Network.Logined;
		}
		return false;
	}

	private static void GetJWTToken()
	{
		if (!isLogin && GetGameIsLogin())
		{
			ZendeskJWTTokenMessage.Instance.Send(new ZendeskJWTTokenMessage.Request());
		}
	}

	private static string GenerateWebUrl(string url)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(url).Append("/hc");
		stringBuilder.Append("?token=");
		if (!string.IsNullOrEmpty(jwtToken))
		{
			string value = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtToken));
			stringBuilder.Append(value);
		}
		stringBuilder.Append("&fields=");
		ZendeskDefine.UserConfig.Update("Entrance", temp_entranceName);
		TDPresetProperties presetProperties = ThinkingAnalyticsAPI.GetPresetProperties();
		if (presetProperties != null)
		{
			ZendeskDefine.UserConfig.Update("available_ram", presetProperties.Ram);
		}
		string text = ZendeskDefine.UserConfig.ToJson();
		if (!string.IsNullOrEmpty(text))
		{
			string value2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
			stringBuilder.Append(value2);
		}
		return stringBuilder.ToString();
	}

	public static void ZendeskGetJWTToken(string token)
	{
		jwtToken = token;
		Login(jwtToken);
	}

	public static void ZendeskSupport(string entranceId)
	{
		if (!isInit || isInRequest)
		{
			return;
		}
		isInRequest = true;
		Action action = delegate
		{
			temp_entranceId = entranceId;
			temp_entranceName = entranceId;
			if (ZendeskDefine.EntranceName.ContainsKey(entranceId))
			{
				temp_entranceName = ZendeskDefine.EntranceName[entranceId].name;
				if (ZendeskDefine.EntranceName[entranceId].isMessaging)
				{
					isInRequest = false;
					ZendeskMessaging();
					return;
				}
			}
			if (GameEntry.Lua != null && GameEntry.Lua.UIManager != null)
			{
				GameEntry.Lua.UIManager.OpenWindow("UIZendesk", "https://firstfungroup.zendesk.com");
			}
			else
			{
				UIZendeskPrivacy.Instance.OpenPrivacyView();
			}
			isInRequest = false;
		};
		if (GetGameIsLogin())
		{
			if (!isLogin)
			{
				waitLoginAction = action;
				GetJWTToken();
			}
			else
			{
				action();
			}
		}
		else
		{
			action();
		}
	}

	public static void ZendeskMessaging()
	{
		if (!isInit || isInRequest)
		{
			return;
		}
		isInRequest = true;
		Action action = delegate
		{
			TDPresetProperties presetProperties = ThinkingAnalyticsAPI.GetPresetProperties();
			if (presetProperties != null)
			{
				ZendeskDefine.UserConfig.Update("available_ram", presetProperties.Ram);
			}
			ZendeskDefine.UserConfig.Update("Entrance", temp_entranceName);
			SetFields(ZendeskDefine.UserConfig.ToFields());
			Show(temp_entranceId, temp_entranceName);
			isInRequest = false;
		};
		if (GetGameIsLogin())
		{
			if (!isLogin)
			{
				waitLoginAction = action;
				GetJWTToken();
			}
			else
			{
				action();
			}
		}
		else
		{
			action();
		}
	}

	public static void OnNativeCallback(string funcName, string data)
	{
		IDictionary dictionary = JsonMapper.ToObject(data);
		switch (funcName)
		{
		case "Zendesk_Auth":
		{
			int code = 0;
			if (dictionary.Contains("code"))
			{
				code = int.Parse(dictionary["code"].ToString());
			}
			string message = "";
			if (dictionary.Contains("info"))
			{
				message = dictionary["info"].ToString();
			}
			OnAuthFailed(code, message);
			break;
		}
		case "Zendesk_Unread":
		{
			int unreadMsgCount2 = 0;
			if (dictionary.Contains("unread"))
			{
				unreadMsgCount2 = int.Parse(dictionary["unread"].ToString());
			}
			OnUnreadChanged(unreadMsgCount2);
			break;
		}
		case "Zendesk_Init":
		{
			int unreadMsgCount = 0;
			bool isSuccess2 = false;
			if (dictionary.Contains("unread"))
			{
				unreadMsgCount = int.Parse(dictionary["unread"].ToString());
			}
			if (dictionary.Contains("success"))
			{
				isSuccess2 = int.Parse(dictionary["success"].ToString()) == 1;
			}
			OnInit(isSuccess2, unreadMsgCount);
			break;
		}
		case "Zendesk_Login":
		{
			bool isSuccess3 = false;
			if (dictionary.Contains("success"))
			{
				isSuccess3 = int.Parse(dictionary["success"].ToString()) == 1;
			}
			string info2 = "";
			if (dictionary.Contains("info"))
			{
				info2 = dictionary["info"].ToString();
			}
			OnLogin(isSuccess3, info2);
			break;
		}
		case "Zendesk_Logout":
		{
			bool isSuccess = false;
			if (dictionary.Contains("success"))
			{
				isSuccess = int.Parse(dictionary["success"].ToString()) == 1;
			}
			string info = "";
			if (dictionary.Contains("info"))
			{
				info = dictionary["info"].ToString();
			}
			OnLogout(isSuccess, info);
			break;
		}
		default:
			Log.Error("ZendeskCore OnNativeCallback error: " + funcName);
			break;
		}
	}

	private static void OnInit(bool isSuccess, int unreadMsgCount)
	{
		isInit = false;
		isLogin = false;
		isInRequest = false;
		jwtToken = "";
		waitLoginAction = null;
		if (isSuccess)
		{
			isInit = true;
			Log.Info("Zendesk init success");
			ZendeskInit.unreadMsgCount = unreadMsgCount;
			SetFields(ZendeskDefine.UserConfig.ToFields());
		}
		else
		{
			Log.Error("Zendesk init failed");
		}
	}

	private static void OnUnreadChanged(int unreadMsgCount)
	{
		ZendeskInit.unreadMsgCount = unreadMsgCount;
		Log.Info("Zendesk OnUnreadChanged: " + unreadMsgCount);
	}

	private static void OnAuthFailed(int code, string message)
	{
		Log.Error("Zendesk OnAuthFailed: " + code + " " + message);
		isLogin = false;
		if (code == 401)
		{
			jwtToken = "";
			GetJWTToken();
		}
	}

	private static void OnLogin(bool isSuccess, string info)
	{
		Log.Info("Zendesk OnLogin: " + isSuccess + " " + info);
		if (isSuccess)
		{
			isLogin = true;
			if (waitLoginAction != null)
			{
				Action action = waitLoginAction;
				waitLoginAction = null;
				action();
			}
		}
		else
		{
			jwtToken = "";
			isLogin = false;
		}
	}

	private static void OnLogout(bool isSuccess, string info)
	{
		Log.Info("Zendesk OnLogout: " + isSuccess + " " + info);
		isLogin = false;
		jwtToken = "";
	}
}
