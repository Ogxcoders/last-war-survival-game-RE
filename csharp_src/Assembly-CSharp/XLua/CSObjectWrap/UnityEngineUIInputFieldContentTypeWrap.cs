using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIInputFieldContentTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(InputField.ContentType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(InputField.ContentType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(InputField.ContentType), L, null, 11, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Standard", InputField.ContentType.Standard);
		Utils.RegisterObject(L, translator, -4, "Autocorrected", InputField.ContentType.Autocorrected);
		Utils.RegisterObject(L, translator, -4, "IntegerNumber", InputField.ContentType.IntegerNumber);
		Utils.RegisterObject(L, translator, -4, "DecimalNumber", InputField.ContentType.DecimalNumber);
		Utils.RegisterObject(L, translator, -4, "Alphanumeric", InputField.ContentType.Alphanumeric);
		Utils.RegisterObject(L, translator, -4, "Name", InputField.ContentType.Name);
		Utils.RegisterObject(L, translator, -4, "EmailAddress", InputField.ContentType.EmailAddress);
		Utils.RegisterObject(L, translator, -4, "Password", InputField.ContentType.Password);
		Utils.RegisterObject(L, translator, -4, "Pin", InputField.ContentType.Pin);
		Utils.RegisterObject(L, translator, -4, "Custom", InputField.ContentType.Custom);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(InputField.ContentType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIInputFieldContentType(L, (InputField.ContentType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Standard"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Standard);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Autocorrected"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Autocorrected);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "IntegerNumber"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.IntegerNumber);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "DecimalNumber"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.DecimalNumber);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Alphanumeric"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Alphanumeric);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Name"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Name);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "EmailAddress"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.EmailAddress);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Password"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Password);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Pin"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Pin);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Custom"))
			{
				objectTranslator.PushUnityEngineUIInputFieldContentType(L, InputField.ContentType.Custom);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.InputField.ContentType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.InputField.ContentType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
