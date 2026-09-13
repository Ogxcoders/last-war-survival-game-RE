namespace Microsoft.Win32;

public enum RegistryValueKind
{
	String = 1,
	ExpandString = 2,
	Binary = 3,
	DWord = 4,
	MultiString = 7,
	QWord = 11,
	Unknown = 0,
	None = -1
}
