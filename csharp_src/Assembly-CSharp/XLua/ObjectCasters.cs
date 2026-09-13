using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using XLua.LuaDLL;

namespace XLua;

public class ObjectCasters
{
	private Dictionary<Type, ObjectCast> castersMap = new Dictionary<Type, ObjectCast>();

	private ObjectTranslator translator;

	public ObjectCasters(ObjectTranslator translator)
	{
		this.translator = translator;
		castersMap[typeof(char)] = charCaster;
		castersMap[typeof(sbyte)] = sbyteCaster;
		castersMap[typeof(byte)] = byteCaster;
		castersMap[typeof(short)] = shortCaster;
		castersMap[typeof(ushort)] = ushortCaster;
		castersMap[typeof(int)] = intCaster;
		castersMap[typeof(uint)] = uintCaster;
		castersMap[typeof(long)] = longCaster;
		castersMap[typeof(ulong)] = ulongCaster;
		castersMap[typeof(double)] = getDouble;
		castersMap[typeof(float)] = floatCaster;
		castersMap[typeof(decimal)] = decimalCaster;
		castersMap[typeof(bool)] = getBoolean;
		castersMap[typeof(string)] = getString;
		castersMap[typeof(object)] = getObject;
		castersMap[typeof(byte[])] = getBytes;
		castersMap[typeof(IntPtr)] = getIntptr;
		castersMap[typeof(LuaTable)] = getLuaTable;
		castersMap[typeof(LuaFunction)] = getLuaFunction;
	}

	private static object charCaster(IntPtr L, int idx, object target)
	{
		return (char)Lua.xlua_tointeger(L, idx);
	}

	private static object sbyteCaster(IntPtr L, int idx, object target)
	{
		return (sbyte)Lua.xlua_tointeger(L, idx);
	}

	private static object byteCaster(IntPtr L, int idx, object target)
	{
		return (byte)Lua.xlua_tointeger(L, idx);
	}

	private static object shortCaster(IntPtr L, int idx, object target)
	{
		return (short)Lua.xlua_tointeger(L, idx);
	}

	private static object ushortCaster(IntPtr L, int idx, object target)
	{
		return (ushort)Lua.xlua_tointeger(L, idx);
	}

	private static object intCaster(IntPtr L, int idx, object target)
	{
		return Lua.xlua_tointeger(L, idx);
	}

	private static object uintCaster(IntPtr L, int idx, object target)
	{
		return Lua.xlua_touint(L, idx);
	}

	private static object longCaster(IntPtr L, int idx, object target)
	{
		return Lua.lua_toint64(L, idx);
	}

	private static object ulongCaster(IntPtr L, int idx, object target)
	{
		return Lua.lua_touint64(L, idx);
	}

	private static object getDouble(IntPtr L, int idx, object target)
	{
		return Lua.lua_tonumber(L, idx);
	}

	private static object floatCaster(IntPtr L, int idx, object target)
	{
		return (float)Lua.lua_tonumber(L, idx);
	}

	private object decimalCaster(IntPtr L, int idx, object target)
	{
		translator.Get(L, idx, out decimal val);
		return val;
	}

	private static object getBoolean(IntPtr L, int idx, object target)
	{
		return Lua.lua_toboolean(L, idx);
	}

	private static object getString(IntPtr L, int idx, object target)
	{
		return Lua.lua_tostring(L, idx);
	}

	private object getBytes(IntPtr L, int idx, object target)
	{
		if (Lua.lua_type(L, idx) != LuaTypes.LUA_TSTRING)
		{
			return translator.SafeGetCSObj(L, idx) as byte[];
		}
		return Lua.lua_tobytes(L, idx);
	}

	private object getIntptr(IntPtr L, int idx, object target)
	{
		return Lua.lua_touserdata(L, idx);
	}

	private object getObject(IntPtr L, int idx, object target)
	{
		switch (Lua.lua_type(L, idx))
		{
		case LuaTypes.LUA_TNUMBER:
			if (Lua.lua_isint64(L, idx))
			{
				return Lua.lua_toint64(L, idx);
			}
			if (Lua.lua_isinteger(L, idx))
			{
				return Lua.xlua_tointeger(L, idx);
			}
			return Lua.lua_tonumber(L, idx);
		case LuaTypes.LUA_TSTRING:
			return Lua.lua_tostring(L, idx);
		case LuaTypes.LUA_TBOOLEAN:
			return Lua.lua_toboolean(L, idx);
		case LuaTypes.LUA_TTABLE:
			return getLuaTable(L, idx, null);
		case LuaTypes.LUA_TFUNCTION:
			return getLuaFunction(L, idx, null);
		case LuaTypes.LUA_TUSERDATA:
		{
			if (Lua.lua_isint64(L, idx))
			{
				return Lua.lua_toint64(L, idx);
			}
			if (Lua.lua_isuint64(L, idx))
			{
				return Lua.lua_touint64(L, idx);
			}
			object obj = translator.SafeGetCSObj(L, idx);
			if (!(obj is RawObject))
			{
				return obj;
			}
			return (obj as RawObject).Target;
		}
		default:
			return null;
		}
	}

