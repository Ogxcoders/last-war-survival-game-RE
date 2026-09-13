using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class XYLayoutItemWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(XYLayoutItem);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 3, 0);
		Utils.RegisterFunc(L, -3, "UpdateSize", _m_UpdateSize);
		Utils.RegisterFunc(L, -3, "PresetCenter", _m_PresetCenter);
		Utils.RegisterFunc(L, -3, "RefreshLocalPosition", _m_RefreshLocalPosition);
		Utils.RegisterFunc(L, -2, "ElementType", _g_get_ElementType);
		Utils.RegisterFunc(L, -2, "Anchor", _g_get_Anchor);
		Utils.RegisterFunc(L, -2, "Size", _g_get_Size);
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
				XYLayoutItem o = new XYLayoutItem();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to XYLayoutItem constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSize(IntPtr L)
	{
		try
		{
			XYLayoutItem obj = (XYLayoutItem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float width = (float)Lua.lua_tonumber(L, 2);
			float height = (float)Lua.lua_tonumber(L, 3);
			obj.UpdateSize(width, height);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PresetCenter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutItem xYLayoutItem = (XYLayoutItem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			xYLayoutItem.PresetCenter(val);
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
			XYLayoutItem xYLayoutItem = (XYLayoutItem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			xYLayoutItem.RefreshLocalPosition(val);
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
			XYLayoutItem xYLayoutItem = (XYLayoutItem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, xYLayoutItem.ElementType);
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
			XYLayoutItem xYLayoutItem = (XYLayoutItem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, xYLayoutItem.Anchor);
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
			XYLayoutItem xYLayoutItem = (XYLayoutItem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, xYLayoutItem.Size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
