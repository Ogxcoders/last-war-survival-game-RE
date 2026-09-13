using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageOrigin360Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Image.Origin360), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Image.Origin360), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Image.Origin360), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Bottom", Image.Origin360.Bottom);
		Utils.RegisterObject(L, translator, -4, "Right", Image.Origin360.Right);
		Utils.RegisterObject(L, translator, -4, "Top", Image.Origin360.Top);
		Utils.RegisterObject(L, translator, -4, "Left", Image.Origin360.Left);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Image.Origin360), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIImageOrigin360(L, (Image.Origin360)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Bottom"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin360(L, Image.Origin360.Bottom);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin360(L, Image.Origin360.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin360(L, Image.Origin360.Top);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin360(L, Image.Origin360.Left);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Image.Origin360!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Origin360! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
