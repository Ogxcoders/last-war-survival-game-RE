using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks;

public static class Parallel
{
	internal static int s_forkJoinContextID;

	internal const int DEFAULT_LOOP_STRIDE = 16;

	internal static readonly ParallelOptions s_defaultParallelOptions = new ParallelOptions();

	public static void Invoke(params Action[] actions)
	{
		Invoke(s_defaultParallelOptions, actions);
	}

	public static void Invoke(ParallelOptions parallelOptions, params Action[] actions)
	{
		if (actions == null)
		{
			throw new ArgumentNullException("actions");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		parallelOptions.CancellationToken.ThrowIfCancellationRequested();
		Action[] actionsCopy = new Action[actions.Length];
		for (int i = 0; i < actionsCopy.Length; i++)
		{
			actionsCopy[i] = actions[i];
			if (actionsCopy[i] == null)
			{
				throw new ArgumentException("One of the actions was null.");
			}
		}
		int forkJoinContextID = 0;
		if (ParallelEtwProvider.Log.IsEnabled())
		{
			forkJoinContextID = Interlocked.Increment(ref s_forkJoinContextID);
			ParallelEtwProvider.Log.ParallelInvokeBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelInvoke, actionsCopy.Length);
		}
		if (actionsCopy.Length < 1)
		{
			return;
		}
		try
		{
			if (actionsCopy.Length > 10 || (parallelOptions.MaxDegreeOfParallelism != -1 && parallelOptions.MaxDegreeOfParallelism < actionsCopy.Length))
			{
				ConcurrentQueue<Exception> exceptionQ = null;
				int actionIndex = 0;
				try
				{
					TaskReplicator.Run(delegate(ref object state, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
					{
						replicationDelegateYieldedBeforeCompletion = false;
						for (int num2 = Interlocked.Increment(ref actionIndex); num2 <= actionsCopy.Length; num2 = Interlocked.Increment(ref actionIndex))
						{
							try
							{
								actionsCopy[num2 - 1]();
							}
							catch (Exception item)
							{
								LazyInitializer.EnsureInitialized(ref exceptionQ, () => new ConcurrentQueue<Exception>());
								exceptionQ.Enqueue(item);
							}
							parallelOptions.CancellationToken.ThrowIfCancellationRequested();
						}
					}, parallelOptions, stopOnFirstFailure: false);
				}
				catch (Exception ex)
				{
					LazyInitializer.EnsureInitialized(ref exceptionQ, () => new ConcurrentQueue<Exception>());
					if (ex is ObjectDisposedException)
					{
						throw;
					}
					if (ex is AggregateException ex2)
					{
						foreach (Exception innerException in ex2.InnerExceptions)
						{
							exceptionQ.Enqueue(innerException);
						}
					}
					else
					{
						exceptionQ.Enqueue(ex);
					}
				}
				if (exceptionQ != null && exceptionQ.Count > 0)
				{
					ThrowSingleCancellationExceptionOrOtherException(exceptionQ, parallelOptions.CancellationToken, new AggregateException(exceptionQ));
				}
				return;
			}
			Task[] array = new Task[actionsCopy.Length];
			parallelOptions.CancellationToken.ThrowIfCancellationRequested();
			for (int num = 1; num < array.Length; num++)
			{
				array[num] = Task.Factory.StartNew(actionsCopy[num], parallelOptions.CancellationToken, TaskCreationOptions.None, parallelOptions.EffectiveTaskScheduler);
			}
			array[0] = new Task(actionsCopy[0], parallelOptions.CancellationToken, TaskCreationOptions.None);
			array[0].RunSynchronously(parallelOptions.EffectiveTaskScheduler);
			try
			{
				Task.WaitAll(array);
			}
			catch (AggregateException ex3)
			{
				ThrowSingleCancellationExceptionOrOtherException(ex3.InnerExceptions, parallelOptions.CancellationToken, ex3);
			}
		}
		finally
		{
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				ParallelEtwProvider.Log.ParallelInvokeEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
			}
		}
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForWorker<object>(fromInclusive, toExclusive, s_defaultParallelOptions, body, null, null, null, null);
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForWorker64<object>(fromInclusive, toExclusive, s_defaultParallelOptions, body, null, null, null, null);
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForWorker<object>(fromInclusive, toExclusive, parallelOptions, body, null, null, null, null);
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, body, null, null, null, null);
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForWorker<object>(fromInclusive, toExclusive, s_defaultParallelOptions, null, body, null, null, null);
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForWorker64<object>(fromInclusive, toExclusive, s_defaultParallelOptions, null, body, null, null, null);
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int, ParallelLoopState> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForWorker<object>(fromInclusive, toExclusive, parallelOptions, null, body, null, null, null);
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long, ParallelLoopState> body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, null, body, null, null, null);
	}

	public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		return ForWorker(fromInclusive, toExclusive, s_defaultParallelOptions, null, null, body, localInit, localFinally);
	}

	public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		return ForWorker64(fromInclusive, toExclusive, s_defaultParallelOptions, null, null, body, localInit, localFinally);
	}

	public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForWorker(fromInclusive, toExclusive, parallelOptions, null, null, body, localInit, localFinally);
	}

	public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForWorker64(fromInclusive, toExclusive, parallelOptions, null, null, body, localInit, localFinally);
	}

	private static bool CheckTimeoutReached(int timeoutOccursAt)
	{
		int tickCount = Environment.TickCount;
		if (tickCount < timeoutOccursAt)
		{
			return false;
		}
		if (0 > timeoutOccursAt && 0 < tickCount)
		{
			return false;
		}
		return true;
	}

	private static int ComputeTimeoutPoint(int timeoutLength)
	{
		return Environment.TickCount + timeoutLength;
	}

	private static ParallelLoopResult ForWorker<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body, Action<int, ParallelLoopState> bodyWithState, Func<int, ParallelLoopState, TLocal, TLocal> bodyWithLocal, Func<TLocal> localInit, Action<TLocal> localFinally)
	{
		ParallelLoopResult result = default(ParallelLoopResult);
		if (toExclusive <= fromInclusive)
		{
			result._completed = true;
			return result;
		}
		ParallelLoopStateFlags32 sharedPStateFlags = new ParallelLoopStateFlags32();
		parallelOptions.CancellationToken.ThrowIfCancellationRequested();
		int nNumExpectedWorkers = ((parallelOptions.EffectiveMaxConcurrencyLevel == -1) ? PlatformHelper.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel);
		RangeManager rangeManager = new RangeManager(fromInclusive, toExclusive, 1L, nNumExpectedWorkers);
		OperationCanceledException oce = null;
		CancellationTokenRegistration cancellationTokenRegistration = ((!parallelOptions.CancellationToken.CanBeCanceled) ? default(CancellationTokenRegistration) : parallelOptions.CancellationToken.Register(delegate
		{
			oce = new OperationCanceledException(parallelOptions.CancellationToken);
			sharedPStateFlags.Cancel();
		}, null, useSynchronizationContext: false));
		int forkJoinContextID = 0;
		if (ParallelEtwProvider.Log.IsEnabled())
		{
			forkJoinContextID = Interlocked.Increment(ref s_forkJoinContextID);
			ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelFor, fromInclusive, toExclusive);
		}
		try
		{
			try
			{
				TaskReplicator.Run(delegate(ref RangeWorker currentWorker, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
				{
					if (!currentWorker.IsInitialized)
					{
						currentWorker = rangeManager.RegisterNewWorker();
					}
					replicationDelegateYieldedBeforeCompletion = false;
					if (!currentWorker.FindNewWork32(out var nFromInclusiveLocal, out var nToExclusiveLocal) || sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal))
					{
						return;
					}
					if (ParallelEtwProvider.Log.IsEnabled())
					{
						ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
					}
					TLocal val = default(TLocal);
					bool flag = false;
					try
					{
						ParallelLoopState32 parallelLoopState = null;
						if (bodyWithState != null)
						{
							parallelLoopState = new ParallelLoopState32(sharedPStateFlags);
						}
						else if (bodyWithLocal != null)
						{
							parallelLoopState = new ParallelLoopState32(sharedPStateFlags);
							if (localInit != null)
							{
								val = localInit();
								flag = true;
							}
						}
						int timeoutOccursAt = ComputeTimeoutPoint(timeout);
						do
						{
							if (body != null)
							{
								for (int i = nFromInclusiveLocal; i < nToExclusiveLocal; i++)
								{
									if (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop())
									{
										break;
									}
									body(i);
								}
							}
							else if (bodyWithState != null)
							{
								for (int j = nFromInclusiveLocal; j < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(j)); j++)
								{
									parallelLoopState.CurrentIteration = j;
									bodyWithState(j, parallelLoopState);
								}
							}
							else
							{
								for (int k = nFromInclusiveLocal; k < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(k)); k++)
								{
									parallelLoopState.CurrentIteration = k;
									val = bodyWithLocal(k, parallelLoopState, val);
								}
							}
							if (CheckTimeoutReached(timeoutOccursAt))
							{
								replicationDelegateYieldedBeforeCompletion = true;
								break;
							}
						}
						while (currentWorker.FindNewWork32(out nFromInclusiveLocal, out nToExclusiveLocal) && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal)));
					}
					catch (Exception source)
					{
						sharedPStateFlags.SetExceptional();
						ExceptionDispatchInfo.Throw(source);
					}
					finally
					{
						if (localFinally != null && flag)
						{
							localFinally(val);
						}
						if (ParallelEtwProvider.Log.IsEnabled())
						{
							ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
						}
					}
				}, parallelOptions, stopOnFirstFailure: true);
			}
			finally
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
			}
			if (oce != null)
			{
				throw oce;
			}
		}
		catch (AggregateException ex)
		{
			ThrowSingleCancellationExceptionOrOtherException(ex.InnerExceptions, parallelOptions.CancellationToken, ex);
		}
		finally
		{
			int loopStateFlags = sharedPStateFlags.LoopStateFlags;
			result._completed = loopStateFlags == 0;
			if ((loopStateFlags & 2) != 0)
			{
				result._lowestBreakIteration = sharedPStateFlags.LowestBreakIteration;
			}
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				int num = 0;
				num = ((loopStateFlags == 0) ? (toExclusive - fromInclusive) : (((loopStateFlags & 2) == 0) ? (-1) : (sharedPStateFlags.LowestBreakIteration - fromInclusive)));
				ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, num);
			}
		}
		return result;
	}

	private static ParallelLoopResult ForWorker64<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body, Action<long, ParallelLoopState> bodyWithState, Func<long, ParallelLoopState, TLocal, TLocal> bodyWithLocal, Func<TLocal> localInit, Action<TLocal> localFinally)
	{
		ParallelLoopResult result = default(ParallelLoopResult);
		if (toExclusive <= fromInclusive)
		{
			result._completed = true;
			return result;
		}
		ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
		parallelOptions.CancellationToken.ThrowIfCancellationRequested();
		int nNumExpectedWorkers = ((parallelOptions.EffectiveMaxConcurrencyLevel == -1) ? PlatformHelper.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel);
		RangeManager rangeManager = new RangeManager(fromInclusive, toExclusive, 1L, nNumExpectedWorkers);
		OperationCanceledException oce = null;
		CancellationTokenRegistration cancellationTokenRegistration = ((!parallelOptions.CancellationToken.CanBeCanceled) ? default(CancellationTokenRegistration) : parallelOptions.CancellationToken.Register(delegate
		{
			oce = new OperationCanceledException(parallelOptions.CancellationToken);
			sharedPStateFlags.Cancel();
		}, null, useSynchronizationContext: false));
		int forkJoinContextID = 0;
		if (ParallelEtwProvider.Log.IsEnabled())
		{
			forkJoinContextID = Interlocked.Increment(ref s_forkJoinContextID);
			ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelFor, fromInclusive, toExclusive);
		}
		try
		{
			try
			{
				TaskReplicator.Run(delegate(ref RangeWorker currentWorker, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
				{
					if (!currentWorker.IsInitialized)
					{
						currentWorker = rangeManager.RegisterNewWorker();
					}
					replicationDelegateYieldedBeforeCompletion = false;
					if (!currentWorker.FindNewWork(out var nFromInclusiveLocal, out var nToExclusiveLocal) || sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal))
					{
						return;
					}
					if (ParallelEtwProvider.Log.IsEnabled())
					{
						ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
					}
					TLocal val = default(TLocal);
					bool flag = false;
					try
					{
						ParallelLoopState64 parallelLoopState = null;
						if (bodyWithState != null)
						{
							parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
						}
						else if (bodyWithLocal != null)
						{
							parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
							if (localInit != null)
							{
								val = localInit();
								flag = true;
							}
						}
						int timeoutOccursAt = ComputeTimeoutPoint(timeout);
						do
						{
							if (body != null)
							{
								for (long num2 = nFromInclusiveLocal; num2 < nToExclusiveLocal; num2++)
								{
									if (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop())
									{
										break;
									}
									body(num2);
								}
							}
							else if (bodyWithState != null)
							{
								for (long num3 = nFromInclusiveLocal; num3 < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(num3)); num3++)
								{
									parallelLoopState.CurrentIteration = num3;
									bodyWithState(num3, parallelLoopState);
								}
							}
							else
							{
								for (long num4 = nFromInclusiveLocal; num4 < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(num4)); num4++)
								{
									parallelLoopState.CurrentIteration = num4;
									val = bodyWithLocal(num4, parallelLoopState, val);
								}
							}
							if (CheckTimeoutReached(timeoutOccursAt))
							{
								replicationDelegateYieldedBeforeCompletion = true;
								break;
							}
						}
						while (currentWorker.FindNewWork(out nFromInclusiveLocal, out nToExclusiveLocal) && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal)));
					}
					catch (Exception source)
					{
						sharedPStateFlags.SetExceptional();
						ExceptionDispatchInfo.Throw(source);
					}
					finally
					{
						if (localFinally != null && flag)
						{
							localFinally(val);
						}
						if (ParallelEtwProvider.Log.IsEnabled())
						{
							ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
						}
					}
				}, parallelOptions, stopOnFirstFailure: true);
			}
			finally
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
			}
			if (oce != null)
			{
				throw oce;
			}
		}
		catch (AggregateException ex)
		{
			ThrowSingleCancellationExceptionOrOtherException(ex.InnerExceptions, parallelOptions.CancellationToken, ex);
		}
		finally
		{
			int loopStateFlags = sharedPStateFlags.LoopStateFlags;
			result._completed = loopStateFlags == 0;
			if ((loopStateFlags & 2) != 0)
			{
				result._lowestBreakIteration = sharedPStateFlags.LowestBreakIteration;
			}
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				long num = 0L;
				num = ((loopStateFlags == 0) ? (toExclusive - fromInclusive) : (((loopStateFlags & 2) == 0) ? (-1) : (sharedPStateFlags.LowestBreakIteration - fromInclusive)));
				ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, num);
			}
		}
		return result;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForEachWorker<TSource, object>(source, s_defaultParallelOptions, body, null, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForEachWorker<TSource, object>(source, parallelOptions, body, null, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, body, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForEachWorker<TSource, object>(source, parallelOptions, null, body, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState, long> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return ForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, null, body, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForEachWorker<TSource, object>(source, parallelOptions, null, null, body, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		return ForEachWorker(source, s_defaultParallelOptions, null, null, null, body, null, localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForEachWorker(source, parallelOptions, null, null, null, body, null, localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		return ForEachWorker(source, s_defaultParallelOptions, null, null, null, null, body, localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return ForEachWorker(source, parallelOptions, null, null, null, null, body, localInit, localFinally);
	}

	private static ParallelLoopResult ForEachWorker<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
	{
		parallelOptions.CancellationToken.ThrowIfCancellationRequested();
		if (source is TSource[] array)
		{
			return ForEachWorker(array, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
		}
		if (source is IList<TSource> list)
		{
			return ForEachWorker(list, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
		}
		return PartitionerForEachWorker(Partitioner.Create(source), parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
	}

	private static ParallelLoopResult ForEachWorker<TSource, TLocal>(TSource[] array, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
	{
		int lowerBound = array.GetLowerBound(0);
		int toExclusive = array.GetUpperBound(0) + 1;
		if (body != null)
		{
			return ForWorker<object>(lowerBound, toExclusive, parallelOptions, delegate(int i)
			{
				body(array[i]);
			}, null, null, null, null);
		}
		if (bodyWithState != null)
		{
			return ForWorker<object>(lowerBound, toExclusive, parallelOptions, null, delegate(int i, ParallelLoopState state)
			{
				bodyWithState(array[i], state);
			}, null, null, null);
		}
		if (bodyWithStateAndIndex != null)
		{
			return ForWorker<object>(lowerBound, toExclusive, parallelOptions, null, delegate(int i, ParallelLoopState state)
			{
				bodyWithStateAndIndex(array[i], state, i);
			}, null, null, null);
		}
		if (bodyWithStateAndLocal != null)
		{
			return ForWorker(lowerBound, toExclusive, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithStateAndLocal(array[i], state, local), localInit, localFinally);
		}
		return ForWorker(lowerBound, toExclusive, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithEverything(array[i], state, i, local), localInit, localFinally);
	}

	private static ParallelLoopResult ForEachWorker<TSource, TLocal>(IList<TSource> list, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
	{
		if (body != null)
		{
			return ForWorker<object>(0, list.Count, parallelOptions, delegate(int i)
			{
				body(list[i]);
			}, null, null, null, null);
		}
		if (bodyWithState != null)
		{
			return ForWorker<object>(0, list.Count, parallelOptions, null, delegate(int i, ParallelLoopState state)
			{
				bodyWithState(list[i], state);
			}, null, null, null);
		}
		if (bodyWithStateAndIndex != null)
		{
			return ForWorker<object>(0, list.Count, parallelOptions, null, delegate(int i, ParallelLoopState state)
			{
				bodyWithStateAndIndex(list[i], state, i);
			}, null, null, null);
		}
		if (bodyWithStateAndLocal != null)
		{
			return ForWorker(0, list.Count, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithStateAndLocal(list[i], state, local), localInit, localFinally);
		}
		return ForWorker(0, list.Count, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithEverything(list[i], state, i, local), localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return PartitionerForEachWorker<TSource, object>(source, s_defaultParallelOptions, body, null, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource, ParallelLoopState> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		return PartitionerForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, body, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, Action<TSource, ParallelLoopState, long> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (!source.KeysNormalized)
		{
			throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
		}
		return PartitionerForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, null, body, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		return PartitionerForEachWorker(source, s_defaultParallelOptions, null, null, null, body, null, localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (!source.KeysNormalized)
		{
			throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
		}
		return PartitionerForEachWorker(source, s_defaultParallelOptions, null, null, null, null, body, localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return PartitionerForEachWorker<TSource, object>(source, parallelOptions, body, null, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return PartitionerForEachWorker<TSource, object>(source, parallelOptions, null, body, null, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		if (!source.KeysNormalized)
		{
			throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
		}
		return PartitionerForEachWorker<TSource, object>(source, parallelOptions, null, null, body, null, null, null, null);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		return PartitionerForEachWorker(source, parallelOptions, null, null, null, body, null, localInit, localFinally);
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (localInit == null)
		{
			throw new ArgumentNullException("localInit");
		}
		if (localFinally == null)
		{
			throw new ArgumentNullException("localFinally");
		}
		if (parallelOptions == null)
		{
			throw new ArgumentNullException("parallelOptions");
		}
		if (!source.KeysNormalized)
		{
			throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
		}
		return PartitionerForEachWorker(source, parallelOptions, null, null, null, null, body, localInit, localFinally);
	}

	private static ParallelLoopResult PartitionerForEachWorker<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> simpleBody, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
	{
		OrderablePartitioner<TSource> orderedSource = source as OrderablePartitioner<TSource>;
		if (!source.SupportsDynamicPartitions)
		{
			throw new InvalidOperationException("The Partitioner used here must support dynamic partitioning.");
		}
		parallelOptions.CancellationToken.ThrowIfCancellationRequested();
		int forkJoinContextID = 0;
		if (ParallelEtwProvider.Log.IsEnabled())
		{
			forkJoinContextID = Interlocked.Increment(ref s_forkJoinContextID);
			ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelForEach, 0L, 0L);
		}
		ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
		ParallelLoopResult result = default(ParallelLoopResult);
		OperationCanceledException oce = null;
		CancellationTokenRegistration cancellationTokenRegistration = ((!parallelOptions.CancellationToken.CanBeCanceled) ? default(CancellationTokenRegistration) : parallelOptions.CancellationToken.Register(delegate
		{
			oce = new OperationCanceledException(parallelOptions.CancellationToken);
			sharedPStateFlags.Cancel();
		}, null, useSynchronizationContext: false));
		IEnumerable<TSource> partitionerSource = null;
		IEnumerable<KeyValuePair<long, TSource>> orderablePartitionerSource = null;
		if (orderedSource != null)
		{
			orderablePartitionerSource = orderedSource.GetOrderableDynamicPartitions();
			if (orderablePartitionerSource == null)
			{
				throw new InvalidOperationException("The Partitioner used here returned a null partitioner source.");
			}
		}
		else
		{
			partitionerSource = source.GetDynamicPartitions();
			if (partitionerSource == null)
			{
				throw new InvalidOperationException("The Partitioner used here returned a null partitioner source.");
			}
		}
		try
		{
			try
			{
				TaskReplicator.Run(delegate(ref IEnumerator partitionState, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
				{
					replicationDelegateYieldedBeforeCompletion = false;
					if (ParallelEtwProvider.Log.IsEnabled())
					{
						ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
					}
					TLocal val = default(TLocal);
					bool flag = false;
					try
					{
						ParallelLoopState64 parallelLoopState = null;
						if (bodyWithState != null || bodyWithStateAndIndex != null)
						{
							parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
						}
						else if (bodyWithStateAndLocal != null || bodyWithEverything != null)
						{
							parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
							if (localInit != null)
							{
								val = localInit();
								flag = true;
							}
						}
						int timeoutOccursAt = ComputeTimeoutPoint(timeout);
						if (orderedSource != null)
						{
							IEnumerator<KeyValuePair<long, TSource>> enumerator = partitionState as IEnumerator<KeyValuePair<long, TSource>>;
							if (enumerator == null)
							{
								enumerator = (IEnumerator<KeyValuePair<long, TSource>>)(partitionState = orderablePartitionerSource.GetEnumerator());
							}
							if (enumerator == null)
							{
								throw new InvalidOperationException("The Partitioner source returned a null enumerator.");
							}
							while (enumerator.MoveNext())
							{
								KeyValuePair<long, TSource> current = enumerator.Current;
								long key = current.Key;
								TSource value = current.Value;
								if (parallelLoopState != null)
								{
									parallelLoopState.CurrentIteration = key;
								}
								if (simpleBody != null)
								{
									simpleBody(value);
								}
								else if (bodyWithState != null)
								{
									bodyWithState(value, parallelLoopState);
								}
								else if (bodyWithStateAndIndex == null)
								{
									val = ((bodyWithStateAndLocal == null) ? bodyWithEverything(value, parallelLoopState, key, val) : bodyWithStateAndLocal(value, parallelLoopState, val));
								}
								else
								{
									bodyWithStateAndIndex(value, parallelLoopState, key);
								}
								if (sharedPStateFlags.ShouldExitLoop(key))
								{
									break;
								}
								if (CheckTimeoutReached(timeoutOccursAt))
								{
									replicationDelegateYieldedBeforeCompletion = true;
									break;
								}
							}
						}
						else
						{
							IEnumerator<TSource> enumerator2 = partitionState as IEnumerator<TSource>;
							if (enumerator2 == null)
							{
								enumerator2 = (IEnumerator<TSource>)(partitionState = partitionerSource.GetEnumerator());
							}
							if (enumerator2 == null)
							{
								throw new InvalidOperationException("The Partitioner source returned a null enumerator.");
							}
							if (parallelLoopState != null)
							{
								parallelLoopState.CurrentIteration = 0L;
							}
							while (enumerator2.MoveNext())
							{
								TSource current2 = enumerator2.Current;
								if (simpleBody != null)
								{
									simpleBody(current2);
								}
								else if (bodyWithState != null)
								{
									bodyWithState(current2, parallelLoopState);
								}
								else if (bodyWithStateAndLocal != null)
								{
									val = bodyWithStateAndLocal(current2, parallelLoopState, val);
								}
								if (sharedPStateFlags.LoopStateFlags != 0)
								{
									break;
								}
								if (CheckTimeoutReached(timeoutOccursAt))
								{
									replicationDelegateYieldedBeforeCompletion = true;
									break;
								}
							}
						}
					}
					catch (Exception source2)
					{
						sharedPStateFlags.SetExceptional();
						ExceptionDispatchInfo.Throw(source2);
					}
					finally
					{
						if (localFinally != null && flag)
						{
							localFinally(val);
						}
						if (!replicationDelegateYieldedBeforeCompletion && partitionState is IDisposable disposable2)
						{
							disposable2.Dispose();
						}
						if (ParallelEtwProvider.Log.IsEnabled())
						{
							ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
						}
					}
				}, parallelOptions, stopOnFirstFailure: true);
			}
			finally
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
			}
			if (oce != null)
			{
				throw oce;
			}
		}
		catch (AggregateException ex)
		{
			ThrowSingleCancellationExceptionOrOtherException(ex.InnerExceptions, parallelOptions.CancellationToken, ex);
		}
		finally
		{
			int loopStateFlags = sharedPStateFlags.LoopStateFlags;
			result._completed = loopStateFlags == 0;
			if ((loopStateFlags & 2) != 0)
			{
				result._lowestBreakIteration = sharedPStateFlags.LowestBreakIteration;
			}
			IDisposable disposable = null;
			((orderablePartitionerSource == null) ? (partitionerSource as IDisposable) : (orderablePartitionerSource as IDisposable))?.Dispose();
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, 0L);
			}
		}
		return result;
	}

	private static OperationCanceledException ReduceToSingleCancellationException(ICollection exceptions, CancellationToken cancelToken)
	{
		if (exceptions == null || exceptions.Count == 0)
		{
			return null;
		}
		if (!cancelToken.IsCancellationRequested)
		{
			return null;
		}
		Exception ex = null;
		foreach (Exception exception in exceptions)
		{
			if (ex == null)
			{
				ex = exception;
			}
			if (!(exception is OperationCanceledException ex3) || !cancelToken.Equals(ex3.CancellationToken))
			{
				return null;
			}
		}
		return (OperationCanceledException)ex;
	}

	private static void ThrowSingleCancellationExceptionOrOtherException(ICollection exceptions, CancellationToken cancelToken, Exception otherException)
	{
		ExceptionDispatchInfo.Throw(ReduceToSingleCancellationException(exceptions, cancelToken) ?? otherException);
	}
}
