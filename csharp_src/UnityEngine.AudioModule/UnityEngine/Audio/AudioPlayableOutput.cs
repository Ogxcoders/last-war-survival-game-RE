using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio;

[RequiredByNativeCode]
[StaticAccessor("AudioPlayableOutputBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/Audio/Public/AudioSource.h")]
[NativeHeader("Modules/Audio/Public/Director/AudioPlayableOutput.h")]
[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioPlayableOutput.bindings.h")]
public struct AudioPlayableOutput : IPlayableOutput
{
	private PlayableOutputHandle m_Handle;

	public static AudioPlayableOutput Null => new AudioPlayableOutput(PlayableOutputHandle.Null);

	public static AudioPlayableOutput Create(PlayableGraph graph, string name, AudioSource target)
	{
		if (!AudioPlayableGraphExtensions.InternalCreateAudioOutput(ref graph, name, out var handle))
		{
			return Null;
		}
		AudioPlayableOutput result = new AudioPlayableOutput(handle);
		result.SetTarget(target);
		return result;
	}

	internal AudioPlayableOutput(PlayableOutputHandle handle)
	{
		if (handle.IsValid() && !handle.IsPlayableOutputOfType<AudioPlayableOutput>())
		{
			throw new InvalidCastException("Can't set handle: the playable is not an AudioPlayableOutput.");
		}
		m_Handle = handle;
	}

	public PlayableOutputHandle GetHandle()
	{
		return m_Handle;
	}

	public static implicit operator PlayableOutput(AudioPlayableOutput output)
	{
		return new PlayableOutput(output.GetHandle());
	}

	public static explicit operator AudioPlayableOutput(PlayableOutput output)
	{
		return new AudioPlayableOutput(output.GetHandle());
	}

	public AudioSource GetTarget()
	{
		return InternalGetTarget(ref m_Handle);
	}

	public void SetTarget(AudioSource value)
	{
		InternalSetTarget(ref m_Handle, value);
	}

	public bool GetEvaluateOnSeek()
	{
		return InternalGetEvaluateOnSeek(ref m_Handle);
	}

	public void SetEvaluateOnSeek(bool value)
	{
		InternalSetEvaluateOnSeek(ref m_Handle, value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern AudioSource InternalGetTarget(ref PlayableOutputHandle output);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void InternalSetTarget(ref PlayableOutputHandle output, AudioSource target);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool InternalGetEvaluateOnSeek(ref PlayableOutputHandle output);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void InternalSetEvaluateOnSeek(ref PlayableOutputHandle output, bool value);
}
