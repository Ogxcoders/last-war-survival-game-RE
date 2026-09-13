using System;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineShaderVariantCollectionShaderVariantWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ShaderVariantCollection.ShaderVariant);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 3, 3);
		Utils.RegisterFunc(L, -2, "shader", _g_get_shader);
		Utils.RegisterFunc(L, -2, "passType", _g_get_passType);
		Utils.RegisterFunc(L, -2, "keywords", _g_get_keywords);
		Utils.RegisterFunc(L, -1, "shader", _s_set_shader);
		Utils.RegisterFunc(L, -1, "passType", _s_set_passType);
		Utils.RegisterFunc(L, -1, "keywords", _s_set_keywords);
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
			if (Lua.lua_gettop(L) >= 3 && objectTranslator.Assignable<Shader>(L, 2) && objectTranslator.Assignable<PassType>(L, 3) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 4) || Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				Shader shader = (Shader)objectTranslator.GetObject(L, 2, typeof(Shader));
				objectTranslator.Get(L, 3, out PassType v);
				string[] keywords = objectTranslator.GetParams<string>(L, 4);
				ShaderVariantCollection.ShaderVariant shaderVariant = new ShaderVariantCollection.ShaderVariant(shader, v, keywords);
				objectTranslator.Push(L, shaderVariant);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.Push(L, default(ShaderVariantCollection.ShaderVariant));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ShaderVariantCollection.ShaderVariant constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ShaderVariantCollection.ShaderVariant v);
			objectTranslator.Push(L, v.shader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_passType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ShaderVariantCollection.ShaderVariant v);
			objectTranslator.Push(L, v.passType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_keywords(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ShaderVariantCollection.ShaderVariant v);
			objectTranslator.Push(L, v.keywords);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ShaderVariantCollection.ShaderVariant v);
			v.shader = (Shader)objectTranslator.GetObject(L, 2, typeof(Shader));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_passType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ShaderVariantCollection.ShaderVariant v);
			objectTranslator.Get(L, 2, out PassType v2);
			v.passType = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_keywords(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ShaderVariantCollection.ShaderVariant v);
			v.keywords = (string[])objectTranslator.GetObject(L, 2, typeof(string[]));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
