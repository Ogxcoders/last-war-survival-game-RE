using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIImageFillMethodWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(Image.FillMethod), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(Image.FillMethod), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(Image.FillMethod), L, null, 6, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Horizontal", Image.FillMethod.Horizontal);
		Utils.RegisterObject(L, translator, -4, "Vertical", Image.FillMethod.Vertical);
		Utils.RegisterObject(L, translator, -4, "Radial90", Image.FillMethod.Radial90);
		Utils.RegisterObject(L, translator, -4, "Radial180", Image.FillMethod.Radial180);
		Utils.RegisterObject(L, translator, -4, "Radial360", Image.FillMethod.Radial360);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(Image.FillMethod), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIImageFillMethod(L, (Image.FillMethod)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Horizontal"))
			{
				objectTranslator.PushUnityEngineUIImageFillMethod(L, Image.FillMethod.Horizontal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Vertical"))
			{
				objectTranslator.PushUnityEngineUIImageFillMethod(L, Image.FillMethod.Vertical);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Radial90"))
			{
				objectTranslator.PushUnityEngineUIImageFillMethod(L, Image.FillMethod.Radial90);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Radial180"))
			{
				objectTranslator.PushUnityEngineUIImageFillMethod(L, Image.FillMethod.Radial180);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Radial360"))
			{
				objectTranslator.PushUnityEngineUIImageFillMethod(L, Image.FillMethod.Radial360);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.Image.FillMethod!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.FillMethod! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
