using System.Collections.Generic;
using System.Threading;

namespace System.Runtime;

internal sealed class InputQueue<T> : IDisposable where T : class
{
	private enum QueueState
	{
		Open,
		Shutdown,
		Closed
	}

	private interface IQueueReader
	{
		void Set(Item item);
	}

	private interface IQueueWaiter
	{
		void Set(bool itemAvailable);
	}

	private struct Item
	{
		private Action dequeuedCallback;

		private Exception exception;

		private T value;

		public Action DequeuedCallback => dequeuedCallback;

		public Exception Exception => exception;

		public T Value => value;

		public Item(T value, Action dequeuedCallback)
			: this(value, null, dequeuedCallback)
		{
		}

		public Item(Exception exception, Action dequeuedCallback)
			: this(null, exception, dequeuedCallback)
		{
		}

		private Item(T value, Exception exception, Action dequeuedCallback)
		{
			this.value = value;
			this.exception = exception;
			this.dequeuedCallback = dequeuedCallback;
		}

		public T GetValue()
		{
			if (exception != null)
			{
				throw Fx.Exception.AsError(exception);
			}
			return value;
		}
	}

	private class AsyncQueueReader : AsyncResult, IQueueReader
	{
		private static Action<object> timerCallback = TimerCallback;

		private bool expired;

		private InputQueue<T> inputQueue;

		private T item;

		private IOThreadTimer timer;

		public AsyncQueueReader(InputQueue<T> inputQueue, TimeSpan timeout, AsyncCallback callback, object state)
			: base(callback, state)
		{
			if (inputQueue.AsyncCallbackGenerator != null)
			{
				base.VirtualCallback = inputQueue.AsyncCallbackGenerator();
			}
			this.inputQueue = inputQueue;
			if (timeout != TimeSpan.MaxValue)
			{
				timer = new IOThreadTimer(timerCallback, this, isTypicallyCanceledShortlyAfterBeingSet: false);
				timer.Set(timeout);
			}
		}

		public static bool End(IAsyncResult result, out T value)
		{
			AsyncQueueReader asyncQueueReader = AsyncResult.End<AsyncQueueReader>(result);
			if (asyncQueueReader.expired)
			{
				value = null;
				return false;
			}
			value = asyncQueueReader.item;
			return true;
		}

		public void Set(Item item)
		{
			this.item = item.Value;
			if (timer != null)
			{
				timer.Cancel();
			}
			Complete(completedSynchronously: false, item.Exception);
		}

		private static void TimerCallback(object state)
		{
			AsyncQueueReader asyncQueueReader = (AsyncQueueReader)state;
			if (asyncQueueReader.inputQueue.RemoveReader(asyncQueueReader))
			{
				asyncQueueReader.expired = true;
				asyncQueueReader.Complete(completedSynchronously: false);
			}
		}
	}

	private class AsyncQueueWaiter : AsyncResult, IQueueWaiter
	{
		private static Action<object> timerCallback = TimerCallback;

		private bool itemAvailable;

		private object thisLock = new object();

		private IOThreadTimer timer;

		private object ThisLock => thisLock;

		public AsyncQueueWaiter(TimeSpan timeout, AsyncCallback callback, object state)
			: base(callback, state)
		{
			if (timeout != TimeSpan.MaxValue)
			{
				timer = new IOThreadTimer(timerCallback, this, isTypicallyCanceledShortlyAfterBeingSet: false);
				timer.Set(timeout);
			}
		}

		public static bool End(IAsyncResult result)
		{
			return AsyncResult.End<AsyncQueueWaiter>(result).itemAvailable;
		}

		public void Set(bool itemAvailable)
		{
			bool flag;
			lock (ThisLock)
			{
				flag = timer == null || timer.Cancel();
				this.itemAvailable = itemAvailable;
			}
			if (flag)
			{
				Complete(completedSynchronously: false);
			}
		}

		private static void TimerCallback(object state)
		{
			((AsyncQueueWaiter)state).Complete(completedSynchronously: false);
		}
	}

	private class ItemQueue
	{
		private int head;

		private Item[] items;

		private int pendingCount;

		private int totalCount;

		public bool HasAnyItem => totalCount > 0;

		public bool HasAvailableItem => totalCount > pendingCount;

		public int ItemCount => totalCount;

		public ItemQueue()
		{
			items = new Item[1];
		}

		public Item DequeueAnyItem()
		{
			if (pendingCount == totalCount)
			{
				pendingCount--;
			}
			return DequeueItemCore();
		}

		public Item DequeueAvailableItem()
		{
			Fx.AssertAndThrow(totalCount != pendingCount, "ItemQueue does not contain any available items");
			return DequeueItemCore();
		}

