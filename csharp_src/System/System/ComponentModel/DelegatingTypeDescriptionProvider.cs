using System.Collections;

namespace System.ComponentModel;

internal sealed class DelegatingTypeDescriptionProvider : TypeDescriptionProvider
{
	private readonly Type _type;

	internal TypeDescriptionProvider Provider => TypeDescriptor.GetProviderRecursive(_type);

	internal DelegatingTypeDescriptionProvider(Type type)
	{
		_type = type;
	}

	public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
	{
		return Provider.CreateInstance(provider, objectType, argTypes, args);
	}

	public override IDictionary GetCache(object instance)
	{
		return Provider.GetCache(instance);
	}

	public override string GetFullComponentName(object component)
	{
		return Provider.GetFullComponentName(component);
	}

	public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
	{
		return Provider.GetExtendedTypeDescriptor(instance);
	}

	protected internal override IExtenderProvider[] GetExtenderProviders(object instance)
	{
		return Provider.GetExtenderProviders(instance);
	}

	public override Type GetReflectionType(Type objectType, object instance)
	{
		return Provider.GetReflectionType(objectType, instance);
	}

	public override Type GetRuntimeType(Type objectType)
	{
		return Provider.GetRuntimeType(objectType);
	}

	public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
	{
		return Provider.GetTypeDescriptor(objectType, instance);
	}

	public override bool IsSupportedType(Type type)
	{
		return Provider.IsSupportedType(type);
	}
}
