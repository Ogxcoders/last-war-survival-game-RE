using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Internal.Runtime.Augments;

namespace System.Threading.Tasks;

internal static class DebuggerSupport
{
	private static readonly LowLevelDictionary<int, Task> s_activeTasks = new LowLevelDictionary<int, Task>();

	private static readonly object s_activeTasksLock = new object();

	public static bool LoggingOn => false;

	public static void TraceOperationCreation(CausalityTraceLevel traceLevel, Task task, string operationName, ulong relatedContext)
	{
	}

	public static void TraceOperationCompletion(CausalityTraceLevel traceLevel, Task task, AsyncStatus status)
	{
	}

	public static void TraceOperationRelation(CausalityTraceLevel traceLevel, Task task, CausalityRelation relation)
	{
	}

	public static void TraceSynchronousWorkStart(CausalityTraceLevel traceLevel, Task task, CausalitySynchronousWork work)
	{
	}

	public static void TraceSynchronousWorkCompletion(CausalityTraceLevel traceLevel, CausalitySynchronousWork work)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AddToActiveTasks(Task task)
	{
		if (Task.s_asyncDebuggingEnabled)
		{
			AddToActiveTasksNonInlined(task);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddToActiveTasksNonInlined(Task task)
	{
		int id = task.Id;
		lock (s_activeTasksLock)
		{
			s_activeTasks[id] = task;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RemoveFromActiveTasks(Task task)
	{
		if (Task.s_asyncDebuggingEnabled)
		{
			RemoveFromActiveTasksNonInlined(task);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RemoveFromActiveTasksNonInlined(Task task)
	{
		int id = task.Id;
		lock (s_activeTasksLock)
		{
			s_activeTasks.Remove(id);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task GetActiveTaskFromId(int taskId)
	{
		Task value = null;
		s_activeTasks.TryGetValue(taskId, out value);
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task GetTaskIfDebuggingEnabled(this AsyncVoidMethodBuilder builder)
	{
		if (LoggingOn || Task.s_asyncDebuggingEnabled)
		{
			return builder.Task;
		}
		return null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task GetTaskIfDebuggingEnabled(this AsyncTaskMethodBuilder builder)
	{
		if (LoggingOn || Task.s_asyncDebuggingEnabled)
		{
			return builder.Task;
		}
		return null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task GetTaskIfDebuggingEnabled<TResult>(this AsyncTaskMethodBuilder<TResult> builder)
	{
		if (LoggingOn || Task.s_asyncDebuggingEnabled)
		{
			return builder.Task;
		}
		return null;
	}
}
