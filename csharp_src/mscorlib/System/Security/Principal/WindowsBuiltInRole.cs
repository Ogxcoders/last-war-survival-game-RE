using System.Runtime.InteropServices;

namespace System.Security.Principal;

[Serializable]
[ComVisible(true)]
public enum WindowsBuiltInRole
{
	Administrator = 544,
	User,
	Guest,
	PowerUser,
	AccountOperator,
	SystemOperator,
	PrintOperator,
	BackupOperator,
	Replicator
}
