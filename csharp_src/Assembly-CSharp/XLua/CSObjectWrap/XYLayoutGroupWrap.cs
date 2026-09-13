using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class XYLayoutGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(XYLayoutGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 7, 3);
		Utils.RegisterFunc(L, -3, "RegisterElement", _m_RegisterElement);
		Utils.RegisterFunc(L, -3, "UnregisterElement", _m_UnregisterElement);
		Utils.RegisterFunc(L, -3, "RefreshLayout", _m_RefreshLayout);
		Utils.RegisterFunc(L, -3, "PresetCenter", _m_PresetCenter);
		Utils.RegisterFunc(L, -3, "RefreshLocalPosition", _m_RefreshLocalPosition);
		Utils.RegisterFunc(L, -2, "ElementType", _g_get_ElementType);
		Utils.RegisterFunc(L, -2, "Anchor", _g_get_Anchor);
		Utils.RegisterFunc(L, -2, "Size", _g_get_Size);
		Utils.RegisterFunc(L, -2, "IsDirty", _g_get_IsDirty);
		Utils.RegisterFunc(L, -2, "direction", _g_get_direction);
		Utils.RegisterFunc(L, -2, "childAlignment", _g_get_childAlignment);
		Utils.RegisterFunc(L, -2, "spacing", _g_get_spacing);
		Utils.RegisterFunc(L, -1, "direction", _s_set_direction);
		Utils.RegisterFunc(L, -1, "childAlignment", _s_set_childAlignment);
		Utils.RegisterFunc(L, -1, "spacing", _s_set_spacing);
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
				XYLayoutGroup o = new XYLayoutGroup();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to XYLayoutGroup constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterElement(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			XYLayoutElement element = (XYLayoutElement)objectTranslator.GetObject(L, 2, typeof(XYLayoutElement));
			xYLayoutGroup.RegisterElement(element);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterElement(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			XYLayoutElement element = (XYLayoutElement)objectTranslator.GetObject(L, 2, typeof(XYLayoutElement));
			xYLayoutGroup.UnregisterElement(element);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshLayout(IntPtr L)
	{
		try
		{
			((XYLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshLayout();
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
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			xYLayoutGroup.PresetCenter(val);
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
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			xYLayoutGroup.RefreshLocalPosition(val);
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
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, xYLayoutGroup.ElementType);
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
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, xYLayoutGroup.Anchor);
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
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, xYLayoutGroup.Size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsDirty(IntPtr L)
	{
		try
		{
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, xYLayoutGroup.IsDirty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_direction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, xYLayoutGroup.direction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineTextAnchor(L, xYLayoutGroup.childAlignment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spacing(IntPtr L)
	{
		try
		{
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, xYLayoutGroup.spacing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_direction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out XYLayoutDirection v);
			xYLayoutGroup.direction = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_childAlignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			XYLayoutGroup xYLayoutGroup = (XYLayoutGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TextAnchor val);
			xYLayoutGroup.childAlignment = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spacing(IntPtr L)
	{
		try
		{
			((XYLayoutGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).spacing = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
