namespace Microsoft.Win32.SafeHandles;

public sealed class SafeNCryptProviderHandle : SafeNCryptHandle
{
	protected override bool ReleaseNativeHandle()
	{
		return false;
	}
}
