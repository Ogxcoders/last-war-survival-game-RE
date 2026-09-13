using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles;

[SecurityCritical]
[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
public abstract class SafeHandleZeroOrMinusOneIsInvalid : SafeHandle
{
	public override bool IsInvalid
	{
		[SecurityCritical]
		get
		{
			if (!handle.IsNull())
			{
				return handle == new IntPtr(-1);
			}
			return true;
		}
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	protected SafeHandleZeroOrMinusOneIsInvalid(bool ownsHandle)
		: base(IntPtr.Zero, ownsHandle)
	{
	}
}
