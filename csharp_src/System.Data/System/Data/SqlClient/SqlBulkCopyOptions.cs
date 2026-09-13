namespace System.Data.SqlClient;

[Flags]
public enum SqlBulkCopyOptions
{
	Default = 0,
	KeepIdentity = 1,
	CheckConstraints = 2,
	TableLock = 4,
	KeepNulls = 8,
	FireTriggers = 0x10,
	UseInternalTransaction = 0x20
}
