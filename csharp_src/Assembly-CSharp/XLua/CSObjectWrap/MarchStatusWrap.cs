using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MarchStatusWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(MarchStatus), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(MarchStatus), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(MarchStatus), L, null, 28, 0, 0);
		Utils.RegisterEnumType(L, typeof(MarchStatus));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(MarchStatus), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushMarchStatus(L, (MarchStatus)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(MarchStatus), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(MarchStatus), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for MarchStatus! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
