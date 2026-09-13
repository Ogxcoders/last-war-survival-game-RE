using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Audio;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeType(Header = "Modules/Audio/Public/ScriptBindings/AudioRenderer.bindings.h")]
public class AudioRenderer
{
	public static bool Start()
	{
		return Internal_AudioRenderer_Start();
	}

	public static bool Stop()
	{
		return Internal_AudioRenderer_Stop();
	}

	public static int GetSampleCountForCaptureFrame()
	{
		return Internal_AudioRenderer_GetSampleCountForCaptureFrame();
	}

	internal unsafe static bool AddMixerGroupSink(AudioMixerGroup mixerGroup, NativeArray<float> buffer, bool excludeFromMix)
	{
		return Internal_AudioRenderer_AddMixerGroupSink(mixerGroup, buffer.GetUnsafePtr(), buffer.Length, excludeFromMix);
	}

	public unsafe static bool Render(NativeArray<float> buffer)
	{
		return Internal_AudioRenderer_Render(buffer.GetUnsafePtr(), buffer.Length);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool Internal_AudioRenderer_Start();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool Internal_AudioRenderer_Stop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int Internal_AudioRenderer_GetSampleCountForCaptureFrame();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal unsafe static extern bool Internal_AudioRenderer_AddMixerGroupSink(AudioMixerGroup mixerGroup, void* ptr, int length, bool excludeFromMix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal unsafe static extern bool Internal_AudioRenderer_Render(void* ptr, int length);
}
