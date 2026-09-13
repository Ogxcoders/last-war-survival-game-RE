using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[RequireComponent(typeof(AudioBehaviour))]
public sealed class AudioLowPassFilter : Behaviour
{
	public AnimationCurve customCutoffCurve
	{
		get
		{
			return GetCustomLowpassLevelCurveCopy();
		}
		set
		{
			SetCustomLowpassLevelCurveHelper(this, value);
		}
	}

	public extern float cutoffFrequency
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float lowpassResonanceQ
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern AnimationCurve GetCustomLowpassLevelCurveCopy();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[NativeMethod(Name = "AudioLowPassFilterBindings::SetCustomLowpassLevelCurveHelper", IsFreeFunction = true)]
	private static extern void SetCustomLowpassLevelCurveHelper(AudioLowPassFilter source, AnimationCurve curve);
}
