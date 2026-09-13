using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SoundComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SoundComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 58, 1, 0);
		Utils.RegisterFunc(L, -3, "HasMixerGroup_Str", _m_HasMixerGroup_Str);
		Utils.RegisterFunc(L, -3, "CheckSoundWrongOutput", _m_CheckSoundWrongOutput);
		Utils.RegisterFunc(L, -3, "IsAudioMixerLoaded", _m_IsAudioMixerLoaded);
		Utils.RegisterFunc(L, -3, "SyncAudioMixerUsing", _m_SyncAudioMixerUsing);
		Utils.RegisterFunc(L, -3, "TryAddAudioSourceAsset", _m_TryAddAudioSourceAsset);
		Utils.RegisterFunc(L, -3, "SpawnAudioSource", _m_SpawnAudioSource);
		Utils.RegisterFunc(L, -3, "UnspawnAudioSource", _m_UnspawnAudioSource);
		Utils.RegisterFunc(L, -3, "SetMasterGroupVolume", _m_SetMasterGroupVolume);
		Utils.RegisterFunc(L, -3, "SetAMBSoundPause", _m_SetAMBSoundPause);
		Utils.RegisterFunc(L, -3, "SetAMBSoundVolumeTo0", _m_SetAMBSoundVolumeTo0);
		Utils.RegisterFunc(L, -3, "ResetAMBSoundVolumeInternal", _m_ResetAMBSoundVolumeInternal);
		Utils.RegisterFunc(L, -3, "ResetAMBSoundVolume", _m_ResetAMBSoundVolume);
		Utils.RegisterFunc(L, -3, "CancelResetAmbSoundVolumeTimer", _m_CancelResetAmbSoundVolumeTimer);
		Utils.RegisterFunc(L, -3, "TryControlGlobalSound", _m_TryControlGlobalSound);
		Utils.RegisterFunc(L, -3, "TryResumeGlobalSoundControl", _m_TryResumeGlobalSoundControl);
		Utils.RegisterFunc(L, -3, "HasSoundGroup", _m_HasSoundGroup);
		Utils.RegisterFunc(L, -3, "SetSoundGroupMute", _m_SetSoundGroupMute);
		Utils.RegisterFunc(L, -3, "PlaySound", _m_PlaySound);
		Utils.RegisterFunc(L, -3, "OnBGMPlayFinished", _m_OnBGMPlayFinished);
		Utils.RegisterFunc(L, -3, "PlayMusic", _m_PlayMusic);
		Utils.RegisterFunc(L, -3, "PlayAMBSound", _m_PlayAMBSound);
		Utils.RegisterFunc(L, -3, "OnAMBSoundPlayFinished", _m_OnAMBSoundPlayFinished);
		Utils.RegisterFunc(L, -3, "PlayEffectFullPath", _m_PlayEffectFullPath);
		Utils.RegisterFunc(L, -3, "PlayEffect", _m_PlayEffect);
		Utils.RegisterFunc(L, -3, "PlayEffectCache", _m_PlayEffectCache);
		Utils.RegisterFunc(L, -3, "ReleaseEffect", _m_ReleaseEffect);
		Utils.RegisterFunc(L, -3, "PlaySpecialMusic", _m_PlaySpecialMusic);
		Utils.RegisterFunc(L, -3, "PlayBGMWithDspTime", _m_PlayBGMWithDspTime);
		Utils.RegisterFunc(L, -3, "PlayMainSceneBGMusic", _m_PlayMainSceneBGMusic);
		Utils.RegisterFunc(L, -3, "PlayBGMusicByName", _m_PlayBGMusicByName);
		Utils.RegisterFunc(L, -3, "PlayBGMusicByNameWithStartTime", _m_PlayBGMusicByNameWithStartTime);
		Utils.RegisterFunc(L, -3, "PlayGuideSceneBgMusic", _m_PlayGuideSceneBgMusic);
		Utils.RegisterFunc(L, -3, "PlayLoadingBgMusic", _m_PlayLoadingBgMusic);
		Utils.RegisterFunc(L, -3, "TryPlayLoadingSeasonBGMMusic", _m_TryPlayLoadingSeasonBGMMusic);
		Utils.RegisterFunc(L, -3, "PlayPveSceneBGMusic", _m_PlayPveSceneBGMusic);
		Utils.RegisterFunc(L, -3, "PlayPveSceneBGMusicLW", _m_PlayPveSceneBGMusicLW);
		Utils.RegisterFunc(L, -3, "PlayParkourBattleBGMusic", _m_PlayParkourBattleBGMusic);
		Utils.RegisterFunc(L, -3, "StopBGMusic", _m_StopBGMusic);
		Utils.RegisterFunc(L, -3, "StopAMBSound", _m_StopAMBSound);
		Utils.RegisterFunc(L, -3, "GetBGMusic", _m_GetBGMusic);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "StopSound", _m_StopSound);
		Utils.RegisterFunc(L, -3, "FadeOutAndPlayMusic", _m_FadeOutAndPlayMusic);
		Utils.RegisterFunc(L, -3, "StopAllSounds", _m_StopAllSounds);
		Utils.RegisterFunc(L, -3, "PauseSound", _m_PauseSound);
		Utils.RegisterFunc(L, -3, "ResumeSound", _m_ResumeSound);
		Utils.RegisterFunc(L, -3, "ChangeVolume", _m_ChangeVolume);
		Utils.RegisterFunc(L, -3, "ChangeGlobalSettingVolumeRatio", _m_ChangeGlobalSettingVolumeRatio);
		Utils.RegisterFunc(L, -3, "PlaySoundById", _m_PlaySoundById);
		Utils.RegisterFunc(L, -3, "PlaySoundByIdWithLimit", _m_PlaySoundByIdWithLimit);
		Utils.RegisterFunc(L, -3, "StopPlayLoopSoundWithLimit", _m_StopPlayLoopSoundWithLimit);
		Utils.RegisterFunc(L, -3, "GetAudioLength", _m_GetAudioLength);
		Utils.RegisterFunc(L, -3, "PlayTimelineSound", _m_PlayTimelineSound);
		Utils.RegisterFunc(L, -3, "GetDSPTime", _m_GetDSPTime);
		Utils.RegisterFunc(L, -3, "SetDspBufferSize", _m_SetDspBufferSize);
		Utils.RegisterFunc(L, -3, "GetDspBufferSize", _m_GetDspBufferSize);
		Utils.RegisterFunc(L, -3, "GetAudioSourceObjs", _m_GetAudioSourceObjs);
		Utils.RegisterFunc(L, -3, "GetAudioSourcePaths", _m_GetAudioSourcePaths);
		Utils.RegisterFunc(L, -2, "SoundGroupCount", _g_get_SoundGroupCount);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "GetSoundPathArray", _m_GetSoundPathArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSoundPath", _m_GetSoundPath_xlua_st_);
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
				SoundComponent o = new SoundComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasMixerGroup_Str(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			bool value = obj.HasMixerGroup_Str(name);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckSoundWrongOutput(IntPtr L)
	{
		try
		{
			bool value = ((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckSoundWrongOutput();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAudioMixerLoaded(IntPtr L)
	{
		try
		{
			bool value = ((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAudioMixerLoaded();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncAudioMixerUsing(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool use = Lua.lua_toboolean(L, 2);
			obj.SyncAudioMixerUsing(use);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryAddAudioSourceAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			Asset soundAsset = (Asset)objectTranslator.GetObject(L, 2, typeof(Asset));
			soundComponent.TryAddAudioSourceAsset(soundAsset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SpawnAudioSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent obj = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			AudioSource o = obj.SpawnAudioSource(path);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnspawnAudioSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			AudioSource audioSource = (AudioSource)objectTranslator.GetObject(L, 3, typeof(AudioSource));
			soundComponent.UnspawnAudioSource(path, audioSource);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMasterGroupVolume(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float masterGroupVolume = (float)Lua.lua_tonumber(L, 2);
			obj.SetMasterGroupVolume(masterGroupVolume);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAMBSoundPause(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool aMBSoundPause = Lua.lua_toboolean(L, 2);
			obj.SetAMBSoundPause(aMBSoundPause);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAMBSoundVolumeTo0(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAMBSoundVolumeTo0();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetAMBSoundVolumeInternal(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetAMBSoundVolumeInternal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetAMBSoundVolume(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetAMBSoundVolume();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelResetAmbSoundVolumeTimer(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CancelResetAmbSoundVolumeTimer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryControlGlobalSound(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			string soundGroupName = Lua.lua_tostring(L, 3);
			float volumeRatio = (float)Lua.lua_tonumber(L, 4);
			obj.TryControlGlobalSound(serialId, soundGroupName, volumeRatio);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryResumeGlobalSoundControl(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			obj.TryResumeGlobalSoundControl(serialId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasSoundGroup(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string soundGroupName = Lua.lua_tostring(L, 2);
			bool value = obj.HasSoundGroup(soundGroupName);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSoundGroupMute(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string soundGroupName = Lua.lua_tostring(L, 2);
				bool mute = Lua.lua_toboolean(L, 3);
				bool newGroup = Lua.lua_toboolean(L, 4);
				soundComponent.SetSoundGroupMute(soundGroupName, mute, newGroup);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string soundGroupName2 = Lua.lua_tostring(L, 2);
				bool mute2 = Lua.lua_toboolean(L, 3);
				soundComponent.SetSoundGroupMute(soundGroupName2, mute2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.SetSoundGroupMute!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySound(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string soundAssetName = Lua.lua_tostring(L, 2);
				string soundGroupName = Lua.lua_tostring(L, 3);
				float volume = (float)Lua.lua_tonumber(L, 4);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 5);
				int value = soundComponent.PlaySound(soundAssetName, soundGroupName, volume, soundVolumeSet);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string soundAssetName2 = Lua.lua_tostring(L, 2);
				string soundGroupName2 = Lua.lua_tostring(L, 3);
				float volume2 = (float)Lua.lua_tonumber(L, 4);
				int value2 = soundComponent.PlaySound(soundAssetName2, soundGroupName2, volume2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string soundAssetName3 = Lua.lua_tostring(L, 2);
				string soundGroupName3 = Lua.lua_tostring(L, 3);
				int value3 = soundComponent.PlaySound(soundAssetName3, soundGroupName3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && objectTranslator.Assignable<object>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				string soundAssetName4 = Lua.lua_tostring(L, 2);
				string soundGroupName4 = Lua.lua_tostring(L, 3);
				SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				object userData = objectTranslator.GetObject(L, 5, typeof(object));
				float startTime = (float)Lua.lua_tonumber(L, 6);
				Action onFinishCallback = objectTranslator.GetDelegate<Action>(L, 7);
				int value4 = soundComponent.PlaySound(soundAssetName4, soundGroupName4, playSoundParams, userData, startTime, onFinishCallback);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && objectTranslator.Assignable<object>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string soundAssetName5 = Lua.lua_tostring(L, 2);
				string soundGroupName5 = Lua.lua_tostring(L, 3);
				SoundComponent.PlaySoundParams playSoundParams2 = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				object userData2 = objectTranslator.GetObject(L, 5, typeof(object));
				float startTime2 = (float)Lua.lua_tonumber(L, 6);
				int value5 = soundComponent.PlaySound(soundAssetName5, soundGroupName5, playSoundParams2, userData2, startTime2);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && objectTranslator.Assignable<object>(L, 5))
			{
				string soundAssetName6 = Lua.lua_tostring(L, 2);
				string soundGroupName6 = Lua.lua_tostring(L, 3);
				SoundComponent.PlaySoundParams playSoundParams3 = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				object userData3 = objectTranslator.GetObject(L, 5, typeof(object));
				int value6 = soundComponent.PlaySound(soundAssetName6, soundGroupName6, playSoundParams3, userData3);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlaySound!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBGMPlayFinished(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBGMPlayFinished();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayMusic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 13 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12) && objectTranslator.Assignable<List<string>>(L, 13))
			{
				string name = Lua.lua_tostring(L, 2);
				bool loop = Lua.lua_toboolean(L, 3);
				float fadeInSeconds = (float)Lua.lua_tonumber(L, 4);
				float startTime = (float)Lua.lua_tonumber(L, 5);
				float volume = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 7);
				float speed = (float)Lua.lua_tonumber(L, 8);
				bool useSoundPath = Lua.lua_toboolean(L, 9);
				int reactive = Lua.xlua_tointeger(L, 10);
				int loop_gap = Lua.xlua_tointeger(L, 11);
				int pre_time = Lua.xlua_tointeger(L, 12);
				List<string> soundPathTable = (List<string>)objectTranslator.GetObject(L, 13, typeof(List<string>));
				int value = soundComponent.PlayMusic(name, loop, fadeInSeconds, startTime, volume, soundVolumeSet, speed, useSoundPath, reactive, loop_gap, pre_time, soundPathTable);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 12 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12))
			{
				string name2 = Lua.lua_tostring(L, 2);
				bool loop2 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds2 = (float)Lua.lua_tonumber(L, 4);
				float startTime2 = (float)Lua.lua_tonumber(L, 5);
				float volume2 = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet2 = (float)Lua.lua_tonumber(L, 7);
				float speed2 = (float)Lua.lua_tonumber(L, 8);
				bool useSoundPath2 = Lua.lua_toboolean(L, 9);
				int reactive2 = Lua.xlua_tointeger(L, 10);
				int loop_gap2 = Lua.xlua_tointeger(L, 11);
				int pre_time2 = Lua.xlua_tointeger(L, 12);
				int value2 = soundComponent.PlayMusic(name2, loop2, fadeInSeconds2, startTime2, volume2, soundVolumeSet2, speed2, useSoundPath2, reactive2, loop_gap2, pre_time2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 11 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				string name3 = Lua.lua_tostring(L, 2);
				bool loop3 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds3 = (float)Lua.lua_tonumber(L, 4);
				float startTime3 = (float)Lua.lua_tonumber(L, 5);
				float volume3 = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet3 = (float)Lua.lua_tonumber(L, 7);
				float speed3 = (float)Lua.lua_tonumber(L, 8);
				bool useSoundPath3 = Lua.lua_toboolean(L, 9);
				int reactive3 = Lua.xlua_tointeger(L, 10);
				int loop_gap3 = Lua.xlua_tointeger(L, 11);
				int value3 = soundComponent.PlayMusic(name3, loop3, fadeInSeconds3, startTime3, volume3, soundVolumeSet3, speed3, useSoundPath3, reactive3, loop_gap3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 10 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				string name4 = Lua.lua_tostring(L, 2);
				bool loop4 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds4 = (float)Lua.lua_tonumber(L, 4);
				float startTime4 = (float)Lua.lua_tonumber(L, 5);
				float volume4 = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet4 = (float)Lua.lua_tonumber(L, 7);
				float speed4 = (float)Lua.lua_tonumber(L, 8);
				bool useSoundPath4 = Lua.lua_toboolean(L, 9);
				int reactive4 = Lua.xlua_tointeger(L, 10);
				int value4 = soundComponent.PlayMusic(name4, loop4, fadeInSeconds4, startTime4, volume4, soundVolumeSet4, speed4, useSoundPath4, reactive4);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 9 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				string name5 = Lua.lua_tostring(L, 2);
				bool loop5 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds5 = (float)Lua.lua_tonumber(L, 4);
				float startTime5 = (float)Lua.lua_tonumber(L, 5);
				float volume5 = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet5 = (float)Lua.lua_tonumber(L, 7);
				float speed5 = (float)Lua.lua_tonumber(L, 8);
				bool useSoundPath5 = Lua.lua_toboolean(L, 9);
				int value5 = soundComponent.PlayMusic(name5, loop5, fadeInSeconds5, startTime5, volume5, soundVolumeSet5, speed5, useSoundPath5);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 8 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				string name6 = Lua.lua_tostring(L, 2);
				bool loop6 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds6 = (float)Lua.lua_tonumber(L, 4);
				float startTime6 = (float)Lua.lua_tonumber(L, 5);
				float volume6 = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet6 = (float)Lua.lua_tonumber(L, 7);
				float speed6 = (float)Lua.lua_tonumber(L, 8);
				int value6 = soundComponent.PlayMusic(name6, loop6, fadeInSeconds6, startTime6, volume6, soundVolumeSet6, speed6);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				string name7 = Lua.lua_tostring(L, 2);
				bool loop7 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds7 = (float)Lua.lua_tonumber(L, 4);
				float startTime7 = (float)Lua.lua_tonumber(L, 5);
				float volume7 = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet7 = (float)Lua.lua_tonumber(L, 7);
				int value7 = soundComponent.PlayMusic(name7, loop7, fadeInSeconds7, startTime7, volume7, soundVolumeSet7);
				Lua.xlua_pushinteger(L, value7);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string name8 = Lua.lua_tostring(L, 2);
				bool loop8 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds8 = (float)Lua.lua_tonumber(L, 4);
				float startTime8 = (float)Lua.lua_tonumber(L, 5);
				float volume8 = (float)Lua.lua_tonumber(L, 6);
				int value8 = soundComponent.PlayMusic(name8, loop8, fadeInSeconds8, startTime8, volume8);
				Lua.xlua_pushinteger(L, value8);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name9 = Lua.lua_tostring(L, 2);
				bool loop9 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds9 = (float)Lua.lua_tonumber(L, 4);
				float startTime9 = (float)Lua.lua_tonumber(L, 5);
				int value9 = soundComponent.PlayMusic(name9, loop9, fadeInSeconds9, startTime9);
				Lua.xlua_pushinteger(L, value9);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string name10 = Lua.lua_tostring(L, 2);
				bool loop10 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds10 = (float)Lua.lua_tonumber(L, 4);
				int value10 = soundComponent.PlayMusic(name10, loop10, fadeInSeconds10);
				Lua.xlua_pushinteger(L, value10);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string name11 = Lua.lua_tostring(L, 2);
				bool loop11 = Lua.lua_toboolean(L, 3);
				int value11 = soundComponent.PlayMusic(name11, loop11);
				Lua.xlua_pushinteger(L, value11);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name12 = Lua.lua_tostring(L, 2);
				int value12 = soundComponent.PlayMusic(name12);
				Lua.xlua_pushinteger(L, value12);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayMusic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAMBSound(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 8 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<List<string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				string name = Lua.lua_tostring(L, 2);
				bool useSoundPath = Lua.lua_toboolean(L, 3);
				List<string> soundPathTable = (List<string>)objectTranslator.GetObject(L, 4, typeof(List<string>));
				int reactive = Lua.xlua_tointeger(L, 5);
				int loop_gap = Lua.xlua_tointeger(L, 6);
				int pre_time = Lua.xlua_tointeger(L, 7);
				float speed = (float)Lua.lua_tonumber(L, 8);
				int value = soundComponent.PlayAMBSound(name, useSoundPath, soundPathTable, reactive, loop_gap, pre_time, speed);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<List<string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				string name2 = Lua.lua_tostring(L, 2);
				bool useSoundPath2 = Lua.lua_toboolean(L, 3);
				List<string> soundPathTable2 = (List<string>)objectTranslator.GetObject(L, 4, typeof(List<string>));
				int reactive2 = Lua.xlua_tointeger(L, 5);
				int loop_gap2 = Lua.xlua_tointeger(L, 6);
				int pre_time2 = Lua.xlua_tointeger(L, 7);
				int value2 = soundComponent.PlayAMBSound(name2, useSoundPath2, soundPathTable2, reactive2, loop_gap2, pre_time2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<List<string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string name3 = Lua.lua_tostring(L, 2);
				bool useSoundPath3 = Lua.lua_toboolean(L, 3);
				List<string> soundPathTable3 = (List<string>)objectTranslator.GetObject(L, 4, typeof(List<string>));
				int reactive3 = Lua.xlua_tointeger(L, 5);
				int loop_gap3 = Lua.xlua_tointeger(L, 6);
				int value3 = soundComponent.PlayAMBSound(name3, useSoundPath3, soundPathTable3, reactive3, loop_gap3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<List<string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name4 = Lua.lua_tostring(L, 2);
				bool useSoundPath4 = Lua.lua_toboolean(L, 3);
				List<string> soundPathTable4 = (List<string>)objectTranslator.GetObject(L, 4, typeof(List<string>));
				int reactive4 = Lua.xlua_tointeger(L, 5);
				int value4 = soundComponent.PlayAMBSound(name4, useSoundPath4, soundPathTable4, reactive4);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<List<string>>(L, 4))
			{
				string name5 = Lua.lua_tostring(L, 2);
				bool useSoundPath5 = Lua.lua_toboolean(L, 3);
				List<string> soundPathTable5 = (List<string>)objectTranslator.GetObject(L, 4, typeof(List<string>));
				int value5 = soundComponent.PlayAMBSound(name5, useSoundPath5, soundPathTable5);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayAMBSound!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAMBSoundPlayFinished(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAMBSoundPlayFinished();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayEffectFullPath(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string path = Lua.lua_tostring(L, 2);
				float volumeScale = (float)Lua.lua_tonumber(L, 3);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 4);
				int value = soundComponent.PlayEffectFullPath(path, volumeScale, soundVolumeSet);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string path2 = Lua.lua_tostring(L, 2);
				float volumeScale2 = (float)Lua.lua_tonumber(L, 3);
				int value2 = soundComponent.PlayEffectFullPath(path2, volumeScale2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string path3 = Lua.lua_tostring(L, 2);
				int value3 = soundComponent.PlayEffectFullPath(path3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayEffectFullPath!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayEffect(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string name = Lua.lua_tostring(L, 2);
				float volumeScale = (float)Lua.lua_tonumber(L, 3);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 4);
				int value = soundComponent.PlayEffect(name, volumeScale, soundVolumeSet);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				float volumeScale2 = (float)Lua.lua_tonumber(L, 3);
				int value2 = soundComponent.PlayEffect(name2, volumeScale2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name3 = Lua.lua_tostring(L, 2);
				int value3 = soundComponent.PlayEffect(name3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayEffect!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayEffectCache(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string soundAssetName = Lua.lua_tostring(L, 2);
				float volumeScale = (float)Lua.lua_tonumber(L, 3);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 4);
				int value = soundComponent.PlayEffectCache(soundAssetName, volumeScale, soundVolumeSet);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string soundAssetName2 = Lua.lua_tostring(L, 2);
				float volumeScale2 = (float)Lua.lua_tonumber(L, 3);
				int value2 = soundComponent.PlayEffectCache(soundAssetName2, volumeScale2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string soundAssetName3 = Lua.lua_tostring(L, 2);
				int value3 = soundComponent.PlayEffectCache(soundAssetName3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayEffectCache!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReleaseEffect(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			bool value = obj.ReleaseEffect(name);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySpecialMusic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Action<float, string>>(L, 5))
			{
				string strMusicPath = Lua.lua_tostring(L, 2);
				bool loop = Lua.lua_toboolean(L, 3);
				float fadeIn = (float)Lua.lua_tonumber(L, 4);
				Action<float, string> onGetAudioLength = objectTranslator.GetDelegate<Action<float, string>>(L, 5);
				soundComponent.PlaySpecialMusic(strMusicPath, loop, fadeIn, onGetAudioLength);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string strMusicPath2 = Lua.lua_tostring(L, 2);
				bool loop2 = Lua.lua_toboolean(L, 3);
				float fadeIn2 = (float)Lua.lua_tonumber(L, 4);
				soundComponent.PlaySpecialMusic(strMusicPath2, loop2, fadeIn2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlaySpecialMusic!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBGMWithDspTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Func<float>>(L, 5) && objectTranslator.Assignable<Action>(L, 6))
			{
				string strMusicPath = Lua.lua_tostring(L, 2);
				bool loop = Lua.lua_toboolean(L, 3);
				float fadeIn = (float)Lua.lua_tonumber(L, 4);
				Func<float> getDspTimeFunc = objectTranslator.GetDelegate<Func<float>>(L, 5);
				Action onMusicLoadFinish = objectTranslator.GetDelegate<Action>(L, 6);
				soundComponent.PlayBGMWithDspTime(strMusicPath, loop, fadeIn, getDspTimeFunc, onMusicLoadFinish);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Func<float>>(L, 5))
			{
				string strMusicPath2 = Lua.lua_tostring(L, 2);
				bool loop2 = Lua.lua_toboolean(L, 3);
				float fadeIn2 = (float)Lua.lua_tonumber(L, 4);
				Func<float> getDspTimeFunc2 = objectTranslator.GetDelegate<Func<float>>(L, 5);
				soundComponent.PlayBGMWithDspTime(strMusicPath2, loop2, fadeIn2, getDspTimeFunc2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayBGMWithDspTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayMainSceneBGMusic(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayMainSceneBGMusic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBGMusicByName(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string nameStr = Lua.lua_tostring(L, 2);
				float volume = (float)Lua.lua_tonumber(L, 3);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 4);
				soundComponent.PlayBGMusicByName(nameStr, volume, soundVolumeSet);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string nameStr2 = Lua.lua_tostring(L, 2);
				float volume2 = (float)Lua.lua_tonumber(L, 3);
				soundComponent.PlayBGMusicByName(nameStr2, volume2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string nameStr3 = Lua.lua_tostring(L, 2);
				soundComponent.PlayBGMusicByName(nameStr3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayBGMusicByName!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayBGMusicByNameWithStartTime(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 12 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 12))
			{
				string nameStr = Lua.lua_tostring(L, 2);
				float startTime = (float)Lua.lua_tonumber(L, 3);
				bool isloop = Lua.lua_toboolean(L, 4);
				float volume = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 6);
				float fadeTime = (float)Lua.lua_tonumber(L, 7);
				float speed = (float)Lua.lua_tonumber(L, 8);
				bool usePath = Lua.lua_toboolean(L, 9);
				int reactive = Lua.xlua_tointeger(L, 10);
				int loop_gap = Lua.xlua_tointeger(L, 11);
				int pre_time = Lua.xlua_tointeger(L, 12);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr, startTime, isloop, volume, soundVolumeSet, fadeTime, speed, usePath, reactive, loop_gap, pre_time);
				return 0;
			}
			if (num == 11 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				string nameStr2 = Lua.lua_tostring(L, 2);
				float startTime2 = (float)Lua.lua_tonumber(L, 3);
				bool isloop2 = Lua.lua_toboolean(L, 4);
				float volume2 = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet2 = (float)Lua.lua_tonumber(L, 6);
				float fadeTime2 = (float)Lua.lua_tonumber(L, 7);
				float speed2 = (float)Lua.lua_tonumber(L, 8);
				bool usePath2 = Lua.lua_toboolean(L, 9);
				int reactive2 = Lua.xlua_tointeger(L, 10);
				int loop_gap2 = Lua.xlua_tointeger(L, 11);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr2, startTime2, isloop2, volume2, soundVolumeSet2, fadeTime2, speed2, usePath2, reactive2, loop_gap2);
				return 0;
			}
			if (num == 10 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				string nameStr3 = Lua.lua_tostring(L, 2);
				float startTime3 = (float)Lua.lua_tonumber(L, 3);
				bool isloop3 = Lua.lua_toboolean(L, 4);
				float volume3 = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet3 = (float)Lua.lua_tonumber(L, 6);
				float fadeTime3 = (float)Lua.lua_tonumber(L, 7);
				float speed3 = (float)Lua.lua_tonumber(L, 8);
				bool usePath3 = Lua.lua_toboolean(L, 9);
				int reactive3 = Lua.xlua_tointeger(L, 10);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr3, startTime3, isloop3, volume3, soundVolumeSet3, fadeTime3, speed3, usePath3, reactive3);
				return 0;
			}
			if (num == 9 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				string nameStr4 = Lua.lua_tostring(L, 2);
				float startTime4 = (float)Lua.lua_tonumber(L, 3);
				bool isloop4 = Lua.lua_toboolean(L, 4);
				float volume4 = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet4 = (float)Lua.lua_tonumber(L, 6);
				float fadeTime4 = (float)Lua.lua_tonumber(L, 7);
				float speed4 = (float)Lua.lua_tonumber(L, 8);
				bool usePath4 = Lua.lua_toboolean(L, 9);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr4, startTime4, isloop4, volume4, soundVolumeSet4, fadeTime4, speed4, usePath4);
				return 0;
			}
			if (num == 8 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				string nameStr5 = Lua.lua_tostring(L, 2);
				float startTime5 = (float)Lua.lua_tonumber(L, 3);
				bool isloop5 = Lua.lua_toboolean(L, 4);
				float volume5 = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet5 = (float)Lua.lua_tonumber(L, 6);
				float fadeTime5 = (float)Lua.lua_tonumber(L, 7);
				float speed5 = (float)Lua.lua_tonumber(L, 8);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr5, startTime5, isloop5, volume5, soundVolumeSet5, fadeTime5, speed5);
				return 0;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				string nameStr6 = Lua.lua_tostring(L, 2);
				float startTime6 = (float)Lua.lua_tonumber(L, 3);
				bool isloop6 = Lua.lua_toboolean(L, 4);
				float volume6 = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet6 = (float)Lua.lua_tonumber(L, 6);
				float fadeTime6 = (float)Lua.lua_tonumber(L, 7);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr6, startTime6, isloop6, volume6, soundVolumeSet6, fadeTime6);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string nameStr7 = Lua.lua_tostring(L, 2);
				float startTime7 = (float)Lua.lua_tonumber(L, 3);
				bool isloop7 = Lua.lua_toboolean(L, 4);
				float volume7 = (float)Lua.lua_tonumber(L, 5);
				float soundVolumeSet7 = (float)Lua.lua_tonumber(L, 6);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr7, startTime7, isloop7, volume7, soundVolumeSet7);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string nameStr8 = Lua.lua_tostring(L, 2);
				float startTime8 = (float)Lua.lua_tonumber(L, 3);
				bool isloop8 = Lua.lua_toboolean(L, 4);
				float volume8 = (float)Lua.lua_tonumber(L, 5);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr8, startTime8, isloop8, volume8);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string nameStr9 = Lua.lua_tostring(L, 2);
				float startTime9 = (float)Lua.lua_tonumber(L, 3);
				bool isloop9 = Lua.lua_toboolean(L, 4);
				soundComponent.PlayBGMusicByNameWithStartTime(nameStr9, startTime9, isloop9);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayBGMusicByNameWithStartTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayGuideSceneBgMusic(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayGuideSceneBgMusic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayLoadingBgMusic(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayLoadingBgMusic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryPlayLoadingSeasonBGMMusic(IntPtr L)
	{
		try
		{
			bool value = ((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TryPlayLoadingSeasonBGMMusic();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayPveSceneBGMusic(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayPveSceneBGMusic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayPveSceneBGMusicLW(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayPveSceneBGMusicLW();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayParkourBattleBGMusic(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayParkourBattleBGMusic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopBGMusic(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopBGMusic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAMBSound(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAMBSound();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBGMusic(IntPtr L)
	{
		try
		{
			int bGMusic = ((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetBGMusic();
			Lua.xlua_pushinteger(L, bGMusic);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float elapseSeconds = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(elapseSeconds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopSound(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			bool value = obj.StopSound(serialId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FadeOutAndPlayMusic(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			float time = (float)Lua.lua_tonumber(L, 3);
			bool value = obj.FadeOutAndPlayMusic(serialId, time);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAllSounds(IntPtr L)
	{
		try
		{
			((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAllSounds();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PauseSound(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			obj.PauseSound(serialId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResumeSound(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			obj.ResumeSound(serialId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeVolume(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string soundGroupName = Lua.lua_tostring(L, 2);
			float toVolume = (float)Lua.lua_tonumber(L, 3);
			float time = (float)Lua.lua_tonumber(L, 4);
			obj.ChangeVolume(soundGroupName, toVolume, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeGlobalSettingVolumeRatio(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string soundGroupName = Lua.lua_tostring(L, 2);
			float toVolume = (float)Lua.lua_tonumber(L, 3);
			float time = (float)Lua.lua_tonumber(L, 4);
			obj.ChangeGlobalSettingVolumeRatio(soundGroupName, toVolume, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySoundById(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int soundId = Lua.xlua_tointeger(L, 2);
			string soundGroupName = Lua.lua_tostring(L, 3);
			obj.PlaySoundById(soundId, soundGroupName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySoundByIdWithLimit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent obj = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			int soundId = Lua.xlua_tointeger(L, 2);
			LoopTimerSound o = obj.PlaySoundByIdWithLimit(soundId);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopPlayLoopSoundWithLimit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			LoopTimerSound timer = (LoopTimerSound)objectTranslator.GetObject(L, 2, typeof(LoopTimerSound));
			soundComponent.StopPlayLoopSoundWithLimit(timer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioLength(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			string soundAssetName = Lua.lua_tostring(L, 2);
			Action<float> onGetAudioLength = objectTranslator.GetDelegate<Action<float>>(L, 3);
			soundComponent.GetAudioLength(soundAssetName, onGetAudioLength);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayTimelineSound(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				string name = Lua.lua_tostring(L, 2);
				bool loop = Lua.lua_toboolean(L, 3);
				float fadeInSeconds = (float)Lua.lua_tonumber(L, 4);
				float startTime = (float)Lua.lua_tonumber(L, 5);
				float volume = (float)Lua.lua_tonumber(L, 6);
				float soundVolumeSet = (float)Lua.lua_tonumber(L, 7);
				int value = soundComponent.PlayTimelineSound(name, loop, fadeInSeconds, startTime, volume, soundVolumeSet);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string name2 = Lua.lua_tostring(L, 2);
				bool loop2 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds2 = (float)Lua.lua_tonumber(L, 4);
				float startTime2 = (float)Lua.lua_tonumber(L, 5);
				float volume2 = (float)Lua.lua_tonumber(L, 6);
				int value2 = soundComponent.PlayTimelineSound(name2, loop2, fadeInSeconds2, startTime2, volume2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name3 = Lua.lua_tostring(L, 2);
				bool loop3 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds3 = (float)Lua.lua_tonumber(L, 4);
				float startTime3 = (float)Lua.lua_tonumber(L, 5);
				int value3 = soundComponent.PlayTimelineSound(name3, loop3, fadeInSeconds3, startTime3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string name4 = Lua.lua_tostring(L, 2);
				bool loop4 = Lua.lua_toboolean(L, 3);
				float fadeInSeconds4 = (float)Lua.lua_tonumber(L, 4);
				int value4 = soundComponent.PlayTimelineSound(name4, loop4, fadeInSeconds4);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string name5 = Lua.lua_tostring(L, 2);
				bool loop5 = Lua.lua_toboolean(L, 3);
				int value5 = soundComponent.PlayTimelineSound(name5, loop5);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name6 = Lua.lua_tostring(L, 2);
				int value6 = soundComponent.PlayTimelineSound(name6);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.PlayTimelineSound!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDSPTime(IntPtr L)
	{
		try
		{
			double dSPTime = ((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDSPTime();
			Lua.lua_pushnumber(L, dSPTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSoundPathArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string[] soundPathArray = SoundComponent.GetSoundPathArray(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, soundPathArray);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSoundPath_xlua_st_(IntPtr L)
	{
		try
		{
			string soundPath = SoundComponent.GetSoundPath(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushstring(L, soundPath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDspBufferSize(IntPtr L)
	{
		try
		{
			SoundComponent obj = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dspBufferSize = Lua.xlua_tointeger(L, 2);
			obj.SetDspBufferSize(dspBufferSize);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDspBufferSize(IntPtr L)
	{
		try
		{
			int dspBufferSize = ((SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDspBufferSize();
			Lua.xlua_pushinteger(L, dspBufferSize);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioSourceObjs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			Action<List<GameObject>> onGetObjs = objectTranslator.GetDelegate<Action<List<GameObject>>>(L, 2);
			soundComponent.GetAudioSourceObjs(onGetObjs);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioSourcePaths(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent soundComponent = (SoundComponent)objectTranslator.FastGetCSObj(L, 1);
			Action<List<string>> onGetPaths = objectTranslator.GetDelegate<Action<List<string>>>(L, 2);
			soundComponent.GetAudioSourcePaths(onGetPaths);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SoundGroupCount(IntPtr L)
	{
		try
		{
			SoundComponent soundComponent = (SoundComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, soundComponent.SoundGroupCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
