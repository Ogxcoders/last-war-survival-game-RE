using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio;

[NativeHeader("Runtime/Director/Core/HPlayable.h")]
[StaticAccessor("AudioClipPlayableBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/Audio/Public/Director/AudioClipPlayable.h")]
[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioClipPlayable.bindings.h")]
[RequiredByNativeCode]
public struct AudioClipPlayable : IPlayable, IEquatable<AudioClipPlayable>
{
	private PlayableHandle m_Handle;

	public static AudioClipPlayable Create(PlayableGraph graph, AudioClip clip, bool looping)
	{
		PlayableHandle handle = CreateHandle(graph, clip, looping);
		AudioClipPlayable audioClipPlayable = new AudioClipPlayable(handle);
		if (clip != null)
		{
			audioClipPlayable.SetDuration(clip.length);
		}
		return audioClipPlayable;
	}

	private static PlayableHandle CreateHandle(PlayableGraph graph, AudioClip clip, bool looping)
	{
		PlayableHandle handle = PlayableHandle.Null;
		if (!InternalCreateAudioClipPlayable(ref graph, clip, looping, ref handle))
		{
			return PlayableHandle.Null;
		}
		return handle;
	}

	internal AudioClipPlayable(PlayableHandle handle)
	{
		if (handle.IsValid() && !handle.IsPlayableOfType<AudioClipPlayable>())
		{
			throw new InvalidCastException("Can't set handle: the playable is not an AudioClipPlayable.");
		}
		m_Handle = handle;
	}

	public PlayableHandle GetHandle()
	{
		return m_Handle;
	}

	public static implicit operator Playable(AudioClipPlayable playable)
	{
		return new Playable(playable.GetHandle());
	}

	public static explicit operator AudioClipPlayable(Playable playable)
	{
		return new AudioClipPlayable(playable.GetHandle());
	}

	public bool Equals(AudioClipPlayable other)
	{
		return GetHandle() == other.GetHandle();
	}

	public AudioClip GetClip()
	{
		return GetClipInternal(ref m_Handle);
	}

	public void SetClip(AudioClip value)
	{
		SetClipInternal(ref m_Handle, value);
	}

	public bool GetLooped()
	{
		return GetLoopedInternal(ref m_Handle);
	}

	public void SetLooped(bool value)
	{
		SetLoopedInternal(ref m_Handle, value);
	}

	internal float GetVolume()
	{
		return GetVolumeInternal(ref m_Handle);
	}

	internal void SetVolume(float value)
	{
		if (value < 0f || value > 1f)
		{
			throw new ArgumentException("Trying to set AudioClipPlayable volume outside of range (0.0 - 1.0): " + value);
		}
		SetVolumeInternal(ref m_Handle, value);
	}

	internal float GetStereoPan()
	{
		return GetStereoPanInternal(ref m_Handle);
	}

	internal void SetStereoPan(float value)
	{
		if (value < -1f || value > 1f)
		{
			throw new ArgumentException("Trying to set AudioClipPlayable stereo pan outside of range (-1.0 - 1.0): " + value);
		}
		SetStereoPanInternal(ref m_Handle, value);
	}

	internal float GetSpatialBlend()
	{
		return GetSpatialBlendInternal(ref m_Handle);
	}

	internal void SetSpatialBlend(float value)
	{
		if (value < 0f || value > 1f)
		{
			throw new ArgumentException("Trying to set AudioClipPlayable spatial blend outside of range (0.0 - 1.0): " + value);
		}
		SetSpatialBlendInternal(ref m_Handle, value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("IsPlaying() has been deprecated. Use IsChannelPlaying() instead (UnityUpgradable) -> IsChannelPlaying()", true)]
	public bool IsPlaying()
	{
		return IsChannelPlaying();
	}

	public bool IsChannelPlaying()
	{
		return GetIsChannelPlayingInternal(ref m_Handle);
	}

	public double GetStartDelay()
	{
		return GetStartDelayInternal(ref m_Handle);
	}

	internal void SetStartDelay(double value)
	{
		SetStartDelayInternal(ref m_Handle, value);
	}

	public double GetPauseDelay()
	{
		return GetPauseDelayInternal(ref m_Handle);
	}

	internal void GetPauseDelay(double value)
	{
		double pauseDelayInternal = GetPauseDelayInternal(ref m_Handle);
		if (m_Handle.GetPlayState() == PlayState.Playing && (value < 0.05 || (pauseDelayInternal != 0.0 && pauseDelayInternal < 0.05)))
		{
			throw new ArgumentException("AudioClipPlayable.pauseDelay: Setting new delay when existing delay is too small or 0.0 (" + pauseDelayInternal + "), audio system will not be able to change in time");
		}
		SetPauseDelayInternal(ref m_Handle, value);
	}

	public void Seek(double startTime, double startDelay)
	{
		Seek(startTime, startDelay, 0.0);
	}

	public void Seek(double startTime, double startDelay, [DefaultValue("0")] double duration)
	{
		SetStartDelayInternal(ref m_Handle, startDelay);
		if (duration > 0.0)
		{
			m_Handle.SetDuration(duration + startTime);
			SetPauseDelayInternal(ref m_Handle, startDelay + duration);
		}
		else
		{
			m_Handle.SetDuration(double.MaxValue);
			SetPauseDelayInternal(ref m_Handle, 0.0);
		}
		m_Handle.SetTime(startTime);
		m_Handle.Play();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern AudioClip GetClipInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetClipInternal(ref PlayableHandle hdl, AudioClip clip);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetLoopedInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetLoopedInternal(ref PlayableHandle hdl, bool looped);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern float GetVolumeInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetVolumeInternal(ref PlayableHandle hdl, float volume);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern float GetStereoPanInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetStereoPanInternal(ref PlayableHandle hdl, float stereoPan);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern float GetSpatialBlendInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetSpatialBlendInternal(ref PlayableHandle hdl, float spatialBlend);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetIsChannelPlayingInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern double GetStartDelayInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetStartDelayInternal(ref PlayableHandle hdl, double delay);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern double GetPauseDelayInternal(ref PlayableHandle hdl);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetPauseDelayInternal(ref PlayableHandle hdl, double delay);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool InternalCreateAudioClipPlayable(ref PlayableGraph graph, AudioClip clip, bool looping, ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool ValidateType(ref PlayableHandle hdl);
}
