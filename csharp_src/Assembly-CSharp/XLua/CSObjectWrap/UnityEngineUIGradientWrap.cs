using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIGradientWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityEngine.UI.Gradient);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 4, 4);
		Utils.RegisterFunc(L, -3, "ModifyMesh", _m_ModifyMesh);
		Utils.RegisterFunc(L, -2, "BlendMode", _g_get_BlendMode);
		Utils.RegisterFunc(L, -2, "EffectGradient", _g_get_EffectGradient);
		Utils.RegisterFunc(L, -2, "GradientType", _g_get_GradientType);
		Utils.RegisterFunc(L, -2, "Offset", _g_get_Offset);
		Utils.RegisterFunc(L, -1, "BlendMode", _s_set_BlendMode);
		Utils.RegisterFunc(L, -1, "EffectGradient", _s_set_EffectGradient);
		Utils.RegisterFunc(L, -1, "GradientType", _s_set_GradientType);
		Utils.RegisterFunc(L, -1, "Offset", _s_set_Offset);
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
				UnityEngine.UI.Gradient o = new UnityEngine.UI.Gradient();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Gradient constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModifyMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1);
			VertexHelper vh = (VertexHelper)objectTranslator.GetObject(L, 2, typeof(VertexHelper));
			gradient.ModifyMesh(vh);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BlendMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gradient.BlendMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EffectGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gradient.EffectGradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GradientType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gradient.GradientType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Offset(IntPtr L)
	{
		try
		{
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, gradient.Offset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BlendMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Blend v);
			gradient.BlendMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EffectGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1)).EffectGradient = (UnityEngine.Gradient)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Gradient));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GradientType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.UI.Gradient gradient = (UnityEngine.UI.Gradient)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GradientType v);
			gradient.GradientType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Offset(IntPtr L)
	{
		try
		{
			((UnityEngine.UI.Gradient)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Offset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
