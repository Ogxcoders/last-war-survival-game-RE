using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineShaderVariantCollectionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ShaderVariantCollection);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 3, 0);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "WarmUp", _m_WarmUp);
		Utils.RegisterFunc(L, -3, "Add", _m_Add);
		Utils.RegisterFunc(L, -3, "Remove", _m_Remove);
		Utils.RegisterFunc(L, -3, "Contains", _m_Contains);
		Utils.RegisterFunc(L, -2, "shaderCount", _g_get_shaderCount);
		Utils.RegisterFunc(L, -2, "variantCount", _g_get_variantCount);
		Utils.RegisterFunc(L, -2, "isWarmedUp", _g_get_isWarmedUp);
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
				ShaderVariantCollection o = new ShaderVariantCollection();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ShaderVariantCollection constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((ShaderVariantCollection)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WarmUp(IntPtr L)
	{
		try
		{
			((ShaderVariantCollection)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WarmUp();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Add(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShaderVariantCollection shaderVariantCollection = (ShaderVariantCollection)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ShaderVariantCollection.ShaderVariant v);
			bool value = shaderVariantCollection.Add(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Remove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShaderVariantCollection shaderVariantCollection = (ShaderVariantCollection)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ShaderVariantCollection.ShaderVariant v);
			bool value = shaderVariantCollection.Remove(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Contains(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ShaderVariantCollection shaderVariantCollection = (ShaderVariantCollection)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ShaderVariantCollection.ShaderVariant v);
			bool value = shaderVariantCollection.Contains(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shaderCount(IntPtr L)
	{
		try
		{
			ShaderVariantCollection shaderVariantCollection = (ShaderVariantCollection)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, shaderVariantCollection.shaderCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_variantCount(IntPtr L)
	{
		try
		{
			ShaderVariantCollection shaderVariantCollection = (ShaderVariantCollection)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, shaderVariantCollection.variantCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isWarmedUp(IntPtr L)
	{
		try
		{
			ShaderVariantCollection shaderVariantCollection = (ShaderVariantCollection)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, shaderVariantCollection.isWarmedUp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
