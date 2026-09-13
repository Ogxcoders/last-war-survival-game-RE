using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SoundComponentPlaySoundParamsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SoundComponent.PlaySoundParams);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 17, 17);
		Utils.RegisterFunc(L, -2, "Time", _g_get_Time);
		Utils.RegisterFunc(L, -2, "MuteInSoundGroup", _g_get_MuteInSoundGroup);
		Utils.RegisterFunc(L, -2, "Loop", _g_get_Loop);
		Utils.RegisterFunc(L, -2, "Priority", _g_get_Priority);
		Utils.RegisterFunc(L, -2, "VolumeInSoundGroup", _g_get_VolumeInSoundGroup);
		Utils.RegisterFunc(L, -2, "ShotVolumeScale", _g_get_ShotVolumeScale);
		Utils.RegisterFunc(L, -2, "FadeInSeconds", _g_get_FadeInSeconds);
		Utils.RegisterFunc(L, -2, "FadeOutSeconds", _g_get_FadeOutSeconds);
		Utils.RegisterFunc(L, -2, "Pitch", _g_get_Pitch);
		Utils.RegisterFunc(L, -2, "PanStereo", _g_get_PanStereo);
		Utils.RegisterFunc(L, -2, "SpatialBlend", _g_get_SpatialBlend);
		Utils.RegisterFunc(L, -2, "MaxDistance", _g_get_MaxDistance);
		Utils.RegisterFunc(L, -2, "DopplerLevel", _g_get_DopplerLevel);
		Utils.RegisterFunc(L, -2, "SoundVolumeSet", _g_get_SoundVolumeSet);
		Utils.RegisterFunc(L, -2, "Loop_Gap", _g_get_Loop_Gap);
		Utils.RegisterFunc(L, -2, "SoundAssetPaths", _g_get_SoundAssetPaths);
		Utils.RegisterFunc(L, -2, "serialId", _g_get_serialId);
		Utils.RegisterFunc(L, -1, "Time", _s_set_Time);
		Utils.RegisterFunc(L, -1, "MuteInSoundGroup", _s_set_MuteInSoundGroup);
		Utils.RegisterFunc(L, -1, "Loop", _s_set_Loop);
		Utils.RegisterFunc(L, -1, "Priority", _s_set_Priority);
		Utils.RegisterFunc(L, -1, "VolumeInSoundGroup", _s_set_VolumeInSoundGroup);
		Utils.RegisterFunc(L, -1, "ShotVolumeScale", _s_set_ShotVolumeScale);
		Utils.RegisterFunc(L, -1, "FadeInSeconds", _s_set_FadeInSeconds);
		Utils.RegisterFunc(L, -1, "FadeOutSeconds", _s_set_FadeOutSeconds);
		Utils.RegisterFunc(L, -1, "Pitch", _s_set_Pitch);
		Utils.RegisterFunc(L, -1, "PanStereo", _s_set_PanStereo);
		Utils.RegisterFunc(L, -1, "SpatialBlend", _s_set_SpatialBlend);
		Utils.RegisterFunc(L, -1, "MaxDistance", _s_set_MaxDistance);
		Utils.RegisterFunc(L, -1, "DopplerLevel", _s_set_DopplerLevel);
		Utils.RegisterFunc(L, -1, "SoundVolumeSet", _s_set_SoundVolumeSet);
		Utils.RegisterFunc(L, -1, "Loop_Gap", _s_set_Loop_Gap);
		Utils.RegisterFunc(L, -1, "SoundAssetPaths", _s_set_SoundAssetPaths);
		Utils.RegisterFunc(L, -1, "serialId", _s_set_serialId);
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
				SoundComponent.PlaySoundParams o = new SoundComponent.PlaySoundParams();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlaySoundParams constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Time(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.Time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MuteInSoundGroup(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, playSoundParams.MuteInSoundGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Loop(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, playSoundParams.Loop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Priority(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, playSoundParams.Priority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VolumeInSoundGroup(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.VolumeInSoundGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShotVolumeScale(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.ShotVolumeScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FadeInSeconds(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.FadeInSeconds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FadeOutSeconds(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.FadeOutSeconds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Pitch(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.Pitch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PanStereo(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.PanStereo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SpatialBlend(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.SpatialBlend);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MaxDistance(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.MaxDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DopplerLevel(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.DopplerLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SoundVolumeSet(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, playSoundParams.SoundVolumeSet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Loop_Gap(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, playSoundParams.Loop_Gap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SoundAssetPaths(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, playSoundParams.SoundAssetPaths);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serialId(IntPtr L)
	{
		try
		{
			SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, playSoundParams.serialId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Time(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Time = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MuteInSoundGroup(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MuteInSoundGroup = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Loop(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Loop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Priority(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Priority = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_VolumeInSoundGroup(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).VolumeInSoundGroup = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ShotVolumeScale(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShotVolumeScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FadeInSeconds(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FadeInSeconds = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FadeOutSeconds(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FadeOutSeconds = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Pitch(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Pitch = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PanStereo(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PanStereo = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SpatialBlend(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SpatialBlend = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MaxDistance(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MaxDistance = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DopplerLevel(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DopplerLevel = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SoundVolumeSet(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SoundVolumeSet = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Loop_Gap(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Loop_Gap = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SoundAssetPaths(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SoundComponent.PlaySoundParams)objectTranslator.FastGetCSObj(L, 1)).SoundAssetPaths = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serialId(IntPtr L)
	{
		try
		{
			((SoundComponent.PlaySoundParams)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serialId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
