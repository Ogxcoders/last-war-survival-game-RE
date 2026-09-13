using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ScrollViewScrollViewLayoutTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ScrollView.ScrollViewLayoutType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ScrollView.ScrollViewLayoutType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ScrollView.ScrollViewLayoutType), L, null, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Horizontal", ScrollView.ScrollViewLayoutType.Horizontal);
		Utils.RegisterObject(L, translator, -4, "Vertical", ScrollView.ScrollViewLayoutType.Vertical);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ScrollView.ScrollViewLayoutType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushScrollViewScrollViewLayoutType(L, (ScrollView.ScrollViewLayoutType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Horizontal"))
			{
				objectTranslator.PushScrollViewScrollViewLayoutType(L, ScrollView.ScrollViewLayoutType.Horizontal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Vertical"))
			{
				objectTranslator.PushScrollViewScrollViewLayoutType(L, ScrollView.ScrollViewLayoutType.Vertical);
				break;
			}
			return Lua.luaL_error(L, "invalid string for ScrollView.ScrollViewLayoutType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for ScrollView.ScrollViewLayoutType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
