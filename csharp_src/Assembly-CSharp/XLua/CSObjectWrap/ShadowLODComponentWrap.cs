using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ShadowLODComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ShadowLODComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 2, 2);
		Utils.RegisterFunc(L, -3, "Awake", _m_Awake);
		Utils.RegisterFunc(L, -3, "OnEnable", _m_OnEnable);
		Utils.RegisterFunc(L, -3, "OnDisable", _m_OnDisable);
		Utils.RegisterFunc(L, -3, "OnValidate", _m_OnValidate);
		Utils.RegisterFunc(L, -3, "ToggleShadow", _m_ToggleShadow);
		Utils.RegisterFunc(L, -2, "renderer", _g_get_renderer);
		Utils.RegisterFunc(L, -2, "material", _g_get_material);
		Utils.RegisterFunc(L, -1, "renderer", _s_set_renderer);
		Utils.RegisterFunc(L, -1, "material", _s_set_material);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "EnableShadow", _m_EnableShadow_xlua_st_);
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
				ShadowLODComponent o = new ShadowLODComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ShadowLODComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Awake(IntPtr L)
	{
		try
		{
			((ShadowLODComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Awake();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEnable(IntPtr L)
	{
		try
		{
			((ShadowLODComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEnable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDisable(IntPtr L)
	{
		try
		{
			((ShadowLODComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDisable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableShadow_xlua_st_(IntPtr L)
	{
		try
		{
			ShadowLODComponent.EnableShadow(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnValidate(IntPtr L)
	{
		try
		{
			((ShadowLODComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnValidate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToggleShadow(IntPtr L)
	{
		try
		{
			((ShadowLODComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToggleShadow();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShadowLODComponent shadowLODComponent = (ShadowLODComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, shadowLODComponent.renderer);
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
			ShadowLODComponent shadowLODComponent = (ShadowLODComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, shadowLODComponent.material);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ShadowLODComponent)objectTranslator.FastGetCSObj(L, 1)).renderer = (Renderer)objectTranslator.GetObject(L, 2, typeof(Renderer));
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
			((ShadowLODComponent)objectTranslator.FastGetCSObj(L, 1)).material = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
