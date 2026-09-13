using System;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalAntialiasingModeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(AntialiasingMode), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(AntialiasingMode), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(AntialiasingMode), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", AntialiasingMode.None);
		Utils.RegisterObject(L, translator, -4, "FastApproximateAntialiasing", AntialiasingMode.FastApproximateAntialiasing);
		Utils.RegisterObject(L, translator, -4, "SubpixelMorphologicalAntiAliasing", AntialiasingMode.SubpixelMorphologicalAntiAliasing);
		Utils.RegisterObject(L, translator, -4, "UsePipelineSettings", AntialiasingMode.UsePipelineSettings);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(AntialiasingMode), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, (AntialiasingMode)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, AntialiasingMode.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FastApproximateAntialiasing"))
			{
				objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, AntialiasingMode.FastApproximateAntialiasing);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SubpixelMorphologicalAntiAliasing"))
			{
				objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, AntialiasingMode.SubpixelMorphologicalAntiAliasing);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UsePipelineSettings"))
			{
				objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, AntialiasingMode.UsePipelineSettings);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Rendering.Universal.AntialiasingMode!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Rendering.Universal.AntialiasingMode! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
