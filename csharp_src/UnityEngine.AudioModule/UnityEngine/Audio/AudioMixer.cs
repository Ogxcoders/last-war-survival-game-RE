using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Audio;

[ExcludeFromPreset]
[ExcludeFromObjectFactory]
[NativeHeader("Modules/Audio/Public/AudioMixer.h")]
[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioMixer.bindings.h")]
public class AudioMixer : Object
{
	[NativeProperty]
	public extern AudioMixerGroup outputAudioMixerGroup
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty]
	public extern AudioMixerUpdateMode updateMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	internal AudioMixer()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("FindSnapshotFromName")]
	public extern AudioMixerSnapshot FindSnapshot(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("AudioMixerBindings::FindMatchingGroups", IsFreeFunction = true, HasExplicitThis = true)]
	public extern AudioMixerGroup[] FindMatchingGroups(string subPath);

	internal void TransitionToSnapshot(AudioMixerSnapshot snapshot, float timeToReach)
	{
		if (snapshot == null)
		{
			throw new ArgumentException("null Snapshot passed to AudioMixer.TransitionToSnapshot of AudioMixer '" + base.name + "'");
		}
		if (snapshot.audioMixer != this)
		{
			throw new ArgumentException("Snapshot '" + snapshot.name + "' passed to AudioMixer.TransitionToSnapshot is not a snapshot from AudioMixer '" + base.name + "'");
		}
		TransitionToSnapshotInternal(snapshot, timeToReach);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("TransitionToSnapshot")]
	private extern void TransitionToSnapshotInternal(AudioMixerSnapshot snapshot, float timeToReach);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("AudioMixerBindings::TransitionToSnapshots", IsFreeFunction = true, HasExplicitThis = true, ThrowsException = true)]
	public extern void TransitionToSnapshots(AudioMixerSnapshot[] snapshots, float[] weights, float timeToReach);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod]
	public extern bool SetFloat(string name, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod]
	public extern bool ClearFloat(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod]
	public extern bool GetFloat(string name, out float value);
}
