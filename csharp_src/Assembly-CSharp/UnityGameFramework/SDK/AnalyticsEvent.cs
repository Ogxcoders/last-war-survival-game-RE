using Sfs2X.Entities.Data;

namespace UnityGameFramework.SDK;

public class AnalyticsEvent
{
	public const string first_open_new = "first open new";

	public const string app_launch = "app_launch";

	public const string trackAppLaunch = "trackAppLaunch";

	public const string EventPurchase = "purchase";

	public const string EventLevelUp = "LevelUp";

	public const string click_buy_car_in_60m = "click_buy_car_in_60m";

	public const string click_gifts = "click_gifts";

	public const string click_gifts_in_30m = "click_gifts_in_30m";

	public const string setCustomerUserID = "setCustomerUserID";

	public const string tutorialComplete = "tutorialComplete";

	public const string triggerEventLoginComplete = "triggerEventLoginComplete";

	public const string EventCompletedRegistration = "CompletedRegistration";

	public const string fbEventCompletedTutorial = "fbEventCompletedTutorial";

	public const string EventSpeedUp = "SpeedUp";

	public const string EventGiftPackage = "GiftPackage";

	public const string EventPurchaseItem = "Purchase_Item";

	public const string EventFBEntrance = "FacebookEntrance";

	public const string EventHireWorker = "HireWorker";

	public const string EventAllianceHonorExchange = "AllianceHonorExchange";

	public const string EventAllianceScoreUsage = "AllianceScoreUsage";

	public const string EventAllianceTalkMore = "alliance_talk_more";

	public const string FBEventDone = "EventDone";

	public const string battle_base = "battle_base";

	public const string battle_resource = "battle_resource";

	public const string battle_rein = "battle_rein";

	public const string battle_scout = "battle_scout";

	public const string social_chat_country = "social_chat_country";

	public const string social_chat_alliance = "social_chat_alliance";

	public const string social_send_gift = "social_send_gift";

	private static string preLv = "";

	public static void TutorialComplete(string data, int _type)
	{
		switch (_type)
		{
		case 0:
			GameEntry.Sdk.LogEvent("fbEventCompletedTutorial", GameEntry.Data.Player.Uid);
			break;
		default:
			_ = 2;
			break;
		case 1:
			break;
		}
	}

	public static void FirstOpenAppsflyer()
	{
		GameEntry.Sdk.LogEvent("trackAppLaunch");
		GameEntry.Sdk.LogEvent("setCustomerUserID", GameEntry.Data.Player.Uid);
	}

	public static void Login(ISFSObject msg, string userId, string userName)
	{
		if (msg.ContainsKey("first_login"))
		{
			GameEntry.Sdk.LogEvent("CompletedRegistration", userId, userName);
		}
		if (msg.ContainsKey("two_days_login"))
		{
			SendAdjustTrack("two_days_login");
		}
		GameEntry.Sdk.LogEvent("triggerEventLoginComplete", userId, userName);
	}

	public static void TriggerEventPurchase(string cost, string key, string orderId, string uid)
	{
		GameEntry.Sdk.LogEvent("purchase", cost, key, orderId, uid);
	}

	public static void LevelUp(string lv)
	{
		if (preLv != lv)
		{
			preLv = lv;
			GameEntry.Sdk.LogEvent("LevelUp", lv);
		}
	}

	public static void ClickBuyIn60m(string id, string name)
	{
		GameEntry.Sdk.LogEvent("click_buy_car_in_60m", GameEntry.Data.Player.Uid, id, name);
	}

	public static void SpeedUp(int user_level, int castle_level, int type, int spend)
	{
		GameEntry.Sdk.LogEvent("SpeedUp", user_level, castle_level, type, spend);
	}

	public static void GiftPackage(string entracnce, string name, int id, int user_castle, int user_level)
	{
		GameEntry.Sdk.LogEvent("GiftPackage", entracnce, name, id, user_castle, user_level);
	}

	public static void AllianceHonorExchange(string name, string id, int user_castle, int user_level, int rank)
	{
		GameEntry.Sdk.LogEvent("AllianceHonorExchange", name, id, user_castle, user_level, rank);
	}

	public static void AllianceScoreUsage(string name, string id, int rank)
	{
		GameEntry.Sdk.LogEvent("AllianceScoreUsage", name, id, rank);
	}

	public static void AllianceChat(string eventName)
	{
		if (eventName == "social_chat_alliance")
		{
			int privateInt = GameEntry.Setting.GetPrivateInt("AllianceTalkCount", 0);
			privateInt++;
			GameEntry.Setting.SetPrivateInt("AllianceTalkCount", privateInt);
			if (privateInt == 10 && GameEntry.Data.Player != null)
			{
				GameEntry.Sdk.LogEvent("alliance_talk_more", GameEntry.Data.Player.Uid);
			}
		}
		else if (eventName == "social_chat_country")
		{
			EventDone("social_chat_country", "");
		}
	}

	public static void EventDone(string eventName, string data)
	{
		GameEntry.Sdk.LogEvent("EventDone", eventName, GameEntry.Data.Player.Uid);
	}

	public static void SendAdjustTrack(string track, string eventValue = "")
	{
	}
}