		public void EnqueueAvailableItem(Item item)
		{
			EnqueueItemCore(item);
		}

		public void EnqueuePendingItem(Item item)
		{
			EnqueueItemCore(item);
			pendingCount++;
		}

		public void MakePendingItemAvailable()
		{
			Fx.AssertAndThrow(pendingCount != 0, "ItemQueue does not contain any pending items");
			pendingCount--;
		}

		private Item DequeueItemCore()
		{
			Fx.AssertAndThrow(totalCount != 0, "ItemQueue does not contain any items");
			Item result = items[head];
			items[head] = default(Item);
			totalCount--;
			head = (head + 1) % items.Length;
			return result;
		}

		private void EnqueueItemCore(Item item)
		{
			if (totalCount == items.Length)
			{
				Item[] array = new Item[items.Length * 2];
				for (int i = 0; i < totalCount; i++)
				{
					array[i] = items[(head + i) % items.Length];
				}
				head = 0;
				items = array;
			}
			int num = (head + totalCount) % items.Length;
			items[num] = item;
			totalCount++;
		}
	}

	private class WaitQueueReader : IQueueReader
	{
		private Exception exception;

		private InputQueue<T> inputQueue;

		private T item;

		private ManualResetEvent waitEvent;

		public WaitQueueReader(InputQueue<T> inputQueue)
		{
			this.inputQueue = inputQueue;
			waitEvent = new ManualResetEvent(initialState: false);
		}

		public void Set(Item item)
		{
			lock (this)
			{
				exception = item.Exception;
				this.item = item.Value;
				waitEvent.Set();
			}
		}

		public bool Wait(TimeSpan timeout, out T value)
		{
			bool flag = false;
			try
			{
				if (!TimeoutHelper.WaitOne(waitEvent, timeout))
				{
					if (inputQueue.RemoveReader(this))
					{
						value = null;
						flag = true;
						return false;
					}
					waitEvent.WaitOne();
				}
				flag = true;
			}
			finally
			{
				if (flag)
				{
					waitEvent.Close();
				}
			}
			if (exception != null)
			{
				throw Fx.Exception.AsError(exception);
			}
			value = item;
			return true;
		}
	}

	private class WaitQueueWaiter : IQueueWaiter
	{
		private bool itemAvailable;

		private ManualResetEvent waitEvent;

		public WaitQueueWaiter()
		{
			waitEvent = new ManualResetEvent(initialState: false);
		}

		public void Set(bool itemAvailable)
		{
			lock (this)
			{
				this.itemAvailable = itemAvailable;
				waitEvent.Set();
			}
		}

		public bool Wait(TimeSpan timeout)
		{
			if (!TimeoutHelper.WaitOne(waitEvent, timeout))
			{
				return false;
			}
			return itemAvailable;
		}
	}

	private static Action<object> completeOutstandingReadersCallback;

	private static Action<object> completeWaitersFalseCallback;

	private static Action<object> completeWaitersTrueCallback;

	private static Action<object> onDispatchCallback;

	private static Action<object> onInvokeDequeuedCallback;

	private QueueState queueState;

	private ItemQueue itemQueue;

	private Queue<IQueueReader> readerQueue;

	private List<IQueueWaiter> waiterList;

	public int PendingCount
	{
		get
		{
			lock (ThisLock)
			{
				return itemQueue.ItemCount;
			}
		}
	}

	public Action<T> DisposeItemCallback { get; set; }

	private Func<Action<AsyncCallback, IAsyncResult>> AsyncCallbackGenerator { get; set; }

	private object ThisLock => itemQueue;

	public InputQueue()
	{
		itemQueue = new ItemQueue();
		readerQueue = new Queue<IQueueReader>();
		waiterList = new List<IQueueWaiter>();
		queueState = QueueState.Open;
	}

	public InputQueue(Func<Action<AsyncCallback, IAsyncResult>> asyncCallbackGenerator)
		: this()
	{
		AsyncCallbackGenerator = asyncCallbackGenerator;
	}

