using System.Collections.Generic;

namespace System.Linq;

internal static class Utilities
{
	public static bool AreEqualityComparersEqual<TSource>(IEqualityComparer<TSource> left, IEqualityComparer<TSource> right)
	{
		if (left == right)
		{
			return true;
		}
		EqualityComparer<TSource> equalityComparer = EqualityComparer<TSource>.Default;
		if (left == null)
		{
			if (right != equalityComparer)
			{
				return right.Equals(equalityComparer);
			}
			return true;
		}
		if (right == null)
		{
			if (left != equalityComparer)
			{
				return left.Equals(equalityComparer);
			}
			return true;
		}
		return left.Equals(right);
	}

	public static Func<TSource, bool> CombinePredicates<TSource>(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)
	{
		return (TSource x) => predicate1(x) && predicate2(x);
	}

	public static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
	{
		return (TSource x) => selector2(selector1(x));
	}
}
