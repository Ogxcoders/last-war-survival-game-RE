using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FOWSystemStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(FOWSystem.State), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(FOWSystem.State), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(FOWSystem.State), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Blending", FOWSystem.State.Blending);
		Utils.RegisterObject(L, translator, -4, "NeedUpdate", FOWSystem.State.NeedUpdate);
		Utils.RegisterObject(L, translator, -4, "UpdateTexture0", FOWSystem.State.UpdateTexture0);
		Utils.RegisterObject(L, translator, -4, "UpdateTexture1", FOWSystem.State.UpdateTexture1);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(FOWSystem.State), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushFOWSystemState(L, (FOWSystem.State)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Blending"))
			{
				objectTranslator.PushFOWSystemState(L, FOWSystem.State.Blending);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "NeedUpdate"))
			{
				objectTranslator.PushFOWSystemState(L, FOWSystem.State.NeedUpdate);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UpdateTexture0"))
			{
				objectTranslator.PushFOWSystemState(L, FOWSystem.State.UpdateTexture0);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UpdateTexture1"))
			{
				objectTranslator.PushFOWSystemState(L, FOWSystem.State.UpdateTexture1);
				break;
			}
			return Lua.luaL_error(L, "invalid string for FOWSystem.State!");
		default:
			return Lua.luaL_error(L, "invalid lua type for FOWSystem.State! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
