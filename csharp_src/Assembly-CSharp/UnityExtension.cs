using System;
using BaseUtils;
using GameFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using XLua.LuaDLL;

public static class UnityExtension
{
	private static object[] arrParam = new object[10];

	public static void SetTimeStamp(this Text text, long leftMilliSecond)
	{
		text.text = GameEntry.Timer.MilliSecondToFmtString(leftMilliSecond);
	}

	public static bool PlayId(this SimpleAnimation ani, int stateNameToId)
	{
		string stateName = LuaStringLookupTable.Get(stateNameToId);
		return ani.Play(stateName);
	}

	public static void PlayQueuedId(this SimpleAnimation simpleAni, int stateNameToId)
	{
		string stateName = LuaStringLookupTable.Get(stateNameToId);
		simpleAni.PlayQueued(stateName);
	}

	public static void PlayId(this Animator ani, int stateNameToId, int layerIdx, float normalizedTime)
	{
		string stateName = LuaStringLookupTable.Get(stateNameToId);
		ani.Play(stateName, layerIdx, normalizedTime);
	}

	public static void SetTriggerId(this Animator ani, int triggerNameToId)
	{
		string trigger = LuaStringLookupTable.Get(triggerNameToId);
		ani.SetTrigger(trigger);
	}

	public static Transform FindId(this Transform tran, int pathToId)
	{
		string n = LuaStringLookupTable.Get(pathToId);
		return tran.Find(n);
	}

	private static string GetParamString()
	{
		IntPtr l = GameEntry.Lua.Env.L;
		int num = Lua.lua_gettop(l);
		if (num < 2)
		{
			return "";
		}
		string key = "";
		if (Lua.lua_isinteger(l, 2))
		{
			key = BaseUtils.StringUtils.IntToString(Lua.xlua_tointeger(l, 2));
		}
		else if (Lua.lua_isstring(l, 2))
		{
			key = Lua.lua_tostring(l, 2);
		}
		if (num == 2)
		{
			return GameEntry.Localization.GetString(key, null);
		}
		if (num > 10)
		{
			Log.Error("SetLocalText too much params! max count = 0");
			num = 10;
		}
		int num2 = num - 2;
		for (int i = 0; i < num2; i++)
		{
			int index = i + 3;
			if (Lua.lua_isinteger(l, index))
			{
				arrParam[i] = Lua.xlua_tointeger(l, index);
			}
			else if (Lua.lua_type(l, index) == LuaTypes.LUA_TNUMBER)
			{
				arrParam[i] = Lua.lua_tonumber(l, index);
			}
			else
			{
				arrParam[i] = Lua.lua_tostring(l, index);
			}
			if (arrParam[i] == null)
			{
				arrParam[i] = "";
			}
		}
		Array.Clear(arrParam, num2, arrParam.Length - num2);
		return GameEntry.Localization.GetString(key, arrParam);
	}

	public static void SetLocalText(this Text obj)
	{
		string paramString = GetParamString();
		obj.text = paramString;
	}

	public static void SetLocalText(this TextMeshProUGUI obj)
	{
		string paramString = GetParamString();
		obj.text = paramString;
	}

	public static void SetLocalText(this InputField obj)
	{
		string paramString = GetParamString();
		obj.text = paramString;
	}

	public static void SetLocalText(this TMP_InputField obj)
	{
		string paramString = GetParamString();
		obj.text = paramString;
	}

	public static void SetLocalText(this SuperTextMesh obj)
	{
		string paramString = GetParamString();
		obj.text = paramString;
	}
}
