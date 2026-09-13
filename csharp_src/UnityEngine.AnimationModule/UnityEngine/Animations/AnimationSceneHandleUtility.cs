using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/ScriptBindings/AnimationStreamHandles.bindings.h")]
[MovedFrom("UnityEngine.Experimental.Animations")]
public static class AnimationSceneHandleUtility
{
	public unsafe static void ReadInts(AnimationStream stream, NativeArray<PropertySceneHandle> handles, NativeArray<int> buffer)
	{
		int num = ValidateAndGetArrayCount(ref stream, handles, buffer);
		if (num != 0)
		{
			ReadSceneIntsInternal(ref stream, handles.GetUnsafePtr(), buffer.GetUnsafePtr(), num);
		}
	}

	public unsafe static void ReadFloats(AnimationStream stream, NativeArray<PropertySceneHandle> handles, NativeArray<float> buffer)
	{
		int num = ValidateAndGetArrayCount(ref stream, handles, buffer);
		if (num != 0)
		{
			ReadSceneFloatsInternal(ref stream, handles.GetUnsafePtr(), buffer.GetUnsafePtr(), num);
		}
	}

	internal static int ValidateAndGetArrayCount<T0, T1>(ref AnimationStream stream, NativeArray<T0> handles, NativeArray<T1> buffer) where T0 : struct where T1 : struct
	{
		stream.CheckIsValid();
		if (!handles.IsCreated)
		{
			throw new NullReferenceException("Handle array is invalid.");
		}
		if (!buffer.IsCreated)
		{
			throw new NullReferenceException("Data buffer is invalid.");
		}
		if (buffer.Length < handles.Length)
		{
			throw new InvalidOperationException("Data buffer array is smaller than handles array.");
		}
		return handles.Length;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AnimationHandleUtilityBindings::ReadSceneIntsInternal", IsFreeFunction = true, HasExplicitThis = false, IsThreadSafe = true)]
	private unsafe static extern void ReadSceneIntsInternal(ref AnimationStream stream, void* propertySceneHandles, void* intBuffer, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AnimationHandleUtilityBindings::ReadSceneFloatsInternal", IsFreeFunction = true, HasExplicitThis = false, IsThreadSafe = true)]
	private unsafe static extern void ReadSceneFloatsInternal(ref AnimationStream stream, void* propertySceneHandles, void* floatBuffer, int count);
}
