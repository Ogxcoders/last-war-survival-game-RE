using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio;

[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioMixerPlayable.bindings.h")]
[NativeHeader("Modules/Audio/Public/Director/AudioMixerPlayable.h")]
[NativeHeader("Runtime/Director/Core/HPlayable.h")]
[StaticAccessor("AudioMixerPlayableBindings", StaticAccessorType.DoubleColon)]
[RequiredByNativeCode]
public struct AudioMixerPlayable : IPlayable, IEquatable<AudioMixerPlayable>
{
	private PlayableHandle m_Handle;

	public static AudioMixerPlayable Create(PlayableGraph graph, int inputCount = 0, bool normalizeInputVolumes = false)
	{
		PlayableHandle handle = CreateHandle(graph, inputCount, normalizeInputVolumes);
		return new AudioMixerPlayable(handle);
	}

	private static PlayableHandle CreateHandle(PlayableGraph graph, int inputCount, bool normalizeInputVolumes)
	{
		PlayableHandle handle = PlayableHandle.Null;
		if (!CreateAudioMixerPlayableInternal(ref graph, normalizeInputVolumes, ref handle))
		{
			return PlayableHandle.Null;
		}
		handle.SetInputCount(inputCount);
		return handle;
	}

	internal AudioMixerPlayable(PlayableHandle handle)
	{
		if (handle.IsValid() && !handle.IsPlayableOfType<AudioMixerPlayable>())
		{
			throw new InvalidCastException("Can't set handle: the playable is not an AudioMixerPlayable.");
		}
		m_Handle = handle;
	}

	public PlayableHandle GetHandle()
	{
		return m_Handle;
	}

	public static implicit operator Playable(AudioMixerPlayable playable)
	{
		return new Playable(playable.GetHandle());
	}

	public static explicit operator AudioMixerPlayable(Playable playable)
	{
		return new AudioMixerPlayable(playable.GetHandle());
	}

	public bool Equals(AudioMixerPlayable other)
	{
		return GetHandle() == other.GetHandle();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool CreateAudioMixerPlayableInternal(ref PlayableGraph graph, bool normalizeInputVolumes, ref PlayableHandle handle);
}
