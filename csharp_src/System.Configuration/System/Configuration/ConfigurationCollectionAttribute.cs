using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class ConfigurationCollectionAttribute : Attribute
{
	public string AddItemName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public string ClearItemsName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ConfigurationElementCollectionType CollectionType
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(ConfigurationElementCollectionType);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public Type ItemType
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public string RemoveItemName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ConfigurationCollectionAttribute(Type itemType)
	{
	}
}
