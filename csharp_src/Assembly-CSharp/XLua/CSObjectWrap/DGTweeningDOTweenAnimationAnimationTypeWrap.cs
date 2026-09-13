using System;
using DG.Tweening;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningDOTweenAnimationAnimationTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(DOTweenAnimation.AnimationType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(DOTweenAnimation.AnimationType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(DOTweenAnimation.AnimationType), L, null, 23, 0, 0);
		Utils.RegisterEnumType(L, typeof(DOTweenAnimation.AnimationType));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(DOTweenAnimation.AnimationType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushDGTweeningDOTweenAnimationAnimationType(L, (DOTweenAnimation.AnimationType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(DOTweenAnimation.AnimationType), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(DOTweenAnimation.AnimationType), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for DG.Tweening.DOTweenAnimation.AnimationType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
