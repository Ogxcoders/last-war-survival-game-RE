using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal sealed class WhereQueryOperator<TInputOutput> : UnaryQueryOperator<TInputOutput, TInputOutput>
{
	private class WhereQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TInputOutput, TKey>
	{
		private readonly QueryOperatorEnumerator<TInputOutput, TKey> _source;

		private readonly Func<TInputOutput, bool> _predicate;

		private CancellationToken _cancellationToken;

		private Shared<int> _outputLoopCount;

		internal WhereQueryOperatorEnumerator(QueryOperatorEnumerator<TInputOutput, TKey> source, Func<TInputOutput, bool> predicate, CancellationToken cancellationToken)
		{
			_source = source;
			_predicate = predicate;
			_cancellationToken = cancellationToken;
		}

		internal override bool MoveNext(ref TInputOutput currentElement, ref TKey currentKey)
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
				if (_predicate(currentElement))
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

	private Func<TInputOutput, bool> _predicate;

	internal override bool LimitsParallelism => false;

	internal WhereQueryOperator(IEnumerable<TInputOutput> child, Func<TInputOutput, bool> predicate)
		: base(child)
	{
		SetOrdinalIndexState(base.Child.OrdinalIndexState.Worse(OrdinalIndexState.Increasing));
		_predicate = predicate;
	}

	internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
	{
		PartitionedStream<TInputOutput, TKey> partitionedStream = new PartitionedStream<TInputOutput, TKey>(inputStream.PartitionCount, inputStream.KeyComparer, OrdinalIndexState);
		for (int i = 0; i < inputStream.PartitionCount; i++)
		{
			partitionedStream[i] = new WhereQueryOperatorEnumerator<TKey>(inputStream[i], _predicate, settings.CancellationState.MergedCancellationToken);
		}
		recipient.Receive(partitionedStream);
	}

	internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
	{
		return new UnaryQueryOperatorResults(base.Child.Open(settings, preferStriping), this, settings, preferStriping);
	}

	internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
	{
		return CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).Where(_predicate);
	}
}
