using System.Collections.Generic;

namespace System.Runtime.InteropServices.WindowsRuntime;

[MonoTODO]
public static class WindowsRuntimeMetadata
{
	public static event EventHandler<DesignerNamespaceResolveEventArgs> DesignerNamespaceResolve;

	public static event EventHandler<NamespaceResolveEventArgs> ReflectionOnlyNamespaceResolve;

	public static IEnumerable<string> ResolveNamespace(string namespaceName, IEnumerable<string> packageGraphFilePaths)
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<string> ResolveNamespace(string namespaceName, string windowsSdkFilePath, IEnumerable<string> packageGraphFilePaths)
	{
		throw new NotImplementedException();
	}
}
