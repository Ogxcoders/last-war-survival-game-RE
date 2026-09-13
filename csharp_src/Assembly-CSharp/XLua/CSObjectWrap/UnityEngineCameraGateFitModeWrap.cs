using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCameraGateFitModeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Camera.GateFitMode), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Camera.GateFitMode), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Camera.GateFitMode), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Vertical", Camera.GateFitMode.Vertical);
		Utils.RegisterObject(L, translator, -4, "Horizontal", Camera.GateFitMode.Horizontal);
		Utils.RegisterObject(L, translator, -4, "Fill", Camera.GateFitMode.Fill);
		Utils.RegisterObject(L, translator, -4, "Overscan", Camera.GateFitMode.Overscan);
		Utils.RegisterObject(L, translator, -4, "None", Camera.GateFitMode.None);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Camera.GateFitMode), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineCameraGateFitMode(L, (Camera.GateFitMode)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Vertical"))
			{
				objectTranslator.PushUnityEngineCameraGateFitMode(L, Camera.GateFitMode.Vertical);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Horizontal"))
			{
				objectTranslator.PushUnityEngineCameraGateFitMode(L, Camera.GateFitMode.Horizontal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Fill"))
			{
				objectTranslator.PushUnityEngineCameraGateFitMode(L, Camera.GateFitMode.Fill);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Overscan"))
			{
				objectTranslator.PushUnityEngineCameraGateFitMode(L, Camera.GateFitMode.Overscan);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushUnityEngineCameraGateFitMode(L, Camera.GateFitMode.None);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Camera.GateFitMode!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Camera.GateFitMode! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
