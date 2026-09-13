using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIScrollRectMovementTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ScrollRect.MovementType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ScrollRect.MovementType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ScrollRect.MovementType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Unrestricted", ScrollRect.MovementType.Unrestricted);
		Utils.RegisterObject(L, translator, -4, "Elastic", ScrollRect.MovementType.Elastic);
		Utils.RegisterObject(L, translator, -4, "Clamped", ScrollRect.MovementType.Clamped);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ScrollRect.MovementType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIScrollRectMovementType(L, (ScrollRect.MovementType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Unrestricted"))
			{
				objectTranslator.PushUnityEngineUIScrollRectMovementType(L, ScrollRect.MovementType.Unrestricted);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Elastic"))
			{
				objectTranslator.PushUnityEngineUIScrollRectMovementType(L, ScrollRect.MovementType.Elastic);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Clamped"))
			{
				objectTranslator.PushUnityEngineUIScrollRectMovementType(L, ScrollRect.MovementType.Clamped);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.ScrollRect.MovementType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.ScrollRect.MovementType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
