using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldCharacterValidationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TMP_InputField.CharacterValidation), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TMP_InputField.CharacterValidation), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TMP_InputField.CharacterValidation), L, null, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", TMP_InputField.CharacterValidation.None);
		Utils.RegisterObject(L, translator, -4, "Digit", TMP_InputField.CharacterValidation.Digit);
		Utils.RegisterObject(L, translator, -4, "Integer", TMP_InputField.CharacterValidation.Integer);
		Utils.RegisterObject(L, translator, -4, "Decimal", TMP_InputField.CharacterValidation.Decimal);
		Utils.RegisterObject(L, translator, -4, "Alphanumeric", TMP_InputField.CharacterValidation.Alphanumeric);
		Utils.RegisterObject(L, translator, -4, "Name", TMP_InputField.CharacterValidation.Name);
		Utils.RegisterObject(L, translator, -4, "Regex", TMP_InputField.CharacterValidation.Regex);
		Utils.RegisterObject(L, translator, -4, "EmailAddress", TMP_InputField.CharacterValidation.EmailAddress);
		Utils.RegisterObject(L, translator, -4, "CustomValidator", TMP_InputField.CharacterValidation.CustomValidator);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TMP_InputField.CharacterValidation), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, (TMP_InputField.CharacterValidation)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Digit"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.Digit);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Integer"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.Integer);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Decimal"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.Decimal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Alphanumeric"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.Alphanumeric);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Name"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.Name);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Regex"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.Regex);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "EmailAddress"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.EmailAddress);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CustomValidator"))
			{
				objectTranslator.PushTMProTMP_InputFieldCharacterValidation(L, TMP_InputField.CharacterValidation.CustomValidator);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TMPro.TMP_InputField.CharacterValidation!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TMPro.TMP_InputField.CharacterValidation! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
