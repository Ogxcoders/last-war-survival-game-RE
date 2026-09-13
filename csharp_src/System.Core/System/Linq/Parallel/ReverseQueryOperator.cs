using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal sealed class ReverseQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
{
	private class ReverseQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TSource, TKey>
	{
		private readonly QueryOperatorEnumerator<TSource, TKey> _source;

		private readonly CancellationToken _cancellationToken;

		private List<Pair<TSource, TKey>> _buffer;

		private Shared<int> _bufferIndex;

		internal ReverseQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TKey> source, CancellationToken cancellationToken)
		{
			_source = source;
			_cancellationToken = cancellationToken;
		}

		internal override bool MoveNext(ref TSource currentElement, ref TKey currentKey)
		{
			if (_buffer == null)
			{
				_bufferIndex = new Shared<int>(0);
				_buffer = new List<Pair<TSource, TKey>>();
				TSource currentElement2 = default(TSource);
				TKey currentKey2 = default(TKey);
				int num = 0;
				while (_source.MoveNext(ref currentElement2, ref currentKey2))
				{
					if ((num++ & 0x3F) == 0)
					{
						CancellationState.ThrowIfCanceled(_cancellationToken);
					}
					_buffer.Add(new Pair<TSource, TKey>(currentElement2, currentKey2));
					_bufferIndex.Value++;
				}
			}
			if (--_bufferIndex.Value >= 0)
			{
				currentElement = _buffer[_bufferIndex.Value].First;
				currentKey = _buffer[_bufferIndex.Value].Second;
				return true;
			}
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			_source.Dispose();
		}
	}

	private class ReverseQueryOperatorResults : UnaryQueryOperatorResults
	{
		private int _count;

		internal override bool IsIndexible => true;

		internal override int ElementsCount => _count;

		public static QueryResults<TSource> NewResults(QueryResults<TSource> childQueryResults, ReverseQueryOperator<TSource> op, QuerySettings settings, bool preferStriping)
		{
			if (childQueryResults.IsIndexible)
			{
				return new ReverseQueryOperatorResults(childQueryResults, op, settings, preferStriping);
			}
			return new UnaryQueryOperatorResults(childQueryResults, op, settings, preferStriping);
		}

		private ReverseQueryOperatorResults(QueryResults<TSource> childQueryResults, ReverseQueryOperator<TSource> op, QuerySettings settings, bool preferStriping)
			: base(childQueryResults, (UnaryQueryOperator<TSource, TSource>)op, settings, preferStriping)
		{
			_count = _childQueryResults.ElementsCount;
		}

		internal override TSource GetElement(int index)
		{
			return _childQueryResults.GetElement(_count - index - 1);
		}
	}

	internal override bool LimitsParallelism => false;

	internal ReverseQueryOperator(IEnumerable<TSource> child)
		: base(child)
	{
		if (base.Child.OrdinalIndexState == OrdinalIndexState.Indexable)
		{
			SetOrdinalIndexState(OrdinalIndexState.Indexable);
		}
		else
		{
			SetOrdinalIndexState(OrdinalIndexState.Shuffled);
		}
	}

	internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
	{
		int partitionCount = inputStream.PartitionCount;
		PartitionedStream<TSource, TKey> partitionedStream = new PartitionedStream<TSource, TKey>(partitionCount, new ReverseComparer<TKey>(inputStream.KeyComparer), OrdinalIndexState.Shuffled);
		for (int i = 0; i < partitionCount; i++)
		{
			partitionedStream[i] = new ReverseQueryOperatorEnumerator<TKey>(inputStream[i], settings.CancellationState.MergedCancellationToken);
		}
		recipient.Receive(partitionedStream);
	}

	internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
	{
		return ReverseQueryOperatorResults.NewResults(base.Child.Open(settings, preferStriping: false), this, settings, preferStriping);
	}

	internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
	{
		return CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).Reverse();
	}
}
