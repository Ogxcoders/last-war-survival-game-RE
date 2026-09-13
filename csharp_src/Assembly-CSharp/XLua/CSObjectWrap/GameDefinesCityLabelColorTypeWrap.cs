using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesCityLabelColorTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(GameDefines.CityLabelColorType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(GameDefines.CityLabelColorType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(GameDefines.CityLabelColorType), L, null, 18, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Green", GameDefines.CityLabelColorType.Green);
		Utils.RegisterObject(L, translator, -4, "Blue", GameDefines.CityLabelColorType.Blue);
		Utils.RegisterObject(L, translator, -4, "White", GameDefines.CityLabelColorType.White);
		Utils.RegisterObject(L, translator, -4, "Yellow", GameDefines.CityLabelColorType.Yellow);
		Utils.RegisterObject(L, translator, -4, "Red", GameDefines.CityLabelColorType.Red);
		Utils.RegisterObject(L, translator, -4, "Purple", GameDefines.CityLabelColorType.Purple);
		Utils.RegisterObject(L, translator, -4, "Black", GameDefines.CityLabelColorType.Black);
		Utils.RegisterObject(L, translator, -4, "AllianceEnemy", GameDefines.CityLabelColorType.AllianceEnemy);
		Utils.RegisterObject(L, translator, -4, "ZoneEnemy", GameDefines.CityLabelColorType.ZoneEnemy);
		Utils.RegisterObject(L, translator, -4, "SeasonEnemy", GameDefines.CityLabelColorType.SeasonEnemy);
		Utils.RegisterObject(L, translator, -4, "SeasonCamp", GameDefines.CityLabelColorType.SeasonCamp);
		Utils.RegisterObject(L, translator, -4, "SeasonAssist", GameDefines.CityLabelColorType.SeasonAssist);
		Utils.RegisterObject(L, translator, -4, "BattlefieldDsbRole1", GameDefines.CityLabelColorType.BattlefieldDsbRole1);
		Utils.RegisterObject(L, translator, -4, "BattlefieldDsbRole2", GameDefines.CityLabelColorType.BattlefieldDsbRole2);
		Utils.RegisterObject(L, translator, -4, "BattlefieldDsbRole3", GameDefines.CityLabelColorType.BattlefieldDsbRole3);
		Utils.RegisterObject(L, translator, -4, "BattlefieldDsbRole4", GameDefines.CityLabelColorType.BattlefieldDsbRole4);
		Utils.RegisterObject(L, translator, -4, "BattlefieldDsbRoleMine", GameDefines.CityLabelColorType.BattlefieldDsbRoleMine);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(GameDefines.CityLabelColorType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushGameDefinesCityLabelColorType(L, (GameDefines.CityLabelColorType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Green"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.Green);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Blue"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.Blue);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "White"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.White);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Yellow"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.Yellow);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Red"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.Red);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Purple"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.Purple);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Black"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.Black);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AllianceEnemy"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.AllianceEnemy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "ZoneEnemy"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.ZoneEnemy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SeasonEnemy"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.SeasonEnemy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SeasonCamp"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.SeasonCamp);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SeasonAssist"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.SeasonAssist);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattlefieldDsbRole1"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.BattlefieldDsbRole1);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattlefieldDsbRole2"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.BattlefieldDsbRole2);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattlefieldDsbRole3"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.BattlefieldDsbRole3);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattlefieldDsbRole4"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.BattlefieldDsbRole4);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattlefieldDsbRoleMine"))
			{
				objectTranslator.PushGameDefinesCityLabelColorType(L, GameDefines.CityLabelColorType.BattlefieldDsbRoleMine);
				break;
			}
			return Lua.luaL_error(L, "invalid string for GameDefines.CityLabelColorType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for GameDefines.CityLabelColorType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
