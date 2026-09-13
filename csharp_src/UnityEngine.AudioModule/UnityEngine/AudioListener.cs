using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[RequireComponent(typeof(Transform))]
[StaticAccessor("AudioListenerBindings", StaticAccessorType.DoubleColon)]
public sealed class AudioListener : AudioBehaviour
{
	public static extern float volume
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("ListenerPause")]
	public static extern bool pause
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern AudioVelocityUpdateMode velocityUpdateMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void GetOutputDataHelper([Out] float[] samples, int channel);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void GetSpectrumDataHelper([Out] float[] samples, int channel, FFTWindow window);

	[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
	public static float[] GetOutputData(int numSamples, int channel)
	{
		float[] array = new float[numSamples];
		GetOutputDataHelper(array, channel);
		return array;
	}

	public static void GetOutputData(float[] samples, int channel)
	{
		GetOutputDataHelper(samples, channel);
	}

	[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetSpectrumData and pass a pre allocated array instead.")]
	public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
	{
		float[] array = new float[numSamples];
		GetSpectrumDataHelper(array, channel, window);
		return array;
	}

	public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
	{
		GetSpectrumDataHelper(samples, channel, window);
	}
}
