using System.Threading.Tasks;

namespace System.Xml;

internal static class AsyncHelper
{
	public static readonly Task DoneTask = Task.FromResult(result: true);

	public static readonly Task<bool> DoneTaskTrue = Task.FromResult(result: true);

	public static readonly Task<bool> DoneTaskFalse = Task.FromResult(result: false);

	public static readonly Task<int> DoneTaskZero = Task.FromResult(0);

	public static bool IsSuccess(this Task task)
	{
		if (task.IsCompleted)
		{
			return task.Exception == null;
		}
		return false;
	}

	public static Task CallVoidFuncWhenFinish(this Task task, Action func)
	{
		if (task.IsSuccess())
		{
			func();
			return DoneTask;
		}
		return task._CallVoidFuncWhenFinish(func);
	}

	private static async Task _CallVoidFuncWhenFinish(this Task task, Action func)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		func();
	}

	public static Task<bool> ReturnTaskBoolWhenFinish(this Task task, bool ret)
	{
		if (task.IsSuccess())
		{
			if (ret)
			{
				return DoneTaskTrue;
			}
			return DoneTaskFalse;
		}
		return task._ReturnTaskBoolWhenFinish(ret);
	}

	public static async Task<bool> _ReturnTaskBoolWhenFinish(this Task task, bool ret)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		return ret;
	}

	public static Task CallTaskFuncWhenFinish(this Task task, Func<Task> func)
	{
		if (task.IsSuccess())
		{
			return func();
		}
		return _CallTaskFuncWhenFinish(task, func);
	}

	private static async Task _CallTaskFuncWhenFinish(Task task, Func<Task> func)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		await func().ConfigureAwait(continueOnCapturedContext: false);
	}

	public static Task<bool> CallBoolTaskFuncWhenFinish(this Task task, Func<Task<bool>> func)
	{
		if (task.IsSuccess())
		{
			return func();
		}
		return task._CallBoolTaskFuncWhenFinish(func);
	}

	private static async Task<bool> _CallBoolTaskFuncWhenFinish(this Task task, Func<Task<bool>> func)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		return await func().ConfigureAwait(continueOnCapturedContext: false);
	}

	public static Task<bool> ContinueBoolTaskFuncWhenFalse(this Task<bool> task, Func<Task<bool>> func)
	{
		if (task.IsSuccess())
		{
			if (task.Result)
			{
				return DoneTaskTrue;
			}
			return func();
		}
		return _ContinueBoolTaskFuncWhenFalse(task, func);
	}

	private static async Task<bool> _ContinueBoolTaskFuncWhenFalse(Task<bool> task, Func<Task<bool>> func)
	{
		if (await task.ConfigureAwait(continueOnCapturedContext: false))
		{
			return true;
		}
		return await func().ConfigureAwait(continueOnCapturedContext: false);
	}
}
