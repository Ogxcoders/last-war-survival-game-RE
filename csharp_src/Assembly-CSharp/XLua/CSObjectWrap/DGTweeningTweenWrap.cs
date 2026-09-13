using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningTweenWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Tween);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 60, 23, 18);
		Utils.RegisterFunc(L, -3, "Complete", _m_Complete);
		Utils.RegisterFunc(L, -3, "Flip", _m_Flip);
		Utils.RegisterFunc(L, -3, "ForceInit", _m_ForceInit);
		Utils.RegisterFunc(L, -3, "Goto", _m_Goto);
		Utils.RegisterFunc(L, -3, "Kill", _m_Kill);
		Utils.RegisterFunc(L, -3, "Pause", _m_Pause);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "PlayBackwards", _m_PlayBackwards);
		Utils.RegisterFunc(L, -3, "PlayForward", _m_PlayForward);
		Utils.RegisterFunc(L, -3, "Restart", _m_Restart);
		Utils.RegisterFunc(L, -3, "Rewind", _m_Rewind);
		Utils.RegisterFunc(L, -3, "SmoothRewind", _m_SmoothRewind);
		Utils.RegisterFunc(L, -3, "TogglePause", _m_TogglePause);
		Utils.RegisterFunc(L, -3, "GotoWaypoint", _m_GotoWaypoint);
		Utils.RegisterFunc(L, -3, "WaitForCompletion", _m_WaitForCompletion);
		Utils.RegisterFunc(L, -3, "WaitForRewind", _m_WaitForRewind);
		Utils.RegisterFunc(L, -3, "WaitForKill", _m_WaitForKill);
		Utils.RegisterFunc(L, -3, "WaitForElapsedLoops", _m_WaitForElapsedLoops);
		Utils.RegisterFunc(L, -3, "WaitForPosition", _m_WaitForPosition);
		Utils.RegisterFunc(L, -3, "WaitForStart", _m_WaitForStart);
		Utils.RegisterFunc(L, -3, "CompletedLoops", _m_CompletedLoops);
		Utils.RegisterFunc(L, -3, "Delay", _m_Delay);
		Utils.RegisterFunc(L, -3, "ElapsedDelay", _m_ElapsedDelay);
		Utils.RegisterFunc(L, -3, "Duration", _m_Duration);
		Utils.RegisterFunc(L, -3, "Elapsed", _m_Elapsed);
		Utils.RegisterFunc(L, -3, "ElapsedPercentage", _m_ElapsedPercentage);
		Utils.RegisterFunc(L, -3, "ElapsedDirectionalPercentage", _m_ElapsedDirectionalPercentage);
		Utils.RegisterFunc(L, -3, "IsActive", _m_IsActive);
		Utils.RegisterFunc(L, -3, "IsBackwards", _m_IsBackwards);
		Utils.RegisterFunc(L, -3, "IsComplete", _m_IsComplete);
		Utils.RegisterFunc(L, -3, "IsInitialized", _m_IsInitialized);
		Utils.RegisterFunc(L, -3, "IsPlaying", _m_IsPlaying);
		Utils.RegisterFunc(L, -3, "Loops", _m_Loops);
		Utils.RegisterFunc(L, -3, "PathGetPoint", _m_PathGetPoint);
		Utils.RegisterFunc(L, -3, "PathGetDrawPoints", _m_PathGetDrawPoints);
		Utils.RegisterFunc(L, -3, "PathLength", _m_PathLength);
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
		Utils.RegisterFunc(L, -3, "SetDelay", _m_SetDelay);
		Utils.RegisterFunc(L, -3, "SetRelative", _m_SetRelative);
		Utils.RegisterFunc(L, -3, "SetSpeedBased", _m_SetSpeedBased);
		Utils.RegisterFunc(L, -3, "DOTimeScale", _m_DOTimeScale);
		Utils.RegisterFunc(L, -3, "SetSpecialStartupMode", _m_SetSpecialStartupMode);
		Utils.RegisterFunc(L, -3, "ScriptOnComplete", _m_ScriptOnComplete);
		Utils.RegisterFunc(L, -2, "isRelative", _g_get_isRelative);
		Utils.RegisterFunc(L, -2, "active", _g_get_active);
		Utils.RegisterFunc(L, -2, "fullPosition", _g_get_fullPosition);
		Utils.RegisterFunc(L, -2, "hasLoops", _g_get_hasLoops);
		Utils.RegisterFunc(L, -2, "playedOnce", _g_get_playedOnce);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "timeScale", _g_get_timeScale);
		Utils.RegisterFunc(L, -2, "isBackwards", _g_get_isBackwards);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "stringId", _g_get_stringId);
		Utils.RegisterFunc(L, -2, "intId", _g_get_intId);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -2, "onPlay", _g_get_onPlay);
		Utils.RegisterFunc(L, -2, "onPause", _g_get_onPause);
		Utils.RegisterFunc(L, -2, "onRewind", _g_get_onRewind);
		Utils.RegisterFunc(L, -2, "onUpdate", _g_get_onUpdate);
		Utils.RegisterFunc(L, -2, "onStepComplete", _g_get_onStepComplete);
		Utils.RegisterFunc(L, -2, "onComplete", _g_get_onComplete);
		Utils.RegisterFunc(L, -2, "onKill", _g_get_onKill);
		Utils.RegisterFunc(L, -2, "onWaypointChange", _g_get_onWaypointChange);
		Utils.RegisterFunc(L, -2, "easeOvershootOrAmplitude", _g_get_easeOvershootOrAmplitude);
		Utils.RegisterFunc(L, -2, "easePeriod", _g_get_easePeriod);
		Utils.RegisterFunc(L, -2, "debugTargetId", _g_get_debugTargetId);
		Utils.RegisterFunc(L, -1, "fullPosition", _s_set_fullPosition);
		Utils.RegisterFunc(L, -1, "timeScale", _s_set_timeScale);
		Utils.RegisterFunc(L, -1, "isBackwards", _s_set_isBackwards);
		Utils.RegisterFunc(L, -1, "id", _s_set_id);
		Utils.RegisterFunc(L, -1, "stringId", _s_set_stringId);
		Utils.RegisterFunc(L, -1, "intId", _s_set_intId);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
		Utils.RegisterFunc(L, -1, "onPlay", _s_set_onPlay);
		Utils.RegisterFunc(L, -1, "onPause", _s_set_onPause);
		Utils.RegisterFunc(L, -1, "onRewind", _s_set_onRewind);
		Utils.RegisterFunc(L, -1, "onUpdate", _s_set_onUpdate);
		Utils.RegisterFunc(L, -1, "onStepComplete", _s_set_onStepComplete);
		Utils.RegisterFunc(L, -1, "onComplete", _s_set_onComplete);
		Utils.RegisterFunc(L, -1, "onKill", _s_set_onKill);
		Utils.RegisterFunc(L, -1, "onWaypointChange", _s_set_onWaypointChange);
		Utils.RegisterFunc(L, -1, "easeOvershootOrAmplitude", _s_set_easeOvershootOrAmplitude);
		Utils.RegisterFunc(L, -1, "easePeriod", _s_set_easePeriod);
		Utils.RegisterFunc(L, -1, "debugTargetId", _s_set_debugTargetId);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "DG.Tweening.Tween does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Complete(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				t.Complete();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool withCallbacks = Lua.lua_toboolean(L, 2);
					t.Complete(withCallbacks);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Complete!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Flip(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Flip();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceInit(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Goto(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float to = (float)Lua.lua_tonumber(L, 2);
				bool andPlay = Lua.lua_toboolean(L, 3);
				t.Goto(to, andPlay);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float to2 = (float)Lua.lua_tonumber(L, 2);
				t.Goto(to2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Goto!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Kill(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool complete = Lua.lua_toboolean(L, 2);
				t.Kill(complete);
				return 0;
			}
			if (num == 1)
			{
				t.Kill();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Kill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween o = ((Tween)objectTranslator.FastGetCSObj(L, 1)).Pause();
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
			Tween o = ((Tween)objectTranslator.FastGetCSObj(L, 1)).Play();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBackwards(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayBackwards();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayForward(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayForward();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Restart(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				bool includeDelay = Lua.lua_toboolean(L, 2);
				float changeDelayTo = (float)Lua.lua_tonumber(L, 3);
				t.Restart(includeDelay, changeDelayTo);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeDelay2 = Lua.lua_toboolean(L, 2);
				t.Restart(includeDelay2);
				return 0;
			}
			if (num == 1)
			{
				t.Restart();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Restart!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rewind(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeDelay = Lua.lua_toboolean(L, 2);
				t.Rewind(includeDelay);
				return 0;
			}
			if (num == 1)
			{
				t.Rewind();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Rewind!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SmoothRewind(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SmoothRewind();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TogglePause(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TogglePause();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GotoWaypoint(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int waypointIndex = Lua.xlua_tointeger(L, 2);
				bool andPlay = Lua.lua_toboolean(L, 3);
				t.GotoWaypoint(waypointIndex, andPlay);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int waypointIndex2 = Lua.xlua_tointeger(L, 2);
				t.GotoWaypoint(waypointIndex2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.GotoWaypoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitForCompletion(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			YieldInstruction o = ((Tween)objectTranslator.FastGetCSObj(L, 1)).WaitForCompletion();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitForRewind(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			YieldInstruction o = ((Tween)objectTranslator.FastGetCSObj(L, 1)).WaitForRewind();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitForKill(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			YieldInstruction o = ((Tween)objectTranslator.FastGetCSObj(L, 1)).WaitForKill();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitForElapsedLoops(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
			int elapsedLoops = Lua.xlua_tointeger(L, 2);
			YieldInstruction o = t.WaitForElapsedLoops(elapsedLoops);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitForPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
			float position = (float)Lua.lua_tonumber(L, 2);
			YieldInstruction o = t.WaitForPosition(position);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WaitForStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Coroutine o = ((Tween)objectTranslator.FastGetCSObj(L, 1)).WaitForStart();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompletedLoops(IntPtr L)
	{
		try
		{
			int value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CompletedLoops();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Delay(IntPtr L)
	{
		try
		{
			float num = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Delay();
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ElapsedDelay(IntPtr L)
	{
		try
		{
			float num = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ElapsedDelay();
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Duration(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeLoops = Lua.lua_toboolean(L, 2);
				float num2 = t.Duration(includeLoops);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 1)
			{
				float num3 = t.Duration();
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Duration!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Elapsed(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeLoops = Lua.lua_toboolean(L, 2);
				float num2 = t.Elapsed(includeLoops);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 1)
			{
				float num3 = t.Elapsed();
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.Elapsed!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ElapsedPercentage(IntPtr L)
	{
		try
		{
			Tween t = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeLoops = Lua.lua_toboolean(L, 2);
				float num2 = t.ElapsedPercentage(includeLoops);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 1)
			{
				float num3 = t.ElapsedPercentage();
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.ElapsedPercentage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ElapsedDirectionalPercentage(IntPtr L)
	{
		try
		{
			float num = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ElapsedDirectionalPercentage();
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsActive(IntPtr L)
	{
		try
		{
			bool value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsActive();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBackwards(IntPtr L)
	{
		try
		{
			bool value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBackwards();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsComplete(IntPtr L)
	{
		try
		{
			bool value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsComplete();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInitialized(IntPtr L)
	{
		try
		{
			bool value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInitialized();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPlaying(IntPtr L)
	{
		try
		{
			bool value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPlaying();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Loops(IntPtr L)
	{
		try
		{
			int value = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Loops();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PathGetPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
			float pathPercentage = (float)Lua.lua_tonumber(L, 2);
			Vector3 val = t.PathGetPoint(pathPercentage);
			objectTranslator.PushUnityEngineVector3(L, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PathGetDrawPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int subdivisionsXSegment = Lua.xlua_tointeger(L, 2);
				Vector3[] o = t.PathGetDrawPoints(subdivisionsXSegment);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1)
			{
				Vector3[] o2 = t.PathGetDrawPoints();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.PathGetDrawPoints!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PathLength(IntPtr L)
	{
		try
		{
			float num = ((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PathLength();
			Lua.lua_pushnumber(L, num);
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
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tween.SetAutoKill();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool autoKillOnCompletion = Lua.lua_toboolean(L, 2);
					Tween o = tween.SetAutoKill(autoKillOnCompletion);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetAutoKill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetId!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLink(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetLink!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetLoops!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetEase(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetEase!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRecyclable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tween.SetRecyclable();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool recyclable = Lua.lua_toboolean(L, 2);
					Tween o = tween.SetRecyclable(recyclable);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetRecyclable!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStart(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetAs!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDelay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetDelay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRelative(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tween.SetRelative();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isRelative = Lua.lua_toboolean(L, 2);
					Tween o = tween.SetRelative(isRelative);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetRelative!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeedBased(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Tween o2 = tween.SetSpeedBased();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool isSpeedBased = Lua.lua_toboolean(L, 2);
					Tween o = tween.SetSpeedBased(isSpeedBased);
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
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.Tween.SetSpeedBased!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTimeScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween target = (Tween)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOTimeScale(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpecialStartupMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween t = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _g_get_isRelative(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tween.isRelative);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_active(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tween.active);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fullPosition(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tween.fullPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasLoops(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tween.hasLoops);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playedOnce(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tween.playedOnce);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tween.position);
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
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tween.timeScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isBackwards(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tween.isBackwards);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_id(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, tween.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stringId(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, tween.stringId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_intId(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tween.intId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, tween.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPlay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onPlay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onPause);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onRewind(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onRewind);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onUpdate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onStepComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onStepComplete);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onComplete);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onKill(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onKill);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onWaypointChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween tween = (Tween)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tween.onWaypointChange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_easeOvershootOrAmplitude(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tween.easeOvershootOrAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_easePeriod(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tween.easePeriod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_debugTargetId(IntPtr L)
	{
		try
		{
			Tween tween = (Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, tween.debugTargetId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fullPosition(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fullPosition = (float)Lua.lua_tonumber(L, 2);
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
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).timeScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isBackwards(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isBackwards = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_id(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).id = objectTranslator.GetObject(L, 2, typeof(object));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stringId(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stringId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_intId(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).intId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).target = objectTranslator.GetObject(L, 2, typeof(object));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPlay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onPlay = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onPause = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onRewind(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onRewind = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onUpdate = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onStepComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onStepComplete = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onComplete = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onKill(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onKill = objectTranslator.GetDelegate<TweenCallback>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onWaypointChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Tween)objectTranslator.FastGetCSObj(L, 1)).onWaypointChange = objectTranslator.GetDelegate<TweenCallback<int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_easeOvershootOrAmplitude(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).easeOvershootOrAmplitude = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_easePeriod(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).easePeriod = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_debugTargetId(IntPtr L)
	{
		try
		{
			((Tween)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).debugTargetId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
