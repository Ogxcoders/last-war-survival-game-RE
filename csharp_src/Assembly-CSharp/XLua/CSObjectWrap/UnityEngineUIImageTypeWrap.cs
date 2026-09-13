using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Image.Type), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Image.Type), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Image.Type), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Simple", Image.Type.Simple);
		Utils.RegisterObject(L, translator, -4, "Sliced", Image.Type.Sliced);
		Utils.RegisterObject(L, translator, -4, "Tiled", Image.Type.Tiled);
		Utils.RegisterObject(L, translator, -4, "Filled", Image.Type.Filled);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Image.Type), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIImageType(L, (Image.Type)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Simple"))
			{
				objectTranslator.PushUnityEngineUIImageType(L, Image.Type.Simple);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Sliced"))
			{
				objectTranslator.PushUnityEngineUIImageType(L, Image.Type.Sliced);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Tiled"))
			{
				objectTranslator.PushUnityEngineUIImageType(L, Image.Type.Tiled);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Filled"))
			{
				objectTranslator.PushUnityEngineUIImageType(L, Image.Type.Filled);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Image.Type!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Type! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
