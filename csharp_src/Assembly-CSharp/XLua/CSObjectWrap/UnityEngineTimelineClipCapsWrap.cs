using System;
using UnityEngine.Timeline;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineTimelineClipCapsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ClipCaps), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ClipCaps), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ClipCaps), L, null, 9, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", ClipCaps.None);
		Utils.RegisterObject(L, translator, -4, "Looping", ClipCaps.Looping);
		Utils.RegisterObject(L, translator, -4, "Extrapolation", ClipCaps.Extrapolation);
		Utils.RegisterObject(L, translator, -4, "ClipIn", ClipCaps.ClipIn);
		Utils.RegisterObject(L, translator, -4, "SpeedMultiplier", ClipCaps.SpeedMultiplier);
		Utils.RegisterObject(L, translator, -4, "Blending", ClipCaps.Blending);
		Utils.RegisterObject(L, translator, -4, "AutoScale", ClipCaps.AutoScale);
		Utils.RegisterObject(L, translator, -4, "All", ClipCaps.All);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ClipCaps), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineTimelineClipCaps(L, (ClipCaps)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Looping"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.Looping);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Extrapolation"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.Extrapolation);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "ClipIn"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.ClipIn);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SpeedMultiplier"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.SpeedMultiplier);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Blending"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.Blending);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AutoScale"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.AutoScale);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "All"))
			{
				objectTranslator.PushUnityEngineTimelineClipCaps(L, ClipCaps.All);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.Timeline.ClipCaps!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.Timeline.ClipCaps! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
