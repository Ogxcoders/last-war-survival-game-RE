using System.Collections.ObjectModel;

namespace System.Runtime.InteropServices.WindowsRuntime;

[ComVisible(false)]
public class DesignerNamespaceResolveEventArgs : EventArgs
{
	public string NamespaceName { get; private set; }

	public Collection<string> ResolvedAssemblyFiles { get; private set; }

	public DesignerNamespaceResolveEventArgs(string namespaceName)
	{
		NamespaceName = namespaceName;
		ResolvedAssemblyFiles = new Collection<string>();
	}
}
