using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class FolderPathAttribute : Attribute
{
	public bool AbsolutePath;

	public string ParentFolder;

	[Obsolete("Use RequireExistingPath instead.", true)]
	public bool RequireValidPath;

	public bool RequireExistingPath;

	public bool UseBackslashes;
}
