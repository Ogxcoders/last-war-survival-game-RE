namespace System.Data.SqlClient;

internal enum RunBehavior
{
	UntilDone = 1,
	ReturnImmediately = 2,
	Clean = 5,
	Attention = 13
}
