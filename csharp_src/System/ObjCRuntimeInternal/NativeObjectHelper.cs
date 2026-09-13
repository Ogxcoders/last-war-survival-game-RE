using System;

namespace ObjCRuntimeInternal;

internal static class NativeObjectHelper
{
	public static IntPtr GetHandle(this INativeObject self)
	{
		return self?.Handle ?? IntPtr.Zero;
	}
}
