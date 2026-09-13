namespace System.Data.SqlClient;

internal enum TransactionType
{
	LocalFromTSQL = 1,
	LocalFromAPI,
	Delegated,
	Distributed,
	Context
}
