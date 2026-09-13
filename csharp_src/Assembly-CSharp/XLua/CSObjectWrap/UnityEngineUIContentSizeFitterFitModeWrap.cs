using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIContentSizeFitterFitModeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ContentSizeFitter.FitMode), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ContentSizeFitter.FitMode), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ContentSizeFitter.FitMode), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Unconstrained", ContentSizeFitter.FitMode.Unconstrained);
		Utils.RegisterObject(L, translator, -4, "MinSize", ContentSizeFitter.FitMode.MinSize);
		Utils.RegisterObject(L, translator, -4, "PreferredSize", ContentSizeFitter.FitMode.PreferredSize);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ContentSizeFitter.FitMode), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIContentSizeFitterFitMode(L, (ContentSizeFitter.FitMode)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Unconstrained"))
			{
				objectTranslator.PushUnityEngineUIContentSizeFitterFitMode(L, ContentSizeFitter.FitMode.Unconstrained);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MinSize"))
			{
				objectTranslator.PushUnityEngineUIContentSizeFitterFitMode(L, ContentSizeFitter.FitMode.MinSize);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PreferredSize"))
			{
				objectTranslator.PushUnityEngineUIContentSizeFitterFitMode(L, ContentSizeFitter.FitMode.PreferredSize);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.ContentSizeFitter.FitMode!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.ContentSizeFitter.FitMode! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
