using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq.Parallel;

internal class DefaultMergeHelper<TInputOutput, TIgnoreKey> : IMergeHelper<TInputOutput>
{
	private QueryTaskGroupState _taskGroupState;

	private PartitionedStream<TInputOutput, TIgnoreKey> _partitions;

	private AsynchronousChannel<TInputOutput>[] _asyncChannels;

	private SynchronousChannel<TInputOutput>[] _syncChannels;

	private IEnumerator<TInputOutput> _channelEnumerator;

	private TaskScheduler _taskScheduler;

	private bool _ignoreOutput;

	internal DefaultMergeHelper(PartitionedStream<TInputOutput, TIgnoreKey> partitions, bool ignoreOutput, ParallelMergeOptions options, TaskScheduler taskScheduler, CancellationState cancellationState, int queryId)
	{
		_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
		_partitions = partitions;
		_taskScheduler = taskScheduler;
		_ignoreOutput = ignoreOutput;
		IntValueEvent consumerEvent = new IntValueEvent();
		if (ignoreOutput)
		{
			return;
		}
		if (options != ParallelMergeOptions.FullyBuffered)
		{
			if (partitions.PartitionCount > 1)
			{
				_asyncChannels = MergeExecutor<TInputOutput>.MakeAsynchronousChannels(partitions.PartitionCount, options, consumerEvent, cancellationState.MergedCancellationToken);
				_channelEnumerator = new AsynchronousChannelMergeEnumerator<TInputOutput>(_taskGroupState, _asyncChannels, consumerEvent);
			}
			else
			{
				_channelEnumerator = ExceptionAggregator.WrapQueryEnumerator(partitions[0], _taskGroupState.CancellationState).GetEnumerator();
			}
		}
		else
		{
			_syncChannels = MergeExecutor<TInputOutput>.MakeSynchronousChannels(partitions.PartitionCount);
			_channelEnumerator = new SynchronousChannelMergeEnumerator<TInputOutput>(_taskGroupState, _syncChannels);
		}
	}

	void IMergeHelper<TInputOutput>.Execute()
	{
		if (_asyncChannels != null)
		{
			SpoolingTask.SpoolPipeline(_taskGroupState, _partitions, _asyncChannels, _taskScheduler);
		}
		else if (_syncChannels != null)
		{
			SpoolingTask.SpoolStopAndGo(_taskGroupState, _partitions, _syncChannels, _taskScheduler);
		}
		else if (_ignoreOutput)
		{
			SpoolingTask.SpoolForAll(_taskGroupState, _partitions, _taskScheduler);
		}
	}

	IEnumerator<TInputOutput> IMergeHelper<TInputOutput>.GetEnumerator()
	{
		return _channelEnumerator;
	}

	public TInputOutput[] GetResultsAsArray()
	{
		if (_syncChannels != null)
		{
			int num = 0;
			for (int i = 0; i < _syncChannels.Length; i++)
			{
				num += _syncChannels[i].Count;
			}
			TInputOutput[] array = new TInputOutput[num];
			int num2 = 0;
			for (int j = 0; j < _syncChannels.Length; j++)
			{
				_syncChannels[j].CopyTo(array, num2);
				num2 += _syncChannels[j].Count;
			}
			return array;
		}
		List<TInputOutput> list = new List<TInputOutput>();
		foreach (TInputOutput item in (IMergeHelper<TInputOutput>)this)
		{
			list.Add(item);
		}
		return list.ToArray();
	}
}
