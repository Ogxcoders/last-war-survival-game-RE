namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ProvidePropertyAttribute : Attribute
{
	public string PropertyName { get; }

	public string ReceiverTypeName { get; }

	public override object TypeId => GetType().FullName + PropertyName;

	public ProvidePropertyAttribute(string propertyName, Type receiverType)
	{
		PropertyName = propertyName;
		ReceiverTypeName = receiverType.AssemblyQualifiedName;
	}

	public ProvidePropertyAttribute(string propertyName, string receiverTypeName)
	{
		PropertyName = propertyName;
		ReceiverTypeName = receiverTypeName;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is ProvidePropertyAttribute providePropertyAttribute && providePropertyAttribute.PropertyName == PropertyName)
		{
			return providePropertyAttribute.ReceiverTypeName == ReceiverTypeName;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return PropertyName.GetHashCode() ^ ReceiverTypeName.GetHashCode();
	}
}
