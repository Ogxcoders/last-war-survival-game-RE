using System.Collections.Generic;
using System.Threading;

namespace System.Runtime;

internal abstract class InternalBufferManager
{
	private class PooledBufferManager : InternalBufferManager
	{
		private abstract class BufferPool
		{
			private class SynchronizedBufferPool : BufferPool
			{
				private SynchronizedPool<byte[]> innerPool;

				internal SynchronizedBufferPool(int bufferSize, int limit)
					: base(bufferSize, limit)
				{
					innerPool = new SynchronizedPool<byte[]>(limit);
				}

				internal override void OnClear()
				{
					innerPool.Clear();
				}

				internal override byte[] Take()
				{
					return innerPool.Take();
				}

				internal override bool Return(byte[] buffer)
				{
					return innerPool.Return(buffer);
				}
			}

			private class LargeBufferPool : BufferPool
			{
				private Stack<byte[]> items;

				private object ThisLock => items;

				internal LargeBufferPool(int bufferSize, int limit)
					: base(bufferSize, limit)
				{
					items = new Stack<byte[]>(limit);
				}

				internal override void OnClear()
				{
					lock (ThisLock)
					{
						items.Clear();
					}
				}

				internal override byte[] Take()
				{
					lock (ThisLock)
					{
						if (items.Count > 0)
						{
							return items.Pop();
						}
					}
					return null;
				}

				internal override bool Return(byte[] buffer)
				{
					lock (ThisLock)
					{
						if (items.Count < base.Limit)
						{
							items.Push(buffer);
							return true;
						}
					}
					return false;
				}
			}

			private int bufferSize;

			private int count;

			private int limit;

			private int misses;

			private int peak;

			public int BufferSize => bufferSize;

			public int Limit => limit;

			public int Misses
			{
				get
				{
					return misses;
				}
				set
				{
					misses = value;
				}
			}

			public int Peak => peak;

			public BufferPool(int bufferSize, int limit)
			{
				this.bufferSize = bufferSize;
				this.limit = limit;
			}

			public void Clear()
			{
				OnClear();
				count = 0;
			}

			public void DecrementCount()
			{
				int num = count - 1;
				if (num >= 0)
				{
					count = num;
				}
			}

			public void IncrementCount()
			{
				int num = count + 1;
				if (num <= limit)
				{
					count = num;
					if (num > peak)
					{
						peak = num;
					}
				}
			}

			internal abstract byte[] Take();

			internal abstract bool Return(byte[] buffer);

			internal abstract void OnClear();

			internal static BufferPool CreatePool(int bufferSize, int limit)
			{
				if (bufferSize < 85000)
				{
					return new SynchronizedBufferPool(bufferSize, limit);
				}
				return new LargeBufferPool(bufferSize, limit);
			}
		}

		private const int minBufferSize = 128;

		private const int maxMissesBeforeTuning = 8;

		private const int initialBufferCount = 1;

		private readonly object tuningLock;

		private int[] bufferSizes;

		private BufferPool[] bufferPools;

		private long memoryLimit;

		private long remainingMemory;

		private bool areQuotasBeingTuned;

		private int totalMisses;

		public PooledBufferManager(long maxMemoryToPool, int maxBufferSize)
		{
			tuningLock = new object();
			memoryLimit = maxMemoryToPool;
			remainingMemory = maxMemoryToPool;
			List<BufferPool> list = new List<BufferPool>();
			int num = 128;
			while (true)
			{
				long num2 = remainingMemory / num;
				int num3 = (int)((num2 > int.MaxValue) ? int.MaxValue : num2);
				if (num3 > 1)
				{
					num3 = 1;
				}
				list.Add(BufferPool.CreatePool(num, num3));
				remainingMemory -= (long)num3 * (long)num;
				if (num >= maxBufferSize)
				{
					break;
				}
				long num4 = (long)num * 2L;
				num = (int)((num4 <= maxBufferSize) ? num4 : maxBufferSize);
			}
			bufferPools = list.ToArray();
			bufferSizes = new int[bufferPools.Length];
			for (int i = 0; i < bufferPools.Length; i++)
			{
				bufferSizes[i] = bufferPools[i].BufferSize;
			}
		}

		public override void Clear()
		{
			for (int i = 0; i < bufferPools.Length; i++)
			{
				bufferPools[i].Clear();
			}
		}

		private void ChangeQuota(ref BufferPool bufferPool, int delta)
		{
			if (TraceCore.BufferPoolChangeQuotaIsEnabled(Fx.Trace))
			{
				TraceCore.BufferPoolChangeQuota(Fx.Trace, bufferPool.BufferSize, delta);
			}
			BufferPool bufferPool2 = bufferPool;
			int num = bufferPool2.Limit + delta;
			BufferPool bufferPool3 = BufferPool.CreatePool(bufferPool2.BufferSize, num);
			for (int i = 0; i < num; i++)
			{
				byte[] array = bufferPool2.Take();
				if (array == null)
				{
					break;
				}
				bufferPool3.Return(array);
				bufferPool3.IncrementCount();
			}
			remainingMemory -= bufferPool2.BufferSize * delta;
			bufferPool = bufferPool3;
		}

