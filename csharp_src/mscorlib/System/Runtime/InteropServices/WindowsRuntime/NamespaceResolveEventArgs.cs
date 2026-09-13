using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.InteropServices.WindowsRuntime;

[ComVisible(false)]
public class NamespaceResolveEventArgs : EventArgs
{
	public string NamespaceName { get; private set; }

	public Assembly RequestingAssembly { get; private set; }

	public Collection<Assembly> ResolvedAssemblies { get; private set; }

	public NamespaceResolveEventArgs(string namespaceName, Assembly requestingAssembly)
	{
		NamespaceName = namespaceName;
		RequestingAssembly = requestingAssembly;
		ResolvedAssemblies = new Collection<Assembly>();
	}
}
