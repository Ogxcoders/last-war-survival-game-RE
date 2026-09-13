using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineTextAnchorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TextAnchor), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TextAnchor), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TextAnchor), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "UpperLeft", TextAnchor.UpperLeft);
		Utils.RegisterObject(L, translator, -4, "UpperCenter", TextAnchor.UpperCenter);
		Utils.RegisterObject(L, translator, -4, "UpperRight", TextAnchor.UpperRight);
		Utils.RegisterObject(L, translator, -4, "MiddleLeft", TextAnchor.MiddleLeft);
		Utils.RegisterObject(L, translator, -4, "MiddleCenter", TextAnchor.MiddleCenter);
		Utils.RegisterObject(L, translator, -4, "MiddleRight", TextAnchor.MiddleRight);
		Utils.RegisterObject(L, translator, -4, "LowerLeft", TextAnchor.LowerLeft);
		Utils.RegisterObject(L, translator, -4, "LowerCenter", TextAnchor.LowerCenter);
		Utils.RegisterObject(L, translator, -4, "LowerRight", TextAnchor.LowerRight);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TextAnchor), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineTextAnchor(L, (TextAnchor)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "UpperLeft"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.UpperLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UpperCenter"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.UpperCenter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UpperRight"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.UpperRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MiddleLeft"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.MiddleLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MiddleCenter"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.MiddleCenter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MiddleRight"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.MiddleRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "LowerLeft"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.LowerLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "LowerCenter"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.LowerCenter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "LowerRight"))
			{
				objectTranslator.PushUnityEngineTextAnchor(L, TextAnchor.LowerRight);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.TextAnchor!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.TextAnchor! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
