using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRectTransformEdgeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(RectTransform.Edge), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(RectTransform.Edge), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(RectTransform.Edge), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Left", RectTransform.Edge.Left);
		Utils.RegisterObject(L, translator, -4, "Right", RectTransform.Edge.Right);
		Utils.RegisterObject(L, translator, -4, "Top", RectTransform.Edge.Top);
		Utils.RegisterObject(L, translator, -4, "Bottom", RectTransform.Edge.Bottom);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(RectTransform.Edge), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineRectTransformEdge(L, (RectTransform.Edge)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushUnityEngineRectTransformEdge(L, RectTransform.Edge.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushUnityEngineRectTransformEdge(L, RectTransform.Edge.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushUnityEngineRectTransformEdge(L, RectTransform.Edge.Top);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Bottom"))
			{
				objectTranslator.PushUnityEngineRectTransformEdge(L, RectTransform.Edge.Bottom);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.RectTransform.Edge!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.RectTransform.Edge! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
