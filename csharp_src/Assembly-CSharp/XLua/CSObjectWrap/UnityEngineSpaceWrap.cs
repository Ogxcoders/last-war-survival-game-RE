using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineSpaceWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Space), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Space), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Space), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "World", Space.World);
		Utils.RegisterObject(L, translator, -4, "Self", Space.Self);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Space), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineSpace(L, (Space)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "World"))
			{
				objectTranslator.PushUnityEngineSpace(L, Space.World);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Self"))
			{
				objectTranslator.PushUnityEngineSpace(L, Space.Self);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Space!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Space! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
