using System.Security;
using System.Security.Permissions;
using Unity;

namespace System.Configuration;

[Serializable]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class ConfigurationPermissionAttribute : CodeAccessSecurityAttribute
{
	public ConfigurationPermissionAttribute(SecurityAction action)
	{
	}

	public override IPermission CreatePermission()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}
}
