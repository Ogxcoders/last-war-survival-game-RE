using System.Collections.ObjectModel;

namespace System.Data.Common;

public static class DbDataReaderExtensions
{
	public static ReadOnlyCollection<DbColumn> GetColumnSchema(this DbDataReader reader)
	{
		if (reader.CanGetColumnSchema())
		{
			return ((IDbColumnSchemaGenerator)reader).GetColumnSchema();
		}
		throw new NotSupportedException();
	}

	public static bool CanGetColumnSchema(this DbDataReader reader)
	{
		return reader is IDbColumnSchemaGenerator;
	}
}
