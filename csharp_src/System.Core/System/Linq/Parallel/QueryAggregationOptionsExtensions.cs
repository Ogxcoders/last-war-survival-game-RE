namespace System.Linq.Parallel;

internal static class QueryAggregationOptionsExtensions
{
	public static bool IsValidQueryAggregationOption(this QueryAggregationOptions value)
	{
		if (value != QueryAggregationOptions.None && value != QueryAggregationOptions.Associative && value != QueryAggregationOptions.Commutative)
		{
			return value == QueryAggregationOptions.AssociativeCommutative;
		}
		return true;
	}
}
