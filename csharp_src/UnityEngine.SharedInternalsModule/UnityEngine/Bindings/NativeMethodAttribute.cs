using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
[VisibleToOtherModules]
internal class NativeMethodAttribute : Attribute, IBindingsNameProviderAttribute, IBindingsAttribute, IBindingsIsThreadSafeProviderAttribute, IBindingsIsFreeFunctionProviderAttribute, IBindingsThrowsProviderAttribute
{
	public string Name { get; set; }

	public bool IsThreadSafe { get; set; }

	public bool IsFreeFunction { get; set; }

	public bool ThrowsException { get; set; }

	public bool HasExplicitThis { get; set; }

	public bool WritableSelf { get; set; }

	public NativeMethodAttribute()
	{
	}

	public NativeMethodAttribute(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == "")
		{
			throw new ArgumentException("name cannot be empty", "name");
		}
		Name = name;
	}

	public NativeMethodAttribute(string name, bool isFreeFunction)
		: this(name)
	{
		IsFreeFunction = isFreeFunction;
	}

	public NativeMethodAttribute(string name, bool isFreeFunction, bool isThreadSafe)
		: this(name, isFreeFunction)
	{
		IsThreadSafe = isThreadSafe;
	}

	public NativeMethodAttribute(string name, bool isFreeFunction, bool isThreadSafe, bool throws)
		: this(name, isFreeFunction, isThreadSafe)
	{
		ThrowsException = throws;
	}
}
