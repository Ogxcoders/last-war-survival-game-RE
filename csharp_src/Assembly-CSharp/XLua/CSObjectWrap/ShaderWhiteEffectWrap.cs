using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ShaderWhiteEffectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ShaderWhiteEffect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "PlayEffect", _m_PlayEffect);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 4, 4);
		Utils.RegisterFunc(L, -2, "ShakeWhiteEffectChangeTime", _g_get_ShakeWhiteEffectChangeTime);
		Utils.RegisterFunc(L, -2, "ShakeWhiteEffectStayTime", _g_get_ShakeWhiteEffectStayTime);
		Utils.RegisterFunc(L, -2, "ShakeWhiteEffectMinValue", _g_get_ShakeWhiteEffectMinValue);
		Utils.RegisterFunc(L, -2, "ShakeWhiteEffectMaxValue", _g_get_ShakeWhiteEffectMaxValue);
		Utils.RegisterFunc(L, -1, "ShakeWhiteEffectChangeTime", _s_set_ShakeWhiteEffectChangeTime);
		Utils.RegisterFunc(L, -1, "ShakeWhiteEffectStayTime", _s_set_ShakeWhiteEffectStayTime);
		Utils.RegisterFunc(L, -1, "ShakeWhiteEffectMinValue", _s_set_ShakeWhiteEffectMinValue);
		Utils.RegisterFunc(L, -1, "ShakeWhiteEffectMaxValue", _s_set_ShakeWhiteEffectMaxValue);
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
				ShaderWhiteEffect o = new ShaderWhiteEffect();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ShaderWhiteEffect constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayEffect(IntPtr L)
	{
		try
		{
			((ShaderWhiteEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShakeWhiteEffectChangeTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ShaderWhiteEffect.ShakeWhiteEffectChangeTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShakeWhiteEffectStayTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ShaderWhiteEffect.ShakeWhiteEffectStayTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShakeWhiteEffectMinValue(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ShaderWhiteEffect.ShakeWhiteEffectMinValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShakeWhiteEffectMaxValue(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ShaderWhiteEffect.ShakeWhiteEffectMaxValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ShakeWhiteEffectChangeTime(IntPtr L)
	{
		try
		{
			ShaderWhiteEffect.ShakeWhiteEffectChangeTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ShakeWhiteEffectStayTime(IntPtr L)
	{
		try
		{
			ShaderWhiteEffect.ShakeWhiteEffectStayTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ShakeWhiteEffectMinValue(IntPtr L)
	{
		try
		{
			ShaderWhiteEffect.ShakeWhiteEffectMinValue = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ShakeWhiteEffectMaxValue(IntPtr L)
	{
		try
		{
			ShaderWhiteEffect.ShakeWhiteEffectMaxValue = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
