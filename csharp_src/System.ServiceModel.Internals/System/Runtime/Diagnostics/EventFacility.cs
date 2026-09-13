namespace System.Runtime.Diagnostics;

internal enum EventFacility : uint
{
	Tracing = 65536u,
	ServiceModel = 131072u,
	TransactionBridge = 196608u,
	SMSvcHost = 262144u,
	InfoCards = 327680u,
	SecurityAudit = 393216u
}
