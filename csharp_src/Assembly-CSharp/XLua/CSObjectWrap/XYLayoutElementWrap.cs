using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class XYLayoutElementWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(XYLayoutElement);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 4, 0);
		Utils.RegisterFunc(L, -3, "PresetCenter", _m_PresetCenter);
		Utils.RegisterFunc(L, -3, "RefreshLocalPosition", _m_RefreshLocalPosition);
		Utils.RegisterFunc(L, -3, "TryUpdateParentLayout", _m_TryUpdateParentLayout);
		Utils.RegisterFunc(L, -2, "ElementType", _g_get_ElementType);
		Utils.RegisterFunc(L, -2, "LocalRect", _g_get_LocalRect);
		Utils.RegisterFunc(L, -2, "Anchor", _g_get_Anchor);
		Utils.RegisterFunc(L, -2, "Size", _g_get_Size);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "XYLayoutElement does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PresetCenter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutElement xYLayoutElement = (XYLayoutElement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			xYLayoutElement.PresetCenter(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshLocalPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutElement xYLayoutElement = (XYLayoutElement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			xYLayoutElement.RefreshLocalPosition(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryUpdateParentLayout(IntPtr L)
	{
		try
		{
			((XYLayoutElement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TryUpdateParentLayout();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ElementType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutElement xYLayoutElement = (XYLayoutElement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, xYLayoutElement.ElementType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LocalRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutElement xYLayoutElement = (XYLayoutElement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, xYLayoutElement.LocalRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Anchor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutElement xYLayoutElement = (XYLayoutElement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, xYLayoutElement.Anchor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutElement xYLayoutElement = (XYLayoutElement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, xYLayoutElement.Size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
