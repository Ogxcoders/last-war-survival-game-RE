using System.Collections.Generic;
using System.Text;
using SFSLitJson;
using ThinkingSDK.PC.Utils;
using UnityEngine;

namespace Zendesk;

public static class ZendeskDefine
{
	public class EntranceInfo
	{
		public string name;

		public bool isMessaging;

		public EntranceInfo(string name, bool isMessaging)
		{
			this.name = name;
			this.isMessaging = isMessaging;
		}
	}

	public static class UserConfig
	{
		public class FieldInfo
		{
			public string id;

			public string old_id;

			public string value;

			public string type;

			public string prefix;

			public FieldInfo(string id, string old_id, string prefix = "")
			{
				this.id = id;
				value = "";
				type = "s";
				this.old_id = old_id;
				this.prefix = prefix;
			}
		}

		private static Dictionary<string, FieldInfo> dict = new Dictionary<string, FieldInfo>();

		private static void Init()
		{
			if (dict.Count <= 0)
			{
				dict.Add("uid", new FieldInfo("", "12145047103759"));
				dict.Add("username", new FieldInfo("", "12145205118863"));
				dict.Add("ServerId", new FieldInfo("41230102933523", "12145175980303"));
				dict.Add("Custom_Data", new FieldInfo("", "12145229067663"));
				dict.Add("Register_Country", new FieldInfo("46497366045587", "12145214344591", "register_country_"));
				dict.Add("Country_Code", new FieldInfo("46531063876243", "12145229528591", "curr_country_"));
				dict.Add("Game_Language", new FieldInfo("46497398721939", "12145229642255", "lang_"));
				dict.Add("Device_Model", new FieldInfo("", "12145191891599"));
				dict.Add("Os_Version", new FieldInfo("", "12145215138831"));
				dict.Add("AppId", new FieldInfo("41231842357395", "12145215236623"));
				dict.Add("Platform", new FieldInfo("41231828063635", "12145192205711"));
				dict.Add("Network_Type", new FieldInfo("", "12145208795663"));
				dict.Add("Main_Level", new FieldInfo("41230304632211", "12145200204687"));
				dict.Add("Alliance_ID", new FieldInfo("", "12145205590671"));
				dict.Add("Allliance_Name", new FieldInfo("", "12145200829327"));
				dict.Add("VIP_Level", new FieldInfo("41230353702931", "12145212592655"));
				dict.Add("userVipStage", new FieldInfo("41303943354003", ""));
				dict.Add("contact_status", new FieldInfo("41230330463507", "12145201209359"));
				dict.Add("contact_person", new FieldInfo("41231689158547", "12145213217039"));
				dict.Add("Mail_Address", new FieldInfo("", "12145213336719"));
				dict.Add("Register_Date", new FieldInfo("", "12145228552719"));
				dict.Add("Device_ID", new FieldInfo("", "12145190710415"));
				dict.Add("AirKey", new FieldInfo("43008303146259", ""));
				dict.Add("Resource_Version", new FieldInfo("", "12145223196431"));
				dict.Add("Distinct_ID", new FieldInfo("", "12145228951567"));
				dict.Add("Game_Version", new FieldInfo("", "12145223602191"));
				dict.Add("Entrance", new FieldInfo("41483777639059", ""));
				dict.Add("is_whale", new FieldInfo("", "12145201093903"));
				dict.Add("available_ram", new FieldInfo("43601836306835", ""));
			}
		}

