using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemEnumWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Enum);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 0, 0);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "CompareTo", _m_CompareTo);
		Utils.RegisterFunc(L, -3, "HasFlag", _m_HasFlag);
		Utils.RegisterFunc(L, -3, "GetTypeCode", _m_GetTypeCode);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 0, 0);
		Utils.RegisterFunc(L, -4, "Parse", _m_Parse_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetUnderlyingType", _m_GetUnderlyingType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetValues", _m_GetValues_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetName", _m_GetName_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetNames", _m_GetNames_xlua_st_);
		Utils.RegisterFunc(L, -4, "ToObject", _m_ToObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsDefined", _m_IsDefined_xlua_st_);
		Utils.RegisterFunc(L, -4, "Format", _m_Format_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "System.Enum does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Parse_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				Type enumType = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				string value = Lua.lua_tostring(L, 2);
				object o = Enum.Parse(enumType, value);
				objectTranslator.PushAny(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type enumType2 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				string value2 = Lua.lua_tostring(L, 2);
				bool ignoreCase = Lua.lua_toboolean(L, 3);
				object o2 = Enum.Parse(enumType2, value2, ignoreCase);
				objectTranslator.PushAny(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Enum.Parse!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUnderlyingType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type underlyingType = Enum.GetUnderlyingType((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.Push(L, underlyingType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetValues_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array values = Enum.GetValues((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.Push(L, values);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetName_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type enumType = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
			object value = objectTranslator.GetObject(L, 2, typeof(object));
			string name = Enum.GetName(enumType, value);
			Lua.lua_pushstring(L, name);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNames_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string[] names = Enum.GetNames((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.Push(L, names);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type enumType = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				sbyte value = (sbyte)Lua.xlua_tointeger(L, 2);
				object o = Enum.ToObject(enumType, value);
				objectTranslator.PushAny(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type enumType2 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				short value2 = (short)Lua.xlua_tointeger(L, 2);
				object o2 = Enum.ToObject(enumType2, value2);
				objectTranslator.PushAny(L, o2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type enumType3 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				int value3 = Lua.xlua_tointeger(L, 2);
				object o3 = Enum.ToObject(enumType3, value3);
				objectTranslator.PushAny(L, o3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type enumType4 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				byte value4 = (byte)Lua.xlua_tointeger(L, 2);
				object o4 = Enum.ToObject(enumType4, value4);
				objectTranslator.PushAny(L, o4);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type enumType5 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				ushort value5 = (ushort)Lua.xlua_tointeger(L, 2);
				object o5 = Enum.ToObject(enumType5, value5);
				objectTranslator.PushAny(L, o5);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type enumType6 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				uint value6 = Lua.xlua_touint(L, 2);
				object o6 = Enum.ToObject(enumType6, value6);
				objectTranslator.PushAny(L, o6);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				Type enumType7 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				long value7 = Lua.lua_toint64(L, 2);
				object o7 = Enum.ToObject(enumType7, value7);
				objectTranslator.PushAny(L, o7);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isuint64(L, 2)))
			{
				Type enumType8 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				ulong value8 = Lua.lua_touint64(L, 2);
				object o8 = Enum.ToObject(enumType8, value8);
				objectTranslator.PushAny(L, o8);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				Type enumType9 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				object value9 = objectTranslator.GetObject(L, 2, typeof(object));
				object o9 = Enum.ToObject(enumType9, value9);
				objectTranslator.PushAny(L, o9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Enum.ToObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDefined_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type enumType = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
			object value = objectTranslator.GetObject(L, 2, typeof(object));
			bool value2 = Enum.IsDefined(enumType, value);
			Lua.lua_pushboolean(L, value2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Format_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type enumType = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
			object value = objectTranslator.GetObject(L, 2, typeof(object));
			string format = Lua.lua_tostring(L, 3);
			string str = Enum.Format(enumType, value, format);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Equals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Enum obj = (Enum)objectTranslator.FastGetCSObj(L, 1);
			object obj2 = objectTranslator.GetObject(L, 2, typeof(object));
			bool value = obj.Equals(obj2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHashCode(IntPtr L)
	{
		try
		{
			int hashCode = ((Enum)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			Enum obj = (Enum)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = obj.ToString();
				Lua.lua_pushstring(L, str2);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = obj.ToString(text);
					Lua.lua_pushstring(L, str);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Enum.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompareTo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Enum obj = (Enum)objectTranslator.FastGetCSObj(L, 1);
			object target = objectTranslator.GetObject(L, 2, typeof(object));
			int value = obj.CompareTo(target);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasFlag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Enum obj = (Enum)objectTranslator.FastGetCSObj(L, 1);
			Enum flag = (Enum)objectTranslator.GetObject(L, 2, typeof(Enum));
			bool value = obj.HasFlag(flag);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTypeCode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TypeCode typeCode = ((Enum)objectTranslator.FastGetCSObj(L, 1)).GetTypeCode();
			objectTranslator.Push(L, typeCode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
