namespace System.Net;

[Flags]
internal enum ThreadKinds
{
	Unknown = 0,
	User = 1,
	System = 2,
	Sync = 4,
	Async = 8,
	Timer = 0x10,
	CompletionPort = 0x20,
	Worker = 0x40,
	Finalization = 0x80,
	Other = 0x100,
	OwnerMask = 3,
	SyncMask = 0xC,
	SourceMask = 0x1F0,
	SafeSources = 0x160,
	ThreadPool = 0x60
}
