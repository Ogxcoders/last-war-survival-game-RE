using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineAnimationClipWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AnimationClip);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 12, 5);
		Utils.RegisterFunc(L, -3, "SampleAnimation", _m_SampleAnimation);
		Utils.RegisterFunc(L, -3, "SetCurve", _m_SetCurve);
		Utils.RegisterFunc(L, -3, "EnsureQuaternionContinuity", _m_EnsureQuaternionContinuity);
		Utils.RegisterFunc(L, -3, "ClearCurves", _m_ClearCurves);
		Utils.RegisterFunc(L, -3, "AddEvent", _m_AddEvent);
		Utils.RegisterFunc(L, -2, "length", _g_get_length);
		Utils.RegisterFunc(L, -2, "frameRate", _g_get_frameRate);
		Utils.RegisterFunc(L, -2, "wrapMode", _g_get_wrapMode);
		Utils.RegisterFunc(L, -2, "localBounds", _g_get_localBounds);
		Utils.RegisterFunc(L, -2, "legacy", _g_get_legacy);
		Utils.RegisterFunc(L, -2, "humanMotion", _g_get_humanMotion);
		Utils.RegisterFunc(L, -2, "empty", _g_get_empty);
		Utils.RegisterFunc(L, -2, "hasGenericRootTransform", _g_get_hasGenericRootTransform);
		Utils.RegisterFunc(L, -2, "hasMotionFloatCurves", _g_get_hasMotionFloatCurves);
		Utils.RegisterFunc(L, -2, "hasMotionCurves", _g_get_hasMotionCurves);
		Utils.RegisterFunc(L, -2, "hasRootCurves", _g_get_hasRootCurves);
		Utils.RegisterFunc(L, -2, "events", _g_get_events);
		Utils.RegisterFunc(L, -1, "frameRate", _s_set_frameRate);
		Utils.RegisterFunc(L, -1, "wrapMode", _s_set_wrapMode);
		Utils.RegisterFunc(L, -1, "localBounds", _s_set_localBounds);
		Utils.RegisterFunc(L, -1, "legacy", _s_set_legacy);
		Utils.RegisterFunc(L, -1, "events", _s_set_events);
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
				AnimationClip o = new AnimationClip();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.AnimationClip constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SampleAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			GameObject go = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			float time = (float)Lua.lua_tonumber(L, 3);
			animationClip.SampleAnimation(go, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			string relativePath = Lua.lua_tostring(L, 2);
			Type type = (Type)objectTranslator.GetObject(L, 3, typeof(Type));
			string propertyName = Lua.lua_tostring(L, 4);
			AnimationCurve curve = (AnimationCurve)objectTranslator.GetObject(L, 5, typeof(AnimationCurve));
			animationClip.SetCurve(relativePath, type, propertyName, curve);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnsureQuaternionContinuity(IntPtr L)
	{
		try
		{
			((AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnsureQuaternionContinuity();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearCurves(IntPtr L)
	{
		try
		{
			((AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearCurves();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			AnimationEvent evt = (AnimationEvent)objectTranslator.GetObject(L, 2, typeof(AnimationEvent));
			animationClip.AddEvent(evt);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_length(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animationClip.length);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameRate(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animationClip.frameRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wrapMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animationClip.wrapMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, animationClip.localBounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_legacy(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.legacy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_humanMotion(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.humanMotion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_empty(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.empty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasGenericRootTransform(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.hasGenericRootTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasMotionFloatCurves(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.hasMotionFloatCurves);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasMotionCurves(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.hasMotionCurves);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasRootCurves(IntPtr L)
	{
		try
		{
			AnimationClip animationClip = (AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animationClip.hasRootCurves);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_events(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animationClip.events);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_frameRate(IntPtr L)
	{
		try
		{
			((AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).frameRate = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wrapMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WrapMode v);
			animationClip.wrapMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_localBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationClip animationClip = (AnimationClip)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Bounds val);
			animationClip.localBounds = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_legacy(IntPtr L)
	{
		try
		{
			((AnimationClip)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).legacy = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_events(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((AnimationClip)objectTranslator.FastGetCSObj(L, 1)).events = (AnimationEvent[])objectTranslator.GetObject(L, 2, typeof(AnimationEvent[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
