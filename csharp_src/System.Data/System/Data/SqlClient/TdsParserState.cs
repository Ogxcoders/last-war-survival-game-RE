namespace System.Data.SqlClient;

internal enum TdsParserState
{
	Closed,
	OpenNotLoggedIn,
	OpenLoggedIn,
	Broken
}
