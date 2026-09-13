using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BuildingStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(BuildingState), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(BuildingState), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(BuildingState), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", BuildingState.None);
		Utils.RegisterObject(L, translator, -4, "FindingPathState", BuildingState.FindingPathState);
		Utils.RegisterObject(L, translator, -4, "PrepareBuild", BuildingState.PrepareBuild);
		Utils.RegisterObject(L, translator, -4, "Building", BuildingState.Building);
		Utils.RegisterObject(L, translator, -4, "Upgrading", BuildingState.Upgrading);
		Utils.RegisterObject(L, translator, -4, "Idle", BuildingState.Idle);
		Utils.RegisterObject(L, translator, -4, "Moving", BuildingState.Moving);
		Utils.RegisterObject(L, translator, -4, "Producting", BuildingState.Producting);
		Utils.RegisterObject(L, translator, -4, "WaitCollecting", BuildingState.WaitCollecting);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(BuildingState), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushBuildingState(L, (BuildingState)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FindingPathState"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.FindingPathState);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PrepareBuild"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.PrepareBuild);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Building"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.Building);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Upgrading"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.Upgrading);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Idle"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.Idle);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Moving"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.Moving);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Producting"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.Producting);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "WaitCollecting"))
			{
				objectTranslator.PushBuildingState(L, BuildingState.WaitCollecting);
				break;
			}
			return Lua.luaL_error(L, "invalid string for BuildingState!");
		default:
			return Lua.luaL_error(L, "invalid lua type for BuildingState! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
