namespace System.IO.MemoryMappedFiles;

[Serializable]
public enum MemoryMappedFileAccess
{
	ReadWrite,
	Read,
	Write,
	CopyOnWrite,
	ReadExecute,
	ReadWriteExecute
}
