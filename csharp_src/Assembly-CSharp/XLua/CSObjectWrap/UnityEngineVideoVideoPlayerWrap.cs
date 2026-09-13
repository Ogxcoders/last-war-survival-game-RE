using System;
using UnityEngine;
using UnityEngine.Video;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineVideoVideoPlayerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VideoPlayer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 24, 43, 24);
		Utils.RegisterFunc(L, -3, "Prepare", _m_Prepare);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "Pause", _m_Pause);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "StepForward", _m_StepForward);
		Utils.RegisterFunc(L, -3, "GetAudioLanguageCode", _m_GetAudioLanguageCode);
		Utils.RegisterFunc(L, -3, "GetAudioChannelCount", _m_GetAudioChannelCount);
		Utils.RegisterFunc(L, -3, "GetAudioSampleRate", _m_GetAudioSampleRate);
		Utils.RegisterFunc(L, -3, "EnableAudioTrack", _m_EnableAudioTrack);
		Utils.RegisterFunc(L, -3, "IsAudioTrackEnabled", _m_IsAudioTrackEnabled);
		Utils.RegisterFunc(L, -3, "GetDirectAudioVolume", _m_GetDirectAudioVolume);
		Utils.RegisterFunc(L, -3, "SetDirectAudioVolume", _m_SetDirectAudioVolume);
		Utils.RegisterFunc(L, -3, "GetDirectAudioMute", _m_GetDirectAudioMute);
		Utils.RegisterFunc(L, -3, "SetDirectAudioMute", _m_SetDirectAudioMute);
		Utils.RegisterFunc(L, -3, "GetTargetAudioSource", _m_GetTargetAudioSource);
		Utils.RegisterFunc(L, -3, "SetTargetAudioSource", _m_SetTargetAudioSource);
		Utils.RegisterFunc(L, -3, "prepareCompleted", _e_prepareCompleted);
		Utils.RegisterFunc(L, -3, "loopPointReached", _e_loopPointReached);
		Utils.RegisterFunc(L, -3, "started", _e_started);
		Utils.RegisterFunc(L, -3, "frameDropped", _e_frameDropped);
		Utils.RegisterFunc(L, -3, "errorReceived", _e_errorReceived);
		Utils.RegisterFunc(L, -3, "seekCompleted", _e_seekCompleted);
		Utils.RegisterFunc(L, -3, "clockResyncOccurred", _e_clockResyncOccurred);
		Utils.RegisterFunc(L, -3, "frameReady", _e_frameReady);
		Utils.RegisterFunc(L, -2, "source", _g_get_source);
		Utils.RegisterFunc(L, -2, "url", _g_get_url);
		Utils.RegisterFunc(L, -2, "clip", _g_get_clip);
		Utils.RegisterFunc(L, -2, "renderMode", _g_get_renderMode);
		Utils.RegisterFunc(L, -2, "targetCamera", _g_get_targetCamera);
		Utils.RegisterFunc(L, -2, "targetTexture", _g_get_targetTexture);
		Utils.RegisterFunc(L, -2, "targetMaterialRenderer", _g_get_targetMaterialRenderer);
		Utils.RegisterFunc(L, -2, "targetMaterialProperty", _g_get_targetMaterialProperty);
		Utils.RegisterFunc(L, -2, "aspectRatio", _g_get_aspectRatio);
		Utils.RegisterFunc(L, -2, "targetCameraAlpha", _g_get_targetCameraAlpha);
		Utils.RegisterFunc(L, -2, "targetCamera3DLayout", _g_get_targetCamera3DLayout);
		Utils.RegisterFunc(L, -2, "texture", _g_get_texture);
		Utils.RegisterFunc(L, -2, "isPrepared", _g_get_isPrepared);
		Utils.RegisterFunc(L, -2, "waitForFirstFrame", _g_get_waitForFirstFrame);
		Utils.RegisterFunc(L, -2, "playOnAwake", _g_get_playOnAwake);
		Utils.RegisterFunc(L, -2, "isPlaying", _g_get_isPlaying);
		Utils.RegisterFunc(L, -2, "isPaused", _g_get_isPaused);
		Utils.RegisterFunc(L, -2, "canSetTime", _g_get_canSetTime);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "frame", _g_get_frame);
		Utils.RegisterFunc(L, -2, "clockTime", _g_get_clockTime);
		Utils.RegisterFunc(L, -2, "canStep", _g_get_canStep);
		Utils.RegisterFunc(L, -2, "canSetPlaybackSpeed", _g_get_canSetPlaybackSpeed);
		Utils.RegisterFunc(L, -2, "playbackSpeed", _g_get_playbackSpeed);
		Utils.RegisterFunc(L, -2, "isLooping", _g_get_isLooping);
		Utils.RegisterFunc(L, -2, "canSetTimeSource", _g_get_canSetTimeSource);
		Utils.RegisterFunc(L, -2, "timeSource", _g_get_timeSource);
		Utils.RegisterFunc(L, -2, "timeReference", _g_get_timeReference);
		Utils.RegisterFunc(L, -2, "externalReferenceTime", _g_get_externalReferenceTime);
		Utils.RegisterFunc(L, -2, "canSetSkipOnDrop", _g_get_canSetSkipOnDrop);
		Utils.RegisterFunc(L, -2, "skipOnDrop", _g_get_skipOnDrop);
		Utils.RegisterFunc(L, -2, "frameCount", _g_get_frameCount);
		Utils.RegisterFunc(L, -2, "frameRate", _g_get_frameRate);
		Utils.RegisterFunc(L, -2, "length", _g_get_length);
		Utils.RegisterFunc(L, -2, "width", _g_get_width);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "pixelAspectRatioNumerator", _g_get_pixelAspectRatioNumerator);
		Utils.RegisterFunc(L, -2, "pixelAspectRatioDenominator", _g_get_pixelAspectRatioDenominator);
		Utils.RegisterFunc(L, -2, "audioTrackCount", _g_get_audioTrackCount);
		Utils.RegisterFunc(L, -2, "controlledAudioTrackCount", _g_get_controlledAudioTrackCount);
		Utils.RegisterFunc(L, -2, "audioOutputMode", _g_get_audioOutputMode);
		Utils.RegisterFunc(L, -2, "canSetDirectAudioVolume", _g_get_canSetDirectAudioVolume);
		Utils.RegisterFunc(L, -2, "sendFrameReadyEvents", _g_get_sendFrameReadyEvents);
		Utils.RegisterFunc(L, -1, "source", _s_set_source);
		Utils.RegisterFunc(L, -1, "url", _s_set_url);
		Utils.RegisterFunc(L, -1, "clip", _s_set_clip);
		Utils.RegisterFunc(L, -1, "renderMode", _s_set_renderMode);
		Utils.RegisterFunc(L, -1, "targetCamera", _s_set_targetCamera);
		Utils.RegisterFunc(L, -1, "targetTexture", _s_set_targetTexture);
		Utils.RegisterFunc(L, -1, "targetMaterialRenderer", _s_set_targetMaterialRenderer);
		Utils.RegisterFunc(L, -1, "targetMaterialProperty", _s_set_targetMaterialProperty);
		Utils.RegisterFunc(L, -1, "aspectRatio", _s_set_aspectRatio);
		Utils.RegisterFunc(L, -1, "targetCameraAlpha", _s_set_targetCameraAlpha);
		Utils.RegisterFunc(L, -1, "targetCamera3DLayout", _s_set_targetCamera3DLayout);
		Utils.RegisterFunc(L, -1, "waitForFirstFrame", _s_set_waitForFirstFrame);
		Utils.RegisterFunc(L, -1, "playOnAwake", _s_set_playOnAwake);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "frame", _s_set_frame);
		Utils.RegisterFunc(L, -1, "playbackSpeed", _s_set_playbackSpeed);
		Utils.RegisterFunc(L, -1, "isLooping", _s_set_isLooping);
		Utils.RegisterFunc(L, -1, "timeSource", _s_set_timeSource);
		Utils.RegisterFunc(L, -1, "timeReference", _s_set_timeReference);
		Utils.RegisterFunc(L, -1, "externalReferenceTime", _s_set_externalReferenceTime);
		Utils.RegisterFunc(L, -1, "skipOnDrop", _s_set_skipOnDrop);
		Utils.RegisterFunc(L, -1, "controlledAudioTrackCount", _s_set_controlledAudioTrackCount);
		Utils.RegisterFunc(L, -1, "audioOutputMode", _s_set_audioOutputMode);
		Utils.RegisterFunc(L, -1, "sendFrameReadyEvents", _s_set_sendFrameReadyEvents);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "controlledAudioTrackMaxCount", _g_get_controlledAudioTrackMaxCount);
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
				VideoPlayer o = new VideoPlayer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Prepare(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Prepare();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Play();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Pause();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Stop(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Stop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StepForward(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StepForward();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioLanguageCode(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			string audioLanguageCode = obj.GetAudioLanguageCode(trackIndex);
			Lua.lua_pushstring(L, audioLanguageCode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioChannelCount(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			ushort audioChannelCount = obj.GetAudioChannelCount(trackIndex);
			Lua.xlua_pushinteger(L, audioChannelCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioSampleRate(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			uint audioSampleRate = obj.GetAudioSampleRate(trackIndex);
			Lua.xlua_pushuint(L, audioSampleRate);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableAudioTrack(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			bool enabled = Lua.lua_toboolean(L, 3);
			obj.EnableAudioTrack(trackIndex, enabled);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAudioTrackEnabled(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			bool value = obj.IsAudioTrackEnabled(trackIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDirectAudioVolume(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			float directAudioVolume = obj.GetDirectAudioVolume(trackIndex);
			Lua.lua_pushnumber(L, directAudioVolume);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDirectAudioVolume(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			float volume = (float)Lua.lua_tonumber(L, 3);
			obj.SetDirectAudioVolume(trackIndex, volume);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDirectAudioMute(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			bool directAudioMute = obj.GetDirectAudioMute(trackIndex);
			Lua.lua_pushboolean(L, directAudioMute);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDirectAudioMute(IntPtr L)
	{
		try
		{
			VideoPlayer obj = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			bool mute = Lua.lua_toboolean(L, 3);
			obj.SetDirectAudioMute(trackIndex, mute);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTargetAudioSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer obj = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			AudioSource targetAudioSource = obj.GetTargetAudioSource(trackIndex);
			objectTranslator.Push(L, targetAudioSource);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTargetAudioSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			ushort trackIndex = (ushort)Lua.xlua_tointeger(L, 2);
			AudioSource source = (AudioSource)objectTranslator.GetObject(L, 3, typeof(AudioSource));
			videoPlayer.SetTargetAudioSource(trackIndex, source);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_source(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.source);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_url(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, videoPlayer.url);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.clip);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.renderMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.targetCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.targetTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetMaterialRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.targetMaterialRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetMaterialProperty(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, videoPlayer.targetMaterialProperty);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aspectRatio(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.aspectRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetCameraAlpha(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.targetCameraAlpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetCamera3DLayout(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.targetCamera3DLayout);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_texture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.texture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPrepared(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.isPrepared);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_waitForFirstFrame(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.waitForFirstFrame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playOnAwake(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.playOnAwake);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPlaying(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.isPlaying);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPaused(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.isPaused);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canSetTime(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.canSetTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frame(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, videoPlayer.frame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clockTime(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.clockTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canStep(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.canStep);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canSetPlaybackSpeed(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.canSetPlaybackSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playbackSpeed(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.playbackSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isLooping(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.isLooping);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canSetTimeSource(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.canSetTimeSource);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.timeSource);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeReference(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.timeReference);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_externalReferenceTime(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.externalReferenceTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canSetSkipOnDrop(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.canSetSkipOnDrop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skipOnDrop(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.skipOnDrop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameCount(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, videoPlayer.frameCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameRate(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.frameRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_length(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, videoPlayer.length);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_width(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushuint(L, videoPlayer.width);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushuint(L, videoPlayer.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelAspectRatioNumerator(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushuint(L, videoPlayer.pixelAspectRatioNumerator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelAspectRatioDenominator(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushuint(L, videoPlayer.pixelAspectRatioDenominator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_audioTrackCount(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, videoPlayer.audioTrackCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_controlledAudioTrackMaxCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, VideoPlayer.controlledAudioTrackMaxCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_controlledAudioTrackCount(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, videoPlayer.controlledAudioTrackCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_audioOutputMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, videoPlayer.audioOutputMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canSetDirectAudioVolume(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.canSetDirectAudioVolume);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sendFrameReadyEvents(IntPtr L)
	{
		try
		{
			VideoPlayer videoPlayer = (VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, videoPlayer.sendFrameReadyEvents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_source(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoSource v);
			videoPlayer.source = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_url(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).url = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((VideoPlayer)objectTranslator.FastGetCSObj(L, 1)).clip = (VideoClip)objectTranslator.GetObject(L, 2, typeof(VideoClip));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoRenderMode v);
			videoPlayer.renderMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((VideoPlayer)objectTranslator.FastGetCSObj(L, 1)).targetCamera = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((VideoPlayer)objectTranslator.FastGetCSObj(L, 1)).targetTexture = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetMaterialRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((VideoPlayer)objectTranslator.FastGetCSObj(L, 1)).targetMaterialRenderer = (Renderer)objectTranslator.GetObject(L, 2, typeof(Renderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetMaterialProperty(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetMaterialProperty = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aspectRatio(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoAspectRatio v);
			videoPlayer.aspectRatio = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetCameraAlpha(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetCameraAlpha = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetCamera3DLayout(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Video3DLayout v);
			videoPlayer.targetCamera3DLayout = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_waitForFirstFrame(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).waitForFirstFrame = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playOnAwake(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playOnAwake = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_time(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).time = Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_frame(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).frame = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playbackSpeed(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playbackSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isLooping(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isLooping = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoTimeSource v);
			videoPlayer.timeSource = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeReference(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoTimeReference v);
			videoPlayer.timeReference = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_externalReferenceTime(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).externalReferenceTime = Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skipOnDrop(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).skipOnDrop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_controlledAudioTrackCount(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).controlledAudioTrackCount = (ushort)Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_audioOutputMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoAudioOutputMode v);
			videoPlayer.audioOutputMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sendFrameReadyEvents(IntPtr L)
	{
		try
		{
			((VideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sendFrameReadyEvents = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_prepareCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.EventHandler eventHandler = objectTranslator.GetDelegate<VideoPlayer.EventHandler>(L, 3);
			if (eventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.EventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.prepareCompleted += eventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.prepareCompleted -= eventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.prepareCompleted!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_loopPointReached(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.EventHandler eventHandler = objectTranslator.GetDelegate<VideoPlayer.EventHandler>(L, 3);
			if (eventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.EventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.loopPointReached += eventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.loopPointReached -= eventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.loopPointReached!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_started(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.EventHandler eventHandler = objectTranslator.GetDelegate<VideoPlayer.EventHandler>(L, 3);
			if (eventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.EventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.started += eventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.started -= eventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.started!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_frameDropped(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.EventHandler eventHandler = objectTranslator.GetDelegate<VideoPlayer.EventHandler>(L, 3);
			if (eventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.EventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.frameDropped += eventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.frameDropped -= eventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.frameDropped!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_errorReceived(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.ErrorEventHandler errorEventHandler = objectTranslator.GetDelegate<VideoPlayer.ErrorEventHandler>(L, 3);
			if (errorEventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.ErrorEventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.errorReceived += errorEventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.errorReceived -= errorEventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.errorReceived!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_seekCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.EventHandler eventHandler = objectTranslator.GetDelegate<VideoPlayer.EventHandler>(L, 3);
			if (eventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.EventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.seekCompleted += eventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.seekCompleted -= eventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.seekCompleted!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_clockResyncOccurred(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.TimeEventHandler timeEventHandler = objectTranslator.GetDelegate<VideoPlayer.TimeEventHandler>(L, 3);
			if (timeEventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.TimeEventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.clockResyncOccurred += timeEventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.clockResyncOccurred -= timeEventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.clockResyncOccurred!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_frameReady(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VideoPlayer videoPlayer = (VideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer.FrameReadyEventHandler frameReadyEventHandler = objectTranslator.GetDelegate<VideoPlayer.FrameReadyEventHandler>(L, 3);
			if (frameReadyEventHandler == null)
			{
				return Lua.luaL_error(L, "#3 need UnityEngine.Video.VideoPlayer.FrameReadyEventHandler!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					videoPlayer.frameReady += frameReadyEventHandler;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					videoPlayer.frameReady -= frameReadyEventHandler;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to UnityEngine.Video.VideoPlayer.frameReady!");
		return 0;
	}
}
