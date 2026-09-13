using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks;

[DebuggerDisplay("Id={Id}")]
[DebuggerTypeProxy(typeof(SystemThreadingTasks_TaskSchedulerDebugView))]
public abstract class TaskScheduler
{
	internal sealed class SystemThreadingTasks_TaskSchedulerDebugView
	{
		private readonly TaskScheduler m_taskScheduler;

		public int Id => m_taskScheduler.Id;

		public IEnumerable<Task> ScheduledTasks => m_taskScheduler.GetScheduledTasks();

		public SystemThreadingTasks_TaskSchedulerDebugView(TaskScheduler scheduler)
		{
			m_taskScheduler = scheduler;
		}
	}

	private static ConditionalWeakTable<TaskScheduler, object> s_activeTaskSchedulers;

	private static readonly TaskScheduler s_defaultTaskScheduler = new ThreadPoolTaskScheduler();

	internal static int s_taskSchedulerIdCounter;

	private volatile int m_taskSchedulerId;

	private static EventHandler<UnobservedTaskExceptionEventArgs> _unobservedTaskException;

	private static readonly Lock _unobservedTaskExceptionLockObject = new Lock();

	public virtual int MaximumConcurrencyLevel => int.MaxValue;

	internal virtual bool RequiresAtomicStartTransition => true;

	public static TaskScheduler Default => s_defaultTaskScheduler;

	public static TaskScheduler Current => InternalCurrent ?? Default;

	internal static TaskScheduler InternalCurrent
	{
		get
		{
			Task internalCurrent = Task.InternalCurrent;
			if (internalCurrent == null || (internalCurrent.CreationOptions & TaskCreationOptions.HideScheduler) != TaskCreationOptions.None)
			{
				return null;
			}
			return internalCurrent.ExecutingTaskScheduler;
		}
	}

	public int Id
	{
		get
		{
			if (m_taskSchedulerId == 0)
			{
				int num = 0;
				do
				{
					num = Interlocked.Increment(ref s_taskSchedulerIdCounter);
				}
				while (num == 0);
				Interlocked.CompareExchange(ref m_taskSchedulerId, num, 0);
			}
			return m_taskSchedulerId;
		}
	}

	public static event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException
	{
		add
		{
			if (value != null)
			{
				using (LockHolder.Hold(_unobservedTaskExceptionLockObject))
				{
					_unobservedTaskException = (EventHandler<UnobservedTaskExceptionEventArgs>)Delegate.Combine(_unobservedTaskException, value);
				}
			}
		}
		remove
		{
			using (LockHolder.Hold(_unobservedTaskExceptionLockObject))
			{
				_unobservedTaskException = (EventHandler<UnobservedTaskExceptionEventArgs>)Delegate.Remove(_unobservedTaskException, value);
			}
		}
	}

	protected internal abstract void QueueTask(Task task);

	protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);

	protected abstract IEnumerable<Task> GetScheduledTasks();

	internal bool TryRunInline(Task task, bool taskWasPreviouslyQueued)
	{
		TaskScheduler executingTaskScheduler = task.ExecutingTaskScheduler;
		if (executingTaskScheduler != this && executingTaskScheduler != null)
		{
			return executingTaskScheduler.TryRunInline(task, taskWasPreviouslyQueued);
		}
		StackGuard currentStackGuard;
		if (executingTaskScheduler == null || (object)task.m_action == null || task.IsDelegateInvoked || task.IsCanceled || !(currentStackGuard = Task.CurrentStackGuard).TryBeginInliningScope())
		{
			return false;
		}
		bool flag = false;
		try
		{
			flag = TryExecuteTaskInline(task, taskWasPreviouslyQueued);
		}
		finally
		{
			currentStackGuard.EndInliningScope();
		}
		if (flag && !task.IsDelegateInvoked && !task.IsCanceled)
		{
			throw new InvalidOperationException("The TryExecuteTaskInline call to the underlying scheduler succeeded, but the task body was not invoked.");
		}
		return flag;
	}

	protected internal virtual bool TryDequeue(Task task)
	{
		return false;
	}

	internal virtual void NotifyWorkItemProgress()
	{
	}

	private void AddToActiveTaskSchedulers()
	{
		ConditionalWeakTable<TaskScheduler, object> conditionalWeakTable = s_activeTaskSchedulers;
		if (conditionalWeakTable == null)
		{
			Interlocked.CompareExchange(ref s_activeTaskSchedulers, new ConditionalWeakTable<TaskScheduler, object>(), null);
			conditionalWeakTable = s_activeTaskSchedulers;
		}
		conditionalWeakTable.Add(this, null);
	}

	public static TaskScheduler FromCurrentSynchronizationContext()
	{
		return new SynchronizationContextTaskScheduler();
	}

	protected bool TryExecuteTask(Task task)
	{
		if (task.ExecutingTaskScheduler != this)
		{
			throw new InvalidOperationException("ExecuteTask may not be called for a task which was previously queued to a different TaskScheduler.");
		}
		return task.ExecuteEntry(bPreventDoubleExecution: true);
	}

	internal static void PublishUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs ueea)
	{
		using (LockHolder.Hold(_unobservedTaskExceptionLockObject))
		{
			_unobservedTaskException?.Invoke(sender, ueea);
		}
	}

	internal Task[] GetScheduledTasksForDebugger()
	{
		IEnumerable<Task> scheduledTasks = GetScheduledTasks();
		if (scheduledTasks == null)
		{
			return null;
		}
		Task[] array = scheduledTasks as Task[];
		if (array == null)
		{
			array = new LowLevelList<Task>(scheduledTasks).ToArray();
		}
		Task[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			_ = array2[i].Id;
		}
		return array;
	}

	internal static TaskScheduler[] GetTaskSchedulersForDebugger()
	{
		if (s_activeTaskSchedulers == null)
		{
			return new TaskScheduler[1] { s_defaultTaskScheduler };
		}
		LowLevelList<TaskScheduler> lowLevelList = new LowLevelList<TaskScheduler>();
		foreach (KeyValuePair<TaskScheduler, object> item in (IEnumerable<KeyValuePair<TaskScheduler, object>>)s_activeTaskSchedulers)
		{
			lowLevelList.Add(item.Key);
		}
		if (!lowLevelList.Contains(s_defaultTaskScheduler))
		{
			lowLevelList.Add(s_defaultTaskScheduler);
		}
		TaskScheduler[] array = lowLevelList.ToArray();
		TaskScheduler[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			_ = array2[i].Id;
		}
		return array;
	}
}
