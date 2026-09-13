using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SimpleAnimationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SimpleAnimation);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 35, 8, 6);
		Utils.RegisterFunc(L, -3, "GetCurrentState", _m_GetCurrentState);
		Utils.RegisterFunc(L, -3, "GetLastState", _m_GetLastState);
		Utils.RegisterFunc(L, -3, "GetInterruptingState", _m_GetInterruptingState);
		Utils.RegisterFunc(L, -3, "IsCrossFading", _m_IsCrossFading);
		Utils.RegisterFunc(L, -3, "InterruptingCrossFading", _m_InterruptingCrossFading);
		Utils.RegisterFunc(L, -3, "AddClip", _m_AddClip);
		Utils.RegisterFunc(L, -3, "Blend", _m_Blend);
		Utils.RegisterFunc(L, -3, "CrossFade", _m_CrossFade);
		Utils.RegisterFunc(L, -3, "CrossFadeQueued", _m_CrossFadeQueued);
		Utils.RegisterFunc(L, -3, "GetClipCount", _m_GetClipCount);
		Utils.RegisterFunc(L, -3, "IsPlaying", _m_IsPlaying);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "Sample", _m_Sample);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "AddState", _m_AddState);
		Utils.RegisterFunc(L, -3, "RemoveState", _m_RemoveState);
		Utils.RegisterFunc(L, -3, "SetStateSpeed", _m_SetStateSpeed);
		Utils.RegisterFunc(L, -3, "PlayQueued", _m_PlayQueued);
		Utils.RegisterFunc(L, -3, "RemoveClip", _m_RemoveClip);
		Utils.RegisterFunc(L, -3, "RewindAndPlay", _m_RewindAndPlay);
		Utils.RegisterFunc(L, -3, "Rewind", _m_Rewind);
		Utils.RegisterFunc(L, -3, "ForceCleanAllQueuedStates", _m_ForceCleanAllQueuedStates);
		Utils.RegisterFunc(L, -3, "GetState", _m_GetState);
		Utils.RegisterFunc(L, -3, "GetStates", _m_GetStates);
		Utils.RegisterFunc(L, -3, "get_Item", _m_get_Item);
		Utils.RegisterFunc(L, -3, "GetClipLength", _m_GetClipLength);
		Utils.RegisterFunc(L, -3, "GetClipTime", _m_GetClipTime);
		Utils.RegisterFunc(L, -3, "SampleAnimationAtTime", _m_SampleAnimationAtTime);
		Utils.RegisterFunc(L, -3, "SampleAnimationAtTimeInEditor", _m_SampleAnimationAtTimeInEditor);
		Utils.RegisterFunc(L, -3, "GetEditorStates", _m_GetEditorStates);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "GetAnimationClips", _m_GetAnimationClips);
		Utils.RegisterFunc(L, -3, "GetEditorState", _m_GetEditorState);
		Utils.RegisterFunc(L, -3, "PlayId", _m_PlayId);
		Utils.RegisterFunc(L, -3, "PlayQueuedId", _m_PlayQueuedId);
		Utils.RegisterFunc(L, -2, "animator", _g_get_animator);
		Utils.RegisterFunc(L, -2, "animatePhysics", _g_get_animatePhysics);
		Utils.RegisterFunc(L, -2, "cullingMode", _g_get_cullingMode);
		Utils.RegisterFunc(L, -2, "isPlaying", _g_get_isPlaying);
		Utils.RegisterFunc(L, -2, "playAutomatically", _g_get_playAutomatically);
		Utils.RegisterFunc(L, -2, "clip", _g_get_clip);
		Utils.RegisterFunc(L, -2, "wrapMode", _g_get_wrapMode);
		Utils.RegisterFunc(L, -2, "UpdateManual", _g_get_UpdateManual);
		Utils.RegisterFunc(L, -1, "animatePhysics", _s_set_animatePhysics);
		Utils.RegisterFunc(L, -1, "cullingMode", _s_set_cullingMode);
		Utils.RegisterFunc(L, -1, "playAutomatically", _s_set_playAutomatically);
		Utils.RegisterFunc(L, -1, "clip", _s_set_clip);
		Utils.RegisterFunc(L, -1, "wrapMode", _s_set_wrapMode);
		Utils.RegisterFunc(L, -1, "UpdateManual", _s_set_UpdateManual);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 2, 2);
		Utils.RegisterFunc(L, -2, "s_DirtySimpleAnims", _g_get_s_DirtySimpleAnims);
		Utils.RegisterFunc(L, -2, "s_DirtySimpleCrossAnims", _g_get_s_DirtySimpleCrossAnims);
		Utils.RegisterFunc(L, -1, "s_DirtySimpleAnims", _s_set_s_DirtySimpleAnims);
		Utils.RegisterFunc(L, -1, "s_DirtySimpleCrossAnims", _s_set_s_DirtySimpleCrossAnims);
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
				SimpleAnimation o = new SimpleAnimation();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			(bool, string, float, float) currentState = ((SimpleAnimation)objectTranslator.FastGetCSObj(L, 1)).GetCurrentState();
			objectTranslator.Push(L, currentState);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLastState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			(bool, string, float, float) lastState = ((SimpleAnimation)objectTranslator.FastGetCSObj(L, 1)).GetLastState();
			objectTranslator.Push(L, lastState);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInterruptingState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			(bool, string, float, float, string, float) interruptingState = ((SimpleAnimation)objectTranslator.FastGetCSObj(L, 1)).GetInterruptingState();
			objectTranslator.Push(L, interruptingState);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCrossFading(IntPtr L)
	{
		try
		{
			bool value = ((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCrossFading();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InterruptingCrossFading(IntPtr L)
	{
		try
		{
			bool value = ((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InterruptingCrossFading();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddClip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			AnimationClip clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
			string newName = Lua.lua_tostring(L, 3);
			simpleAnimation.AddClip(clip, newName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Blend(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			float targetWeight = (float)Lua.lua_tonumber(L, 3);
			float fadeLength = (float)Lua.lua_tonumber(L, 4);
			obj.Blend(stateName, targetWeight, fadeLength);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFade(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			float fadeLength = (float)Lua.lua_tonumber(L, 3);
			obj.CrossFade(stateName, fadeLength);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeQueued(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			float fadeLength = (float)Lua.lua_tonumber(L, 3);
			objectTranslator.Get(L, 4, out QueueMode v);
			simpleAnimation.CrossFadeQueued(stateName, fadeLength, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClipCount(IntPtr L)
	{
		try
		{
			int clipCount = ((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetClipCount();
			Lua.xlua_pushinteger(L, clipCount);
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
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			bool value = obj.IsPlaying(stateName);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				simpleAnimation.Stop();
				return 0;
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string stateName = Lua.lua_tostring(L, 2);
					simpleAnimation.Stop(stateName);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.Stop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sample(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				simpleAnimation.Sample();
				return 0;
			case 2:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
				{
					float time = (float)Lua.lua_tonumber(L, 2);
					simpleAnimation.Sample(time);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.Sample!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				bool value2 = simpleAnimation.Play();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string stateName = Lua.lua_tostring(L, 2);
					bool value = simpleAnimation.Play(stateName);
					Lua.lua_pushboolean(L, value);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			AnimationClip clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
			string name = Lua.lua_tostring(L, 3);
			simpleAnimation.AddState(clip, name);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveState(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			obj.RemoveState(name);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStateSpeed(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string aniName = Lua.lua_tostring(L, 2);
			float speed = (float)Lua.lua_tonumber(L, 3);
			obj.SetStateSpeed(aniName, speed);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayQueued(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<QueueMode>(L, 3))
			{
				string stateName = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out QueueMode v);
				simpleAnimation.PlayQueued(stateName, v);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string stateName2 = Lua.lua_tostring(L, 2);
				simpleAnimation.PlayQueued(stateName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.PlayQueued!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveClip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			AnimationClip clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
			simpleAnimation.RemoveClip(clip);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RewindAndPlay(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			obj.RewindAndPlay(stateName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rewind(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				simpleAnimation.Rewind();
				return 0;
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string stateName = Lua.lua_tostring(L, 2);
					simpleAnimation.Rewind(stateName);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.Rewind!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceCleanAllQueuedStates(IntPtr L)
	{
		try
		{
			((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ForceCleanAllQueuedStates();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation obj = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			SimpleAnimation.State state = obj.GetState(stateName);
			objectTranslator.PushAny(L, state);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStates(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IEnumerable<SimpleAnimation.State> states = ((SimpleAnimation)objectTranslator.FastGetCSObj(L, 1)).GetStates();
			objectTranslator.PushAny(L, states);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_get_Item(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			objectTranslator.PushAny(L, simpleAnimation[name]);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClipLength(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			float clipLength = obj.GetClipLength(stateName);
			Lua.lua_pushnumber(L, clipLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClipTime(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			float clipTime = obj.GetClipTime(stateName);
			Lua.lua_pushnumber(L, clipTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SampleAnimationAtTime(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string stateName = Lua.lua_tostring(L, 2);
			float normalizedTime = (float)Lua.lua_tonumber(L, 3);
			bool value = obj.SampleAnimationAtTime(stateName, normalizedTime);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SampleAnimationAtTimeInEditor(IntPtr L)
	{
		try
		{
			SimpleAnimation obj = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			float time = (float)Lua.lua_tonumber(L, 3);
			obj.SampleAnimationAtTimeInEditor(name, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEditorStates(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				SimpleAnimation.EditorState[] editorStates2 = simpleAnimation.GetEditorStates();
				objectTranslator.Push(L, editorStates2);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string name = Lua.lua_tostring(L, 2);
					SimpleAnimation.EditorState editorStates = simpleAnimation.GetEditorStates(name);
					objectTranslator.Push(L, editorStates);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.GetEditorStates!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Initialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAnimationClips(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			List<AnimationClip> results = (List<AnimationClip>)objectTranslator.GetObject(L, 2, typeof(List<AnimationClip>));
			simpleAnimation.GetAnimationClips(results);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEditorState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				SimpleAnimation.EditorState editorState = simpleAnimation.GetEditorState(name);
				objectTranslator.Push(L, editorState);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<AnimationClip>(L, 2))
			{
				AnimationClip clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
				SimpleAnimation.EditorState editorState2 = simpleAnimation.GetEditorState(clip);
				objectTranslator.Push(L, editorState2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.GetEditorState!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayId(IntPtr L)
	{
		try
		{
			SimpleAnimation ani = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int stateNameToId = Lua.xlua_tointeger(L, 2);
			bool value = ani.PlayId(stateNameToId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayQueuedId(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAni = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int stateNameToId = Lua.xlua_tointeger(L, 2);
			simpleAni.PlayQueuedId(stateNameToId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, simpleAnimation.animator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animatePhysics(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, simpleAnimation.animatePhysics);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cullingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineAnimatorCullingMode(L, simpleAnimation.cullingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPlaying(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, simpleAnimation.isPlaying);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playAutomatically(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, simpleAnimation.playAutomatically);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, simpleAnimation.clip);
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
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, simpleAnimation.wrapMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UpdateManual(IntPtr L)
	{
		try
		{
			SimpleAnimation simpleAnimation = (SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, simpleAnimation.UpdateManual);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_s_DirtySimpleAnims(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SimpleAnimation.s_DirtySimpleAnims);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_s_DirtySimpleCrossAnims(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SimpleAnimation.s_DirtySimpleCrossAnims);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animatePhysics(IntPtr L)
	{
		try
		{
			((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).animatePhysics = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cullingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AnimatorCullingMode val);
			simpleAnimation.cullingMode = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playAutomatically(IntPtr L)
	{
		try
		{
			((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playAutomatically = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SimpleAnimation)objectTranslator.FastGetCSObj(L, 1)).clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
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
			SimpleAnimation simpleAnimation = (SimpleAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WrapMode v);
			simpleAnimation.wrapMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UpdateManual(IntPtr L)
	{
		try
		{
			((SimpleAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateManual = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_s_DirtySimpleAnims(IntPtr L)
	{
		try
		{
			SimpleAnimation.s_DirtySimpleAnims = (HashSet<SimpleAnimation>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(HashSet<SimpleAnimation>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_s_DirtySimpleCrossAnims(IntPtr L)
	{
		try
		{
			SimpleAnimation.s_DirtySimpleCrossAnims = (HashSet<SimpleAnimation>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(HashSet<SimpleAnimation>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
