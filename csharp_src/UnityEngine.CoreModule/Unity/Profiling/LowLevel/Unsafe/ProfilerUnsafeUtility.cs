using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Profiling.LowLevel.Unsafe;

[NativeHeader("Runtime/Profiler/ScriptBindings/ProfilerUnsafeUtility.Bindings.h")]
[UsedByNativeCode]
internal static class ProfilerUnsafeUtility
{
	public struct TimestampConversionRatio
	{
		public long Numerator;

		public long Denominator;
	}

	public const ushort CategoryRender = 0;

	public const ushort CategoryScripts = 1;

	public const ushort CategoryGUI = 4;

	public const ushort CategoryPhysics = 5;

	public const ushort CategoryAnimation = 6;

	public const ushort CategoryAi = 7;

	public const ushort CategoryAudio = 8;

	public const ushort CategoryVideo = 11;

	public const ushort CategoryParticles = 12;

	public const ushort CategoryLighting = 13;

	public const ushort CategoryNetwork = 14;

	public const ushort CategoryLoading = 15;

	public const ushort CategoryOther = 16;

	public const ushort CategoryVr = 22;

	public const ushort CategoryAllocation = 23;

	public const ushort CategoryInternal = 24;

	public const ushort CategoryInput = 30;

	public const ushort CategoryVirtualTexturing = 31;

	internal const ushort CategoryAny = ushort.MaxValue;

	public static TimestampConversionRatio TimestampToNanosecondsConversionRatio
	{
		[ThreadSafe]
		get
		{
			get_TimestampToNanosecondsConversionRatio_Injected(out var ret);
			return ret;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal static extern ushort GetCategoryByName(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal unsafe static extern ushort GetCategoryByName__Unmanaged(byte* name, int nameLen);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static ushort GetCategoryByName(char* name, int nameLen)
	{
		return GetCategoryByName_Unsafe(name, nameLen);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private unsafe static extern ushort GetCategoryByName_Unsafe(char* name, int nameLen);

	[ThreadSafe]
	public static ProfilerCategoryDescription GetCategoryDescription(ushort categoryId)
	{
		GetCategoryDescription_Injected(categoryId, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern IntPtr CreateMarker(string name, ushort categoryId, MarkerFlags flags, int metadataCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal unsafe static extern IntPtr CreateMarker__Unmanaged(byte* name, int nameLen, ushort categoryId, MarkerFlags flags, int metadataCount);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static IntPtr CreateMarker(char* name, int nameLen, ushort categoryId, MarkerFlags flags, int metadataCount)
	{
		return CreateMarker_Unsafe(name, nameLen, categoryId, flags, metadataCount);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private unsafe static extern IntPtr CreateMarker_Unsafe(char* name, int nameLen, ushort categoryId, MarkerFlags flags, int metadataCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal static extern IntPtr GetMarker(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern void SetMarkerMetadata(IntPtr markerPtr, int index, string name, byte type, byte unit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal unsafe static extern void SetMarkerMetadata__Unmanaged(IntPtr markerPtr, int index, byte* name, int nameLen, byte type, byte unit);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void SetMarkerMetadata(IntPtr markerPtr, int index, char* name, int nameLen, byte type, byte unit)
	{
		SetMarkerMetadata_Unsafe(markerPtr, index, name, nameLen, type, unit);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private unsafe static extern void SetMarkerMetadata_Unsafe(IntPtr markerPtr, int index, char* name, int nameLen, byte type, byte unit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern void BeginSample(IntPtr markerPtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void BeginSampleWithMetadata(IntPtr markerPtr, int metadataCount, void* metadata);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern void EndSample(IntPtr markerPtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void SingleSampleWithMetadata(IntPtr markerPtr, int metadataCount, void* metadata);

	internal unsafe static string Utf8ToString(byte* chars, int charsLen)
	{
		if (chars == null)
		{
			return null;
		}
		byte[] array = new byte[charsLen];
		Marshal.Copy((IntPtr)chars, array, 0, charsLen);
		return Encoding.UTF8.GetString(array, 0, charsLen);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetCategoryDescription_Injected(ushort categoryId, out ProfilerCategoryDescription ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_TimestampToNanosecondsConversionRatio_Injected(out TimestampConversionRatio ret);
}
