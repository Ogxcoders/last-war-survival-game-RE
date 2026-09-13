using System.Threading.Tasks;

namespace System.Linq.Parallel;

internal class PartitionedStreamMerger<TOutput> : IPartitionedStreamRecipient<TOutput>
{
	private bool _forEffectMerge;

	private ParallelMergeOptions _mergeOptions;

	private bool _isOrdered;

	private MergeExecutor<TOutput> _mergeExecutor;

	private TaskScheduler _taskScheduler;

	private int _queryId;

	private CancellationState _cancellationState;

	internal MergeExecutor<TOutput> MergeExecutor => _mergeExecutor;

	internal PartitionedStreamMerger(bool forEffectMerge, ParallelMergeOptions mergeOptions, TaskScheduler taskScheduler, bool outputOrdered, CancellationState cancellationState, int queryId)
	{
		_forEffectMerge = forEffectMerge;
		_mergeOptions = mergeOptions;
		_isOrdered = outputOrdered;
		_taskScheduler = taskScheduler;
		_cancellationState = cancellationState;
		_queryId = queryId;
	}

	public void Receive<TKey>(PartitionedStream<TOutput, TKey> partitionedStream)
	{
		_mergeExecutor = MergeExecutor<TOutput>.Execute(partitionedStream, _forEffectMerge, _mergeOptions, _taskScheduler, _isOrdered, _cancellationState, _queryId);
	}
}
