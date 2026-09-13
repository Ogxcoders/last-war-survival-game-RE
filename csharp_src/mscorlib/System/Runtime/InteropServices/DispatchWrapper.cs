using System.Security;
using System.Security.Permissions;

namespace System.Runtime.InteropServices;

[Serializable]
[ComVisible(true)]
public sealed class DispatchWrapper
{
	private object m_WrappedObject;

	public object WrappedObject => m_WrappedObject;

	[SecuritySafeCritical]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public DispatchWrapper(object obj)
	{
		if (obj != null)
		{
			Marshal.Release(Marshal.GetIDispatchForObject(obj));
		}
		m_WrappedObject = obj;
	}
}
