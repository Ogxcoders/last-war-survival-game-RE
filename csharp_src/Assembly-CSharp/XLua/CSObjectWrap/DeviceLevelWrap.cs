using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DeviceLevelWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(DeviceLevel), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(DeviceLevel), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(DeviceLevel), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "UnInitialized", DeviceLevel.UnInitialized);
		Utils.RegisterObject(L, translator, -4, "Unknown", DeviceLevel.Unknown);
		Utils.RegisterObject(L, translator, -4, "UltraLow", DeviceLevel.UltraLow);
		Utils.RegisterObject(L, translator, -4, "Low", DeviceLevel.Low);
		Utils.RegisterObject(L, translator, -4, "MidLow", DeviceLevel.MidLow);
		Utils.RegisterObject(L, translator, -4, "Mid", DeviceLevel.Mid);
		Utils.RegisterObject(L, translator, -4, "MidHigh", DeviceLevel.MidHigh);
		Utils.RegisterObject(L, translator, -4, "High", DeviceLevel.High);
		Utils.RegisterObject(L, translator, -4, "UltraHigh", DeviceLevel.UltraHigh);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(DeviceLevel), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushDeviceLevel(L, (DeviceLevel)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "UnInitialized"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.UnInitialized);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Unknown"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.Unknown);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UltraLow"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.UltraLow);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Low"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.Low);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MidLow"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.MidLow);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Mid"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.Mid);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MidHigh"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.MidHigh);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "High"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.High);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UltraHigh"))
			{
				objectTranslator.PushDeviceLevel(L, DeviceLevel.UltraHigh);
				break;
			}
			return Lua.luaL_error(L, "invalid string for DeviceLevel!");
		default:
			return Lua.luaL_error(L, "invalid lua type for DeviceLevel! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
