using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineAnimatorCullingModeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(AnimatorCullingMode), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(AnimatorCullingMode), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(AnimatorCullingMode), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "AlwaysAnimate", AnimatorCullingMode.AlwaysAnimate);
		Utils.RegisterObject(L, translator, -4, "CullUpdateTransforms", AnimatorCullingMode.CullUpdateTransforms);
		Utils.RegisterObject(L, translator, -4, "CullCompletely", AnimatorCullingMode.CullCompletely);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(AnimatorCullingMode), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineAnimatorCullingMode(L, (AnimatorCullingMode)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "AlwaysAnimate"))
			{
				objectTranslator.PushUnityEngineAnimatorCullingMode(L, AnimatorCullingMode.AlwaysAnimate);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CullUpdateTransforms"))
			{
				objectTranslator.PushUnityEngineAnimatorCullingMode(L, AnimatorCullingMode.CullUpdateTransforms);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CullCompletely"))
			{
				objectTranslator.PushUnityEngineAnimatorCullingMode(L, AnimatorCullingMode.CullCompletely);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.AnimatorCullingMode!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.AnimatorCullingMode! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
