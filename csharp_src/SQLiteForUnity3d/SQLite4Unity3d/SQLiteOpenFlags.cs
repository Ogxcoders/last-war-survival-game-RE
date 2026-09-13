using System;

namespace SQLite4Unity3d;

[Flags]
public enum SQLiteOpenFlags
{
	ReadOnly = 1,
	ReadWrite = 2,
	Create = 4,
	NoMutex = 0x8000,
	FullMutex = 0x10000,
	SharedCache = 0x20000,
	PrivateCache = 0x40000,
	ProtectionComplete = 0x100000,
	ProtectionCompleteUnlessOpen = 0x200000,
	ProtectionCompleteUntilFirstUserAuthentication = 0x300000,
	ProtectionNone = 0x400000
}
