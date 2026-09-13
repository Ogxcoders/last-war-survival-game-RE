using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DynamicVerticalScrollRectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DynamicVerticalScrollRect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 16, 7, 5);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "SetDatas", _m_SetDatas);
		Utils.RegisterFunc(L, -3, "GetScrollOffsetOfDataIdx", _m_GetScrollOffsetOfDataIdx);
		Utils.RegisterFunc(L, -3, "SetScrollOffset", _m_SetScrollOffset);
		Utils.RegisterFunc(L, -3, "FindItemByDataIdx", _m_FindItemByDataIdx);
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
		Utils.RegisterFunc(L, -2, "contentW", _g_get_contentW);
		Utils.RegisterFunc(L, -2, "contentH", _g_get_contentH);
		Utils.RegisterFunc(L, -2, "normalizedOffset", _g_get_normalizedOffset);
		Utils.RegisterFunc(L, -2, "itemPrefab", _g_get_itemPrefab);
		Utils.RegisterFunc(L, -2, "itemSize", _g_get_itemSize);
		Utils.RegisterFunc(L, -2, "padding", _g_get_padding);
		Utils.RegisterFunc(L, -2, "margin", _g_get_margin);
		Utils.RegisterFunc(L, -1, "normalizedOffset", _s_set_normalizedOffset);
		Utils.RegisterFunc(L, -1, "itemPrefab", _s_set_itemPrefab);
		Utils.RegisterFunc(L, -1, "itemSize", _s_set_itemSize);
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
				DynamicVerticalScrollRect o = new DynamicVerticalScrollRect();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRect constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
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
			DynamicVerticalScrollRect obj = (DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int datas = Lua.xlua_tointeger(L, 2);
			obj.SetDatas(datas);
			return 0;
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
			DynamicVerticalScrollRect obj = (DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			DynamicVerticalScrollRect obj = (DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_FindItemByDataIdx(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect obj = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_Rebuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CanvasUpdate v);
			dynamicVerticalScrollRect.Rebuild(v);
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
			((DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateItems();
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData data = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRect.OnScroll(data);
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRect.OnInitializePotentialDrag(eventData);
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRect.OnBeginDrag(eventData);
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRect.OnEndDrag(eventData);
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dynamicVerticalScrollRect.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_contentW(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRect.contentW);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_contentH(IntPtr L)
	{
		try
		{
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRect.contentH);
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dynamicVerticalScrollRect.normalizedOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemPrefab(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dynamicVerticalScrollRect.itemPrefab);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, dynamicVerticalScrollRect.itemSize);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, dynamicVerticalScrollRect.padding);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, dynamicVerticalScrollRect.margin);
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
			((DynamicVerticalScrollRect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).normalizedOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemPrefab(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1)).itemPrefab = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			dynamicVerticalScrollRect.itemSize = val;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			dynamicVerticalScrollRect.padding = val;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			dynamicVerticalScrollRect.margin = val;
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
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject> action = objectTranslator.GetDelegate<Action<GameObject>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRect.onInstantiateItem += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRect.onInstantiateItem -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRect.onInstantiateItem!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onDisplayItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject, int> action = objectTranslator.GetDelegate<Action<GameObject, int>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject, int>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRect.onDisplayItem += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRect.onDisplayItem -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRect.onDisplayItem!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onClearItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			Action<GameObject> action = objectTranslator.GetDelegate<Action<GameObject>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.GameObject>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRect.onClearItem += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRect.onClearItem -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRect.onClearItem!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onOffsetChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			DynamicVerticalScrollRect dynamicVerticalScrollRect = (DynamicVerticalScrollRect)objectTranslator.FastGetCSObj(L, 1);
			Action<float> action = objectTranslator.GetDelegate<Action<float>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<float>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					dynamicVerticalScrollRect.onOffsetChanged += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					dynamicVerticalScrollRect.onOffsetChanged -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to DynamicVerticalScrollRect.onOffsetChanged!");
		return 0;
	}
}
