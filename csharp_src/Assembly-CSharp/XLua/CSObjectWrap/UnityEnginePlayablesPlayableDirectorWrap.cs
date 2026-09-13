using System;
using UnityEngine;
using UnityEngine.Playables;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEnginePlayablesPlayableDirectorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PlayableDirector);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 17, 9, 6);
		Utils.RegisterFunc(L, -3, "DeferredEvaluate", _m_DeferredEvaluate);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "SetGenericBinding", _m_SetGenericBinding);
		Utils.RegisterFunc(L, -3, "Evaluate", _m_Evaluate);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "Pause", _m_Pause);
		Utils.RegisterFunc(L, -3, "Resume", _m_Resume);
		Utils.RegisterFunc(L, -3, "RebuildGraph", _m_RebuildGraph);
		Utils.RegisterFunc(L, -3, "ClearReferenceValue", _m_ClearReferenceValue);
		Utils.RegisterFunc(L, -3, "SetReferenceValue", _m_SetReferenceValue);
		Utils.RegisterFunc(L, -3, "GetReferenceValue", _m_GetReferenceValue);
		Utils.RegisterFunc(L, -3, "GetGenericBinding", _m_GetGenericBinding);
		Utils.RegisterFunc(L, -3, "ClearGenericBinding", _m_ClearGenericBinding);
		Utils.RegisterFunc(L, -3, "RebindPlayableGraphOutputs", _m_RebindPlayableGraphOutputs);
		Utils.RegisterFunc(L, -3, "played", _e_played);
		Utils.RegisterFunc(L, -3, "paused", _e_paused);
		Utils.RegisterFunc(L, -3, "stopped", _e_stopped);
		Utils.RegisterFunc(L, -2, "state", _g_get_state);
		Utils.RegisterFunc(L, -2, "extrapolationMode", _g_get_extrapolationMode);
		Utils.RegisterFunc(L, -2, "playableAsset", _g_get_playableAsset);
		Utils.RegisterFunc(L, -2, "playableGraph", _g_get_playableGraph);
		Utils.RegisterFunc(L, -2, "playOnAwake", _g_get_playOnAwake);
		Utils.RegisterFunc(L, -2, "timeUpdateMode", _g_get_timeUpdateMode);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "initialTime", _g_get_initialTime);
		Utils.RegisterFunc(L, -2, "duration", _g_get_duration);
		Utils.RegisterFunc(L, -1, "extrapolationMode", _s_set_extrapolationMode);
		Utils.RegisterFunc(L, -1, "playableAsset", _s_set_playableAsset);
		Utils.RegisterFunc(L, -1, "playOnAwake", _s_set_playOnAwake);
		Utils.RegisterFunc(L, -1, "timeUpdateMode", _s_set_timeUpdateMode);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "initialTime", _s_set_initialTime);
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
				PlayableDirector o = new PlayableDirector();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.PlayableDirector constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeferredEvaluate(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DeferredEvaluate();
			return 0;
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
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
				playableDirector.Play();
				return 0;
			case 2:
				if (objectTranslator.Assignable<PlayableAsset>(L, 2))
				{
					PlayableAsset asset = (PlayableAsset)objectTranslator.GetObject(L, 2, typeof(PlayableAsset));
					playableDirector.Play(asset);
					return 0;
				}
				break;
			}
			if (num == 3 && objectTranslator.Assignable<PlayableAsset>(L, 2) && objectTranslator.Assignable<DirectorWrapMode>(L, 3))
			{
				PlayableAsset asset2 = (PlayableAsset)objectTranslator.GetObject(L, 2, typeof(PlayableAsset));
				objectTranslator.Get(L, 3, out DirectorWrapMode v);
				playableDirector.Play(asset2, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.PlayableDirector.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGenericBinding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			UnityEngine.Object key = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
			UnityEngine.Object value = (UnityEngine.Object)objectTranslator.GetObject(L, 3, typeof(UnityEngine.Object));
			playableDirector.SetGenericBinding(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Evaluate(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Evaluate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Stop(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Stop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Pause();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Resume(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Resume();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RebuildGraph(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RebuildGraph();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearReferenceValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out PropertyName v);
			playableDirector.ClearReferenceValue(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetReferenceValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out PropertyName v);
			UnityEngine.Object value = (UnityEngine.Object)objectTranslator.GetObject(L, 3, typeof(UnityEngine.Object));
			playableDirector.SetReferenceValue(v, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetReferenceValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out PropertyName v);
			bool idValid;
			UnityEngine.Object referenceValue = playableDirector.GetReferenceValue(v, out idValid);
			objectTranslator.Push(L, referenceValue);
			Lua.lua_pushboolean(L, idValid);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGenericBinding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			UnityEngine.Object key = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
			UnityEngine.Object genericBinding = playableDirector.GetGenericBinding(key);
			objectTranslator.Push(L, genericBinding);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearGenericBinding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			UnityEngine.Object key = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
			playableDirector.ClearGenericBinding(key);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RebindPlayableGraphOutputs(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RebindPlayableGraphOutputs();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_state(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, playableDirector.state);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extrapolationMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, playableDirector.extrapolationMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playableAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, playableDirector.playableAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playableGraph(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, playableDirector.playableGraph);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playOnAwake(IntPtr L)
	{
		try
		{
			PlayableDirector playableDirector = (PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, playableDirector.playOnAwake);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeUpdateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, playableDirector.timeUpdateMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			PlayableDirector playableDirector = (PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playableDirector.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_initialTime(IntPtr L)
	{
		try
		{
			PlayableDirector playableDirector = (PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playableDirector.initialTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_duration(IntPtr L)
	{
		try
		{
			PlayableDirector playableDirector = (PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playableDirector.duration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extrapolationMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DirectorWrapMode v);
			playableDirector.extrapolationMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playableAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PlayableDirector)objectTranslator.FastGetCSObj(L, 1)).playableAsset = (PlayableAsset)objectTranslator.GetObject(L, 2, typeof(PlayableAsset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playOnAwake(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playOnAwake = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeUpdateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DirectorUpdateMode v);
			playableDirector.timeUpdateMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_time(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).time = Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_initialTime(IntPtr L)
	{
		try
		{
			((PlayableDirector)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).initialTime = Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_played(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			Action<PlayableDirector> action = objectTranslator.GetDelegate<Action<PlayableDirector>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.Playables.PlayableDirector>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					playableDirector.played += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					playableDirector.played -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.PlayableDirector.played!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_paused(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			Action<PlayableDirector> action = objectTranslator.GetDelegate<Action<PlayableDirector>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.Playables.PlayableDirector>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					playableDirector.paused += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					playableDirector.paused -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.PlayableDirector.paused!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_stopped(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			PlayableDirector playableDirector = (PlayableDirector)objectTranslator.FastGetCSObj(L, 1);
			Action<PlayableDirector> action = objectTranslator.GetDelegate<Action<PlayableDirector>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<UnityEngine.Playables.PlayableDirector>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					playableDirector.stopped += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					playableDirector.stopped -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.PlayableDirector.stopped!");
		return 0;
	}
}
