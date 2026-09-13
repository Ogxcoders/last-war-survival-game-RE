using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningCoreTweenerCore_3_UnityEngineVector3UnityEngineVector3DGTweeningPluginsOptionsVectorOptions_Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TweenerCore<Vector3, Vector3, VectorOptions>);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 30, 6, 6);
		Utils.RegisterFunc(L, -3, "ChangeStartValue", _m_ChangeStartValue);
		Utils.RegisterFunc(L, -3, "ChangeEndValue", _m_ChangeEndValue);
		Utils.RegisterFunc(L, -3, "ChangeValues", _m_ChangeValues);
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
		Utils.RegisterFunc(L, -3, "From", _m_From);
		Utils.RegisterFunc(L, -3, "SetDelay", _m_SetDelay);
		Utils.RegisterFunc(L, -3, "SetRelative", _m_SetRelative);
		Utils.RegisterFunc(L, -3, "SetSpeedBased", _m_SetSpeedBased);
		Utils.RegisterFunc(L, -3, "SetOptions", _m_SetOptions);
		Utils.RegisterFunc(L, -3, "SetSpecialStartupMode", _m_SetSpecialStartupMode);
		Utils.RegisterFunc(L, -3, "ScriptOnComplete", _m_ScriptOnComplete);
		Utils.RegisterFunc(L, -2, "startValue", _g_get_startValue);
		Utils.RegisterFunc(L, -2, "endValue", _g_get_endValue);
		Utils.RegisterFunc(L, -2, "changeValue", _g_get_changeValue);
		Utils.RegisterFunc(L, -2, "plugOptions", _g_get_plugOptions);
		Utils.RegisterFunc(L, -2, "getter", _g_get_getter);
		Utils.RegisterFunc(L, -2, "setter", _g_get_setter);
		Utils.RegisterFunc(L, -1, "startValue", _s_set_startValue);
		Utils.RegisterFunc(L, -1, "endValue", _s_set_endValue);
		Utils.RegisterFunc(L, -1, "changeValue", _s_set_changeValue);
		Utils.RegisterFunc(L, -1, "plugOptions", _s_set_plugOptions);
		Utils.RegisterFunc(L, -1, "getter", _s_set_getter);
		Utils.RegisterFunc(L, -1, "setter", _s_set_setter);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions> does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeStartValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				object newStartValue = objectTranslator.GetObject(L, 2, typeof(object));
				float newDuration = (float)Lua.lua_tonumber(L, 3);
				Tweener o = tweenerCore.ChangeStartValue(newStartValue, newDuration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object newStartValue2 = objectTranslator.GetObject(L, 2, typeof(object));
				Tweener o2 = tweenerCore.ChangeStartValue(newStartValue2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float newDuration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o3 = tweenerCore.ChangeStartValue(val, newDuration2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				TweenerCore<Vector3, Vector3, VectorOptions> o4 = tweenerCore.ChangeStartValue(val2);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.ChangeStartValue!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeEndValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				object newEndValue = objectTranslator.GetObject(L, 2, typeof(object));
				bool snapStartValue = Lua.lua_toboolean(L, 3);
				Tweener o = tweenerCore.ChangeEndValue(newEndValue, snapStartValue);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				bool snapStartValue2 = Lua.lua_toboolean(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = tweenerCore.ChangeEndValue(val, snapStartValue2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				object newEndValue2 = objectTranslator.GetObject(L, 2, typeof(object));
				float newDuration = (float)Lua.lua_tonumber(L, 3);
				bool snapStartValue3 = Lua.lua_toboolean(L, 4);
				Tweener o3 = tweenerCore.ChangeEndValue(newEndValue2, newDuration, snapStartValue3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				object newEndValue3 = objectTranslator.GetObject(L, 2, typeof(object));
				float newDuration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = tweenerCore.ChangeEndValue(newEndValue3, newDuration2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object newEndValue4 = objectTranslator.GetObject(L, 2, typeof(object));
				Tweener o5 = tweenerCore.ChangeEndValue(newEndValue4);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float newDuration3 = (float)Lua.lua_tonumber(L, 3);
				bool snapStartValue4 = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o6 = tweenerCore.ChangeEndValue(val2, newDuration3, snapStartValue4);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float newDuration4 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o7 = tweenerCore.ChangeEndValue(val3, newDuration4);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				TweenerCore<Vector3, Vector3, VectorOptions> o8 = tweenerCore.ChangeEndValue(val4);
				objectTranslator.Push(L, o8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.ChangeEndValue!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeValues(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				object newStartValue = objectTranslator.GetObject(L, 2, typeof(object));
				object newEndValue = objectTranslator.GetObject(L, 3, typeof(object));
				float newDuration = (float)Lua.lua_tonumber(L, 4);
				Tweener o = tweenerCore.ChangeValues(newStartValue, newEndValue, newDuration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3))
			{
				object newStartValue2 = objectTranslator.GetObject(L, 2, typeof(object));
				object newEndValue2 = objectTranslator.GetObject(L, 3, typeof(object));
				Tweener o2 = tweenerCore.ChangeValues(newStartValue2, newEndValue2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Vector3 val2);
				float newDuration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o3 = tweenerCore.ChangeValues(val, val2, newDuration2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Vector3 val4);
				TweenerCore<Vector3, Vector3, VectorOptions> o4 = tweenerCore.ChangeValues(val3, val4);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.ChangeValues!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween o = ((TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1)).Pause();
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
			Tween o = ((TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1)).Play();
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
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tweenerCore.SetAutoKill();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool autoKillOnCompletion = Lua.lua_toboolean(L, 2);
					Tween o = tweenerCore.SetAutoKill(autoKillOnCompletion);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetAutoKill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetId!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLink(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetLink!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetLoops!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetEase(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetEase!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRecyclable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tweenerCore.SetRecyclable();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool recyclable = Lua.lua_toboolean(L, 2);
					Tween o = tweenerCore.SetRecyclable(recyclable);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetRecyclable!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetAs!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_From(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
			{
				Tweener o2 = t.From();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isRelative = Lua.lua_toboolean(L, 2);
					Tweener o = t.From(isRelative);
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float fromValue = (float)Lua.lua_tonumber(L, 2);
				bool setImmediately = Lua.lua_toboolean(L, 3);
				bool isRelative2 = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o3 = t.From(fromValue, setImmediately, isRelative2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float fromValue2 = (float)Lua.lua_tonumber(L, 2);
				bool setImmediately2 = Lua.lua_toboolean(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o4 = t.From(fromValue2, setImmediately2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float fromValue3 = (float)Lua.lua_tonumber(L, 2);
				TweenerCore<Vector3, Vector3, VectorOptions> o5 = t.From(fromValue3);
				objectTranslator.Push(L, o5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.From!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDelay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetDelay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRelative(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tweenerCore.SetRelative();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isRelative = Lua.lua_toboolean(L, 2);
					Tween o = tweenerCore.SetRelative(isRelative);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetRelative!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeedBased(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tweenerCore.SetSpeedBased();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isSpeedBased = Lua.lua_toboolean(L, 2);
					Tween o = tweenerCore.SetSpeedBased(isSpeedBased);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetSpeedBased!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOptions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool snapping = Lua.lua_toboolean(L, 2);
				Tweener o = t.SetOptions(snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<AxisConstraint>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out AxisConstraint v);
				bool snapping2 = Lua.lua_toboolean(L, 3);
				Tweener o2 = t.SetOptions(v, snapping2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<AxisConstraint>(L, 2))
			{
				objectTranslator.Get(L, 2, out AxisConstraint v2);
				Tweener o3 = t.SetOptions(v2);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>.SetOptions!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpecialStartupMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> t = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
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
			TweenerCore<Vector3, Vector3, VectorOptions> tween = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			TweenCallback action = objectTranslator.GetDelegate<TweenCallback>(L, 2);
			tween.ScriptOnComplete(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, tweenerCore.startValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, tweenerCore.endValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_changeValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, tweenerCore.changeValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_plugOptions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tweenerCore.plugOptions);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_getter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tweenerCore.getter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_setter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tweenerCore.setter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			tweenerCore.startValue = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			tweenerCore.endValue = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_changeValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			tweenerCore.changeValue = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_plugOptions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = (TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VectorOptions v);
			tweenerCore.plugOptions = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_getter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1)).getter = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_setter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TweenerCore<Vector3, Vector3, VectorOptions>)objectTranslator.FastGetCSObj(L, 1)).setter = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
