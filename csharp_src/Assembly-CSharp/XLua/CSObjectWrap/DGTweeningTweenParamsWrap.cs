using System;
using DG.Tweening;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningTweenParamsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TweenParams);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 19, 0, 0);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "SetAutoKill", _m_SetAutoKill);
		Utils.RegisterFunc(L, -3, "SetId", _m_SetId);
		Utils.RegisterFunc(L, -3, "SetTarget", _m_SetTarget);
		Utils.RegisterFunc(L, -3, "SetLoops", _m_SetLoops);
		Utils.RegisterFunc(L, -3, "SetEase", _m_SetEase);
		Utils.RegisterFunc(L, -3, "SetRecyclable", _m_SetRecyclable);
		Utils.RegisterFunc(L, -3, "SetUpdate", _m_SetUpdate);
		Utils.RegisterFunc(L, -3, "OnStart", _m_OnStart);
		Utils.RegisterFunc(L, -3, "OnPlay", _m_OnPlay);
		Utils.RegisterFunc(L, -3, "OnRewind", _m_OnRewind);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "OnStepComplete", _m_OnStepComplete);
		Utils.RegisterFunc(L, -3, "OnComplete", _m_OnComplete);
		Utils.RegisterFunc(L, -3, "OnKill", _m_OnKill);
		Utils.RegisterFunc(L, -3, "OnWaypointChange", _m_OnWaypointChange);
		Utils.RegisterFunc(L, -3, "SetDelay", _m_SetDelay);
		Utils.RegisterFunc(L, -3, "SetRelative", _m_SetRelative);
		Utils.RegisterFunc(L, -3, "SetSpeedBased", _m_SetSpeedBased);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Params", TweenParams.Params);
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
				TweenParams o = new TweenParams();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams o = ((TweenParams)objectTranslator.FastGetCSObj(L, 1)).Clear();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAutoKill(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool autoKill = Lua.lua_toboolean(L, 2);
				TweenParams o = tweenParams.SetAutoKill(autoKill);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1)
			{
				TweenParams o2 = tweenParams.SetAutoKill();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetAutoKill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			object id = objectTranslator.GetObject(L, 2, typeof(object));
			TweenParams o = tweenParams.SetId(id);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			object target = objectTranslator.GetObject(L, 2, typeof(object));
			TweenParams o = tweenParams.SetTarget(target);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLoops(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<LoopType?>(L, 3))
			{
				int loops = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out LoopType? v);
				TweenParams o = tweenParams.SetLoops(loops, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int loops2 = Lua.xlua_tointeger(L, 2);
				TweenParams o2 = tweenParams.SetLoops(loops2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetLoops!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetEase(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<AnimationCurve>(L, 2))
			{
				AnimationCurve ease = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
				TweenParams o = tweenParams.SetEase(ease);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<EaseFunction>(L, 2))
			{
				EaseFunction ease2 = objectTranslator.GetDelegate<EaseFunction>(L, 2);
				TweenParams o2 = tweenParams.SetEase(ease2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Ease>(L, 2) && objectTranslator.Assignable<float?>(L, 3) && objectTranslator.Assignable<float?>(L, 4))
			{
				objectTranslator.Get(L, 2, out Ease val);
				objectTranslator.Get(L, 3, out float? v);
				objectTranslator.Get(L, 4, out float? v2);
				TweenParams o3 = tweenParams.SetEase(val, v, v2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ease>(L, 2) && objectTranslator.Assignable<float?>(L, 3))
			{
				objectTranslator.Get(L, 2, out Ease val2);
				objectTranslator.Get(L, 3, out float? v3);
				TweenParams o4 = tweenParams.SetEase(val2, v3);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Ease>(L, 2))
			{
				objectTranslator.Get(L, 2, out Ease val3);
				TweenParams o5 = tweenParams.SetEase(val3);
				objectTranslator.Push(L, o5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetEase!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRecyclable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool recyclable = Lua.lua_toboolean(L, 2);
				TweenParams o = tweenParams.SetRecyclable(recyclable);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1)
			{
				TweenParams o2 = tweenParams.SetRecyclable();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetRecyclable!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool update = Lua.lua_toboolean(L, 2);
				TweenParams o = tweenParams.SetUpdate(update);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UpdateType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out UpdateType v);
				bool isIndependentUpdate = Lua.lua_toboolean(L, 3);
				TweenParams o2 = tweenParams.SetUpdate(v, isIndependentUpdate);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<UpdateType>(L, 2))
			{
				objectTranslator.Get(L, 2, out UpdateType v2);
				TweenParams o3 = tweenParams.SetUpdate(v2);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnStart(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPlay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnPlay(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnRewind(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnRewind(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnUpdate(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStepComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnStepComplete(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnComplete(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnKill(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			TweenParams o = tweenParams.OnKill(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnWaypointChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback<int> action = objectTranslator.GetDelegate<TweenCallback<int>>(L, 2);
			TweenParams o = tweenParams.OnWaypointChange(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDelay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams obj = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			float delay = (float)Lua.lua_tonumber(L, 2);
			TweenParams o = obj.SetDelay(delay);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRelative(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool relative = Lua.lua_toboolean(L, 2);
				TweenParams o = tweenParams.SetRelative(relative);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1)
			{
				TweenParams o2 = tweenParams.SetRelative();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetRelative!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeedBased(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenParams tweenParams = (TweenParams)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool speedBased = Lua.lua_toboolean(L, 2);
				TweenParams o = tweenParams.SetSpeedBased(speedBased);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1)
			{
				TweenParams o2 = tweenParams.SetSpeedBased();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.TweenParams.SetSpeedBased!");
	}
}
