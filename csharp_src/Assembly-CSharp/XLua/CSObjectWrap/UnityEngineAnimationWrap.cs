using System;
using System.Collections;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineAnimationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Animation);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 16, 7, 6);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "Rewind", _m_Rewind);
		Utils.RegisterFunc(L, -3, "Sample", _m_Sample);
		Utils.RegisterFunc(L, -3, "IsPlaying", _m_IsPlaying);
		Utils.RegisterFunc(L, -3, "get_Item", _m_get_Item);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "CrossFade", _m_CrossFade);
		Utils.RegisterFunc(L, -3, "Blend", _m_Blend);
		Utils.RegisterFunc(L, -3, "CrossFadeQueued", _m_CrossFadeQueued);
		Utils.RegisterFunc(L, -3, "PlayQueued", _m_PlayQueued);
		Utils.RegisterFunc(L, -3, "AddClip", _m_AddClip);
		Utils.RegisterFunc(L, -3, "RemoveClip", _m_RemoveClip);
		Utils.RegisterFunc(L, -3, "GetClipCount", _m_GetClipCount);
		Utils.RegisterFunc(L, -3, "SyncLayer", _m_SyncLayer);
		Utils.RegisterFunc(L, -3, "GetEnumerator", _m_GetEnumerator);
		Utils.RegisterFunc(L, -3, "GetClip", _m_GetClip);
		Utils.RegisterFunc(L, -2, "clip", _g_get_clip);
		Utils.RegisterFunc(L, -2, "playAutomatically", _g_get_playAutomatically);
		Utils.RegisterFunc(L, -2, "wrapMode", _g_get_wrapMode);
		Utils.RegisterFunc(L, -2, "isPlaying", _g_get_isPlaying);
		Utils.RegisterFunc(L, -2, "animatePhysics", _g_get_animatePhysics);
		Utils.RegisterFunc(L, -2, "cullingType", _g_get_cullingType);
		Utils.RegisterFunc(L, -2, "localBounds", _g_get_localBounds);
		Utils.RegisterFunc(L, -1, "clip", _s_set_clip);
		Utils.RegisterFunc(L, -1, "playAutomatically", _s_set_playAutomatically);
		Utils.RegisterFunc(L, -1, "wrapMode", _s_set_wrapMode);
		Utils.RegisterFunc(L, -1, "animatePhysics", _s_set_animatePhysics);
		Utils.RegisterFunc(L, -1, "cullingType", _s_set_cullingType);
		Utils.RegisterFunc(L, -1, "localBounds", _s_set_localBounds);
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
				Animation o = new Animation();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Stop(IntPtr L)
	{
		try
		{
			Animation animation = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				animation.Stop();
				return 0;
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string name = Lua.lua_tostring(L, 2);
					animation.Stop(name);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.Stop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rewind(IntPtr L)
	{
		try
		{
			Animation animation = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				animation.Rewind();
				return 0;
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string name = Lua.lua_tostring(L, 2);
					animation.Rewind(name);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.Rewind!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sample(IntPtr L)
	{
		try
		{
			((Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Sample();
			return 0;
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
			Animation obj = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			bool value = obj.IsPlaying(name);
			Lua.lua_pushboolean(L, value);
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
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			objectTranslator.Push(L, animation[name]);
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
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
			{
				bool value2 = animation.Play();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			case 2:
				if (objectTranslator.Assignable<PlayMode>(L, 2))
				{
					objectTranslator.Get(L, 2, out PlayMode v);
					bool value = animation.Play(v);
					Lua.lua_pushboolean(L, value);
					return 1;
				}
				break;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animation2 = Lua.lua_tostring(L, 2);
				bool value3 = animation.Play(animation2);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<PlayMode>(L, 3))
			{
				string animation3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out PlayMode v2);
				bool value4 = animation.Play(animation3, v2);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFade(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animation2 = Lua.lua_tostring(L, 2);
				animation.CrossFade(animation2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animation3 = Lua.lua_tostring(L, 2);
				float fadeLength = (float)Lua.lua_tonumber(L, 3);
				animation.CrossFade(animation3, fadeLength);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PlayMode>(L, 4))
			{
				string animation4 = Lua.lua_tostring(L, 2);
				float fadeLength2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PlayMode v);
				animation.CrossFade(animation4, fadeLength2, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.CrossFade!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Blend(IntPtr L)
	{
		try
		{
			Animation animation = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animation2 = Lua.lua_tostring(L, 2);
				animation.Blend(animation2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animation3 = Lua.lua_tostring(L, 2);
				float targetWeight = (float)Lua.lua_tonumber(L, 3);
				animation.Blend(animation3, targetWeight);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string animation4 = Lua.lua_tostring(L, 2);
				float targetWeight2 = (float)Lua.lua_tonumber(L, 3);
				float fadeLength = (float)Lua.lua_tonumber(L, 4);
				animation.Blend(animation4, targetWeight2, fadeLength);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.Blend!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeQueued(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animation2 = Lua.lua_tostring(L, 2);
				AnimationState o = animation.CrossFadeQueued(animation2);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string animation3 = Lua.lua_tostring(L, 2);
				float fadeLength = (float)Lua.lua_tonumber(L, 3);
				AnimationState o2 = animation.CrossFadeQueued(animation3, fadeLength);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueueMode>(L, 4))
			{
				string animation4 = Lua.lua_tostring(L, 2);
				float fadeLength2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out QueueMode v);
				AnimationState o3 = animation.CrossFadeQueued(animation4, fadeLength2, v);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueueMode>(L, 4) && objectTranslator.Assignable<PlayMode>(L, 5))
			{
				string animation5 = Lua.lua_tostring(L, 2);
				float fadeLength3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out QueueMode v2);
				objectTranslator.Get(L, 5, out PlayMode v3);
				AnimationState o4 = animation.CrossFadeQueued(animation5, fadeLength3, v2, v3);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.CrossFadeQueued!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayQueued(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string animation2 = Lua.lua_tostring(L, 2);
				AnimationState o = animation.PlayQueued(animation2);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<QueueMode>(L, 3))
			{
				string animation3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out QueueMode v);
				AnimationState o2 = animation.PlayQueued(animation3, v);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<QueueMode>(L, 3) && objectTranslator.Assignable<PlayMode>(L, 4))
			{
				string animation4 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out QueueMode v2);
				objectTranslator.Get(L, 4, out PlayMode v3);
				AnimationState o3 = animation.PlayQueued(animation4, v2, v3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.PlayQueued!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddClip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<AnimationClip>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				AnimationClip clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
				string newName = Lua.lua_tostring(L, 3);
				animation.AddClip(clip, newName);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<AnimationClip>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				AnimationClip clip2 = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
				string newName2 = Lua.lua_tostring(L, 3);
				int firstFrame = Lua.xlua_tointeger(L, 4);
				int lastFrame = Lua.xlua_tointeger(L, 5);
				animation.AddClip(clip2, newName2, firstFrame, lastFrame);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<AnimationClip>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				AnimationClip clip3 = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
				string newName3 = Lua.lua_tostring(L, 3);
				int firstFrame2 = Lua.xlua_tointeger(L, 4);
				int lastFrame2 = Lua.xlua_tointeger(L, 5);
				bool addLoopFrame = Lua.lua_toboolean(L, 6);
				animation.AddClip(clip3, newName3, firstFrame2, lastFrame2, addLoopFrame);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.AddClip!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveClip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<AnimationClip>(L, 2))
			{
				AnimationClip clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
				animation.RemoveClip(clip);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string clipName = Lua.lua_tostring(L, 2);
				animation.RemoveClip(clipName);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animation.RemoveClip!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClipCount(IntPtr L)
	{
		try
		{
			int clipCount = ((Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetClipCount();
			Lua.xlua_pushinteger(L, clipCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncLayer(IntPtr L)
	{
		try
		{
			Animation obj = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layer = Lua.xlua_tointeger(L, 2);
			obj.SyncLayer(layer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEnumerator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IEnumerator enumerator = ((Animation)objectTranslator.FastGetCSObj(L, 1)).GetEnumerator();
			objectTranslator.PushAny(L, enumerator);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation obj = (Animation)objectTranslator.FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			AnimationClip clip = obj.GetClip(name);
			objectTranslator.Push(L, clip);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animation.clip);
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
			Animation animation = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animation.playAutomatically);
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
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animation.wrapMode);
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
			Animation animation = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animation.isPlaying);
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
			Animation animation = (Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animation.animatePhysics);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cullingType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animation.cullingType);
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
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, animation.localBounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Animation)objectTranslator.FastGetCSObj(L, 1)).clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
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
			((Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playAutomatically = Lua.lua_toboolean(L, 2);
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
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WrapMode v);
			animation.wrapMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animatePhysics(IntPtr L)
	{
		try
		{
			((Animation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).animatePhysics = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cullingType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AnimationCullingType v);
			animation.cullingType = v;
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
			Animation animation = (Animation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Bounds val);
			animation.localBounds = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
