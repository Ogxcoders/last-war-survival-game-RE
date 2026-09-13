using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldExHorizontalAlignmentOptionsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(TMP_InputFieldEx.HorizontalAlignmentOptions), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(TMP_InputFieldEx.HorizontalAlignmentOptions), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(TMP_InputFieldEx.HorizontalAlignmentOptions), L, null, 7, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Left", TMP_InputFieldEx.HorizontalAlignmentOptions.Left);
		Utils.RegisterObject(L, translator, -4, "Center", TMP_InputFieldEx.HorizontalAlignmentOptions.Center);
		Utils.RegisterObject(L, translator, -4, "Right", TMP_InputFieldEx.HorizontalAlignmentOptions.Right);
		Utils.RegisterObject(L, translator, -4, "Justified", TMP_InputFieldEx.HorizontalAlignmentOptions.Justified);
		Utils.RegisterObject(L, translator, -4, "Flush", TMP_InputFieldEx.HorizontalAlignmentOptions.Flush);
		Utils.RegisterObject(L, translator, -4, "Geometry", TMP_InputFieldEx.HorizontalAlignmentOptions.Geometry);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(TMP_InputFieldEx.HorizontalAlignmentOptions), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, (TMP_InputFieldEx.HorizontalAlignmentOptions)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Left"))
			{
				objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, TMP_InputFieldEx.HorizontalAlignmentOptions.Left);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Center"))
			{
				objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, TMP_InputFieldEx.HorizontalAlignmentOptions.Center);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Right"))
			{
				objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, TMP_InputFieldEx.HorizontalAlignmentOptions.Right);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Justified"))
			{
				objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, TMP_InputFieldEx.HorizontalAlignmentOptions.Justified);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Flush"))
			{
				objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, TMP_InputFieldEx.HorizontalAlignmentOptions.Flush);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Geometry"))
			{
				objectTranslator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, TMP_InputFieldEx.HorizontalAlignmentOptions.Geometry);
				break;
			}
			return Lua.luaL_error(L, "invalid string for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions!");
		default:
			return Lua.luaL_error(L, "invalid lua type for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
