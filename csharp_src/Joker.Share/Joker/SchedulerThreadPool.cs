using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Joker;

public class SchedulerThreadPool : SchedulerThreadBase
{
	private class Holder : SchedulerThreadSingle
	{
		private int _ref;

		public int Index { get; private set; }

		public Holder(int index)
			: base(ESchedulerType.PooledThread, -1)
		{
			Index = index;
		}

		public override void Start()
		{
			base.Start();
			_thread.Name = "ThreadPool_" + Index;
		}

		public void AddRef()
		{
			Interlocked.Increment(ref _ref);
		}

		public void DelRef()
		{
			Interlocked.Decrement(ref _ref);
		}

		public bool IsBusy()
		{
			return Interlocked.CompareExchange(ref _ref, 0, 0) != 0;
		}

		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				BaseDispose();
			}
		}

		public void Shutdown()
		{
			base.Dispose();
		}
	}

	internal static int PoolSize = Environment.ProcessorCount * 2;

	private static object _holderLock = new object();

	private static Holder[] _holders = null;

	private Holder _holder;

	private readonly int _poolIndex;

	public SchedulerThreadPool(ESchedulerType type, int id)
		: base(type, id)
	{
		int poolIndex = Math.Abs(RuntimeHelpers.GetHashCode(this)) % PoolSize;
		_poolIndex = poolIndex;
	}

	public override void Start()
	{
		if (_holder != null)
		{
			throw new InvalidOperationException("Already started");
		}
		_holder = AcquireHolder(_poolIndex);
		_holder.Updater += Update;
		_holder.LateUpdater += LateUpdate;
	}

	internal override void Update()
	{
		if (!base.IsDisposed)
		{
			base.Update();
		}
	}

	internal override void LateUpdate()
	{
		if (base.IsDisposed)
		{
			base.LateUpdate();
		}
	}

	public override void Dispose()
	{
		if (!base.IsDisposed)
		{
			base.Dispose();
			if (_holder != null)
			{
				_holder.Updater -= Update;
				_holder.LateUpdater -= LateUpdate;
				ReleaseHolder(_poolIndex, _holder);
				_holder = null;
			}
		}
	}

	private static Holder AcquireHolder(int index)
	{
		lock (_holderLock)
		{
			_holders = _holders ?? new Holder[PoolSize];
			if (_holders.Length < index)
			{
				throw new IndexOutOfRangeException($"PoolSize {PoolSize} is too small for index {index}. Maybe changed in runtime.");
			}
			Holder holder = _holders[index];
			if (holder == null)
			{
				holder = new Holder(index);
				_holders[index] = holder;
				holder.Start();
			}
			holder.AddRef();
			return holder;
		}
	}

	private static void ReleaseHolder(int index, Holder holder)
	{
		lock (_holderLock)
		{
			holder.DelRef();
			if (!holder.IsBusy())
			{
				holder.Dispose();
				_holders[index] = null;
			}
		}
	}

	public static void WaitForShutdown()
	{
		lock (_holderLock)
		{
			if (_holders != null)
			{
				for (int i = 0; i < _holders.Length; i++)
				{
					_holders[i]?.Dispose();
				}
			}
		}
	}
}
