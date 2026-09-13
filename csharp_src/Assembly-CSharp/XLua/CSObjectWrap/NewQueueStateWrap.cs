using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NewQueueStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(NewQueueState), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(NewQueueState), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(NewQueueState), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Free", NewQueueState.Free);
		Utils.RegisterObject(L, translator, -4, "Prepare", NewQueueState.Prepare);
		Utils.RegisterObject(L, translator, -4, "Work", NewQueueState.Work);
		Utils.RegisterObject(L, translator, -4, "Finish", NewQueueState.Finish);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(NewQueueState), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushNewQueueState(L, (NewQueueState)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Free"))
			{
				objectTranslator.PushNewQueueState(L, NewQueueState.Free);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Prepare"))
			{
				objectTranslator.PushNewQueueState(L, NewQueueState.Prepare);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Work"))
			{
				objectTranslator.PushNewQueueState(L, NewQueueState.Work);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Finish"))
			{
				objectTranslator.PushNewQueueState(L, NewQueueState.Finish);
				break;
			}
			return Lua.luaL_error(L, "invalid string for NewQueueState!");
		default:
			return Lua.luaL_error(L, "invalid lua type for NewQueueState! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
