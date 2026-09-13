using System.Collections.Generic;

namespace System.Linq.Parallel;

internal class ProducerComparerInt : IComparer<Producer<int>>
{
	public int Compare(Producer<int> x, Producer<int> y)
	{
		return y.MaxKey - x.MaxKey;
	}
}