	private object getLuaTable(IntPtr L, int idx, object target)
	{
		if (Lua.lua_type(L, idx) == LuaTypes.LUA_TUSERDATA)
		{
			object obj = translator.SafeGetCSObj(L, idx);
			if (obj == null || !(obj is LuaTable))
			{
				return null;
			}
			return obj;
		}
		if (!Lua.lua_istable(L, idx))
		{
			return null;
		}
		Lua.lua_pushvalue(L, idx);
		return new LuaTable(Lua.luaL_ref(L), translator.luaEnv);
	}

	private object getLuaFunction(IntPtr L, int idx, object target)
	{
		if (Lua.lua_type(L, idx) == LuaTypes.LUA_TUSERDATA)
		{
			object obj = translator.SafeGetCSObj(L, idx);
			if (obj == null || !(obj is LuaFunction))
			{
				return null;
			}
			return obj;
		}
		if (!Lua.lua_isfunction(L, idx))
		{
			return null;
		}
		Lua.lua_pushvalue(L, idx);
		return new LuaFunction(Lua.luaL_ref(L), translator.luaEnv);
	}

	public void AddCaster(Type type, ObjectCast oc)
	{
		castersMap[type] = oc;
	}

	private ObjectCast genCaster(Type type)
	{
		ObjectCast fixTypeGetter = delegate(IntPtr L, int idx, object target)
		{
			if (Lua.lua_type(L, idx) == LuaTypes.LUA_TUSERDATA)
			{
				object obj = translator.SafeGetCSObj(L, idx);
				if (obj == null || !type.IsAssignableFrom(obj.GetType()))
				{
					return (object)null;
				}
				return obj;
			}
			return (object)null;
		};
		if (typeof(Delegate).IsAssignableFrom(type))
		{
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				return (!Lua.lua_isfunction(L, idx)) ? null : translator.CreateDelegateBridge(L, type, idx);
			};
		}
		if (typeof(DelegateBridgeBase).IsAssignableFrom(type))
		{
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				return (!Lua.lua_isfunction(L, idx)) ? null : translator.CreateDelegateBridge(L, null, idx);
			};
		}
		if (type.IsInterface())
		{
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				return (!Lua.lua_istable(L, idx)) ? null : translator.CreateInterfaceBridge(L, type, idx);
			};
		}
		if (type.IsEnum())
		{
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				return (obj != null) ? obj : (Lua.lua_type(L, idx) switch
				{
					LuaTypes.LUA_TSTRING => Enum.Parse(type, Lua.lua_tostring(L, idx)), 
					LuaTypes.LUA_TNUMBER => Enum.ToObject(type, Lua.xlua_tointeger(L, idx)), 
					_ => throw new InvalidCastException("invalid value for enum " + type), 
				});
			};
		}
		if (type.IsArray)
		{
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				if (!Lua.lua_istable(L, idx))
				{
					return (object)null;
				}
				uint num = Lua.xlua_objlen(L, idx);
				int num2 = Lua.lua_gettop(L);
				idx = ((idx > 0) ? idx : (Lua.lua_gettop(L) + idx + 1));
				Type elementType2 = type.GetElementType();
				ObjectCast caster = GetCaster(elementType2);
				Array array = ((target == null) ? Array.CreateInstance(elementType2, (int)num) : (target as Array));
				if (!Lua.lua_checkstack(L, 1))
				{
					throw new Exception("stack overflow while cast to Array");
				}
				for (int i = 0; i < num; i++)
				{
					Lua.lua_pushnumber(L, i + 1);
					Lua.lua_rawget(L, idx);
					if (elementType2.IsPrimitive())
					{
						if (!StaticLuaCallbacks.TryPrimitiveArraySet(type, L, array, i, num2 + 1))
						{
							array.SetValue(caster(L, num2 + 1, null), i);
						}
					}
					else if (InternalGlobals.genTryArraySetPtr == null || !InternalGlobals.genTryArraySetPtr(type, L, translator, array, i, num2 + 1))
					{
						array.SetValue(caster(L, num2 + 1, null), i);
					}
					Lua.lua_pop(L, 1);
				}
				return array;
			};
		}
		if (typeof(IList).IsAssignableFrom(type) && type.IsGenericType())
		{
			Type elementType = type.GetGenericArguments()[0];
			ObjectCast elementCaster = GetCaster(elementType);
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				if (!Lua.lua_istable(L, idx))
				{
					return (object)null;
				}
				obj = ((target == null) ? Activator.CreateInstance(type) : target);
				int num = Lua.lua_gettop(L);
				idx = ((idx > 0) ? idx : (Lua.lua_gettop(L) + idx + 1));
				IList list = obj as IList;
				uint num2 = Lua.xlua_objlen(L, idx);
				if (!Lua.lua_checkstack(L, 1))
				{
					throw new Exception("stack overflow while cast to IList");
				}
				for (int i = 0; i < num2; i++)
				{
					Lua.lua_pushnumber(L, i + 1);
					Lua.lua_rawget(L, idx);
					if (i < list.Count && target != null)
					{
						if (translator.Assignable(L, num + 1, elementType))
						{
							list[i] = elementCaster(L, num + 1, list[i]);
						}
					}
					else if (translator.Assignable(L, num + 1, elementType))
					{
						list.Add(elementCaster(L, num + 1, null));
					}
					Lua.lua_pop(L, 1);
				}
				return obj;
			};
		}
		if (typeof(IDictionary).IsAssignableFrom(type) && type.IsGenericType())
		{
			Type keyType = type.GetGenericArguments()[0];
			ObjectCast keyCaster = GetCaster(keyType);
			Type valueType = type.GetGenericArguments()[1];
			ObjectCast valueCaster = GetCaster(valueType);
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				if (!Lua.lua_istable(L, idx))
				{
					return (object)null;
				}
				IDictionary dictionary = ((target == null) ? Activator.CreateInstance(type) : target) as IDictionary;
				int num = Lua.lua_gettop(L);
				idx = ((idx > 0) ? idx : (Lua.lua_gettop(L) + idx + 1));
				Lua.lua_pushnil(L);
				if (!Lua.lua_checkstack(L, 1))
				{
					throw new Exception("stack overflow while cast to IDictionary");
				}
				while (Lua.lua_next(L, idx) != 0)
				{
					if (translator.Assignable(L, num + 1, keyType) && translator.Assignable(L, num + 2, valueType))
					{
						object key = keyCaster(L, num + 1, null);
						dictionary[key] = valueCaster(L, num + 2, (!dictionary.Contains(key)) ? null : dictionary[key]);
					}
					Lua.lua_pop(L, 1);
				}
				return dictionary;
			};
		}
		if ((type.IsClass() && type.GetConstructor(Type.EmptyTypes) != null) || (type.IsValueType() && !type.IsEnum()))
		{
			return delegate(IntPtr L, int idx, object target)
			{
				object obj = fixTypeGetter(L, idx, target);
				if (obj != null)
				{
					return obj;
				}
				if (!Lua.lua_istable(L, idx))
				{
					return (object)null;
				}
				obj = ((target == null) ? Activator.CreateInstance(type) : target);
				int num = Lua.lua_gettop(L);
				idx = ((idx > 0) ? idx : (Lua.lua_gettop(L) + idx + 1));
				if (!Lua.lua_checkstack(L, 1))
				{
					throw new Exception("stack overflow while cast to " + type);
				}
				FieldInfo[] fields = type.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					Lua.xlua_pushasciistring(L, fieldInfo.Name);
					Lua.lua_rawget(L, idx);
					if (!Lua.lua_isnil(L, -1))
					{
						try
						{
							fieldInfo.SetValue(obj, GetCaster(fieldInfo.FieldType)(L, num + 1, (target == null || fieldInfo.FieldType.IsPrimitive() || fieldInfo.FieldType == typeof(string)) ? null : fieldInfo.GetValue(obj)));
						}
						catch (Exception ex)
						{
							throw new Exception("exception in tran " + fieldInfo.Name + ", msg=" + ex.Message);
						}
					}
					Lua.lua_pop(L, 1);
				}
				return obj;
			};
		}
		return fixTypeGetter;
	}

	private ObjectCast genNullableCaster(ObjectCast oc)
	{
		return (IntPtr L, int idx, object target) => Lua.lua_isnil(L, idx) ? null : oc(L, idx, target);
	}

	public ObjectCast GetCaster(Type type)
	{
		if (type.IsByRef)
		{
			type = type.GetElementType();
		}
		Type underlyingType = Nullable.GetUnderlyingType(type);
		if (underlyingType != null)
		{
			return genNullableCaster(GetCaster(underlyingType));
		}
		if (!castersMap.TryGetValue(type, out var value))
		{
			value = genCaster(type);
			castersMap.Add(type, value);
		}
		return value;
	}
}
