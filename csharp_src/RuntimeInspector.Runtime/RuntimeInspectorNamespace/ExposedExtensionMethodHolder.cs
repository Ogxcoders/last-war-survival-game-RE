using System;
using System.Reflection;

namespace RuntimeInspectorNamespace;

public struct ExposedExtensionMethodHolder
{
	public readonly Type extendedType;

	public readonly MethodInfo method;

	public readonly RuntimeInspectorButtonAttribute properties;

	public ExposedExtensionMethodHolder(Type extendedType, MethodInfo method, RuntimeInspectorButtonAttribute properties)
	{
		this.extendedType = extendedType;
		this.method = method;
		this.properties = properties;
	}
}
