using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GPUSkinningAnimatorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GPUSkinningAnimator);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 9, 3);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "PlayQueued", _m_PlayQueued);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "Resume", _m_Resume);
		Utils.RegisterFunc(L, -3, "GetClipLength", _m_GetClipLength);
		Utils.RegisterFunc(L, -3, "HasClip", _m_HasClip);
		Utils.RegisterFunc(L, -2, "Visible", _g_get_Visible);
		Utils.RegisterFunc(L, -2, "IsPlaying", _g_get_IsPlaying);
		Utils.RegisterFunc(L, -2, "PlayingClipName", _g_get_PlayingClipName);
		Utils.RegisterFunc(L, -2, "Position", _g_get_Position);
		Utils.RegisterFunc(L, -2, "LocalPosition", _g_get_LocalPosition);
		Utils.RegisterFunc(L, -2, "WrapMode", _g_get_WrapMode);
		Utils.RegisterFunc(L, -2, "IsTimeAtTheEndOfLoop", _g_get_IsTimeAtTheEndOfLoop);
		Utils.RegisterFunc(L, -2, "NormalizedTime", _g_get_NormalizedTime);
		Utils.RegisterFunc(L, -2, "PlayEndCallBack", _g_get_PlayEndCallBack);
		Utils.RegisterFunc(L, -1, "Visible", _s_set_Visible);
		Utils.RegisterFunc(L, -1, "NormalizedTime", _s_set_NormalizedTime);
		Utils.RegisterFunc(L, -1, "PlayEndCallBack", _s_set_PlayEndCallBack);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "PropertyID_TextureSize", GPUSkinningAnimator.PropertyID_TextureSize);
		Utils.RegisterObject(L, translator, -4, "PropertyID_ClipParams", GPUSkinningAnimator.PropertyID_ClipParams);
		Utils.RegisterObject(L, translator, -4, "PropertyID_Matrix", GPUSkinningAnimator.PropertyID_Matrix);
		Utils.RegisterObject(L, translator, -4, "PropertyID_GPUSkin", GPUSkinningAnimator.PropertyID_GPUSkin);
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
				GPUSkinningAnimator o = new GPUSkinningAnimator();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GPUSkinningAnimator constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
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
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string clipName = Lua.lua_tostring(L, 2);
				float normalizedTime = (float)Lua.lua_tonumber(L, 3);
				gPUSkinningAnimator.Play(clipName, normalizedTime);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string clipName2 = Lua.lua_tostring(L, 2);
				gPUSkinningAnimator.Play(clipName2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GPUSkinningAnimator.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayQueued(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator obj = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string clipName = Lua.lua_tostring(L, 2);
			obj.PlayQueued(clipName);
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
			((GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Stop();
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
			((GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Resume();
			return 0;
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
			GPUSkinningAnimator obj = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			float clipLength = obj.GetClipLength(name);
			Lua.lua_pushnumber(L, clipLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasClip(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator obj = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			bool value = obj.HasClip(name);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Visible(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gPUSkinningAnimator.Visible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsPlaying(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gPUSkinningAnimator.IsPlaying);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PlayingClipName(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gPUSkinningAnimator.PlayingClipName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, gPUSkinningAnimator.Position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LocalPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, gPUSkinningAnimator.LocalPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WrapMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinningAnimator.WrapMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsTimeAtTheEndOfLoop(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gPUSkinningAnimator.IsTimeAtTheEndOfLoop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NormalizedTime(IntPtr L)
	{
		try
		{
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, gPUSkinningAnimator.NormalizedTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PlayEndCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimator gPUSkinningAnimator = (GPUSkinningAnimator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinningAnimator.PlayEndCallBack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Visible(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Visible = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_NormalizedTime(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NormalizedTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PlayEndCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinningAnimator)objectTranslator.FastGetCSObj(L, 1)).PlayEndCallBack = objectTranslator.GetDelegate<Action<string>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
