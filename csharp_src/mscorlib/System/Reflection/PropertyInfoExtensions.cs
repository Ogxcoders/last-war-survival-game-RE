namespace System.Reflection;

public static class PropertyInfoExtensions
{
	public static MethodInfo[] GetAccessors(PropertyInfo property)
	{
		Requires.NotNull(property, "property");
		return property.GetAccessors();
	}

	public static MethodInfo[] GetAccessors(PropertyInfo property, bool nonPublic)
	{
		Requires.NotNull(property, "property");
		return property.GetAccessors(nonPublic);
	}

	public static MethodInfo GetGetMethod(PropertyInfo property)
	{
		Requires.NotNull(property, "property");
		return property.GetGetMethod();
	}

	public static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic)
	{
		Requires.NotNull(property, "property");
		return property.GetGetMethod(nonPublic);
	}

	public static MethodInfo GetSetMethod(PropertyInfo property)
	{
		Requires.NotNull(property, "property");
		return property.GetSetMethod();
	}

	public static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic)
	{
		Requires.NotNull(property, "property");
		return property.GetSetMethod(nonPublic);
	}
}
