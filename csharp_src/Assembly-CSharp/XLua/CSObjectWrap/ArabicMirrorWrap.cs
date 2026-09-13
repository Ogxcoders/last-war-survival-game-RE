using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ArabicMirrorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ArabicMirror);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 4, 4);
		Utils.RegisterFunc(L, -3, "Awake", _m_Awake);
		Utils.RegisterFunc(L, -3, "WriteData", _m_WriteData);
		Utils.RegisterFunc(L, -3, "RecordOriginalData", _m_RecordOriginalData);
		Utils.RegisterFunc(L, -3, "SwitchData", _m_SwitchData);
		Utils.RegisterFunc(L, -3, "ClearData", _m_ClearData);
		Utils.RegisterFunc(L, -2, "mirrorObjects", _g_get_mirrorObjects);
		Utils.RegisterFunc(L, -2, "IsApplyAutoMirror", _g_get_IsApplyAutoMirror);
		Utils.RegisterFunc(L, -2, "IsNoLuaControl", _g_get_IsNoLuaControl);
		Utils.RegisterFunc(L, -2, "IsInnerMirror", _g_get_IsInnerMirror);
		Utils.RegisterFunc(L, -1, "mirrorObjects", _s_set_mirrorObjects);
		Utils.RegisterFunc(L, -1, "IsApplyAutoMirror", _s_set_IsApplyAutoMirror);
		Utils.RegisterFunc(L, -1, "IsNoLuaControl", _s_set_IsNoLuaControl);
		Utils.RegisterFunc(L, -1, "IsInnerMirror", _s_set_IsInnerMirror);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "MirrorEntry", _m_MirrorEntry_xlua_st_);
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
				ArabicMirror o = new ArabicMirror();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ArabicMirror constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MirrorEntry_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			bool isInnerMirror = Lua.lua_toboolean(L, 1);
			bool isProcessRootAnchorAndPivot = Lua.lua_toboolean(L, 2);
			GameObject go = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			ArabicMirror.MirrorEntry(isInnerMirror, isProcessRootAnchorAndPivot, go);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Awake(IntPtr L)
	{
		try
		{
			((ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Awake();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WriteData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicMirror arabicMirror = (ArabicMirror)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			arabicMirror.WriteData(index, gameObject);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordOriginalData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicMirror arabicMirror = (ArabicMirror)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			arabicMirror.RecordOriginalData(index, gameObject);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SwitchData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicMirror arabicMirror = (ArabicMirror)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			bool value = arabicMirror.SwitchData(index, gameObject);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearData(IntPtr L)
	{
		try
		{
			ArabicMirror obj = (ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.ClearData(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mirrorObjects(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicMirror arabicMirror = (ArabicMirror)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, arabicMirror.mirrorObjects);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsApplyAutoMirror(IntPtr L)
	{
		try
		{
			ArabicMirror arabicMirror = (ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, arabicMirror.IsApplyAutoMirror);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsNoLuaControl(IntPtr L)
	{
		try
		{
			ArabicMirror arabicMirror = (ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, arabicMirror.IsNoLuaControl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInnerMirror(IntPtr L)
	{
		try
		{
			ArabicMirror arabicMirror = (ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, arabicMirror.IsInnerMirror);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mirrorObjects(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ArabicMirror)objectTranslator.FastGetCSObj(L, 1)).mirrorObjects = (List<GameObject>)objectTranslator.GetObject(L, 2, typeof(List<GameObject>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsApplyAutoMirror(IntPtr L)
	{
		try
		{
			((ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsApplyAutoMirror = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsNoLuaControl(IntPtr L)
	{
		try
		{
			((ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNoLuaControl = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsInnerMirror(IntPtr L)
	{
		try
		{
			((ArabicMirror)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInnerMirror = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
