using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldMeteoritePointMeteoritePointStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(WorldMeteoritePoint.MeteoritePointState), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(WorldMeteoritePoint.MeteoritePointState), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(WorldMeteoritePoint.MeteoritePointState), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Default", WorldMeteoritePoint.MeteoritePointState.Default);
		Utils.RegisterObject(L, translator, -4, "Prepare", WorldMeteoritePoint.MeteoritePointState.Prepare);
		Utils.RegisterObject(L, translator, -4, "Drop", WorldMeteoritePoint.MeteoritePointState.Drop);
		Utils.RegisterObject(L, translator, -4, "Collecting", WorldMeteoritePoint.MeteoritePointState.Collecting);
		Utils.RegisterObject(L, translator, -4, "Idle", WorldMeteoritePoint.MeteoritePointState.Idle);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(WorldMeteoritePoint.MeteoritePointState), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, (WorldMeteoritePoint.MeteoritePointState)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Default"))
			{
				objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, WorldMeteoritePoint.MeteoritePointState.Default);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Prepare"))
			{
				objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, WorldMeteoritePoint.MeteoritePointState.Prepare);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Drop"))
			{
				objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, WorldMeteoritePoint.MeteoritePointState.Drop);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Collecting"))
			{
				objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, WorldMeteoritePoint.MeteoritePointState.Collecting);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Idle"))
			{
				objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, WorldMeteoritePoint.MeteoritePointState.Idle);
				break;
			}
			return Lua.luaL_error(L, "invalid string for WorldMeteoritePoint.MeteoritePointState!");
		default:
			return Lua.luaL_error(L, "invalid lua type for WorldMeteoritePoint.MeteoritePointState! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
