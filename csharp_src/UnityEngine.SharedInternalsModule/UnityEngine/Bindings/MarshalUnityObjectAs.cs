using System;

namespace UnityEngine.Bindings;

[VisibleToOtherModules]
[AttributeUsage(AttributeTargets.Class)]
internal class MarshalUnityObjectAs : Attribute, IBindingsAttribute
{
	public Type MarshalAsType { get; set; }

	public MarshalUnityObjectAs(Type marshalAsType)
	{
		MarshalAsType = marshalAsType;
	}
}
