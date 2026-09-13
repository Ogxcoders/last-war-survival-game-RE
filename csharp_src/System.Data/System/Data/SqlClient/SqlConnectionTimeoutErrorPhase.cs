namespace System.Data.SqlClient;

internal enum SqlConnectionTimeoutErrorPhase
{
	Undefined,
	PreLoginBegin,
	InitializeConnection,
	SendPreLoginHandshake,
	ConsumePreLoginHandshake,
	LoginBegin,
	ProcessConnectionAuth,
	PostLogin,
	Complete,
	Count
}
