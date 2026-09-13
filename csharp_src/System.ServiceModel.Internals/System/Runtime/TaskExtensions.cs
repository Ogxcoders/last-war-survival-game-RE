using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Runtime;

internal static class TaskExtensions
{
	public static IAsyncResult AsAsyncResult<T>(this Task<T> task, AsyncCallback callback, object state)
	{
		if (task == null)
		{
			throw Fx.Exception.ArgumentNull("task");
		}
		if (task.Status == TaskStatus.Created)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("SFx Task Not Started"));
		}
		TaskCompletionSource<T> tcs = new TaskCompletionSource<T>(state);
		task.ContinueWith(delegate(Task<T> t)
		{
			if (t.IsFaulted)
			{
				tcs.TrySetException(t.Exception.InnerExceptions);
			}
			else if (t.IsCanceled)
			{
				tcs.TrySetCanceled();
			}
			else
			{
				tcs.TrySetResult(t.Result);
			}
			if (callback != null)
			{
				callback(tcs.Task);
			}
		}, TaskContinuationOptions.ExecuteSynchronously);
		return tcs.Task;
	}

	public static IAsyncResult AsAsyncResult(this Task task, AsyncCallback callback, object state)
	{
		if (task == null)
		{
			throw Fx.Exception.ArgumentNull("task");
		}
		if (task.Status == TaskStatus.Created)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("SFx Task Not Started"));
		}
		TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state);
		task.ContinueWith(delegate(Task t)
		{
			if (t.IsFaulted)
			{
				tcs.TrySetException(t.Exception.InnerExceptions);
			}
			else if (t.IsCanceled)
			{
				tcs.TrySetCanceled();
			}
			else
			{
				tcs.TrySetResult(null);
			}
			if (callback != null)
			{
				callback(tcs.Task);
			}
		}, TaskContinuationOptions.ExecuteSynchronously);
		return tcs.Task;
	}

	public static ConfiguredTaskAwaitable SuppressContextFlow(this Task task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: false);
	}

	public static ConfiguredTaskAwaitable<T> SuppressContextFlow<T>(this Task<T> task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: false);
	}

	public static ConfiguredTaskAwaitable ContinueOnCapturedContextFlow(this Task task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: true);
	}

	public static ConfiguredTaskAwaitable<T> ContinueOnCapturedContextFlow<T>(this Task<T> task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: true);
	}

	public static void Wait<TException>(this Task task)
	{
		try
		{
			task.Wait();
		}
		catch (AggregateException aggregateException)
		{
			throw Fx.Exception.AsError<TException>(aggregateException);
		}
	}

	public static bool Wait<TException>(this Task task, int millisecondsTimeout)
	{
		try
		{
			return task.Wait(millisecondsTimeout);
		}
		catch (AggregateException aggregateException)
		{
			throw Fx.Exception.AsError<TException>(aggregateException);
		}
	}

	public static bool Wait<TException>(this Task task, TimeSpan timeout)
	{
		try
		{
			if (timeout == TimeSpan.MaxValue)
			{
				return task.Wait(-1);
			}
			return task.Wait(timeout);
		}
		catch (AggregateException aggregateException)
		{
			throw Fx.Exception.AsError<TException>(aggregateException);
		}
	}

	public static void Wait(this Task task, TimeSpan timeout, Action<Exception, TimeSpan, string> exceptionConverter, string operationType)
	{
		bool flag = false;
		try
		{
			if (timeout > TimeoutHelper.MaxWait)
			{
				task.Wait();
			}
			else
			{
				flag = !task.Wait(timeout);
			}
		}
		catch (Exception ex)
		{
			if (Fx.IsFatal(ex) || exceptionConverter == null)
			{
				throw;
			}
			exceptionConverter(ex, timeout, operationType);
		}
		if (flag)
		{
			throw Fx.Exception.AsError(new TimeoutException(InternalSR.TaskTimedOutError(timeout)));
		}
	}

	public static Task<TBase> Upcast<TDerived, TBase>(this Task<TDerived> task) where TDerived : TBase
	{
		if (task.Status != TaskStatus.RanToCompletion)
		{
			return task.UpcastPrivate<TDerived, TBase>();
		}
		return Task.FromResult((TBase)(object)task.Result);
	}

	private static async Task<TBase> UpcastPrivate<TDerived, TBase>(this Task<TDerived> task) where TDerived : TBase
	{
		return (TBase)(object)(await task.ConfigureAwait(continueOnCapturedContext: false));
	}
}
