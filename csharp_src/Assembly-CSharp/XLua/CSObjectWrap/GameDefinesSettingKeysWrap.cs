using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSettingKeysWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SettingKeys);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 70, 0, 0);
		Utils.RegisterObject(L, translator, -4, "ACCOUNT_LIST_DEBUG", "ACCOUNT_LIST_DEBUG");
		Utils.RegisterObject(L, translator, -4, "LAST_SERVER_KEY", "DEBUG_LAST_SERVERID");
		Utils.RegisterObject(L, translator, -4, "GAME_UID", "Setting.GAME_UID");
		Utils.RegisterObject(L, translator, -4, "GM_FLAG", "Setting.GM_FLAG");
		Utils.RegisterObject(L, translator, -4, "UUID", "Setting.UUID");
		Utils.RegisterObject(L, translator, -4, "DEVICE_ID", "DEVICE_ID");
		Utils.RegisterObject(L, translator, -4, "SERVER_IP", "SERVER_IP");
		Utils.RegisterObject(L, translator, -4, "SERVER_PORT", "SERVER_PORT");
		Utils.RegisterObject(L, translator, -4, "SERVER_ZONE", "SERVER_ZONE");
		Utils.RegisterObject(L, translator, -4, "SERVER_CONNECTION_TYPE", "SERVER_CONNECTION_TYPE");
		Utils.RegisterObject(L, translator, -4, "ACCESS_TOKEN", "Login.access_token");
		Utils.RegisterObject(L, translator, -4, "ACCESS_TOKEN_TIME", "Login.access_token_time");
		Utils.RegisterObject(L, translator, -4, "REFRESH_TOKEN", "Login.refresh_token");
		Utils.RegisterObject(L, translator, -4, "REFRESH_TOKEN_TIME", "Login.refresh_token_time");
		Utils.RegisterObject(L, translator, -4, "LOGIN_KEY", "Login.login_key");
		Utils.RegisterObject(L, translator, -4, "COK_PURCHASE_SUCCESSED_KEY", "Setting.COK_PURCHASE_SUCCESSED_KEY");
		Utils.RegisterObject(L, translator, -4, "COK_PURCHASE_KEY", "Setting.COK_PURCHASE_KEY");
		Utils.RegisterObject(L, translator, -4, "CATCH_ITEM_ID", "Setting.CATCH_ITEM_ID");
		Utils.RegisterObject(L, translator, -4, "EFFECT_MUSIC_ON", "isEffectMusicOn");
		Utils.RegisterObject(L, translator, -4, "BG_MUSIC_ON", "isBGMusicOn");
		Utils.RegisterObject(L, translator, -4, "ENV_SOUND_ON", "ENV_SOUND_ON");
		Utils.RegisterObject(L, translator, -4, "ThreeDWORLD_SWITCH", "3dworld_switch");
		Utils.RegisterObject(L, translator, -4, "SHOW_FAVORITE", "show_favorite");
		Utils.RegisterObject(L, translator, -4, "TASK_TIPS_ON", "isTaskTipsOn");
		Utils.RegisterObject(L, translator, -4, "WORLD_SCROLL_UI", "world_scroll_gameui");
		Utils.RegisterObject(L, translator, -4, "HIDE_BASE_TEMPERATURE", "HIDE_BASE_TEMPERATURE");
		Utils.RegisterObject(L, translator, -4, "TOUCH_SP_FUN", "touch_sp_fun");
		Utils.RegisterObject(L, translator, -4, "COORDINATE_ON_SHOW", "COORDINATE_ON_SHOW");
		Utils.RegisterObject(L, translator, -4, "Transporter_Hidden", "transporter_hidden");
		Utils.RegisterObject(L, translator, -4, "ISETTING_CASTLE_CLICK_PRIORITY", "ISetting_CastleClickPriority");
		Utils.RegisterObject(L, translator, -4, "USER_LANGUAGE", "Setting.USER_LANGUAGE");
		Utils.RegisterObject(L, translator, -4, "RECHARGE_ACTV_TOMORROW_TIME", "recharge.actv.tomorrow.time.mark");
		Utils.RegisterObject(L, translator, -4, "GUIDE_STEP", "guideStep");
		Utils.RegisterObject(L, translator, -4, "GUIDE_MP4", "guideMp4");
		Utils.RegisterObject(L, translator, -4, "GUIDE_FOR_TERRITORY", "Setting.Armygroup_Territory6");
		Utils.RegisterObject(L, translator, -4, "GUIDE_FOR_MARCH", "Setting.Armygroup_Over6");
		Utils.RegisterObject(L, translator, -4, "POST_PROCESSING_BLOOM", "POST_PROCESSING_BLOOM");
		Utils.RegisterObject(L, translator, -4, "POST_PROCESSING_VIGNETTE", "POST_PROCESSING_VIGNETTE");
		Utils.RegisterObject(L, translator, -4, "SCENE_PARTICLES", "SCENE_PARTICLES");
		Utils.RegisterObject(L, translator, -4, "RESOURCE_LOGGER", "Setting.Resource.Logger");
		Utils.RegisterObject(L, translator, -4, "SCENE_GRAPHIC_LEVEL", "SCENE_GRAPHIC_LEVEL");
		Utils.RegisterObject(L, translator, -4, "SCENE_FPS_LEVEL", "SCENE_FPS_LEVEL");
		Utils.RegisterObject(L, translator, -4, "SHOW_DEBUG_CHOOSE_SERVER", "SHOW_DEBUG_CHOOSE_SERVER");
		Utils.RegisterObject(L, translator, -4, "CITY_TROOP_POSITION", "CITY_TROOP_POSITION");
		Utils.RegisterObject(L, translator, -4, "MAIL_LAST_OPEN_TIME_BY_GROUP", "MAIL_LAST_OPEN_TIME_BY_GROUP_");
		Utils.RegisterObject(L, translator, -4, "ALLIANCE_WAR_OLD_DATA", "ALLIANCE_WAR_OLD_DATA");
		Utils.RegisterObject(L, translator, -4, "ARABIC_AUTO_MIRROR_SWITCH", "ARABIC_AUTO_MIRROR_SWITCH");
		Utils.RegisterObject(L, translator, -4, "SERVER_COUNTRY", "SERVER_COUNTRY");
		Utils.RegisterObject(L, translator, -4, "SEASON_MAP_TYPE", "SEASON_MAP_TYPE");
		Utils.RegisterObject(L, translator, -4, "SEASON_START_TIME", "SeasonStartTime");
		Utils.RegisterObject(L, translator, -4, "SEASON_SETTLE_TIME", "SeasonSettleTime");
		Utils.RegisterObject(L, translator, -4, "SEASON_END_TIME", "SeasonEndTime");
		Utils.RegisterObject(L, translator, -4, "SEASON_LOADING_BGM", "SeasonBGM");
		Utils.RegisterObject(L, translator, -4, "USE_SEASON_BGM", "USE_SEASON_BGM");
		Utils.RegisterObject(L, translator, -4, "DEBUG_CHOOSE_URL_GROUP", "DEBUG_CHOOSE_URL_GROUP_NEW");
		Utils.RegisterObject(L, translator, -4, "IS_CHANGE_DEBUG_CHOOSE_URL_GROUP", "IS_CHANGE_DEBUG_CHOOSE_URL_GROUP");
		Utils.RegisterObject(L, translator, -4, "RELOAD_DEBUG_SHOW_SERVER_LIST", "RELOAD_DEBUG_SHOW_SERVER_LIST");
		Utils.RegisterObject(L, translator, -4, "SHUMEI_SDK_IS_FUNCTION_OPEN", "SHUMEI_SDK_IS_FUNCTION_OPEN");
		Utils.RegisterObject(L, translator, -4, "UNPACK_RESOURCE_DOWNLOAD_RECORD", "UNPACK_RESOURCE_DOWNLOAD_RECORD");
		Utils.RegisterObject(L, translator, -4, "UNPACK_RESOURCE_DOWNLOAD_PAUSE_RECORD", "UNPACK_RESOURCE_DOWNLOAD_PAUSE_RECORD");
		Utils.RegisterObject(L, translator, -4, "LOADING_NAME_FIRST", "LOADING_NAME_FIRST");
		Utils.RegisterObject(L, translator, -4, "PC_DOWNLOAD_SETUP_NAME", "PC_DOWNLOAD_SETUP_NAME");
		Utils.RegisterObject(L, translator, -4, "PC_DOWNLOAD_CLICK_KEY", "PC_DOWNLOAD_CLICK_KEY");
		Utils.RegisterObject(L, translator, -4, "EFFECT_VOLUME", "EFFECT_VOLUME");
		Utils.RegisterObject(L, translator, -4, "MUSIC_VOLUME", "MUSIC_VOLUME");
		Utils.RegisterObject(L, translator, -4, "FULL_SCREEN_ON", "FULL_SCREEN_ON");
		Utils.RegisterObject(L, translator, -4, "IS_FIRST_LOGIN", "IS_FIRST_LOGIN");
		Utils.RegisterObject(L, translator, -4, "ENV_SOUND_VOLUME", "ENV_SOUND_VOLUME");
		Utils.RegisterObject(L, translator, -4, "LOADING_DEFAULT_BGM", "LOADING_DEFAULT_BGM");
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
				GameDefines.SettingKeys o = new GameDefines.SettingKeys();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.SettingKeys constructor!");
	}
}
