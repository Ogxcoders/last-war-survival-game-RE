#define UNITY_ASSERTIONS
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.UIElements.UIR;

internal class Page : IDisposable
{
	public class DataSet<T> : IDisposable where T : struct
	{
		public Utility.GPUBuffer<T> gpuData;

		public NativeArray<T> cpuData;

		public NativeArray<GfxUpdateBufferRange> updateRanges;

		public GPUBufferAllocator allocator;

		private readonly uint m_UpdateRangePoolSize;

		private uint m_ElemStride;

		private uint m_UpdateRangeMin;

		private uint m_UpdateRangeMax;

		private uint m_UpdateRangesEnqueued;

		private uint m_UpdateRangesBatchStart;

		private bool m_UpdateRangesSaturated;

		protected bool disposed { get; private set; }

		public DataSet(Utility.GPUBufferType bufferType, uint totalCount, uint maxQueuedFrameCount, uint updateRangePoolSize, bool mockBuffer)
		{
			if (!mockBuffer)
			{
				gpuData = new Utility.GPUBuffer<T>((int)totalCount, bufferType);
			}
			cpuData = new NativeArray<T>((int)totalCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			allocator = new GPUBufferAllocator(totalCount);
			if (!mockBuffer)
			{
				m_ElemStride = (uint)gpuData.ElementStride;
			}
			m_UpdateRangePoolSize = updateRangePoolSize;
			uint length = m_UpdateRangePoolSize * maxQueuedFrameCount;
			updateRanges = new NativeArray<GfxUpdateBufferRange>((int)length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			m_UpdateRangeMin = uint.MaxValue;
			m_UpdateRangeMax = 0u;
			m_UpdateRangesEnqueued = 0u;
			m_UpdateRangesBatchStart = 0u;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					gpuData?.Dispose();
					cpuData.Dispose();
					updateRanges.Dispose();
				}
				disposed = true;
			}
		}

		public unsafe void RegisterUpdate(uint start, uint size)
		{
			Debug.Assert(start + size <= cpuData.Length);
			int num = (int)(m_UpdateRangesBatchStart + m_UpdateRangesEnqueued);
			if (m_UpdateRangesEnqueued != 0)
			{
				int index = num - 1;
				GfxUpdateBufferRange gfxUpdateBufferRange = updateRanges[index];
				uint num2 = start * m_ElemStride;
				if (gfxUpdateBufferRange.offsetFromWriteStart + gfxUpdateBufferRange.size == num2)
				{
					updateRanges[index] = new GfxUpdateBufferRange
					{
						source = gfxUpdateBufferRange.source,
						offsetFromWriteStart = gfxUpdateBufferRange.offsetFromWriteStart,
						size = gfxUpdateBufferRange.size + size * m_ElemStride
					};
					m_UpdateRangeMax = Math.Max(m_UpdateRangeMax, start + size);
					return;
				}
			}
			m_UpdateRangeMin = Math.Min(m_UpdateRangeMin, start);
			m_UpdateRangeMax = Math.Max(m_UpdateRangeMax, start + size);
			if (m_UpdateRangesEnqueued == m_UpdateRangePoolSize)
			{
				m_UpdateRangesSaturated = true;
				return;
			}
			UIntPtr source = new UIntPtr(cpuData.Slice((int)start, (int)size).GetUnsafeReadOnlyPtr());
			updateRanges[num] = new GfxUpdateBufferRange
			{
				source = source,
				offsetFromWriteStart = start * m_ElemStride,
				size = size * m_ElemStride
			};
			m_UpdateRangesEnqueued++;
		}

		public unsafe void SendUpdates()
		{
			if (m_UpdateRangesEnqueued == 0)
			{
				return;
			}
			if (m_UpdateRangesSaturated)
			{
				uint num = m_UpdateRangeMax - m_UpdateRangeMin;
				m_UpdateRangesEnqueued = 1u;
				updateRanges[(int)m_UpdateRangesBatchStart] = new GfxUpdateBufferRange
				{
					source = new UIntPtr(cpuData.Slice((int)m_UpdateRangeMin, (int)num).GetUnsafeReadOnlyPtr()),
					offsetFromWriteStart = m_UpdateRangeMin * m_ElemStride,
					size = num * m_ElemStride
				};
			}
			uint num2 = m_UpdateRangeMin * m_ElemStride;
			uint rangesMax = m_UpdateRangeMax * m_ElemStride;
			if (num2 != 0)
			{
				for (uint num3 = 0u; num3 < m_UpdateRangesEnqueued; num3++)
				{
					int index = (int)(num3 + m_UpdateRangesBatchStart);
					updateRanges[index] = new GfxUpdateBufferRange
					{
						source = updateRanges[index].source,
						offsetFromWriteStart = updateRanges[index].offsetFromWriteStart - num2,
						size = updateRanges[index].size
					};
				}
			}
			gpuData?.UpdateRanges(updateRanges.Slice((int)m_UpdateRangesBatchStart, (int)m_UpdateRangesEnqueued), (int)num2, (int)rangesMax);
			m_UpdateRangeMin = uint.MaxValue;
			m_UpdateRangeMax = 0u;
			m_UpdateRangesEnqueued = 0u;
			m_UpdateRangesBatchStart += m_UpdateRangePoolSize;
			if (m_UpdateRangesBatchStart >= updateRanges.Length)
			{
				m_UpdateRangesBatchStart = 0u;
			}
			m_UpdateRangesSaturated = false;
		}
	}

	public DataSet<Vertex> vertices;

	public DataSet<ushort> indices;

	public Page next;

	public int framesEmpty;

	protected bool disposed { get; private set; }

	public bool isEmpty => vertices.allocator.isEmpty && indices.allocator.isEmpty;

	public Page(uint vertexMaxCount, uint indexMaxCount, uint maxQueuedFrameCount, bool mockPage)
	{
		vertexMaxCount = Math.Min(vertexMaxCount, 65536u);
		vertices = new DataSet<Vertex>(Utility.GPUBufferType.Vertex, vertexMaxCount, maxQueuedFrameCount, 32u, mockPage);
		indices = new DataSet<ushort>(Utility.GPUBufferType.Index, indexMaxCount, maxQueuedFrameCount, 32u, mockPage);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				indices.Dispose();
				vertices.Dispose();
			}
			disposed = true;
		}
	}
}
