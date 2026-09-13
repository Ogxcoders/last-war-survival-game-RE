namespace System.Data.Common;

[Serializable]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DbProviderSpecificTypePropertyAttribute : Attribute
{
	public bool IsProviderSpecificTypeProperty { get; }

	public DbProviderSpecificTypePropertyAttribute(bool isProviderSpecificTypeProperty)
	{
		IsProviderSpecificTypeProperty = isProviderSpecificTypeProperty;
	}
}
