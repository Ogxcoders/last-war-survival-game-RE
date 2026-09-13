using System.Runtime.Serialization;

namespace System.Threading.Tasks;

[Serializable]
public class TaskCanceledException : OperationCanceledException
{
	[NonSerialized]
	private readonly Task _canceledTask;

	public Task Task => _canceledTask;

	public TaskCanceledException()
		: base("A task was canceled.")
	{
	}

	public TaskCanceledException(string message)
		: base(message)
	{
	}

	public TaskCanceledException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public TaskCanceledException(string message, Exception innerException, CancellationToken token)
		: base(message, innerException, token)
	{
	}

	public TaskCanceledException(Task task)
		: base("A task was canceled.", task?.CancellationToken ?? default(CancellationToken))
	{
		_canceledTask = task;
	}

	protected TaskCanceledException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
