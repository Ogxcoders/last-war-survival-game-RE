using System;
using Framework.Utils.UnityEx;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FrameworkUtilsUnityExScreenToucherWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ScreenToucher);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 4, 1);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "IsPointerOverUIObject", _m_IsPointerOverUIObject);
		Utils.RegisterFunc(L, -2, "_pointerDownPosi", _g_get__pointerDownPosi);
		Utils.RegisterFunc(L, -2, "_pointerDownPosiIgnoreUI", _g_get__pointerDownPosiIgnoreUI);
		Utils.RegisterFunc(L, -2, "_tempMovePosi", _g_get__tempMovePosi);
		Utils.RegisterFunc(L, -2, "eventSystem", _g_get_eventSystem);
		Utils.RegisterFunc(L, -1, "eventSystem", _s_set_eventSystem);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 1, 0);
		Utils.RegisterFunc(L, -4, "RegisterScreenRect", _m_RegisterScreenRect_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearInterestedRect", _m_ClearInterestedRect_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				ScreenToucher o = new ScreenToucher();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Framework.Utils.UnityEx.ScreenToucher constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((ScreenToucher)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPointerOverUIObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScreenToucher screenToucher = (ScreenToucher)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector2>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				bool value = screenToucher.IsPointerOverUIObject(val);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				int fingerID = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val2);
				bool value2 = screenToucher.IsPointerOverUIObject(fingerID, val2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Framework.Utils.UnityEx.ScreenToucher.IsPointerOverUIObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterScreenRect_xlua_st_(IntPtr L)
	{
		try
		{
			ScreenToucher.RegisterScreenRect((RectTransform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RectTransform)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearInterestedRect_xlua_st_(IntPtr L)
	{
		try
		{
			ScreenToucher.ClearInterestedRect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, ScreenToucher.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__pointerDownPosi(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScreenToucher screenToucher = (ScreenToucher)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, screenToucher._pointerDownPosi);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__pointerDownPosiIgnoreUI(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScreenToucher screenToucher = (ScreenToucher)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, screenToucher._pointerDownPosiIgnoreUI);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__tempMovePosi(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScreenToucher screenToucher = (ScreenToucher)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, screenToucher._tempMovePosi);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eventSystem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScreenToucher screenToucher = (ScreenToucher)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, screenToucher.eventSystem);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eventSystem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScreenToucher)objectTranslator.FastGetCSObj(L, 1)).eventSystem = (EventSystem)objectTranslator.GetObject(L, 2, typeof(EventSystem));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
