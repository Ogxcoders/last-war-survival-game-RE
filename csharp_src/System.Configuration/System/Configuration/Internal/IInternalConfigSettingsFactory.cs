using System.Runtime.InteropServices;

namespace System.Configuration.Internal;

[ComVisible(false)]
public interface IInternalConfigSettingsFactory
{
	void CompleteInit();

	void SetConfigurationSystem(IInternalConfigSystem internalConfigSystem, bool initComplete);
}
