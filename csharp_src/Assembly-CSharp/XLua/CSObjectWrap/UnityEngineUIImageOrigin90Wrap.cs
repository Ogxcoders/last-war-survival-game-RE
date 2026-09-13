using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageOrigin90Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Image.Origin90), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Image.Origin90), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Image.Origin90), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "BottomLeft", Image.Origin90.BottomLeft);
		Utils.RegisterObject(L, translator, -4, "TopLeft", Image.Origin90.TopLeft);
		Utils.RegisterObject(L, translator, -4, "TopRight", Image.Origin90.TopRight);
		Utils.RegisterObject(L, translator, -4, "BottomRight", Image.Origin90.BottomRight);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Image.Origin90), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIImageOrigin90(L, (Image.Origin90)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "BottomLeft"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin90(L, Image.Origin90.BottomLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "TopLeft"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin90(L, Image.Origin90.TopLeft);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "TopRight"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin90(L, Image.Origin90.TopRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BottomRight"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin90(L, Image.Origin90.BottomRight);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Image.Origin90!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Origin90! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
