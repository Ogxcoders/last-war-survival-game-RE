using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/Director/AnimationSceneHandles.h")]
[MovedFrom("UnityEngine.Experimental.Animations")]
public struct PropertySceneHandle
{
	private uint valid;

	private int handleIndex;

	private bool createdByNative => valid != 0;

	private bool hasHandleIndex => handleIndex != -1;

	public bool IsValid(AnimationStream stream)
	{
		return IsValidInternal(ref stream);
	}

	private bool IsValidInternal(ref AnimationStream stream)
	{
		return stream.isValid && createdByNative && hasHandleIndex && HasValidTransform(ref stream);
	}

	public void Resolve(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		ResolveInternal(ref stream);
	}

	public bool IsResolved(AnimationStream stream)
	{
		return IsValidInternal(ref stream) && IsBound(ref stream);
	}

	private void CheckIsValid(ref AnimationStream stream)
	{
		stream.CheckIsValid();
		if (!createdByNative || !hasHandleIndex)
		{
			throw new InvalidOperationException("The PropertySceneHandle is invalid. Please use proper function to create the handle.");
		}
		if (!HasValidTransform(ref stream))
		{
			throw new NullReferenceException("The transform is invalid.");
		}
	}

	public float GetFloat(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetFloatInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetFloat(AnimationStream stream, float value)
	{
	}

	public int GetInt(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetIntInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetInt(AnimationStream stream, int value)
	{
	}

	public bool GetBool(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetBoolInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetBool(AnimationStream stream, bool value)
	{
	}

	[ThreadSafe]
	private bool HasValidTransform(ref AnimationStream stream)
	{
		return HasValidTransform_Injected(ref this, ref stream);
	}

	[ThreadSafe]
	private bool IsBound(ref AnimationStream stream)
	{
		return IsBound_Injected(ref this, ref stream);
	}

	[NativeMethod(Name = "Resolve", IsThreadSafe = true)]
	private void ResolveInternal(ref AnimationStream stream)
	{
		ResolveInternal_Injected(ref this, ref stream);
	}

	[NativeMethod(Name = "GetFloat", IsThreadSafe = true)]
	private float GetFloatInternal(ref AnimationStream stream)
	{
		return GetFloatInternal_Injected(ref this, ref stream);
	}

	[NativeMethod(Name = "GetInt", IsThreadSafe = true)]
	private int GetIntInternal(ref AnimationStream stream)
	{
		return GetIntInternal_Injected(ref this, ref stream);
	}

	[NativeMethod(Name = "GetBool", IsThreadSafe = true)]
	private bool GetBoolInternal(ref AnimationStream stream)
	{
		return GetBoolInternal_Injected(ref this, ref stream);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool HasValidTransform_Injected(ref PropertySceneHandle _unity_self, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsBound_Injected(ref PropertySceneHandle _unity_self, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ResolveInternal_Injected(ref PropertySceneHandle _unity_self, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float GetFloatInternal_Injected(ref PropertySceneHandle _unity_self, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetIntInternal_Injected(ref PropertySceneHandle _unity_self, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool GetBoolInternal_Injected(ref PropertySceneHandle _unity_self, ref AnimationStream stream);
}
