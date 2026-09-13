namespace System.Security.Permissions;

[Flags]
public enum HostProtectionResource
{
	All = 0x1FF,
	ExternalProcessMgmt = 4,
	ExternalThreading = 0x10,
	MayLeakOnAbort = 0x100,
	None = 0,
	SecurityInfrastructure = 0x40,
	SelfAffectingProcessMgmt = 8,
	SelfAffectingThreading = 0x20,
	SharedState = 2,
	Synchronization = 1,
	UI = 0x80
}
