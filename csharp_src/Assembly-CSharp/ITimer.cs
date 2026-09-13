public interface ITimer
{
	bool isCompleted { get; }

	bool isCancelled { get; }

	bool isDone { get; }

	bool isPause { get; set; }
}
