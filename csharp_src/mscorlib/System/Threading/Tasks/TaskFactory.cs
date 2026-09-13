using System.Collections.Generic;
using Internal.Runtime.Augments;

namespace System.Threading.Tasks;

public class TaskFactory<TResult>
{
	private sealed class FromAsyncTrimPromise<TInstance> : Task<TResult> where TInstance : class
	{
		internal static readonly AsyncCallback s_completeFromAsyncResult = CompleteFromAsyncResult;

		private TInstance m_thisRef;

		private Func<TInstance, IAsyncResult, TResult> m_endMethod;

		internal FromAsyncTrimPromise(TInstance thisRef, Func<TInstance, IAsyncResult, TResult> endMethod)
		{
			m_thisRef = thisRef;
			m_endMethod = endMethod;
		}

		internal static void CompleteFromAsyncResult(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult.AsyncState is FromAsyncTrimPromise<TInstance> { m_thisRef: var thisRef, m_endMethod: var endMethod } fromAsyncTrimPromise))
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.", "asyncResult");
			}
			fromAsyncTrimPromise.m_thisRef = null;
			fromAsyncTrimPromise.m_endMethod = null;
			if (endMethod == null)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.", "asyncResult");
			}
			if (!asyncResult.CompletedSynchronously)
			{
				fromAsyncTrimPromise.Complete(thisRef, endMethod, asyncResult, requiresSynchronization: true);
			}
		}

		internal void Complete(TInstance thisRef, Func<TInstance, IAsyncResult, TResult> endMethod, IAsyncResult asyncResult, bool requiresSynchronization)
		{
			try
			{
				TResult result = endMethod(thisRef, asyncResult);
				if (requiresSynchronization)
				{
					TrySetResult(result);
				}
				else
				{
					DangerousSetResult(result);
				}
			}
			catch (OperationCanceledException ex)
			{
				TrySetCanceled(ex.CancellationToken, ex);
			}
			catch (Exception exceptionObject)
			{
				TrySetException(exceptionObject);
			}
		}
	}

	private CancellationToken m_defaultCancellationToken;

	private TaskScheduler m_defaultScheduler;

	private TaskCreationOptions m_defaultCreationOptions;

	private TaskContinuationOptions m_defaultContinuationOptions;

	private TaskScheduler DefaultScheduler
	{
		get
		{
			if (m_defaultScheduler == null)
			{
				return TaskScheduler.Current;
			}
			return m_defaultScheduler;
		}
	}

	public CancellationToken CancellationToken => m_defaultCancellationToken;

	public TaskScheduler Scheduler => m_defaultScheduler;

	public TaskCreationOptions CreationOptions => m_defaultCreationOptions;

	public TaskContinuationOptions ContinuationOptions => m_defaultContinuationOptions;

	private TaskScheduler GetDefaultScheduler(Task currTask)
	{
		if (m_defaultScheduler != null)
		{
			return m_defaultScheduler;
		}
		if (currTask != null && (currTask.CreationOptions & TaskCreationOptions.HideScheduler) == 0)
		{
			return currTask.ExecutingTaskScheduler;
		}
		return TaskScheduler.Default;
	}

	public TaskFactory()
		: this(default(CancellationToken), TaskCreationOptions.None, TaskContinuationOptions.None, (TaskScheduler)null)
	{
	}

	public TaskFactory(CancellationToken cancellationToken)
		: this(cancellationToken, TaskCreationOptions.None, TaskContinuationOptions.None, (TaskScheduler)null)
	{
	}

	public TaskFactory(TaskScheduler scheduler)
		: this(default(CancellationToken), TaskCreationOptions.None, TaskContinuationOptions.None, scheduler)
	{
	}

	public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
		: this(default(CancellationToken), creationOptions, continuationOptions, (TaskScheduler)null)
	{
	}

	public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		TaskFactory.CheckCreationOptions(creationOptions);
		m_defaultCancellationToken = cancellationToken;
		m_defaultScheduler = scheduler;
		m_defaultCreationOptions = creationOptions;
		m_defaultContinuationOptions = continuationOptions;
	}

	public Task<TResult> StartNew(Func<TResult> function)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew(Func<TResult> function, TaskCreationOptions creationOptions)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
	}

	public Task<TResult> StartNew(Func<object, TResult> function, object state)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
	}

	private static void FromAsyncCoreLogic(IAsyncResult iar, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, Task<TResult> promise, bool requiresSynchronization)
	{
		Exception ex = null;
		OperationCanceledException ex2 = null;
		TResult result = default(TResult);
		try
		{
			if (endFunction != null)
			{
				result = endFunction(iar);
			}
			else
			{
				endAction(iar);
			}
		}
		catch (OperationCanceledException ex3)
		{
			ex2 = ex3;
		}
		catch (Exception ex4)
		{
			ex = ex4;
		}
		finally
		{
			if (ex2 != null)
			{
				promise.TrySetCanceled(ex2.CancellationToken, ex2);
			}
			else if (ex != null)
			{
				promise.TrySetException(ex);
			}
			else
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Completed);
				}
				DebuggerSupport.RemoveFromActiveTasks(promise);
				if (requiresSynchronization)
				{
					promise.TrySetResult(result);
				}
				else
				{
					promise.DangerousSetResult(result);
				}
			}
		}
	}

	public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
	{
		return FromAsyncImpl(asyncResult, endMethod, null, m_defaultCreationOptions, DefaultScheduler);
	}

	public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
	{
		return FromAsyncImpl(asyncResult, endMethod, null, creationOptions, DefaultScheduler);
	}

	public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return FromAsyncImpl(asyncResult, endMethod, null, creationOptions, scheduler);
	}

	internal static Task<TResult> FromAsyncImpl(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		if (endFunction == null && endAction == null)
		{
			throw new ArgumentNullException("endMethod");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: false);
		Task<TResult> promise = new Task<TResult>((object)null, creationOptions);
		Task t = new Task((Action<object>)delegate
		{
			FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, requiresSynchronization: true);
		}, null, null, default(CancellationToken), TaskCreationOptions.None, InternalTaskOptions.None, null);
		if (asyncResult.IsCompleted)
		{
			try
			{
				t.InternalRunSynchronously(scheduler, waitForCompletion: false);
			}
			catch (Exception exceptionObject)
			{
				promise.TrySetException(exceptionObject);
			}
		}
		else
		{
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, delegate
			{
				try
				{
					t.InternalRunSynchronously(scheduler, waitForCompletion: false);
				}
				catch (Exception exceptionObject2)
				{
					promise.TrySetException(exceptionObject2);
				}
			}, null, -1, executeOnlyOnce: true);
		}
		return promise;
	}

	public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, state, creationOptions);
	}

	internal static Task<TResult> FromAsyncImpl(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, object state, TaskCreationOptions creationOptions)
	{
		if (beginMethod == null)
		{
			throw new ArgumentNullException("beginMethod");
		}
		if (endFunction == null && endAction == null)
		{
			throw new ArgumentNullException("endMethod");
		}
		TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
		Task<TResult> promise = new Task<TResult>(state, creationOptions);
		if (DebuggerSupport.LoggingOn)
		{
			DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + beginMethod, 0uL);
		}
		DebuggerSupport.AddToActiveTasks(promise);
		try
		{
			IAsyncResult asyncResult = beginMethod(delegate(IAsyncResult iar)
			{
				if (!iar.CompletedSynchronously)
				{
					FromAsyncCoreLogic(iar, endFunction, endAction, promise, requiresSynchronization: true);
				}
			}, state);
			if (asyncResult.CompletedSynchronously)
			{
				FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, requiresSynchronization: false);
			}
		}
		catch
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
			}
			DebuggerSupport.RemoveFromActiveTasks(promise);
			promise.TrySetResult(default(TResult));
			throw;
		}
		return promise;
	}

	public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, arg1, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, arg1, state, creationOptions);
	}

	internal static Task<TResult> FromAsyncImpl<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TArg1 arg1, object state, TaskCreationOptions creationOptions)
	{
		if (beginMethod == null)
		{
			throw new ArgumentNullException("beginMethod");
		}
		if (endFunction == null && endAction == null)
		{
			throw new ArgumentNullException("endFunction");
		}
		TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
		Task<TResult> promise = new Task<TResult>(state, creationOptions);
		if (DebuggerSupport.LoggingOn)
		{
			DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + beginMethod, 0uL);
		}
		DebuggerSupport.AddToActiveTasks(promise);
		try
		{
			IAsyncResult asyncResult = beginMethod(arg1, delegate(IAsyncResult iar)
			{
				if (!iar.CompletedSynchronously)
				{
					FromAsyncCoreLogic(iar, endFunction, endAction, promise, requiresSynchronization: true);
				}
			}, state);
			if (asyncResult.CompletedSynchronously)
			{
				FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, requiresSynchronization: false);
			}
		}
		catch
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
			}
			DebuggerSupport.RemoveFromActiveTasks(promise);
			promise.TrySetResult(default(TResult));
			throw;
		}
		return promise;
	}

	public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, state, creationOptions);
	}

	internal static Task<TResult> FromAsyncImpl<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
	{
		if (beginMethod == null)
		{
			throw new ArgumentNullException("beginMethod");
		}
		if (endFunction == null && endAction == null)
		{
			throw new ArgumentNullException("endMethod");
		}
		TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
		Task<TResult> promise = new Task<TResult>(state, creationOptions);
		if (DebuggerSupport.LoggingOn)
		{
			DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + beginMethod, 0uL);
		}
		DebuggerSupport.AddToActiveTasks(promise);
		try
		{
			IAsyncResult asyncResult = beginMethod(arg1, arg2, delegate(IAsyncResult iar)
			{
				if (!iar.CompletedSynchronously)
				{
					FromAsyncCoreLogic(iar, endFunction, endAction, promise, requiresSynchronization: true);
				}
			}, state);
			if (asyncResult.CompletedSynchronously)
			{
				FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, requiresSynchronization: false);
			}
		}
		catch
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
			}
			DebuggerSupport.RemoveFromActiveTasks(promise);
			promise.TrySetResult(default(TResult));
			throw;
		}
		return promise;
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, arg3, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
	{
		return FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, arg3, state, creationOptions);
	}

	internal static Task<TResult> FromAsyncImpl<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
	{
		if (beginMethod == null)
		{
			throw new ArgumentNullException("beginMethod");
		}
		if (endFunction == null && endAction == null)
		{
			throw new ArgumentNullException("endMethod");
		}
		TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
		Task<TResult> promise = new Task<TResult>(state, creationOptions);
		if (DebuggerSupport.LoggingOn)
		{
			DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + beginMethod, 0uL);
		}
		DebuggerSupport.AddToActiveTasks(promise);
		try
		{
			IAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, delegate(IAsyncResult iar)
			{
				if (!iar.CompletedSynchronously)
				{
					FromAsyncCoreLogic(iar, endFunction, endAction, promise, requiresSynchronization: true);
				}
			}, state);
			if (asyncResult.CompletedSynchronously)
			{
				FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, requiresSynchronization: false);
			}
		}
		catch
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
			}
			DebuggerSupport.RemoveFromActiveTasks(promise);
			promise.TrySetResult(default(TResult));
			throw;
		}
		return promise;
	}

	internal static Task<TResult> FromAsyncTrim<TInstance, TArgs>(TInstance thisRef, TArgs args, Func<TInstance, TArgs, AsyncCallback, object, IAsyncResult> beginMethod, Func<TInstance, IAsyncResult, TResult> endMethod) where TInstance : class
	{
		FromAsyncTrimPromise<TInstance> fromAsyncTrimPromise = new FromAsyncTrimPromise<TInstance>(thisRef, endMethod);
		IAsyncResult asyncResult = beginMethod(thisRef, args, FromAsyncTrimPromise<TInstance>.s_completeFromAsyncResult, fromAsyncTrimPromise);
		if (asyncResult.CompletedSynchronously)
		{
			fromAsyncTrimPromise.Complete(thisRef, endMethod, asyncResult, requiresSynchronization: false);
		}
		return fromAsyncTrimPromise;
	}

	private static Task<TResult> CreateCanceledTask(TaskContinuationOptions continuationOptions, CancellationToken ct)
	{
		Task.CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var _);
		return new Task<TResult>(canceled: true, default(TResult), creationOptions, ct);
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	internal static Task<TResult> ContinueWhenAllImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task<TAntecedentResult>[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return TaskFactory.CommonCWAllLogic(tasksCopy).ContinueWith(GenericDelegateCache<TAntecedentResult, TResult>.CWAllFuncDelegate, continuationFunction, scheduler, cancellationToken, continuationOptions);
	}

	internal static Task<TResult> ContinueWhenAllImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task<TAntecedentResult>[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return TaskFactory.CommonCWAllLogic(tasksCopy).ContinueWith(GenericDelegateCache<TAntecedentResult, TResult>.CWAllActionDelegate, continuationAction, scheduler, cancellationToken, continuationOptions);
	}

	internal static Task<TResult> ContinueWhenAllImpl(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return TaskFactory.CommonCWAllLogic(tasksCopy).ContinueWith(delegate(Task<Task[]> completedTasks, object state)
		{
			completedTasks.NotifyDebuggerOfWaitCompletionIfNecessary();
			return ((Func<Task[], TResult>)state)(completedTasks.Result);
		}, continuationFunction, scheduler, cancellationToken, continuationOptions);
	}

	internal static Task<TResult> ContinueWhenAllImpl(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return TaskFactory.CommonCWAllLogic(tasksCopy).ContinueWith(delegate(Task<Task[]> completedTasks, object state)
		{
			completedTasks.NotifyDebuggerOfWaitCompletionIfNecessary();
			((Action<Task[]>)state)(completedTasks.Result);
			return default(TResult);
		}, continuationAction, scheduler, cancellationToken, continuationOptions);
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	internal static Task<TResult> ContinueWhenAnyImpl(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (tasks.Length == 0)
		{
			throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return task.ContinueWith(delegate(Task<Task> completedTask, object state)
		{
			((Action<Task>)state)(completedTask.Result);
			return default(TResult);
		}, continuationAction, scheduler, cancellationToken, continuationOptions);
	}

	internal static Task<TResult> ContinueWhenAnyImpl(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (tasks.Length == 0)
		{
			throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return task.ContinueWith((Task<Task> completedTask, object state) => ((Func<Task, TResult>)state)(completedTask.Result), continuationFunction, scheduler, cancellationToken, continuationOptions);
	}

	internal static Task<TResult> ContinueWhenAnyImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (tasks.Length == 0)
		{
			throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return task.ContinueWith(GenericDelegateCache<TAntecedentResult, TResult>.CWAnyFuncDelegate, continuationFunction, scheduler, cancellationToken, continuationOptions);
	}

	internal static Task<TResult> ContinueWhenAnyImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
	{
		TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (tasks.Length == 0)
		{
			throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
		}
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
		if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == 0)
		{
			return CreateCanceledTask(continuationOptions, cancellationToken);
		}
		return task.ContinueWith(GenericDelegateCache<TAntecedentResult, TResult>.CWAnyActionDelegate, continuationAction, scheduler, cancellationToken, continuationOptions);
	}
}
public class TaskFactory
{
	private sealed class CompleteOnCountdownPromise : Task<Task[]>, ITaskCompletionAction
	{
		private readonly Task[] _tasks;

		private int _count;

		public bool InvokeMayRunArbitraryCode => true;

		internal override bool ShouldNotifyDebuggerOfWaitCompletion
		{
			get
			{
				if (base.ShouldNotifyDebuggerOfWaitCompletion)
				{
					return Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion(_tasks);
				}
				return false;
			}
		}

		internal CompleteOnCountdownPromise(Task[] tasksCopy)
		{
			_tasks = tasksCopy;
			_count = tasksCopy.Length;
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, this, "TaskFactory.ContinueWhenAll", 0uL);
			}
			DebuggerSupport.AddToActiveTasks(this);
		}

		public void Invoke(Task completingTask)
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationRelation(CausalityTraceLevel.Important, this, CausalityRelation.Join);
			}
			if (completingTask.IsWaitNotificationEnabled)
			{
				SetNotificationForWaitCompletion(enabled: true);
			}
			if (Interlocked.Decrement(ref _count) == 0)
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, this, AsyncStatus.Completed);
				}
				DebuggerSupport.RemoveFromActiveTasks(this);
				TrySetResult(_tasks);
			}
		}
	}

	private sealed class CompleteOnCountdownPromise<T> : Task<Task<T>[]>, ITaskCompletionAction
	{
		private readonly Task<T>[] _tasks;

		private int _count;

		public bool InvokeMayRunArbitraryCode => true;

		internal override bool ShouldNotifyDebuggerOfWaitCompletion
		{
			get
			{
				if (base.ShouldNotifyDebuggerOfWaitCompletion)
				{
					Task[] tasks = _tasks;
					return Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion(tasks);
				}
				return false;
			}
		}

		internal CompleteOnCountdownPromise(Task<T>[] tasksCopy)
		{
			_tasks = tasksCopy;
			_count = tasksCopy.Length;
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, this, "TaskFactory.ContinueWhenAll<>", 0uL);
			}
			DebuggerSupport.AddToActiveTasks(this);
		}

		public void Invoke(Task completingTask)
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationRelation(CausalityTraceLevel.Important, this, CausalityRelation.Join);
			}
			if (completingTask.IsWaitNotificationEnabled)
			{
				SetNotificationForWaitCompletion(enabled: true);
			}
			if (Interlocked.Decrement(ref _count) == 0)
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, this, AsyncStatus.Completed);
				}
				DebuggerSupport.RemoveFromActiveTasks(this);
				TrySetResult(_tasks);
			}
		}
	}

	internal sealed class CompleteOnInvokePromise : Task<Task>, ITaskCompletionAction
	{
		private IList<Task> _tasks;

		public bool InvokeMayRunArbitraryCode => true;

		public CompleteOnInvokePromise(IList<Task> tasks)
		{
			_tasks = tasks;
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, this, "TaskFactory.ContinueWhenAny", 0uL);
			}
			DebuggerSupport.AddToActiveTasks(this);
		}

		public void Invoke(Task completingTask)
		{
			if (!TrySetResult(completingTask))
			{
				return;
			}
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationRelation(CausalityTraceLevel.Important, this, CausalityRelation.Choice);
				DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, this, AsyncStatus.Completed);
			}
			DebuggerSupport.RemoveFromActiveTasks(this);
			IList<Task> tasks = _tasks;
			int count = tasks.Count;
			for (int i = 0; i < count; i++)
			{
				Task task = tasks[i];
				if (task != null && !task.IsCompleted)
				{
					task.RemoveContinuation(this);
				}
			}
			_tasks = null;
		}
	}

	private readonly CancellationToken m_defaultCancellationToken;

	private readonly TaskScheduler m_defaultScheduler;

	private readonly TaskCreationOptions m_defaultCreationOptions;

	private readonly TaskContinuationOptions m_defaultContinuationOptions;

	private TaskScheduler DefaultScheduler
	{
		get
		{
			if (m_defaultScheduler == null)
			{
				return TaskScheduler.Current;
			}
			return m_defaultScheduler;
		}
	}

	public CancellationToken CancellationToken => m_defaultCancellationToken;

	public TaskScheduler Scheduler => m_defaultScheduler;

	public TaskCreationOptions CreationOptions => m_defaultCreationOptions;

	public TaskContinuationOptions ContinuationOptions => m_defaultContinuationOptions;

	private TaskScheduler GetDefaultScheduler(Task currTask)
	{
		if (m_defaultScheduler != null)
		{
			return m_defaultScheduler;
		}
		if (currTask != null && (currTask.CreationOptions & TaskCreationOptions.HideScheduler) == 0)
		{
			return currTask.ExecutingTaskScheduler;
		}
		return TaskScheduler.Default;
	}

	public TaskFactory()
		: this(default(CancellationToken), TaskCreationOptions.None, TaskContinuationOptions.None, null)
	{
	}

	public TaskFactory(CancellationToken cancellationToken)
		: this(cancellationToken, TaskCreationOptions.None, TaskContinuationOptions.None, null)
	{
	}

	public TaskFactory(TaskScheduler scheduler)
		: this(default(CancellationToken), TaskCreationOptions.None, TaskContinuationOptions.None, scheduler)
	{
	}

	public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
		: this(default(CancellationToken), creationOptions, continuationOptions, null)
	{
	}

	public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		CheckMultiTaskContinuationOptions(continuationOptions);
		CheckCreationOptions(creationOptions);
		m_defaultCancellationToken = cancellationToken;
		m_defaultScheduler = scheduler;
		m_defaultCreationOptions = creationOptions;
		m_defaultContinuationOptions = continuationOptions;
	}

	internal static void CheckCreationOptions(TaskCreationOptions creationOptions)
	{
		if ((creationOptions & ~(TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler | TaskCreationOptions.RunContinuationsAsynchronously)) != TaskCreationOptions.None)
		{
			throw new ArgumentOutOfRangeException("creationOptions");
		}
	}

	public Task StartNew(Action action)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task.InternalStartNew(internalCurrent, action, null, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None);
	}

	public Task StartNew(Action action, CancellationToken cancellationToken)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task.InternalStartNew(internalCurrent, action, null, cancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None);
	}

	public Task StartNew(Action action, TaskCreationOptions creationOptions)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task.InternalStartNew(internalCurrent, action, null, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), creationOptions, InternalTaskOptions.None);
	}

	public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return Task.InternalStartNew(Task.InternalCurrentIfAttached(creationOptions), action, null, cancellationToken, scheduler, creationOptions, InternalTaskOptions.None);
	}

	internal Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
	{
		return Task.InternalStartNew(Task.InternalCurrentIfAttached(creationOptions), action, null, cancellationToken, scheduler, creationOptions, internalOptions);
	}

	public Task StartNew(Action<object> action, object state)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task.InternalStartNew(internalCurrent, action, state, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None);
	}

	public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task.InternalStartNew(internalCurrent, action, state, cancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None);
	}

	public Task StartNew(Action<object> action, object state, TaskCreationOptions creationOptions)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task.InternalStartNew(internalCurrent, action, state, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), creationOptions, InternalTaskOptions.None);
	}

	public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return Task.InternalStartNew(Task.InternalCurrentIfAttached(creationOptions), action, state, cancellationToken, scheduler, creationOptions, InternalTaskOptions.None);
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function, TaskCreationOptions creationOptions)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
	}

	public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
	{
		Task internalCurrent = Task.InternalCurrent;
		return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent));
	}

	public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
	}

	public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod)
	{
		return FromAsync(asyncResult, endMethod, m_defaultCreationOptions, DefaultScheduler);
	}

	public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions)
	{
		return FromAsync(asyncResult, endMethod, creationOptions, DefaultScheduler);
	}

	public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return TaskFactory<VoidTaskResult>.FromAsyncImpl(asyncResult, null, endMethod, creationOptions, scheduler);
	}

	public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
	{
		return FromAsync(beginMethod, endMethod, state, m_defaultCreationOptions);
	}

	public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<VoidTaskResult>.FromAsyncImpl(beginMethod, null, endMethod, state, creationOptions);
	}

	public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
	{
		return FromAsync(beginMethod, endMethod, arg1, state, m_defaultCreationOptions);
	}

	public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<VoidTaskResult>.FromAsyncImpl(beginMethod, null, endMethod, arg1, state, creationOptions);
	}

	public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
	{
		return FromAsync(beginMethod, endMethod, arg1, arg2, state, m_defaultCreationOptions);
	}

	public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<VoidTaskResult>.FromAsyncImpl(beginMethod, null, endMethod, arg1, arg2, state, creationOptions);
	}

	public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
	{
		return FromAsync(beginMethod, endMethod, arg1, arg2, arg3, state, m_defaultCreationOptions);
	}

	public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<VoidTaskResult>.FromAsyncImpl(beginMethod, null, endMethod, arg1, arg2, arg3, state, creationOptions);
	}

	public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
	{
		return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, null, m_defaultCreationOptions, DefaultScheduler);
	}

	public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
	{
		return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, null, creationOptions, DefaultScheduler);
	}

	public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
	{
		return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, null, creationOptions, scheduler);
	}

	public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, state, creationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, arg1, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, arg1, state, creationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, state, creationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, arg3, state, m_defaultCreationOptions);
	}

	public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
	{
		return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, arg1, arg2, arg3, state, creationOptions);
	}

	internal static void CheckFromAsyncOptions(TaskCreationOptions creationOptions, bool hasBeginMethod)
	{
		if (hasBeginMethod)
		{
			if ((creationOptions & TaskCreationOptions.LongRunning) != TaskCreationOptions.None)
			{
				throw new ArgumentOutOfRangeException("creationOptions", "It is invalid to specify TaskCreationOptions.LongRunning in calls to FromAsync.");
			}
			if ((creationOptions & TaskCreationOptions.PreferFairness) != TaskCreationOptions.None)
			{
				throw new ArgumentOutOfRangeException("creationOptions", "It is invalid to specify TaskCreationOptions.PreferFairness in calls to FromAsync.");
			}
		}
		if ((creationOptions & ~(TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler)) != TaskCreationOptions.None)
		{
			throw new ArgumentOutOfRangeException("creationOptions");
		}
	}

	internal static Task<Task[]> CommonCWAllLogic(Task[] tasksCopy)
	{
		CompleteOnCountdownPromise completeOnCountdownPromise = new CompleteOnCountdownPromise(tasksCopy);
		for (int i = 0; i < tasksCopy.Length; i++)
		{
			if (tasksCopy[i].IsCompleted)
			{
				completeOnCountdownPromise.Invoke(tasksCopy[i]);
			}
			else
			{
				tasksCopy[i].AddCompletionAction(completeOnCountdownPromise);
			}
		}
		return completeOnCountdownPromise;
	}

	internal static Task<Task<T>[]> CommonCWAllLogic<T>(Task<T>[] tasksCopy)
	{
		CompleteOnCountdownPromise<T> completeOnCountdownPromise = new CompleteOnCountdownPromise<T>(tasksCopy);
		for (int i = 0; i < tasksCopy.Length; i++)
		{
			if (tasksCopy[i].IsCompleted)
			{
				completeOnCountdownPromise.Invoke(tasksCopy[i]);
			}
			else
			{
				tasksCopy[i].AddCompletionAction(completeOnCountdownPromise);
			}
		}
		return completeOnCountdownPromise;
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, continuationOptions, cancellationToken, scheduler);
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, continuationAction, continuationOptions, cancellationToken, scheduler);
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	internal static Task<Task> CommonCWAnyLogic(IList<Task> tasks)
	{
		CompleteOnInvokePromise completeOnInvokePromise = new CompleteOnInvokePromise(tasks);
		bool flag = false;
		int count = tasks.Count;
		for (int i = 0; i < count; i++)
		{
			Task task = tasks[i];
			if (task == null)
			{
				throw new ArgumentException("The tasks argument included a null value.", "tasks");
			}
			if (flag)
			{
				continue;
			}
			if (completeOnInvokePromise.IsCompleted)
			{
				flag = true;
				continue;
			}
			if (task.IsCompleted)
			{
				completeOnInvokePromise.Invoke(task);
				flag = true;
				continue;
			}
			task.AddCompletionAction(completeOnInvokePromise);
			if (completeOnInvokePromise.IsCompleted)
			{
				task.RemoveContinuation(completeOnInvokePromise);
			}
		}
		return completeOnInvokePromise;
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, continuationOptions, cancellationToken, scheduler);
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationFunction == null)
		{
			throw new ArgumentNullException("continuationFunction");
		}
		return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler);
	}

	public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		if (continuationAction == null)
		{
			throw new ArgumentNullException("continuationAction");
		}
		return TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, continuationAction, continuationOptions, cancellationToken, scheduler);
	}

	internal static Task[] CheckMultiContinuationTasksAndCopy(Task[] tasks)
	{
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (tasks.Length == 0)
		{
			throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
		}
		Task[] array = new Task[tasks.Length];
		for (int i = 0; i < tasks.Length; i++)
		{
			array[i] = tasks[i];
			if (array[i] == null)
			{
				throw new ArgumentException("The tasks argument included a null value.", "tasks");
			}
		}
		return array;
	}

	internal static Task<TResult>[] CheckMultiContinuationTasksAndCopy<TResult>(Task<TResult>[] tasks)
	{
		if (tasks == null)
		{
			throw new ArgumentNullException("tasks");
		}
		if (tasks.Length == 0)
		{
			throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
		}
		Task<TResult>[] array = new Task<TResult>[tasks.Length];
		for (int i = 0; i < tasks.Length; i++)
		{
			array[i] = tasks[i];
			if (array[i] == null)
			{
				throw new ArgumentException("The tasks argument included a null value.", "tasks");
			}
		}
		return array;
	}

	internal static void CheckMultiTaskContinuationOptions(TaskContinuationOptions continuationOptions)
	{
		if ((continuationOptions & (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously)) == (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously))
		{
			throw new ArgumentOutOfRangeException("continuationOptions", "The specified TaskContinuationOptions combined LongRunning and ExecuteSynchronously.  Synchronous continuations should not be long running.");
		}
		if ((continuationOptions & ~(TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.HideScheduler | TaskContinuationOptions.LazyCancellation | TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously)) != TaskContinuationOptions.None)
		{
			throw new ArgumentOutOfRangeException("continuationOptions");
		}
		if ((continuationOptions & (TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion)) != TaskContinuationOptions.None)
		{
			throw new ArgumentOutOfRangeException("continuationOptions", "It is invalid to exclude specific continuation kinds for continuations off of multiple tasks.");
		}
	}
}
