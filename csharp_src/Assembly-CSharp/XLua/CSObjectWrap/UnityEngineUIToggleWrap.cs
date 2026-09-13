using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIToggleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Toggle);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 5, 5);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "SetIsOnWithoutNotify", _m_SetIsOnWithoutNotify);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnSubmit", _m_OnSubmit);
		Utils.RegisterFunc(L, -2, "group", _g_get_group);
		Utils.RegisterFunc(L, -2, "isOn", _g_get_isOn);
		Utils.RegisterFunc(L, -2, "toggleTransition", _g_get_toggleTransition);
		Utils.RegisterFunc(L, -2, "graphic", _g_get_graphic);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -1, "group", _s_set_group);
		Utils.RegisterFunc(L, -1, "isOn", _s_set_isOn);
		Utils.RegisterFunc(L, -1, "toggleTransition", _s_set_toggleTransition);
		Utils.RegisterFunc(L, -1, "graphic", _s_set_graphic);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Toggle does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			toggle.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LayoutComplete(IntPtr L)
	{
		try
		{
			((Toggle)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GraphicUpdateComplete(IntPtr L)
	{
		try
		{
			((Toggle)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsOnWithoutNotify(IntPtr L)
	{
		try
		{
			Toggle obj = (Toggle)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isOnWithoutNotify = Lua.lua_toboolean(L, 2);
			obj.SetIsOnWithoutNotify(isOnWithoutNotify);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			toggle.OnPointerClick(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSubmit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			toggle.OnSubmit(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_group(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, toggle.group);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isOn(IntPtr L)
	{
		try
		{
			Toggle toggle = (Toggle)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, toggle.isOn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_toggleTransition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIToggleToggleTransition(L, toggle.toggleTransition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, toggle.graphic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, toggle.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_group(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Toggle)objectTranslator.FastGetCSObj(L, 1)).group = (ToggleGroup)objectTranslator.GetObject(L, 2, typeof(ToggleGroup));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isOn(IntPtr L)
	{
		try
		{
			((Toggle)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isOn = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_toggleTransition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Toggle toggle = (Toggle)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Toggle.ToggleTransition val);
			toggle.toggleTransition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_graphic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Toggle)objectTranslator.FastGetCSObj(L, 1)).graphic = (Graphic)objectTranslator.GetObject(L, 2, typeof(Graphic));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Toggle)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (Toggle.ToggleEvent)objectTranslator.GetObject(L, 2, typeof(Toggle.ToggleEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
