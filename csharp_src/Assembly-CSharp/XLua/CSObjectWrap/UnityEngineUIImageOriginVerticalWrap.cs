using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageOriginVerticalWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Image.OriginVertical), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Image.OriginVertical), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Image.OriginVertical), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Bottom", Image.OriginVertical.Bottom);
		Utils.RegisterObject(L, translator, -4, "Top", Image.OriginVertical.Top);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Image.OriginVertical), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIImageOriginVertical(L, (Image.OriginVertical)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Bottom"))
			{
				objectTranslator.PushUnityEngineUIImageOriginVertical(L, Image.OriginVertical.Bottom);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushUnityEngineUIImageOriginVertical(L, Image.OriginVertical.Top);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Image.OriginVertical!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.OriginVertical! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
