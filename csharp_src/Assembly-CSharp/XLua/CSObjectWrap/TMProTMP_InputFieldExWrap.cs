using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_InputFieldExWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_InputFieldEx);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 0, 0);
		Utils.RegisterFunc(L, -3, "CheckisArabicLang", _m_CheckisArabicLang);
		Utils.RegisterFunc(L, -3, "InputFieldContentForceMeshUpdate", _m_InputFieldContentForceMeshUpdate);
		Utils.RegisterFunc(L, -3, "GetInputFieldLineCount", _m_GetInputFieldLineCount);
		Utils.RegisterFunc(L, -3, "GetInputFieldLineHeightByNum", _m_GetInputFieldLineHeightByNum);
		Utils.RegisterFunc(L, -3, "InsertStrInCaretPos", _m_InsertStrInCaretPos);
		Utils.RegisterFunc(L, -3, "InitTMPComponents", _m_InitTMPComponents);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				TMP_InputFieldEx o = new TMP_InputFieldEx();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_InputFieldEx constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckisArabicLang(IntPtr L)
	{
		try
		{
			((TMP_InputFieldEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckisArabicLang();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InputFieldContentForceMeshUpdate(IntPtr L)
	{
		try
		{
			((TMP_InputFieldEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InputFieldContentForceMeshUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInputFieldLineCount(IntPtr L)
	{
		try
		{
			int inputFieldLineCount = ((TMP_InputFieldEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetInputFieldLineCount();
			Lua.xlua_pushinteger(L, inputFieldLineCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInputFieldLineHeightByNum(IntPtr L)
	{
		try
		{
			TMP_InputFieldEx obj = (TMP_InputFieldEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lineNum = Lua.xlua_tointeger(L, 2);
			float inputFieldLineHeightByNum = obj.GetInputFieldLineHeightByNum(lineNum);
			Lua.lua_pushnumber(L, inputFieldLineHeightByNum);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertStrInCaretPos(IntPtr L)
	{
		try
		{
			TMP_InputFieldEx obj = (TMP_InputFieldEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string insertStr = Lua.lua_tostring(L, 2);
			obj.InsertStrInCaretPos(insertStr);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitTMPComponents(IntPtr L)
	{
		try
		{
			((TMP_InputFieldEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitTMPComponents();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
