using System;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineEventSystemsPointerEventDataFramePressStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(PointerEventData.FramePressState), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(PointerEventData.FramePressState), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(PointerEventData.FramePressState), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Pressed", PointerEventData.FramePressState.Pressed);
		Utils.RegisterObject(L, translator, -4, "Released", PointerEventData.FramePressState.Released);
		Utils.RegisterObject(L, translator, -4, "PressedAndReleased", PointerEventData.FramePressState.PressedAndReleased);
		Utils.RegisterObject(L, translator, -4, "NotChanged", PointerEventData.FramePressState.NotChanged);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(PointerEventData.FramePressState), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineEventSystemsPointerEventDataFramePressState(L, (PointerEventData.FramePressState)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Pressed"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataFramePressState(L, PointerEventData.FramePressState.Pressed);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Released"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataFramePressState(L, PointerEventData.FramePressState.Released);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PressedAndReleased"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataFramePressState(L, PointerEventData.FramePressState.PressedAndReleased);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "NotChanged"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataFramePressState(L, PointerEventData.FramePressState.NotChanged);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.EventSystems.PointerEventData.FramePressState!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.EventSystems.PointerEventData.FramePressState! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
