using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldLineTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TMP_InputField.LineType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TMP_InputField.LineType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TMP_InputField.LineType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "SingleLine", TMP_InputField.LineType.SingleLine);
		Utils.RegisterObject(L, translator, -4, "MultiLineSubmit", TMP_InputField.LineType.MultiLineSubmit);
		Utils.RegisterObject(L, translator, -4, "MultiLineNewline", TMP_InputField.LineType.MultiLineNewline);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TMP_InputField.LineType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTMProTMP_InputFieldLineType(L, (TMP_InputField.LineType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "SingleLine"))
			{
				objectTranslator.PushTMProTMP_InputFieldLineType(L, TMP_InputField.LineType.SingleLine);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MultiLineSubmit"))
			{
				objectTranslator.PushTMProTMP_InputFieldLineType(L, TMP_InputField.LineType.MultiLineSubmit);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MultiLineNewline"))
			{
				objectTranslator.PushTMProTMP_InputFieldLineType(L, TMP_InputField.LineType.MultiLineNewline);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TMPro.TMP_InputField.LineType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TMPro.TMP_InputField.LineType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
