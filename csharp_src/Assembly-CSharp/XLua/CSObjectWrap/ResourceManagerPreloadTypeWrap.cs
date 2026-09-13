using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourceManagerPreloadTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ResourceManager.PreloadType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ResourceManager.PreloadType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ResourceManager.PreloadType), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Cache", ResourceManager.PreloadType.Cache);
		Utils.RegisterObject(L, translator, -4, "KeepAlive", ResourceManager.PreloadType.KeepAlive);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ResourceManager.PreloadType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushResourceManagerPreloadType(L, (ResourceManager.PreloadType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Cache"))
			{
				objectTranslator.PushResourceManagerPreloadType(L, ResourceManager.PreloadType.Cache);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "KeepAlive"))
			{
				objectTranslator.PushResourceManagerPreloadType(L, ResourceManager.PreloadType.KeepAlive);
				break;
			}
			return Lua.luaL_error(L, "invalid string for ResourceManager.PreloadType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for ResourceManager.PreloadType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
