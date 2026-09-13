using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesUIAssetsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.UIAssets);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 53, 0, 0);
		Utils.RegisterObject(L, translator, -4, "SubLoadingNormal", "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingAmerica", "Assets/Main/Loading/Prefabs/UILoading_America.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingArabic", "Assets/Main/Loading/Prefabs/UILoading_Arabic.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingChineseSimplified", "Assets/Main/Loading/Prefabs/UILoading_ChineseSimplified.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingKorean", "Assets/Main/Loading/Prefabs/UILoading_Korea.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingJapanese", "Assets/Main/Loading/Prefabs/UILoading_Japan.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingThai", "Assets/Main/Loading/Prefabs/UILoading_Thai.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingIndo", "Assets/Main/Loading/Prefabs/UILoading_Indo.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingFrench", "Assets/Main/Loading/Prefabs/UILoading_French.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingGerman", "Assets/Main/Loading/Prefabs/UILoading_German.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingBrazil", "Assets/Main/Loading/Prefabs/UILoading_Brazil.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingSeasonLondon", "Assets/Main/Loading/Prefabs/UILoading_SeasonLondon.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingSeasonSnow", "Assets/Main/Loading/Prefabs/UILoading_SeasonSnow.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingSeasonMummy", "Assets/Main/Loading/Prefabs/UILoading_SeasonMummy.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingSeasonDark", "Assets/Main/Loading/Prefabs/UILoading_SeasonDark.prefab");
		Utils.RegisterObject(L, translator, -4, "SubLoadingSeasonNineNation", "Assets/Main/Loading/Prefabs/UILoading_SeasonNineNation.prefab");
		Utils.RegisterObject(L, translator, -4, "UILoading", "Assets/Main/Loading/Prefabs/UILoading_Base.prefab");
		Utils.RegisterObject(L, translator, -4, "UIPrivacy", "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacy.prefab");
		Utils.RegisterObject(L, translator, -4, "UIPrivacyKR", "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacyKR.prefab");
		Utils.RegisterObject(L, translator, -4, "UICoppaView", "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacyCoppa.prefab");
		Utils.RegisterObject(L, translator, -4, "UIZendesk", "Assets/Main/Prefabs/UI/UIZendesk/UIZendesk.prefab");
		Utils.RegisterObject(L, translator, -4, "ProfileGraphy", "Assets/Main/Prefabs/Debug/Graphy.prefab");
		Utils.RegisterObject(L, translator, -4, "GFXConsole", "Assets/Main/Prefabs/Debug/GFXConsole.prefab");
		Utils.RegisterObject(L, translator, -4, "UIChooseLocalUpdate", "Assets/Main/Prefabs/Debug/UIChooseLocalUpdate.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck1", "Assets/Main/Prefabs/Vehicle/WorldCityTruck01.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck2", "Assets/Main/Prefabs/Vehicle/WorldCityTruck02.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck3", "Assets/Main/Prefabs/Vehicle/WorldCityTruck03.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck4", "Assets/Main/Prefabs/Vehicle/WorldCityTruck04.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck5", "Assets/Main/Prefabs/Vehicle/WorldCityTruck05.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck6", "Assets/Main/Prefabs/Vehicle/WorldCityTruck06.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTruck7", "Assets/Main/Prefabs/Vehicle/WorldCityTruck07.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople1", "Assets/Main/Prefabs/Vehicle/WorldCityPeople01.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople2", "Assets/Main/Prefabs/Vehicle/WorldCityPeople02.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople3", "Assets/Main/Prefabs/Vehicle/WorldCityPeople03.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople4", "Assets/Main/Prefabs/Vehicle/WorldCityPeople04.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople5", "Assets/Main/Prefabs/Vehicle/WorldCityPeople05.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople6", "Assets/Main/Prefabs/Vehicle/WorldCityPeople06.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople7", "Assets/Main/Prefabs/Vehicle/WorldCityPeople07.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityPeople8", "Assets/Main/Prefabs/Vehicle/WorldCityPeople08.prefab");
		Utils.RegisterObject(L, translator, -4, "UIPartsMaterialInfo", "Assets/Main/Prefabs/UI/Parts/UIPartsMaterialInfo.prefab");
		Utils.RegisterObject(L, translator, -4, "UITokenShop", "Assets/Main/Prefabs/UI/Shop/UITimeLimitShop.prefab");
		Utils.RegisterObject(L, translator, -4, "UIMultipleShop", "Assets/Main/Prefabs/UI/MilitaryInformation/UIMultipleShop.prefab");
		Utils.RegisterObject(L, translator, -4, "SceneRocketFireEffect", "Assets/Main/Prefabs/RocketEffect/SceneRocketFireEffect.prefab");
		Utils.RegisterObject(L, translator, -4, "SceneRocketSmokeEffect", "Assets/Main/Prefabs/RocketEffect/SceneRocketSmokeEffect.prefab");
		Utils.RegisterObject(L, translator, -4, "UIMultiKill", "Assets/Main/Prefabs/UI/MultiKill/UIMultiKill.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldAssistanceLabelPlayer", "Assets/Main/Prefabs/MainCity/WorldAssistanceLabelPlayer.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldAssistanceLabelAllianceBuilding", "Assets/Main/Prefabs/MainCity/WorldAssistanceLabelAllianceBuilding.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityFireEffectAsync", "Assets/Main/Prefabs/MainCity/Eff_ui_zhushou_build_fire.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityMummyFireEffectAsync", "Assets/Main/SeasonRes/Shared/Prefabs/World/Eff_ui_zhushou_build_fire_blue.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldAssistanceHeroPlayer", "Assets/Main/Prefabs/MainCity/WorldAssistanceHeroPlayer.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldAssistanceHeroAllianceBuilding", "Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceBuilding.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldAssistanceHeroAllianceTrade", "Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceTrade.prefab");
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
				GameDefines.UIAssets o = new GameDefines.UIAssets();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.UIAssets constructor!");
	}
}
