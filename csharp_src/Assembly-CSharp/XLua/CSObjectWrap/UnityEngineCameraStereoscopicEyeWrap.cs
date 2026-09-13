using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCameraStereoscopicEyeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Camera.StereoscopicEye), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Camera.StereoscopicEye), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Camera.StereoscopicEye), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Left", Camera.StereoscopicEye.Left);
		Utils.RegisterObject(L, translator, -4, "Right", Camera.StereoscopicEye.Right);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Camera.StereoscopicEye), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineCameraStereoscopicEye(L, (Camera.StereoscopicEye)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushUnityEngineCameraStereoscopicEye(L, Camera.StereoscopicEye.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushUnityEngineCameraStereoscopicEye(L, Camera.StereoscopicEye.Right);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Camera.StereoscopicEye!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Camera.StereoscopicEye! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
