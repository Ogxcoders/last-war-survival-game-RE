using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningSequenceWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Sequence);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 34, 0, 0);
		Utils.RegisterFunc(L, -3, "Pause", _m_Pause);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "SetAutoKill", _m_SetAutoKill);
		Utils.RegisterFunc(L, -3, "SetId", _m_SetId);
		Utils.RegisterFunc(L, -3, "SetLink", _m_SetLink);
		Utils.RegisterFunc(L, -3, "SetTarget", _m_SetTarget);
		Utils.RegisterFunc(L, -3, "SetLoops", _m_SetLoops);
		Utils.RegisterFunc(L, -3, "SetEase", _m_SetEase);
		Utils.RegisterFunc(L, -3, "SetRecyclable", _m_SetRecyclable);
		Utils.RegisterFunc(L, -3, "SetUpdate", _m_SetUpdate);
		Utils.RegisterFunc(L, -3, "OnStart", _m_OnStart);
		Utils.RegisterFunc(L, -3, "OnPlay", _m_OnPlay);
		Utils.RegisterFunc(L, -3, "OnPause", _m_OnPause);
		Utils.RegisterFunc(L, -3, "OnRewind", _m_OnRewind);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "OnStepComplete", _m_OnStepComplete);
		Utils.RegisterFunc(L, -3, "OnComplete", _m_OnComplete);
		Utils.RegisterFunc(L, -3, "OnKill", _m_OnKill);
		Utils.RegisterFunc(L, -3, "OnWaypointChange", _m_OnWaypointChange);
		Utils.RegisterFunc(L, -3, "SetAs", _m_SetAs);
		Utils.RegisterFunc(L, -3, "Append", _m_Append);
		Utils.RegisterFunc(L, -3, "Prepend", _m_Prepend);
		Utils.RegisterFunc(L, -3, "Join", _m_Join);
		Utils.RegisterFunc(L, -3, "Insert", _m_Insert);
		Utils.RegisterFunc(L, -3, "AppendInterval", _m_AppendInterval);
		Utils.RegisterFunc(L, -3, "PrependInterval", _m_PrependInterval);
		Utils.RegisterFunc(L, -3, "AppendCallback", _m_AppendCallback);
		Utils.RegisterFunc(L, -3, "PrependCallback", _m_PrependCallback);
		Utils.RegisterFunc(L, -3, "InsertCallback", _m_InsertCallback);
		Utils.RegisterFunc(L, -3, "SetDelay", _m_SetDelay);
		Utils.RegisterFunc(L, -3, "SetRelative", _m_SetRelative);
		Utils.RegisterFunc(L, -3, "SetSpeedBased", _m_SetSpeedBased);
		Utils.RegisterFunc(L, -3, "SetSpecialStartupMode", _m_SetSpecialStartupMode);
		Utils.RegisterFunc(L, -3, "ScriptOnComplete", _m_ScriptOnComplete);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "DG.Tweening.Sequence does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween o = ((Sequence)objectTranslator.FastGetCSObj(L, 1)).Pause();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween o = ((Sequence)objectTranslator.FastGetCSObj(L, 1)).Play();
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
			Sequence sequence = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = sequence.SetAutoKill();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool autoKillOnCompletion = Lua.lua_toboolean(L, 2);
					Tween o = sequence.SetAutoKill(autoKillOnCompletion);
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetAutoKill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int intId = Lua.xlua_tointeger(L, 2);
				Tween o = t.SetId(intId);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object objectId = objectTranslator.GetObject(L, 2, typeof(object));
				Tween o2 = t.SetId(objectId);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string stringId = Lua.lua_tostring(L, 2);
				Tween o3 = t.SetId(stringId);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetId!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLink(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<GameObject>(L, 2))
			{
				GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
				Tween o = t.SetLink(gameObject);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<GameObject>(L, 2) && objectTranslator.Assignable<LinkBehaviour>(L, 3))
			{
				GameObject gameObject2 = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
				objectTranslator.Get(L, 3, out LinkBehaviour v);
				Tween o2 = t.SetLink(gameObject2, v);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetLink!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			object target = objectTranslator.GetObject(L, 2, typeof(object));
			Tween o = t.SetTarget(target);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int loops = Lua.xlua_tointeger(L, 2);
				Tween o = t.SetLoops(loops);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<LoopType>(L, 3))
			{
				int loops2 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out LoopType v);
				Tween o2 = t.SetLoops(loops2, v);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetLoops!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetEase(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Ease>(L, 2))
			{
				objectTranslator.Get(L, 2, out Ease val);
				Tween o = t.SetEase(val);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<AnimationCurve>(L, 2))
			{
				AnimationCurve animCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
				Tween o2 = t.SetEase(animCurve);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<EaseFunction>(L, 2))
			{
				EaseFunction customEase = objectTranslator.GetDelegate<EaseFunction>(L, 2);
				Tween o3 = t.SetEase(customEase);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ease>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Ease val2);
				float overshoot = (float)Lua.lua_tonumber(L, 3);
				Tween o4 = t.SetEase(val2, overshoot);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Ease>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Ease val3);
				float amplitude = (float)Lua.lua_tonumber(L, 3);
				float period = (float)Lua.lua_tonumber(L, 4);
				Tween o5 = t.SetEase(val3, amplitude, period);
				objectTranslator.Push(L, o5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetEase!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRecyclable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence sequence = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = sequence.SetRecyclable();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool recyclable = Lua.lua_toboolean(L, 2);
					Tween o = sequence.SetRecyclable(recyclable);
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetRecyclable!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool isIndependentUpdate = Lua.lua_toboolean(L, 2);
				Tween o = t.SetUpdate(isIndependentUpdate);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<UpdateType>(L, 2))
			{
				objectTranslator.Get(L, 2, out UpdateType v);
				Tween o2 = t.SetUpdate(v);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UpdateType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out UpdateType v2);
				bool isIndependentUpdate2 = Lua.lua_toboolean(L, 3);
				Tween o3 = t.SetUpdate(v2, isIndependentUpdate2);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnStart(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnPlay(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnPause(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnRewind(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnUpdate(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnStepComplete(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnComplete(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Tween o = t.OnKill(action);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback<int> action = objectTranslator.GetDelegate<TweenCallback<int>>(L, 2);
			Tween o = t.OnWaypointChange(action);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Tween>(L, 2))
			{
				Tween asTween = (Tween)objectTranslator.GetObject(L, 2, typeof(Tween));
				Tween o = t.SetAs(asTween);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<TweenParams>(L, 2))
			{
				TweenParams tweenParams = (TweenParams)objectTranslator.GetObject(L, 2, typeof(TweenParams));
				Tween o2 = t.SetAs(tweenParams);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetAs!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Append(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			Tween t = (Tween)objectTranslator.GetObject(L, 2, typeof(Tween));
			Sequence o = s.Append(t);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Prepend(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			Tween t = (Tween)objectTranslator.GetObject(L, 2, typeof(Tween));
			Sequence o = s.Prepend(t);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Join(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			Tween t = (Tween)objectTranslator.GetObject(L, 2, typeof(Tween));
			Sequence o = s.Join(t);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Insert(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			float atPosition = (float)Lua.lua_tonumber(L, 2);
			Tween t = (Tween)objectTranslator.GetObject(L, 3, typeof(Tween));
			Sequence o = s.Insert(atPosition, t);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AppendInterval(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			float interval = (float)Lua.lua_tonumber(L, 2);
			Sequence o = s.AppendInterval(interval);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrependInterval(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			float interval = (float)Lua.lua_tonumber(L, 2);
			Sequence o = s.PrependInterval(interval);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AppendCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback callback = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Sequence o = s.AppendCallback(callback);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrependCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback callback = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			Sequence o = s.PrependCallback(callback);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence s = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			float atPosition = (float)Lua.lua_tonumber(L, 2);
			TweenCallback callback = objectTranslator.GetDelegate<TweenCallback>(L, 3);
			Sequence o = s.InsertCallback(atPosition, callback);
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
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float delay = (float)Lua.lua_tonumber(L, 2);
				Tween o = t.SetDelay(delay);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float delay2 = (float)Lua.lua_tonumber(L, 2);
				bool asPrependedIntervalIfSequence = Lua.lua_toboolean(L, 3);
				Tween o2 = t.SetDelay(delay2, asPrependedIntervalIfSequence);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetDelay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRelative(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence sequence = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = sequence.SetRelative();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isRelative = Lua.lua_toboolean(L, 2);
					Tween o = sequence.SetRelative(isRelative);
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetRelative!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeedBased(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence sequence = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = sequence.SetSpeedBased();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isSpeedBased = Lua.lua_toboolean(L, 2);
					Tween o = sequence.SetSpeedBased(isSpeedBased);
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Sequence.SetSpeedBased!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpecialStartupMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence t = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SpecialStartupMode v);
			Tween o = t.SetSpecialStartupMode(v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScriptOnComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence tween = (Sequence)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			tween.ScriptOnComplete(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
