using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FakeModelManagerTempRoadTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(FakeModelManager.TempRoadType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(FakeModelManager.TempRoadType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(FakeModelManager.TempRoadType), L, null, 7, 0, 0);
		Utils.RegisterObject(L, translator, -4, "MakeRoad", FakeModelManager.TempRoadType.MakeRoad);
		Utils.RegisterObject(L, translator, -4, "DeleteRoad", FakeModelManager.TempRoadType.DeleteRoad);
		Utils.RegisterObject(L, translator, -4, "Collect", FakeModelManager.TempRoadType.Collect);
		Utils.RegisterObject(L, translator, -4, "CreateBoard", FakeModelManager.TempRoadType.CreateBoard);
		Utils.RegisterObject(L, translator, -4, "StartPoint", FakeModelManager.TempRoadType.StartPoint);
		Utils.RegisterObject(L, translator, -4, "Green", FakeModelManager.TempRoadType.Green);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(FakeModelManager.TempRoadType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushFakeModelManagerTempRoadType(L, (FakeModelManager.TempRoadType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "MakeRoad"))
			{
				objectTranslator.PushFakeModelManagerTempRoadType(L, FakeModelManager.TempRoadType.MakeRoad);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "DeleteRoad"))
			{
				objectTranslator.PushFakeModelManagerTempRoadType(L, FakeModelManager.TempRoadType.DeleteRoad);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Collect"))
			{
				objectTranslator.PushFakeModelManagerTempRoadType(L, FakeModelManager.TempRoadType.Collect);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CreateBoard"))
			{
				objectTranslator.PushFakeModelManagerTempRoadType(L, FakeModelManager.TempRoadType.CreateBoard);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "StartPoint"))
			{
				objectTranslator.PushFakeModelManagerTempRoadType(L, FakeModelManager.TempRoadType.StartPoint);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Green"))
			{
				objectTranslator.PushFakeModelManagerTempRoadType(L, FakeModelManager.TempRoadType.Green);
				break;
			}
			return Lua.luaL_error(L, "invalid string for FakeModelManager.TempRoadType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for FakeModelManager.TempRoadType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
