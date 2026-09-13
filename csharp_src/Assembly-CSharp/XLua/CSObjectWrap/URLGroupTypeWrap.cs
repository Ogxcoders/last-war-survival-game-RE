using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class URLGroupTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(URLGroupType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(URLGroupType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(URLGroupType), L, null, 8, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Online", URLGroupType.Online);
		Utils.RegisterObject(L, translator, -4, "PressureTest", URLGroupType.PressureTest);
		Utils.RegisterObject(L, translator, -4, "Vietnam", URLGroupType.Vietnam);
		Utils.RegisterObject(L, translator, -4, "Tencent", URLGroupType.Tencent);
		Utils.RegisterObject(L, translator, -4, "GCP", URLGroupType.GCP);
		Utils.RegisterObject(L, translator, -4, "Local", URLGroupType.Local);
		Utils.RegisterObject(L, translator, -4, "AWS", URLGroupType.AWS);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(URLGroupType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushURLGroupType(L, (URLGroupType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Online"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.Online);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PressureTest"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.PressureTest);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Vietnam"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.Vietnam);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Tencent"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.Tencent);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "GCP"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.GCP);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Local"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.Local);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AWS"))
			{
				objectTranslator.PushURLGroupType(L, URLGroupType.AWS);
				break;
			}
			return Lua.luaL_error(L, "invalid string for URLGroupType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for URLGroupType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
