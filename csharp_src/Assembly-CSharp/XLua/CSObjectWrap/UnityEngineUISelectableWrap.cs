using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUISelectableWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Selectable);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 14, 9, 8);
		Utils.RegisterFunc(L, -3, "IsInteractable", _m_IsInteractable);
		Utils.RegisterFunc(L, -3, "FindSelectable", _m_FindSelectable);
		Utils.RegisterFunc(L, -3, "FindSelectableOnLeft", _m_FindSelectableOnLeft);
		Utils.RegisterFunc(L, -3, "FindSelectableOnRight", _m_FindSelectableOnRight);
		Utils.RegisterFunc(L, -3, "FindSelectableOnUp", _m_FindSelectableOnUp);
		Utils.RegisterFunc(L, -3, "FindSelectableOnDown", _m_FindSelectableOnDown);
		Utils.RegisterFunc(L, -3, "OnMove", _m_OnMove);
		Utils.RegisterFunc(L, -3, "OnPointerDown", _m_OnPointerDown);
		Utils.RegisterFunc(L, -3, "OnPointerUp", _m_OnPointerUp);
		Utils.RegisterFunc(L, -3, "OnPointerEnter", _m_OnPointerEnter);
		Utils.RegisterFunc(L, -3, "OnPointerExit", _m_OnPointerExit);
		Utils.RegisterFunc(L, -3, "OnSelect", _m_OnSelect);
		Utils.RegisterFunc(L, -3, "OnDeselect", _m_OnDeselect);
		Utils.RegisterFunc(L, -3, "Select", _m_Select);
		Utils.RegisterFunc(L, -2, "navigation", _g_get_navigation);
		Utils.RegisterFunc(L, -2, "transition", _g_get_transition);
		Utils.RegisterFunc(L, -2, "colors", _g_get_colors);
		Utils.RegisterFunc(L, -2, "spriteState", _g_get_spriteState);
		Utils.RegisterFunc(L, -2, "animationTriggers", _g_get_animationTriggers);
		Utils.RegisterFunc(L, -2, "targetGraphic", _g_get_targetGraphic);
		Utils.RegisterFunc(L, -2, "interactable", _g_get_interactable);
		Utils.RegisterFunc(L, -2, "image", _g_get_image);
		Utils.RegisterFunc(L, -2, "animator", _g_get_animator);
		Utils.RegisterFunc(L, -1, "navigation", _s_set_navigation);
		Utils.RegisterFunc(L, -1, "transition", _s_set_transition);
		Utils.RegisterFunc(L, -1, "colors", _s_set_colors);
		Utils.RegisterFunc(L, -1, "spriteState", _s_set_spriteState);
		Utils.RegisterFunc(L, -1, "animationTriggers", _s_set_animationTriggers);
		Utils.RegisterFunc(L, -1, "targetGraphic", _s_set_targetGraphic);
		Utils.RegisterFunc(L, -1, "interactable", _s_set_interactable);
		Utils.RegisterFunc(L, -1, "image", _s_set_image);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 2, 0);
		Utils.RegisterFunc(L, -4, "AllSelectablesNoAlloc", _m_AllSelectablesNoAlloc_xlua_st_);
		Utils.RegisterFunc(L, -2, "allSelectablesArray", _g_get_allSelectablesArray);
		Utils.RegisterFunc(L, -2, "allSelectableCount", _g_get_allSelectableCount);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Selectable does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AllSelectablesNoAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			int value = Selectable.AllSelectablesNoAlloc((Selectable[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Selectable[])));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInteractable(IntPtr L)
	{
		try
		{
			bool value = ((Selectable)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInteractable();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindSelectable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Selectable o = selectable.FindSelectable(val);
			objectTranslator.Push(L, o);
			return 1;
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
			Selectable o = ((Selectable)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnLeft();
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
			Selectable o = ((Selectable)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnRight();
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
			Selectable o = ((Selectable)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnUp();
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
			Selectable o = ((Selectable)objectTranslator.FastGetCSObj(L, 1)).FindSelectableOnDown();
			objectTranslator.Push(L, o);
			return 1;
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
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			AxisEventData eventData = (AxisEventData)objectTranslator.GetObject(L, 2, typeof(AxisEventData));
			selectable.OnMove(eventData);
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
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			selectable.OnPointerDown(eventData);
			return 0;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			selectable.OnPointerUp(eventData);
			return 0;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			selectable.OnPointerEnter(eventData);
			return 0;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			selectable.OnPointerExit(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSelect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			selectable.OnSelect(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDeselect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			selectable.OnDeselect(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Select(IntPtr L)
	{
		try
		{
			((Selectable)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Select();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allSelectablesArray(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Selectable.allSelectablesArray);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allSelectableCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Selectable.allSelectableCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_navigation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.navigation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUISelectableTransition(L, selectable.transition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.colors);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.spriteState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animationTriggers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.animationTriggers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetGraphic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.targetGraphic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interactable(IntPtr L)
	{
		try
		{
			Selectable selectable = (Selectable)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, selectable.interactable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_image(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.image);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, selectable.animator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_navigation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Navigation v);
			selectable.navigation = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_transition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Selectable.Transition val);
			selectable.transition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ColorBlock v);
			selectable.colors = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Selectable selectable = (Selectable)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SpriteState v);
			selectable.spriteState = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animationTriggers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Selectable)objectTranslator.FastGetCSObj(L, 1)).animationTriggers = (AnimationTriggers)objectTranslator.GetObject(L, 2, typeof(AnimationTriggers));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetGraphic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Selectable)objectTranslator.FastGetCSObj(L, 1)).targetGraphic = (Graphic)objectTranslator.GetObject(L, 2, typeof(Graphic));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_interactable(IntPtr L)
	{
		try
		{
			((Selectable)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).interactable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_image(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Selectable)objectTranslator.FastGetCSObj(L, 1)).image = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
