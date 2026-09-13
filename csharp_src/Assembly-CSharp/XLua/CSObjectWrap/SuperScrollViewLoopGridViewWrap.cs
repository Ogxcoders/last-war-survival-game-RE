using System;
using SuperScrollView;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SuperScrollViewLoopGridViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LoopGridView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 42, 19, 11);
		Utils.RegisterFunc(L, -3, "SetOnBeginDragAction", _m_SetOnBeginDragAction);
		Utils.RegisterFunc(L, -3, "SetOnDragingAction", _m_SetOnDragingAction);
		Utils.RegisterFunc(L, -3, "SetOnEndDragAction", _m_SetOnEndDragAction);
		Utils.RegisterFunc(L, -3, "SetOnSnapItemFinished", _m_SetOnSnapItemFinished);
		Utils.RegisterFunc(L, -3, "SetOnSnapNearestChanged", _m_SetOnSnapNearestChanged);
		Utils.RegisterFunc(L, -3, "GetItemPrefabConfData", _m_GetItemPrefabConfData);
		Utils.RegisterFunc(L, -3, "InitGridView", _m_InitGridView);
		Utils.RegisterFunc(L, -3, "SetListItemCount", _m_SetListItemCount);
		Utils.RegisterFunc(L, -3, "NewListViewItem", _m_NewListViewItem);
		Utils.RegisterFunc(L, -3, "RefreshItemByItemIndex", _m_RefreshItemByItemIndex);
		Utils.RegisterFunc(L, -3, "RefreshItemByRowColumn", _m_RefreshItemByRowColumn);
		Utils.RegisterFunc(L, -3, "ClearSnapData", _m_ClearSnapData);
		Utils.RegisterFunc(L, -3, "SetSnapTargetItemRowColumn", _m_SetSnapTargetItemRowColumn);
		Utils.RegisterFunc(L, -3, "ForceSnapUpdateCheck", _m_ForceSnapUpdateCheck);
		Utils.RegisterFunc(L, -3, "ForceToCheckContentPos", _m_ForceToCheckContentPos);
		Utils.RegisterFunc(L, -3, "MovePanelToItemByIndex", _m_MovePanelToItemByIndex);
		Utils.RegisterFunc(L, -3, "MovePanelToItemByRowColumn", _m_MovePanelToItemByRowColumn);
		Utils.RegisterFunc(L, -3, "RefreshAllShownItem", _m_RefreshAllShownItem);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "GetItemIndexByRowColumn", _m_GetItemIndexByRowColumn);
		Utils.RegisterFunc(L, -3, "GetRowColumnByItemIndex", _m_GetRowColumnByItemIndex);
		Utils.RegisterFunc(L, -3, "GetItemAbsPos", _m_GetItemAbsPos);
		Utils.RegisterFunc(L, -3, "GetItemPos", _m_GetItemPos);
		Utils.RegisterFunc(L, -3, "GetShownItemByItemIndex", _m_GetShownItemByItemIndex);
		Utils.RegisterFunc(L, -3, "GetShownItemByRowColumn", _m_GetShownItemByRowColumn);
		Utils.RegisterFunc(L, -3, "UpdateAllGridSetting", _m_UpdateAllGridSetting);
		Utils.RegisterFunc(L, -3, "SetGridFixedGroupCount", _m_SetGridFixedGroupCount);
		Utils.RegisterFunc(L, -3, "SetItemSize", _m_SetItemSize);
		Utils.RegisterFunc(L, -3, "SetItemPadding", _m_SetItemPadding);
		Utils.RegisterFunc(L, -3, "SetPadding", _m_SetPadding);
		Utils.RegisterFunc(L, -3, "UpdateContentSize", _m_UpdateContentSize);
		Utils.RegisterFunc(L, -3, "VaildAndSetContainerPos", _m_VaildAndSetContainerPos);
		Utils.RegisterFunc(L, -3, "ClearAllTmpRecycledItem", _m_ClearAllTmpRecycledItem);
		Utils.RegisterFunc(L, -3, "RecycleAllItem", _m_RecycleAllItem);
		Utils.RegisterFunc(L, -3, "ClearAllItems", _m_ClearAllItems);
		Utils.RegisterFunc(L, -3, "UpdateGridViewContent", _m_UpdateGridViewContent);
		Utils.RegisterFunc(L, -3, "UpdateStartEndPadding", _m_UpdateStartEndPadding);
		Utils.RegisterFunc(L, -3, "UpdateItemSize", _m_UpdateItemSize);
		Utils.RegisterFunc(L, -3, "UpdateColumnRowCount", _m_UpdateColumnRowCount);
		Utils.RegisterFunc(L, -3, "FinishSnapImmediately", _m_FinishSnapImmediately);
		Utils.RegisterFunc(L, -2, "ArrangeType", _g_get_ArrangeType);
		Utils.RegisterFunc(L, -2, "ItemPrefabDataList", _g_get_ItemPrefabDataList);
		Utils.RegisterFunc(L, -2, "ItemTotalCount", _g_get_ItemTotalCount);
		Utils.RegisterFunc(L, -2, "ContainerTrans", _g_get_ContainerTrans);
		Utils.RegisterFunc(L, -2, "ViewPortWidth", _g_get_ViewPortWidth);
		Utils.RegisterFunc(L, -2, "ViewPortHeight", _g_get_ViewPortHeight);
		Utils.RegisterFunc(L, -2, "ScrollRect", _g_get_ScrollRect);
		Utils.RegisterFunc(L, -2, "IsDraging", _g_get_IsDraging);
		Utils.RegisterFunc(L, -2, "ItemSnapEnable", _g_get_ItemSnapEnable);
		Utils.RegisterFunc(L, -2, "ItemSize", _g_get_ItemSize);
		Utils.RegisterFunc(L, -2, "ItemPadding", _g_get_ItemPadding);
		Utils.RegisterFunc(L, -2, "ItemSizeWithPadding", _g_get_ItemSizeWithPadding);
		Utils.RegisterFunc(L, -2, "Padding", _g_get_Padding);
		Utils.RegisterFunc(L, -2, "CurSnapNearestItemRowColumn", _g_get_CurSnapNearestItemRowColumn);
		Utils.RegisterFunc(L, -2, "mOnBeginDragAction", _g_get_mOnBeginDragAction);
		Utils.RegisterFunc(L, -2, "mOnDragingAction", _g_get_mOnDragingAction);
		Utils.RegisterFunc(L, -2, "mOnEndDragAction", _g_get_mOnEndDragAction);
		Utils.RegisterFunc(L, -2, "mOnSnapItemFinished", _g_get_mOnSnapItemFinished);
		Utils.RegisterFunc(L, -2, "mOnSnapNearestChanged", _g_get_mOnSnapNearestChanged);
		Utils.RegisterFunc(L, -1, "ArrangeType", _s_set_ArrangeType);
		Utils.RegisterFunc(L, -1, "OnGetItemByRowColumn", _s_set_OnGetItemByRowColumn);
		Utils.RegisterFunc(L, -1, "ItemSnapEnable", _s_set_ItemSnapEnable);
		Utils.RegisterFunc(L, -1, "ItemSize", _s_set_ItemSize);
		Utils.RegisterFunc(L, -1, "ItemPadding", _s_set_ItemPadding);
		Utils.RegisterFunc(L, -1, "Padding", _s_set_Padding);
		Utils.RegisterFunc(L, -1, "mOnBeginDragAction", _s_set_mOnBeginDragAction);
		Utils.RegisterFunc(L, -1, "mOnDragingAction", _s_set_mOnDragingAction);
		Utils.RegisterFunc(L, -1, "mOnEndDragAction", _s_set_mOnEndDragAction);
		Utils.RegisterFunc(L, -1, "mOnSnapItemFinished", _s_set_mOnSnapItemFinished);
		Utils.RegisterFunc(L, -1, "mOnSnapNearestChanged", _s_set_mOnSnapNearestChanged);
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
				LoopGridView o = new LoopGridView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopGridView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnBeginDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			Action<PointerEventData> onBeginDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
			loopGridView.SetOnBeginDragAction(onBeginDragAction);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnDragingAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			Action<PointerEventData> onDragingAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
			loopGridView.SetOnDragingAction(onDragingAction);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnEndDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			Action<PointerEventData> onEndDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
			loopGridView.SetOnEndDragAction(onEndDragAction);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnSnapItemFinished(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			Action<LoopGridView, LoopGridViewItem> onSnapItemFinished = objectTranslator.GetDelegate<Action<LoopGridView, LoopGridViewItem>>(L, 2);
			loopGridView.SetOnSnapItemFinished(onSnapItemFinished);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnSnapNearestChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			Action<LoopGridView> onSnapNearestChanged = objectTranslator.GetDelegate<Action<LoopGridView>>(L, 2);
			loopGridView.SetOnSnapNearestChanged(onSnapNearestChanged);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemPrefabConfData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			string prefabName = Lua.lua_tostring(L, 2);
			GridViewItemPrefabConfData itemPrefabConfData = obj.GetItemPrefabConfData(prefabName);
			objectTranslator.Push(L, itemPrefabConfData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitGridView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 3) && objectTranslator.Assignable<LoopGridViewSettingParam>(L, 4) && objectTranslator.Assignable<LoopGridViewInitParam>(L, 5))
			{
				int itemTotalCount = Lua.xlua_tointeger(L, 2);
				Func<LoopGridView, int, int, int, LoopGridViewItem> onGetItemByRowColumn = objectTranslator.GetDelegate<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 3);
				LoopGridViewSettingParam settingParam = (LoopGridViewSettingParam)objectTranslator.GetObject(L, 4, typeof(LoopGridViewSettingParam));
				LoopGridViewInitParam initParam = (LoopGridViewInitParam)objectTranslator.GetObject(L, 5, typeof(LoopGridViewInitParam));
				loopGridView.InitGridView(itemTotalCount, onGetItemByRowColumn, settingParam, initParam);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 3) && objectTranslator.Assignable<LoopGridViewSettingParam>(L, 4))
			{
				int itemTotalCount2 = Lua.xlua_tointeger(L, 2);
				Func<LoopGridView, int, int, int, LoopGridViewItem> onGetItemByRowColumn2 = objectTranslator.GetDelegate<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 3);
				LoopGridViewSettingParam settingParam2 = (LoopGridViewSettingParam)objectTranslator.GetObject(L, 4, typeof(LoopGridViewSettingParam));
				loopGridView.InitGridView(itemTotalCount2, onGetItemByRowColumn2, settingParam2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 3))
			{
				int itemTotalCount3 = Lua.xlua_tointeger(L, 2);
				Func<LoopGridView, int, int, int, LoopGridViewItem> onGetItemByRowColumn3 = objectTranslator.GetDelegate<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 3);
				loopGridView.InitGridView(itemTotalCount3, onGetItemByRowColumn3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopGridView.InitGridView!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetListItemCount(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int itemCount = Lua.xlua_tointeger(L, 2);
				bool resetPos = Lua.lua_toboolean(L, 3);
				loopGridView.SetListItemCount(itemCount, resetPos);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int itemCount2 = Lua.xlua_tointeger(L, 2);
				loopGridView.SetListItemCount(itemCount2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopGridView.SetListItemCount!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NewListViewItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			string itemPrefabName = Lua.lua_tostring(L, 2);
			LoopGridViewItem o = obj.NewListViewItem(itemPrefabName);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshItemByItemIndex(IntPtr L)
	{
		try
		{
			LoopGridView obj = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			obj.RefreshItemByItemIndex(itemIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshItemByRowColumn(IntPtr L)
	{
		try
		{
			LoopGridView obj = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int row = Lua.xlua_tointeger(L, 2);
			int column = Lua.xlua_tointeger(L, 3);
			obj.RefreshItemByRowColumn(row, column);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearSnapData(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearSnapData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSnapTargetItemRowColumn(IntPtr L)
	{
		try
		{
			LoopGridView obj = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int row = Lua.xlua_tointeger(L, 2);
			int column = Lua.xlua_tointeger(L, 3);
			obj.SetSnapTargetItemRowColumn(row, column);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceSnapUpdateCheck(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceSnapUpdateCheck();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceToCheckContentPos(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceToCheckContentPos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePanelToItemByIndex(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int itemIndex = Lua.xlua_tointeger(L, 2);
				float offsetX = (float)Lua.lua_tonumber(L, 3);
				float offsetY = (float)Lua.lua_tonumber(L, 4);
				loopGridView.MovePanelToItemByIndex(itemIndex, offsetX, offsetY);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int itemIndex2 = Lua.xlua_tointeger(L, 2);
				float offsetX2 = (float)Lua.lua_tonumber(L, 3);
				loopGridView.MovePanelToItemByIndex(itemIndex2, offsetX2);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int itemIndex3 = Lua.xlua_tointeger(L, 2);
				loopGridView.MovePanelToItemByIndex(itemIndex3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopGridView.MovePanelToItemByIndex!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePanelToItemByRowColumn(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int row = Lua.xlua_tointeger(L, 2);
				int column = Lua.xlua_tointeger(L, 3);
				float offsetX = (float)Lua.lua_tonumber(L, 4);
				float offsetY = (float)Lua.lua_tonumber(L, 5);
				loopGridView.MovePanelToItemByRowColumn(row, column, offsetX, offsetY);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int row2 = Lua.xlua_tointeger(L, 2);
				int column2 = Lua.xlua_tointeger(L, 3);
				float offsetX2 = (float)Lua.lua_tonumber(L, 4);
				loopGridView.MovePanelToItemByRowColumn(row2, column2, offsetX2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int row3 = Lua.xlua_tointeger(L, 2);
				int column3 = Lua.xlua_tointeger(L, 3);
				loopGridView.MovePanelToItemByRowColumn(row3, column3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopGridView.MovePanelToItemByRowColumn!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshAllShownItem(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshAllShownItem();
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
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopGridView.OnBeginDrag(eventData);
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
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopGridView.OnEndDrag(eventData);
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
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopGridView.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemIndexByRowColumn(IntPtr L)
	{
		try
		{
			LoopGridView obj = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int row = Lua.xlua_tointeger(L, 2);
			int column = Lua.xlua_tointeger(L, 3);
			int itemIndexByRowColumn = obj.GetItemIndexByRowColumn(row, column);
			Lua.xlua_pushinteger(L, itemIndexByRowColumn);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRowColumnByItemIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			RowColumnPair rowColumnByItemIndex = obj.GetRowColumnByItemIndex(itemIndex);
			objectTranslator.Push(L, rowColumnByItemIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemAbsPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			int row = Lua.xlua_tointeger(L, 2);
			int column = Lua.xlua_tointeger(L, 3);
			Vector2 itemAbsPos = obj.GetItemAbsPos(row, column);
			objectTranslator.PushUnityEngineVector2(L, itemAbsPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			int row = Lua.xlua_tointeger(L, 2);
			int column = Lua.xlua_tointeger(L, 3);
			Vector2 itemPos = obj.GetItemPos(row, column);
			objectTranslator.PushUnityEngineVector2(L, itemPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShownItemByItemIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			LoopGridViewItem shownItemByItemIndex = obj.GetShownItemByItemIndex(itemIndex);
			objectTranslator.Push(L, shownItemByItemIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShownItemByRowColumn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView obj = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			int row = Lua.xlua_tointeger(L, 2);
			int column = Lua.xlua_tointeger(L, 3);
			LoopGridViewItem shownItemByRowColumn = obj.GetShownItemByRowColumn(row, column);
			objectTranslator.Push(L, shownItemByRowColumn);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAllGridSetting(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateAllGridSetting();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGridFixedGroupCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GridFixedType v);
			int count = Lua.xlua_tointeger(L, 3);
			loopGridView.SetGridFixedGroupCount(v, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetItemSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			loopGridView.SetItemSize(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetItemPadding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			loopGridView.SetItemPadding(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPadding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			RectOffset padding = (RectOffset)objectTranslator.GetObject(L, 2, typeof(RectOffset));
			loopGridView.SetPadding(padding);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateContentSize(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateContentSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_VaildAndSetContainerPos(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).VaildAndSetContainerPos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllTmpRecycledItem(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllTmpRecycledItem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecycleAllItem(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecycleAllItem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllItems(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllItems();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGridViewContent(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateGridViewContent();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateStartEndPadding(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateStartEndPadding();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateItemSize(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateItemSize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateColumnRowCount(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateColumnRowCount();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FinishSnapImmediately(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FinishSnapImmediately();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ArrangeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.ArrangeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemPrefabDataList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.ItemPrefabDataList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemTotalCount(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopGridView.ItemTotalCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ContainerTrans(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.ContainerTrans);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ViewPortWidth(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopGridView.ViewPortWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ViewPortHeight(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopGridView.ViewPortHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ScrollRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.ScrollRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsDraging(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopGridView.IsDraging);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemSnapEnable(IntPtr L)
	{
		try
		{
			LoopGridView loopGridView = (LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopGridView.ItemSnapEnable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, loopGridView.ItemSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemPadding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, loopGridView.ItemPadding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemSizeWithPadding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, loopGridView.ItemSizeWithPadding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Padding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.Padding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurSnapNearestItemRowColumn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.CurSnapNearestItemRowColumn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mOnBeginDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.mOnBeginDragAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mOnDragingAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.mOnDragingAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mOnEndDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.mOnEndDragAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mOnSnapItemFinished(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.mOnSnapItemFinished);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mOnSnapNearestChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopGridView.mOnSnapNearestChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ArrangeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GridItemArrangeType v);
			loopGridView.ArrangeType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnGetItemByRowColumn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).OnGetItemByRowColumn = objectTranslator.GetDelegate<Func<LoopGridView, int, int, int, LoopGridViewItem>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemSnapEnable(IntPtr L)
	{
		try
		{
			((LoopGridView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemSnapEnable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			loopGridView.ItemSize = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemPadding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopGridView loopGridView = (LoopGridView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			loopGridView.ItemPadding = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Padding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).Padding = (RectOffset)objectTranslator.GetObject(L, 2, typeof(RectOffset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mOnBeginDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).mOnBeginDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mOnDragingAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).mOnDragingAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mOnEndDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).mOnEndDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mOnSnapItemFinished(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).mOnSnapItemFinished = objectTranslator.GetDelegate<Action<LoopGridView, LoopGridViewItem>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mOnSnapNearestChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopGridView)objectTranslator.FastGetCSObj(L, 1)).mOnSnapNearestChanged = objectTranslator.GetDelegate<Action<LoopGridView>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
