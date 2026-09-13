using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUISliderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Slider);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 14, 9, 9);
		Utils.RegisterFunc(L, -3, "SetValueWithoutNotify", _m_SetValueWithoutNotify);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "LayoutComplete", _m_LayoutComplete);
		Utils.RegisterFunc(L, -3, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnMove", _m_OnMove);
		Utils.RegisterFunc(L, -3, "FindSelectableOnLeft", _m_FindSelectableOnLeft);
		Utils.RegisterFunc(L, -3, "FindSelectableOnRight", _m_FindSelectableOnRight);
		Utils.RegisterFunc(L, -3, "FindSelectableOnUp", _m_FindSelectableOnUp);
		Utils.RegisterFunc(L, -3, "FindSelectableOnDown", _m_FindSelectableOnDown);
		Utils.RegisterFunc(L, -3, "OnInitializePotentialDrag", _m_OnInitializePotentialDrag);
		Utils.RegisterFunc(L, -3, "SetDirection", _m_SetDirection);
		Utils.RegisterFunc(L, -3, "DOValue", _m_DOValue);
		Utils.RegisterFunc(L, -2, "fillRect", _g_get_fillRect);
		Utils.RegisterFunc(L, -2, "handleRect", _g_get_handleRect);
		Utils.RegisterFunc(L, -2, "direction", _g_get_direction);
		Utils.RegisterFunc(L, -2, "minValue", _g_get_minValue);
		Utils.RegisterFunc(L, -2, "maxValue", _g_get_maxValue);
		Utils.RegisterFunc(L, -2, "wholeNumbers", _g_get_wholeNumbers);
		Utils.RegisterFunc(L, -2, "value", _g_get_value);
		Utils.RegisterFunc(L, -2, "normalizedValue", _g_get_normalizedValue);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -1, "fillRect", _s_set_fillRect);
		Utils.RegisterFunc(L, -1, "handleRect", _s_set_handleRect);
		Utils.RegisterFunc(L, -1, "direction", _s_set_direction);
		Utils.RegisterFunc(L, -1, "minValue", _s_set_minValue);
		Utils.RegisterFunc(L, -1, "maxValue", _s_set_maxValue);
		Utils.RegisterFunc(L, -1, "wholeNumbers", _s_set_wholeNumbers);
		Utils.RegisterFunc(L, -1, "value", _s_set_value);
		Utils.RegisterFunc(L, -1, "normalizedValue", _s_set_normalizedValue);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Slider does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetValueWithoutNotify(IntPtr L)
	{
		try
		{
			Slider obj = (Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float valueWithoutNotify = (float)Lua.lua_tonumber(L, 2);
			obj.SetValueWithoutNotify(valueWithoutNotify);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			slider.Rebuild(v);
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
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LayoutComplete();
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
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GraphicUpdateComplete();
			return 0;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			slider.OnPointerDown(eventData);
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
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			slider.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			AxisEventData eventData = (AxisEventData)objectTranslator.GetObject(L, 2, typeof(AxisEventData));
			slider.OnMove(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindSelectableOnLeft(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable o = ((Slider)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnLeft();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindSelectableOnRight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable o = ((Slider)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnRight();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindSelectableOnUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable o = ((Slider)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnUp();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindSelectableOnDown(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable o = ((Slider)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnDown();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnInitializePotentialDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			slider.OnInitializePotentialDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDirection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Slider.Direction val);
			bool includeRectLayouts = Lua.lua_toboolean(L, 3);
			slider.SetDirection(val, includeRectLayouts);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider target = (Slider)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<float, float, FloatOptions> o = target.DOValue(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<float, float, FloatOptions> o2 = target.DOValue(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Slider.DOValue!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, slider.fillRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_handleRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, slider.handleRect);
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
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUISliderDirection(L, slider.direction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minValue(IntPtr L)
	{
		try
		{
			Slider slider = (Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, slider.minValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxValue(IntPtr L)
	{
		try
		{
			Slider slider = (Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, slider.maxValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wholeNumbers(IntPtr L)
	{
		try
		{
			Slider slider = (Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, slider.wholeNumbers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_value(IntPtr L)
	{
		try
		{
			Slider slider = (Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, slider.value);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalizedValue(IntPtr L)
	{
		try
		{
			Slider slider = (Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, slider.normalizedValue);
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
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, slider.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Slider)objectTranslator.FastGetCSObj(L, 1)).fillRect = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_handleRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Slider)objectTranslator.FastGetCSObj(L, 1)).handleRect = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_direction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Slider slider = (Slider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Slider.Direction val);
			slider.direction = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minValue(IntPtr L)
	{
		try
		{
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).minValue = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxValue(IntPtr L)
	{
		try
		{
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxValue = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wholeNumbers(IntPtr L)
	{
		try
		{
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).wholeNumbers = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_value(IntPtr L)
	{
		try
		{
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).value = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normalizedValue(IntPtr L)
	{
		try
		{
			((Slider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).normalizedValue = (float)Lua.lua_tonumber(L, 2);
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
			((Slider)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (Slider.SliderEvent)objectTranslator.GetObject(L, 2, typeof(Slider.SliderEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
