using System.Runtime.InteropServices;

namespace System.Configuration.Internal;

[ComVisible(false)]
public interface IConfigurationManagerHelper
{
	void EnsureNetConfigLoaded();
}
