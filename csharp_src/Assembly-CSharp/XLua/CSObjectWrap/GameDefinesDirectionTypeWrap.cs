using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesDirectionTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(GameDefines.DirectionType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(GameDefines.DirectionType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(GameDefines.DirectionType), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Top", GameDefines.DirectionType.Top);
		Utils.RegisterObject(L, translator, -4, "Right", GameDefines.DirectionType.Right);
		Utils.RegisterObject(L, translator, -4, "Left", GameDefines.DirectionType.Left);
		Utils.RegisterObject(L, translator, -4, "Down", GameDefines.DirectionType.Down);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(GameDefines.DirectionType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushGameDefinesDirectionType(L, (GameDefines.DirectionType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushGameDefinesDirectionType(L, GameDefines.DirectionType.Top);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushGameDefinesDirectionType(L, GameDefines.DirectionType.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushGameDefinesDirectionType(L, GameDefines.DirectionType.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Down"))
			{
				objectTranslator.PushGameDefinesDirectionType(L, GameDefines.DirectionType.Down);
				break;
			}
			return Lua.luaL_error(L, "invalid string for GameDefines.DirectionType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for GameDefines.DirectionType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