	public IAsyncResult BeginDequeue(TimeSpan timeout, AsyncCallback callback, object state)
	{
		Item item = default(Item);
		lock (ThisLock)
		{
			if (queueState == QueueState.Open)
			{
				if (!itemQueue.HasAvailableItem)
				{
					AsyncQueueReader asyncQueueReader = new AsyncQueueReader(this, timeout, callback, state);
					readerQueue.Enqueue(asyncQueueReader);
					return asyncQueueReader;
				}
				item = itemQueue.DequeueAvailableItem();
			}
			else if (queueState == QueueState.Shutdown)
			{
				if (itemQueue.HasAvailableItem)
				{
					item = itemQueue.DequeueAvailableItem();
				}
				else if (itemQueue.HasAnyItem)
				{
					AsyncQueueReader asyncQueueReader2 = new AsyncQueueReader(this, timeout, callback, state);
					readerQueue.Enqueue(asyncQueueReader2);
					return asyncQueueReader2;
				}
			}
		}
		InvokeDequeuedCallback(item.DequeuedCallback);
		return new CompletedAsyncResult<T>(item.GetValue(), callback, state);
	}

	public IAsyncResult BeginWaitForItem(TimeSpan timeout, AsyncCallback callback, object state)
	{
		lock (ThisLock)
		{
			if (queueState == QueueState.Open)
			{
				if (!itemQueue.HasAvailableItem)
				{
					AsyncQueueWaiter asyncQueueWaiter = new AsyncQueueWaiter(timeout, callback, state);
					waiterList.Add(asyncQueueWaiter);
					return asyncQueueWaiter;
				}
			}
			else if (queueState == QueueState.Shutdown && !itemQueue.HasAvailableItem && itemQueue.HasAnyItem)
			{
				AsyncQueueWaiter asyncQueueWaiter2 = new AsyncQueueWaiter(timeout, callback, state);
				waiterList.Add(asyncQueueWaiter2);
				return asyncQueueWaiter2;
			}
		}
		return new CompletedAsyncResult<bool>(data: true, callback, state);
	}

	public void Close()
	{
		Dispose();
	}

	public T Dequeue(TimeSpan timeout)
	{
		if (!Dequeue(timeout, out var value))
		{
			throw Fx.Exception.AsError(new TimeoutException(InternalSR.TimeoutInputQueueDequeue(timeout)));
		}
		return value;
	}

	public bool Dequeue(TimeSpan timeout, out T value)
	{
		WaitQueueReader waitQueueReader = null;
		Item item = default(Item);
		lock (ThisLock)
		{
			if (queueState == QueueState.Open)
			{
				if (itemQueue.HasAvailableItem)
				{
					item = itemQueue.DequeueAvailableItem();
				}
				else
				{
					waitQueueReader = new WaitQueueReader(this);
					readerQueue.Enqueue(waitQueueReader);
				}
			}
			else
			{
				if (queueState != QueueState.Shutdown)
				{
					value = null;
					return true;
				}
				if (itemQueue.HasAvailableItem)
				{
					item = itemQueue.DequeueAvailableItem();
				}
				else
				{
					if (!itemQueue.HasAnyItem)
					{
						value = null;
						return true;
					}
					waitQueueReader = new WaitQueueReader(this);
					readerQueue.Enqueue(waitQueueReader);
				}
			}
		}
		if (waitQueueReader != null)
		{
			return waitQueueReader.Wait(timeout, out value);
		}
		InvokeDequeuedCallback(item.DequeuedCallback);
		value = item.GetValue();
		return true;
	}

	public void Dispatch()
	{
		IQueueReader queueReader = null;
		Item item = default(Item);
		IQueueReader[] array = null;
		IQueueWaiter[] waiters = null;
		bool itemAvailable = true;
		lock (ThisLock)
		{
			itemAvailable = queueState != QueueState.Closed && queueState != QueueState.Shutdown;
			GetWaiters(out waiters);
			if (queueState != QueueState.Closed)
			{
				itemQueue.MakePendingItemAvailable();
				if (readerQueue.Count > 0)
				{
					item = itemQueue.DequeueAvailableItem();
					queueReader = readerQueue.Dequeue();
					if (queueState == QueueState.Shutdown && readerQueue.Count > 0 && itemQueue.ItemCount == 0)
					{
						array = new IQueueReader[readerQueue.Count];
						readerQueue.CopyTo(array, 0);
						readerQueue.Clear();
						itemAvailable = false;
					}
				}
			}
		}
		if (array != null)
		{
			if (completeOutstandingReadersCallback == null)
			{
				completeOutstandingReadersCallback = CompleteOutstandingReadersCallback;
			}
			ActionItem.Schedule(completeOutstandingReadersCallback, array);
		}
		if (waiters != null)
		{
			CompleteWaitersLater(itemAvailable, waiters);
		}
		if (queueReader != null)
		{
			InvokeDequeuedCallback(item.DequeuedCallback);
			queueReader.Set(item);
		}
	}

	public bool EndDequeue(IAsyncResult result, out T value)
	{
		if (result is CompletedAsyncResult<T>)
		{
			value = CompletedAsyncResult<T>.End(result);
			return true;
		}
		return AsyncQueueReader.End(result, out value);
	}

