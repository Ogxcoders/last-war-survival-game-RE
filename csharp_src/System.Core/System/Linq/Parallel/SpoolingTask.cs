using System.Threading.Tasks;

namespace System.Linq.Parallel;

internal static class SpoolingTask
{
	internal static void SpoolStopAndGo<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, SynchronousChannel<TInputOutput>[] channels, TaskScheduler taskScheduler)
	{
		Task task = new Task(delegate
		{
			int num = partitions.PartitionCount - 1;
			for (int i = 0; i < num; i++)
			{
				new StopAndGoSpoolingTask<TInputOutput, TIgnoreKey>(i, groupState, partitions[i], channels[i]).RunAsynchronously(taskScheduler);
			}
			new StopAndGoSpoolingTask<TInputOutput, TIgnoreKey>(num, groupState, partitions[num], channels[num]).RunSynchronously(taskScheduler);
		});
		groupState.QueryBegin(task);
		task.RunSynchronously(taskScheduler);
		groupState.QueryEnd(userInitiatedDispose: false);
	}

	internal static void SpoolPipeline<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, AsynchronousChannel<TInputOutput>[] channels, TaskScheduler taskScheduler)
	{
		Task task = new Task(delegate
		{
			for (int i = 0; i < partitions.PartitionCount; i++)
			{
				new PipelineSpoolingTask<TInputOutput, TIgnoreKey>(i, groupState, partitions[i], channels[i]).RunAsynchronously(taskScheduler);
			}
		});
		groupState.QueryBegin(task);
		task.Start(taskScheduler);
	}

	internal static void SpoolForAll<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, TaskScheduler taskScheduler)
	{
		Task task = new Task(delegate
		{
			int num = partitions.PartitionCount - 1;
			for (int i = 0; i < num; i++)
			{
				new ForAllSpoolingTask<TInputOutput, TIgnoreKey>(i, groupState, partitions[i]).RunAsynchronously(taskScheduler);
			}
			new ForAllSpoolingTask<TInputOutput, TIgnoreKey>(num, groupState, partitions[num]).RunSynchronously(taskScheduler);
		});
		groupState.QueryBegin(task);
		task.RunSynchronously(taskScheduler);
		groupState.QueryEnd(userInitiatedDispose: false);
	}
}
