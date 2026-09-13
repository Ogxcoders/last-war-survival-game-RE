using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Renderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 29, 24);
		Utils.RegisterFunc(L, -3, "HasPropertyBlock", _m_HasPropertyBlock);
		Utils.RegisterFunc(L, -3, "SetPropertyBlock", _m_SetPropertyBlock);
		Utils.RegisterFunc(L, -3, "GetPropertyBlock", _m_GetPropertyBlock);
		Utils.RegisterFunc(L, -3, "GetMaterials", _m_GetMaterials);
		Utils.RegisterFunc(L, -3, "GetSharedMaterials", _m_GetSharedMaterials);
		Utils.RegisterFunc(L, -3, "GetClosestReflectionProbes", _m_GetClosestReflectionProbes);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "isVisible", _g_get_isVisible);
		Utils.RegisterFunc(L, -2, "shadowCastingMode", _g_get_shadowCastingMode);
		Utils.RegisterFunc(L, -2, "receiveShadows", _g_get_receiveShadows);
		Utils.RegisterFunc(L, -2, "forceRenderingOff", _g_get_forceRenderingOff);
		Utils.RegisterFunc(L, -2, "motionVectorGenerationMode", _g_get_motionVectorGenerationMode);
		Utils.RegisterFunc(L, -2, "lightProbeUsage", _g_get_lightProbeUsage);
		Utils.RegisterFunc(L, -2, "reflectionProbeUsage", _g_get_reflectionProbeUsage);
		Utils.RegisterFunc(L, -2, "renderingLayerMask", _g_get_renderingLayerMask);
		Utils.RegisterFunc(L, -2, "rendererPriority", _g_get_rendererPriority);
		Utils.RegisterFunc(L, -2, "rayTracingMode", _g_get_rayTracingMode);
		Utils.RegisterFunc(L, -2, "sortingLayerName", _g_get_sortingLayerName);
		Utils.RegisterFunc(L, -2, "sortingLayerID", _g_get_sortingLayerID);
		Utils.RegisterFunc(L, -2, "sortingOrder", _g_get_sortingOrder);
		Utils.RegisterFunc(L, -2, "allowOcclusionWhenDynamic", _g_get_allowOcclusionWhenDynamic);
		Utils.RegisterFunc(L, -2, "isPartOfStaticBatch", _g_get_isPartOfStaticBatch);
		Utils.RegisterFunc(L, -2, "worldToLocalMatrix", _g_get_worldToLocalMatrix);
		Utils.RegisterFunc(L, -2, "localToWorldMatrix", _g_get_localToWorldMatrix);
		Utils.RegisterFunc(L, -2, "lightProbeProxyVolumeOverride", _g_get_lightProbeProxyVolumeOverride);
		Utils.RegisterFunc(L, -2, "probeAnchor", _g_get_probeAnchor);
		Utils.RegisterFunc(L, -2, "lightmapIndex", _g_get_lightmapIndex);
		Utils.RegisterFunc(L, -2, "realtimeLightmapIndex", _g_get_realtimeLightmapIndex);
		Utils.RegisterFunc(L, -2, "lightmapScaleOffset", _g_get_lightmapScaleOffset);
		Utils.RegisterFunc(L, -2, "realtimeLightmapScaleOffset", _g_get_realtimeLightmapScaleOffset);
		Utils.RegisterFunc(L, -2, "materials", _g_get_materials);
		Utils.RegisterFunc(L, -2, "material", _g_get_material);
		Utils.RegisterFunc(L, -2, "sharedMaterial", _g_get_sharedMaterial);
		Utils.RegisterFunc(L, -2, "sharedMaterials", _g_get_sharedMaterials);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "shadowCastingMode", _s_set_shadowCastingMode);
		Utils.RegisterFunc(L, -1, "receiveShadows", _s_set_receiveShadows);
		Utils.RegisterFunc(L, -1, "forceRenderingOff", _s_set_forceRenderingOff);
		Utils.RegisterFunc(L, -1, "motionVectorGenerationMode", _s_set_motionVectorGenerationMode);
		Utils.RegisterFunc(L, -1, "lightProbeUsage", _s_set_lightProbeUsage);
		Utils.RegisterFunc(L, -1, "reflectionProbeUsage", _s_set_reflectionProbeUsage);
		Utils.RegisterFunc(L, -1, "renderingLayerMask", _s_set_renderingLayerMask);
		Utils.RegisterFunc(L, -1, "rendererPriority", _s_set_rendererPriority);
		Utils.RegisterFunc(L, -1, "rayTracingMode", _s_set_rayTracingMode);
		Utils.RegisterFunc(L, -1, "sortingLayerName", _s_set_sortingLayerName);
		Utils.RegisterFunc(L, -1, "sortingLayerID", _s_set_sortingLayerID);
		Utils.RegisterFunc(L, -1, "sortingOrder", _s_set_sortingOrder);
		Utils.RegisterFunc(L, -1, "allowOcclusionWhenDynamic", _s_set_allowOcclusionWhenDynamic);
		Utils.RegisterFunc(L, -1, "lightProbeProxyVolumeOverride", _s_set_lightProbeProxyVolumeOverride);
		Utils.RegisterFunc(L, -1, "probeAnchor", _s_set_probeAnchor);
		Utils.RegisterFunc(L, -1, "lightmapIndex", _s_set_lightmapIndex);
		Utils.RegisterFunc(L, -1, "realtimeLightmapIndex", _s_set_realtimeLightmapIndex);
		Utils.RegisterFunc(L, -1, "lightmapScaleOffset", _s_set_lightmapScaleOffset);
		Utils.RegisterFunc(L, -1, "realtimeLightmapScaleOffset", _s_set_realtimeLightmapScaleOffset);
		Utils.RegisterFunc(L, -1, "materials", _s_set_materials);
		Utils.RegisterFunc(L, -1, "material", _s_set_material);
		Utils.RegisterFunc(L, -1, "sharedMaterial", _s_set_sharedMaterial);
		Utils.RegisterFunc(L, -1, "sharedMaterials", _s_set_sharedMaterials);
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
				Renderer o = new Renderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Renderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasPropertyBlock(IntPtr L)
	{
		try
		{
			bool value = ((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HasPropertyBlock();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPropertyBlock(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<MaterialPropertyBlock>(L, 2))
			{
				MaterialPropertyBlock propertyBlock = (MaterialPropertyBlock)objectTranslator.GetObject(L, 2, typeof(MaterialPropertyBlock));
				renderer.SetPropertyBlock(propertyBlock);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<MaterialPropertyBlock>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 2, typeof(MaterialPropertyBlock));
				int materialIndex = Lua.xlua_tointeger(L, 3);
				renderer.SetPropertyBlock(properties, materialIndex);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Renderer.SetPropertyBlock!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropertyBlock(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<MaterialPropertyBlock>(L, 2))
			{
				MaterialPropertyBlock properties = (MaterialPropertyBlock)objectTranslator.GetObject(L, 2, typeof(MaterialPropertyBlock));
				renderer.GetPropertyBlock(properties);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<MaterialPropertyBlock>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				MaterialPropertyBlock properties2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 2, typeof(MaterialPropertyBlock));
				int materialIndex = Lua.xlua_tointeger(L, 3);
				renderer.GetPropertyBlock(properties2, materialIndex);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Renderer.GetPropertyBlock!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			List<Material> m = (List<Material>)objectTranslator.GetObject(L, 2, typeof(List<Material>));
			renderer.GetMaterials(m);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSharedMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			List<Material> m = (List<Material>)objectTranslator.GetObject(L, 2, typeof(List<Material>));
			renderer.GetSharedMaterials(m);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClosestReflectionProbes(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			List<ReflectionProbeBlendInfo> result = (List<ReflectionProbeBlendInfo>)objectTranslator.GetObject(L, 2, typeof(List<ReflectionProbeBlendInfo>));
			renderer.GetClosestReflectionProbes(result);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, renderer.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderer.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isVisible(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderer.isVisible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowCastingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.shadowCastingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_receiveShadows(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderer.receiveShadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forceRenderingOff(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderer.forceRenderingOff);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_motionVectorGenerationMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.motionVectorGenerationMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lightProbeUsage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.lightProbeUsage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_reflectionProbeUsage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.reflectionProbeUsage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderingLayerMask(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushuint(L, renderer.renderingLayerMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rendererPriority(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderer.rendererPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rayTracingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.rayTracingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingLayerName(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, renderer.sortingLayerName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingLayerID(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderer.sortingLayerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingOrder(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderer.sortingOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowOcclusionWhenDynamic(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderer.allowOcclusionWhenDynamic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPartOfStaticBatch(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderer.isPartOfStaticBatch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldToLocalMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.worldToLocalMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localToWorldMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.localToWorldMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lightProbeProxyVolumeOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.lightProbeProxyVolumeOverride);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_probeAnchor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.probeAnchor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lightmapIndex(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderer.lightmapIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_realtimeLightmapIndex(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderer.realtimeLightmapIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lightmapScaleOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, renderer.lightmapScaleOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_realtimeLightmapScaleOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, renderer.realtimeLightmapScaleOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_materials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.materials);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_material(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.material);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.sharedMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sharedMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderer.sharedMaterials);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enabled(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enabled = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowCastingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ShadowCastingMode v);
			renderer.shadowCastingMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_receiveShadows(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).receiveShadows = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_forceRenderingOff(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).forceRenderingOff = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_motionVectorGenerationMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MotionVectorGenerationMode v);
			renderer.motionVectorGenerationMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lightProbeUsage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LightProbeUsage v);
			renderer.lightProbeUsage = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_reflectionProbeUsage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ReflectionProbeUsage v);
			renderer.reflectionProbeUsage = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderingLayerMask(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).renderingLayerMask = Lua.xlua_touint(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rendererPriority(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rendererPriority = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rayTracingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RayTracingMode v);
			renderer.rayTracingMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingLayerName(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingLayerName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingLayerID(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingLayerID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingOrder(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingOrder = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowOcclusionWhenDynamic(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowOcclusionWhenDynamic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lightProbeProxyVolumeOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Renderer)objectTranslator.FastGetCSObj(L, 1)).lightProbeProxyVolumeOverride = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_probeAnchor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Renderer)objectTranslator.FastGetCSObj(L, 1)).probeAnchor = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lightmapIndex(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lightmapIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_realtimeLightmapIndex(IntPtr L)
	{
		try
		{
			((Renderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).realtimeLightmapIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lightmapScaleOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			renderer.lightmapScaleOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_realtimeLightmapScaleOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			renderer.realtimeLightmapScaleOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_materials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Renderer)objectTranslator.FastGetCSObj(L, 1)).materials = (Material[])objectTranslator.GetObject(L, 2, typeof(Material[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_material(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Renderer)objectTranslator.FastGetCSObj(L, 1)).material = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Renderer)objectTranslator.FastGetCSObj(L, 1)).sharedMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sharedMaterials(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Renderer)objectTranslator.FastGetCSObj(L, 1)).sharedMaterials = (Material[])objectTranslator.GetObject(L, 2, typeof(Material[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
