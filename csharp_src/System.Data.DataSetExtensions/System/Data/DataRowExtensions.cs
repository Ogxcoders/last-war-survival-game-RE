namespace System.Data;

public static class DataRowExtensions
{
	private static class UnboxT<T>
	{
		internal static readonly Converter<object, T> s_unbox = Create();

		private static Converter<object, T> Create()
		{
			if (default(T) == null)
			{
				return ReferenceOrNullableField;
			}
			return ValueField;
		}

		private static T ReferenceOrNullableField(object value)
		{
			if (DBNull.Value != value)
			{
				return (T)value;
			}
			return default(T);
		}

		private static T ValueField(object value)
		{
			if (DBNull.Value == value)
			{
				throw DataSetUtil.InvalidCast($"Cannot cast DBNull. Value to type '{typeof(T).ToString()}'. Please use a nullable type.");
			}
			return (T)value;
		}
	}

	public static T Field<T>(this DataRow row, string columnName)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		return UnboxT<T>.s_unbox(row[columnName]);
	}

	public static T Field<T>(this DataRow row, DataColumn column)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		return UnboxT<T>.s_unbox(row[column]);
	}

	public static T Field<T>(this DataRow row, int columnIndex)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		return UnboxT<T>.s_unbox(row[columnIndex]);
	}

	public static T Field<T>(this DataRow row, int columnIndex, DataRowVersion version)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		return UnboxT<T>.s_unbox(row[columnIndex, version]);
	}

	public static T Field<T>(this DataRow row, string columnName, DataRowVersion version)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		return UnboxT<T>.s_unbox(row[columnName, version]);
	}

	public static T Field<T>(this DataRow row, DataColumn column, DataRowVersion version)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		return UnboxT<T>.s_unbox(row[column, version]);
	}

	public static void SetField<T>(this DataRow row, int columnIndex, T value)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		row[columnIndex] = ((object)value) ?? DBNull.Value;
	}

	public static void SetField<T>(this DataRow row, string columnName, T value)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		row[columnName] = ((object)value) ?? DBNull.Value;
	}

	public static void SetField<T>(this DataRow row, DataColumn column, T value)
	{
		DataSetUtil.CheckArgumentNull(row, "row");
		row[column] = ((object)value) ?? DBNull.Value;
	}
}
