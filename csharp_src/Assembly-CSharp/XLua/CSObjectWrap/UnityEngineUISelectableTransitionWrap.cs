using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUISelectableTransitionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Selectable.Transition), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Selectable.Transition), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Selectable.Transition), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", Selectable.Transition.None);
		Utils.RegisterObject(L, translator, -4, "ColorTint", Selectable.Transition.ColorTint);
		Utils.RegisterObject(L, translator, -4, "SpriteSwap", Selectable.Transition.SpriteSwap);
		Utils.RegisterObject(L, translator, -4, "Animation", Selectable.Transition.Animation);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Selectable.Transition), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUISelectableTransition(L, (Selectable.Transition)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushUnityEngineUISelectableTransition(L, Selectable.Transition.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "ColorTint"))
			{
				objectTranslator.PushUnityEngineUISelectableTransition(L, Selectable.Transition.ColorTint);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SpriteSwap"))
			{
				objectTranslator.PushUnityEngineUISelectableTransition(L, Selectable.Transition.SpriteSwap);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Animation"))
			{
				objectTranslator.PushUnityEngineUISelectableTransition(L, Selectable.Transition.Animation);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Selectable.Transition!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Selectable.Transition! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
