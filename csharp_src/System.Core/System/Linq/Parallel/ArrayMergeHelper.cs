using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq.Parallel;

internal class ArrayMergeHelper<TInputOutput> : IMergeHelper<TInputOutput>
{
	private QueryResults<TInputOutput> _queryResults;

	private TInputOutput[] _outputArray;

	private QuerySettings _settings;

	public ArrayMergeHelper(QuerySettings settings, QueryResults<TInputOutput> queryResults)
	{
		_settings = settings;
		_queryResults = queryResults;
		int count = _queryResults.Count;
		_outputArray = new TInputOutput[count];
	}

	private void ToArrayElement(int index)
	{
		_outputArray[index] = _queryResults[index];
	}

	public void Execute()
	{
		new QueryExecutionOption<int>(QueryOperator<int>.AsQueryOperator(ParallelEnumerable.Range(0, _queryResults.Count)), _settings).ForAll(ToArrayElement);
	}

	[ExcludeFromCodeCoverage]
	public IEnumerator<TInputOutput> GetEnumerator()
	{
		return ((IEnumerable<TInputOutput>)GetResultsAsArray()).GetEnumerator();
	}

	public TInputOutput[] GetResultsAsArray()
	{
		return _outputArray;
	}
}
