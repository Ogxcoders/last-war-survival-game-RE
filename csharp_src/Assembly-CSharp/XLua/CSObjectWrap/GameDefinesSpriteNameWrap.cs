using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSpriteNameWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SpriteName);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 23, 0, 0);
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_100_normal", "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_bai.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_100_enemy", "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_hong.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_100_alliance", "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_lan.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_100_self", "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_lv.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_100_sameServer", "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_huang.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteScoreIcon_Red", "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_hong.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteScoreIcon_Yellow", "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_huang.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteScoreIcon_Blue", "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_lan.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteScoreIcon_Green", "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_lv.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteScoreIcon_Gray", "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_hui.png");
		Utils.RegisterObject(L, translator, -4, "MyAssistanceIcon", "Assets/Main/Sprites/LodIcon/wxy_dashijie_zhufang_zhushouwo.png");
		Utils.RegisterObject(L, translator, -4, "OtherAssistanceIcon", "Assets/Main/Sprites/LodIcon/wxy_dashijie_zhufang_zhushou.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_101_normal", "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_bai.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_101_enemy", "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_hong.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_101_alliance", "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_lan.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_101_self", "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_lv.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_101_sameServer", "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_huang.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_102_normal", "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_bai.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_102_enemy", "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_hong.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_102_alliance", "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_lan.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_102_self", "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_lv.png");
		Utils.RegisterObject(L, translator, -4, "MeteoriteLodIcon_102_sameServer", "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_huang.png");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.SpriteName does not have a constructor!");
	}
}
