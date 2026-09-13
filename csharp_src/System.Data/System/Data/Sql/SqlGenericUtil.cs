using System.Data.Common;

namespace System.Data.Sql;

internal sealed class SqlGenericUtil
{
	private SqlGenericUtil()
	{
	}

	internal static Exception NullCommandText()
	{
		return ADP.Argument(Res.GetString("Command parameter must have a non null and non empty command text."));
	}

	internal static Exception MismatchedMetaDataDirectionArrayLengths()
	{
		return ADP.Argument(Res.GetString("MetaData parameter array must have length equivalent to ParameterDirection array argument."));
	}
}
