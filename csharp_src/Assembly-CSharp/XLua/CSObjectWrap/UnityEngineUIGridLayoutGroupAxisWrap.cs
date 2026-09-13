using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIGridLayoutGroupAxisWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(GridLayoutGroup.Axis), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(GridLayoutGroup.Axis), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(GridLayoutGroup.Axis), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Horizontal", GridLayoutGroup.Axis.Horizontal);
		Utils.RegisterObject(L, translator, -4, "Vertical", GridLayoutGroup.Axis.Vertical);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(GridLayoutGroup.Axis), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIGridLayoutGroupAxis(L, (GridLayoutGroup.Axis)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Horizontal"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupAxis(L, GridLayoutGroup.Axis.Horizontal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Vertical"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupAxis(L, GridLayoutGroup.Axis.Vertical);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.GridLayoutGroup.Axis!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.GridLayoutGroup.Axis! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
