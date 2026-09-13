using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class InstanceRequestStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(InstanceRequest.State), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(InstanceRequest.State), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(InstanceRequest.State), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Init", InstanceRequest.State.Init);
		Utils.RegisterObject(L, translator, -4, "Loading", InstanceRequest.State.Loading);
		Utils.RegisterObject(L, translator, -4, "Instanced", InstanceRequest.State.Instanced);
		Utils.RegisterObject(L, translator, -4, "Destroy", InstanceRequest.State.Destroy);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(InstanceRequest.State), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushInstanceRequestState(L, (InstanceRequest.State)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Init"))
			{
				objectTranslator.PushInstanceRequestState(L, InstanceRequest.State.Init);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Loading"))
			{
				objectTranslator.PushInstanceRequestState(L, InstanceRequest.State.Loading);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Instanced"))
			{
				objectTranslator.PushInstanceRequestState(L, InstanceRequest.State.Instanced);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Destroy"))
			{
				objectTranslator.PushInstanceRequestState(L, InstanceRequest.State.Destroy);
				break;
			}
			return Lua.luaL_error(L, "invalid string for InstanceRequest.State!");
		default:
			return Lua.luaL_error(L, "invalid lua type for InstanceRequest.State! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
