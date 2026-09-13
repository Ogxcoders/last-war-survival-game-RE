using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TextMeshProUGUIExVerticalAlignmentOptionsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TextMeshProUGUIEx.VerticalAlignmentOptions), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TextMeshProUGUIEx.VerticalAlignmentOptions), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TextMeshProUGUIEx.VerticalAlignmentOptions), L, null, 7, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Top", TextMeshProUGUIEx.VerticalAlignmentOptions.Top);
		Utils.RegisterObject(L, translator, -4, "Middle", TextMeshProUGUIEx.VerticalAlignmentOptions.Middle);
		Utils.RegisterObject(L, translator, -4, "Bottom", TextMeshProUGUIEx.VerticalAlignmentOptions.Bottom);
		Utils.RegisterObject(L, translator, -4, "Baseline", TextMeshProUGUIEx.VerticalAlignmentOptions.Baseline);
		Utils.RegisterObject(L, translator, -4, "Geometry", TextMeshProUGUIEx.VerticalAlignmentOptions.Geometry);
		Utils.RegisterObject(L, translator, -4, "Capline", TextMeshProUGUIEx.VerticalAlignmentOptions.Capline);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TextMeshProUGUIEx.VerticalAlignmentOptions), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, (TextMeshProUGUIEx.VerticalAlignmentOptions)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Top"))
			{
				objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, TextMeshProUGUIEx.VerticalAlignmentOptions.Top);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Middle"))
			{
				objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, TextMeshProUGUIEx.VerticalAlignmentOptions.Middle);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Bottom"))
			{
				objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, TextMeshProUGUIEx.VerticalAlignmentOptions.Bottom);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Baseline"))
			{
				objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, TextMeshProUGUIEx.VerticalAlignmentOptions.Baseline);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Geometry"))
			{
				objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, TextMeshProUGUIEx.VerticalAlignmentOptions.Geometry);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Capline"))
			{
				objectTranslator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, TextMeshProUGUIEx.VerticalAlignmentOptions.Capline);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TextMeshProUGUIEx.VerticalAlignmentOptions!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TextMeshProUGUIEx.VerticalAlignmentOptions! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
