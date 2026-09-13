using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

internal class ParameterPeekAheadValue
{
	internal IEnumerator<SqlDataRecord> Enumerator;

	internal SqlDataRecord FirstRecord;
}
