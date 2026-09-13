namespace System.Runtime.Diagnostics;

internal enum EventLogCategory : ushort
{
	ServiceAuthorization = 1,
	MessageAuthentication,
	ObjectAccess,
	Tracing,
	WebHost,
	FailFast,
	MessageLogging,
	PerformanceCounter,
	Wmi,
	ComPlus,
	StateMachine,
	Wsat,
	SharingService,
	ListenerAdapter
}
