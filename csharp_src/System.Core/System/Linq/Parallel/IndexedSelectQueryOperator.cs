using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal sealed class IndexedSelectQueryOperator<TInput, TOutput> : UnaryQueryOperator<TInput, TOutput>
{
	private class IndexedSelectQueryOperatorEnumerator : QueryOperatorEnumerator<TOutput, int>
	{
		private readonly QueryOperatorEnumerator<TInput, int> _source;

		private readonly Func<TInput, int, TOutput> _selector;

		internal IndexedSelectQueryOperatorEnumerator(QueryOperatorEnumerator<TInput, int> source, Func<TInput, int, TOutput> selector)
		{
			_source = source;
			_selector = selector;
		}

		internal override bool MoveNext(ref TOutput currentElement, ref int currentKey)
		{
			TInput currentElement2 = default(TInput);
			if (_source.MoveNext(ref currentElement2, ref currentKey))
			{
				currentElement = _selector(currentElement2, currentKey);
				return true;
			}
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			_source.Dispose();
		}
	}

	private class IndexedSelectQueryOperatorResults : UnaryQueryOperatorResults
	{
		private IndexedSelectQueryOperator<TInput, TOutput> _selectOp;

		private int _childCount;

		internal override int ElementsCount => _childQueryResults.ElementsCount;

		internal override bool IsIndexible => true;

		public static QueryResults<TOutput> NewResults(QueryResults<TInput> childQueryResults, IndexedSelectQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
		{
			if (childQueryResults.IsIndexible)
			{
				return new IndexedSelectQueryOperatorResults(childQueryResults, op, settings, preferStriping);
			}
			return new UnaryQueryOperatorResults(childQueryResults, op, settings, preferStriping);
		}

		private IndexedSelectQueryOperatorResults(QueryResults<TInput> childQueryResults, IndexedSelectQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
			: base(childQueryResults, (UnaryQueryOperator<TInput, TOutput>)op, settings, preferStriping)
		{
			_selectOp = op;
			_childCount = _childQueryResults.ElementsCount;
		}

		internal override TOutput GetElement(int index)
		{
			return _selectOp._selector(_childQueryResults.GetElement(index), index);
		}
	}

	private readonly Func<TInput, int, TOutput> _selector;

	private bool _prematureMerge;

	private bool _limitsParallelism;

	internal override bool LimitsParallelism => _limitsParallelism;

	internal IndexedSelectQueryOperator(IEnumerable<TInput> child, Func<TInput, int, TOutput> selector)
		: base(child)
	{
		_selector = selector;
		_outputOrdered = true;
		InitOrdinalIndexState();
	}

	private void InitOrdinalIndexState()
	{
		OrdinalIndexState ordinalIndexState = base.Child.OrdinalIndexState;
		OrdinalIndexState ordinalIndexState2 = ordinalIndexState;
		if (ordinalIndexState.IsWorseThan(OrdinalIndexState.Correct))
		{
			_prematureMerge = true;
			_limitsParallelism = ordinalIndexState != OrdinalIndexState.Shuffled;
			ordinalIndexState2 = OrdinalIndexState.Correct;
		}
		SetOrdinalIndexState(ordinalIndexState2);
	}

	internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
	{
		return IndexedSelectQueryOperatorResults.NewResults(base.Child.Open(settings, preferStriping), this, settings, preferStriping);
	}

	internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, bool preferStriping, QuerySettings settings)
	{
		int partitionCount = inputStream.PartitionCount;
		PartitionedStream<TInput, int> partitionedStream = ((!_prematureMerge) ? ((PartitionedStream<TInput, int>)(object)inputStream) : QueryOperator<TInput>.ExecuteAndCollectResults(inputStream, partitionCount, base.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream());
		PartitionedStream<TOutput, int> partitionedStream2 = new PartitionedStream<TOutput, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState);
		for (int i = 0; i < partitionCount; i++)
		{
			partitionedStream2[i] = new IndexedSelectQueryOperatorEnumerator(partitionedStream[i], _selector);
		}
		recipient.Receive(partitionedStream2);
	}

	internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
	{
		return base.Child.AsSequentialQuery(token).Select(_selector);
	}
}
