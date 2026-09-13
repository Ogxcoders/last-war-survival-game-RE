using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldContentTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TMP_InputField.ContentType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TMP_InputField.ContentType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TMP_InputField.ContentType), L, null, 11, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Standard", TMP_InputField.ContentType.Standard);
		Utils.RegisterObject(L, translator, -4, "Autocorrected", TMP_InputField.ContentType.Autocorrected);
		Utils.RegisterObject(L, translator, -4, "IntegerNumber", TMP_InputField.ContentType.IntegerNumber);
		Utils.RegisterObject(L, translator, -4, "DecimalNumber", TMP_InputField.ContentType.DecimalNumber);
		Utils.RegisterObject(L, translator, -4, "Alphanumeric", TMP_InputField.ContentType.Alphanumeric);
		Utils.RegisterObject(L, translator, -4, "Name", TMP_InputField.ContentType.Name);
		Utils.RegisterObject(L, translator, -4, "EmailAddress", TMP_InputField.ContentType.EmailAddress);
		Utils.RegisterObject(L, translator, -4, "Password", TMP_InputField.ContentType.Password);
		Utils.RegisterObject(L, translator, -4, "Pin", TMP_InputField.ContentType.Pin);
		Utils.RegisterObject(L, translator, -4, "Custom", TMP_InputField.ContentType.Custom);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TMP_InputField.ContentType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTMProTMP_InputFieldContentType(L, (TMP_InputField.ContentType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Standard"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Standard);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Autocorrected"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Autocorrected);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "IntegerNumber"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.IntegerNumber);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "DecimalNumber"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.DecimalNumber);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Alphanumeric"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Alphanumeric);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Name"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Name);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "EmailAddress"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.EmailAddress);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Password"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Password);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Pin"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Pin);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Custom"))
			{
				objectTranslator.PushTMProTMP_InputFieldContentType(L, TMP_InputField.ContentType.Custom);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TMPro.TMP_InputField.ContentType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TMPro.TMP_InputField.ContentType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
