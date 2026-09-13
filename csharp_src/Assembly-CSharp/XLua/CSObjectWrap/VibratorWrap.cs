using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VibratorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Vibrator);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 15, 0, 0);
		Utils.RegisterFunc(L, -4, "HapticsSupported", _m_HapticsSupported_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetDebugMode", _m_SetDebugMode_xlua_st_);
		Utils.RegisterFunc(L, -4, "SoftImpact", _m_SoftImpact_xlua_st_);
		Utils.RegisterFunc(L, -4, "LightImpact", _m_LightImpact_xlua_st_);
		Utils.RegisterFunc(L, -4, "MediumImpact", _m_MediumImpact_xlua_st_);
		Utils.RegisterFunc(L, -4, "HeavyImpact", _m_HeavyImpact_xlua_st_);
		Utils.RegisterFunc(L, -4, "Warning", _m_Warning_xlua_st_);
		Utils.RegisterFunc(L, -4, "Selection", _m_Selection_xlua_st_);
		Utils.RegisterFunc(L, -4, "Success", _m_Success_xlua_st_);
		Utils.RegisterFunc(L, -4, "Failure", _m_Failure_xlua_st_);
		Utils.RegisterFunc(L, -4, "RigidImpact", _m_RigidImpact_xlua_st_);
		Utils.RegisterFunc(L, -4, "Vibrate", _m_Vibrate_xlua_st_);
		Utils.RegisterFunc(L, -4, "ContinuousHaptic", _m_ContinuousHaptic_xlua_st_);
		Utils.RegisterFunc(L, -4, "StopContinuousHaptic", _m_StopContinuousHaptic_xlua_st_);
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
				Vibrator o = new Vibrator();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Vibrator constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HapticsSupported_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = Vibrator.HapticsSupported();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDebugMode_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.SetDebugMode(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SoftImpact_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.SoftImpact();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LightImpact_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.LightImpact();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MediumImpact_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.MediumImpact();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HeavyImpact_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.HeavyImpact();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Warning_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.Warning();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Selection_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.Selection();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Success_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.Success();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Failure_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.Failure();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RigidImpact_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.RigidImpact();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Vibrate_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.Vibrate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ContinuousHaptic_xlua_st_(IntPtr L)
	{
		try
		{
			float intensity = (float)Lua.lua_tonumber(L, 1);
			float sharpness = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			int fallbackOldiOS = Lua.xlua_tointeger(L, 4);
			Vibrator.ContinuousHaptic(intensity, sharpness, duration, fallbackOldiOS);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopContinuousHaptic_xlua_st_(IntPtr L)
	{
		try
		{
			Vibrator.StopContinuousHaptic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
