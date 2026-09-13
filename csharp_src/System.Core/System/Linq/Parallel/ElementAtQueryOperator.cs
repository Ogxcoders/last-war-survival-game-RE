using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System.Linq.Parallel;

internal sealed class ElementAtQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
{
	private class ElementAtQueryOperatorEnumerator : QueryOperatorEnumerator<TSource, int>
	{
		private QueryOperatorEnumerator<TSource, int> _source;

		private int _index;

		private Shared<bool> _resultFoundFlag;

		private CancellationToken _cancellationToken;

		internal ElementAtQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, int> source, int index, Shared<bool> resultFoundFlag, CancellationToken cancellationToken)
		{
			_source = source;
			_index = index;
			_resultFoundFlag = resultFoundFlag;
			_cancellationToken = cancellationToken;
		}

		internal override bool MoveNext(ref TSource currentElement, ref int currentKey)
		{
			int num = 0;
			while (_source.MoveNext(ref currentElement, ref currentKey))
			{
				if ((num++ & 0x3F) == 0)
				{
					CancellationState.ThrowIfCanceled(_cancellationToken);
				}
				if (_resultFoundFlag.Value)
				{
					break;
				}
				if (currentKey == _index)
				{
					_resultFoundFlag.Value = true;
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

	private readonly int _index;

	private readonly bool _prematureMerge;

	private readonly bool _limitsParallelism;

	internal override bool LimitsParallelism => _limitsParallelism;

	internal ElementAtQueryOperator(IEnumerable<TSource> child, int index)
		: base(child)
	{
		_index = index;
		OrdinalIndexState ordinalIndexState = base.Child.OrdinalIndexState;
		if (ordinalIndexState.IsWorseThan(OrdinalIndexState.Correct))
		{
			_prematureMerge = true;
			_limitsParallelism = ordinalIndexState != OrdinalIndexState.Shuffled;
		}
	}

	internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
	{
		return new UnaryQueryOperatorResults(base.Child.Open(settings, preferStriping: false), this, settings, preferStriping);
	}

	internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
	{
		int partitionCount = inputStream.PartitionCount;
		PartitionedStream<TSource, int> partitionedStream = ((!_prematureMerge) ? ((PartitionedStream<TSource, int>)(object)inputStream) : QueryOperator<TSource>.ExecuteAndCollectResults(inputStream, partitionCount, base.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream());
		Shared<bool> resultFoundFlag = new Shared<bool>(value: false);
		PartitionedStream<TSource, int> partitionedStream2 = new PartitionedStream<TSource, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
		for (int i = 0; i < partitionCount; i++)
		{
			partitionedStream2[i] = new ElementAtQueryOperatorEnumerator(partitionedStream[i], _index, resultFoundFlag, settings.CancellationState.MergedCancellationToken);
		}
		recipient.Receive(partitionedStream2);
	}

	[ExcludeFromCodeCoverage]
	internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
	{
		throw new NotSupportedException();
	}

	internal bool Aggregate(out TSource result, bool withDefaultValue)
	{
		if (LimitsParallelism && base.SpecifiedQuerySettings.WithDefaults().ExecutionMode.Value != ParallelExecutionMode.ForceParallelism)
		{
			CancellationState cancellationState = base.SpecifiedQuerySettings.CancellationState;
			if (withDefaultValue)
			{
				IEnumerable<TSource> source = CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(cancellationState.ExternalCancellationToken), cancellationState.ExternalCancellationToken);
				result = ExceptionAggregator.WrapEnumerable(source, cancellationState).ElementAtOrDefault(_index);
			}
			else
			{
				IEnumerable<TSource> source2 = CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(cancellationState.ExternalCancellationToken), cancellationState.ExternalCancellationToken);
				result = ExceptionAggregator.WrapEnumerable(source2, cancellationState).ElementAt(_index);
			}
			return true;
		}
		using (IEnumerator<TSource> enumerator = GetEnumerator(ParallelMergeOptions.FullyBuffered))
		{
			if (enumerator.MoveNext())
			{
				TSource current = enumerator.Current;
				result = current;
				return true;
			}
		}
		result = default(TSource);
		return false;
	}
}
