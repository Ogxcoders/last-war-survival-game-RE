using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LODTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(LODType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(LODType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(LODType), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Effect", LODType.Effect);
		Utils.RegisterObject(L, translator, -4, "Squad", LODType.Squad);
		Utils.RegisterObject(L, translator, -4, "March", LODType.March);
		Utils.RegisterObject(L, translator, -4, "Building", LODType.Building);
		Utils.RegisterObject(L, translator, -4, "Skill", LODType.Skill);
		Utils.RegisterObject(L, translator, -4, "CPU", LODType.CPU);
		Utils.RegisterObject(L, translator, -4, "Firework", LODType.Firework);
		Utils.RegisterObject(L, translator, -4, "Effect_WorldTroop", LODType.Effect_WorldTroop);
		Utils.RegisterObject(L, translator, -4, "Effect_WorldCommon", LODType.Effect_WorldCommon);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(LODType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushLODType(L, (LODType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Effect"))
			{
				objectTranslator.PushLODType(L, LODType.Effect);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Squad"))
			{
				objectTranslator.PushLODType(L, LODType.Squad);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "March"))
			{
				objectTranslator.PushLODType(L, LODType.March);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Building"))
			{
				objectTranslator.PushLODType(L, LODType.Building);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Skill"))
			{
				objectTranslator.PushLODType(L, LODType.Skill);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CPU"))
			{
				objectTranslator.PushLODType(L, LODType.CPU);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Firework"))
			{
				objectTranslator.PushLODType(L, LODType.Firework);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Effect_WorldTroop"))
			{
				objectTranslator.PushLODType(L, LODType.Effect_WorldTroop);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Effect_WorldCommon"))
			{
				objectTranslator.PushLODType(L, LODType.Effect_WorldCommon);
				break;
			}
			return Lua.luaL_error(L, "invalid string for LODType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for LODType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
