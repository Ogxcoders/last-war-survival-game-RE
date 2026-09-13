using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldMarchDataManagerBattleWordTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(WorldMarchDataManager.BattleWordType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(WorldMarchDataManager.BattleWordType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(WorldMarchDataManager.BattleWordType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Normal", WorldMarchDataManager.BattleWordType.Normal);
		Utils.RegisterObject(L, translator, -4, "Cure", WorldMarchDataManager.BattleWordType.Cure);
		Utils.RegisterObject(L, translator, -4, "Skill", WorldMarchDataManager.BattleWordType.Skill);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(WorldMarchDataManager.BattleWordType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushWorldMarchDataManagerBattleWordType(L, (WorldMarchDataManager.BattleWordType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Normal"))
			{
				objectTranslator.PushWorldMarchDataManagerBattleWordType(L, WorldMarchDataManager.BattleWordType.Normal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Cure"))
			{
				objectTranslator.PushWorldMarchDataManagerBattleWordType(L, WorldMarchDataManager.BattleWordType.Cure);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Skill"))
			{
				objectTranslator.PushWorldMarchDataManagerBattleWordType(L, WorldMarchDataManager.BattleWordType.Skill);
				break;
			}
			return Lua.luaL_error(L, "invalid string for WorldMarchDataManager.BattleWordType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for WorldMarchDataManager.BattleWordType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
