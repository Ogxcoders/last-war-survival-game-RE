using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesBuildConnectRoadDirectionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(GameDefines.BuildConnectRoadDirection), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(GameDefines.BuildConnectRoadDirection), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(GameDefines.BuildConnectRoadDirection), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", GameDefines.BuildConnectRoadDirection.None);
		Utils.RegisterObject(L, translator, -4, "Top", GameDefines.BuildConnectRoadDirection.Top);
		Utils.RegisterObject(L, translator, -4, "Right", GameDefines.BuildConnectRoadDirection.Right);
		Utils.RegisterObject(L, translator, -4, "Left", GameDefines.BuildConnectRoadDirection.Left);
		Utils.RegisterObject(L, translator, -4, "Down", GameDefines.BuildConnectRoadDirection.Down);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(GameDefines.BuildConnectRoadDirection), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, (GameDefines.BuildConnectRoadDirection)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, GameDefines.BuildConnectRoadDirection.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, GameDefines.BuildConnectRoadDirection.Top);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, GameDefines.BuildConnectRoadDirection.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, GameDefines.BuildConnectRoadDirection.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Down"))
			{
				objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, GameDefines.BuildConnectRoadDirection.Down);
				break;
			}
			return Lua.luaL_error(L, "invalid string for GameDefines.BuildConnectRoadDirection!");
		default:
			return Lua.luaL_error(L, "invalid lua type for GameDefines.BuildConnectRoadDirection! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
