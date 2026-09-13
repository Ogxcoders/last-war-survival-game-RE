namespace System.Data.SqlClient;

internal enum TransactionState
{
	Pending,
	Active,
	Aborted,
	Committed,
	Unknown
}
