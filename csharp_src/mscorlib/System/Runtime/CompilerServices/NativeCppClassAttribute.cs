using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[Serializable]
[AttributeUsage(AttributeTargets.Struct, Inherited = true)]
[ComVisible(true)]
public sealed class NativeCppClassAttribute : Attribute
{
}
