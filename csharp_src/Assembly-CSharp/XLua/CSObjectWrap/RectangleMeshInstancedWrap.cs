using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RectangleMeshInstancedWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RectangleMeshInstanced);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 6, 7);
		Utils.RegisterFunc(L, -3, "UpdateValues", _m_UpdateValues);
		Utils.RegisterFunc(L, -2, "MeshRenderer", _g_get_MeshRenderer);
		Utils.RegisterFunc(L, -2, "MeshFilter", _g_get_MeshFilter);
		Utils.RegisterFunc(L, -2, "lineColor", _g_get_lineColor);
		Utils.RegisterFunc(L, -2, "thickness", _g_get_thickness);
		Utils.RegisterFunc(L, -2, "alpha", _g_get_alpha);
		Utils.RegisterFunc(L, -2, "planeType", _g_get_planeType);
		Utils.RegisterFunc(L, -1, "LineColor", _s_set_LineColor);
		Utils.RegisterFunc(L, -1, "Thickness", _s_set_Thickness);
		Utils.RegisterFunc(L, -1, "Alpha", _s_set_Alpha);
		Utils.RegisterFunc(L, -1, "lineColor", _s_set_lineColor);
		Utils.RegisterFunc(L, -1, "thickness", _s_set_thickness);
		Utils.RegisterFunc(L, -1, "alpha", _s_set_alpha);
		Utils.RegisterFunc(L, -1, "planeType", _s_set_planeType);
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
				RectangleMeshInstanced o = new RectangleMeshInstanced();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RectangleMeshInstanced constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateValues(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			float thickness = (float)Lua.lua_tonumber(L, 3);
			float alpha = (float)Lua.lua_tonumber(L, 4);
			rectangleMeshInstanced.UpdateValues(val, thickness, alpha);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MeshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectangleMeshInstanced.MeshRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MeshFilter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectangleMeshInstanced.MeshFilter);
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
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, rectangleMeshInstanced.lineColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_thickness(IntPtr L)
	{
		try
		{
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rectangleMeshInstanced.thickness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alpha(IntPtr L)
	{
		try
		{
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rectangleMeshInstanced.alpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_planeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectangleMeshInstanced.planeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			rectangleMeshInstanced.LineColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Thickness(IntPtr L)
	{
		try
		{
			((RectangleMeshInstanced)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Thickness = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Alpha(IntPtr L)
	{
		try
		{
			((RectangleMeshInstanced)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Alpha = (float)Lua.lua_tonumber(L, 2);
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
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			rectangleMeshInstanced.lineColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_thickness(IntPtr L)
	{
		try
		{
			((RectangleMeshInstanced)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).thickness = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alpha(IntPtr L)
	{
		try
		{
			((RectangleMeshInstanced)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alpha = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_planeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMeshInstanced rectangleMeshInstanced = (RectangleMeshInstanced)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RectangleMeshInstanced.PlaneType v);
			rectangleMeshInstanced.planeType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
