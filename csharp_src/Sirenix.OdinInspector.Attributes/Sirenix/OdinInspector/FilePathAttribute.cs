using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class FilePathAttribute : Attribute
{
	public bool AbsolutePath;

	public string Extensions;

	public string ParentFolder;

	[Obsolete("Use RequireExistingPath instead.", true)]
	public bool RequireValidPath;

	public bool RequireExistingPath;

	public bool UseBackslashes;

	[Obsolete("Add a ReadOnly attribute to the property instead.", true)]
	public bool ReadOnly { get; set; }
}
