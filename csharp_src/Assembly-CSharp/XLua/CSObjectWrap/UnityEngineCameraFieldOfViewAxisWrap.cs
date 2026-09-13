using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCameraFieldOfViewAxisWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Camera.FieldOfViewAxis), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Camera.FieldOfViewAxis), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Camera.FieldOfViewAxis), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Vertical", Camera.FieldOfViewAxis.Vertical);
		Utils.RegisterObject(L, translator, -4, "Horizontal", Camera.FieldOfViewAxis.Horizontal);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Camera.FieldOfViewAxis), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineCameraFieldOfViewAxis(L, (Camera.FieldOfViewAxis)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Vertical"))
			{
				objectTranslator.PushUnityEngineCameraFieldOfViewAxis(L, Camera.FieldOfViewAxis.Vertical);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Horizontal"))
			{
				objectTranslator.PushUnityEngineCameraFieldOfViewAxis(L, Camera.FieldOfViewAxis.Horizontal);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Camera.FieldOfViewAxis!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Camera.FieldOfViewAxis! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
