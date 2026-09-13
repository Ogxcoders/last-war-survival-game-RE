namespace System.ComponentModel.Design;

[Flags]
public enum SelectionTypes
{
	Auto = 1,
	[Obsolete("This value has been deprecated. Use SelectionTypes.Auto instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	Normal = 1,
	Replace = 2,
	[Obsolete("This value has been deprecated.  It is no longer supported. http://go.microsoft.com/fwlink/?linkid=14202")]
	MouseDown = 4,
	[Obsolete("This value has been deprecated.  It is no longer supported. http://go.microsoft.com/fwlink/?linkid=14202")]
	MouseUp = 8,
	[Obsolete("This value has been deprecated. Use SelectionTypes.Primary instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	Click = 0x10,
	Primary = 0x10,
	Toggle = 0x20,
	Add = 0x40,
	Remove = 0x80,
	[Obsolete("This value has been deprecated. Use Enum class methods to determine valid values, or use a type converter. http://go.microsoft.com/fwlink/?linkid=14202")]
	Valid = 0x1F
}
