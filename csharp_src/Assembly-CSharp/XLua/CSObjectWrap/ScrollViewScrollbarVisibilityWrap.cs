using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ScrollViewScrollbarVisibilityWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ScrollView.ScrollbarVisibility), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ScrollView.ScrollbarVisibility), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ScrollView.ScrollbarVisibility), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Permanent", ScrollView.ScrollbarVisibility.Permanent);
		Utils.RegisterObject(L, translator, -4, "AutoHide", ScrollView.ScrollbarVisibility.AutoHide);
		Utils.RegisterObject(L, translator, -4, "AutoHideAndExpandViewport", ScrollView.ScrollbarVisibility.AutoHideAndExpandViewport);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ScrollView.ScrollbarVisibility), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushScrollViewScrollbarVisibility(L, (ScrollView.ScrollbarVisibility)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Permanent"))
			{
				objectTranslator.PushScrollViewScrollbarVisibility(L, ScrollView.ScrollbarVisibility.Permanent);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AutoHide"))
			{
				objectTranslator.PushScrollViewScrollbarVisibility(L, ScrollView.ScrollbarVisibility.AutoHide);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AutoHideAndExpandViewport"))
			{
				objectTranslator.PushScrollViewScrollbarVisibility(L, ScrollView.ScrollbarVisibility.AutoHideAndExpandViewport);
				break;
			}
			return Lua.luaL_error(L, "invalid string for ScrollView.ScrollbarVisibility!");
		default:
			return Lua.luaL_error(L, "invalid lua type for ScrollView.ScrollbarVisibility! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
