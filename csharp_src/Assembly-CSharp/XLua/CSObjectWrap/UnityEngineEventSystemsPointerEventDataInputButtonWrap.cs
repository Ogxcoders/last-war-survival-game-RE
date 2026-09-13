using System;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineEventSystemsPointerEventDataInputButtonWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(PointerEventData.InputButton), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(PointerEventData.InputButton), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(PointerEventData.InputButton), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Left", PointerEventData.InputButton.Left);
		Utils.RegisterObject(L, translator, -4, "Right", PointerEventData.InputButton.Right);
		Utils.RegisterObject(L, translator, -4, "Middle", PointerEventData.InputButton.Middle);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(PointerEventData.InputButton), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineEventSystemsPointerEventDataInputButton(L, (PointerEventData.InputButton)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataInputButton(L, PointerEventData.InputButton.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataInputButton(L, PointerEventData.InputButton.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Middle"))
			{
				objectTranslator.PushUnityEngineEventSystemsPointerEventDataInputButton(L, PointerEventData.InputButton.Middle);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.EventSystems.PointerEventData.InputButton!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.EventSystems.PointerEventData.InputButton! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
