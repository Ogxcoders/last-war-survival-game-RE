namespace System.Security.Permissions;

[Flags]
public enum RegistryPermissionAccess
{
	AllAccess = 7,
	Create = 4,
	NoAccess = 0,
	Read = 1,
	Write = 2
}
