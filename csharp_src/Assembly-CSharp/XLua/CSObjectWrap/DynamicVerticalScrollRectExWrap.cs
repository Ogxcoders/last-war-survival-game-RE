using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DynamicVerticalScrollRectExWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DynamicVerticalScrollRectEx);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 18, 6, 5);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "SetDatas", _m_SetDatas);
		Utils.RegisterFunc(L, -3, "SetOverrideItemHeight", _m_SetOverrideItemHeight);
		Utils.RegisterFunc(L, -3, "FindItemByDataIdx", _m_FindItemByDataIdx);
		Utils.RegisterFunc(L, -3, "GetScrollOffsetOfDataIdx", _m_GetScrollOffsetOfDataIdx);
		Utils.RegisterFunc(L, -3, "SetScrollOffset", _m_SetScrollOffset);
		Utils.RegisterFunc(L, -3, "Rebuild", _m_Rebuild);
		Utils.RegisterFunc(L, -3, "UpdateItems", _m_UpdateItems);
		Utils.RegisterFunc(L, -3, "OnScroll", _m_OnScroll);
		Utils.RegisterFunc(L, -3, "OnInitializePotentialDrag", _m_OnInitializePotentialDrag);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "onInstantiateItem", _e_onInstantiateItem);
		Utils.RegisterFunc(L, -3, "onDisplayItem", _e_onDisplayItem);
		Utils.RegisterFunc(L, -3, "onClearItem", _e_onClearItem);
		Utils.RegisterFunc(L, -3, "onOffsetChanged", _e_onOffsetChanged);
		Utils.RegisterFunc(L, -3, "onArabicMirrorItemInstantiated", _e_onArabicMirrorItemInstantiated);
		Utils.RegisterFunc(L, -2, "contentH", _g_get_contentH);
		Utils.RegisterFunc(L, -2, "normalizedOffset", _g_get_normalizedOffset);
		Utils.RegisterFunc(L, -2, "itemPrefabs", _g_get_itemPrefabs);
		Utils.RegisterFunc(L, -2, "itemHeights", _g_get_itemHeights);
		Utils.RegisterFunc(L, -2, "padding", _g_get_padding);
		Utils.RegisterFunc(L, -2, "margin", _g_get_margin);
		Utils.RegisterFunc(L, -1, "normalizedOffset", _s_set_normalizedOffset);
		Utils.RegisterFunc(L, -1, "itemPrefabs", _s_set_itemPrefabs);
		Utils.RegisterFunc(L, -1, "itemHeights", _s_set_itemHeights);
		Utils.RegisterFunc(L, -1, "padding", _s_set_padding);
		Utils.RegisterFunc(L, -1, "margin", _s_set_margin);
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
				DynamicVerticalScrollRectEx o = new DynamicVerticalScrollRectEx();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRectEx constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDatas(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			int[] datas = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			dynamicVerticalScrollRectEx.SetDatas(datas);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOverrideItemHeight(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx obj = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dataIdx = Lua.xlua_tointeger(L, 2);
			float height = (float)Lua.lua_tonumber(L, 3);
			obj.SetOverrideItemHeight(dataIdx, height);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindItemByDataIdx(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRectEx obj = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			int dataIdx = Lua.xlua_tointeger(L, 2);
			GameObject o = obj.FindItemByDataIdx(dataIdx);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetScrollOffsetOfDataIdx(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx obj = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dataIdx = Lua.xlua_tointeger(L, 2);
			float additionOffset = (float)Lua.lua_tonumber(L, 3);
			float scrollOffsetOfDataIdx = obj.GetScrollOffsetOfDataIdx(dataIdx, additionOffset);
			Lua.lua_pushnumber(L, scrollOffsetOfDataIdx);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetScrollOffset(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx obj = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float scrollOffset = (float)Lua.lua_tonumber(L, 2);
			obj.SetScrollOffset(scrollOffset);
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
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			dynamicVerticalScrollRectEx.Rebuild(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateItems(IntPtr L)
	{
		try
		{
			((DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateItems();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnScroll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRectEx.OnScroll(data);
			return 0;
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
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRectEx.OnInitializePotentialDrag(eventData);
			return 0;
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
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRectEx.OnBeginDrag(eventData);
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
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRectEx.OnEndDrag(eventData);
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
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRectEx.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_contentH(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRectEx.contentH);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalizedOffset(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRectEx.normalizedOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemPrefabs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dynamicVerticalScrollRectEx.itemPrefabs);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemHeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dynamicVerticalScrollRectEx.itemHeights);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_padding(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRectEx.padding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_margin(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRectEx.margin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normalizedOffset(IntPtr L)
	{
		try
		{
			((DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).normalizedOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemPrefabs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1)).itemPrefabs = (List<GameObject>)objectTranslator.GetObject(L, 2, typeof(List<GameObject>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemHeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1)).itemHeights = (List<float>)objectTranslator.GetObject(L, 2, typeof(List<float>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_padding(IntPtr L)
	{
		try
		{
			((DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).padding = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_margin(IntPtr L)
	{
		try
		{
			((DynamicVerticalScrollRectEx)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).margin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onInstantiateItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject, int> action = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject, int>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRectEx.onInstantiateItem += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRectEx.onInstantiateItem -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRectEx.onInstantiateItem!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onDisplayItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject, int> action = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject, int>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRectEx.onDisplayItem += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRectEx.onDisplayItem -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRectEx.onDisplayItem!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onClearItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject> action = objectTranslator.GetDelegate<Action<GameObject>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRectEx.onClearItem += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRectEx.onClearItem -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRectEx.onClearItem!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onOffsetChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			Action<float> action = objectTranslator.GetDelegate<Action<float>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<float>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRectEx.onOffsetChanged += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRectEx.onOffsetChanged -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRectEx.onOffsetChanged!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onArabicMirrorItemInstantiated(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRectEx dynamicVerticalScrollRectEx = (DynamicVerticalScrollRectEx)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject> action = objectTranslator.GetDelegate<Action<GameObject>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRectEx.onArabicMirrorItemInstantiated += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRectEx.onArabicMirrorItemInstantiated -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRectEx.onArabicMirrorItemInstantiated!");
		return 0;
	}
}
