using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BezierMovementWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BezierMovement);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 1, 1);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "StartAuto", _m_StartAuto);
		Utils.RegisterFunc(L, -3, "ManualUpdate", _m_ManualUpdate);
		Utils.RegisterFunc(L, -2, "curve", _g_get_curve);
		Utils.RegisterFunc(L, -1, "curve", _s_set_curve);
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
				BezierMovement o = new BezierMovement();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BezierMovement constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BezierMovement bezierMovement = (BezierMovement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			float duration = (float)Lua.lua_tonumber(L, 4);
			float maxHeight = (float)Lua.lua_tonumber(L, 5);
			bezierMovement.Init(val, val2, duration, maxHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartAuto(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BezierMovement bezierMovement = (BezierMovement)objectTranslator.FastGetCSObj(L, 1);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 2);
			bezierMovement.StartAuto(onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ManualUpdate(IntPtr L)
	{
		try
		{
			BezierMovement obj = (BezierMovement)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float progress = (float)Lua.lua_tonumber(L, 2);
			obj.ManualUpdate(progress);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BezierMovement bezierMovement = (BezierMovement)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, bezierMovement.curve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BezierMovement)objectTranslator.FastGetCSObj(L, 1)).curve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
