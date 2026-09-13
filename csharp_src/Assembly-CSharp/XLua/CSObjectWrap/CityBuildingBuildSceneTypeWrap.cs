using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CityBuildingBuildSceneTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(CityBuilding.BuildSceneType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(CityBuilding.BuildSceneType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(CityBuilding.BuildSceneType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "City", CityBuilding.BuildSceneType.City);
		Utils.RegisterObject(L, translator, -4, "World", CityBuilding.BuildSceneType.World);
		Utils.RegisterObject(L, translator, -4, "Fake", CityBuilding.BuildSceneType.Fake);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(CityBuilding.BuildSceneType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushCityBuildingBuildSceneType(L, (CityBuilding.BuildSceneType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "City"))
			{
				objectTranslator.PushCityBuildingBuildSceneType(L, CityBuilding.BuildSceneType.City);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "World"))
			{
				objectTranslator.PushCityBuildingBuildSceneType(L, CityBuilding.BuildSceneType.World);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Fake"))
			{
				objectTranslator.PushCityBuildingBuildSceneType(L, CityBuilding.BuildSceneType.Fake);
				break;
			}
			return Lua.luaL_error(L, "invalid string for CityBuilding.BuildSceneType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for CityBuilding.BuildSceneType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
