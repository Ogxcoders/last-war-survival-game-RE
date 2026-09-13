using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCanvasGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CanvasGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 4, 4);
		Utils.RegisterFunc(L, -3, "IsRaycastLocationValid", _m_IsRaycastLocationValid);
		Utils.RegisterFunc(L, -3, "DOFade", _m_DOFade);
		Utils.RegisterFunc(L, -2, "alpha", _g_get_alpha);
		Utils.RegisterFunc(L, -2, "interactable", _g_get_interactable);
		Utils.RegisterFunc(L, -2, "blocksRaycasts", _g_get_blocksRaycasts);
		Utils.RegisterFunc(L, -2, "ignoreParentGroups", _g_get_ignoreParentGroups);
		Utils.RegisterFunc(L, -1, "alpha", _s_set_alpha);
		Utils.RegisterFunc(L, -1, "interactable", _s_set_interactable);
		Utils.RegisterFunc(L, -1, "blocksRaycasts", _s_set_blocksRaycasts);
		Utils.RegisterFunc(L, -1, "ignoreParentGroups", _s_set_ignoreParentGroups);
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
				CanvasGroup o = new CanvasGroup();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.CanvasGroup constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRaycastLocationValid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CanvasGroup canvasGroup = (CanvasGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Camera eventCamera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = canvasGroup.IsRaycastLocationValid(val, eventCamera);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFade(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CanvasGroup target = (CanvasGroup)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOFade(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alpha(IntPtr L)
	{
		try
		{
			CanvasGroup canvasGroup = (CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, canvasGroup.alpha);
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
			CanvasGroup canvasGroup = (CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvasGroup.interactable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blocksRaycasts(IntPtr L)
	{
		try
		{
			CanvasGroup canvasGroup = (CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvasGroup.blocksRaycasts);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ignoreParentGroups(IntPtr L)
	{
		try
		{
			CanvasGroup canvasGroup = (CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, canvasGroup.ignoreParentGroups);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alpha(IntPtr L)
	{
		try
		{
			((CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alpha = (float)Lua.lua_tonumber(L, 2);
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
			((CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).interactable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_blocksRaycasts(IntPtr L)
	{
		try
		{
			((CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).blocksRaycasts = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ignoreParentGroups(IntPtr L)
	{
		try
		{
			((CanvasGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ignoreParentGroups = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
