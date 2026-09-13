using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourceTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ResourceType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ResourceType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ResourceType), L, null, 8, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", ResourceType.None);
		Utils.RegisterObject(L, translator, -4, "Oil", ResourceType.Oil);
		Utils.RegisterObject(L, translator, -4, "Metal", ResourceType.Metal);
		Utils.RegisterObject(L, translator, -4, "Water", ResourceType.Water);
		Utils.RegisterObject(L, translator, -4, "Electricity", ResourceType.Electricity);
		Utils.RegisterObject(L, translator, -4, "Food", ResourceType.Food);
		Utils.RegisterObject(L, translator, -4, "GOLD", ResourceType.GOLD);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ResourceType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushResourceType(L, (ResourceType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushResourceType(L, ResourceType.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Oil"))
			{
				objectTranslator.PushResourceType(L, ResourceType.Oil);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Metal"))
			{
				objectTranslator.PushResourceType(L, ResourceType.Metal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Water"))
			{
				objectTranslator.PushResourceType(L, ResourceType.Water);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Electricity"))
			{
				objectTranslator.PushResourceType(L, ResourceType.Electricity);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Food"))
			{
				objectTranslator.PushResourceType(L, ResourceType.Food);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "GOLD"))
			{
				objectTranslator.PushResourceType(L, ResourceType.GOLD);
				break;
			}
			return Lua.luaL_error(L, "invalid string for ResourceType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for ResourceType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
