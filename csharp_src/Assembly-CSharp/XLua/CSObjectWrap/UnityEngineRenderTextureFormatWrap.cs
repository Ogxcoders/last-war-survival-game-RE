using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderTextureFormatWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(RenderTextureFormat), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(RenderTextureFormat), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(RenderTextureFormat), L, null, 29, 0, 0);
		Utils.RegisterEnumType(L, typeof(RenderTextureFormat));
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(RenderTextureFormat), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineRenderTextureFormat(L, (RenderTextureFormat)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			try
			{
				objectTranslator.TranslateToEnumToTop(L, typeof(RenderTextureFormat), 1);
			}
			catch (Exception ex)
			{
				return Lua.luaL_error(L, string.Concat("cast to ", typeof(RenderTextureFormat), " exception:", ex));
			}
			break;
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.RenderTextureFormat! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
