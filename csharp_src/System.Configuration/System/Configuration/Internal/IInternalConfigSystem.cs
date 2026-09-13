using System.Runtime.InteropServices;

namespace System.Configuration.Internal;

[ComVisible(false)]
public interface IInternalConfigSystem
{
	bool SupportsUserConfig { get; }

	object GetSection(string configKey);

	void RefreshConfig(string sectionName);
}
