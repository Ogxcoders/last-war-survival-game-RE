using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TouchObjectEventTriggerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TouchObjectEventTrigger);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 14, 17, 14);
		Utils.RegisterFunc(L, -3, "OnClick", _m_OnClick);
		Utils.RegisterFunc(L, -3, "GetPointInfo", _m_GetPointInfo);
		Utils.RegisterFunc(L, -3, "OnDoubleClick", _m_OnDoubleClick);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnBeginLongTap", _m_OnBeginLongTap);
		Utils.RegisterFunc(L, -3, "OnEndLongTap", _m_OnEndLongTap);
		Utils.RegisterFunc(L, -3, "OnPointerEnter", _m_OnPointerEnter);
		Utils.RegisterFunc(L, -3, "OnPointerExit", _m_OnPointerExit);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnPointerUp", _m_OnPointerUp);
		Utils.RegisterFunc(L, -3, "GetPreviewType", _m_GetPreviewType);
		Utils.RegisterFunc(L, -3, "OnDestroy", _m_OnDestroy);
		Utils.RegisterFunc(L, -2, "Priority", _g_get_Priority);
		Utils.RegisterFunc(L, -2, "TilePos", _g_get_TilePos);
		Utils.RegisterFunc(L, -2, "PreviewType", _g_get_PreviewType);
		Utils.RegisterFunc(L, -2, "onBeginDrag", _g_get_onBeginDrag);
		Utils.RegisterFunc(L, -2, "onDrag", _g_get_onDrag);
		Utils.RegisterFunc(L, -2, "onEndDrag", _g_get_onEndDrag);
		Utils.RegisterFunc(L, -2, "onBeginLongTab", _g_get_onBeginLongTab);
		Utils.RegisterFunc(L, -2, "onEndLongTab", _g_get_onEndLongTab);
		Utils.RegisterFunc(L, -2, "onPointerDown", _g_get_onPointerDown);
		Utils.RegisterFunc(L, -2, "onPointerClick", _g_get_onPointerClick);
		Utils.RegisterFunc(L, -2, "onPointerUp", _g_get_onPointerUp);
		Utils.RegisterFunc(L, -2, "onPointerDoubleClick", _g_get_onPointerDoubleClick);
		Utils.RegisterFunc(L, -2, "onPointerEnter", _g_get_onPointerEnter);
		Utils.RegisterFunc(L, -2, "onPointerExit", _g_get_onPointerExit);
		Utils.RegisterFunc(L, -2, "previewIconPath", _g_get_previewIconPath);
		Utils.RegisterFunc(L, -2, "previewName", _g_get_previewName);
		Utils.RegisterFunc(L, -2, "previewType", _g_get_previewType);
		Utils.RegisterFunc(L, -1, "onBeginDrag", _s_set_onBeginDrag);
		Utils.RegisterFunc(L, -1, "onDrag", _s_set_onDrag);
		Utils.RegisterFunc(L, -1, "onEndDrag", _s_set_onEndDrag);
		Utils.RegisterFunc(L, -1, "onBeginLongTab", _s_set_onBeginLongTab);
		Utils.RegisterFunc(L, -1, "onEndLongTab", _s_set_onEndLongTab);
		Utils.RegisterFunc(L, -1, "onPointerDown", _s_set_onPointerDown);
		Utils.RegisterFunc(L, -1, "onPointerClick", _s_set_onPointerClick);
		Utils.RegisterFunc(L, -1, "onPointerUp", _s_set_onPointerUp);
		Utils.RegisterFunc(L, -1, "onPointerDoubleClick", _s_set_onPointerDoubleClick);
		Utils.RegisterFunc(L, -1, "onPointerEnter", _s_set_onPointerEnter);
		Utils.RegisterFunc(L, -1, "onPointerExit", _s_set_onPointerExit);
		Utils.RegisterFunc(L, -1, "previewIconPath", _s_set_previewIconPath);
		Utils.RegisterFunc(L, -1, "previewName", _s_set_previewName);
		Utils.RegisterFunc(L, -1, "previewType", _s_set_previewType);
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
				TouchObjectEventTrigger o = new TouchObjectEventTrigger();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TouchObjectEventTrigger constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnClick(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClick();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = ((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).GetPointInfo();
			objectTranslator.Push(L, pointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDoubleClick(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDoubleClick();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = touchObjectEventTrigger.OnBeginDrag(val);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = touchObjectEventTrigger.OnEndDrag(val);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			bool value = touchObjectEventTrigger.OnDrag(val, val2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginLongTap(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeginLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndLongTap(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEndLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerEnter(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnPointerEnter();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerExit(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnPointerExit();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerDown(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnPointerDown();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerUp(IntPtr L)
	{
		try
		{
			bool value = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnPointerUp();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPreviewType(IntPtr L)
	{
		try
		{
			int previewType = ((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPreviewType();
			Lua.xlua_pushinteger(L, previewType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDestroy(IntPtr L)
	{
		try
		{
			((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDestroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Priority(IntPtr L)
	{
		try
		{
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, touchObjectEventTrigger.Priority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.TilePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PreviewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.PreviewType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onBeginDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onEndDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onBeginLongTab(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onBeginLongTab);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onEndLongTab(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onEndLongTab);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onPointerDown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onPointerClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onPointerUp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerDoubleClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onPointerDoubleClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onPointerEnter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPointerExit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.onPointerExit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewIconPath(IntPtr L)
	{
		try
		{
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, touchObjectEventTrigger.previewIconPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewName(IntPtr L)
	{
		try
		{
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, touchObjectEventTrigger.previewName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchObjectEventTrigger.previewType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onBeginDrag = objectTranslator.GetDelegate<Action<Vector3>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onDrag = objectTranslator.GetDelegate<Action<Vector3, Vector3>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onEndDrag = objectTranslator.GetDelegate<Action<Vector3>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onBeginLongTab(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onBeginLongTab = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onEndLongTab(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onEndLongTab = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerDown = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerClick = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerUp = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerDoubleClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerDoubleClick = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerEnter = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPointerExit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1)).onPointerExit = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewIconPath(IntPtr L)
	{
		try
		{
			((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).previewIconPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewName(IntPtr L)
	{
		try
		{
			((TouchObjectEventTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).previewName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchObjectEventTrigger touchObjectEventTrigger = (TouchObjectEventTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldPreviewType v);
			touchObjectEventTrigger.previewType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
