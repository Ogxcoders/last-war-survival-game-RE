using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NewMarchTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(NewMarchType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(NewMarchType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(NewMarchType), L, null, 46, 0, 0);
		Utils.RegisterEnumType(L, typeof(NewMarchType));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(NewMarchType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushNewMarchType(L, (NewMarchType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(NewMarchType), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(NewMarchType), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for NewMarchType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