	public T EndDequeue(IAsyncResult result)
	{
		if (!EndDequeue(result, out var value))
		{
			throw Fx.Exception.AsError(new TimeoutException());
		}
		return value;
	}

	public bool EndWaitForItem(IAsyncResult result)
	{
		if (result is CompletedAsyncResult<bool>)
		{
			return CompletedAsyncResult<bool>.End(result);
		}
		return AsyncQueueWaiter.End(result);
	}

	public void EnqueueAndDispatch(T item)
	{
		EnqueueAndDispatch(item, null);
	}

	public void EnqueueAndDispatch(T item, Action dequeuedCallback)
	{
		EnqueueAndDispatch(item, dequeuedCallback, canDispatchOnThisThread: true);
	}

	public void EnqueueAndDispatch(Exception exception, Action dequeuedCallback, bool canDispatchOnThisThread)
	{
		EnqueueAndDispatch(new Item(exception, dequeuedCallback), canDispatchOnThisThread);
	}

	public void EnqueueAndDispatch(T item, Action dequeuedCallback, bool canDispatchOnThisThread)
	{
		EnqueueAndDispatch(new Item(item, dequeuedCallback), canDispatchOnThisThread);
	}

	public bool EnqueueWithoutDispatch(T item, Action dequeuedCallback)
	{
		return EnqueueWithoutDispatch(new Item(item, dequeuedCallback));
	}

	public bool EnqueueWithoutDispatch(Exception exception, Action dequeuedCallback)
	{
		return EnqueueWithoutDispatch(new Item(exception, dequeuedCallback));
	}

	public void Shutdown()
	{
		Shutdown(null);
	}

