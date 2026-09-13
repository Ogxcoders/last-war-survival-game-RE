using System.Runtime.CompilerServices;

namespace UnityEngine;

[RequireComponent(typeof(AudioBehaviour))]
public sealed class AudioHighPassFilter : Behaviour
{
	public extern float cutoffFrequency
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float highpassResonanceQ
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}
}
