using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIShadowWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Shadow);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 3, 3);
		Utils.RegisterFunc(L, -3, "ModifyMesh", _m_ModifyMesh);
		Utils.RegisterFunc(L, -3, "Set_color", _m_Set_color);
		Utils.RegisterFunc(L, -2, "effectColor", _g_get_effectColor);
		Utils.RegisterFunc(L, -2, "effectDistance", _g_get_effectDistance);
		Utils.RegisterFunc(L, -2, "useGraphicAlpha", _g_get_useGraphicAlpha);
		Utils.RegisterFunc(L, -1, "effectColor", _s_set_effectColor);
		Utils.RegisterFunc(L, -1, "effectDistance", _s_set_effectDistance);
		Utils.RegisterFunc(L, -1, "useGraphicAlpha", _s_set_useGraphicAlpha);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Shadow does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModifyMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shadow shadow = (Shadow)objectTranslator.FastGetCSObj(L, 1);
			VertexHelper vh = (VertexHelper)objectTranslator.GetObject(L, 2, typeof(VertexHelper));
			shadow.ModifyMesh(vh);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color(IntPtr L)
	{
		try
		{
			Shadow shadow = (Shadow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			shadow.Set_color(r, g, b, a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_effectColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shadow shadow = (Shadow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, shadow.effectColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_effectDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shadow shadow = (Shadow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, shadow.effectDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useGraphicAlpha(IntPtr L)
	{
		try
		{
			Shadow shadow = (Shadow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, shadow.useGraphicAlpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_effectColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shadow shadow = (Shadow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			shadow.effectColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_effectDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Shadow shadow = (Shadow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			shadow.effectDistance = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useGraphicAlpha(IntPtr L)
	{
		try
		{
			((Shadow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useGraphicAlpha = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
