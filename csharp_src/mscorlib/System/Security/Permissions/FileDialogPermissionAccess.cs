namespace System.Security.Permissions;

[Flags]
public enum FileDialogPermissionAccess
{
	None = 0,
	Open = 1,
	OpenSave = 3,
	Save = 2
}
