using System.Collections.Generic;

namespace System.Data;

public static class TypedTableBaseExtensions
{
	public static EnumerableRowCollection<TRow> Where<TRow>(this TypedTableBase<TRow> source, Func<TRow, bool> predicate) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source).Where(predicate);
	}

	public static OrderedEnumerableRowCollection<TRow> OrderBy<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source).OrderBy(keySelector);
	}

	public static OrderedEnumerableRowCollection<TRow> OrderBy<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source).OrderBy(keySelector, comparer);
	}

	public static OrderedEnumerableRowCollection<TRow> OrderByDescending<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source).OrderByDescending(keySelector);
	}

	public static OrderedEnumerableRowCollection<TRow> OrderByDescending<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source).OrderByDescending(keySelector, comparer);
	}

	public static EnumerableRowCollection<S> Select<TRow, S>(this TypedTableBase<TRow> source, Func<TRow, S> selector) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source).Select(selector);
	}

	public static EnumerableRowCollection<TRow> AsEnumerable<TRow>(this TypedTableBase<TRow> source) where TRow : DataRow
	{
		DataSetUtil.CheckArgumentNull(source, "source");
		return new EnumerableRowCollection<TRow>(source);
	}

	public static TRow ElementAtOrDefault<TRow>(this TypedTableBase<TRow> source, int index) where TRow : DataRow
	{
		if (index >= 0 && index < source.Rows.Count)
		{
			return (TRow)source.Rows[index];
		}
		return null;
	}
}
