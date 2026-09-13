using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIGuideMaskCtrlWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIGuideMaskCtrl);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 5, 4);
		Utils.RegisterFunc(L, -3, "SetMaterialProperties", _m_SetMaterialProperties);
		Utils.RegisterFunc(L, -3, "Guide", _m_Guide);
		Utils.RegisterFunc(L, -3, "IsRaycastLocationValid", _m_IsRaycastLocationValid);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnClickBlocked", _e_OnClickBlocked);
		Utils.RegisterFunc(L, -2, "Target", _g_get_Target);
		Utils.RegisterFunc(L, -2, "FollowTargetMode", _g_get_FollowTargetMode);
		Utils.RegisterFunc(L, -2, "FollowModeCanvas", _g_get_FollowModeCanvas);
		Utils.RegisterFunc(L, -2, "interactOffset", _g_get_interactOffset);
		Utils.RegisterFunc(L, -2, "visualOffset", _g_get_visualOffset);
		Utils.RegisterFunc(L, -1, "FollowTargetMode", _s_set_FollowTargetMode);
		Utils.RegisterFunc(L, -1, "FollowModeCanvas", _s_set_FollowModeCanvas);
		Utils.RegisterFunc(L, -1, "interactOffset", _s_set_interactOffset);
		Utils.RegisterFunc(L, -1, "visualOffset", _s_set_visualOffset);
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
				UIGuideMaskCtrl o = new UIGuideMaskCtrl();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIGuideMaskCtrl constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaterialProperties(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			float rcr = (float)Lua.lua_tonumber(L, 3);
			float fade = (float)Lua.lua_tonumber(L, 4);
			bool inverse = Lua.lua_toboolean(L, 5);
			uIGuideMaskCtrl.SetMaterialProperties(val, rcr, fade, inverse);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Guide(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Canvas>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Ease>(L, 5))
			{
				Canvas canvas = (Canvas)objectTranslator.GetObject(L, 2, typeof(Canvas));
				RectTransform target = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				float duration = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out Ease val);
				Vector2[] o = uIGuideMaskCtrl.Guide(canvas, target, duration, val);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Canvas>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Canvas canvas2 = (Canvas)objectTranslator.GetObject(L, 2, typeof(Canvas));
				RectTransform target2 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				Vector2[] o2 = uIGuideMaskCtrl.Guide(canvas2, target2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Canvas>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3))
			{
				Canvas canvas3 = (Canvas)objectTranslator.GetObject(L, 2, typeof(Canvas));
				RectTransform target3 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				Vector2[] o3 = uIGuideMaskCtrl.Guide(canvas3, target3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIGuideMaskCtrl.Guide!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRaycastLocationValid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Camera eventCamera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = uIGuideMaskCtrl.IsRaycastLocationValid(val, eventCamera);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			uIGuideMaskCtrl.OnPointerClick(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGuideMaskCtrl.Target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FollowTargetMode(IntPtr L)
	{
		try
		{
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIGuideMaskCtrl.FollowTargetMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FollowModeCanvas(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGuideMaskCtrl.FollowModeCanvas);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interactOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, uIGuideMaskCtrl.interactOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_visualOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, uIGuideMaskCtrl.visualOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FollowTargetMode(IntPtr L)
	{
		try
		{
			((UIGuideMaskCtrl)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FollowTargetMode = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FollowModeCanvas(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1)).FollowModeCanvas = (Canvas)objectTranslator.GetObject(L, 2, typeof(Canvas));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_interactOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			uIGuideMaskCtrl.interactOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_visualOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			uIGuideMaskCtrl.visualOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnClickBlocked(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			UIGuideMaskCtrl uIGuideMaskCtrl = (UIGuideMaskCtrl)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					uIGuideMaskCtrl.OnClickBlocked += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					uIGuideMaskCtrl.OnClickBlocked -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UIGuideMaskCtrl.OnClickBlocked!");
		return 0;
	}
}
