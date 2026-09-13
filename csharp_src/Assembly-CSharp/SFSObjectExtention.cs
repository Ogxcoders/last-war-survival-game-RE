using System;
using System.Collections.Generic;
using System.Reflection;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

public static class SFSObjectExtention
{
	private static FieldInfo _fiDataHolder;

	private static Dictionary<string, SFSDataWrapper> GetDataHolder(this SFSObject sfsData)
	{
		if (_fiDataHolder == null)
		{
			_fiDataHolder = typeof(SFSObject).GetField("dataHolder", BindingFlags.Instance | BindingFlags.NonPublic);
			if (_fiDataHolder == null)
			{
				Debug.LogError("GetDataHolder Error FieldInfo is null!");
				return null;
			}
		}
		return (Dictionary<string, SFSDataWrapper>)_fiDataHolder.GetValue(sfsData);
	}

	public static LuaStackTable ToLuaTable(this bool[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (bool value in array)
		{
			result.SetBool(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this short[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (short value in array)
		{
			result.SetShort(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this int[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (int value in array)
		{
			result.SetInt(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this long[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (long value in array)
		{
			result.SetLong(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this float[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (float value in array)
		{
			result.SetFloat(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this double[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (double value in array)
		{
			result.SetDouble(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this string[] array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		foreach (string value in array)
		{
			result.SetString(++num, value);
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this SFSObject sfsData, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		IntPtr l = GameEntry.Lua.Env.L;
		foreach (KeyValuePair<string, SFSDataWrapper> item in sfsData.GetDataHolder())
		{
			SFSDataType type = (SFSDataType)item.Value.Type;
			string key = item.Key;
			switch (type)
			{
			case SFSDataType.BOOL:
				result.SetBool(key, (bool)item.Value.Data);
				break;
			case SFSDataType.BYTE:
				result.SetByte(key, sfsData.GetByte(key));
				break;
			case SFSDataType.SHORT:
				result.SetShort(key, sfsData.GetShort(key));
				break;
			case SFSDataType.INT:
				result.SetInt(key, (int)item.Value.Data);
				break;
			case SFSDataType.LONG:
				result.SetLong(key, (long)item.Value.Data);
				break;
			case SFSDataType.FLOAT:
				result.SetFloat(key, (float)item.Value.Data);
				break;
			case SFSDataType.DOUBLE:
				result.SetDouble(key, (double)item.Value.Data);
				break;
			case SFSDataType.UTF_STRING:
				result.SetString(key, (string)item.Value.Data);
				break;
			case SFSDataType.BOOL_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetBoolArray(key).ToLuaTable(env));
				break;
			case SFSDataType.BYTE_ARRAY:
				result.SetBytes(key, sfsData.GetByteArray(key).Bytes);
				break;
			case SFSDataType.SHORT_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetShortArray(key).ToLuaTable(env));
				break;
			case SFSDataType.INT_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetIntArray(key).ToLuaTable(env));
				break;
			case SFSDataType.LONG_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetLongArray(key).ToLuaTable(env));
				break;
			case SFSDataType.FLOAT_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetFloatArray(key).ToLuaTable(env));
				break;
			case SFSDataType.DOUBLE_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetDoubleArray(key).ToLuaTable(env));
				break;
			case SFSDataType.UTF_STRING_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, sfsData.GetUtfStringArray(key).ToLuaTable(env));
				break;
			case SFSDataType.SFS_ARRAY:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, ((SFSArray)sfsData.GetSFSArray(key)).ToLuaTable(env));
				break;
			case SFSDataType.SFS_OBJECT:
				Lua.lua_pushstring(l, key);
				result.SetStackTable(key, ((SFSObject)sfsData.GetSFSObject(key)).ToLuaTable(env));
				break;
			case SFSDataType.TEXT:
				result.SetString(key, sfsData.GetText(key));
				break;
			case SFSDataType.NULL:
				result.SetString(key, null);
				break;
			default:
				Debug.LogWarningFormat("Unsupport type {0}", type);
				break;
			}
		}
		return result;
	}

	public static LuaStackTable ToLuaTable(this SFSArray array, LuaEnv env)
	{
		LuaStackTable result = new LuaStackTable(env.L);
		int num = 0;
		IntPtr l = GameEntry.Lua.Env.L;
		for (int i = 0; i < array.Size(); i++)
		{
			SFSDataType type = (SFSDataType)array.GetWrappedElementAt(i).Type;
			switch (type)
			{
			case SFSDataType.BOOL:
				result.SetBool(++num, array.GetBool(i));
				break;
			case SFSDataType.BYTE:
				result.SetByte(++num, array.GetByte(i));
				break;
			case SFSDataType.SHORT:
				result.SetShort(++num, array.GetShort(i));
				break;
			case SFSDataType.INT:
				result.SetInt(++num, array.GetInt(i));
				break;
			case SFSDataType.LONG:
				result.SetLong(++num, array.GetLong(i));
				break;
			case SFSDataType.FLOAT:
				result.SetFloat(++num, array.GetFloat(i));
				break;
			case SFSDataType.DOUBLE:
				result.SetDouble(++num, array.GetDouble(i));
				break;
			case SFSDataType.UTF_STRING:
				result.SetString(++num, array.GetUtfString(i));
				break;
			case SFSDataType.BOOL_ARRAY:
				Lua.xlua_pushinteger(GameEntry.Lua.Env.L, ++num);
				result.SetStackTable(num, array.GetBoolArray(i).ToLuaTable(env));
				break;
			case SFSDataType.BYTE_ARRAY:
				result.SetBytes(++num, array.GetByteArray(i).Bytes);
				break;
			case SFSDataType.SHORT_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, array.GetShortArray(i).ToLuaTable(env));
				break;
			case SFSDataType.INT_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, array.GetIntArray(i).ToLuaTable(env));
				break;
			case SFSDataType.LONG_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, array.GetLongArray(i).ToLuaTable(env));
				break;
			case SFSDataType.FLOAT_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, array.GetFloatArray(i).ToLuaTable(env));
				break;
			case SFSDataType.DOUBLE_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, array.GetDoubleArray(i).ToLuaTable(env));
				break;
			case SFSDataType.UTF_STRING_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, array.GetUtfStringArray(i).ToLuaTable(env));
				break;
			case SFSDataType.SFS_ARRAY:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, ((SFSArray)array.GetSFSArray(i)).ToLuaTable(env));
				break;
			case SFSDataType.SFS_OBJECT:
				Lua.xlua_pushinteger(l, ++num);
				result.SetStackTable(num, ((SFSObject)array.GetSFSObject(i)).ToLuaTable(env));
				break;
			case SFSDataType.TEXT:
				result.SetString(++num, array.GetText(i));
				break;
			default:
				Debug.LogWarningFormat("Unsupport type {0}", type);
				break;
			}
		}
		return result;
	}
}