		public static void Update(string key, string value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key].value = value;
			}
		}

		public static void Set(string userId, string userName, string serverId, string userTags, string customData)
		{
			Init();
			if (userName != "--" && !string.IsNullOrEmpty(serverId) && !string.IsNullOrEmpty(userTags) && !string.IsNullOrEmpty(customData))
			{
				Update("uid", userId);
				Update("username", userName);
				Update("ServerId", serverId);
				Update("Custom_Data", userTags);
				Update("Register_Country", GameEntry.Data.Player.GetRegisterCountry());
				JsonData jsonData = JsonMapper.ToObject(customData);
				{
					foreach (string key in jsonData.Keys)
					{
						JsonData jsonData2 = jsonData[key];
						if (dict.ContainsKey(key))
						{
							dict[key].value = jsonData2.ToString();
						}
					}
					return;
				}
			}
			Update("Device_ID", userId);
			Update("AirKey", GameEntry.Device.GetDeviceUid_Transcoding());
			Update("Country_Code", GameEntry.GlobalData.fromCountry);
			Update("Game_Language", GameEntry.Localization.GetLanguageNameToLoading(GameEntry.Setting.UserLanguage));
			Update("Device_Model", SystemInfo.deviceModel);
			Update("Os_Version", SystemInfo.operatingSystem);
			Update("AppId", GameEntry.Sdk.GetPackageName());
			Update("Platform", GameUtility.GetPlatformName());
			Update("Network_Type", ThinkingSDKDeviceInfo.NetworkType());
		}

		public static string ToFields()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string key in dict.Keys)
			{
				FieldInfo fieldInfo = dict[key];
				if (fieldInfo.value == null || fieldInfo.value.Length == 0)
				{
					continue;
				}
				if (!string.IsNullOrEmpty(fieldInfo.id))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(fieldInfo.id).Append(":").Append(fieldInfo.type)
						.Append(":")
						.Append(fieldInfo.prefix)
						.Append(fieldInfo.value);
				}
				if (!string.IsNullOrEmpty(fieldInfo.old_id))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(fieldInfo.old_id).Append(":").Append(fieldInfo.type)
						.Append(":")
						.Append(fieldInfo.value);
				}
			}
			return stringBuilder.ToString();
		}

		public static string ToJson()
		{
			JsonData jsonData = new JsonData();
			foreach (string key in dict.Keys)
			{
				FieldInfo fieldInfo = dict[key];
				if (fieldInfo.value != null && fieldInfo.value.Length != 0)
				{
					if (!string.IsNullOrEmpty(fieldInfo.id))
					{
						jsonData[fieldInfo.id] = fieldInfo.prefix + fieldInfo.value;
					}
					if (!string.IsNullOrEmpty(fieldInfo.old_id))
					{
						jsonData[fieldInfo.old_id] = fieldInfo.value;
					}
				}
			}
			return jsonData.ToJson();
		}
	}

	public const string SupportUrl = "https://firstfungroup.zendesk.com";

	public const string INIT = "Zendesk_Init";

	public const string SHOW = "Zendesk_Show";

	public const string LOGIN = "Zendesk_Login";

	public const string LOGOUT = "Zendesk_Logout";

	public const string FIELDS = "Zendesk_Fields";

	public const string UNREAD = "Zendesk_Unread";

	public const string AUTH = "Zendesk_Auth";

	public const string TAGS = "Zendesk_Tags";

	public const string CLEAR_FIELDS = "Zendesk_Clear_Fields";

	public const string CLEAR_TAGS = "Zendesk_Clear_Tags";

	public static readonly Dictionary<string, EntranceInfo> EntranceName = new Dictionary<string, EntranceInfo>
	{
		{
			"E001",
			new EntranceInfo("banned_e001", isMessaging: true)
		},
		{
			"E002",
			new EntranceInfo("loadingcall_e002", isMessaging: false)
		},
		{
			"E003",
			new EntranceInfo("refund_banned_e003", isMessaging: true)
		},
		{
			"E004",
			new EntranceInfo("delete_e004", isMessaging: true)
		},
		{
			"E005",
			new EntranceInfo("login_error_e005", isMessaging: true)
		},
		{
			"E006",
			new EntranceInfo("vip0-7_e006", isMessaging: false)
		},
		{
			"E007",
			new EntranceInfo("vip8-12_e007", isMessaging: false)
		},
		{
			"E008",
			new EntranceInfo("v12-17_e008", isMessaging: false)
		},
		{
			"E018",
			new EntranceInfo("vip18_e018", isMessaging: false)
		},
		{
			"E009",
			new EntranceInfo("krage_under14", isMessaging: false)
		},
		{
			"E019",
			new EntranceInfo("coppa_age_gate_e019", isMessaging: false)
		},
		{
			"E020",
			new EntranceInfo("coppa_u13_appeal_e020", isMessaging: false)
		},
		{
			"E010",
			new EntranceInfo("vip0-7_e006", isMessaging: true)
		},
		{
			"E011",
			new EntranceInfo("vip8-12_e007", isMessaging: true)
		},
		{
			"E012",
			new EntranceInfo("v12-17_e008", isMessaging: true)
		},
		{
			"E013",
			new EntranceInfo("season_creat_role_tips", isMessaging: true)
		},
		{
			"E015",
			new EntranceInfo("season_open_tips01", isMessaging: true)
		},
		{
			"E021",
			new EntranceInfo("shumei_id_risk_e021", isMessaging: true)
		},
		{
			"E022",
			new EntranceInfo("shumei_id_null_e022", isMessaging: true)
		}
	};
}
