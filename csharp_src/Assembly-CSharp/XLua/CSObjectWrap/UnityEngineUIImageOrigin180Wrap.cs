using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageOrigin180Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Image.Origin180), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Image.Origin180), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Image.Origin180), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Bottom", Image.Origin180.Bottom);
		Utils.RegisterObject(L, translator, -4, "Left", Image.Origin180.Left);
		Utils.RegisterObject(L, translator, -4, "Top", Image.Origin180.Top);
		Utils.RegisterObject(L, translator, -4, "Right", Image.Origin180.Right);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Image.Origin180), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIImageOrigin180(L, (Image.Origin180)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Bottom"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin180(L, Image.Origin180.Bottom);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin180(L, Image.Origin180.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin180(L, Image.Origin180.Top);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushUnityEngineUIImageOrigin180(L, Image.Origin180.Right);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Image.Origin180!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Origin180! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
