using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles;

[SecurityCritical]
[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
public abstract class SafeHandleMinusOneIsInvalid : SafeHandle
{
	public override bool IsInvalid
	{
		[SecurityCritical]
		get
		{
			return handle == new IntPtr(-1);
		}
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	protected SafeHandleMinusOneIsInvalid(bool ownsHandle)
		: base(new IntPtr(-1), ownsHandle)
	{
	}
}
