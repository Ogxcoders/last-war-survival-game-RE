using System;
using System.Reflection;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemReflectionBindingFlagsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(BindingFlags), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(BindingFlags), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(BindingFlags), L, null, 21, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Default", BindingFlags.Default);
		Utils.RegisterObject(L, translator, -4, "IgnoreCase", BindingFlags.IgnoreCase);
		Utils.RegisterObject(L, translator, -4, "DeclaredOnly", BindingFlags.DeclaredOnly);
		Utils.RegisterObject(L, translator, -4, "Instance", BindingFlags.Instance);
		Utils.RegisterObject(L, translator, -4, "Static", BindingFlags.Static);
		Utils.RegisterObject(L, translator, -4, "Public", BindingFlags.Public);
		Utils.RegisterObject(L, translator, -4, "NonPublic", BindingFlags.NonPublic);
		Utils.RegisterObject(L, translator, -4, "FlattenHierarchy", BindingFlags.FlattenHierarchy);
		Utils.RegisterObject(L, translator, -4, "InvokeMethod", BindingFlags.InvokeMethod);
		Utils.RegisterObject(L, translator, -4, "CreateInstance", BindingFlags.CreateInstance);
		Utils.RegisterObject(L, translator, -4, "GetField", BindingFlags.GetField);
		Utils.RegisterObject(L, translator, -4, "SetField", BindingFlags.SetField);
		Utils.RegisterObject(L, translator, -4, "GetProperty", BindingFlags.GetProperty);
		Utils.RegisterObject(L, translator, -4, "SetProperty", BindingFlags.SetProperty);
		Utils.RegisterObject(L, translator, -4, "PutDispProperty", BindingFlags.PutDispProperty);
		Utils.RegisterObject(L, translator, -4, "PutRefDispProperty", BindingFlags.PutRefDispProperty);
		Utils.RegisterObject(L, translator, -4, "ExactBinding", BindingFlags.ExactBinding);
		Utils.RegisterObject(L, translator, -4, "SuppressChangeType", BindingFlags.SuppressChangeType);
		Utils.RegisterObject(L, translator, -4, "OptionalParamBinding", BindingFlags.OptionalParamBinding);
		Utils.RegisterObject(L, translator, -4, "IgnoreReturn", BindingFlags.IgnoreReturn);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(BindingFlags), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushSystemReflectionBindingFlags(L, (BindingFlags)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Default"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.Default);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "IgnoreCase"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.IgnoreCase);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "DeclaredOnly"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.DeclaredOnly);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Instance"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.Instance);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Static"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.Static);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Public"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.Public);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "NonPublic"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.NonPublic);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FlattenHierarchy"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.FlattenHierarchy);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "InvokeMethod"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.InvokeMethod);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "CreateInstance"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.CreateInstance);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "GetField"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.GetField);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SetField"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.SetField);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "GetProperty"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.GetProperty);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SetProperty"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.SetProperty);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PutDispProperty"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.PutDispProperty);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "PutRefDispProperty"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.PutRefDispProperty);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "ExactBinding"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.ExactBinding);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SuppressChangeType"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.SuppressChangeType);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "OptionalParamBinding"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.OptionalParamBinding);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "IgnoreReturn"))
			{
				objectTranslator.PushSystemReflectionBindingFlags(L, BindingFlags.IgnoreReturn);
				break;
			}
			return Lua.luaL_error(L, "invalid string for System.Reflection.BindingFlags!");
		default:
			return Lua.luaL_error(L, "invalid lua type for System.Reflection.BindingFlags! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
