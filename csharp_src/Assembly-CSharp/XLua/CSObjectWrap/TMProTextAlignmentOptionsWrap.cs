using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTextAlignmentOptionsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TextAlignmentOptions), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TextAlignmentOptions), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TextAlignmentOptions), L, null, 38, 0, 0);
		Utils.RegisterEnumType(L, typeof(TextAlignmentOptions));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TextAlignmentOptions), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTMProTextAlignmentOptions(L, (TextAlignmentOptions)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(TextAlignmentOptions), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(TextAlignmentOptions), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for TMPro.TextAlignmentOptions! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
