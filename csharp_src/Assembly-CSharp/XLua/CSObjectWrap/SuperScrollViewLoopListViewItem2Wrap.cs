using System;
using SuperScrollView;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SuperScrollViewLoopListViewItem2Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LoopListViewItem2);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 23, 16);
		Utils.RegisterFunc(L, -3, "CancleMouseClickAnimation", _m_CancleMouseClickAnimation);
		Utils.RegisterFunc(L, -3, "SetVisible", _m_SetVisible);
		Utils.RegisterFunc(L, -2, "UserObjectData", _g_get_UserObjectData);
		Utils.RegisterFunc(L, -2, "UserIntData1", _g_get_UserIntData1);
		Utils.RegisterFunc(L, -2, "UserIntData2", _g_get_UserIntData2);
		Utils.RegisterFunc(L, -2, "UserStringData1", _g_get_UserStringData1);
		Utils.RegisterFunc(L, -2, "UserStringData2", _g_get_UserStringData2);
		Utils.RegisterFunc(L, -2, "DistanceWithViewPortSnapCenter", _g_get_DistanceWithViewPortSnapCenter);
		Utils.RegisterFunc(L, -2, "StartPosOffset", _g_get_StartPosOffset);
		Utils.RegisterFunc(L, -2, "ItemCreatedCheckFrameCount", _g_get_ItemCreatedCheckFrameCount);
		Utils.RegisterFunc(L, -2, "Padding", _g_get_Padding);
		Utils.RegisterFunc(L, -2, "CachedRectTransform", _g_get_CachedRectTransform);
		Utils.RegisterFunc(L, -2, "ItemPrefabName", _g_get_ItemPrefabName);
		Utils.RegisterFunc(L, -2, "ItemAnimatorPath", _g_get_ItemAnimatorPath);
		Utils.RegisterFunc(L, -2, "ItemCancleAnimation", _g_get_ItemCancleAnimation);
		Utils.RegisterFunc(L, -2, "ItemIndex", _g_get_ItemIndex);
		Utils.RegisterFunc(L, -2, "ItemId", _g_get_ItemId);
		Utils.RegisterFunc(L, -2, "IsInitHandlerCalled", _g_get_IsInitHandlerCalled);
		Utils.RegisterFunc(L, -2, "ParentListView", _g_get_ParentListView);
		Utils.RegisterFunc(L, -2, "TopY", _g_get_TopY);
		Utils.RegisterFunc(L, -2, "BottomY", _g_get_BottomY);
		Utils.RegisterFunc(L, -2, "LeftX", _g_get_LeftX);
		Utils.RegisterFunc(L, -2, "RightX", _g_get_RightX);
		Utils.RegisterFunc(L, -2, "ItemSize", _g_get_ItemSize);
		Utils.RegisterFunc(L, -2, "ItemSizeWithPadding", _g_get_ItemSizeWithPadding);
		Utils.RegisterFunc(L, -1, "UserObjectData", _s_set_UserObjectData);
		Utils.RegisterFunc(L, -1, "UserIntData1", _s_set_UserIntData1);
		Utils.RegisterFunc(L, -1, "UserIntData2", _s_set_UserIntData2);
		Utils.RegisterFunc(L, -1, "UserStringData1", _s_set_UserStringData1);
		Utils.RegisterFunc(L, -1, "UserStringData2", _s_set_UserStringData2);
		Utils.RegisterFunc(L, -1, "DistanceWithViewPortSnapCenter", _s_set_DistanceWithViewPortSnapCenter);
		Utils.RegisterFunc(L, -1, "StartPosOffset", _s_set_StartPosOffset);
		Utils.RegisterFunc(L, -1, "ItemCreatedCheckFrameCount", _s_set_ItemCreatedCheckFrameCount);
		Utils.RegisterFunc(L, -1, "Padding", _s_set_Padding);
		Utils.RegisterFunc(L, -1, "ItemPrefabName", _s_set_ItemPrefabName);
		Utils.RegisterFunc(L, -1, "ItemAnimatorPath", _s_set_ItemAnimatorPath);
		Utils.RegisterFunc(L, -1, "ItemCancleAnimation", _s_set_ItemCancleAnimation);
		Utils.RegisterFunc(L, -1, "ItemIndex", _s_set_ItemIndex);
		Utils.RegisterFunc(L, -1, "ItemId", _s_set_ItemId);
		Utils.RegisterFunc(L, -1, "IsInitHandlerCalled", _s_set_IsInitHandlerCalled);
		Utils.RegisterFunc(L, -1, "ParentListView", _s_set_ParentListView);
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
				LoopListViewItem2 o = new LoopListViewItem2();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListViewItem2 constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancleMouseClickAnimation(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CancleMouseClickAnimation();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisible(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				bool visible = Lua.lua_toboolean(L, 2);
				bool useGo = Lua.lua_toboolean(L, 3);
				loopListViewItem.SetVisible(visible, useGo);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool visible2 = Lua.lua_toboolean(L, 2);
				loopListViewItem.SetVisible(visible2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SuperScrollView.LoopListViewItem2.SetVisible!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserObjectData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, loopListViewItem.UserObjectData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserIntData1(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListViewItem.UserIntData1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserIntData2(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListViewItem.UserIntData2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserStringData1(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loopListViewItem.UserStringData1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserStringData2(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loopListViewItem.UserStringData2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DistanceWithViewPortSnapCenter(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.DistanceWithViewPortSnapCenter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_StartPosOffset(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.StartPosOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemCreatedCheckFrameCount(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListViewItem.ItemCreatedCheckFrameCount);
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
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.Padding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CachedRectTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListViewItem.CachedRectTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemPrefabName(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loopListViewItem.ItemPrefabName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemAnimatorPath(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loopListViewItem.ItemAnimatorPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemCancleAnimation(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopListViewItem.ItemCancleAnimation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemIndex(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListViewItem.ItemIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ItemId(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loopListViewItem.ItemId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInitHandlerCalled(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loopListViewItem.IsInitHandlerCalled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ParentListView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, loopListViewItem.ParentListView);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TopY(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.TopY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BottomY(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.BottomY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LeftX(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.LeftX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RightX(IntPtr L)
	{
		try
		{
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.RightX);
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
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.ItemSize);
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
			LoopListViewItem2 loopListViewItem = (LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, loopListViewItem.ItemSizeWithPadding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserObjectData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopListViewItem2)objectTranslator.FastGetCSObj(L, 1)).UserObjectData = objectTranslator.GetObject(L, 2, typeof(object));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserIntData1(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UserIntData1 = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserIntData2(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UserIntData2 = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserStringData1(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UserStringData1 = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserStringData2(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UserStringData2 = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DistanceWithViewPortSnapCenter(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DistanceWithViewPortSnapCenter = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_StartPosOffset(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartPosOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemCreatedCheckFrameCount(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemCreatedCheckFrameCount = Lua.xlua_tointeger(L, 2);
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
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Padding = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemPrefabName(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemPrefabName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemAnimatorPath(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemAnimatorPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemCancleAnimation(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemCancleAnimation = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemIndex(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ItemId(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ItemId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsInitHandlerCalled(IntPtr L)
	{
		try
		{
			((LoopListViewItem2)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInitHandlerCalled = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ParentListView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LoopListViewItem2)objectTranslator.FastGetCSObj(L, 1)).ParentListView = (LoopListView2)objectTranslator.GetObject(L, 2, typeof(LoopListView2));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
