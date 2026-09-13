using System.Threading.Tasks;

namespace System.Linq.Parallel;

internal class OrderPreservingSpoolingTask<TInputOutput, TKey> : SpoolingTaskBase
{
	private Shared<TInputOutput[]> _results;

	private SortHelper<TInputOutput> _sortHelper;

	private OrderPreservingSpoolingTask(int taskIndex, QueryTaskGroupState groupState, Shared<TInputOutput[]> results, SortHelper<TInputOutput> sortHelper)
		: base(taskIndex, groupState)
	{
		_results = results;
		_sortHelper = sortHelper;
	}

	internal static void Spool(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TKey> partitions, Shared<TInputOutput[]> results, TaskScheduler taskScheduler)
	{
		int maxToRunInParallel = partitions.PartitionCount - 1;
		SortHelper<TInputOutput, TKey>[] sortHelpers = SortHelper<TInputOutput, TKey>.GenerateSortHelpers(partitions, groupState);
		Task task = new Task(delegate
		{
			for (int i = 0; i < maxToRunInParallel; i++)
			{
				new OrderPreservingSpoolingTask<TInputOutput, TKey>(i, groupState, results, sortHelpers[i]).RunAsynchronously(taskScheduler);
			}
			new OrderPreservingSpoolingTask<TInputOutput, TKey>(maxToRunInParallel, groupState, results, sortHelpers[maxToRunInParallel]).RunSynchronously(taskScheduler);
		});
		groupState.QueryBegin(task);
		task.RunSynchronously(taskScheduler);
		for (int num = 0; num < sortHelpers.Length; num++)
		{
			sortHelpers[num].Dispose();
		}
		groupState.QueryEnd(userInitiatedDispose: false);
	}

	protected override void SpoolingWork()
	{
		TInputOutput[] value = _sortHelper.Sort();
		if (!_groupState.CancellationState.MergedCancellationToken.IsCancellationRequested && _taskIndex == 0)
		{
			_results.Value = value;
		}
	}
}
