using System;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CenterOnChildWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CenterOnChild);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 1, 1);
		Utils.RegisterFunc(L, -3, "Reset", _m_Reset);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "onCenter", _e_onCenter);
		Utils.RegisterFunc(L, -2, "centerSpeed", _g_get_centerSpeed);
		Utils.RegisterFunc(L, -1, "centerSpeed", _s_set_centerSpeed);
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
				CenterOnChild o = new CenterOnChild();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CenterOnChild constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset(IntPtr L)
	{
		try
		{
			((CenterOnChild)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CenterOnChild centerOnChild = (CenterOnChild)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			centerOnChild.OnEndDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CenterOnChild centerOnChild = (CenterOnChild)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			centerOnChild.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_centerSpeed(IntPtr L)
	{
		try
		{
			CenterOnChild centerOnChild = (CenterOnChild)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, centerOnChild.centerSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_centerSpeed(IntPtr L)
	{
		try
		{
			((CenterOnChild)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).centerSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onCenter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			CenterOnChild centerOnChild = (CenterOnChild)objectTranslator.FastGetCSObj(L, 1);
			CenterOnChild.OnCenterHandler onCenterHandler = objectTranslator.GetDelegate<CenterOnChild.OnCenterHandler>(L, 3);
			if (onCenterHandler == null)
			{
				return Lua.luaL_error(L, "#3 need CenterOnChild.OnCenterHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					centerOnChild.onCenter += onCenterHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					centerOnChild.onCenter -= onCenterHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to CenterOnChild.onCenter!");
		return 0;
	}
}
