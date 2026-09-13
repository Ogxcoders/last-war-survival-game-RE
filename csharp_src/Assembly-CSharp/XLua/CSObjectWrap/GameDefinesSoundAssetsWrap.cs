using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSoundAssetsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SoundAssets);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 52, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Music_M_city_1", "m_city");
		Utils.RegisterObject(L, translator, -4, "Music_M_city_3", "m_field");
		Utils.RegisterObject(L, translator, -4, "Music_Sfx_logo_loading", "sfx_logo_loading");
		Utils.RegisterObject(L, translator, -4, "Music_M_battle_1", "m_city");
		Utils.RegisterObject(L, translator, -4, "Video_bg_1", "vedio_bg_1");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_city_night", "bgm_base_night");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_city_night_01", "bgm_base_night_01");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_city_day", "bgm_base_day");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_city_day_01", "Assets/Main/Sound/Music/bgm_base_day_01.ogg");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_city_day_02", "bgm_base_day_02");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_pve", "Bgm_Movie_Battle1");
		Utils.RegisterObject(L, translator, -4, "Music_Bgm_parkour_battle", "bgm_pve_30034");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Open", "effect_open");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Close", "effect_close");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Ground", "effect_ground");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Mist", "effect_mist");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Building", "effect_building");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Farm", "effect_farm");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Ranch", "effect_ranch");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Creeps", "effect_creeps");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Army", "effect_army");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Electric", "effect_electric");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Crystal", "effect_crystal");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Water", "effect_water");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Gas", "effect_gas");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Coin", "effect_coin");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Product1", "effect_product1");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Product2", "effect_product2");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Product3", "effect_product3");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Trained", "effect_trained");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Plant", "effect_plant");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Feed", "effect_feed");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Produce_Put", "effect_produce_put");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Produce_Box", "effect_produce_box");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Bill", "effect_bill");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Button", "effect_button");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Road", "effect_road");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Message", "effect_message");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Finish", "effect_finished");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Rocket", "effect_rocket");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Rocket_Land", "effect_rocket_land");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Radar", "effect_radar");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Alliance", "effect_alliance");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Attack", "effect_attack");
		Utils.RegisterObject(L, translator, -4, "Music_Effect_Skill_Attack", "effect_skill");
		Utils.RegisterObject(L, translator, -4, "Music_Invasion_Aisilla_Born", "aisila_born");
		Utils.RegisterObject(L, translator, -4, "Music_Invasion_Aisilla_Dead", "aisila_dead");
		Utils.RegisterObject(L, translator, -4, "Music_Invasion_Aisilla_Skill_Down", "aisila_skill_down_1");
		Utils.RegisterObject(L, translator, -4, "Music_Invasion_Aisilla_Skill_Up", "aisila_skill_up");
		Utils.RegisterObject(L, translator, -4, "MUSIC_NEW_CHALLENGE_BOX_BORN", "Gameplay/challenge_zombie/Baoxiang/SFX_Env_Baoxiang_World_Open");
		Utils.RegisterObject(L, translator, -4, "DOMINATOR_COCKATRICE_TIMELINE", "Dominator/SFX_Env_Xunzhaohuoban_TL");
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
				GameDefines.SoundAssets o = new GameDefines.SoundAssets();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.SoundAssets constructor!");
	}
}
