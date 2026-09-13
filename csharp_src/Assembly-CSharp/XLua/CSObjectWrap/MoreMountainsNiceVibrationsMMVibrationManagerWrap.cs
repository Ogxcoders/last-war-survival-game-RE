using System;
using MoreMountains.NiceVibrations;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MoreMountainsNiceVibrationsMMVibrationManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MMVibrationManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 15, 11, 11);
		Utils.RegisterFunc(L, -4, "SetHapticsActive", _m_SetHapticsActive_xlua_st_);
		Utils.RegisterFunc(L, -4, "HapticsSupported", _m_HapticsSupported_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetDebugMode", _m_SetDebugMode_xlua_st_);
		Utils.RegisterFunc(L, -4, "Android", _m_Android_xlua_st_);
		Utils.RegisterFunc(L, -4, "iOS", _m_iOS_xlua_st_);
		Utils.RegisterFunc(L, -4, "Vibrate", _m_Vibrate_xlua_st_);
		Utils.RegisterFunc(L, -4, "Haptic", _m_Haptic_xlua_st_);
		Utils.RegisterFunc(L, -4, "TransientHaptic", _m_TransientHaptic_xlua_st_);
		Utils.RegisterFunc(L, -4, "ContinuousHaptic", _m_ContinuousHaptic_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateContinuousHaptic", _m_UpdateContinuousHaptic_xlua_st_);
		Utils.RegisterFunc(L, -4, "StopAllHaptics", _m_StopAllHaptics_xlua_st_);
		Utils.RegisterFunc(L, -4, "StopContinuousHaptic", _m_StopContinuousHaptic_xlua_st_);
		Utils.RegisterFunc(L, -4, "AdvancedHapticPattern", _m_AdvancedHapticPattern_xlua_st_);
		Utils.RegisterFunc(L, -4, "Remap", _m_Remap_xlua_st_);
		Utils.RegisterFunc(L, -2, "iOSVersion", _g_get_iOSVersion);
		Utils.RegisterFunc(L, -2, "LightDuration", _g_get_LightDuration);
		Utils.RegisterFunc(L, -2, "MediumDuration", _g_get_MediumDuration);
		Utils.RegisterFunc(L, -2, "HeavyDuration", _g_get_HeavyDuration);
		Utils.RegisterFunc(L, -2, "RigidDuration", _g_get_RigidDuration);
		Utils.RegisterFunc(L, -2, "SoftDuration", _g_get_SoftDuration);
		Utils.RegisterFunc(L, -2, "LightAmplitude", _g_get_LightAmplitude);
		Utils.RegisterFunc(L, -2, "MediumAmplitude", _g_get_MediumAmplitude);
		Utils.RegisterFunc(L, -2, "HeavyAmplitude", _g_get_HeavyAmplitude);
		Utils.RegisterFunc(L, -2, "RigidAmplitude", _g_get_RigidAmplitude);
		Utils.RegisterFunc(L, -2, "SoftAmplitude", _g_get_SoftAmplitude);
		Utils.RegisterFunc(L, -1, "iOSVersion", _s_set_iOSVersion);
		Utils.RegisterFunc(L, -1, "LightDuration", _s_set_LightDuration);
		Utils.RegisterFunc(L, -1, "MediumDuration", _s_set_MediumDuration);
		Utils.RegisterFunc(L, -1, "HeavyDuration", _s_set_HeavyDuration);
		Utils.RegisterFunc(L, -1, "RigidDuration", _s_set_RigidDuration);
		Utils.RegisterFunc(L, -1, "SoftDuration", _s_set_SoftDuration);
		Utils.RegisterFunc(L, -1, "LightAmplitude", _s_set_LightAmplitude);
		Utils.RegisterFunc(L, -1, "MediumAmplitude", _s_set_MediumAmplitude);
		Utils.RegisterFunc(L, -1, "HeavyAmplitude", _s_set_HeavyAmplitude);
		Utils.RegisterFunc(L, -1, "RigidAmplitude", _s_set_RigidAmplitude);
		Utils.RegisterFunc(L, -1, "SoftAmplitude", _s_set_SoftAmplitude);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MoreMountains.NiceVibrations.MMVibrationManager does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetHapticsActive_xlua_st_(IntPtr L)
	{
		try
		{
			MMVibrationManager.SetHapticsActive(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HapticsSupported_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = MMVibrationManager.HapticsSupported();
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
			MMVibrationManager.SetDebugMode(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Android_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = MMVibrationManager.Android();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_iOS_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = MMVibrationManager.iOS();
			Lua.lua_pushboolean(L, value);
			return 1;
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
			MMVibrationManager.Vibrate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Haptic_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<HapticTypes>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<MonoBehaviour>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out HapticTypes v);
				bool defaultToRegularVibrate = Lua.lua_toboolean(L, 2);
				bool alsoRumble = Lua.lua_toboolean(L, 3);
				MonoBehaviour coroutineSupport = (MonoBehaviour)objectTranslator.GetObject(L, 4, typeof(MonoBehaviour));
				int controllerID = Lua.xlua_tointeger(L, 5);
				MMVibrationManager.Haptic(v, defaultToRegularVibrate, alsoRumble, coroutineSupport, controllerID);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<HapticTypes>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<MonoBehaviour>(L, 4))
			{
				objectTranslator.Get(L, 1, out HapticTypes v2);
				bool defaultToRegularVibrate2 = Lua.lua_toboolean(L, 2);
				bool alsoRumble2 = Lua.lua_toboolean(L, 3);
				MonoBehaviour coroutineSupport2 = (MonoBehaviour)objectTranslator.GetObject(L, 4, typeof(MonoBehaviour));
				MMVibrationManager.Haptic(v2, defaultToRegularVibrate2, alsoRumble2, coroutineSupport2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<HapticTypes>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out HapticTypes v3);
				bool defaultToRegularVibrate3 = Lua.lua_toboolean(L, 2);
				bool alsoRumble3 = Lua.lua_toboolean(L, 3);
				MMVibrationManager.Haptic(v3, defaultToRegularVibrate3, alsoRumble3);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<HapticTypes>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out HapticTypes v4);
				bool defaultToRegularVibrate4 = Lua.lua_toboolean(L, 2);
				MMVibrationManager.Haptic(v4, defaultToRegularVibrate4);
				return 0;
			}
			if (num == 1 && objectTranslator.Assignable<HapticTypes>(L, 1))
			{
				objectTranslator.Get(L, 1, out HapticTypes v5);
				MMVibrationManager.Haptic(v5);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.Haptic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TransientHaptic_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<MonoBehaviour>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float intensity = (float)Lua.lua_tonumber(L, 1);
				float sharpness = (float)Lua.lua_tonumber(L, 2);
				bool alsoRumble = Lua.lua_toboolean(L, 3);
				MonoBehaviour coroutineSupport = (MonoBehaviour)objectTranslator.GetObject(L, 4, typeof(MonoBehaviour));
				int controllerID = Lua.xlua_tointeger(L, 5);
				MMVibrationManager.TransientHaptic(intensity, sharpness, alsoRumble, coroutineSupport, controllerID);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<MonoBehaviour>(L, 4))
			{
				float intensity2 = (float)Lua.lua_tonumber(L, 1);
				float sharpness2 = (float)Lua.lua_tonumber(L, 2);
				bool alsoRumble2 = Lua.lua_toboolean(L, 3);
				MonoBehaviour coroutineSupport2 = (MonoBehaviour)objectTranslator.GetObject(L, 4, typeof(MonoBehaviour));
				MMVibrationManager.TransientHaptic(intensity2, sharpness2, alsoRumble2, coroutineSupport2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float intensity3 = (float)Lua.lua_tonumber(L, 1);
				float sharpness3 = (float)Lua.lua_tonumber(L, 2);
				bool alsoRumble3 = Lua.lua_toboolean(L, 3);
				MMVibrationManager.TransientHaptic(intensity3, sharpness3, alsoRumble3);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float intensity4 = (float)Lua.lua_tonumber(L, 1);
				float sharpness4 = (float)Lua.lua_tonumber(L, 2);
				MMVibrationManager.TransientHaptic(intensity4, sharpness4);
				return 0;
			}
			if (num == 13 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && objectTranslator.Assignable<MonoBehaviour>(L, 12) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 13))
			{
				bool vibrateiOS = Lua.lua_toboolean(L, 1);
				float iOSIntensity = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid = Lua.lua_toboolean(L, 4);
				float androidIntensity = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport = Lua.lua_toboolean(L, 7);
				bool rumble = Lua.lua_toboolean(L, 8);
				float rumbleLowFrequency = (float)Lua.lua_tonumber(L, 9);
				float rumbleHighFrequency = (float)Lua.lua_tonumber(L, 10);
				int controllerID2 = Lua.xlua_tointeger(L, 11);
				MonoBehaviour coroutineSupport3 = (MonoBehaviour)objectTranslator.GetObject(L, 12, typeof(MonoBehaviour));
				bool threaded = Lua.lua_toboolean(L, 13);
				MMVibrationManager.TransientHaptic(vibrateiOS, iOSIntensity, iOSSharpness, vibrateAndroid, androidIntensity, androidSharpness, vibrateAndroidIfNoSupport, rumble, rumbleLowFrequency, rumbleHighFrequency, controllerID2, coroutineSupport3, threaded);
				return 0;
			}
			if (num == 12 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && objectTranslator.Assignable<MonoBehaviour>(L, 12))
			{
				bool vibrateiOS2 = Lua.lua_toboolean(L, 1);
				float iOSIntensity2 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness2 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid2 = Lua.lua_toboolean(L, 4);
				float androidIntensity2 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness2 = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport2 = Lua.lua_toboolean(L, 7);
				bool rumble2 = Lua.lua_toboolean(L, 8);
				float rumbleLowFrequency2 = (float)Lua.lua_tonumber(L, 9);
				float rumbleHighFrequency2 = (float)Lua.lua_tonumber(L, 10);
				int controllerID3 = Lua.xlua_tointeger(L, 11);
				MonoBehaviour coroutineSupport4 = (MonoBehaviour)objectTranslator.GetObject(L, 12, typeof(MonoBehaviour));
				MMVibrationManager.TransientHaptic(vibrateiOS2, iOSIntensity2, iOSSharpness2, vibrateAndroid2, androidIntensity2, androidSharpness2, vibrateAndroidIfNoSupport2, rumble2, rumbleLowFrequency2, rumbleHighFrequency2, controllerID3, coroutineSupport4);
				return 0;
			}
			if (num == 11 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				bool vibrateiOS3 = Lua.lua_toboolean(L, 1);
				float iOSIntensity3 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness3 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid3 = Lua.lua_toboolean(L, 4);
				float androidIntensity3 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness3 = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport3 = Lua.lua_toboolean(L, 7);
				bool rumble3 = Lua.lua_toboolean(L, 8);
				float rumbleLowFrequency3 = (float)Lua.lua_tonumber(L, 9);
				float rumbleHighFrequency3 = (float)Lua.lua_tonumber(L, 10);
				int controllerID4 = Lua.xlua_tointeger(L, 11);
				MMVibrationManager.TransientHaptic(vibrateiOS3, iOSIntensity3, iOSSharpness3, vibrateAndroid3, androidIntensity3, androidSharpness3, vibrateAndroidIfNoSupport3, rumble3, rumbleLowFrequency3, rumbleHighFrequency3, controllerID4);
				return 0;
			}
			if (num == 10 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				bool vibrateiOS4 = Lua.lua_toboolean(L, 1);
				float iOSIntensity4 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness4 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid4 = Lua.lua_toboolean(L, 4);
				float androidIntensity4 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness4 = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport4 = Lua.lua_toboolean(L, 7);
				bool rumble4 = Lua.lua_toboolean(L, 8);
				float rumbleLowFrequency4 = (float)Lua.lua_tonumber(L, 9);
				float rumbleHighFrequency4 = (float)Lua.lua_tonumber(L, 10);
				MMVibrationManager.TransientHaptic(vibrateiOS4, iOSIntensity4, iOSSharpness4, vibrateAndroid4, androidIntensity4, androidSharpness4, vibrateAndroidIfNoSupport4, rumble4, rumbleLowFrequency4, rumbleHighFrequency4);
				return 0;
			}
			if (num == 9 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				bool vibrateiOS5 = Lua.lua_toboolean(L, 1);
				float iOSIntensity5 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness5 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid5 = Lua.lua_toboolean(L, 4);
				float androidIntensity5 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness5 = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport5 = Lua.lua_toboolean(L, 7);
				bool rumble5 = Lua.lua_toboolean(L, 8);
				float rumbleLowFrequency5 = (float)Lua.lua_tonumber(L, 9);
				MMVibrationManager.TransientHaptic(vibrateiOS5, iOSIntensity5, iOSSharpness5, vibrateAndroid5, androidIntensity5, androidSharpness5, vibrateAndroidIfNoSupport5, rumble5, rumbleLowFrequency5);
				return 0;
			}
			if (num == 8 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				bool vibrateiOS6 = Lua.lua_toboolean(L, 1);
				float iOSIntensity6 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness6 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid6 = Lua.lua_toboolean(L, 4);
				float androidIntensity6 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness6 = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport6 = Lua.lua_toboolean(L, 7);
				bool rumble6 = Lua.lua_toboolean(L, 8);
				MMVibrationManager.TransientHaptic(vibrateiOS6, iOSIntensity6, iOSSharpness6, vibrateAndroid6, androidIntensity6, androidSharpness6, vibrateAndroidIfNoSupport6, rumble6);
				return 0;
			}
			if (num == 7 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				bool vibrateiOS7 = Lua.lua_toboolean(L, 1);
				float iOSIntensity7 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness7 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid7 = Lua.lua_toboolean(L, 4);
				float androidIntensity7 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness7 = (float)Lua.lua_tonumber(L, 6);
				bool vibrateAndroidIfNoSupport7 = Lua.lua_toboolean(L, 7);
				MMVibrationManager.TransientHaptic(vibrateiOS7, iOSIntensity7, iOSSharpness7, vibrateAndroid7, androidIntensity7, androidSharpness7, vibrateAndroidIfNoSupport7);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				bool vibrateiOS8 = Lua.lua_toboolean(L, 1);
				float iOSIntensity8 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness8 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid8 = Lua.lua_toboolean(L, 4);
				float androidIntensity8 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness8 = (float)Lua.lua_tonumber(L, 6);
				MMVibrationManager.TransientHaptic(vibrateiOS8, iOSIntensity8, iOSSharpness8, vibrateAndroid8, androidIntensity8, androidSharpness8);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				bool vibrateiOS9 = Lua.lua_toboolean(L, 1);
				float iOSIntensity9 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness9 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid9 = Lua.lua_toboolean(L, 4);
				float androidIntensity9 = (float)Lua.lua_tonumber(L, 5);
				MMVibrationManager.TransientHaptic(vibrateiOS9, iOSIntensity9, iOSSharpness9, vibrateAndroid9, androidIntensity9);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				bool vibrateiOS10 = Lua.lua_toboolean(L, 1);
				float iOSIntensity10 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness10 = (float)Lua.lua_tonumber(L, 3);
				bool vibrateAndroid10 = Lua.lua_toboolean(L, 4);
				MMVibrationManager.TransientHaptic(vibrateiOS10, iOSIntensity10, iOSSharpness10, vibrateAndroid10);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.TransientHaptic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ContinuousHaptic_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 9 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && objectTranslator.Assignable<MonoBehaviour>(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				float intensity = (float)Lua.lua_tonumber(L, 1);
				float sharpness = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v);
				MMVibrationManager.ContinuousHaptic(mono: (MonoBehaviour)objectTranslator.GetObject(L, 5, typeof(MonoBehaviour)), alsoRumble: Lua.lua_toboolean(L, 6), controllerID: Lua.xlua_tointeger(L, 7), threaded: Lua.lua_toboolean(L, 8), fullIntensity: Lua.lua_toboolean(L, 9), intensity: intensity, sharpness: sharpness, duration: duration, fallbackOldiOS: v);
				return 0;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && objectTranslator.Assignable<MonoBehaviour>(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				float intensity2 = (float)Lua.lua_tonumber(L, 1);
				float sharpness2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v2);
				MMVibrationManager.ContinuousHaptic(mono: (MonoBehaviour)objectTranslator.GetObject(L, 5, typeof(MonoBehaviour)), alsoRumble: Lua.lua_toboolean(L, 6), controllerID: Lua.xlua_tointeger(L, 7), threaded: Lua.lua_toboolean(L, 8), intensity: intensity2, sharpness: sharpness2, duration: duration2, fallbackOldiOS: v2);
				return 0;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && objectTranslator.Assignable<MonoBehaviour>(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				float intensity3 = (float)Lua.lua_tonumber(L, 1);
				float sharpness3 = (float)Lua.lua_tonumber(L, 2);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v3);
				MMVibrationManager.ContinuousHaptic(mono: (MonoBehaviour)objectTranslator.GetObject(L, 5, typeof(MonoBehaviour)), alsoRumble: Lua.lua_toboolean(L, 6), controllerID: Lua.xlua_tointeger(L, 7), intensity: intensity3, sharpness: sharpness3, duration: duration3, fallbackOldiOS: v3);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && objectTranslator.Assignable<MonoBehaviour>(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float intensity4 = (float)Lua.lua_tonumber(L, 1);
				float sharpness4 = (float)Lua.lua_tonumber(L, 2);
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v4);
				MMVibrationManager.ContinuousHaptic(mono: (MonoBehaviour)objectTranslator.GetObject(L, 5, typeof(MonoBehaviour)), alsoRumble: Lua.lua_toboolean(L, 6), intensity: intensity4, sharpness: sharpness4, duration: duration4, fallbackOldiOS: v4);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && objectTranslator.Assignable<MonoBehaviour>(L, 5))
			{
				float intensity5 = (float)Lua.lua_tonumber(L, 1);
				float sharpness5 = (float)Lua.lua_tonumber(L, 2);
				float duration5 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v5);
				MMVibrationManager.ContinuousHaptic(mono: (MonoBehaviour)objectTranslator.GetObject(L, 5, typeof(MonoBehaviour)), intensity: intensity5, sharpness: sharpness5, duration: duration5, fallbackOldiOS: v5);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4))
			{
				float intensity6 = (float)Lua.lua_tonumber(L, 1);
				float sharpness6 = (float)Lua.lua_tonumber(L, 2);
				float duration6 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v6);
				MMVibrationManager.ContinuousHaptic(intensity6, sharpness6, duration6, v6);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float intensity7 = (float)Lua.lua_tonumber(L, 1);
				float sharpness7 = (float)Lua.lua_tonumber(L, 2);
				float duration7 = (float)Lua.lua_tonumber(L, 3);
				MMVibrationManager.ContinuousHaptic(intensity7, sharpness7, duration7);
				return 0;
			}
			if (num == 16 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 13) && objectTranslator.Assignable<MonoBehaviour>(L, 14) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 15) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 16))
			{
				bool vibrateiOS = Lua.lua_toboolean(L, 1);
				float iOSIntensity = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v7);
				MMVibrationManager.ContinuousHaptic(vibrateAndroid: Lua.lua_toboolean(L, 5), androidIntensity: (float)Lua.lua_tonumber(L, 6), androidSharpness: (float)Lua.lua_tonumber(L, 7), vibrateAndroidIfNoSupport: Lua.lua_toboolean(L, 8), rumble: Lua.lua_toboolean(L, 9), rumbleLowFrequency: (float)Lua.lua_tonumber(L, 10), rumbleHighFrequency: (float)Lua.lua_tonumber(L, 11), controllerID: Lua.xlua_tointeger(L, 12), duration: (float)Lua.lua_tonumber(L, 13), mono: (MonoBehaviour)objectTranslator.GetObject(L, 14, typeof(MonoBehaviour)), threaded: Lua.lua_toboolean(L, 15), fullIntensity: Lua.lua_toboolean(L, 16), vibrateiOS: vibrateiOS, iOSIntensity: iOSIntensity, iOSSharpness: iOSSharpness, fallbackOldiOS: v7);
				return 0;
			}
			if (num == 15 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 13) && objectTranslator.Assignable<MonoBehaviour>(L, 14) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 15))
			{
				bool vibrateiOS2 = Lua.lua_toboolean(L, 1);
				float iOSIntensity2 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v8);
				MMVibrationManager.ContinuousHaptic(vibrateAndroid: Lua.lua_toboolean(L, 5), androidIntensity: (float)Lua.lua_tonumber(L, 6), androidSharpness: (float)Lua.lua_tonumber(L, 7), vibrateAndroidIfNoSupport: Lua.lua_toboolean(L, 8), rumble: Lua.lua_toboolean(L, 9), rumbleLowFrequency: (float)Lua.lua_tonumber(L, 10), rumbleHighFrequency: (float)Lua.lua_tonumber(L, 11), controllerID: Lua.xlua_tointeger(L, 12), duration: (float)Lua.lua_tonumber(L, 13), mono: (MonoBehaviour)objectTranslator.GetObject(L, 14, typeof(MonoBehaviour)), threaded: Lua.lua_toboolean(L, 15), vibrateiOS: vibrateiOS2, iOSIntensity: iOSIntensity2, iOSSharpness: iOSSharpness2, fallbackOldiOS: v8);
				return 0;
			}
			if (num == 14 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 13) && objectTranslator.Assignable<MonoBehaviour>(L, 14))
			{
				bool vibrateiOS3 = Lua.lua_toboolean(L, 1);
				float iOSIntensity3 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v9);
				MMVibrationManager.ContinuousHaptic(vibrateAndroid: Lua.lua_toboolean(L, 5), androidIntensity: (float)Lua.lua_tonumber(L, 6), androidSharpness: (float)Lua.lua_tonumber(L, 7), vibrateAndroidIfNoSupport: Lua.lua_toboolean(L, 8), rumble: Lua.lua_toboolean(L, 9), rumbleLowFrequency: (float)Lua.lua_tonumber(L, 10), rumbleHighFrequency: (float)Lua.lua_tonumber(L, 11), controllerID: Lua.xlua_tointeger(L, 12), duration: (float)Lua.lua_tonumber(L, 13), mono: (MonoBehaviour)objectTranslator.GetObject(L, 14, typeof(MonoBehaviour)), vibrateiOS: vibrateiOS3, iOSIntensity: iOSIntensity3, iOSSharpness: iOSSharpness3, fallbackOldiOS: v9);
				return 0;
			}
			if (num == 13 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<HapticTypes>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 13))
			{
				bool vibrateiOS4 = Lua.lua_toboolean(L, 1);
				float iOSIntensity4 = (float)Lua.lua_tonumber(L, 2);
				float iOSSharpness4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out HapticTypes v10);
				MMVibrationManager.ContinuousHaptic(vibrateAndroid: Lua.lua_toboolean(L, 5), androidIntensity: (float)Lua.lua_tonumber(L, 6), androidSharpness: (float)Lua.lua_tonumber(L, 7), vibrateAndroidIfNoSupport: Lua.lua_toboolean(L, 8), rumble: Lua.lua_toboolean(L, 9), rumbleLowFrequency: (float)Lua.lua_tonumber(L, 10), rumbleHighFrequency: (float)Lua.lua_tonumber(L, 11), controllerID: Lua.xlua_tointeger(L, 12), duration: (float)Lua.lua_tonumber(L, 13), vibrateiOS: vibrateiOS4, iOSIntensity: iOSIntensity4, iOSSharpness: iOSSharpness4, fallbackOldiOS: v10);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.ContinuousHaptic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateContinuousHaptic_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				float intensity = (float)Lua.lua_tonumber(L, 1);
				float sharpness = (float)Lua.lua_tonumber(L, 2);
				bool alsoRumble = Lua.lua_toboolean(L, 3);
				int controllerID = Lua.xlua_tointeger(L, 4);
				bool threaded = Lua.lua_toboolean(L, 5);
				MMVibrationManager.UpdateContinuousHaptic(intensity, sharpness, alsoRumble, controllerID, threaded);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float intensity2 = (float)Lua.lua_tonumber(L, 1);
				float sharpness2 = (float)Lua.lua_tonumber(L, 2);
				bool alsoRumble2 = Lua.lua_toboolean(L, 3);
				int controllerID2 = Lua.xlua_tointeger(L, 4);
				MMVibrationManager.UpdateContinuousHaptic(intensity2, sharpness2, alsoRumble2, controllerID2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float intensity3 = (float)Lua.lua_tonumber(L, 1);
				float sharpness3 = (float)Lua.lua_tonumber(L, 2);
				bool alsoRumble3 = Lua.lua_toboolean(L, 3);
				MMVibrationManager.UpdateContinuousHaptic(intensity3, sharpness3, alsoRumble3);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float intensity4 = (float)Lua.lua_tonumber(L, 1);
				float sharpness4 = (float)Lua.lua_tonumber(L, 2);
				MMVibrationManager.UpdateContinuousHaptic(intensity4, sharpness4);
				return 0;
			}
			if (num == 11 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 11))
			{
				bool ios = Lua.lua_toboolean(L, 1);
				float iosIntensity = (float)Lua.lua_tonumber(L, 2);
				float iosSharpness = (float)Lua.lua_tonumber(L, 3);
				bool android = Lua.lua_toboolean(L, 4);
				float androidIntensity = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness = (float)Lua.lua_tonumber(L, 6);
				bool rumble = Lua.lua_toboolean(L, 7);
				float rumbleLowFrequency = (float)Lua.lua_tonumber(L, 8);
				float rumbleHighFrequency = (float)Lua.lua_tonumber(L, 9);
				int controllerID3 = Lua.xlua_tointeger(L, 10);
				bool threaded2 = Lua.lua_toboolean(L, 11);
				MMVibrationManager.UpdateContinuousHaptic(ios, iosIntensity, iosSharpness, android, androidIntensity, androidSharpness, rumble, rumbleLowFrequency, rumbleHighFrequency, controllerID3, threaded2);
				return 0;
			}
			if (num == 10 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				bool ios2 = Lua.lua_toboolean(L, 1);
				float iosIntensity2 = (float)Lua.lua_tonumber(L, 2);
				float iosSharpness2 = (float)Lua.lua_tonumber(L, 3);
				bool android2 = Lua.lua_toboolean(L, 4);
				float androidIntensity2 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness2 = (float)Lua.lua_tonumber(L, 6);
				bool rumble2 = Lua.lua_toboolean(L, 7);
				float rumbleLowFrequency2 = (float)Lua.lua_tonumber(L, 8);
				float rumbleHighFrequency2 = (float)Lua.lua_tonumber(L, 9);
				int controllerID4 = Lua.xlua_tointeger(L, 10);
				MMVibrationManager.UpdateContinuousHaptic(ios2, iosIntensity2, iosSharpness2, android2, androidIntensity2, androidSharpness2, rumble2, rumbleLowFrequency2, rumbleHighFrequency2, controllerID4);
				return 0;
			}
			if (num == 9 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				bool ios3 = Lua.lua_toboolean(L, 1);
				float iosIntensity3 = (float)Lua.lua_tonumber(L, 2);
				float iosSharpness3 = (float)Lua.lua_tonumber(L, 3);
				bool android3 = Lua.lua_toboolean(L, 4);
				float androidIntensity3 = (float)Lua.lua_tonumber(L, 5);
				float androidSharpness3 = (float)Lua.lua_tonumber(L, 6);
				bool rumble3 = Lua.lua_toboolean(L, 7);
				float rumbleLowFrequency3 = (float)Lua.lua_tonumber(L, 8);
				float rumbleHighFrequency3 = (float)Lua.lua_tonumber(L, 9);
				MMVibrationManager.UpdateContinuousHaptic(ios3, iosIntensity3, iosSharpness3, android3, androidIntensity3, androidSharpness3, rumble3, rumbleLowFrequency3, rumbleHighFrequency3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.UpdateContinuousHaptic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAllHaptics_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				MMVibrationManager.StopAllHaptics(Lua.lua_toboolean(L, 1));
				return 0;
			}
			if (num == 0)
			{
				MMVibrationManager.StopAllHaptics();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.StopAllHaptics!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopContinuousHaptic_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				MMVibrationManager.StopContinuousHaptic(Lua.lua_toboolean(L, 1));
				return 0;
			}
			if (num == 0)
			{
				MMVibrationManager.StopContinuousHaptic();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.StopContinuousHaptic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AdvancedHapticPattern_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 12 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<long[]>(L, 2) && objectTranslator.Assignable<int[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<long[]>(L, 5) && objectTranslator.Assignable<int[]>(L, 6) && objectTranslator.Assignable<int[]>(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<HapticTypes>(L, 9) && objectTranslator.Assignable<MonoBehaviour>(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 12))
			{
				string iOSJSONString = Lua.lua_tostring(L, 1);
				long[] androidPattern = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				int[] androidAmplitudes = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
				int androidRepeat = Lua.xlua_tointeger(L, 4);
				long[] rumblePattern = (long[])objectTranslator.GetObject(L, 5, typeof(long[]));
				int[] rumbleLowFreqAmplitudes = (int[])objectTranslator.GetObject(L, 6, typeof(int[]));
				int[] rumbleHighFreqAmplitudes = (int[])objectTranslator.GetObject(L, 7, typeof(int[]));
				int rumbleRepeat = Lua.xlua_tointeger(L, 8);
				objectTranslator.Get(L, 9, out HapticTypes v);
				MMVibrationManager.AdvancedHapticPattern(coroutineSupport: (MonoBehaviour)objectTranslator.GetObject(L, 10, typeof(MonoBehaviour)), controllerID: Lua.xlua_tointeger(L, 11), threaded: Lua.lua_toboolean(L, 12), iOSJSONString: iOSJSONString, androidPattern: androidPattern, androidAmplitudes: androidAmplitudes, androidRepeat: androidRepeat, rumblePattern: rumblePattern, rumbleLowFreqAmplitudes: rumbleLowFreqAmplitudes, rumbleHighFreqAmplitudes: rumbleHighFreqAmplitudes, rumbleRepeat: rumbleRepeat, fallbackOldiOS: v);
				return 0;
			}
			if (num == 11 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<long[]>(L, 2) && objectTranslator.Assignable<int[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<long[]>(L, 5) && objectTranslator.Assignable<int[]>(L, 6) && objectTranslator.Assignable<int[]>(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<HapticTypes>(L, 9) && objectTranslator.Assignable<MonoBehaviour>(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				string iOSJSONString2 = Lua.lua_tostring(L, 1);
				long[] androidPattern2 = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				int[] androidAmplitudes2 = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
				int androidRepeat2 = Lua.xlua_tointeger(L, 4);
				long[] rumblePattern2 = (long[])objectTranslator.GetObject(L, 5, typeof(long[]));
				int[] rumbleLowFreqAmplitudes2 = (int[])objectTranslator.GetObject(L, 6, typeof(int[]));
				int[] rumbleHighFreqAmplitudes2 = (int[])objectTranslator.GetObject(L, 7, typeof(int[]));
				int rumbleRepeat2 = Lua.xlua_tointeger(L, 8);
				objectTranslator.Get(L, 9, out HapticTypes v2);
				MMVibrationManager.AdvancedHapticPattern(coroutineSupport: (MonoBehaviour)objectTranslator.GetObject(L, 10, typeof(MonoBehaviour)), controllerID: Lua.xlua_tointeger(L, 11), iOSJSONString: iOSJSONString2, androidPattern: androidPattern2, androidAmplitudes: androidAmplitudes2, androidRepeat: androidRepeat2, rumblePattern: rumblePattern2, rumbleLowFreqAmplitudes: rumbleLowFreqAmplitudes2, rumbleHighFreqAmplitudes: rumbleHighFreqAmplitudes2, rumbleRepeat: rumbleRepeat2, fallbackOldiOS: v2);
				return 0;
			}
			if (num == 10 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<long[]>(L, 2) && objectTranslator.Assignable<int[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<long[]>(L, 5) && objectTranslator.Assignable<int[]>(L, 6) && objectTranslator.Assignable<int[]>(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<HapticTypes>(L, 9) && objectTranslator.Assignable<MonoBehaviour>(L, 10))
			{
				string iOSJSONString3 = Lua.lua_tostring(L, 1);
				long[] androidPattern3 = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				int[] androidAmplitudes3 = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
				int androidRepeat3 = Lua.xlua_tointeger(L, 4);
				long[] rumblePattern3 = (long[])objectTranslator.GetObject(L, 5, typeof(long[]));
				int[] rumbleLowFreqAmplitudes3 = (int[])objectTranslator.GetObject(L, 6, typeof(int[]));
				int[] rumbleHighFreqAmplitudes3 = (int[])objectTranslator.GetObject(L, 7, typeof(int[]));
				int rumbleRepeat3 = Lua.xlua_tointeger(L, 8);
				objectTranslator.Get(L, 9, out HapticTypes v3);
				MMVibrationManager.AdvancedHapticPattern(coroutineSupport: (MonoBehaviour)objectTranslator.GetObject(L, 10, typeof(MonoBehaviour)), iOSJSONString: iOSJSONString3, androidPattern: androidPattern3, androidAmplitudes: androidAmplitudes3, androidRepeat: androidRepeat3, rumblePattern: rumblePattern3, rumbleLowFreqAmplitudes: rumbleLowFreqAmplitudes3, rumbleHighFreqAmplitudes: rumbleHighFreqAmplitudes3, rumbleRepeat: rumbleRepeat3, fallbackOldiOS: v3);
				return 0;
			}
			if (num == 9 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<long[]>(L, 2) && objectTranslator.Assignable<int[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<long[]>(L, 5) && objectTranslator.Assignable<int[]>(L, 6) && objectTranslator.Assignable<int[]>(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && objectTranslator.Assignable<HapticTypes>(L, 9))
			{
				string iOSJSONString4 = Lua.lua_tostring(L, 1);
				long[] androidPattern4 = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				int[] androidAmplitudes4 = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
				int androidRepeat4 = Lua.xlua_tointeger(L, 4);
				long[] rumblePattern4 = (long[])objectTranslator.GetObject(L, 5, typeof(long[]));
				int[] rumbleLowFreqAmplitudes4 = (int[])objectTranslator.GetObject(L, 6, typeof(int[]));
				int[] rumbleHighFreqAmplitudes4 = (int[])objectTranslator.GetObject(L, 7, typeof(int[]));
				int rumbleRepeat4 = Lua.xlua_tointeger(L, 8);
				objectTranslator.Get(L, 9, out HapticTypes v4);
				MMVibrationManager.AdvancedHapticPattern(iOSJSONString4, androidPattern4, androidAmplitudes4, androidRepeat4, rumblePattern4, rumbleLowFreqAmplitudes4, rumbleHighFreqAmplitudes4, rumbleRepeat4, v4);
				return 0;
			}
			if (num == 8 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<long[]>(L, 2) && objectTranslator.Assignable<int[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<long[]>(L, 5) && objectTranslator.Assignable<int[]>(L, 6) && objectTranslator.Assignable<int[]>(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				string iOSJSONString5 = Lua.lua_tostring(L, 1);
				long[] androidPattern5 = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				int[] androidAmplitudes5 = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
				int androidRepeat5 = Lua.xlua_tointeger(L, 4);
				long[] rumblePattern5 = (long[])objectTranslator.GetObject(L, 5, typeof(long[]));
				int[] rumbleLowFreqAmplitudes5 = (int[])objectTranslator.GetObject(L, 6, typeof(int[]));
				int[] rumbleHighFreqAmplitudes5 = (int[])objectTranslator.GetObject(L, 7, typeof(int[]));
				int rumbleRepeat5 = Lua.xlua_tointeger(L, 8);
				MMVibrationManager.AdvancedHapticPattern(iOSJSONString5, androidPattern5, androidAmplitudes5, androidRepeat5, rumblePattern5, rumbleLowFreqAmplitudes5, rumbleHighFreqAmplitudes5, rumbleRepeat5);
				return 0;
			}
			if (num == 16 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<long[]>(L, 4) && objectTranslator.Assignable<int[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && objectTranslator.Assignable<long[]>(L, 9) && objectTranslator.Assignable<int[]>(L, 10) && objectTranslator.Assignable<int[]>(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && objectTranslator.Assignable<HapticTypes>(L, 13) && objectTranslator.Assignable<MonoBehaviour>(L, 14) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 15) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 16))
			{
				bool ios = Lua.lua_toboolean(L, 1);
				string iOSJSONString6 = Lua.lua_tostring(L, 2);
				bool android = Lua.lua_toboolean(L, 3);
				long[] androidPattern6 = (long[])objectTranslator.GetObject(L, 4, typeof(long[]));
				int[] androidAmplitudes6 = (int[])objectTranslator.GetObject(L, 5, typeof(int[]));
				int androidRepeat6 = Lua.xlua_tointeger(L, 6);
				bool vibrateAndroidIfNoSupport = Lua.lua_toboolean(L, 7);
				bool rumble = Lua.lua_toboolean(L, 8);
				long[] rumblePattern6 = (long[])objectTranslator.GetObject(L, 9, typeof(long[]));
				int[] rumbleLowFreqAmplitudes6 = (int[])objectTranslator.GetObject(L, 10, typeof(int[]));
				int[] rumbleHighFreqAmplitudes6 = (int[])objectTranslator.GetObject(L, 11, typeof(int[]));
				int rumbleRepeat6 = Lua.xlua_tointeger(L, 12);
				objectTranslator.Get(L, 13, out HapticTypes v5);
				MMVibrationManager.AdvancedHapticPattern(coroutineSupport: (MonoBehaviour)objectTranslator.GetObject(L, 14, typeof(MonoBehaviour)), controllerID: Lua.xlua_tointeger(L, 15), threaded: Lua.lua_toboolean(L, 16), ios: ios, iOSJSONString: iOSJSONString6, android: android, androidPattern: androidPattern6, androidAmplitudes: androidAmplitudes6, androidRepeat: androidRepeat6, vibrateAndroidIfNoSupport: vibrateAndroidIfNoSupport, rumble: rumble, rumblePattern: rumblePattern6, rumbleLowFreqAmplitudes: rumbleLowFreqAmplitudes6, rumbleHighFreqAmplitudes: rumbleHighFreqAmplitudes6, rumbleRepeat: rumbleRepeat6, fallbackOldiOS: v5);
				return 0;
			}
			if (num == 15 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<long[]>(L, 4) && objectTranslator.Assignable<int[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && objectTranslator.Assignable<long[]>(L, 9) && objectTranslator.Assignable<int[]>(L, 10) && objectTranslator.Assignable<int[]>(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && objectTranslator.Assignable<HapticTypes>(L, 13) && objectTranslator.Assignable<MonoBehaviour>(L, 14) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 15))
			{
				bool ios2 = Lua.lua_toboolean(L, 1);
				string iOSJSONString7 = Lua.lua_tostring(L, 2);
				bool android2 = Lua.lua_toboolean(L, 3);
				long[] androidPattern7 = (long[])objectTranslator.GetObject(L, 4, typeof(long[]));
				int[] androidAmplitudes7 = (int[])objectTranslator.GetObject(L, 5, typeof(int[]));
				int androidRepeat7 = Lua.xlua_tointeger(L, 6);
				bool vibrateAndroidIfNoSupport2 = Lua.lua_toboolean(L, 7);
				bool rumble2 = Lua.lua_toboolean(L, 8);
				long[] rumblePattern7 = (long[])objectTranslator.GetObject(L, 9, typeof(long[]));
				int[] rumbleLowFreqAmplitudes7 = (int[])objectTranslator.GetObject(L, 10, typeof(int[]));
				int[] rumbleHighFreqAmplitudes7 = (int[])objectTranslator.GetObject(L, 11, typeof(int[]));
				int rumbleRepeat7 = Lua.xlua_tointeger(L, 12);
				objectTranslator.Get(L, 13, out HapticTypes v6);
				MMVibrationManager.AdvancedHapticPattern(coroutineSupport: (MonoBehaviour)objectTranslator.GetObject(L, 14, typeof(MonoBehaviour)), controllerID: Lua.xlua_tointeger(L, 15), ios: ios2, iOSJSONString: iOSJSONString7, android: android2, androidPattern: androidPattern7, androidAmplitudes: androidAmplitudes7, androidRepeat: androidRepeat7, vibrateAndroidIfNoSupport: vibrateAndroidIfNoSupport2, rumble: rumble2, rumblePattern: rumblePattern7, rumbleLowFreqAmplitudes: rumbleLowFreqAmplitudes7, rumbleHighFreqAmplitudes: rumbleHighFreqAmplitudes7, rumbleRepeat: rumbleRepeat7, fallbackOldiOS: v6);
				return 0;
			}
			if (num == 14 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<long[]>(L, 4) && objectTranslator.Assignable<int[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && objectTranslator.Assignable<long[]>(L, 9) && objectTranslator.Assignable<int[]>(L, 10) && objectTranslator.Assignable<int[]>(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && objectTranslator.Assignable<HapticTypes>(L, 13) && objectTranslator.Assignable<MonoBehaviour>(L, 14))
			{
				bool ios3 = Lua.lua_toboolean(L, 1);
				string iOSJSONString8 = Lua.lua_tostring(L, 2);
				bool android3 = Lua.lua_toboolean(L, 3);
				long[] androidPattern8 = (long[])objectTranslator.GetObject(L, 4, typeof(long[]));
				int[] androidAmplitudes8 = (int[])objectTranslator.GetObject(L, 5, typeof(int[]));
				int androidRepeat8 = Lua.xlua_tointeger(L, 6);
				bool vibrateAndroidIfNoSupport3 = Lua.lua_toboolean(L, 7);
				bool rumble3 = Lua.lua_toboolean(L, 8);
				long[] rumblePattern8 = (long[])objectTranslator.GetObject(L, 9, typeof(long[]));
				int[] rumbleLowFreqAmplitudes8 = (int[])objectTranslator.GetObject(L, 10, typeof(int[]));
				int[] rumbleHighFreqAmplitudes8 = (int[])objectTranslator.GetObject(L, 11, typeof(int[]));
				int rumbleRepeat8 = Lua.xlua_tointeger(L, 12);
				objectTranslator.Get(L, 13, out HapticTypes v7);
				MMVibrationManager.AdvancedHapticPattern(coroutineSupport: (MonoBehaviour)objectTranslator.GetObject(L, 14, typeof(MonoBehaviour)), ios: ios3, iOSJSONString: iOSJSONString8, android: android3, androidPattern: androidPattern8, androidAmplitudes: androidAmplitudes8, androidRepeat: androidRepeat8, vibrateAndroidIfNoSupport: vibrateAndroidIfNoSupport3, rumble: rumble3, rumblePattern: rumblePattern8, rumbleLowFreqAmplitudes: rumbleLowFreqAmplitudes8, rumbleHighFreqAmplitudes: rumbleHighFreqAmplitudes8, rumbleRepeat: rumbleRepeat8, fallbackOldiOS: v7);
				return 0;
			}
			if (num == 13 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<long[]>(L, 4) && objectTranslator.Assignable<int[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && objectTranslator.Assignable<long[]>(L, 9) && objectTranslator.Assignable<int[]>(L, 10) && objectTranslator.Assignable<int[]>(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && objectTranslator.Assignable<HapticTypes>(L, 13))
			{
				bool ios4 = Lua.lua_toboolean(L, 1);
				string iOSJSONString9 = Lua.lua_tostring(L, 2);
				bool android4 = Lua.lua_toboolean(L, 3);
				long[] androidPattern9 = (long[])objectTranslator.GetObject(L, 4, typeof(long[]));
				int[] androidAmplitudes9 = (int[])objectTranslator.GetObject(L, 5, typeof(int[]));
				int androidRepeat9 = Lua.xlua_tointeger(L, 6);
				bool vibrateAndroidIfNoSupport4 = Lua.lua_toboolean(L, 7);
				bool rumble4 = Lua.lua_toboolean(L, 8);
				long[] rumblePattern9 = (long[])objectTranslator.GetObject(L, 9, typeof(long[]));
				int[] rumbleLowFreqAmplitudes9 = (int[])objectTranslator.GetObject(L, 10, typeof(int[]));
				int[] rumbleHighFreqAmplitudes9 = (int[])objectTranslator.GetObject(L, 11, typeof(int[]));
				int rumbleRepeat9 = Lua.xlua_tointeger(L, 12);
				objectTranslator.Get(L, 13, out HapticTypes v8);
				MMVibrationManager.AdvancedHapticPattern(ios4, iOSJSONString9, android4, androidPattern9, androidAmplitudes9, androidRepeat9, vibrateAndroidIfNoSupport4, rumble4, rumblePattern9, rumbleLowFreqAmplitudes9, rumbleHighFreqAmplitudes9, rumbleRepeat9, v8);
				return 0;
			}
			if (num == 12 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<long[]>(L, 4) && objectTranslator.Assignable<int[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8) && objectTranslator.Assignable<long[]>(L, 9) && objectTranslator.Assignable<int[]>(L, 10) && objectTranslator.Assignable<int[]>(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12))
			{
				bool ios5 = Lua.lua_toboolean(L, 1);
				string iOSJSONString10 = Lua.lua_tostring(L, 2);
				bool android5 = Lua.lua_toboolean(L, 3);
				long[] androidPattern10 = (long[])objectTranslator.GetObject(L, 4, typeof(long[]));
				int[] androidAmplitudes10 = (int[])objectTranslator.GetObject(L, 5, typeof(int[]));
				int androidRepeat10 = Lua.xlua_tointeger(L, 6);
				bool vibrateAndroidIfNoSupport5 = Lua.lua_toboolean(L, 7);
				bool rumble5 = Lua.lua_toboolean(L, 8);
				long[] rumblePattern10 = (long[])objectTranslator.GetObject(L, 9, typeof(long[]));
				int[] rumbleLowFreqAmplitudes10 = (int[])objectTranslator.GetObject(L, 10, typeof(int[]));
				int[] rumbleHighFreqAmplitudes10 = (int[])objectTranslator.GetObject(L, 11, typeof(int[]));
				int rumbleRepeat10 = Lua.xlua_tointeger(L, 12);
				MMVibrationManager.AdvancedHapticPattern(ios5, iOSJSONString10, android5, androidPattern10, androidAmplitudes10, androidRepeat10, vibrateAndroidIfNoSupport5, rumble5, rumblePattern10, rumbleLowFreqAmplitudes10, rumbleHighFreqAmplitudes10, rumbleRepeat10);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MoreMountains.NiceVibrations.MMVibrationManager.AdvancedHapticPattern!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Remap_xlua_st_(IntPtr L)
	{
		try
		{
			float x = (float)Lua.lua_tonumber(L, 1);
			float a = (float)Lua.lua_tonumber(L, 2);
			float b = (float)Lua.lua_tonumber(L, 3);
			float c = (float)Lua.lua_tonumber(L, 4);
			float d = (float)Lua.lua_tonumber(L, 5);
			float num = MMVibrationManager.Remap(x, a, b, c, d);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_iOSVersion(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, MMVibrationManager.iOSVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LightDuration(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, MMVibrationManager.LightDuration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MediumDuration(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, MMVibrationManager.MediumDuration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeavyDuration(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, MMVibrationManager.HeavyDuration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RigidDuration(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, MMVibrationManager.RigidDuration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SoftDuration(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, MMVibrationManager.SoftDuration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LightAmplitude(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, MMVibrationManager.LightAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MediumAmplitude(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, MMVibrationManager.MediumAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HeavyAmplitude(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, MMVibrationManager.HeavyAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RigidAmplitude(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, MMVibrationManager.RigidAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SoftAmplitude(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, MMVibrationManager.SoftAmplitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_iOSVersion(IntPtr L)
	{
		try
		{
			MMVibrationManager.iOSVersion = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LightDuration(IntPtr L)
	{
		try
		{
			MMVibrationManager.LightDuration = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MediumDuration(IntPtr L)
	{
		try
		{
			MMVibrationManager.MediumDuration = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HeavyDuration(IntPtr L)
	{
		try
		{
			MMVibrationManager.HeavyDuration = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RigidDuration(IntPtr L)
	{
		try
		{
			MMVibrationManager.RigidDuration = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SoftDuration(IntPtr L)
	{
		try
		{
			MMVibrationManager.SoftDuration = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LightAmplitude(IntPtr L)
	{
		try
		{
			MMVibrationManager.LightAmplitude = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MediumAmplitude(IntPtr L)
	{
		try
		{
			MMVibrationManager.MediumAmplitude = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HeavyAmplitude(IntPtr L)
	{
		try
		{
			MMVibrationManager.HeavyAmplitude = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RigidAmplitude(IntPtr L)
	{
		try
		{
			MMVibrationManager.RigidAmplitude = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SoftAmplitude(IntPtr L)
	{
		try
		{
			MMVibrationManager.SoftAmplitude = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
