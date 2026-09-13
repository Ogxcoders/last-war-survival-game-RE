namespace System.Runtime.Diagnostics;

internal enum EventSeverity : uint
{
	Success = 0u,
	Informational = 1073741824u,
	Warning = 2147483648u,
	Error = 3221225472u
}
