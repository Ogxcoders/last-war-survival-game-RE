using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Video;

[RequireComponent(typeof(Transform))]
[NativeHeader("Modules/Video/Public/VideoPlayer.h")]
[RequiredByNativeCode]
public sealed class VideoPlayer : Behaviour
{
	public delegate void EventHandler(VideoPlayer source);

	public delegate void ErrorEventHandler(VideoPlayer source, string message);

	public delegate void FrameReadyEventHandler(VideoPlayer source, long frameIdx);

	public delegate void TimeEventHandler(VideoPlayer source, double seconds);

	public extern VideoSource source
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeName("VideoUrl")]
	public extern string url
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeName("VideoClip")]
	public extern VideoClip clip
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern VideoRenderMode renderMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeHeader("Runtime/Camera/Camera.h")]
	public extern Camera targetCamera
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeHeader("Runtime/Graphics/RenderTexture.h")]
	public extern RenderTexture targetTexture
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeHeader("Runtime/Graphics/Renderer.h")]
	public extern Renderer targetMaterialRenderer
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern string targetMaterialProperty
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern VideoAspectRatio aspectRatio
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float targetCameraAlpha
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Video3DLayout targetCamera3DLayout
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeHeader("Runtime/Graphics/Texture.h")]
	public extern Texture texture
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool isPrepared
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsPrepared")]
		get;
	}

	public extern bool waitForFirstFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool playOnAwake
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isPlaying
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsPlaying")]
		get;
	}

	public extern bool isPaused
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsPaused")]
		get;
	}

	public extern bool canSetTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("CanSetTime")]
		get;
	}

	[NativeName("SecPosition")]
	public extern double time
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeName("FramePosition")]
	public extern long frame
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern double clockTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool canStep
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("CanStep")]
		get;
	}

	public extern bool canSetPlaybackSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("CanSetPlaybackSpeed")]
		get;
	}

	public extern float playbackSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeName("Loop")]
	public extern bool isLooping
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool canSetTimeSource
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("CanSetTimeSource")]
		get;
	}

	public extern VideoTimeSource timeSource
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern VideoTimeReference timeReference
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern double externalReferenceTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool canSetSkipOnDrop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("CanSetSkipOnDrop")]
		get;
	}

	public extern bool skipOnDrop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ulong frameCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float frameRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeName("Duration")]
	public extern double length
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern uint width
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern uint height
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern uint pixelAspectRatioNumerator
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern uint pixelAspectRatioDenominator
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern ushort audioTrackCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static extern ushort controlledAudioTrackMaxCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public ushort controlledAudioTrackCount
	{
		get
		{
			return GetControlledAudioTrackCount();
		}
		set
		{
			int num = controlledAudioTrackMaxCount;
			if (value > num)
			{
				throw new ArgumentException($"Cannot control more than {num} tracks.", "value");
			}
			SetControlledAudioTrackCount(value);
		}
	}

	public extern VideoAudioOutputMode audioOutputMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool canSetDirectAudioVolume
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("CanSetDirectAudioVolume")]
		get;
	}

	public extern bool sendFrameReadyEvents
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("AreFrameReadyEventsEnabled")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("EnableFrameReadyEvents")]
		set;
	}

	public event EventHandler prepareCompleted;

	public event EventHandler loopPointReached;

	public event EventHandler started;

	public event EventHandler frameDropped;

	public event ErrorEventHandler errorReceived;

	public event EventHandler seekCompleted;

	public event TimeEventHandler clockResyncOccurred;

	public event FrameReadyEventHandler frameReady;

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Prepare();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Play();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Pause();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Stop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void StepForward();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern string GetAudioLanguageCode(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern ushort GetAudioChannelCount(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern uint GetAudioSampleRate(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern ushort GetControlledAudioTrackCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetControlledAudioTrackCount(ushort value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void EnableAudioTrack(ushort trackIndex, bool enabled);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool IsAudioTrackEnabled(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern float GetDirectAudioVolume(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetDirectAudioVolume(ushort trackIndex, float volume);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool GetDirectAudioMute(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetDirectAudioMute(ushort trackIndex, bool mute);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeHeader("Modules/Audio/Public/AudioSource.h")]
	public extern AudioSource GetTargetAudioSource(ushort trackIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetTargetAudioSource(ushort trackIndex, AudioSource source);

	[RequiredByNativeCode]
	private static void InvokePrepareCompletedCallback_Internal(VideoPlayer source)
	{
		if (source.prepareCompleted != null)
		{
			source.prepareCompleted(source);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeFrameReadyCallback_Internal(VideoPlayer source, long frameIdx)
	{
		if (source.frameReady != null)
		{
			source.frameReady(source, frameIdx);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeLoopPointReachedCallback_Internal(VideoPlayer source)
	{
		if (source.loopPointReached != null)
		{
			source.loopPointReached(source);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeStartedCallback_Internal(VideoPlayer source)
	{
		if (source.started != null)
		{
			source.started(source);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeFrameDroppedCallback_Internal(VideoPlayer source)
	{
		if (source.frameDropped != null)
		{
			source.frameDropped(source);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeErrorReceivedCallback_Internal(VideoPlayer source, string errorStr)
	{
		if (source.errorReceived != null)
		{
			source.errorReceived(source, errorStr);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeSeekCompletedCallback_Internal(VideoPlayer source)
	{
		if (source.seekCompleted != null)
		{
			source.seekCompleted(source);
		}
	}

	[RequiredByNativeCode]
	private static void InvokeClockResyncOccurredCallback_Internal(VideoPlayer source, double seconds)
	{
		if (source.clockResyncOccurred != null)
		{
			source.clockResyncOccurred(source, seconds);
		}
	}
}
