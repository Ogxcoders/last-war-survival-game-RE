using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineSkinnedMeshRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SkinnedMeshRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 8, 8);
		Utils.RegisterFunc(L, -3, "GetBlendShapeWeight", _m_GetBlendShapeWeight);
		Utils.RegisterFunc(L, -3, "SetBlendShapeWeight", _m_SetBlendShapeWeight);
		Utils.RegisterFunc(L, -3, "BakeMesh", _m_BakeMesh);
		Utils.RegisterFunc(L, -2, "quality", _g_get_quality);
		Utils.RegisterFunc(L, -2, "updateWhenOffscreen", _g_get_updateWhenOffscreen);
		Utils.RegisterFunc(L, -2, "forceMatrixRecalculationPerRender", _g_get_forceMatrixRecalculationPerRender);
		Utils.RegisterFunc(L, -2, "rootBone", _g_get_rootBone);
		Utils.RegisterFunc(L, -2, "bones", _g_get_bones);
		Utils.RegisterFunc(L, -2, "sharedMesh", _g_get_sharedMesh);
		Utils.RegisterFunc(L, -2, "skinnedMotionVectors", _g_get_skinnedMotionVectors);
		Utils.RegisterFunc(L, -2, "localBounds", _g_get_localBounds);
		Utils.RegisterFunc(L, -1, "quality", _s_set_quality);
		Utils.RegisterFunc(L, -1, "updateWhenOffscreen", _s_set_updateWhenOffscreen);
		Utils.RegisterFunc(L, -1, "forceMatrixRecalculationPerRender", _s_set_forceMatrixRecalculationPerRender);
		Utils.RegisterFunc(L, -1, "rootBone", _s_set_rootBone);
		Utils.RegisterFunc(L, -1, "bones", _s_set_bones);
		Utils.RegisterFunc(L, -1, "sharedMesh", _s_set_sharedMesh);
		Utils.RegisterFunc(L, -1, "skinnedMotionVectors", _s_set_skinnedMotionVectors);
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
				SkinnedMeshRenderer o = new SkinnedMeshRenderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SkinnedMeshRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBlendShapeWeight(IntPtr L)
	{
		try
		{
			SkinnedMeshRenderer obj = (SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			float blendShapeWeight = obj.GetBlendShapeWeight(index);
			Lua.lua_pushnumber(L, blendShapeWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBlendShapeWeight(IntPtr L)
	{
		try
		{
			SkinnedMeshRenderer obj = (SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			obj.SetBlendShapeWeight(index, value);
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
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
			skinnedMeshRenderer.BakeMesh(mesh);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quality(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skinnedMeshRenderer.quality);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateWhenOffscreen(IntPtr L)
	{
		try
		{
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skinnedMeshRenderer.updateWhenOffscreen);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forceMatrixRecalculationPerRender(IntPtr L)
	{
		try
		{
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skinnedMeshRenderer.forceMatrixRecalculationPerRender);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rootBone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skinnedMeshRenderer.rootBone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bones(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skinnedMeshRenderer.bones);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sharedMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skinnedMeshRenderer.sharedMesh);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skinnedMotionVectors(IntPtr L)
	{
		try
		{
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skinnedMeshRenderer.skinnedMotionVectors);
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
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, skinnedMeshRenderer.localBounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quality(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SkinQuality v);
			skinnedMeshRenderer.quality = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateWhenOffscreen(IntPtr L)
	{
		try
		{
			((SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).updateWhenOffscreen = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_forceMatrixRecalculationPerRender(IntPtr L)
	{
		try
		{
			((SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).forceMatrixRecalculationPerRender = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rootBone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).rootBone = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bones(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).bones = (Transform[])objectTranslator.GetObject(L, 2, typeof(Transform[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sharedMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).sharedMesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skinnedMotionVectors(IntPtr L)
	{
		try
		{
			((SkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).skinnedMotionVectors = Lua.lua_toboolean(L, 2);
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
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Bounds val);
			skinnedMeshRenderer.localBounds = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
