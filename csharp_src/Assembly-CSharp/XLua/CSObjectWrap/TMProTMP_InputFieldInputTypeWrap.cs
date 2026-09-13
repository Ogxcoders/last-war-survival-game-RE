using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldInputTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TMP_InputField.InputType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TMP_InputField.InputType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TMP_InputField.InputType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Standard", TMP_InputField.InputType.Standard);
		Utils.RegisterObject(L, translator, -4, "AutoCorrect", TMP_InputField.InputType.AutoCorrect);
		Utils.RegisterObject(L, translator, -4, "Password", TMP_InputField.InputType.Password);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TMP_InputField.InputType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTMProTMP_InputFieldInputType(L, (TMP_InputField.InputType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Standard"))
			{
				objectTranslator.PushTMProTMP_InputFieldInputType(L, TMP_InputField.InputType.Standard);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "AutoCorrect"))
			{
				objectTranslator.PushTMProTMP_InputFieldInputType(L, TMP_InputField.InputType.AutoCorrect);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Password"))
			{
				objectTranslator.PushTMProTMP_InputFieldInputType(L, TMP_InputField.InputType.Password);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TMPro.TMP_InputField.InputType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TMPro.TMP_InputField.InputType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
