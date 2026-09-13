namespace System.Threading.Tasks;

internal class SystemThreadingTasks_TaskDebugView
{
	private Task m_task;

	public object AsyncState => m_task.AsyncState;

	public TaskCreationOptions CreationOptions => m_task.CreationOptions;

	public Exception Exception => m_task.Exception;

	public int Id => m_task.Id;

	public bool CancellationPending
	{
		get
		{
			if (m_task.Status == TaskStatus.WaitingToRun)
			{
				return m_task.CancellationToken.IsCancellationRequested;
			}
			return false;
		}
	}

	public TaskStatus Status => m_task.Status;

	public SystemThreadingTasks_TaskDebugView(Task task)
	{
		m_task = task;
	}
}
