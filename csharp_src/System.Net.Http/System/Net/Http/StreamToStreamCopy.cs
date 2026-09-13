using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static class StreamToStreamCopy
{
	public static Task CopyAsync(Stream source, Stream destination, int bufferSize, bool disposeSource, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			Task task = ((bufferSize == 0) ? source.CopyToAsync(destination, cancellationToken) : source.CopyToAsync(destination, bufferSize, cancellationToken));
			return disposeSource ? DisposeSourceWhenCompleteAsync(task, source) : task;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}

	private static Task DisposeSourceWhenCompleteAsync(Task task, Stream source)
	{
		switch (task.Status)
		{
		case TaskStatus.RanToCompletion:
			DisposeSource(source);
			return Task.CompletedTask;
		case TaskStatus.Canceled:
		case TaskStatus.Faulted:
			return task;
		default:
			return task.ContinueWith(delegate(Task completed, object innerSource)
			{
				completed.GetAwaiter().GetResult();
				DisposeSource((Stream)innerSource);
			}, source, CancellationToken.None, TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}
	}

	private static void DisposeSource(Stream source)
	{
		try
		{
			source.Dispose();
		}
		catch (Exception ex)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(null, ex);
			}
		}
	}
}
