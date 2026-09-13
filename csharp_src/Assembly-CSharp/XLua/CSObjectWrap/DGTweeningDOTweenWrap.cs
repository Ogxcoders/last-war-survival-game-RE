using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningDOTweenWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DOTween);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 45, 21, 21);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetTweensCapacity", _m_SetTweensCapacity_xlua_st_);
		Utils.RegisterFunc(L, -4, "Clear", _m_Clear_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearCachedTweens", _m_ClearCachedTweens_xlua_st_);
		Utils.RegisterFunc(L, -4, "Validate", _m_Validate_xlua_st_);
		Utils.RegisterFunc(L, -4, "ManualUpdate", _m_ManualUpdate_xlua_st_);
		Utils.RegisterFunc(L, -4, "To", _m_To_xlua_st_);
		Utils.RegisterFunc(L, -4, "ToAxis", _m_ToAxis_xlua_st_);
		Utils.RegisterFunc(L, -4, "ToAlpha", _m_ToAlpha_xlua_st_);
		Utils.RegisterFunc(L, -4, "Punch", _m_Punch_xlua_st_);
		Utils.RegisterFunc(L, -4, "Shake", _m_Shake_xlua_st_);
		Utils.RegisterFunc(L, -4, "ToArray", _m_ToArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "Sequence", _m_Sequence_xlua_st_);
		Utils.RegisterFunc(L, -4, "CompleteAll", _m_CompleteAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Complete", _m_Complete_xlua_st_);
		Utils.RegisterFunc(L, -4, "FlipAll", _m_FlipAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Flip", _m_Flip_xlua_st_);
		Utils.RegisterFunc(L, -4, "GotoAll", _m_GotoAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Goto", _m_Goto_xlua_st_);
		Utils.RegisterFunc(L, -4, "KillAll", _m_KillAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Kill", _m_Kill_xlua_st_);
		Utils.RegisterFunc(L, -4, "PauseAll", _m_PauseAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Pause", _m_Pause_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayAll", _m_PlayAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Play", _m_Play_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayBackwardsAll", _m_PlayBackwardsAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayBackwards", _m_PlayBackwards_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayForwardAll", _m_PlayForwardAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayForward", _m_PlayForward_xlua_st_);
		Utils.RegisterFunc(L, -4, "RestartAll", _m_RestartAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Restart", _m_Restart_xlua_st_);
		Utils.RegisterFunc(L, -4, "RewindAll", _m_RewindAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Rewind", _m_Rewind_xlua_st_);
		Utils.RegisterFunc(L, -4, "SmoothRewindAll", _m_SmoothRewindAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "SmoothRewind", _m_SmoothRewind_xlua_st_);
		Utils.RegisterFunc(L, -4, "TogglePauseAll", _m_TogglePauseAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "TogglePause", _m_TogglePause_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsTweening", _m_IsTweening_xlua_st_);
		Utils.RegisterFunc(L, -4, "TotalPlayingTweens", _m_TotalPlayingTweens_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayingTweens", _m_PlayingTweens_xlua_st_);
		Utils.RegisterFunc(L, -4, "PausedTweens", _m_PausedTweens_xlua_st_);
		Utils.RegisterFunc(L, -4, "TweensById", _m_TweensById_xlua_st_);
		Utils.RegisterFunc(L, -4, "TweensByTarget", _m_TweensByTarget_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "Version", DOTween.Version);
		Utils.RegisterFunc(L, -2, "logBehaviour", _g_get_logBehaviour);
		Utils.RegisterFunc(L, -2, "debugStoreTargetId", _g_get_debugStoreTargetId);
		Utils.RegisterFunc(L, -2, "useSafeMode", _g_get_useSafeMode);
		Utils.RegisterFunc(L, -2, "nestedTweenFailureBehaviour", _g_get_nestedTweenFailureBehaviour);
		Utils.RegisterFunc(L, -2, "showUnityEditorReport", _g_get_showUnityEditorReport);
		Utils.RegisterFunc(L, -2, "timeScale", _g_get_timeScale);
		Utils.RegisterFunc(L, -2, "useSmoothDeltaTime", _g_get_useSmoothDeltaTime);
		Utils.RegisterFunc(L, -2, "maxSmoothUnscaledTime", _g_get_maxSmoothUnscaledTime);
		Utils.RegisterFunc(L, -2, "onWillLog", _g_get_onWillLog);
		Utils.RegisterFunc(L, -2, "drawGizmos", _g_get_drawGizmos);
		Utils.RegisterFunc(L, -2, "debugMode", _g_get_debugMode);
		Utils.RegisterFunc(L, -2, "defaultUpdateType", _g_get_defaultUpdateType);
		Utils.RegisterFunc(L, -2, "defaultTimeScaleIndependent", _g_get_defaultTimeScaleIndependent);
		Utils.RegisterFunc(L, -2, "defaultAutoPlay", _g_get_defaultAutoPlay);
		Utils.RegisterFunc(L, -2, "defaultAutoKill", _g_get_defaultAutoKill);
		Utils.RegisterFunc(L, -2, "defaultLoopType", _g_get_defaultLoopType);
		Utils.RegisterFunc(L, -2, "defaultRecyclable", _g_get_defaultRecyclable);
		Utils.RegisterFunc(L, -2, "defaultEaseType", _g_get_defaultEaseType);
		Utils.RegisterFunc(L, -2, "defaultEaseOvershootOrAmplitude", _g_get_defaultEaseOvershootOrAmplitude);
		Utils.RegisterFunc(L, -2, "defaultEasePeriod", _g_get_defaultEasePeriod);
		Utils.RegisterFunc(L, -2, "instance", _g_get_instance);
		Utils.RegisterFunc(L, -1, "logBehaviour", _s_set_logBehaviour);
		Utils.RegisterFunc(L, -1, "debugStoreTargetId", _s_set_debugStoreTargetId);
		Utils.RegisterFunc(L, -1, "useSafeMode", _s_set_useSafeMode);
		Utils.RegisterFunc(L, -1, "nestedTweenFailureBehaviour", _s_set_nestedTweenFailureBehaviour);
		Utils.RegisterFunc(L, -1, "showUnityEditorReport", _s_set_showUnityEditorReport);
		Utils.RegisterFunc(L, -1, "timeScale", _s_set_timeScale);
		Utils.RegisterFunc(L, -1, "useSmoothDeltaTime", _s_set_useSmoothDeltaTime);
		Utils.RegisterFunc(L, -1, "maxSmoothUnscaledTime", _s_set_maxSmoothUnscaledTime);
		Utils.RegisterFunc(L, -1, "onWillLog", _s_set_onWillLog);
		Utils.RegisterFunc(L, -1, "drawGizmos", _s_set_drawGizmos);
		Utils.RegisterFunc(L, -1, "debugMode", _s_set_debugMode);
		Utils.RegisterFunc(L, -1, "defaultUpdateType", _s_set_defaultUpdateType);
		Utils.RegisterFunc(L, -1, "defaultTimeScaleIndependent", _s_set_defaultTimeScaleIndependent);
		Utils.RegisterFunc(L, -1, "defaultAutoPlay", _s_set_defaultAutoPlay);
		Utils.RegisterFunc(L, -1, "defaultAutoKill", _s_set_defaultAutoKill);
		Utils.RegisterFunc(L, -1, "defaultLoopType", _s_set_defaultLoopType);
		Utils.RegisterFunc(L, -1, "defaultRecyclable", _s_set_defaultRecyclable);
		Utils.RegisterFunc(L, -1, "defaultEaseType", _s_set_defaultEaseType);
		Utils.RegisterFunc(L, -1, "defaultEaseOvershootOrAmplitude", _s_set_defaultEaseOvershootOrAmplitude);
		Utils.RegisterFunc(L, -1, "defaultEasePeriod", _s_set_defaultEasePeriod);
		Utils.RegisterFunc(L, -1, "instance", _s_set_instance);
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
				DOTween o = new DOTween();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<bool?>(L, 1) && objectTranslator.Assignable<bool?>(L, 2) && objectTranslator.Assignable<LogBehaviour?>(L, 3))
			{
				objectTranslator.Get(L, 1, out bool? v);
				objectTranslator.Get(L, 2, out bool? v2);
				objectTranslator.Get(L, 3, out LogBehaviour? v3);
				IDOTweenInit o = DOTween.Init(v, v2, v3);
				objectTranslator.PushAny(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<bool?>(L, 1) && objectTranslator.Assignable<bool?>(L, 2))
			{
				objectTranslator.Get(L, 1, out bool? v4);
				objectTranslator.Get(L, 2, out bool? v5);
				IDOTweenInit o2 = DOTween.Init(v4, v5);
				objectTranslator.PushAny(L, o2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<bool?>(L, 1))
			{
				objectTranslator.Get(L, 1, out bool? v6);
				IDOTweenInit o3 = DOTween.Init(v6);
				objectTranslator.PushAny(L, o3);
				return 1;
			}
			if (num == 0)
			{
				IDOTweenInit o4 = DOTween.Init();
				objectTranslator.PushAny(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Init!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTweensCapacity_xlua_st_(IntPtr L)
	{
		try
		{
			int tweenersCapacity = Lua.xlua_tointeger(L, 1);
			int sequencesCapacity = Lua.xlua_tointeger(L, 2);
			DOTween.SetTweensCapacity(tweenersCapacity, sequencesCapacity);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				DOTween.Clear(Lua.lua_toboolean(L, 1));
				return 0;
			}
			if (num == 0)
			{
				DOTween.Clear();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Clear!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearCachedTweens_xlua_st_(IntPtr L)
	{
		try
		{
			DOTween.ClearCachedTweens();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Validate_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.Validate();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ManualUpdate_xlua_st_(IntPtr L)
	{
		try
		{
			float deltaTime = (float)Lua.lua_tonumber(L, 1);
			float unscaledDeltaTime = (float)Lua.lua_tonumber(L, 2);
			DOTween.ManualUpdate(deltaTime, unscaledDeltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_To_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<DOSetter<float>>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOSetter<float> setter = objectTranslator.GetDelegate<DOSetter<float>>(L, 1);
				float startValue = (float)Lua.lua_tonumber(L, 2);
				float endValue = (float)Lua.lua_tonumber(L, 3);
				float duration = (float)Lua.lua_tonumber(L, 4);
				Tweener o = DOTween.To(setter, startValue, endValue, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<float>>(L, 1) && objectTranslator.Assignable<DOSetter<float>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<float> getter = objectTranslator.GetDelegate<DOGetter<float>>(L, 1);
				DOSetter<float> setter2 = objectTranslator.GetDelegate<DOSetter<float>>(L, 2);
				float endValue2 = (float)Lua.lua_tonumber(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<float, float, FloatOptions> o2 = DOTween.To(getter, setter2, endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<double>>(L, 1) && objectTranslator.Assignable<DOSetter<double>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<double> getter2 = objectTranslator.GetDelegate<DOGetter<double>>(L, 1);
				DOSetter<double> setter3 = objectTranslator.GetDelegate<DOSetter<double>>(L, 2);
				double endValue3 = Lua.lua_tonumber(L, 3);
				float duration3 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<double, double, NoOptions> o3 = DOTween.To(getter2, setter3, endValue3, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<int>>(L, 1) && objectTranslator.Assignable<DOSetter<int>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<int> getter3 = objectTranslator.GetDelegate<DOGetter<int>>(L, 1);
				DOSetter<int> setter4 = objectTranslator.GetDelegate<DOSetter<int>>(L, 2);
				int endValue4 = Lua.xlua_tointeger(L, 3);
				float duration4 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<int, int, NoOptions> o4 = DOTween.To(getter3, setter4, endValue4, duration4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<uint>>(L, 1) && objectTranslator.Assignable<DOSetter<uint>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<uint> getter4 = objectTranslator.GetDelegate<DOGetter<uint>>(L, 1);
				DOSetter<uint> setter5 = objectTranslator.GetDelegate<DOSetter<uint>>(L, 2);
				uint endValue5 = Lua.xlua_touint(L, 3);
				float duration5 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<uint, uint, UintOptions> o5 = DOTween.To(getter4, setter5, endValue5, duration5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<long>>(L, 1) && objectTranslator.Assignable<DOSetter<long>>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<long> getter5 = objectTranslator.GetDelegate<DOGetter<long>>(L, 1);
				DOSetter<long> setter6 = objectTranslator.GetDelegate<DOSetter<long>>(L, 2);
				long endValue6 = Lua.lua_toint64(L, 3);
				float duration6 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<long, long, NoOptions> o6 = DOTween.To(getter5, setter6, endValue6, duration6);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<ulong>>(L, 1) && objectTranslator.Assignable<DOSetter<ulong>>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isuint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<ulong> getter6 = objectTranslator.GetDelegate<DOGetter<ulong>>(L, 1);
				DOSetter<ulong> setter7 = objectTranslator.GetDelegate<DOSetter<ulong>>(L, 2);
				ulong endValue7 = Lua.lua_touint64(L, 3);
				float duration7 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<ulong, ulong, NoOptions> o7 = DOTween.To(getter6, setter7, endValue7, duration7);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<string>>(L, 1) && objectTranslator.Assignable<DOSetter<string>>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<string> getter7 = objectTranslator.GetDelegate<DOGetter<string>>(L, 1);
				DOSetter<string> setter8 = objectTranslator.GetDelegate<DOSetter<string>>(L, 2);
				string endValue8 = Lua.lua_tostring(L, 3);
				float duration8 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<string, string, StringOptions> o8 = DOTween.To(getter7, setter8, endValue8, duration8);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector2>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector2>>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Vector2> getter8 = objectTranslator.GetDelegate<DOGetter<Vector2>>(L, 1);
				DOSetter<Vector2> setter9 = objectTranslator.GetDelegate<DOSetter<Vector2>>(L, 2);
				objectTranslator.Get(L, 3, out Vector2 val);
				TweenerCore<Vector2, Vector2, VectorOptions> o9 = DOTween.To(duration: (float)Lua.lua_tonumber(L, 4), getter: getter8, setter: setter9, endValue: val);
				objectTranslator.Push(L, o9);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Vector3> getter9 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter10 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				TweenerCore<Vector3, Vector3, VectorOptions> o10 = DOTween.To(duration: (float)Lua.lua_tonumber(L, 4), getter: getter9, setter: setter10, endValue: val2);
				objectTranslator.Push(L, o10);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector4>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector4>>(L, 2) && objectTranslator.Assignable<Vector4>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Vector4> getter10 = objectTranslator.GetDelegate<DOGetter<Vector4>>(L, 1);
				DOSetter<Vector4> setter11 = objectTranslator.GetDelegate<DOSetter<Vector4>>(L, 2);
				objectTranslator.Get(L, 3, out Vector4 val3);
				TweenerCore<Vector4, Vector4, VectorOptions> o11 = DOTween.To(duration: (float)Lua.lua_tonumber(L, 4), getter: getter10, setter: setter11, endValue: val3);
				objectTranslator.Push(L, o11);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Quaternion>>(L, 1) && objectTranslator.Assignable<DOSetter<Quaternion>>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Quaternion> getter11 = objectTranslator.GetDelegate<DOGetter<Quaternion>>(L, 1);
				DOSetter<Quaternion> setter12 = objectTranslator.GetDelegate<DOSetter<Quaternion>>(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val4);
				TweenerCore<Quaternion, Vector3, QuaternionOptions> o12 = DOTween.To(duration: (float)Lua.lua_tonumber(L, 4), getter: getter11, setter: setter12, endValue: val4);
				objectTranslator.Push(L, o12);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Color>>(L, 1) && objectTranslator.Assignable<DOSetter<Color>>(L, 2) && objectTranslator.Assignable<Color>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Color> getter12 = objectTranslator.GetDelegate<DOGetter<Color>>(L, 1);
				DOSetter<Color> setter13 = objectTranslator.GetDelegate<DOSetter<Color>>(L, 2);
				objectTranslator.Get(L, 3, out Color val5);
				TweenerCore<Color, Color, ColorOptions> o13 = DOTween.To(duration: (float)Lua.lua_tonumber(L, 4), getter: getter12, setter: setter13, endValue: val5);
				objectTranslator.Push(L, o13);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Rect>>(L, 1) && objectTranslator.Assignable<DOSetter<Rect>>(L, 2) && objectTranslator.Assignable<Rect>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Rect> getter13 = objectTranslator.GetDelegate<DOGetter<Rect>>(L, 1);
				DOSetter<Rect> setter14 = objectTranslator.GetDelegate<DOSetter<Rect>>(L, 2);
				objectTranslator.Get(L, 3, out Rect v);
				TweenerCore<Rect, Rect, RectOptions> o14 = DOTween.To(duration: (float)Lua.lua_tonumber(L, 4), getter: getter13, setter: setter14, endValue: v);
				objectTranslator.Push(L, o14);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<RectOffset>>(L, 1) && objectTranslator.Assignable<DOSetter<RectOffset>>(L, 2) && objectTranslator.Assignable<RectOffset>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<RectOffset> getter14 = objectTranslator.GetDelegate<DOGetter<RectOffset>>(L, 1);
				DOSetter<RectOffset> setter15 = objectTranslator.GetDelegate<DOSetter<RectOffset>>(L, 2);
				RectOffset endValue9 = (RectOffset)objectTranslator.GetObject(L, 3, typeof(RectOffset));
				float duration9 = (float)Lua.lua_tonumber(L, 4);
				Tweener o15 = DOTween.To(getter14, setter15, endValue9, duration9);
				objectTranslator.Push(L, o15);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.To!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToAxis_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<AxisConstraint>(L, 5))
			{
				DOGetter<Vector3> getter = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float endValue = (float)Lua.lua_tonumber(L, 3);
				float duration = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out AxisConstraint v);
				TweenerCore<Vector3, Vector3, VectorOptions> o = DOTween.ToAxis(getter, setter, endValue, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Vector3> getter2 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter2 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float endValue2 = (float)Lua.lua_tonumber(L, 3);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = DOTween.ToAxis(getter2, setter2, endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.ToAxis!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToAlpha_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOGetter<Color> getter = objectTranslator.GetDelegate<DOGetter<Color>>(L, 1);
			DOSetter<Color> setter = objectTranslator.GetDelegate<DOSetter<Color>>(L, 2);
			float endValue = (float)Lua.lua_tonumber(L, 3);
			float duration = (float)Lua.lua_tonumber(L, 4);
			TweenerCore<Color, Color, ColorOptions> o = DOTween.ToAlpha(getter, setter, endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Punch_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				DOGetter<Vector3> getter = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o = DOTween.Punch(duration: (float)Lua.lua_tonumber(L, 4), vibrato: Lua.xlua_tointeger(L, 5), elasticity: (float)Lua.lua_tonumber(L, 6), getter: getter, setter: setter, direction: val);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				DOGetter<Vector3> getter2 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter2 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o2 = DOTween.Punch(duration: (float)Lua.lua_tonumber(L, 4), vibrato: Lua.xlua_tointeger(L, 5), getter: getter2, setter: setter2, direction: val2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Vector3> getter3 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter3 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o3 = DOTween.Punch(duration: (float)Lua.lua_tonumber(L, 4), getter: getter3, setter: setter3, direction: val3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Punch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shake_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 8 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				DOGetter<Vector3> getter = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				float strength = (float)Lua.lua_tonumber(L, 4);
				int vibrato = Lua.xlua_tointeger(L, 5);
				float randomness = (float)Lua.lua_tonumber(L, 6);
				bool ignoreZAxis = Lua.lua_toboolean(L, 7);
				bool fadeOut = Lua.lua_toboolean(L, 8);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o = DOTween.Shake(getter, setter, duration, strength, vibrato, randomness, ignoreZAxis, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				DOGetter<Vector3> getter2 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter2 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				float strength2 = (float)Lua.lua_tonumber(L, 4);
				int vibrato2 = Lua.xlua_tointeger(L, 5);
				float randomness2 = (float)Lua.lua_tonumber(L, 6);
				bool ignoreZAxis2 = Lua.lua_toboolean(L, 7);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o2 = DOTween.Shake(getter2, setter2, duration2, strength2, vibrato2, randomness2, ignoreZAxis2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				DOGetter<Vector3> getter3 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter3 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				float strength3 = (float)Lua.lua_tonumber(L, 4);
				int vibrato3 = Lua.xlua_tointeger(L, 5);
				float randomness3 = (float)Lua.lua_tonumber(L, 6);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o3 = DOTween.Shake(getter3, setter3, duration3, strength3, vibrato3, randomness3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				DOGetter<Vector3> getter4 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter4 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				float strength4 = (float)Lua.lua_tonumber(L, 4);
				int vibrato4 = Lua.xlua_tointeger(L, 5);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o4 = DOTween.Shake(getter4, setter4, duration4, strength4, vibrato4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				DOGetter<Vector3> getter5 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter5 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration5 = (float)Lua.lua_tonumber(L, 3);
				float strength5 = (float)Lua.lua_tonumber(L, 4);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o5 = DOTween.Shake(getter5, setter5, duration5, strength5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				DOGetter<Vector3> getter6 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter6 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration6 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o6 = DOTween.Shake(getter6, setter6, duration6);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				DOGetter<Vector3> getter7 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter7 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration7 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o7 = DOTween.Shake(vibrato: Lua.xlua_tointeger(L, 5), randomness: (float)Lua.lua_tonumber(L, 6), fadeOut: Lua.lua_toboolean(L, 7), getter: getter7, setter: setter7, duration: duration7, strength: val);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				DOGetter<Vector3> getter8 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter8 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration8 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val2);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o8 = DOTween.Shake(vibrato: Lua.xlua_tointeger(L, 5), randomness: (float)Lua.lua_tonumber(L, 6), getter: getter8, setter: setter8, duration: duration8, strength: val2);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				DOGetter<Vector3> getter9 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter9 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration9 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val3);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o9 = DOTween.Shake(vibrato: Lua.xlua_tointeger(L, 5), getter: getter9, setter: setter9, duration: duration9, strength: val3);
				objectTranslator.Push(L, o9);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<DOGetter<Vector3>>(L, 1) && objectTranslator.Assignable<DOSetter<Vector3>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4))
			{
				DOGetter<Vector3> getter10 = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
				DOSetter<Vector3> setter10 = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
				float duration10 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val4);
				TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o10 = DOTween.Shake(getter10, setter10, duration10, val4);
				objectTranslator.Push(L, o10);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Shake!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOGetter<Vector3> getter = objectTranslator.GetDelegate<DOGetter<Vector3>>(L, 1);
			DOSetter<Vector3> setter = objectTranslator.GetDelegate<DOSetter<Vector3>>(L, 2);
			Vector3[] endValues = (Vector3[])objectTranslator.GetObject(L, 3, typeof(Vector3[]));
			float[] durations = (float[])objectTranslator.GetObject(L, 4, typeof(float[]));
			TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> o = DOTween.ToArray(getter, setter, endValues, durations);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sequence_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sequence o = DOTween.Sequence();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompleteAll_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				int value = DOTween.CompleteAll(Lua.lua_toboolean(L, 1));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 0)
			{
				int value2 = DOTween.CompleteAll();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.CompleteAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Complete_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object targetOrId = objectTranslator.GetObject(L, 1, typeof(object));
				bool withCallbacks = Lua.lua_toboolean(L, 2);
				int value = DOTween.Complete(targetOrId, withCallbacks);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value2 = DOTween.Complete(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Complete!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FlipAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.FlipAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Flip_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.Flip(ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(object)));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GotoAll_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				float to = (float)Lua.lua_tonumber(L, 1);
				bool andPlay = Lua.lua_toboolean(L, 2);
				int value = DOTween.GotoAll(to, andPlay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				int value2 = DOTween.GotoAll((float)Lua.lua_tonumber(L, 1));
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.GotoAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Goto_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				object targetOrId = objectTranslator.GetObject(L, 1, typeof(object));
				float to = (float)Lua.lua_tonumber(L, 2);
				bool andPlay = Lua.lua_toboolean(L, 3);
				int value = DOTween.Goto(targetOrId, to, andPlay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				object targetOrId2 = objectTranslator.GetObject(L, 1, typeof(object));
				float to2 = (float)Lua.lua_tonumber(L, 2);
				int value2 = DOTween.Goto(targetOrId2, to2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Goto!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_KillAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				int value = DOTween.KillAll(Lua.lua_toboolean(L, 1));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 0)
			{
				int value2 = DOTween.KillAll();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num >= 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				bool complete = Lua.lua_toboolean(L, 1);
				object[] idsOrTargetsToExclude = objectTranslator.GetParams<object>(L, 2);
				int value3 = DOTween.KillAll(complete, idsOrTargetsToExclude);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.KillAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Kill_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object targetOrId = objectTranslator.GetObject(L, 1, typeof(object));
				bool complete = Lua.lua_toboolean(L, 2);
				int value = DOTween.Kill(targetOrId, complete);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value2 = DOTween.Kill(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Kill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PauseAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.PauseAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.Pause(ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(object)));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.PlayAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value = DOTween.Play(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				object target = objectTranslator.GetObject(L, 1, typeof(object));
				object id = objectTranslator.GetObject(L, 2, typeof(object));
				int value2 = DOTween.Play(target, id);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBackwardsAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.PlayBackwardsAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBackwards_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value = DOTween.PlayBackwards(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				object target = objectTranslator.GetObject(L, 1, typeof(object));
				object id = objectTranslator.GetObject(L, 2, typeof(object));
				int value2 = DOTween.PlayBackwards(target, id);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.PlayBackwards!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayForwardAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.PlayForwardAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayForward_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value = DOTween.PlayForward(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				object target = objectTranslator.GetObject(L, 1, typeof(object));
				object id = objectTranslator.GetObject(L, 2, typeof(object));
				int value2 = DOTween.PlayForward(target, id);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.PlayForward!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RestartAll_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				int value = DOTween.RestartAll(Lua.lua_toboolean(L, 1));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 0)
			{
				int value2 = DOTween.RestartAll();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.RestartAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Restart_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				object targetOrId = objectTranslator.GetObject(L, 1, typeof(object));
				bool includeDelay = Lua.lua_toboolean(L, 2);
				float changeDelayTo = (float)Lua.lua_tonumber(L, 3);
				int value = DOTween.Restart(targetOrId, includeDelay, changeDelayTo);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object targetOrId2 = objectTranslator.GetObject(L, 1, typeof(object));
				bool includeDelay2 = Lua.lua_toboolean(L, 2);
				int value2 = DOTween.Restart(targetOrId2, includeDelay2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value3 = DOTween.Restart(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				object target = objectTranslator.GetObject(L, 1, typeof(object));
				object id = objectTranslator.GetObject(L, 2, typeof(object));
				bool includeDelay3 = Lua.lua_toboolean(L, 3);
				float changeDelayTo2 = (float)Lua.lua_tonumber(L, 4);
				int value4 = DOTween.Restart(target, id, includeDelay3, changeDelayTo2);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				object target2 = objectTranslator.GetObject(L, 1, typeof(object));
				object id2 = objectTranslator.GetObject(L, 2, typeof(object));
				bool includeDelay4 = Lua.lua_toboolean(L, 3);
				int value5 = DOTween.Restart(target2, id2, includeDelay4);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				object target3 = objectTranslator.GetObject(L, 1, typeof(object));
				object id3 = objectTranslator.GetObject(L, 2, typeof(object));
				int value6 = DOTween.Restart(target3, id3);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Restart!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RewindAll_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				int value = DOTween.RewindAll(Lua.lua_toboolean(L, 1));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 0)
			{
				int value2 = DOTween.RewindAll();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.RewindAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rewind_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object targetOrId = objectTranslator.GetObject(L, 1, typeof(object));
				bool includeDelay = Lua.lua_toboolean(L, 2);
				int value = DOTween.Rewind(targetOrId, includeDelay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				int value2 = DOTween.Rewind(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.Rewind!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SmoothRewindAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.SmoothRewindAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SmoothRewind_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.SmoothRewind(ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(object)));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TogglePauseAll_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.TogglePauseAll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TogglePause_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.TogglePause(ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(object)));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTweening_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object targetOrId = objectTranslator.GetObject(L, 1, typeof(object));
				bool alsoCheckIfIsPlaying = Lua.lua_toboolean(L, 2);
				bool value = DOTween.IsTweening(targetOrId, alsoCheckIfIsPlaying);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				bool value2 = DOTween.IsTweening(objectTranslator.GetObject(L, 1, typeof(object)));
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.IsTweening!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TotalPlayingTweens_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DOTween.TotalPlayingTweens();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayingTweens_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<List<Tween>>(L, 1))
			{
				List<Tween> o = DOTween.PlayingTweens((List<Tween>)objectTranslator.GetObject(L, 1, typeof(List<Tween>)));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 0)
			{
				List<Tween> o2 = DOTween.PlayingTweens();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.PlayingTweens!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PausedTweens_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<List<Tween>>(L, 1))
			{
				List<Tween> o = DOTween.PausedTweens((List<Tween>)objectTranslator.GetObject(L, 1, typeof(List<Tween>)));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 0)
			{
				List<Tween> o2 = DOTween.PausedTweens();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.PausedTweens!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TweensById_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Tween>>(L, 3))
			{
				object id = objectTranslator.GetObject(L, 1, typeof(object));
				bool playingOnly = Lua.lua_toboolean(L, 2);
				List<Tween> fillableList = (List<Tween>)objectTranslator.GetObject(L, 3, typeof(List<Tween>));
				List<Tween> o = DOTween.TweensById(id, playingOnly, fillableList);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object id2 = objectTranslator.GetObject(L, 1, typeof(object));
				bool playingOnly2 = Lua.lua_toboolean(L, 2);
				List<Tween> o2 = DOTween.TweensById(id2, playingOnly2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				List<Tween> o3 = DOTween.TweensById(objectTranslator.GetObject(L, 1, typeof(object)));
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.TweensById!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TweensByTarget_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<Tween>>(L, 3))
			{
				object target = objectTranslator.GetObject(L, 1, typeof(object));
				bool playingOnly = Lua.lua_toboolean(L, 2);
				List<Tween> fillableList = (List<Tween>)objectTranslator.GetObject(L, 3, typeof(List<Tween>));
				List<Tween> o = DOTween.TweensByTarget(target, playingOnly, fillableList);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				object target2 = objectTranslator.GetObject(L, 1, typeof(object));
				bool playingOnly2 = Lua.lua_toboolean(L, 2);
				List<Tween> o2 = DOTween.TweensByTarget(target2, playingOnly2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				List<Tween> o3 = DOTween.TweensByTarget(objectTranslator.GetObject(L, 1, typeof(object)));
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTween.TweensByTarget!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_logBehaviour(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.logBehaviour);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_debugStoreTargetId(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.debugStoreTargetId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useSafeMode(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.useSafeMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nestedTweenFailureBehaviour(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.nestedTweenFailureBehaviour);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_showUnityEditorReport(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.showUnityEditorReport);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeScale(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, DOTween.timeScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useSmoothDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.useSmoothDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxSmoothUnscaledTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, DOTween.maxSmoothUnscaledTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onWillLog(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.onWillLog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_drawGizmos(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.drawGizmos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_debugMode(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.debugMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultUpdateType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.defaultUpdateType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultTimeScaleIndependent(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.defaultTimeScaleIndependent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultAutoPlay(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.defaultAutoPlay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultAutoKill(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.defaultAutoKill);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultLoopType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.defaultLoopType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultRecyclable(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DOTween.defaultRecyclable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultEaseType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushDGTweeningEase(L, DOTween.defaultEaseType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultEaseOvershootOrAmplitude(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, DOTween.defaultEaseOvershootOrAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultEasePeriod(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, DOTween.defaultEasePeriod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DOTween.instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_logBehaviour(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out LogBehaviour v);
			DOTween.logBehaviour = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_debugStoreTargetId(IntPtr L)
	{
		try
		{
			DOTween.debugStoreTargetId = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useSafeMode(IntPtr L)
	{
		try
		{
			DOTween.useSafeMode = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nestedTweenFailureBehaviour(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out NestedTweenFailureBehaviour v);
			DOTween.nestedTweenFailureBehaviour = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_showUnityEditorReport(IntPtr L)
	{
		try
		{
			DOTween.showUnityEditorReport = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeScale(IntPtr L)
	{
		try
		{
			DOTween.timeScale = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useSmoothDeltaTime(IntPtr L)
	{
		try
		{
			DOTween.useSmoothDeltaTime = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxSmoothUnscaledTime(IntPtr L)
	{
		try
		{
			DOTween.maxSmoothUnscaledTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onWillLog(IntPtr L)
	{
		try
		{
			DOTween.onWillLog = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Func<LogType, object, bool>>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_drawGizmos(IntPtr L)
	{
		try
		{
			DOTween.drawGizmos = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_debugMode(IntPtr L)
	{
		try
		{
			DOTween.debugMode = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultUpdateType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out UpdateType v);
			DOTween.defaultUpdateType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultTimeScaleIndependent(IntPtr L)
	{
		try
		{
			DOTween.defaultTimeScaleIndependent = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultAutoPlay(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out AutoPlay v);
			DOTween.defaultAutoPlay = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultAutoKill(IntPtr L)
	{
		try
		{
			DOTween.defaultAutoKill = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultLoopType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out LoopType v);
			DOTween.defaultLoopType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultRecyclable(IntPtr L)
	{
		try
		{
			DOTween.defaultRecyclable = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultEaseType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Ease val);
			DOTween.defaultEaseType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultEaseOvershootOrAmplitude(IntPtr L)
	{
		try
		{
			DOTween.defaultEaseOvershootOrAmplitude = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultEasePeriod(IntPtr L)
	{
		try
		{
			DOTween.defaultEasePeriod = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_instance(IntPtr L)
	{
		try
		{
			DOTween.instance = (DOTweenComponent)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(DOTweenComponent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
