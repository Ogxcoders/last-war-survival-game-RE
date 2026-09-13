using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore;

[Serializable]
[ComVisible(true)]
public enum SymAddressKind
{
	ILOffset = 1,
	NativeRVA,
	NativeRegister,
	NativeRegisterRelative,
	NativeOffset,
	NativeRegisterRegister,
	NativeRegisterStack,
	NativeStackRegister,
	BitField,
	NativeSectionOffset
}
