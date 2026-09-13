namespace System.IO.MemoryMappedFiles;

[Serializable]
[Flags]
public enum MemoryMappedFileOptions
{
	None = 0,
	DelayAllocatePages = 0x4000000
}
