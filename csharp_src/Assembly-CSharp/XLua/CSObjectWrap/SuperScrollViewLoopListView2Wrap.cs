using System;
using System.Collections.Generic;
using SuperScrollView;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SuperScrollViewLoopListView2Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LoopListView2);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 52, 19, 12);
		Utils.RegisterFunc(L, -3, "SetItemUseCanvas", _m_SetItemUseCanvas);
		Utils.RegisterFunc(L, -3, "SetStopAdjustVelocity", _m_SetStopAdjustVelocity);
		Utils.RegisterFunc(L, -3, "SetSmoothDraggingInertia", _m_SetSmoothDraggingInertia);
		Utils.RegisterFunc(L, -3, "SetAutoLoad", _m_SetAutoLoad);
		Utils.RegisterFunc(L, -3, "SetOnBeginDragAction", _m_SetOnBeginDragAction);
		Utils.RegisterFunc(L, -3, "SetOnDragingAction", _m_SetOnDragingAction);
		Utils.RegisterFunc(L, -3, "SetOnEndDragAction", _m_SetOnEndDragAction);
		Utils.RegisterFunc(L, -3, "SetOnListClickAction", _m_SetOnListClickAction);
		Utils.RegisterFunc(L, -3, "SetOnSnapItemFinished", _m_SetOnSnapItemFinished);
		Utils.RegisterFunc(L, -3, "SetOnSnapNearestChanged", _m_SetOnSnapNearestChanged);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "GetItemPrefabConfData", _m_GetItemPrefabConfData);
		Utils.RegisterFunc(L, -3, "OnItemPrefabChanged", _m_OnItemPrefabChanged);
		Utils.RegisterFunc(L, -3, "InitListView", _m_InitListView);
		Utils.RegisterFunc(L, -3, "ResetListView", _m_ResetListView);
		Utils.RegisterFunc(L, -3, "SetListItemCount", _m_SetListItemCount);
		Utils.RegisterFunc(L, -3, "ClearItemPosCache", _m_ClearItemPosCache);
		Utils.RegisterFunc(L, -3, "InsertFront_Mod", _m_InsertFront_Mod);
		Utils.RegisterFunc(L, -3, "AppendTail_Mod", _m_AppendTail_Mod);
		Utils.RegisterFunc(L, -3, "SetListItemCount_Mod", _m_SetListItemCount_Mod);
		Utils.RegisterFunc(L, -3, "GetShownItemByItemIndex", _m_GetShownItemByItemIndex);
		Utils.RegisterFunc(L, -3, "GetShownItemByIndex", _m_GetShownItemByIndex);
		Utils.RegisterFunc(L, -3, "GetShownItemByIndexWithoutCheck", _m_GetShownItemByIndexWithoutCheck);
		Utils.RegisterFunc(L, -3, "GetIndexInShownItemList", _m_GetIndexInShownItemList);
		Utils.RegisterFunc(L, -3, "DoActionForEachShownItem", _m_DoActionForEachShownItem);
		Utils.RegisterFunc(L, -3, "NewListViewItem", _m_NewListViewItem);
		Utils.RegisterFunc(L, -3, "OnItemSizeChanged", _m_OnItemSizeChanged);
		Utils.RegisterFunc(L, -3, "RefreshItemByItemIndex", _m_RefreshItemByItemIndex);
		Utils.RegisterFunc(L, -3, "FinishSnapImmediately", _m_FinishSnapImmediately);
		Utils.RegisterFunc(L, -3, "GetCurShowIndexes", _m_GetCurShowIndexes);
		Utils.RegisterFunc(L, -3, "GetMovePanelToIndexPos", _m_GetMovePanelToIndexPos);
		Utils.RegisterFunc(L, -3, "MovePanelToItemIndex", _m_MovePanelToItemIndex);
		Utils.RegisterFunc(L, -3, "MovePanelToItemIndex_Mod", _m_MovePanelToItemIndex_Mod);
		Utils.RegisterFunc(L, -3, "MovePanelToItemIndexToBottom", _m_MovePanelToItemIndexToBottom);
		Utils.RegisterFunc(L, -3, "RefreshAllShownItem", _m_RefreshAllShownItem);
		Utils.RegisterFunc(L, -3, "RefreshAllShownItemWithFirstIndex", _m_RefreshAllShownItemWithFirstIndex);
		Utils.RegisterFunc(L, -3, "RefreshAllShownItemWithFirstIndexAndPos", _m_RefreshAllShownItemWithFirstIndexAndPos);
		Utils.RegisterFunc(L, -3, "RecycleAllItem", _m_RecycleAllItem);
		Utils.RegisterFunc(L, -3, "AddItemPoolByItemProvider", _m_AddItemPoolByItemProvider);
		Utils.RegisterFunc(L, -3, "EnableLoadingGoTail", _m_EnableLoadingGoTail);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "GetItemCornerPosInViewPort", _m_GetItemCornerPosInViewPort);
		Utils.RegisterFunc(L, -3, "ForceUpdate", _m_ForceUpdate);
		Utils.RegisterFunc(L, -3, "UpdateAllShownItemSnapData", _m_UpdateAllShownItemSnapData);
		Utils.RegisterFunc(L, -3, "ClearSnapData", _m_ClearSnapData);
		Utils.RegisterFunc(L, -3, "SetSnapTargetItemIndex", _m_SetSnapTargetItemIndex);
		Utils.RegisterFunc(L, -3, "ForceSnapUpdateCheck", _m_ForceSnapUpdateCheck);
		Utils.RegisterFunc(L, -3, "UpdateListView", _m_UpdateListView);
		Utils.RegisterFunc(L, -3, "ClearAllItems", _m_ClearAllItems);
		Utils.RegisterFunc(L, -3, "InitArabicItemPrefabData", _m_InitArabicItemPrefabData);
		Utils.RegisterFunc(L, -2, "ArrangeType", _g_get_ArrangeType);
		Utils.RegisterFunc(L, -2, "IsVertList", _g_get_IsVertList);
		Utils.RegisterFunc(L, -2, "ItemTotalCount", _g_get_ItemTotalCount);
		Utils.RegisterFunc(L, -2, "ContainerTrans", _g_get_ContainerTrans);
		Utils.RegisterFunc(L, -2, "ScrollRect", _g_get_ScrollRect);
		Utils.RegisterFunc(L, -2, "IsDraging", _g_get_IsDraging);
		Utils.RegisterFunc(L, -2, "ItemSnapEnable", _g_get_ItemSnapEnable);
		Utils.RegisterFunc(L, -2, "SupportScrollBar", _g_get_SupportScrollBar);
		Utils.RegisterFunc(L, -2, "ShownItemCount", _g_get_ShownItemCount);
		Utils.RegisterFunc(L, -2, "ViewPortSize", _g_get_ViewPortSize);
		Utils.RegisterFunc(L, -2, "ViewPortWidth", _g_get_ViewPortWidth);
		Utils.RegisterFunc(L, -2, "ViewPortHeight", _g_get_ViewPortHeight);
		Utils.RegisterFunc(L, -2, "CurSnapNearestItemIndex", _g_get_CurSnapNearestItemIndex);
		Utils.RegisterFunc(L, -2, "mOnBeginDragAction", _g_get_mOnBeginDragAction);
		Utils.RegisterFunc(L, -2, "mOnDragingAction", _g_get_mOnDragingAction);
		Utils.RegisterFunc(L, -2, "mOnEndDragAction", _g_get_mOnEndDragAction);
		Utils.RegisterFunc(L, -2, "mOnListClickAction", _g_get_mOnListClickAction);
		Utils.RegisterFunc(L, -2, "mOnSnapItemFinished", _g_get_mOnSnapItemFinished);
		Utils.RegisterFunc(L, -2, "mOnSnapNearestChanged", _g_get_mOnSnapNearestChanged);
		Utils.RegisterFunc(L, -1, "ArrangeType", _s_set_ArrangeType);
		Utils.RegisterFunc(L, -1, "OnGetItemByIndex", _s_set_OnGetItemByIndex);
		Utils.RegisterFunc(L, -1, "OnGetItemNameByIndex", _s_set_OnGetItemNameByIndex);
		Utils.RegisterFunc(L, -1, "OnRecycleItemFunc", _s_set_OnRecycleItemFunc);
		Utils.RegisterFunc(L, -1, "ItemSnapEnable", _s_set_ItemSnapEnable);
		Utils.RegisterFunc(L, -1, "SupportScrollBar", _s_set_SupportScrollBar);
		Utils.RegisterFunc(L, -1, "mOnBeginDragAction", _s_set_mOnBeginDragAction);
		Utils.RegisterFunc(L, -1, "mOnDragingAction", _s_set_mOnDragingAction);
		Utils.RegisterFunc(L, -1, "mOnEndDragAction", _s_set_mOnEndDragAction);
		Utils.RegisterFunc(L, -1, "mOnListClickAction", _s_set_mOnListClickAction);
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
				LoopListView2 o = new LoopListView2();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2 constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetItemUseCanvas(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool itemUseCanvas = Lua.lua_toboolean(L, 2);
			obj.SetItemUseCanvas(itemUseCanvas);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStopAdjustVelocity(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool stopAdjustVelocity = Lua.lua_toboolean(L, 2);
			obj.SetStopAdjustVelocity(stopAdjustVelocity);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSmoothDraggingInertia(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool smoothDraggingInertia = Lua.lua_toboolean(L, 2);
			obj.SetSmoothDraggingInertia(smoothDraggingInertia);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAutoLoad(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int remain = Lua.xlua_tointeger(L, 2);
			Action loadPrev = objectTranslator.GetDelegate<Action>(L, 3);
			Action loadLast = objectTranslator.GetDelegate<Action>(L, 4);
			loopListView.SetAutoLoad(remain, loadPrev, loadLast);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnBeginDragAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action<PointerEventData> onBeginDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
			loopListView.SetOnBeginDragAction(onBeginDragAction);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action<PointerEventData> onDragingAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
			loopListView.SetOnDragingAction(onDragingAction);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action<PointerEventData> onEndDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
			loopListView.SetOnEndDragAction(onEndDragAction);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOnListClickAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action onListClickAction = objectTranslator.GetDelegate<Action>(L, 2);
			loopListView.SetOnListClickAction(onListClickAction);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action<LoopListView2, LoopListViewItem2> onSnapItemFinished = objectTranslator.GetDelegate<Action<LoopListView2, LoopListViewItem2>>(L, 2);
			loopListView.SetOnSnapItemFinished(onSnapItemFinished);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action<LoopListView2, LoopListViewItem2> onSnapNearestChanged = objectTranslator.GetDelegate<Action<LoopListView2, LoopListViewItem2>>(L, 2);
			loopListView.SetOnSnapNearestChanged(onSnapNearestChanged);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopListView.OnPointerClick(eventData);
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
			LoopListView2 obj = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			string prefabName = Lua.lua_tostring(L, 2);
			ItemPrefabConfData itemPrefabConfData = obj.GetItemPrefabConfData(prefabName);
			objectTranslator.Push(L, itemPrefabConfData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnItemPrefabChanged(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabName = Lua.lua_tostring(L, 2);
			obj.OnItemPrefabChanged(prefabName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitListView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopListView2, int, LoopListViewItem2>>(L, 3) && objectTranslator.Assignable<LoopListViewInitParam>(L, 4) && objectTranslator.Assignable<Func<LoopListView2, int, string>>(L, 5) && objectTranslator.Assignable<Action<LoopListViewItem2, bool>>(L, 6) && objectTranslator.Assignable<Action<LoopListViewItem2>>(L, 7))
			{
				int itemTotalCount = Lua.xlua_tointeger(L, 2);
				Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex = objectTranslator.GetDelegate<Func<LoopListView2, int, LoopListViewItem2>>(L, 3);
				LoopListViewInitParam initParam = (LoopListViewInitParam)objectTranslator.GetObject(L, 4, typeof(LoopListViewInitParam));
				Func<LoopListView2, int, string> onGetItemNameByIndex = objectTranslator.GetDelegate<Func<LoopListView2, int, string>>(L, 5);
				Action<LoopListViewItem2, bool> arabicMirrorCallback = objectTranslator.GetDelegate<Action<LoopListViewItem2, bool>>(L, 6);
				Action<LoopListViewItem2> onRecycleItemFunc = objectTranslator.GetDelegate<Action<LoopListViewItem2>>(L, 7);
				loopListView.InitListView(itemTotalCount, onGetItemByIndex, initParam, onGetItemNameByIndex, arabicMirrorCallback, onRecycleItemFunc);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopListView2, int, LoopListViewItem2>>(L, 3) && objectTranslator.Assignable<LoopListViewInitParam>(L, 4) && objectTranslator.Assignable<Func<LoopListView2, int, string>>(L, 5) && objectTranslator.Assignable<Action<LoopListViewItem2, bool>>(L, 6))
			{
				int itemTotalCount2 = Lua.xlua_tointeger(L, 2);
				Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex2 = objectTranslator.GetDelegate<Func<LoopListView2, int, LoopListViewItem2>>(L, 3);
				LoopListViewInitParam initParam2 = (LoopListViewInitParam)objectTranslator.GetObject(L, 4, typeof(LoopListViewInitParam));
				Func<LoopListView2, int, string> onGetItemNameByIndex2 = objectTranslator.GetDelegate<Func<LoopListView2, int, string>>(L, 5);
				Action<LoopListViewItem2, bool> arabicMirrorCallback2 = objectTranslator.GetDelegate<Action<LoopListViewItem2, bool>>(L, 6);
				loopListView.InitListView(itemTotalCount2, onGetItemByIndex2, initParam2, onGetItemNameByIndex2, arabicMirrorCallback2);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopListView2, int, LoopListViewItem2>>(L, 3) && objectTranslator.Assignable<LoopListViewInitParam>(L, 4) && objectTranslator.Assignable<Func<LoopListView2, int, string>>(L, 5))
			{
				int itemTotalCount3 = Lua.xlua_tointeger(L, 2);
				Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex3 = objectTranslator.GetDelegate<Func<LoopListView2, int, LoopListViewItem2>>(L, 3);
				LoopListViewInitParam initParam3 = (LoopListViewInitParam)objectTranslator.GetObject(L, 4, typeof(LoopListViewInitParam));
				Func<LoopListView2, int, string> onGetItemNameByIndex3 = objectTranslator.GetDelegate<Func<LoopListView2, int, string>>(L, 5);
				loopListView.InitListView(itemTotalCount3, onGetItemByIndex3, initParam3, onGetItemNameByIndex3);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopListView2, int, LoopListViewItem2>>(L, 3) && objectTranslator.Assignable<LoopListViewInitParam>(L, 4))
			{
				int itemTotalCount4 = Lua.xlua_tointeger(L, 2);
				Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex4 = objectTranslator.GetDelegate<Func<LoopListView2, int, LoopListViewItem2>>(L, 3);
				LoopListViewInitParam initParam4 = (LoopListViewInitParam)objectTranslator.GetObject(L, 4, typeof(LoopListViewInitParam));
				loopListView.InitListView(itemTotalCount4, onGetItemByIndex4, initParam4);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Func<LoopListView2, int, LoopListViewItem2>>(L, 3))
			{
				int itemTotalCount5 = Lua.xlua_tointeger(L, 2);
				Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex5 = objectTranslator.GetDelegate<Func<LoopListView2, int, LoopListViewItem2>>(L, 3);
				loopListView.InitListView(itemTotalCount5, onGetItemByIndex5);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.InitListView!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetListView(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetListView();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetListItemCount(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				int itemCount = Lua.xlua_tointeger(L, 2);
				bool resetPos = Lua.lua_toboolean(L, 3);
				bool needMoveToIndex = Lua.lua_toboolean(L, 4);
				loopListView.SetListItemCount(itemCount, resetPos, needMoveToIndex);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int itemCount2 = Lua.xlua_tointeger(L, 2);
				bool resetPos2 = Lua.lua_toboolean(L, 3);
				loopListView.SetListItemCount(itemCount2, resetPos2);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int itemCount3 = Lua.xlua_tointeger(L, 2);
				loopListView.SetListItemCount(itemCount3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.SetListItemCount!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearItemPosCache(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearItemPosCache();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertFront_Mod(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemCount = Lua.xlua_tointeger(L, 2);
			obj.InsertFront_Mod(itemCount);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AppendTail_Mod(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemCount = Lua.xlua_tointeger(L, 2);
			obj.AppendTail_Mod(itemCount);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetListItemCount_Mod(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				int itemCount = Lua.xlua_tointeger(L, 2);
				bool resetPos = Lua.lua_toboolean(L, 3);
				bool needMoveToIndex = Lua.lua_toboolean(L, 4);
				bool refresh = Lua.lua_toboolean(L, 5);
				loopListView.SetListItemCount_Mod(itemCount, resetPos, needMoveToIndex, refresh);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				int itemCount2 = Lua.xlua_tointeger(L, 2);
				bool resetPos2 = Lua.lua_toboolean(L, 3);
				bool needMoveToIndex2 = Lua.lua_toboolean(L, 4);
				loopListView.SetListItemCount_Mod(itemCount2, resetPos2, needMoveToIndex2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int itemCount3 = Lua.xlua_tointeger(L, 2);
				bool resetPos3 = Lua.lua_toboolean(L, 3);
				loopListView.SetListItemCount_Mod(itemCount3, resetPos3);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int itemCount4 = Lua.xlua_tointeger(L, 2);
				loopListView.SetListItemCount_Mod(itemCount4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.SetListItemCount_Mod!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShownItemByItemIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 obj = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			LoopListViewItem2 shownItemByItemIndex = obj.GetShownItemByItemIndex(itemIndex);
			objectTranslator.Push(L, shownItemByItemIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShownItemByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 obj = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			LoopListViewItem2 shownItemByIndex = obj.GetShownItemByIndex(index);
			objectTranslator.Push(L, shownItemByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShownItemByIndexWithoutCheck(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 obj = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			LoopListViewItem2 shownItemByIndexWithoutCheck = obj.GetShownItemByIndexWithoutCheck(index);
			objectTranslator.Push(L, shownItemByIndexWithoutCheck);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexInShownItemList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			LoopListViewItem2 item = (LoopListViewItem2)objectTranslator.GetObject(L, 2, typeof(LoopListViewItem2));
			int indexInShownItemList = loopListView.GetIndexInShownItemList(item);
			Lua.xlua_pushinteger(L, indexInShownItemList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoActionForEachShownItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			Action<LoopListViewItem2, object> action = objectTranslator.GetDelegate<Action<LoopListViewItem2, object>>(L, 2);
			object param = objectTranslator.GetObject(L, 3, typeof(object));
			loopListView.DoActionForEachShownItem(action, param);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NewListViewItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 obj = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			string itemPrefabName = Lua.lua_tostring(L, 2);
			LoopListViewItem2 o = obj.NewListViewItem(itemPrefabName);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnItemSizeChanged(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			obj.OnItemSizeChanged(itemIndex);
			return 0;
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
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_FinishSnapImmediately(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FinishSnapImmediately();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurShowIndexes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<int> curShowIndexes = ((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).GetCurShowIndexes();
			objectTranslator.Push(L, curShowIndexes);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMovePanelToIndexPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 obj = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			float offset = (float)Lua.lua_tonumber(L, 3);
			Vector3 movePanelToIndexPos = obj.GetMovePanelToIndexPos(itemIndex, offset);
			objectTranslator.PushUnityEngineVector3(L, movePanelToIndexPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePanelToItemIndex(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				int itemIndex = Lua.xlua_tointeger(L, 2);
				float offset = (float)Lua.lua_tonumber(L, 3);
				bool reverse = Lua.lua_toboolean(L, 4);
				loopListView.MovePanelToItemIndex(itemIndex, offset, reverse);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int itemIndex2 = Lua.xlua_tointeger(L, 2);
				float offset2 = (float)Lua.lua_tonumber(L, 3);
				loopListView.MovePanelToItemIndex(itemIndex2, offset2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.MovePanelToItemIndex!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePanelToItemIndex_Mod(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				int itemIndex = Lua.xlua_tointeger(L, 2);
				float offset = (float)Lua.lua_tonumber(L, 3);
				bool reverse = Lua.lua_toboolean(L, 4);
				loopListView.MovePanelToItemIndex_Mod(itemIndex, offset, reverse);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int itemIndex2 = Lua.xlua_tointeger(L, 2);
				float offset2 = (float)Lua.lua_tonumber(L, 3);
				loopListView.MovePanelToItemIndex_Mod(itemIndex2, offset2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.MovePanelToItemIndex_Mod!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePanelToItemIndexToBottom(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemIndex = Lua.xlua_tointeger(L, 2);
			obj.MovePanelToItemIndexToBottom(itemIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshAllShownItem(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshAllShownItem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshAllShownItemWithFirstIndex(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int firstItemIndex = Lua.xlua_tointeger(L, 2);
			obj.RefreshAllShownItemWithFirstIndex(firstItemIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshAllShownItemWithFirstIndexAndPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int firstItemIndex = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			loopListView.RefreshAllShownItemWithFirstIndexAndPos(firstItemIndex, val);
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
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RecycleAllItem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddItemPoolByItemProvider(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			string prefabName = Lua.lua_tostring(L, 2);
			ItemPrefabConfData data = (ItemPrefabConfData)objectTranslator.GetObject(L, 3, typeof(ItemPrefabConfData));
			loopListView.AddItemPoolByItemProvider(prefabName, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableLoadingGoTail(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				bool enable = Lua.lua_toboolean(L, 2);
				int focusIndex = Lua.xlua_tointeger(L, 3);
				float offset = (float)Lua.lua_tonumber(L, 4);
				loopListView.EnableLoadingGoTail(enable, focusIndex, offset);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				bool enable2 = Lua.lua_toboolean(L, 2);
				int focusIndex2 = Lua.xlua_tointeger(L, 3);
				loopListView.EnableLoadingGoTail(enable2, focusIndex2);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool enable3 = Lua.lua_toboolean(L, 2);
				loopListView.EnableLoadingGoTail(enable3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.EnableLoadingGoTail!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopListView.OnBeginDrag(eventData);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopListView.OnEndDrag(eventData);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			loopListView.OnDrag(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemCornerPosInViewPort(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<LoopListViewItem2>(L, 2) && objectTranslator.Assignable<ItemCornerEnum>(L, 3))
			{
				LoopListViewItem2 item = (LoopListViewItem2)objectTranslator.GetObject(L, 2, typeof(LoopListViewItem2));
				objectTranslator.Get(L, 3, out ItemCornerEnum v);
				Vector3 itemCornerPosInViewPort = loopListView.GetItemCornerPosInViewPort(item, v);
				objectTranslator.PushUnityEngineVector3(L, itemCornerPosInViewPort);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<LoopListViewItem2>(L, 2))
			{
				LoopListViewItem2 item2 = (LoopListViewItem2)objectTranslator.GetObject(L, 2, typeof(LoopListViewItem2));
				Vector3 itemCornerPosInViewPort2 = loopListView.GetItemCornerPosInViewPort(item2);
				objectTranslator.PushUnityEngineVector3(L, itemCornerPosInViewPort2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListView2.GetItemCornerPosInViewPort!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceUpdate(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAllShownItemSnapData(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateAllShownItemSnapData();
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
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearSnapData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSnapTargetItemIndex(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int snapTargetItemIndex = Lua.xlua_tointeger(L, 2);
			obj.SetSnapTargetItemIndex(snapTargetItemIndex);
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
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceSnapUpdateCheck();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateListView(IntPtr L)
	{
		try
		{
			LoopListView2 obj = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float distanceForRecycle = (float)Lua.lua_tonumber(L, 2);
			float distanceForRecycle2 = (float)Lua.lua_tonumber(L, 3);
			float distanceForNew = (float)Lua.lua_tonumber(L, 4);
			float distanceForNew2 = (float)Lua.lua_tonumber(L, 5);
			obj.UpdateListView(distanceForRecycle, distanceForRecycle2, distanceForNew, distanceForNew2);
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
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllItems();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitArabicItemPrefabData(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitArabicItemPrefabData();
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushSuperScrollViewListItemArrangeType(L, loopListView.ArrangeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsVertList(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopListView.IsVertList);
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
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListView.ItemTotalCount);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.ContainerTrans);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.ScrollRect);
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
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopListView.IsDraging);
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
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopListView.ItemSnapEnable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SupportScrollBar(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopListView.SupportScrollBar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShownItemCount(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListView.ShownItemCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ViewPortSize(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListView.ViewPortSize);
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
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListView.ViewPortWidth);
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
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListView.ViewPortHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurSnapNearestItemIndex(IntPtr L)
	{
		try
		{
			LoopListView2 loopListView = (LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListView.CurSnapNearestItemIndex);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.mOnBeginDragAction);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.mOnDragingAction);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.mOnEndDragAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mOnListClickAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.mOnListClickAction);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.mOnSnapItemFinished);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListView.mOnSnapNearestChanged);
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
			LoopListView2 loopListView = (LoopListView2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ListItemArrangeType val);
			loopListView.ArrangeType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnGetItemByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).OnGetItemByIndex = objectTranslator.GetDelegate<Func<LoopListView2, int, LoopListViewItem2>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnGetItemNameByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).OnGetItemNameByIndex = objectTranslator.GetDelegate<Func<LoopListView2, int, string>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnRecycleItemFunc(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).OnRecycleItemFunc = objectTranslator.GetDelegate<Action<LoopListViewItem2>>(L, 2);
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
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemSnapEnable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SupportScrollBar(IntPtr L)
	{
		try
		{
			((LoopListView2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SupportScrollBar = Lua.lua_toboolean(L, 2);
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
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).mOnBeginDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
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
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).mOnDragingAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
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
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).mOnEndDragAction = objectTranslator.GetDelegate<Action<PointerEventData>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mOnListClickAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).mOnListClickAction = objectTranslator.GetDelegate<Action>(L, 2);
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
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).mOnSnapItemFinished = objectTranslator.GetDelegate<Action<LoopListView2, LoopListViewItem2>>(L, 2);
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
			((LoopListView2)objectTranslator.FastGetCSObj(L, 1)).mOnSnapNearestChanged = objectTranslator.GetDelegate<Action<LoopListView2, LoopListViewItem2>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
