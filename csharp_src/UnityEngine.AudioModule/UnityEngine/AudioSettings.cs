using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StaticAccessor("GetAudioManager()", StaticAccessorType.Dot)]
[NativeHeader("Modules/Audio/Public/ScriptBindings/Audio.bindings.h")]
public sealed class AudioSettings
{
	public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);

	public static class Mobile
	{
		private static bool _stopAudioOutputOnMute = false;

		public static bool muteState { get; private set; } = false;

		public static bool stopAudioOutputOnMute
		{
			get
			{
				return _stopAudioOutputOnMute;
			}
			set
			{
				_stopAudioOutputOnMute = value;
				if (value && muteState && audioOutputStarted)
				{
					StopAudioOutput();
				}
				else if (!value && !audioOutputStarted)
				{
					StartAudioOutput();
				}
			}
		}

		public static bool audioOutputStarted => AudioSettings.audioOutputStarted;

		public static event Action<bool> OnMuteStateChanged;

		[RequiredByNativeCode]
		internal static void InvokeOnMuteStateChanged(bool mute)
		{
			if (mute == muteState)
			{
				return;
			}
			muteState = mute;
			if (stopAudioOutputOnMute)
			{
				if (muteState)
				{
					StopAudioOutput();
				}
				else
				{
					StartAudioOutput();
				}
			}
			if (Mobile.OnMuteStateChanged != null)
			{
				Mobile.OnMuteStateChanged(mute);
			}
		}

		[RequiredByNativeCode]
		internal static bool InvokeIsStopAudioOutputOnMuteEnabled()
		{
			return stopAudioOutputOnMute;
		}

		public static void StartAudioOutput()
		{
			AudioSettings.StartAudioOutput();
		}

		public static void StopAudioOutput()
		{
			AudioSettings.StopAudioOutput();
		}
	}

	public static extern AudioSpeakerMode driverCapabilities
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetSpeakerModeCaps")]
		get;
	}

	public static AudioSpeakerMode speakerMode
	{
		get
		{
			return GetSpeakerMode();
		}
		set
		{
			Debug.LogWarning("Setting AudioSettings.speakerMode is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.");
			AudioConfiguration configuration = GetConfiguration();
			configuration.speakerMode = value;
			if (!SetConfiguration(configuration))
			{
				Debug.LogWarning("Setting AudioSettings.speakerMode failed");
			}
		}
	}

	internal static extern int profilerCaptureFlags
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static extern double dspTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "GetDSPTime", IsThreadSafe = true)]
		get;
	}

	public static int outputSampleRate
	{
		get
		{
			return GetSampleRate();
		}
		set
		{
			Debug.LogWarning("Setting AudioSettings.outputSampleRate is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.");
			AudioConfiguration configuration = GetConfiguration();
			configuration.sampleRate = value;
			if (!SetConfiguration(configuration))
			{
				Debug.LogWarning("Setting AudioSettings.outputSampleRate failed");
			}
		}
	}

	internal static extern bool unityAudioDisabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsAudioDisabled")]
		get;
	}

	internal static extern bool audioOutputStarted
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static event AudioConfigurationChangeHandler OnAudioConfigurationChanged;

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AudioSpeakerMode GetSpeakerMode();

	[NativeMethod(Name = "AudioSettings::SetConfiguration", IsFreeFunction = true)]
	[NativeThrows]
	private static bool SetConfiguration(AudioConfiguration config)
	{
		return SetConfiguration_Injected(ref config);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AudioSettings::GetSampleRate", IsFreeFunction = true)]
	private static extern int GetSampleRate();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AudioSettings::GetDSPBufferSize", IsFreeFunction = true)]
	public static extern void GetDSPBufferSize(out int bufferLength, out int numBuffers);

	[Obsolete("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.")]
	public static void SetDSPBufferSize(int bufferLength, int numBuffers)
	{
		Debug.LogWarning("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.");
		AudioConfiguration configuration = GetConfiguration();
		configuration.dspBufferSize = bufferLength;
		if (!SetConfiguration(configuration))
		{
			Debug.LogWarning("SetDSPBufferSize failed");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetCurrentSpatializerDefinitionName")]
	public static extern string GetSpatializerPluginName();

	public static AudioConfiguration GetConfiguration()
	{
		GetConfiguration_Injected(out var ret);
		return ret;
	}

	public static bool Reset(AudioConfiguration config)
	{
		return SetConfiguration(config);
	}

	[RequiredByNativeCode]
	internal static void InvokeOnAudioConfigurationChanged(bool deviceWasChanged)
	{
		if (AudioSettings.OnAudioConfigurationChanged != null)
		{
			AudioSettings.OnAudioConfigurationChanged(deviceWasChanged);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AudioSettings::GetCurrentAmbisonicDefinitionName", IsFreeFunction = true)]
	internal static extern string GetAmbisonicDecoderPluginName();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool StartAudioOutput();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool StopAudioOutput();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool SetConfiguration_Injected(ref AudioConfiguration config);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetConfiguration_Injected(out AudioConfiguration ret);
}