	public void Shutdown(Func<Exception> pendingExceptionGenerator)
	{
		IQueueReader[] array = null;
		lock (ThisLock)
		{
			if (queueState == QueueState.Shutdown || queueState == QueueState.Closed)
			{
				return;
			}
			queueState = QueueState.Shutdown;
			if (readerQueue.Count > 0 && itemQueue.ItemCount == 0)
			{
				array = new IQueueReader[readerQueue.Count];
				readerQueue.CopyTo(array, 0);
				readerQueue.Clear();
			}
		}
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Exception exception = pendingExceptionGenerator?.Invoke();
				array[i].Set(new Item(exception, null));
			}
		}
	}

	public bool WaitForItem(TimeSpan timeout)
	{
		WaitQueueWaiter waitQueueWaiter = null;
		bool flag = false;
		lock (ThisLock)
		{
			if (queueState == QueueState.Open)
			{
				if (itemQueue.HasAvailableItem)
				{
					flag = true;
				}
				else
				{
					waitQueueWaiter = new WaitQueueWaiter();
					waiterList.Add(waitQueueWaiter);
				}
			}
			else
			{
				if (queueState != QueueState.Shutdown)
				{
					return true;
				}
				if (itemQueue.HasAvailableItem)
				{
					flag = true;
				}
				else
				{
					if (!itemQueue.HasAnyItem)
					{
						return true;
					}
					waitQueueWaiter = new WaitQueueWaiter();
					waiterList.Add(waitQueueWaiter);
				}
			}
		}
		return waitQueueWaiter?.Wait(timeout) ?? flag;
	}

	public void Dispose()
	{
		bool flag = false;
		lock (ThisLock)
		{
			if (queueState != QueueState.Closed)
			{
				queueState = QueueState.Closed;
				flag = true;
			}
		}
		if (flag)
		{
			while (readerQueue.Count > 0)
			{
				readerQueue.Dequeue().Set(default(Item));
			}
			while (itemQueue.HasAnyItem)
			{
				Item item = itemQueue.DequeueAnyItem();
				DisposeItem(item);
				InvokeDequeuedCallback(item.DequeuedCallback);
			}
		}
	}

	private void DisposeItem(Item item)
	{
		T value = item.Value;
		if (value != null)
		{
			if (value is IDisposable)
			{
				((IDisposable)value).Dispose();
			}
			else
			{
				DisposeItemCallback?.Invoke(value);
			}
		}
	}

	private static void CompleteOutstandingReadersCallback(object state)
	{
		IQueueReader[] array = (IQueueReader[])state;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Set(default(Item));
		}
	}

	private static void CompleteWaiters(bool itemAvailable, IQueueWaiter[] waiters)
	{
		for (int i = 0; i < waiters.Length; i++)
		{
			waiters[i].Set(itemAvailable);
		}
	}

	private static void CompleteWaitersFalseCallback(object state)
	{
		CompleteWaiters(itemAvailable: false, (IQueueWaiter[])state);
	}

	private static void CompleteWaitersLater(bool itemAvailable, IQueueWaiter[] waiters)
	{
		if (itemAvailable)
		{
			if (completeWaitersTrueCallback == null)
			{
				completeWaitersTrueCallback = CompleteWaitersTrueCallback;
			}
			ActionItem.Schedule(completeWaitersTrueCallback, waiters);
		}
		else
		{
			if (completeWaitersFalseCallback == null)
			{
				completeWaitersFalseCallback = CompleteWaitersFalseCallback;
			}
			ActionItem.Schedule(completeWaitersFalseCallback, waiters);
		}
	}

	private static void CompleteWaitersTrueCallback(object state)
	{
		CompleteWaiters(itemAvailable: true, (IQueueWaiter[])state);
	}

	private static void InvokeDequeuedCallback(Action dequeuedCallback)
	{
		dequeuedCallback?.Invoke();
	}

	private static void InvokeDequeuedCallbackLater(Action dequeuedCallback)
	{
		if (dequeuedCallback != null)
		{
			if (onInvokeDequeuedCallback == null)
			{
				onInvokeDequeuedCallback = OnInvokeDequeuedCallback;
			}
			ActionItem.Schedule(onInvokeDequeuedCallback, dequeuedCallback);
		}
	}

	private static void OnDispatchCallback(object state)
	{
		((InputQueue<T>)state).Dispatch();
	}

	private static void OnInvokeDequeuedCallback(object state)
	{
		((Action)state)();
	}

	private void EnqueueAndDispatch(Item item, bool canDispatchOnThisThread)
	{
		bool flag = false;
		IQueueReader queueReader = null;
		bool flag2 = false;
		IQueueWaiter[] waiters = null;
		bool itemAvailable = true;
		lock (ThisLock)
		{
			itemAvailable = queueState != QueueState.Closed && queueState != QueueState.Shutdown;
			GetWaiters(out waiters);
			if (queueState == QueueState.Open)
			{
				if (canDispatchOnThisThread)
				{
					if (readerQueue.Count == 0)
					{
						itemQueue.EnqueueAvailableItem(item);
					}
					else
					{
						queueReader = readerQueue.Dequeue();
					}
				}
				else if (readerQueue.Count == 0)
				{
					itemQueue.EnqueueAvailableItem(item);
				}
				else
				{
					itemQueue.EnqueuePendingItem(item);
					flag2 = true;
				}
			}
			else
			{
				flag = true;
			}
		}
		if (waiters != null)
		{
			if (canDispatchOnThisThread)
			{
				CompleteWaiters(itemAvailable, waiters);
			}
			else
			{
				CompleteWaitersLater(itemAvailable, waiters);
			}
		}
		if (queueReader != null)
		{
			InvokeDequeuedCallback(item.DequeuedCallback);
			queueReader.Set(item);
		}
		if (flag2)
		{
			if (onDispatchCallback == null)
			{
				onDispatchCallback = OnDispatchCallback;
			}
			ActionItem.Schedule(onDispatchCallback, this);
		}
		else if (flag)
		{
			InvokeDequeuedCallback(item.DequeuedCallback);
			DisposeItem(item);
		}
	}

	private bool EnqueueWithoutDispatch(Item item)
	{
		lock (ThisLock)
		{
			if (queueState != QueueState.Closed && queueState != QueueState.Shutdown)
			{
				if (readerQueue.Count == 0 && waiterList.Count == 0)
				{
					itemQueue.EnqueueAvailableItem(item);
					return false;
				}
				itemQueue.EnqueuePendingItem(item);
				return true;
			}
		}
		DisposeItem(item);
		InvokeDequeuedCallbackLater(item.DequeuedCallback);
		return false;
	}

	private void GetWaiters(out IQueueWaiter[] waiters)
	{
		if (waiterList.Count > 0)
		{
			waiters = waiterList.ToArray();
			waiterList.Clear();
		}
		else
		{
			waiters = null;
		}
	}

	private bool RemoveReader(IQueueReader reader)
	{
		lock (ThisLock)
		{
			if (queueState == QueueState.Open || queueState == QueueState.Shutdown)
			{
				bool result = false;
				for (int num = readerQueue.Count; num > 0; num--)
				{
					IQueueReader queueReader = readerQueue.Dequeue();
					if (queueReader == reader)
					{
						result = true;
					}
					else
					{
						readerQueue.Enqueue(queueReader);
					}
				}
				return result;
			}
		}
		return false;
	}
}
