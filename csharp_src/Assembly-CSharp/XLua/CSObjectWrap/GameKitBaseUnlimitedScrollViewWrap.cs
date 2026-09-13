using System;
using GameKit.Base;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameKitBaseUnlimitedScrollViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnlimitedScrollView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 11, 9);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "GetItemWrapCount", _m_GetItemWrapCount);
		Utils.RegisterFunc(L, -3, "GetItem", _m_GetItem);
		Utils.RegisterFunc(L, -3, "GetItemWrap", _m_GetItemWrap);
		Utils.RegisterFunc(L, -3, "AddItemWrap", _m_AddItemWrap);
		Utils.RegisterFunc(L, -3, "InsertItemWrap", _m_InsertItemWrap);
		Utils.RegisterFunc(L, -3, "RemoveItemWrap", _m_RemoveItemWrap);
		Utils.RegisterFunc(L, -3, "AddItemToTail", _m_AddItemToTail);
		Utils.RegisterFunc(L, -2, "HeadIndex", _g_get_HeadIndex);
		Utils.RegisterFunc(L, -2, "TailIndex", _g_get_TailIndex);
		Utils.RegisterFunc(L, -2, "ViewportSize", _g_get_ViewportSize);
		Utils.RegisterFunc(L, -2, "ContentSize", _g_get_ContentSize);
		Utils.RegisterFunc(L, -2, "ContentPosition", _g_get_ContentPosition);
		Utils.RegisterFunc(L, -2, "Velocity", _g_get_Velocity);
		Utils.RegisterFunc(L, -2, "DragOnHeadOrTailOfTheScrollView", _g_get_DragOnHeadOrTailOfTheScrollView);
		Utils.RegisterFunc(L, -2, "OnPointerDownScrollView", _g_get_OnPointerDownScrollView);
		Utils.RegisterFunc(L, -2, "OnBeginDrag", _g_get_OnBeginDrag);
		Utils.RegisterFunc(L, -2, "OnItemMoveIn", _g_get_OnItemMoveIn);
		Utils.RegisterFunc(L, -2, "OnItemMoveOut", _g_get_OnItemMoveOut);
		Utils.RegisterFunc(L, -1, "ToHead", _s_set_ToHead);
		Utils.RegisterFunc(L, -1, "ToTail", _s_set_ToTail);
		Utils.RegisterFunc(L, -1, "ContentPosition", _s_set_ContentPosition);
		Utils.RegisterFunc(L, -1, "Velocity", _s_set_Velocity);
		Utils.RegisterFunc(L, -1, "DragOnHeadOrTailOfTheScrollView", _s_set_DragOnHeadOrTailOfTheScrollView);
		Utils.RegisterFunc(L, -1, "OnPointerDownScrollView", _s_set_OnPointerDownScrollView);
		Utils.RegisterFunc(L, -1, "OnBeginDrag", _s_set_OnBeginDrag);
		Utils.RegisterFunc(L, -1, "OnItemMoveIn", _s_set_OnItemMoveIn);
		Utils.RegisterFunc(L, -1, "OnItemMoveOut", _s_set_OnItemMoveOut);
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
				UnlimitedScrollView o = new UnlimitedScrollView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameKit.Base.UnlimitedScrollView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((UnlimitedScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemWrapCount(IntPtr L)
	{
		try
		{
			int itemWrapCount = ((UnlimitedScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetItemWrapCount();
			Lua.xlua_pushinteger(L, itemWrapCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			ItemWrap wrap = (ItemWrap)objectTranslator.GetObject(L, 2, typeof(ItemWrap));
			GameObject item = unlimitedScrollView.GetItem(wrap);
			objectTranslator.Push(L, item);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetItemWrap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index = Lua.xlua_tointeger(L, 2);
				ItemWrap itemWrap = unlimitedScrollView.GetItemWrap(index);
				objectTranslator.Push(L, itemWrap);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object userdata = objectTranslator.GetObject(L, 2, typeof(object));
				ItemWrap itemWrap2 = unlimitedScrollView.GetItemWrap(userdata);
				objectTranslator.Push(L, itemWrap2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameKit.Base.UnlimitedScrollView.GetItemWrap!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddItemWrap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			GameObject prefab = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			object userdata = objectTranslator.GetObject(L, 3, typeof(object));
			unlimitedScrollView.AddItemWrap(prefab, userdata);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertItemWrap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			GameObject prefab = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			object userdata = objectTranslator.GetObject(L, 4, typeof(object));
			unlimitedScrollView.InsertItemWrap(index, prefab, userdata);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveItemWrap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index = Lua.xlua_tointeger(L, 2);
				unlimitedScrollView.RemoveItemWrap(index);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object userdata = objectTranslator.GetObject(L, 2, typeof(object));
				unlimitedScrollView.RemoveItemWrap(userdata);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameKit.Base.UnlimitedScrollView.RemoveItemWrap!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddItemToTail(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			GameObject prefab = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			object userdata = objectTranslator.GetObject(L, 3, typeof(object));
			unlimitedScrollView.AddItemToTail(prefab, userdata);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeadIndex(IntPtr L)
	{
		try
		{
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, unlimitedScrollView.HeadIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TailIndex(IntPtr L)
	{
		try
		{
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, unlimitedScrollView.TailIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ViewportSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, unlimitedScrollView.ViewportSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ContentSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, unlimitedScrollView.ContentSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ContentPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, unlimitedScrollView.ContentPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, unlimitedScrollView.Velocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DragOnHeadOrTailOfTheScrollView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unlimitedScrollView.DragOnHeadOrTailOfTheScrollView);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnPointerDownScrollView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unlimitedScrollView.OnPointerDownScrollView);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unlimitedScrollView.OnBeginDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnItemMoveIn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unlimitedScrollView.OnItemMoveIn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnItemMoveOut(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unlimitedScrollView.OnItemMoveOut);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ToHead(IntPtr L)
	{
		try
		{
			((UnlimitedScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToHead = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ToTail(IntPtr L)
	{
		try
		{
			((UnlimitedScrollView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToTail = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ContentPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			unlimitedScrollView.ContentPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnlimitedScrollView unlimitedScrollView = (UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			unlimitedScrollView.Velocity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DragOnHeadOrTailOfTheScrollView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1)).DragOnHeadOrTailOfTheScrollView = objectTranslator.GetDelegate<UnlimitedScrollView.DragOnHeadOrTailOfTheScrollViewDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnPointerDownScrollView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1)).OnPointerDownScrollView = objectTranslator.GetDelegate<UnlimitedScrollView.OnPointerDownScrollViewDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1)).OnBeginDrag = objectTranslator.GetDelegate<UnlimitedScrollView.BeginDragDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnItemMoveIn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1)).OnItemMoveIn = objectTranslator.GetDelegate<UnlimitedScrollView.ItemMoveInDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnItemMoveOut(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnlimitedScrollView)objectTranslator.FastGetCSObj(L, 1)).OnItemMoveOut = objectTranslator.GetDelegate<UnlimitedScrollView.ItemMoveOutDelegate>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
