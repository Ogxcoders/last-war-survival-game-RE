using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIGridLayoutGroupCornerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(GridLayoutGroup.Corner), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(GridLayoutGroup.Corner), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(GridLayoutGroup.Corner), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "UpperLeft", GridLayoutGroup.Corner.UpperLeft);
		Utils.RegisterObject(L, translator, -4, "UpperRight", GridLayoutGroup.Corner.UpperRight);
		Utils.RegisterObject(L, translator, -4, "LowerLeft", GridLayoutGroup.Corner.LowerLeft);
		Utils.RegisterObject(L, translator, -4, "LowerRight", GridLayoutGroup.Corner.LowerRight);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(GridLayoutGroup.Corner), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIGridLayoutGroupCorner(L, (GridLayoutGroup.Corner)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "UpperLeft"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupCorner(L, GridLayoutGroup.Corner.UpperLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UpperRight"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupCorner(L, GridLayoutGroup.Corner.UpperRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "LowerLeft"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupCorner(L, GridLayoutGroup.Corner.LowerLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "LowerRight"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupCorner(L, GridLayoutGroup.Corner.LowerRight);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.GridLayoutGroup.Corner!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.GridLayoutGroup.Corner! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
