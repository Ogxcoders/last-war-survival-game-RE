using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal sealed class JoinQueryOperator<TLeftInput, TRightInput, TKey, TOutput> : BinaryQueryOperator<TLeftInput, TRightInput, TOutput>
{
	private readonly Func<TLeftInput, TKey> _leftKeySelector;

	private readonly Func<TRightInput, TKey> _rightKeySelector;

	private readonly Func<TLeftInput, TRightInput, TOutput> _resultSelector;

	private readonly IEqualityComparer<TKey> _keyComparer;

	internal override bool LimitsParallelism => false;

	internal JoinQueryOperator(ParallelQuery<TLeftInput> left, ParallelQuery<TRightInput> right, Func<TLeftInput, TKey> leftKeySelector, Func<TRightInput, TKey> rightKeySelector, Func<TLeftInput, TRightInput, TOutput> resultSelector, IEqualityComparer<TKey> keyComparer)
		: base(left, right)
	{
		_leftKeySelector = leftKeySelector;
		_rightKeySelector = rightKeySelector;
		_resultSelector = resultSelector;
		_keyComparer = keyComparer;
		_outputOrdered = base.LeftChild.OutputOrdered;
		SetOrdinalIndex(OrdinalIndexState.Shuffled);
	}

	public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TLeftInput, TLeftKey> leftStream, PartitionedStream<TRightInput, TRightKey> rightStream, IPartitionedStreamRecipient<TOutput> outputRecipient, bool preferStriping, QuerySettings settings)
	{
		if (base.LeftChild.OutputOrdered)
		{
			WrapPartitionedStreamHelper(ExchangeUtilities.HashRepartitionOrdered(leftStream, _leftKeySelector, _keyComparer, null, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
		}
		else
		{
			WrapPartitionedStreamHelper(ExchangeUtilities.HashRepartition(leftStream, _leftKeySelector, _keyComparer, null, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
		}
	}

	private void WrapPartitionedStreamHelper<TLeftKey, TRightKey>(PartitionedStream<Pair<TLeftInput, TKey>, TLeftKey> leftHashStream, PartitionedStream<TRightInput, TRightKey> rightPartitionedStream, IPartitionedStreamRecipient<TOutput> outputRecipient, CancellationToken cancellationToken)
	{
		int partitionCount = leftHashStream.PartitionCount;
		PartitionedStream<Pair<TRightInput, TKey>, int> partitionedStream = ExchangeUtilities.HashRepartition(rightPartitionedStream, _rightKeySelector, _keyComparer, null, cancellationToken);
		PartitionedStream<TOutput, TLeftKey> partitionedStream2 = new PartitionedStream<TOutput, TLeftKey>(partitionCount, leftHashStream.KeyComparer, OrdinalIndexState);
		for (int i = 0; i < partitionCount; i++)
		{
			partitionedStream2[i] = new HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, TKey, TOutput>(leftHashStream[i], partitionedStream[i], _resultSelector, null, _keyComparer, cancellationToken);
		}
		outputRecipient.Receive(partitionedStream2);
	}

	internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
	{
		QueryResults<TLeftInput> leftChildQueryResults = base.LeftChild.Open(settings, preferStriping: false);
		QueryResults<TRightInput> rightChildQueryResults = base.RightChild.Open(settings, preferStriping: false);
		return new BinaryQueryOperatorResults(leftChildQueryResults, rightChildQueryResults, this, settings, preferStriping: false);
	}

	internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
	{
		IEnumerable<TLeftInput> outer = CancellableEnumerable.Wrap(base.LeftChild.AsSequentialQuery(token), token);
		IEnumerable<TRightInput> inner = CancellableEnumerable.Wrap(base.RightChild.AsSequentialQuery(token), token);
		return outer.Join(inner, _leftKeySelector, _rightKeySelector, _resultSelector, _keyComparer);
	}
}
