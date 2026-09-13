using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal class PartitionerQueryOperator<TElement> : QueryOperator<TElement>
{
	private class PartitionerQueryOperatorResults : QueryResults<TElement>
	{
		private Partitioner<TElement> _partitioner;

		private QuerySettings _settings;

		internal PartitionerQueryOperatorResults(Partitioner<TElement> partitioner, QuerySettings settings)
		{
			_partitioner = partitioner;
			_settings = settings;
		}

		internal override void GivePartitionedStream(IPartitionedStreamRecipient<TElement> recipient)
		{
			int value = _settings.DegreeOfParallelism.Value;
			OrderablePartitioner<TElement> orderablePartitioner = _partitioner as OrderablePartitioner<TElement>;
			OrdinalIndexState indexState = ((orderablePartitioner != null) ? PartitionerQueryOperator<TElement>.GetOrdinalIndexState((Partitioner<TElement>)orderablePartitioner) : OrdinalIndexState.Shuffled);
			PartitionedStream<TElement, int> partitionedStream = new PartitionedStream<TElement, int>(value, Util.GetDefaultComparer<int>(), indexState);
			if (orderablePartitioner != null)
			{
				IList<IEnumerator<KeyValuePair<long, TElement>>> orderablePartitions = orderablePartitioner.GetOrderablePartitions(value);
				if (orderablePartitions == null)
				{
					throw new InvalidOperationException("Partitioner returned null instead of a list of partitions.");
				}
				if (orderablePartitions.Count != value)
				{
					throw new InvalidOperationException("Partitioner returned a wrong number of partitions.");
				}
				for (int i = 0; i < value; i++)
				{
					IEnumerator<KeyValuePair<long, TElement>> enumerator = orderablePartitions[i];
					if (enumerator == null)
					{
						throw new InvalidOperationException("Partitioner returned a null partition.");
					}
					partitionedStream[i] = new OrderablePartitionerEnumerator(enumerator);
				}
			}
			else
			{
				IList<IEnumerator<TElement>> partitions = _partitioner.GetPartitions(value);
				if (partitions == null)
				{
					throw new InvalidOperationException("Partitioner returned null instead of a list of partitions.");
				}
				if (partitions.Count != value)
				{
					throw new InvalidOperationException("Partitioner returned a wrong number of partitions.");
				}
				for (int j = 0; j < value; j++)
				{
					IEnumerator<TElement> enumerator2 = partitions[j];
					if (enumerator2 == null)
					{
						throw new InvalidOperationException("Partitioner returned a null partition.");
					}
					partitionedStream[j] = new PartitionerEnumerator(enumerator2);
				}
			}
			recipient.Receive(partitionedStream);
		}
	}

	private class OrderablePartitionerEnumerator : QueryOperatorEnumerator<TElement, int>
	{
		private IEnumerator<KeyValuePair<long, TElement>> _sourceEnumerator;

		internal OrderablePartitionerEnumerator(IEnumerator<KeyValuePair<long, TElement>> sourceEnumerator)
		{
			_sourceEnumerator = sourceEnumerator;
		}

		internal override bool MoveNext(ref TElement currentElement, ref int currentKey)
		{
			if (!_sourceEnumerator.MoveNext())
			{
				return false;
			}
			KeyValuePair<long, TElement> current = _sourceEnumerator.Current;
			currentElement = current.Value;
			currentKey = checked((int)current.Key);
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			_sourceEnumerator.Dispose();
		}
	}

	private class PartitionerEnumerator : QueryOperatorEnumerator<TElement, int>
	{
		private IEnumerator<TElement> _sourceEnumerator;

		internal PartitionerEnumerator(IEnumerator<TElement> sourceEnumerator)
		{
			_sourceEnumerator = sourceEnumerator;
		}

		internal override bool MoveNext(ref TElement currentElement, ref int currentKey)
		{
			if (!_sourceEnumerator.MoveNext())
			{
				return false;
			}
			currentElement = _sourceEnumerator.Current;
			currentKey = 0;
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			_sourceEnumerator.Dispose();
		}
	}

	private Partitioner<TElement> _partitioner;

	internal bool Orderable => _partitioner is OrderablePartitioner<TElement>;

	internal override OrdinalIndexState OrdinalIndexState => GetOrdinalIndexState(_partitioner);

	internal override bool LimitsParallelism => false;

	internal PartitionerQueryOperator(Partitioner<TElement> partitioner)
		: base(isOrdered: false, QuerySettings.Empty)
	{
		_partitioner = partitioner;
	}

	internal override QueryResults<TElement> Open(QuerySettings settings, bool preferStriping)
	{
		return new PartitionerQueryOperatorResults(_partitioner, settings);
	}

	internal override IEnumerable<TElement> AsSequentialQuery(CancellationToken token)
	{
		using IEnumerator<TElement> enumerator = _partitioner.GetPartitions(1)[0];
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current;
		}
	}

	internal static OrdinalIndexState GetOrdinalIndexState(Partitioner<TElement> partitioner)
	{
		if (!(partitioner is OrderablePartitioner<TElement> orderablePartitioner))
		{
			return OrdinalIndexState.Shuffled;
		}
		if (orderablePartitioner.KeysOrderedInEachPartition)
		{
			if (orderablePartitioner.KeysNormalized)
			{
				return OrdinalIndexState.Correct;
			}
			return OrdinalIndexState.Increasing;
		}
		return OrdinalIndexState.Shuffled;
	}
}
