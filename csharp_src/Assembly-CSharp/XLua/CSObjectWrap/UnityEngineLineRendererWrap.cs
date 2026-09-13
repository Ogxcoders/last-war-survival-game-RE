using System;
using DG.Tweening;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineLineRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LineRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 16, 16);
		Utils.RegisterFunc(L, -3, "SetPosition", _m_SetPosition);
		Utils.RegisterFunc(L, -3, "GetPosition", _m_GetPosition);
		Utils.RegisterFunc(L, -3, "Simplify", _m_Simplify);
		Utils.RegisterFunc(L, -3, "BakeMesh", _m_BakeMesh);
		Utils.RegisterFunc(L, -3, "GetPositions", _m_GetPositions);
		Utils.RegisterFunc(L, -3, "SetPositions", _m_SetPositions);
		Utils.RegisterFunc(L, -3, "DOColor", _m_DOColor);
		Utils.RegisterFunc(L, -2, "startWidth", _g_get_startWidth);
		Utils.RegisterFunc(L, -2, "endWidth", _g_get_endWidth);
		Utils.RegisterFunc(L, -2, "widthMultiplier", _g_get_widthMultiplier);
		Utils.RegisterFunc(L, -2, "numCornerVertices", _g_get_numCornerVertices);
		Utils.RegisterFunc(L, -2, "numCapVertices", _g_get_numCapVertices);
		Utils.RegisterFunc(L, -2, "useWorldSpace", _g_get_useWorldSpace);
		Utils.RegisterFunc(L, -2, "loop", _g_get_loop);
		Utils.RegisterFunc(L, -2, "startColor", _g_get_startColor);
		Utils.RegisterFunc(L, -2, "endColor", _g_get_endColor);
		Utils.RegisterFunc(L, -2, "positionCount", _g_get_positionCount);
		Utils.RegisterFunc(L, -2, "shadowBias", _g_get_shadowBias);
		Utils.RegisterFunc(L, -2, "generateLightingData", _g_get_generateLightingData);
		Utils.RegisterFunc(L, -2, "textureMode", _g_get_textureMode);
		Utils.RegisterFunc(L, -2, "alignment", _g_get_alignment);
		Utils.RegisterFunc(L, -2, "widthCurve", _g_get_widthCurve);
		Utils.RegisterFunc(L, -2, "colorGradient", _g_get_colorGradient);
		Utils.RegisterFunc(L, -1, "startWidth", _s_set_startWidth);
		Utils.RegisterFunc(L, -1, "endWidth", _s_set_endWidth);
		Utils.RegisterFunc(L, -1, "widthMultiplier", _s_set_widthMultiplier);
		Utils.RegisterFunc(L, -1, "numCornerVertices", _s_set_numCornerVertices);
		Utils.RegisterFunc(L, -1, "numCapVertices", _s_set_numCapVertices);
		Utils.RegisterFunc(L, -1, "useWorldSpace", _s_set_useWorldSpace);
		Utils.RegisterFunc(L, -1, "loop", _s_set_loop);
		Utils.RegisterFunc(L, -1, "startColor", _s_set_startColor);
		Utils.RegisterFunc(L, -1, "endColor", _s_set_endColor);
		Utils.RegisterFunc(L, -1, "positionCount", _s_set_positionCount);
		Utils.RegisterFunc(L, -1, "shadowBias", _s_set_shadowBias);
		Utils.RegisterFunc(L, -1, "generateLightingData", _s_set_generateLightingData);
		Utils.RegisterFunc(L, -1, "textureMode", _s_set_textureMode);
		Utils.RegisterFunc(L, -1, "alignment", _s_set_alignment);
		Utils.RegisterFunc(L, -1, "widthCurve", _s_set_widthCurve);
		Utils.RegisterFunc(L, -1, "colorGradient", _s_set_colorGradient);
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
				LineRenderer o = new LineRenderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.LineRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			lineRenderer.SetPosition(index, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer obj = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			Vector3 position = obj.GetPosition(index);
			objectTranslator.PushUnityEngineVector3(L, position);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Simplify(IntPtr L)
	{
		try
		{
			LineRenderer obj = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float tolerance = (float)Lua.lua_tonumber(L, 2);
			obj.Simplify(tolerance);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BakeMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Mesh>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
				bool useTransform = Lua.lua_toboolean(L, 3);
				lineRenderer.BakeMesh(mesh, useTransform);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Mesh>(L, 2))
			{
				Mesh mesh2 = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
				lineRenderer.BakeMesh(mesh2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Mesh>(L, 2) && objectTranslator.Assignable<Camera>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				Mesh mesh3 = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
				Camera camera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
				bool useTransform2 = Lua.lua_toboolean(L, 4);
				lineRenderer.BakeMesh(mesh3, camera, useTransform2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Mesh>(L, 2) && objectTranslator.Assignable<Camera>(L, 3))
			{
				Mesh mesh4 = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
				Camera camera2 = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
				lineRenderer.BakeMesh(mesh4, camera2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.LineRenderer.BakeMesh!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPositions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			Vector3[] positions = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
			int positions2 = lineRenderer.GetPositions(positions);
			Lua.xlua_pushinteger(L, positions2);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPositions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			Vector3[] positions = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
			lineRenderer.SetPositions(positions);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer target = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color2 v);
			objectTranslator.Get(L, 3, out Color2 v2);
			Tweener o = ShortcutExtensions.DOColor(duration: (float)Lua.lua_tonumber(L, 4), target: target, startValue: v, endValue: v2);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startWidth(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lineRenderer.startWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endWidth(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lineRenderer.endWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_widthMultiplier(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lineRenderer.widthMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_numCornerVertices(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lineRenderer.numCornerVertices);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_numCapVertices(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lineRenderer.numCapVertices);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useWorldSpace(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lineRenderer.useWorldSpace);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loop(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lineRenderer.loop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, lineRenderer.startColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, lineRenderer.endColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_positionCount(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lineRenderer.positionCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowBias(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lineRenderer.shadowBias);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_generateLightingData(IntPtr L)
	{
		try
		{
			LineRenderer lineRenderer = (LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lineRenderer.generateLightingData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lineRenderer.textureMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lineRenderer.alignment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_widthCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lineRenderer.widthCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lineRenderer.colorGradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startWidth(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startWidth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endWidth(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).endWidth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_widthMultiplier(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).widthMultiplier = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_numCornerVertices(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).numCornerVertices = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_numCapVertices(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).numCapVertices = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useWorldSpace(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useWorldSpace = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loop(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			lineRenderer.startColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			lineRenderer.endColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_positionCount(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).positionCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowBias(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shadowBias = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_generateLightingData(IntPtr L)
	{
		try
		{
			((LineRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).generateLightingData = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LineTextureMode v);
			lineRenderer.textureMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alignment(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer lineRenderer = (LineRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LineAlignment v);
			lineRenderer.alignment = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_widthCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LineRenderer)objectTranslator.FastGetCSObj(L, 1)).widthCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorGradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LineRenderer)objectTranslator.FastGetCSObj(L, 1)).colorGradient = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
