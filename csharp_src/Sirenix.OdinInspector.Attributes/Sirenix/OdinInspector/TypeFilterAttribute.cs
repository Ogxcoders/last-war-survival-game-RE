using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class TypeFilterAttribute : Attribute
{
	public string FilterGetter;

	public string DropdownTitle;

	public bool DrawValueNormally;

	[Obsolete("Use the FilterGetter member instead.", false)]
	public string MemberName
	{
		get
		{
			return FilterGetter;
		}
		set
		{
			FilterGetter = value;
		}
	}

	public TypeFilterAttribute(string filterGetter)
	{
		FilterGetter = filterGetter;
	}
}
