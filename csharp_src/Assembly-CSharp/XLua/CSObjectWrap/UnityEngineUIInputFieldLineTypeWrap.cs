using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIInputFieldLineTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(InputField.LineType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(InputField.LineType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(InputField.LineType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "SingleLine", InputField.LineType.SingleLine);
		Utils.RegisterObject(L, translator, -4, "MultiLineSubmit", InputField.LineType.MultiLineSubmit);
		Utils.RegisterObject(L, translator, -4, "MultiLineNewline", InputField.LineType.MultiLineNewline);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(InputField.LineType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushUnityEngineUIInputFieldLineType(L, (InputField.LineType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "SingleLine"))
			{
				objectTranslator.PushUnityEngineUIInputFieldLineType(L, InputField.LineType.SingleLine);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MultiLineSubmit"))
			{
				objectTranslator.PushUnityEngineUIInputFieldLineType(L, InputField.LineType.MultiLineSubmit);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MultiLineNewline"))
			{
				objectTranslator.PushUnityEngineUIInputFieldLineType(L, InputField.LineType.MultiLineNewline);
				break;
			}
			return Lua.luaL_error(L, "invalid string for UnityEngine.UI.InputField.LineType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for UnityEngine.UI.InputField.LineType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
