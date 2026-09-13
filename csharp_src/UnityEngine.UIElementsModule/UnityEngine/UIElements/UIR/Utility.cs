#define UNITY_ASSERTIONS
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.UIElements.UIR;

[NativeHeader("Modules/UIElements/UIRendererUtility.h")]
internal class Utility
{
	internal enum GPUBufferType
	{
		Vertex,
		Index
	}

	public class GPUBuffer<T> : IDisposable where T : struct
	{
		private IntPtr buffer;

		private int elemCount;

		private int elemStride;

		public int ElementStride => elemStride;

		public int Count => elemCount;

		internal IntPtr BufferPointer => buffer;

		public GPUBuffer(int elementCount, GPUBufferType type)
		{
			elemCount = elementCount;
			elemStride = UnsafeUtility.SizeOf<T>();
			buffer = AllocateBuffer(elementCount, elemStride, type == GPUBufferType.Vertex);
		}

		public void Dispose()
		{
			FreeBuffer(buffer);
		}

		public unsafe void UpdateRanges(NativeSlice<GfxUpdateBufferRange> ranges, int rangesMin, int rangesMax)
		{
			UpdateBufferRanges(buffer, new IntPtr(ranges.GetUnsafePtr()), ranges.Length, rangesMin, rangesMax);
		}
	}

	private static ProfilerMarker s_MarkerRaiseEngineUpdate = new ProfilerMarker("UIR.RaiseEngineUpdate");

	public static event Action<bool> GraphicsResourcesRecreate;

	public static event Action EngineUpdate;

	public static event Action FlushPendingResources;

	public unsafe static void DrawRanges<I, T>(GPUBuffer<I> ib, GPUBuffer<T> vb, NativeSlice<DrawBufferRange> ranges) where I : struct where T : struct
	{
		Debug.Assert(ib.ElementStride == 2);
		DrawRanges(ib.BufferPointer, vb.BufferPointer, vb.ElementStride, new IntPtr(ranges.GetUnsafePtr()), ranges.Length);
	}

	public unsafe static void SetVectorArray<T>(Material mat, int name, NativeSlice<T> vector4s) where T : struct
	{
		int count = vector4s.Length * vector4s.Stride / 16;
		SetVectorArray(mat, name, new IntPtr(vector4s.GetUnsafePtr()), count);
	}

	[RequiredByNativeCode]
	internal static void RaiseGraphicsResourcesRecreate(bool recreate)
	{
		Utility.GraphicsResourcesRecreate?.Invoke(recreate);
	}

	[RequiredByNativeCode]
	internal static void RaiseEngineUpdate()
	{
		if (Utility.EngineUpdate != null)
		{
			Utility.EngineUpdate();
		}
	}

	[RequiredByNativeCode]
	internal static void RaiseFlushPendingResources()
	{
		Utility.FlushPendingResources?.Invoke();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr AllocateBuffer(int elementCount, int elementStride, bool vertexBuffer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void FreeBuffer(IntPtr buffer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateBufferRanges(IntPtr buffer, IntPtr ranges, int rangeCount, int writeRangeStart, int writeRangeEnd);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DrawRanges(IntPtr ib, IntPtr vb, int vbElemStride, IntPtr ranges, int rangeCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetVectorArray(Material mat, int name, IntPtr vector4s, int count);

	public static void SetScissorRect(RectInt scissorRect)
	{
		SetScissorRect_Injected(ref scissorRect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void DisableScissor();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool IsScissorEnabled();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern uint InsertCPUFence();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool CPUFencePassed(uint fence);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void WaitForCPUFencePassed(uint fence);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void SyncRenderThread();

	public static RectInt GetActiveViewport()
	{
		GetActiveViewport_Injected(out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void ProfileDrawChainBegin();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void ProfileDrawChainEnd();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void ProfileImmediateRendererBegin();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void ProfileImmediateRendererEnd();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void NotifyOfUIREvents(bool subscribe);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool GetInvertProjectionMatrix();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetScissorRect_Injected(ref RectInt scissorRect);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetActiveViewport_Injected(out RectInt ret);
}
