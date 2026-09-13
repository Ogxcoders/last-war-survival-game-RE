using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RectangleMeshWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RectangleMesh);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 7, 5);
		Utils.RegisterFunc(L, -3, "SetMaterialFloat", _m_SetMaterialFloat);
		Utils.RegisterFunc(L, -3, "SetMaterialColor", _m_SetMaterialColor);
		Utils.RegisterFunc(L, -3, "SetRenderOrder", _m_SetRenderOrder);
		Utils.RegisterFunc(L, -3, "RebuildMesh", _m_RebuildMesh);
		Utils.RegisterFunc(L, -2, "MeshRenderer", _g_get_MeshRenderer);
		Utils.RegisterFunc(L, -2, "MainMaterial", _g_get_MainMaterial);
		Utils.RegisterFunc(L, -2, "center", _g_get_center);
		Utils.RegisterFunc(L, -2, "width", _g_get_width);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "lineWidth", _g_get_lineWidth);
		Utils.RegisterFunc(L, -2, "planeType", _g_get_planeType);
		Utils.RegisterFunc(L, -1, "center", _s_set_center);
		Utils.RegisterFunc(L, -1, "width", _s_set_width);
		Utils.RegisterFunc(L, -1, "height", _s_set_height);
		Utils.RegisterFunc(L, -1, "lineWidth", _s_set_lineWidth);
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
				RectangleMesh o = new RectangleMesh();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RectangleMesh constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaterialFloat(IntPtr L)
	{
		try
		{
			RectangleMesh obj = (RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			obj.SetMaterialFloat(id, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaterialColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out Color val);
			rectangleMesh.SetMaterialColor(id, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRenderOrder(IntPtr L)
	{
		try
		{
			RectangleMesh obj = (RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int renderOrder = Lua.xlua_tointeger(L, 2);
			obj.SetRenderOrder(renderOrder);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RebuildMesh(IntPtr L)
	{
		try
		{
			RectangleMesh obj = (RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float width = (float)Lua.lua_tonumber(L, 2);
			float height = (float)Lua.lua_tonumber(L, 3);
			float lineWidth = (float)Lua.lua_tonumber(L, 4);
			obj.RebuildMesh(width, height, lineWidth);
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
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectangleMesh.MeshRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MainMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectangleMesh.MainMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_center(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, rectangleMesh.center);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_width(IntPtr L)
	{
		try
		{
			RectangleMesh rectangleMesh = (RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rectangleMesh.width);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			RectangleMesh rectangleMesh = (RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rectangleMesh.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineWidth(IntPtr L)
	{
		try
		{
			RectangleMesh rectangleMesh = (RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rectangleMesh.lineWidth);
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
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rectangleMesh.planeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_center(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			rectangleMesh.center = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_width(IntPtr L)
	{
		try
		{
			((RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).width = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_height(IntPtr L)
	{
		try
		{
			((RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).height = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineWidth(IntPtr L)
	{
		try
		{
			((RectangleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineWidth = (float)Lua.lua_tonumber(L, 2);
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
			RectangleMesh rectangleMesh = (RectangleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RectangleMesh.PlaneType v);
			rectangleMesh.planeType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