		private void DecreaseQuota(ref BufferPool bufferPool)
		{
			ChangeQuota(ref bufferPool, -1);
		}

		private int FindMostExcessivePool()
		{
			long num = 0L;
			int result = -1;
			for (int i = 0; i < bufferPools.Length; i++)
			{
				BufferPool bufferPool = bufferPools[i];
				if (bufferPool.Peak < bufferPool.Limit)
				{
					long num2 = (long)(bufferPool.Limit - bufferPool.Peak) * (long)bufferPool.BufferSize;
					if (num2 > num)
					{
						result = i;
						num = num2;
					}
				}
			}
			return result;
		}

		private int FindMostStarvedPool()
		{
			long num = 0L;
			int result = -1;
			for (int i = 0; i < bufferPools.Length; i++)
			{
				BufferPool bufferPool = bufferPools[i];
				if (bufferPool.Peak == bufferPool.Limit)
				{
					long num2 = (long)bufferPool.Misses * (long)bufferPool.BufferSize;
					if (num2 > num)
					{
						result = i;
						num = num2;
					}
				}
			}
			return result;
		}

		private BufferPool FindPool(int desiredBufferSize)
		{
			for (int i = 0; i < bufferSizes.Length; i++)
			{
				if (desiredBufferSize <= bufferSizes[i])
				{
					return bufferPools[i];
				}
			}
			return null;
		}

		private void IncreaseQuota(ref BufferPool bufferPool)
		{
			ChangeQuota(ref bufferPool, 1);
		}

		public override void ReturnBuffer(byte[] buffer)
		{
			BufferPool bufferPool = FindPool(buffer.Length);
			if (bufferPool != null)
			{
				if (buffer.Length != bufferPool.BufferSize)
				{
					throw Fx.Exception.Argument("buffer", "Buffer Is Not Right Size For Buffer Manager");
				}
				if (bufferPool.Return(buffer))
				{
					bufferPool.IncrementCount();
				}
			}
		}

		public override byte[] TakeBuffer(int bufferSize)
		{
			BufferPool bufferPool = FindPool(bufferSize);
			if (bufferPool != null)
			{
				byte[] array = bufferPool.Take();
				if (array != null)
				{
					bufferPool.DecrementCount();
					return array;
				}
				if (bufferPool.Peak == bufferPool.Limit)
				{
					bufferPool.Misses++;
					if (++totalMisses >= 8)
					{
						TuneQuotas();
					}
				}
				if (TraceCore.BufferPoolAllocationIsEnabled(Fx.Trace))
				{
					TraceCore.BufferPoolAllocation(Fx.Trace, bufferPool.BufferSize);
				}
				return Fx.AllocateByteArray(bufferPool.BufferSize);
			}
			if (TraceCore.BufferPoolAllocationIsEnabled(Fx.Trace))
			{
				TraceCore.BufferPoolAllocation(Fx.Trace, bufferSize);
			}
			return Fx.AllocateByteArray(bufferSize);
		}

		private void TuneQuotas()
		{
			if (areQuotasBeingTuned)
			{
				return;
			}
			bool lockTaken = false;
			try
			{
				Monitor.TryEnter(tuningLock, ref lockTaken);
				if (!lockTaken || areQuotasBeingTuned)
				{
					return;
				}
				areQuotasBeingTuned = true;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(tuningLock);
				}
			}
			int num = FindMostStarvedPool();
			if (num >= 0)
			{
				BufferPool bufferPool = bufferPools[num];
				if (remainingMemory < bufferPool.BufferSize)
				{
					int num2 = FindMostExcessivePool();
					if (num2 >= 0)
					{
						DecreaseQuota(ref bufferPools[num2]);
					}
				}
				if (remainingMemory >= bufferPool.BufferSize)
				{
					IncreaseQuota(ref bufferPools[num]);
				}
			}
			for (int i = 0; i < bufferPools.Length; i++)
			{
				bufferPools[i].Misses = 0;
			}
			totalMisses = 0;
			areQuotasBeingTuned = false;
		}
	}

	private class GCBufferManager : InternalBufferManager
	{
		private static GCBufferManager value = new GCBufferManager();

		public static GCBufferManager Value => value;

		private GCBufferManager()
		{
		}

		public override void Clear()
		{
		}

		public override byte[] TakeBuffer(int bufferSize)
		{
			return Fx.AllocateByteArray(bufferSize);
		}

		public override void ReturnBuffer(byte[] buffer)
		{
		}
	}

	public abstract byte[] TakeBuffer(int bufferSize);

	public abstract void ReturnBuffer(byte[] buffer);

	public abstract void Clear();

	public static InternalBufferManager Create(long maxBufferPoolSize, int maxBufferSize)
	{
		if (maxBufferPoolSize == 0L)
		{
			return GCBufferManager.Value;
		}
		return new PooledBufferManager(maxBufferPoolSize, maxBufferSize);
	}
}
