using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

public class MemoryMappedFileSecurity : ObjectSecurity<MemoryMappedFileRights>
{
	public MemoryMappedFileSecurity()
		: base(isContainer: false, ResourceType.KernelObject)
	{
	}

	[SecuritySafeCritical]
	internal MemoryMappedFileSecurity(SafeMemoryMappedFileHandle safeHandle, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.KernelObject, (SafeHandle)safeHandle, includeSections)
	{
	}

	[SecuritySafeCritical]
	internal void PersistHandle(SafeHandle handle)
	{
		Persist(handle);
	}
}
