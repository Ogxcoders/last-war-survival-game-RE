using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq.Parallel;

internal class OrderPreservingMergeHelper<TInputOutput, TKey> : IMergeHelper<TInputOutput>
{
	private QueryTaskGroupState _taskGroupState;

	private PartitionedStream<TInputOutput, TKey> _partitions;

	private Shared<TInputOutput[]> _results;

	private TaskScheduler _taskScheduler;

	internal OrderPreservingMergeHelper(PartitionedStream<TInputOutput, TKey> partitions, TaskScheduler taskScheduler, CancellationState cancellationState, int queryId)
	{
		_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
		_partitions = partitions;
		_results = new Shared<TInputOutput[]>(null);
		_taskScheduler = taskScheduler;
	}

	void IMergeHelper<TInputOutput>.Execute()
	{
		OrderPreservingSpoolingTask<TInputOutput, TKey>.Spool(_taskGroupState, _partitions, _results, _taskScheduler);
	}

	IEnumerator<TInputOutput> IMergeHelper<TInputOutput>.GetEnumerator()
	{
		return ((IEnumerable<TInputOutput>)_results.Value).GetEnumerator();
	}

	public TInputOutput[] GetResultsAsArray()
	{
		return _results.Value;
	}
}
