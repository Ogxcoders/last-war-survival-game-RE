using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SimpleAnimationEditorStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SimpleAnimation.EditorState);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 3, 3);
		Utils.RegisterFunc(L, -2, "clip", _g_get_clip);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -2, "defaultState", _g_get_defaultState);
		Utils.RegisterFunc(L, -1, "clip", _s_set_clip);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
		Utils.RegisterFunc(L, -1, "defaultState", _s_set_defaultState);
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
				SimpleAnimation.EditorState o = new SimpleAnimation.EditorState();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SimpleAnimation.EditorState constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation.EditorState editorState = (SimpleAnimation.EditorState)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, editorState.clip);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			SimpleAnimation.EditorState editorState = (SimpleAnimation.EditorState)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, editorState.name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultState(IntPtr L)
	{
		try
		{
			SimpleAnimation.EditorState editorState = (SimpleAnimation.EditorState)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, editorState.defaultState);
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
			((SimpleAnimation.EditorState)objectTranslator.FastGetCSObj(L, 1)).clip = (AnimationClip)objectTranslator.GetObject(L, 2, typeof(AnimationClip));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((SimpleAnimation.EditorState)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultState(IntPtr L)
	{
		try
		{
			((SimpleAnimation.EditorState)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).defaultState = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
