using System;
using DG.Tweening;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningEaseWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Ease), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Ease), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Ease), L, null, 39, 0, 0);
		Utils.RegisterEnumType(L, typeof(Ease));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Ease), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushDGTweeningEase(L, (Ease)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(Ease), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(Ease), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for DG.Tweening.Ease! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
