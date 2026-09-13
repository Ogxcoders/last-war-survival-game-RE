using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TargetFlowWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TargetFlow);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 10, 10);
		Utils.RegisterFunc(L, -3, "DoFlowAnim", _m_DoFlowAnim);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -2, "drawPoint", _g_get_drawPoint);
		Utils.RegisterFunc(L, -2, "render", _g_get_render);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -2, "zOffset", _g_get_zOffset);
		Utils.RegisterFunc(L, -2, "xOffset", _g_get_xOffset);
		Utils.RegisterFunc(L, -2, "lineColor", _g_get_lineColor);
		Utils.RegisterFunc(L, -2, "maxYoffset", _g_get_maxYoffset);
		Utils.RegisterFunc(L, -2, "minYoffset", _g_get_minYoffset);
		Utils.RegisterFunc(L, -2, "_a", _g_get__a);
		Utils.RegisterFunc(L, -2, "_b", _g_get__b);
		Utils.RegisterFunc(L, -1, "drawPoint", _s_set_drawPoint);
		Utils.RegisterFunc(L, -1, "render", _s_set_render);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
		Utils.RegisterFunc(L, -1, "zOffset", _s_set_zOffset);
		Utils.RegisterFunc(L, -1, "xOffset", _s_set_xOffset);
		Utils.RegisterFunc(L, -1, "lineColor", _s_set_lineColor);
		Utils.RegisterFunc(L, -1, "maxYoffset", _s_set_maxYoffset);
		Utils.RegisterFunc(L, -1, "minYoffset", _s_set_minYoffset);
		Utils.RegisterFunc(L, -1, "_a", _s_set__a);
		Utils.RegisterFunc(L, -1, "_b", _s_set__b);
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
				TargetFlow o = new TargetFlow();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TargetFlow constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoFlowAnim(IntPtr L)
	{
		try
		{
			TargetFlow obj = (TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.DoFlowAnim(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_drawPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, targetFlow.drawPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_render(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, targetFlow.render);
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
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, targetFlow.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zOffset(IntPtr L)
	{
		try
		{
			TargetFlow targetFlow = (TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, targetFlow.zOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_xOffset(IntPtr L)
	{
		try
		{
			TargetFlow targetFlow = (TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, targetFlow.xOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, targetFlow.lineColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxYoffset(IntPtr L)
	{
		try
		{
			TargetFlow targetFlow = (TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, targetFlow.maxYoffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minYoffset(IntPtr L)
	{
		try
		{
			TargetFlow targetFlow = (TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, targetFlow.minYoffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__a(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, targetFlow._a);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__b(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, targetFlow._b);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_drawPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TargetFlow)objectTranslator.FastGetCSObj(L, 1)).drawPoint = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_render(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TargetFlow)objectTranslator.FastGetCSObj(L, 1)).render = (LineRenderer)objectTranslator.GetObject(L, 2, typeof(LineRenderer));
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
			((TargetFlow)objectTranslator.FastGetCSObj(L, 1)).target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zOffset(IntPtr L)
	{
		try
		{
			((TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).zOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_xOffset(IntPtr L)
	{
		try
		{
			((TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).xOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TargetFlow targetFlow = (TargetFlow)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			targetFlow.lineColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxYoffset(IntPtr L)
	{
		try
		{
			((TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxYoffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minYoffset(IntPtr L)
	{
		try
		{
			((TargetFlow)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).minYoffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__a(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TargetFlow)objectTranslator.FastGetCSObj(L, 1))._a = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__b(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TargetFlow)objectTranslator.FastGetCSObj(L, 1))._b = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
