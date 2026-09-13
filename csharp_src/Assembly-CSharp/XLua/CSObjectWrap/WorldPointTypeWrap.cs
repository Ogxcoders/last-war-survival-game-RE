using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldPointTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(WorldPointType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(WorldPointType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(WorldPointType), L, null, 58, 0, 0);
		Utils.RegisterEnumType(L, typeof(WorldPointType));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(WorldPointType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushWorldPointType(L, (WorldPointType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(WorldPointType), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(WorldPointType), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for WorldPointType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
