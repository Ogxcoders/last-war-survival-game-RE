using System.Security;

namespace System.Runtime.InteropServices;

[ComVisible(false)]
public interface ICustomQueryInterface
{
	[SecurityCritical]
	CustomQueryInterfaceResult GetInterface([In] ref Guid iid, out IntPtr ppv);
}
