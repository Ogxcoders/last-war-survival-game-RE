using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIInputFieldCharacterValidationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(InputField.CharacterValidation), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(InputField.CharacterValidation), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(InputField.CharacterValidation), L, null, 7, 0, 0);
		Utils.RegisterObject(L, translator, -4, "None", InputField.CharacterValidation.None);
		Utils.RegisterObject(L, translator, -4, "Integer", InputField.CharacterValidation.Integer);
		Utils.RegisterObject(L, translator, -4, "Decimal", InputField.CharacterValidation.Decimal);
		Utils.RegisterObject(L, translator, -4, "Alphanumeric", InputField.CharacterValidation.Alphanumeric);
		Utils.RegisterObject(L, translator, -4, "Name", InputField.CharacterValidation.Name);
		Utils.RegisterObject(L, translator, -4, "EmailAddress", InputField.CharacterValidation.EmailAddress);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(InputField.CharacterValidation), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, (InputField.CharacterValidation)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "None"))
			{
				objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, InputField.CharacterValidation.None);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Integer"))
			{
				objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, InputField.CharacterValidation.Integer);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Decimal"))
			{
				objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, InputField.CharacterValidation.Decimal);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Alphanumeric"))
			{
				objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, InputField.CharacterValidation.Alphanumeric);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Name"))
			{
				objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, InputField.CharacterValidation.Name);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "EmailAddress"))
			{
				objectTranslator.PushUnityEngineUIInputFieldCharacterValidation(L, InputField.CharacterValidation.EmailAddress);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.InputField.CharacterValidation!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.InputField.CharacterValidation! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
