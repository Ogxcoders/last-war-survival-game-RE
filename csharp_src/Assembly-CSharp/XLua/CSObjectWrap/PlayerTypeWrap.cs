using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PlayerTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(PlayerType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(PlayerType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(PlayerType), L, null, 11, 0, 0);
		Utils.RegisterObject(L, translator, -4, "PlayerNone", PlayerType.PlayerNone);
		Utils.RegisterObject(L, translator, -4, "PlayerSelf", PlayerType.PlayerSelf);
		Utils.RegisterObject(L, translator, -4, "PlayerAlliance", PlayerType.PlayerAlliance);
		Utils.RegisterObject(L, translator, -4, "PlayerOther", PlayerType.PlayerOther);
		Utils.RegisterObject(L, translator, -4, "PlayerAllianceLeader", PlayerType.PlayerAllianceLeader);
		Utils.RegisterObject(L, translator, -4, "PlayerAllianceEnemy", PlayerType.PlayerAllianceEnemy);
		Utils.RegisterObject(L, translator, -4, "PlayerZoneEnemy", PlayerType.PlayerZoneEnemy);
		Utils.RegisterObject(L, translator, -4, "PlayerSeasonEnemy", PlayerType.PlayerSeasonEnemy);
		Utils.RegisterObject(L, translator, -4, "PlayerSeasonCamp", PlayerType.PlayerSeasonCamp);
		Utils.RegisterObject(L, translator, -4, "PlayerSeasonAssist", PlayerType.PlayerSeasonAssist);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(PlayerType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushPlayerType(L, (PlayerType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "PlayerNone"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerNone);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerSelf"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerSelf);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerAlliance"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerAlliance);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerOther"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerOther);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerAllianceLeader"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerAllianceLeader);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerAllianceEnemy"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerAllianceEnemy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerZoneEnemy"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerZoneEnemy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerSeasonEnemy"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerSeasonEnemy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerSeasonCamp"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerSeasonCamp);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PlayerSeasonAssist"))
			{
				objectTranslator.PushPlayerType(L, PlayerType.PlayerSeasonAssist);
				break;
			}
			return Lua.luaL_error(L, "invalid string for PlayerType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for PlayerType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
