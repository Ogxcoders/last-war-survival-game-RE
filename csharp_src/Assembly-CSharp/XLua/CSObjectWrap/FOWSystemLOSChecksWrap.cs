using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FOWSystemLOSChecksWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(FOWSystem.LOSChecks), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(FOWSystem.LOSChecks), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(FOWSystem.LOSChecks), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", FOWSystem.LOSChecks.None);
		Utils.RegisterObject(L, translator, -4, "OnlyOnce", FOWSystem.LOSChecks.OnlyOnce);
		Utils.RegisterObject(L, translator, -4, "EveryUpdate", FOWSystem.LOSChecks.EveryUpdate);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(FOWSystem.LOSChecks), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushFOWSystemLOSChecks(L, (FOWSystem.LOSChecks)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushFOWSystemLOSChecks(L, FOWSystem.LOSChecks.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "OnlyOnce"))
			{
				objectTranslator.PushFOWSystemLOSChecks(L, FOWSystem.LOSChecks.OnlyOnce);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "EveryUpdate"))
			{
				objectTranslator.PushFOWSystemLOSChecks(L, FOWSystem.LOSChecks.EveryUpdate);
				break;
			}
			return Lua.luaL_error(L, "invalid string for FOWSystem.LOSChecks!");
		default:
			return Lua.luaL_error(L, "invalid lua type for FOWSystem.LOSChecks! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
