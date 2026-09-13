namespace System.Security.Policy;

public interface IIdentityPermissionFactory
{
	IPermission CreateIdentityPermission(Evidence evidence);
}
