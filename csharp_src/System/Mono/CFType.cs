using System;
using System.Runtime.InteropServices;

namespace Mono;

internal class CFType
{
	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation", EntryPoint = "CFGetTypeID")]
	public static extern IntPtr GetTypeID(IntPtr typeRef);
}
