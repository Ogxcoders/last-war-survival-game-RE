using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesGuideTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.GuideType);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 26, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", 0);
		Utils.RegisterObject(L, translator, -4, "ClickButton", 1);
		Utils.RegisterObject(L, translator, -4, "ShowTalk", 2);
		Utils.RegisterObject(L, translator, -4, "ClickBuild", 3);
		Utils.RegisterObject(L, translator, -4, "BuildPlace", 4);
		Utils.RegisterObject(L, translator, -4, "BuildRoad", 5);
		Utils.RegisterObject(L, translator, -4, "PlantFarm", 6);
		Utils.RegisterObject(L, translator, -4, "GetFarm", 7);
		Utils.RegisterObject(L, translator, -4, "QueueBuild", 8);
		Utils.RegisterObject(L, translator, -4, "PlantAnimal", 9);
		Utils.RegisterObject(L, translator, -4, "Factory", 10);
		Utils.RegisterObject(L, translator, -4, "Bubble", 11);
		Utils.RegisterObject(L, translator, -4, "CityGarbage", 12);
		Utils.RegisterObject(L, translator, -4, "GotoMoveBubble", 13);
		Utils.RegisterObject(L, translator, -4, "OpenFog", 14);
		Utils.RegisterObject(L, translator, -4, "CityGarbageResultShow", 15);
		Utils.RegisterObject(L, translator, -4, "DragCityTroop", 16);
		Utils.RegisterObject(L, translator, -4, "PlayMovie", 17);
		Utils.RegisterObject(L, translator, -4, "WaitMovieComplete", 18);
		Utils.RegisterObject(L, translator, -4, "ClickQuest", 19);
		Utils.RegisterObject(L, translator, -4, "WaitPlaceBuilding", 20);
		Utils.RegisterObject(L, translator, -4, "WaitTroopArrive", 21);
		Utils.RegisterObject(L, translator, -4, "WaitGarbageTroopMoveLeft", 22);
		Utils.RegisterObject(L, translator, -4, "WaitCloseUI", 23);
		Utils.RegisterObject(L, translator, -4, "ClickBuildFinishBox", 24);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.GuideType does not have a constructor!");
	}
}
