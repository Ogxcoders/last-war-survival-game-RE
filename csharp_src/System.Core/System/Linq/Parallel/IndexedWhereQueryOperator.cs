using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal sealed class IndexedWhereQueryOperator<TInputOutput> : UnaryQueryOperator<TInputOutput, TInputOutput>
{
	private class IndexedWhereQueryOperatorEnumerator : QueryOperatorEnumerator<TInputOutput, int>
	{
		private readonly QueryOperatorEnumerator<TInputOutput, int> _source;

		private readonly Func<TInputOutput, int, bool> _predicate;

		private CancellationToken _cancellationToken;

		private Shared<int> _outputLoopCount;

		internal IndexedWhereQueryOperatorEnumerator(QueryOperatorEnumerator<TInputOutput, int> source, Func<TInputOutput, int, bool> predicate, CancellationToken cancellationToken)
		{
			_source = source;
			_predicate = predicate;
			_cancellationToken = cancellationToken;
		}

		internal override bool MoveNext(ref TInputOutput currentElement, ref int currentKey)
		{
			if (_outputLoopCount == null)
			{
				_outputLoopCount = new Shared<int>(0);
			}
			while (_source.MoveNext(ref currentElement, ref currentKey))
			{
				if ((_outputLoopCount.Value++ & 0x3F) == 0)
				{
					CancellationState.ThrowIfCanceled(_cancellationToken);
				}
				if (_predicate(currentElement, currentKey))
				{
					return true;
				}
			}
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			_source.Dispose();
		}
	}

	private Func<TInputOutput, int, bool> _predicate;

	private bool _prematureMerge;

	private bool _limitsParallelism;

	internal override bool LimitsParallelism => _limitsParallelism;

	internal IndexedWhereQueryOperator(IEnumerable<TInputOutput> child, Func<TInputOutput, int, bool> predicate)
		: base(child)
	{
		_predicate = predicate;
		_outputOrdered = true;
		InitOrdinalIndexState();
	}

	private void InitOrdinalIndexState()
	{
		OrdinalIndexState ordinalIndexState = base.Child.OrdinalIndexState;
		if (ordinalIndexState.IsWorseThan(OrdinalIndexState.Correct))
		{
			_prematureMerge = true;
			_limitsParallelism = ordinalIndexState != OrdinalIndexState.Shuffled;
		}
		SetOrdinalIndexState(OrdinalIndexState.Increasing);
	}

	internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
	{
		return new UnaryQueryOperatorResults(base.Child.Open(settings, preferStriping), this, settings, preferStriping);
	}

	internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
	{
		int partitionCount = inputStream.PartitionCount;
		PartitionedStream<TInputOutput, int> partitionedStream = ((!_prematureMerge) ? ((PartitionedStream<TInputOutput, int>)(object)inputStream) : QueryOperator<TInputOutput>.ExecuteAndCollectResults(inputStream, partitionCount, base.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream());
		PartitionedStream<TInputOutput, int> partitionedStream2 = new PartitionedStream<TInputOutput, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState);
		for (int i = 0; i < partitionCount; i++)
		{
			partitionedStream2[i] = new IndexedWhereQueryOperatorEnumerator(partitionedStream[i], _predicate, settings.CancellationState.MergedCancellationToken);
		}
		recipient.Receive(partitionedStream2);
	}

	internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
	{
		return CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).Where(_predicate);
	}
}
