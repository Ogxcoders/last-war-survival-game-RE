using System;
using TMPro;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_TextUtilitiesWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_TextUtilities);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 21, 0, 0);
		Utils.RegisterFunc(L, -4, "GetCursorIndexFromPosition", _m_GetCursorIndexFromPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindNearestLine", _m_FindNearestLine_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindNearestCharacterOnLine", _m_FindNearestCharacterOnLine_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsIntersectingRectTransform", _m_IsIntersectingRectTransform_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindIntersectingCharacter", _m_FindIntersectingCharacter_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindNearestCharacter", _m_FindNearestCharacter_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindIntersectingWord", _m_FindIntersectingWord_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindNearestWord", _m_FindNearestWord_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindIntersectingLine", _m_FindIntersectingLine_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindIntersectingLink", _m_FindIntersectingLink_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindNearestLink", _m_FindNearestLink_xlua_st_);
		Utils.RegisterFunc(L, -4, "ScreenPointToWorldPointInRectangle", _m_ScreenPointToWorldPointInRectangle_xlua_st_);
		Utils.RegisterFunc(L, -4, "DistanceToLine", _m_DistanceToLine_xlua_st_);
		Utils.RegisterFunc(L, -4, "ToLowerFast", _m_ToLowerFast_xlua_st_);
		Utils.RegisterFunc(L, -4, "ToUpperFast", _m_ToUpperFast_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetHashCode", _m_GetHashCode_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSimpleHashCode", _m_GetSimpleHashCode_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSimpleHashCodeLowercase", _m_GetSimpleHashCodeLowercase_xlua_st_);
		Utils.RegisterFunc(L, -4, "HexToInt", _m_HexToInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "StringHexToInt", _m_StringHexToInt_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "TMPro.TMP_TextUtilities does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCursorIndexFromPosition_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<TMP_Text>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera>(L, 3))
			{
				TMP_Text textComponent = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
				objectTranslator.Get(L, 2, out Vector3 val);
				int value = TMP_TextUtilities.GetCursorIndexFromPosition(camera: (Camera)objectTranslator.GetObject(L, 3, typeof(Camera)), textComponent: textComponent, position: val);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<TMP_Text>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera>(L, 3))
			{
				TMP_Text textComponent2 = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
				objectTranslator.Get(L, 2, out Vector3 val2);
				CaretPosition cursor;
				int value2 = TMP_TextUtilities.GetCursorIndexFromPosition(camera: (Camera)objectTranslator.GetObject(L, 3, typeof(Camera)), textComponent: textComponent2, position: val2, cursor: out cursor);
				Lua.xlua_pushinteger(L, value2);
				objectTranslator.Push(L, cursor);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_TextUtilities.GetCursorIndexFromPosition!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindNearestLine_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			int value = TMP_TextUtilities.FindNearestLine(text, val, camera);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindNearestCharacterOnLine_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			int line = Lua.xlua_tointeger(L, 3);
			Camera camera = (Camera)objectTranslator.GetObject(L, 4, typeof(Camera));
			bool visibleOnly = Lua.lua_toboolean(L, 5);
			int value = TMP_TextUtilities.FindNearestCharacterOnLine(text, val, line, camera, visibleOnly);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsIntersectingRectTransform_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = TMP_TextUtilities.IsIntersectingRectTransform(rectTransform, val, camera);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindIntersectingCharacter_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool visibleOnly = Lua.lua_toboolean(L, 4);
			int value = TMP_TextUtilities.FindIntersectingCharacter(text, val, camera, visibleOnly);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindNearestCharacter_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool visibleOnly = Lua.lua_toboolean(L, 4);
			int value = TMP_TextUtilities.FindNearestCharacter(text, val, camera, visibleOnly);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindIntersectingWord_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			int value = TMP_TextUtilities.FindIntersectingWord(text, val, camera);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindNearestWord_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			int value = TMP_TextUtilities.FindNearestWord(text, val, camera);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindIntersectingLine_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			int value = TMP_TextUtilities.FindIntersectingLine(text, val, camera);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindIntersectingLink_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			int value = TMP_TextUtilities.FindIntersectingLink(text, val, camera);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindNearestLink_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Text text = (TMP_Text)objectTranslator.GetObject(L, 1, typeof(TMP_Text));
			objectTranslator.Get(L, 2, out Vector3 val);
			Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			int value = TMP_TextUtilities.FindNearestLink(text, val, camera);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToWorldPointInRectangle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			objectTranslator.Get(L, 2, out Vector2 val);
			Vector3 worldPoint;
			bool value = TMP_TextUtilities.ScreenPointToWorldPointInRectangle(cam: (Camera)objectTranslator.GetObject(L, 3, typeof(Camera)), transform: transform, screenPoint: val, worldPoint: out worldPoint);
			Lua.lua_pushboolean(L, value);
			objectTranslator.PushUnityEngineVector3(L, worldPoint);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DistanceToLine_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			float num = TMP_TextUtilities.DistanceToLine(val, val2, val3);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToLowerFast_xlua_st_(IntPtr L)
	{
		try
		{
			char value = TMP_TextUtilities.ToLowerFast((char)Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToUpperFast_xlua_st_(IntPtr L)
	{
		try
		{
			char value = TMP_TextUtilities.ToUpperFast((char)Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHashCode_xlua_st_(IntPtr L)
	{
		try
		{
			int hashCode = TMP_TextUtilities.GetHashCode(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, hashCode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSimpleHashCode_xlua_st_(IntPtr L)
	{
		try
		{
			int simpleHashCode = TMP_TextUtilities.GetSimpleHashCode(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, simpleHashCode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSimpleHashCodeLowercase_xlua_st_(IntPtr L)
	{
		try
		{
			uint simpleHashCodeLowercase = TMP_TextUtilities.GetSimpleHashCodeLowercase(Lua.lua_tostring(L, 1));
			Lua.xlua_pushuint(L, simpleHashCodeLowercase);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HexToInt_xlua_st_(IntPtr L)
	{
		try
		{
			int value = TMP_TextUtilities.HexToInt((char)Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringHexToInt_xlua_st_(IntPtr L)
	{
		try
		{
			int value = TMP_TextUtilities.StringHexToInt(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
