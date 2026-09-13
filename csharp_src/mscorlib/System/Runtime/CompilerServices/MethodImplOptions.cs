using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[Serializable]
[Flags]
[ComVisible(true)]
public enum MethodImplOptions
{
	Unmanaged = 4,
	ForwardRef = 0x10,
	PreserveSig = 0x80,
	InternalCall = 0x1000,
	Synchronized = 0x20,
	NoInlining = 8,
	[ComVisible(false)]
	AggressiveInlining = 0x100,
	NoOptimization = 0x40
}
