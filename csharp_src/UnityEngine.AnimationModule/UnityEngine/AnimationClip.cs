using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/Animation/ScriptBindings/AnimationClip.bindings.h")]
[NativeType("Modules/Animation/AnimationClip.h")]
public sealed class AnimationClip : Motion
{
	[NativeProperty("Length", false, TargetType.Function)]
	public extern float length
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("StartTime", false, TargetType.Function)]
	internal extern float startTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("StopTime", false, TargetType.Function)]
	internal extern float stopTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("SampleRate", false, TargetType.Function)]
	public extern float frameRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("WrapMode", false, TargetType.Function)]
	public extern WrapMode wrapMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("Bounds", false, TargetType.Function)]
	public Bounds localBounds
	{
		get
		{
			get_localBounds_Injected(out var ret);
			return ret;
		}
		set
		{
			set_localBounds_Injected(ref value);
		}
	}

	public new extern bool legacy
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsLegacy")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetLegacy")]
		set;
	}

	public extern bool humanMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsHumanMotion")]
		get;
	}

	public extern bool empty
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsEmpty")]
		get;
	}

	public extern bool hasGenericRootTransform
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("HasGenericRootTransform")]
		get;
	}

	public extern bool hasMotionFloatCurves
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("HasMotionFloatCurves")]
		get;
	}

	public extern bool hasMotionCurves
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("HasMotionCurves")]
		get;
	}

	public extern bool hasRootCurves
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("HasRootCurves")]
		get;
	}

	internal extern bool hasRootMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "AnimationClipBindings::Internal_GetHasRootMotion", HasExplicitThis = true)]
		get;
	}

	public AnimationEvent[] events
	{
		get
		{
			return (AnimationEvent[])GetEventsInternal();
		}
		set
		{
			SetEventsInternal(value);
		}
	}

	public AnimationClip()
	{
		Internal_CreateAnimationClip(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("AnimationClipBindings::Internal_CreateAnimationClip")]
	private static extern void Internal_CreateAnimationClip([Writable] AnimationClip self);

	public void SampleAnimation(GameObject go, float time)
	{
		SampleAnimation(go, this, time, wrapMode);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeHeader("Modules/Animation/AnimationUtility.h")]
	[FreeFunction]
	internal static extern void SampleAnimation([NotNull] GameObject go, [NotNull] AnimationClip clip, float inTime, WrapMode wrapMode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("AnimationClipBindings::Internal_SetCurve", HasExplicitThis = true)]
	public extern void SetCurve([NotNull] string relativePath, [NotNull] Type type, [NotNull] string propertyName, AnimationCurve curve);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void EnsureQuaternionContinuity();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ClearCurves();

	public void AddEvent(AnimationEvent evt)
	{
		if (evt == null)
		{
			throw new ArgumentNullException("evt");
		}
		AddEventInternal(evt);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	internal extern void AddEventInternal(object evt);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	internal extern void SetEventsInternal(Array value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	internal extern Array GetEventsInternal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_localBounds_Injected(out Bounds ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_localBounds_Injected(ref Bounds value);
}
