using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIScrollRectScrollbarVisibilityWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ScrollRect.ScrollbarVisibility), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ScrollRect.ScrollbarVisibility), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ScrollRect.ScrollbarVisibility), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Permanent", ScrollRect.ScrollbarVisibility.Permanent);
		Utils.RegisterObject(L, translator, -4, "AutoHide", ScrollRect.ScrollbarVisibility.AutoHide);
		Utils.RegisterObject(L, translator, -4, "AutoHideAndExpandViewport", ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ScrollRect.ScrollbarVisibility), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIScrollRectScrollbarVisibility(L, (ScrollRect.ScrollbarVisibility)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Permanent"))
			{
				objectTranslator.PushUnityEngineUIScrollRectScrollbarVisibility(L, ScrollRect.ScrollbarVisibility.Permanent);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AutoHide"))
			{
				objectTranslator.PushUnityEngineUIScrollRectScrollbarVisibility(L, ScrollRect.ScrollbarVisibility.AutoHide);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AutoHideAndExpandViewport"))
			{
				objectTranslator.PushUnityEngineUIScrollRectScrollbarVisibility(L, ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.ScrollRect.ScrollbarVisibility!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.ScrollRect.ScrollbarVisibility! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
