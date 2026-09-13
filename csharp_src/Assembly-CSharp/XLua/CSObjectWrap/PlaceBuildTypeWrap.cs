using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PlaceBuildTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(PlaceBuildType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(PlaceBuildType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(PlaceBuildType), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", PlaceBuildType.None);
		Utils.RegisterObject(L, translator, -4, "Build", PlaceBuildType.Build);
		Utils.RegisterObject(L, translator, -4, "Move", PlaceBuildType.Move);
		Utils.RegisterObject(L, translator, -4, "Replace", PlaceBuildType.Replace);
		Utils.RegisterObject(L, translator, -4, "MoveCity", PlaceBuildType.MoveCity);
		Utils.RegisterObject(L, translator, -4, "MoveCity_Al", PlaceBuildType.MoveCity_Al);
		Utils.RegisterObject(L, translator, -4, "MoveCity_Cmn", PlaceBuildType.MoveCity_Cmn);
		Utils.RegisterObject(L, translator, -4, "CityAttachment", PlaceBuildType.CityAttachment);
		Utils.RegisterObject(L, translator, -4, "EpidemicSkill", PlaceBuildType.EpidemicSkill);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(PlaceBuildType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushPlaceBuildType(L, (PlaceBuildType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Build"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.Build);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Move"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.Move);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Replace"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.Replace);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MoveCity"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.MoveCity);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MoveCity_Al"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.MoveCity_Al);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MoveCity_Cmn"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.MoveCity_Cmn);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CityAttachment"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.CityAttachment);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "EpidemicSkill"))
			{
				objectTranslator.PushPlaceBuildType(L, PlaceBuildType.EpidemicSkill);
				break;
			}
			return Lua.luaL_error(L, "invalid string for PlaceBuildType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for PlaceBuildType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
