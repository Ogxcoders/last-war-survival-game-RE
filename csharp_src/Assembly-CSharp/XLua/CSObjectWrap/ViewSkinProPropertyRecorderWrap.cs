using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ViewSkinProPropertyRecorderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ViewSkinProPropertyRecorder);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 0, 0);
		Utils.RegisterFunc(L, -3, "GetAllProperties", _m_GetAllProperties);
		Utils.RegisterFunc(L, -3, "GetProperty", _m_GetProperty);
		Utils.RegisterFunc(L, -3, "FillLua", _m_FillLua);
		Utils.RegisterFunc(L, -3, "SetCodeGuid", _m_SetCodeGuid);
		Utils.RegisterFunc(L, -3, "GetCodeGuid", _m_GetCodeGuid);
		Utils.RegisterFunc(L, -3, "GetCodeType", _m_GetCodeType);
		Utils.RegisterFunc(L, -3, "SetCodeType", _m_SetCodeType);
		Utils.RegisterFunc(L, -3, "AddProperty", _m_AddProperty);
		Utils.RegisterFunc(L, -3, "ClearProperties", _m_ClearProperties);
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
				ViewSkinProPropertyRecorder o = new ViewSkinProPropertyRecorder();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ViewSkinProPropertyRecorder constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllProperties(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<ViewSkinProPropertyRecorder.ViewSkinProProperty> allProperties = ((ViewSkinProPropertyRecorder)objectTranslator.FastGetCSObj(L, 1)).GetAllProperties();
			objectTranslator.Push(L, allProperties);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetProperty(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder obj = (ViewSkinProPropertyRecorder)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			UnityEngine.Object property = obj.GetProperty(index);
			objectTranslator.Push(L, property);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FillLua(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder viewSkinProPropertyRecorder = (ViewSkinProPropertyRecorder)objectTranslator.FastGetCSObj(L, 1);
			LuaTable luaTable = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			viewSkinProPropertyRecorder.FillLua(luaTable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCodeGuid(IntPtr L)
	{
		try
		{
			ViewSkinProPropertyRecorder obj = (ViewSkinProPropertyRecorder)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string codeGuid = Lua.lua_tostring(L, 2);
			obj.SetCodeGuid(codeGuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCodeGuid(IntPtr L)
	{
		try
		{
			string codeGuid = ((ViewSkinProPropertyRecorder)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCodeGuid();
			Lua.lua_pushstring(L, codeGuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCodeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder.CodeType codeType = ((ViewSkinProPropertyRecorder)objectTranslator.FastGetCSObj(L, 1)).GetCodeType();
			objectTranslator.PushViewSkinProPropertyRecorderCodeType(L, codeType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCodeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder viewSkinProPropertyRecorder = (ViewSkinProPropertyRecorder)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ViewSkinProPropertyRecorder.CodeType val);
			viewSkinProPropertyRecorder.SetCodeType(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddProperty(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder viewSkinProPropertyRecorder = (ViewSkinProPropertyRecorder)objectTranslator.FastGetCSObj(L, 1);
			ViewSkinProPropertyRecorder.ViewSkinProProperty property = (ViewSkinProPropertyRecorder.ViewSkinProProperty)objectTranslator.GetObject(L, 2, typeof(ViewSkinProPropertyRecorder.ViewSkinProProperty));
			bool value = viewSkinProPropertyRecorder.AddProperty(property);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearProperties(IntPtr L)
	{
		try
		{
			((ViewSkinProPropertyRecorder)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearProperties();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
