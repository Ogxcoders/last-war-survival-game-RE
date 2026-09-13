using System;
using Sfs2X.Entities.Data;
using UnityGameFramework.SDK;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityGameFrameworkSDKAnalyticsEventWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AnalyticsEvent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 43, 0, 0);
		Utils.RegisterFunc(L, -4, "TutorialComplete", _m_TutorialComplete_xlua_st_);
		Utils.RegisterFunc(L, -4, "FirstOpenAppsflyer", _m_FirstOpenAppsflyer_xlua_st_);
		Utils.RegisterFunc(L, -4, "Login", _m_Login_xlua_st_);
		Utils.RegisterFunc(L, -4, "TriggerEventPurchase", _m_TriggerEventPurchase_xlua_st_);
		Utils.RegisterFunc(L, -4, "LevelUp", _m_LevelUp_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClickBuyIn60m", _m_ClickBuyIn60m_xlua_st_);
		Utils.RegisterFunc(L, -4, "SpeedUp", _m_SpeedUp_xlua_st_);
		Utils.RegisterFunc(L, -4, "GiftPackage", _m_GiftPackage_xlua_st_);
		Utils.RegisterFunc(L, -4, "AllianceHonorExchange", _m_AllianceHonorExchange_xlua_st_);
		Utils.RegisterFunc(L, -4, "AllianceScoreUsage", _m_AllianceScoreUsage_xlua_st_);
		Utils.RegisterFunc(L, -4, "AllianceChat", _m_AllianceChat_xlua_st_);
		Utils.RegisterFunc(L, -4, "EventDone", _m_EventDone_xlua_st_);
		Utils.RegisterFunc(L, -4, "SendAdjustTrack", _m_SendAdjustTrack_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "first_open_new", "first open new");
		Utils.RegisterObject(L, translator, -4, "app_launch", "app_launch");
		Utils.RegisterObject(L, translator, -4, "trackAppLaunch", "trackAppLaunch");
		Utils.RegisterObject(L, translator, -4, "EventPurchase", "purchase");
		Utils.RegisterObject(L, translator, -4, "EventLevelUp", "LevelUp");
		Utils.RegisterObject(L, translator, -4, "click_buy_car_in_60m", "click_buy_car_in_60m");
		Utils.RegisterObject(L, translator, -4, "click_gifts", "click_gifts");
		Utils.RegisterObject(L, translator, -4, "click_gifts_in_30m", "click_gifts_in_30m");
		Utils.RegisterObject(L, translator, -4, "setCustomerUserID", "setCustomerUserID");
		Utils.RegisterObject(L, translator, -4, "tutorialComplete", "tutorialComplete");
		Utils.RegisterObject(L, translator, -4, "triggerEventLoginComplete", "triggerEventLoginComplete");
		Utils.RegisterObject(L, translator, -4, "EventCompletedRegistration", "CompletedRegistration");
		Utils.RegisterObject(L, translator, -4, "fbEventCompletedTutorial", "fbEventCompletedTutorial");
		Utils.RegisterObject(L, translator, -4, "EventSpeedUp", "SpeedUp");
		Utils.RegisterObject(L, translator, -4, "EventGiftPackage", "GiftPackage");
		Utils.RegisterObject(L, translator, -4, "EventPurchaseItem", "Purchase_Item");
		Utils.RegisterObject(L, translator, -4, "EventFBEntrance", "FacebookEntrance");
		Utils.RegisterObject(L, translator, -4, "EventHireWorker", "HireWorker");
		Utils.RegisterObject(L, translator, -4, "EventAllianceHonorExchange", "AllianceHonorExchange");
		Utils.RegisterObject(L, translator, -4, "EventAllianceScoreUsage", "AllianceScoreUsage");
		Utils.RegisterObject(L, translator, -4, "EventAllianceTalkMore", "alliance_talk_more");
		Utils.RegisterObject(L, translator, -4, "FBEventDone", "EventDone");
		Utils.RegisterObject(L, translator, -4, "battle_base", "battle_base");
		Utils.RegisterObject(L, translator, -4, "battle_resource", "battle_resource");
		Utils.RegisterObject(L, translator, -4, "battle_rein", "battle_rein");
		Utils.RegisterObject(L, translator, -4, "battle_scout", "battle_scout");
		Utils.RegisterObject(L, translator, -4, "social_chat_country", "social_chat_country");
		Utils.RegisterObject(L, translator, -4, "social_chat_alliance", "social_chat_alliance");
		Utils.RegisterObject(L, translator, -4, "social_send_gift", "social_send_gift");
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
				AnalyticsEvent o = new AnalyticsEvent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityGameFramework.SDK.AnalyticsEvent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TutorialComplete_xlua_st_(IntPtr L)
	{
		try
		{
			string data = Lua.lua_tostring(L, 1);
			int type = Lua.xlua_tointeger(L, 2);
			AnalyticsEvent.TutorialComplete(data, type);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FirstOpenAppsflyer_xlua_st_(IntPtr L)
	{
		try
		{
			AnalyticsEvent.FirstOpenAppsflyer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Login_xlua_st_(IntPtr L)
	{
		try
		{
			ISFSObject msg = (ISFSObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(ISFSObject));
			string userId = Lua.lua_tostring(L, 2);
			string userName = Lua.lua_tostring(L, 3);
			AnalyticsEvent.Login(msg, userId, userName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TriggerEventPurchase_xlua_st_(IntPtr L)
	{
		try
		{
			string cost = Lua.lua_tostring(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string orderId = Lua.lua_tostring(L, 3);
			string uid = Lua.lua_tostring(L, 4);
			AnalyticsEvent.TriggerEventPurchase(cost, key, orderId, uid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LevelUp_xlua_st_(IntPtr L)
	{
		try
		{
			AnalyticsEvent.LevelUp(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClickBuyIn60m_xlua_st_(IntPtr L)
	{
		try
		{
			string id = Lua.lua_tostring(L, 1);
			string name = Lua.lua_tostring(L, 2);
			AnalyticsEvent.ClickBuyIn60m(id, name);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SpeedUp_xlua_st_(IntPtr L)
	{
		try
		{
			int user_level = Lua.xlua_tointeger(L, 1);
			int castle_level = Lua.xlua_tointeger(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			int spend = Lua.xlua_tointeger(L, 4);
			AnalyticsEvent.SpeedUp(user_level, castle_level, type, spend);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GiftPackage_xlua_st_(IntPtr L)
	{
		try
		{
			string entracnce = Lua.lua_tostring(L, 1);
			string name = Lua.lua_tostring(L, 2);
			int id = Lua.xlua_tointeger(L, 3);
			int user_castle = Lua.xlua_tointeger(L, 4);
			int user_level = Lua.xlua_tointeger(L, 5);
			AnalyticsEvent.GiftPackage(entracnce, name, id, user_castle, user_level);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AllianceHonorExchange_xlua_st_(IntPtr L)
	{
		try
		{
			string name = Lua.lua_tostring(L, 1);
			string id = Lua.lua_tostring(L, 2);
			int user_castle = Lua.xlua_tointeger(L, 3);
			int user_level = Lua.xlua_tointeger(L, 4);
			int rank = Lua.xlua_tointeger(L, 5);
			AnalyticsEvent.AllianceHonorExchange(name, id, user_castle, user_level, rank);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AllianceScoreUsage_xlua_st_(IntPtr L)
	{
		try
		{
			string name = Lua.lua_tostring(L, 1);
			string id = Lua.lua_tostring(L, 2);
			int rank = Lua.xlua_tointeger(L, 3);
			AnalyticsEvent.AllianceScoreUsage(name, id, rank);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AllianceChat_xlua_st_(IntPtr L)
	{
		try
		{
			AnalyticsEvent.AllianceChat(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EventDone_xlua_st_(IntPtr L)
	{
		try
		{
			string eventName = Lua.lua_tostring(L, 1);
			string data = Lua.lua_tostring(L, 2);
			AnalyticsEvent.EventDone(eventName, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendAdjustTrack_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string track = Lua.lua_tostring(L, 1);
				string eventValue = Lua.lua_tostring(L, 2);
				AnalyticsEvent.SendAdjustTrack(track, eventValue);
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				AnalyticsEvent.SendAdjustTrack(Lua.lua_tostring(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityGameFramework.SDK.AnalyticsEvent.SendAdjustTrack!");
	}
}
