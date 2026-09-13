using System;

namespace UnityEngine.Bindings;

[VisibleToOtherModules]
[AttributeUsage(AttributeTargets.Field)]
internal class IgnoreAttribute : Attribute, IBindingsAttribute
{
	public bool DoesNotContributeToSize { get; set; }
}
