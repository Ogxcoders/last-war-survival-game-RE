using System;
using GameFramework.Localization;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameFrameworkLocalizationLanguageWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Language), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Language), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Language), L, null, 52, 0, 0);
		Utils.RegisterEnumType(L, typeof(Language));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Language), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushGameFrameworkLocalizationLanguage(L, (Language)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(Language), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(Language), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for GameFramework.Localization.Language! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
