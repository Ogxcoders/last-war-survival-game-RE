using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ModelManagerModelObjectTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ModelManager.ModelObjectType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ModelManager.ModelObjectType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ModelManager.ModelObjectType), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Build", ModelManager.ModelObjectType.Build);
		Utils.RegisterObject(L, translator, -4, "Road", ModelManager.ModelObjectType.Road);
		Utils.RegisterObject(L, translator, -4, "Garbage", ModelManager.ModelObjectType.Garbage);
		Utils.RegisterObject(L, translator, -4, "Monster", ModelManager.ModelObjectType.Monster);
		Utils.RegisterObject(L, translator, -4, "MonsterReward", ModelManager.ModelObjectType.MonsterReward);
		Utils.RegisterObject(L, translator, -4, "GarbageReward", ModelManager.ModelObjectType.GarbageReward);
		Utils.RegisterObject(L, translator, -4, "MonsterLock", ModelManager.ModelObjectType.MonsterLock);
		Utils.RegisterObject(L, translator, -4, "Collect", ModelManager.ModelObjectType.Collect);
		Utils.RegisterObject(L, translator, -4, "FreeGarbage", ModelManager.ModelObjectType.FreeGarbage);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ModelManager.ModelObjectType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushModelManagerModelObjectType(L, (ModelManager.ModelObjectType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Build"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.Build);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Road"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.Road);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Garbage"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.Garbage);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Monster"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.Monster);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MonsterReward"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.MonsterReward);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "GarbageReward"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.GarbageReward);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MonsterLock"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.MonsterLock);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Collect"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.Collect);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FreeGarbage"))
			{
				objectTranslator.PushModelManagerModelObjectType(L, ModelManager.ModelObjectType.FreeGarbage);
				break;
			}
			return Lua.luaL_error(L, "invalid string for ModelManager.ModelObjectType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for ModelManager.ModelObjectType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
