namespace System.Linq.Parallel;

internal class SortQueryOperatorResults<TInputOutput, TSortKey> : QueryResults<TInputOutput>
{
	private class ChildResultsRecipient : IPartitionedStreamRecipient<TInputOutput>
	{
		private IPartitionedStreamRecipient<TInputOutput> _outputRecipient;

		private SortQueryOperator<TInputOutput, TSortKey> _op;

		private QuerySettings _settings;

		internal ChildResultsRecipient(IPartitionedStreamRecipient<TInputOutput> outputRecipient, SortQueryOperator<TInputOutput, TSortKey> op, QuerySettings settings)
		{
			_outputRecipient = outputRecipient;
			_op = op;
			_settings = settings;
		}

		public void Receive<TKey>(PartitionedStream<TInputOutput, TKey> childPartitionedStream)
		{
			_op.WrapPartitionedStream(childPartitionedStream, _outputRecipient, preferStriping: false, _settings);
		}
	}

	protected QueryResults<TInputOutput> _childQueryResults;

	private SortQueryOperator<TInputOutput, TSortKey> _op;

	private QuerySettings _settings;

	internal override bool IsIndexible => false;

	internal SortQueryOperatorResults(QueryResults<TInputOutput> childQueryResults, SortQueryOperator<TInputOutput, TSortKey> op, QuerySettings settings)
	{
		_childQueryResults = childQueryResults;
		_op = op;
		_settings = settings;
	}

	internal override void GivePartitionedStream(IPartitionedStreamRecipient<TInputOutput> recipient)
	{
		_childQueryResults.GivePartitionedStream(new ChildResultsRecipient(recipient, _op, _settings));
	}
}
