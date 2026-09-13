using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineTouchPhaseWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TouchPhase), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TouchPhase), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TouchPhase), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Began", TouchPhase.Began);
		Utils.RegisterObject(L, translator, -4, "Moved", TouchPhase.Moved);
		Utils.RegisterObject(L, translator, -4, "Stationary", TouchPhase.Stationary);
		Utils.RegisterObject(L, translator, -4, "Ended", TouchPhase.Ended);
		Utils.RegisterObject(L, translator, -4, "Canceled", TouchPhase.Canceled);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TouchPhase), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineTouchPhase(L, (TouchPhase)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Began"))
			{
				objectTranslator.PushUnityEngineTouchPhase(L, TouchPhase.Began);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Moved"))
			{
				objectTranslator.PushUnityEngineTouchPhase(L, TouchPhase.Moved);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Stationary"))
			{
				objectTranslator.PushUnityEngineTouchPhase(L, TouchPhase.Stationary);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Ended"))
			{
				objectTranslator.PushUnityEngineTouchPhase(L, TouchPhase.Ended);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Canceled"))
			{
				objectTranslator.PushUnityEngineTouchPhase(L, TouchPhase.Canceled);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.TouchPhase!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.TouchPhase! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
