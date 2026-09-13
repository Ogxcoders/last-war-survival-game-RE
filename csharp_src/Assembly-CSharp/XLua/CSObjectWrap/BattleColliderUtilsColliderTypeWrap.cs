using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BattleColliderUtilsColliderTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(BattleColliderUtils.ColliderType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(BattleColliderUtils.ColliderType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(BattleColliderUtils.ColliderType), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", BattleColliderUtils.ColliderType.None);
		Utils.RegisterObject(L, translator, -4, "Capsule", BattleColliderUtils.ColliderType.Capsule);
		Utils.RegisterObject(L, translator, -4, "Box", BattleColliderUtils.ColliderType.Box);
		Utils.RegisterObject(L, translator, -4, "Sphere", BattleColliderUtils.ColliderType.Sphere);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(BattleColliderUtils.ColliderType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushBattleColliderUtilsColliderType(L, (BattleColliderUtils.ColliderType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushBattleColliderUtilsColliderType(L, BattleColliderUtils.ColliderType.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Capsule"))
			{
				objectTranslator.PushBattleColliderUtilsColliderType(L, BattleColliderUtils.ColliderType.Capsule);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Box"))
			{
				objectTranslator.PushBattleColliderUtilsColliderType(L, BattleColliderUtils.ColliderType.Box);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Sphere"))
			{
				objectTranslator.PushBattleColliderUtilsColliderType(L, BattleColliderUtils.ColliderType.Sphere);
				break;
			}
			return Lua.luaL_error(L, "invalid string for BattleColliderUtils.ColliderType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for BattleColliderUtils.ColliderType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
