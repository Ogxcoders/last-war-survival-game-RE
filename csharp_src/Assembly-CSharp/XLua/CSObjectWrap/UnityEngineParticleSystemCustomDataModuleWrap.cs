using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemCustomDataModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.CustomDataModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 1, 1);
		Utils.RegisterFunc(L, -3, "SetMode", _m_SetMode);
		Utils.RegisterFunc(L, -3, "GetMode", _m_GetMode);
		Utils.RegisterFunc(L, -3, "SetVectorComponentCount", _m_SetVectorComponentCount);
		Utils.RegisterFunc(L, -3, "GetVectorComponentCount", _m_GetVectorComponentCount);
		Utils.RegisterFunc(L, -3, "SetVector", _m_SetVector);
		Utils.RegisterFunc(L, -3, "GetVector", _m_GetVector);
		Utils.RegisterFunc(L, -3, "SetColor", _m_SetColor);
		Utils.RegisterFunc(L, -3, "GetColor", _m_GetColor);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
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
				objectTranslator.Push(L, default(ParticleSystem.CustomDataModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.CustomDataModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			objectTranslator.Get(L, 3, out ParticleSystemCustomDataMode v3);
			v.SetMode(v2, v3);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			ParticleSystemCustomDataMode mode = v.GetMode(v2);
			objectTranslator.Push(L, mode);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVectorComponentCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			v.SetVectorComponentCount(count: Lua.xlua_tointeger(L, 3), stream: v2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVectorComponentCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			int vectorComponentCount = v.GetVectorComponentCount(v2);
			Lua.xlua_pushinteger(L, vectorComponentCount);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			int component = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out ParticleSystem.MinMaxCurve v3);
			v.SetVector(v2, component, v3);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			ParticleSystem.MinMaxCurve minMaxCurve = v.GetVector(component: Lua.xlua_tointeger(L, 3), stream: v2);
			objectTranslator.Push(L, minMaxCurve);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			objectTranslator.Get(L, 3, out ParticleSystem.MinMaxGradient v3);
			v.SetColor(v2, v3);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			objectTranslator.Get(L, 2, out ParticleSystemCustomData v2);
			ParticleSystem.MinMaxGradient color = v.GetColor(v2);
			objectTranslator.Push(L, color);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.CustomDataModule v);
			Lua.lua_pushboolean(L, v.enabled);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.CustomDataModule v);
			v.enabled = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
