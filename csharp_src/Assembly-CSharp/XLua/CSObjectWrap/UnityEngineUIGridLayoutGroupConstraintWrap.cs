using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIGridLayoutGroupConstraintWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(GridLayoutGroup.Constraint), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(GridLayoutGroup.Constraint), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(GridLayoutGroup.Constraint), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Flexible", GridLayoutGroup.Constraint.Flexible);
		Utils.RegisterObject(L, translator, -4, "FixedColumnCount", GridLayoutGroup.Constraint.FixedColumnCount);
		Utils.RegisterObject(L, translator, -4, "FixedRowCount", GridLayoutGroup.Constraint.FixedRowCount);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(GridLayoutGroup.Constraint), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIGridLayoutGroupConstraint(L, (GridLayoutGroup.Constraint)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Flexible"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupConstraint(L, GridLayoutGroup.Constraint.Flexible);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FixedColumnCount"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupConstraint(L, GridLayoutGroup.Constraint.FixedColumnCount);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FixedRowCount"))
			{
				objectTranslator.PushUnityEngineUIGridLayoutGroupConstraint(L, GridLayoutGroup.Constraint.FixedRowCount);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.GridLayoutGroup.Constraint!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.GridLayoutGroup.Constraint! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
