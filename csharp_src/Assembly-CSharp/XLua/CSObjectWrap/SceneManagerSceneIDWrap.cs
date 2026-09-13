using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneManagerSceneIDWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(SceneManager.SceneID), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(SceneManager.SceneID), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(SceneManager.SceneID), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", SceneManager.SceneID.None);
		Utils.RegisterObject(L, translator, -4, "City", SceneManager.SceneID.City);
		Utils.RegisterObject(L, translator, -4, "World", SceneManager.SceneID.World);
		Utils.RegisterObject(L, translator, -4, "PVE", SceneManager.SceneID.PVE);
		Utils.RegisterObject(L, translator, -4, "Custom", SceneManager.SceneID.Custom);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(SceneManager.SceneID), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushSceneManagerSceneID(L, (SceneManager.SceneID)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushSceneManagerSceneID(L, SceneManager.SceneID.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "City"))
			{
				objectTranslator.PushSceneManagerSceneID(L, SceneManager.SceneID.City);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "World"))
			{
				objectTranslator.PushSceneManagerSceneID(L, SceneManager.SceneID.World);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PVE"))
			{
				objectTranslator.PushSceneManagerSceneID(L, SceneManager.SceneID.PVE);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Custom"))
			{
				objectTranslator.PushSceneManagerSceneID(L, SceneManager.SceneID.Custom);
				break;
			}
			return Lua.luaL_error(L, "invalid string for SceneManager.SceneID!");
		default:
			return Lua.luaL_error(L, "invalid lua type for SceneManager.SceneID! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
