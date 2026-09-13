using System.IO;
using System.Reflection;
using System.Security;

namespace System.Globalization;

internal sealed class GlobalizationAssembly
{
	[SecurityCritical]
	internal unsafe static byte* GetGlobalizationResourceBytePtr(Assembly assembly, string tableName)
	{
		if (assembly.GetManifestResourceStream(tableName) is UnmanagedMemoryStream { PositionPointer: var positionPointer } && positionPointer != null)
		{
			return positionPointer;
		}
		throw new InvalidOperationException();
	}
}
