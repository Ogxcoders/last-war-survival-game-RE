namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property)]
public class AttributeProviderAttribute : Attribute
{
	public string TypeName { get; }

	public string PropertyName { get; }

	public AttributeProviderAttribute(string typeName)
	{
		if (typeName == null)
		{
			throw new ArgumentNullException("typeName");
		}
		TypeName = typeName;
	}

	public AttributeProviderAttribute(string typeName, string propertyName)
	{
		if (typeName == null)
		{
			throw new ArgumentNullException("typeName");
		}
		if (propertyName == null)
		{
			throw new ArgumentNullException("propertyName");
		}
		TypeName = typeName;
		PropertyName = propertyName;
	}

	public AttributeProviderAttribute(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		TypeName = type.AssemblyQualifiedName;
	}
}
