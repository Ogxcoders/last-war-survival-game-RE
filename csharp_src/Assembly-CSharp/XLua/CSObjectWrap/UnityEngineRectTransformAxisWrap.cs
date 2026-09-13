using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRectTransformAxisWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(RectTransform.Axis), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(RectTransform.Axis), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(RectTransform.Axis), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Horizontal", RectTransform.Axis.Horizontal);
		Utils.RegisterObject(L, translator, -4, "Vertical", RectTransform.Axis.Vertical);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(RectTransform.Axis), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineRectTransformAxis(L, (RectTransform.Axis)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Horizontal"))
			{
				objectTranslator.PushUnityEngineRectTransformAxis(L, RectTransform.Axis.Horizontal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Vertical"))
			{
				objectTranslator.PushUnityEngineRectTransformAxis(L, RectTransform.Axis.Vertical);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.RectTransform.Axis!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.RectTransform.Axis! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
