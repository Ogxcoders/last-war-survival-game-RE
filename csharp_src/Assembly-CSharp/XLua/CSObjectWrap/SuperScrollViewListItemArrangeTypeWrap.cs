using System;
using SuperScrollView;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SuperScrollViewListItemArrangeTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(ListItemArrangeType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(ListItemArrangeType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(ListItemArrangeType), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "TopToBottom", ListItemArrangeType.TopToBottom);
		Utils.RegisterObject(L, translator, -4, "BottomToTop", ListItemArrangeType.BottomToTop);
		Utils.RegisterObject(L, translator, -4, "LeftToRight", ListItemArrangeType.LeftToRight);
		Utils.RegisterObject(L, translator, -4, "RightToLeft", ListItemArrangeType.RightToLeft);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(ListItemArrangeType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushSuperScrollViewListItemArrangeType(L, (ListItemArrangeType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "TopToBottom"))
			{
				objectTranslator.PushSuperScrollViewListItemArrangeType(L, ListItemArrangeType.TopToBottom);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "BottomToTop"))
			{
				objectTranslator.PushSuperScrollViewListItemArrangeType(L, ListItemArrangeType.BottomToTop);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "LeftToRight"))
			{
				objectTranslator.PushSuperScrollViewListItemArrangeType(L, ListItemArrangeType.LeftToRight);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "RightToLeft"))
			{
				objectTranslator.PushSuperScrollViewListItemArrangeType(L, ListItemArrangeType.RightToLeft);
				break;
			}
			return Lua.luaL_error(L, "invalid string for SuperScrollView.ListItemArrangeType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for SuperScrollView.ListItemArrangeType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
