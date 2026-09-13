using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TextMeshProUGUIExHorizontalAlignmentOptionsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions), L, null, 7, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Left", TextMeshProUGUIEx.HorizontalAlignmentOptions.Left);
		Utils.RegisterObject(L, translator, -4, "Center", TextMeshProUGUIEx.HorizontalAlignmentOptions.Center);
		Utils.RegisterObject(L, translator, -4, "Right", TextMeshProUGUIEx.HorizontalAlignmentOptions.Right);
		Utils.RegisterObject(L, translator, -4, "Justified", TextMeshProUGUIEx.HorizontalAlignmentOptions.Justified);
		Utils.RegisterObject(L, translator, -4, "Flush", TextMeshProUGUIEx.HorizontalAlignmentOptions.Flush);
		Utils.RegisterObject(L, translator, -4, "Geometry", TextMeshProUGUIEx.HorizontalAlignmentOptions.Geometry);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, (TextMeshProUGUIEx.HorizontalAlignmentOptions)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, TextMeshProUGUIEx.HorizontalAlignmentOptions.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Center"))
			{
				objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, TextMeshProUGUIEx.HorizontalAlignmentOptions.Center);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, TextMeshProUGUIEx.HorizontalAlignmentOptions.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Justified"))
			{
				objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, TextMeshProUGUIEx.HorizontalAlignmentOptions.Justified);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Flush"))
			{
				objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, TextMeshProUGUIEx.HorizontalAlignmentOptions.Flush);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Geometry"))
			{
				objectTranslator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, TextMeshProUGUIEx.HorizontalAlignmentOptions.Geometry);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TextMeshProUGUIEx.HorizontalAlignmentOptions!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TextMeshProUGUIEx.HorizontalAlignmentOptions! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
