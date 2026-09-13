using System.Diagnostics;

namespace System.Runtime.CompilerServices;

[Conditional("NOT_MONO")]
internal class DependencyReductionRootAttribute : Attribute
{
}
