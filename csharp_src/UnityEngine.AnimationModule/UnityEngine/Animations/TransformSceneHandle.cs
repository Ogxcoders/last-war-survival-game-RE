using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/Director/AnimationSceneHandles.h")]
[NativeHeader("Modules/Animation/ScriptBindings/AnimationStreamHandles.bindings.h")]
[MovedFrom("UnityEngine.Experimental.Animations")]
public struct TransformSceneHandle
{
	private uint valid;

	private int transformSceneHandleDefinitionIndex;

	private bool createdByNative => valid != 0;

	private bool hasTransformSceneHandleDefinitionIndex => transformSceneHandleDefinitionIndex != -1;

	public bool IsValid(AnimationStream stream)
	{
		return stream.isValid && createdByNative && hasTransformSceneHandleDefinitionIndex && HasValidTransform(ref stream);
	}

	private void CheckIsValid(ref AnimationStream stream)
	{
		stream.CheckIsValid();
		if (!createdByNative || !hasTransformSceneHandleDefinitionIndex)
		{
			throw new InvalidOperationException("The TransformSceneHandle is invalid. Please use proper function to create the handle.");
		}
		if (!HasValidTransform(ref stream))
		{
			throw new NullReferenceException("The transform is invalid.");
		}
	}

	public Vector3 GetPosition(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetPositionInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetPosition(AnimationStream stream, Vector3 position)
	{
	}

	public Vector3 GetLocalPosition(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetLocalPositionInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetLocalPosition(AnimationStream stream, Vector3 position)
	{
	}

	public Quaternion GetRotation(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetRotationInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetRotation(AnimationStream stream, Quaternion rotation)
	{
	}

	public Quaternion GetLocalRotation(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetLocalRotationInternal(ref stream);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetLocalRotation(AnimationStream stream, Quaternion rotation)
	{
	}

	public Vector3 GetLocalScale(AnimationStream stream)
	{
		CheckIsValid(ref stream);
		return GetLocalScaleInternal(ref stream);
	}

	public void GetLocalTRS(AnimationStream stream, out Vector3 position, out Quaternion rotation, out Vector3 scale)
	{
		CheckIsValid(ref stream);
		GetLocalTRSInternal(ref stream, out position, out rotation, out scale);
	}

	public void GetGlobalTR(AnimationStream stream, out Vector3 position, out Quaternion rotation)
	{
		CheckIsValid(ref stream);
		GetGlobalTRInternal(ref stream, out position, out rotation);
	}

	[Obsolete("SceneHandle is now read-only; it was problematic with the engine multithreading and determinism", true)]
	public void SetLocalScale(AnimationStream stream, Vector3 scale)
	{
	}

	[ThreadSafe]
	private bool HasValidTransform(ref AnimationStream stream)
	{
		return HasValidTransform_Injected(ref this, ref stream);
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetPositionInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetPositionInternal(ref AnimationStream stream)
	{
		GetPositionInternal_Injected(ref this, ref stream, out var ret);
		return ret;
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetLocalPositionInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetLocalPositionInternal(ref AnimationStream stream)
	{
		GetLocalPositionInternal_Injected(ref this, ref stream, out var ret);
		return ret;
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetRotationInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion GetRotationInternal(ref AnimationStream stream)
	{
		GetRotationInternal_Injected(ref this, ref stream, out var ret);
		return ret;
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetLocalRotationInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion GetLocalRotationInternal(ref AnimationStream stream)
	{
		GetLocalRotationInternal_Injected(ref this, ref stream, out var ret);
		return ret;
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetLocalScaleInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetLocalScaleInternal(ref AnimationStream stream)
	{
		GetLocalScaleInternal_Injected(ref this, ref stream, out var ret);
		return ret;
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetLocalTRSInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void GetLocalTRSInternal(ref AnimationStream stream, out Vector3 position, out Quaternion rotation, out Vector3 scale)
	{
		GetLocalTRSInternal_Injected(ref this, ref stream, out position, out rotation, out scale);
	}

	[NativeMethod(Name = "TransformSceneHandleBindings::GetGlobalTRInternal", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void GetGlobalTRInternal(ref AnimationStream stream, out Vector3 position, out Quaternion rotation)
	{
		GetGlobalTRInternal_Injected(ref this, ref stream, out position, out rotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool HasValidTransform_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetPositionInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetLocalPositionInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetRotationInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetLocalRotationInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetLocalScaleInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetLocalTRSInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Vector3 position, out Quaternion rotation, out Vector3 scale);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetGlobalTRInternal_Injected(ref TransformSceneHandle _unity_self, ref AnimationStream stream, out Vector3 position, out Quaternion rotation);
}
