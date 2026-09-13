using System;
using RiverGame.Rendering.MaterialPropertyBlockUtilities;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AppearenceUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AppearenceUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 0, 0);
		Utils.RegisterFunc(L, -4, "FindAttachmentPoint", _m_FindAttachmentPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "ApplyRimMPB", _m_ApplyRimMPB_xlua_st_);
		Utils.RegisterFunc(L, -4, "ProcessAndGetDefaultMaterial", _m_ProcessAndGetDefaultMaterial_xlua_st_);
		Utils.RegisterFunc(L, -4, "GrayEffect", _m_GrayEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "ModelScaleEffect", _m_ModelScaleEffect_xlua_st_);
		Utils.RegisterFunc(L, -4, "HitWhiteV2", _m_HitWhiteV2_xlua_st_);
		Utils.RegisterFunc(L, -4, "HitWhiteV3", _m_HitWhiteV3_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReplaceMaterial", _m_ReplaceMaterial_xlua_st_);
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
				AppearenceUtils o = new AppearenceUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AppearenceUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindAttachmentPoint_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform charRoot = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			string boneFullPath = Lua.lua_tostring(L, 2);
			Transform o = AppearenceUtils.FindAttachmentPoint(charRoot, boneFullPath);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ApplyRimMPB_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Renderer>(L, 1) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 2))
			{
				Renderer renderer = (Renderer)objectTranslator.GetObject(L, 1, typeof(Renderer));
				MaterialPropertyBlock mpb = (MaterialPropertyBlock)objectTranslator.GetObject(L, 2, typeof(MaterialPropertyBlock));
				AppearenceUtils.ApplyRimMPB(renderer, mpb);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<GPUSkinnedMeshRenderer>(L, 1) && objectTranslator.Assignable<Renderer>(L, 2) && objectTranslator.Assignable<MaterialPropertyBlock>(L, 3))
			{
				GPUSkinnedMeshRenderer gsRenderer = (GPUSkinnedMeshRenderer)objectTranslator.GetObject(L, 1, typeof(GPUSkinnedMeshRenderer));
				Renderer renderer2 = (Renderer)objectTranslator.GetObject(L, 2, typeof(Renderer));
				MaterialPropertyBlock mpb2 = (MaterialPropertyBlock)objectTranslator.GetObject(L, 3, typeof(MaterialPropertyBlock));
				AppearenceUtils.ApplyRimMPB(gsRenderer, renderer2, mpb2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AppearenceUtils.ApplyRimMPB!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProcessAndGetDefaultMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			(Material, Material) tuple = AppearenceUtils.ProcessAndGetDefaultMaterial((Renderer)objectTranslator.GetObject(L, 1, typeof(Renderer)));
			objectTranslator.Push(L, tuple);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GrayEffect_xlua_st_(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Renderer));
			bool enable = Lua.lua_toboolean(L, 2);
			AppearenceUtils.GrayEffect(renderer, enable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModelScaleEffect_xlua_st_(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Renderer));
			bool enable = Lua.lua_toboolean(L, 2);
			AppearenceUtils.ModelScaleEffect(renderer, enable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HitWhiteV2_xlua_st_(IntPtr L)
	{
		try
		{
			Renderer renderer = (Renderer)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Renderer));
			bool enable = Lua.lua_toboolean(L, 2);
			bool red = Lua.lua_toboolean(L, 3);
			AppearenceUtils.HitWhiteV2(renderer, enable, red);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HitWhiteV3_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<GPUSkinnedMeshRenderer>(L, 1) && objectTranslator.Assignable<Renderer>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				GPUSkinnedMeshRenderer gsRenderer = (GPUSkinnedMeshRenderer)objectTranslator.GetObject(L, 1, typeof(GPUSkinnedMeshRenderer));
				Renderer renderer = (Renderer)objectTranslator.GetObject(L, 2, typeof(Renderer));
				bool enable = Lua.lua_toboolean(L, 3);
				bool red = Lua.lua_toboolean(L, 4);
				AppearenceUtils.HitWhiteV3(gsRenderer, renderer, enable, red);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<GPUSkinnedMeshRenderer>(L, 1) && objectTranslator.Assignable<Renderer>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<MaterialPropertyGroup>(L, 4))
			{
				GPUSkinnedMeshRenderer gsRenderer2 = (GPUSkinnedMeshRenderer)objectTranslator.GetObject(L, 1, typeof(GPUSkinnedMeshRenderer));
				Renderer renderer2 = (Renderer)objectTranslator.GetObject(L, 2, typeof(Renderer));
				bool enable2 = Lua.lua_toboolean(L, 3);
				MaterialPropertyGroup style = (MaterialPropertyGroup)objectTranslator.GetObject(L, 4, typeof(MaterialPropertyGroup));
				AppearenceUtils.HitWhiteV3(gsRenderer2, renderer2, enable2, style);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AppearenceUtils.HitWhiteV3!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReplaceMaterial_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Renderer renderer = (Renderer)objectTranslator.GetObject(L, 1, typeof(Renderer));
			bool replace = Lua.lua_toboolean(L, 2);
			Material mat = (Material)objectTranslator.GetObject(L, 3, typeof(Material));
			AppearenceUtils.ReplaceMaterial(renderer, replace, mat);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
