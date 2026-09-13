using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCameraMonoOrStereoscopicEyeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Camera.MonoOrStereoscopicEye), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Camera.MonoOrStereoscopicEye), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Camera.MonoOrStereoscopicEye), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Left", Camera.MonoOrStereoscopicEye.Left);
		Utils.RegisterObject(L, translator, -4, "Right", Camera.MonoOrStereoscopicEye.Right);
		Utils.RegisterObject(L, translator, -4, "Mono", Camera.MonoOrStereoscopicEye.Mono);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Camera.MonoOrStereoscopicEye), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineCameraMonoOrStereoscopicEye(L, (Camera.MonoOrStereoscopicEye)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushUnityEngineCameraMonoOrStereoscopicEye(L, Camera.MonoOrStereoscopicEye.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushUnityEngineCameraMonoOrStereoscopicEye(L, Camera.MonoOrStereoscopicEye.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Mono"))
			{
				objectTranslator.PushUnityEngineCameraMonoOrStereoscopicEye(L, Camera.MonoOrStereoscopicEye.Mono);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Camera.MonoOrStereoscopicEye!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Camera.MonoOrStereoscopicEye! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
