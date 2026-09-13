using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/ScriptBindings/AnimationStreamHandles.bindings.h")]
[MovedFrom("UnityEngine.Experimental.Animations")]
public static class AnimationStreamHandleUtility
{
	public unsafe static void WriteInts(AnimationStream stream, NativeArray<PropertyStreamHandle> handles, NativeArray<int> buffer, bool useMask)
	{
		stream.CheckIsValid();
		int num = AnimationSceneHandleUtility.ValidateAndGetArrayCount(ref stream, handles, buffer);
		if (num != 0)
		{
			WriteStreamIntsInternal(ref stream, handles.GetUnsafePtr(), buffer.GetUnsafePtr(), num, useMask);
		}
	}

	public unsafe static void WriteFloats(AnimationStream stream, NativeArray<PropertyStreamHandle> handles, NativeArray<float> buffer, bool useMask)
	{
		stream.CheckIsValid();
		int num = AnimationSceneHandleUtility.ValidateAndGetArrayCount(ref stream, handles, buffer);
		if (num != 0)
		{
			WriteStreamFloatsInternal(ref stream, handles.GetUnsafePtr(), buffer.GetUnsafePtr(), num, useMask);
		}
	}

	public unsafe static void ReadInts(AnimationStream stream, NativeArray<PropertyStreamHandle> handles, NativeArray<int> buffer)
	{
		stream.CheckIsValid();
		int num = AnimationSceneHandleUtility.ValidateAndGetArrayCount(ref stream, handles, buffer);
		if (num != 0)
		{
			ReadStreamIntsInternal(ref stream, handles.GetUnsafePtr(), buffer.GetUnsafePtr(), num);
		}
	}

	public unsafe static void ReadFloats(AnimationStream stream, NativeArray<PropertyStreamHandle> handles, NativeArray<float> buffer)
	{
		stream.CheckIsValid();
		int num = AnimationSceneHandleUtility.ValidateAndGetArrayCount(ref stream, handles, buffer);
		if (num != 0)
		{
			ReadStreamFloatsInternal(ref stream, handles.GetUnsafePtr(), buffer.GetUnsafePtr(), num);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AnimationHandleUtilityBindings::ReadStreamIntsInternal", IsFreeFunction = true, HasExplicitThis = false, IsThreadSafe = true)]
	private unsafe static extern void ReadStreamIntsInternal(ref AnimationStream stream, void* propertyStreamHandles, void* intBuffer, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AnimationHandleUtilityBindings::ReadStreamFloatsInternal", IsFreeFunction = true, HasExplicitThis = false, IsThreadSafe = true)]
	private unsafe static extern void ReadStreamFloatsInternal(ref AnimationStream stream, void* propertyStreamHandles, void* floatBuffer, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AnimationHandleUtilityBindings::WriteStreamIntsInternal", IsFreeFunction = true, HasExplicitThis = false, IsThreadSafe = true)]
	private unsafe static extern void WriteStreamIntsInternal(ref AnimationStream stream, void* propertyStreamHandles, void* intBuffer, int count, bool useMask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "AnimationHandleUtilityBindings::WriteStreamFloatsInternal", IsFreeFunction = true, HasExplicitThis = false, IsThreadSafe = true)]
	private unsafe static extern void WriteStreamFloatsInternal(ref AnimationStream stream, void* propertyStreamHandles, void* floatBuffer, int count, bool useMask);
}
