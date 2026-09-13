using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ObjectPoolTagWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ObjectPoolTag), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ObjectPoolTag), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ObjectPoolTag), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Normal", ObjectPoolTag.Normal);
		Utils.RegisterObject(L, translator, -4, "Battle", ObjectPoolTag.Battle);
		Utils.RegisterObject(L, translator, -4, "BattleScene", ObjectPoolTag.BattleScene);
		Utils.RegisterObject(L, translator, -4, "BattleBullet", ObjectPoolTag.BattleBullet);
		Utils.RegisterObject(L, translator, -4, "City", ObjectPoolTag.City);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ObjectPoolTag), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushObjectPoolTag(L, (ObjectPoolTag)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Normal"))
			{
				objectTranslator.PushObjectPoolTag(L, ObjectPoolTag.Normal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Battle"))
			{
				objectTranslator.PushObjectPoolTag(L, ObjectPoolTag.Battle);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattleScene"))
			{
				objectTranslator.PushObjectPoolTag(L, ObjectPoolTag.BattleScene);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BattleBullet"))
			{
				objectTranslator.PushObjectPoolTag(L, ObjectPoolTag.BattleBullet);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "City"))
			{
				objectTranslator.PushObjectPoolTag(L, ObjectPoolTag.City);
				break;
			}
			return Lua.luaL_error(L, "invalid string for ObjectPoolTag!");
		default:
			return Lua.luaL_error(L, "invalid lua type for ObjectPoolTag! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
