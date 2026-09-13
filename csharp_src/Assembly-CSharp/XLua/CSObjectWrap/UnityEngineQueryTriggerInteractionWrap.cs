using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineQueryTriggerInteractionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(QueryTriggerInteraction), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(QueryTriggerInteraction), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(QueryTriggerInteraction), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "UseGlobal", QueryTriggerInteraction.UseGlobal);
		Utils.RegisterObject(L, translator, -4, "Ignore", QueryTriggerInteraction.Ignore);
		Utils.RegisterObject(L, translator, -4, "Collide", QueryTriggerInteraction.Collide);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(QueryTriggerInteraction), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineQueryTriggerInteraction(L, (QueryTriggerInteraction)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "UseGlobal"))
			{
				objectTranslator.PushUnityEngineQueryTriggerInteraction(L, QueryTriggerInteraction.UseGlobal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Ignore"))
			{
				objectTranslator.PushUnityEngineQueryTriggerInteraction(L, QueryTriggerInteraction.Ignore);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Collide"))
			{
				objectTranslator.PushUnityEngineQueryTriggerInteraction(L, QueryTriggerInteraction.Collide);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.QueryTriggerInteraction!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.QueryTriggerInteraction! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
