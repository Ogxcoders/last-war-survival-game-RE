namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class DefaultParameterValueAttribute : Attribute
{
	private object value;

	public object Value => value;

	public DefaultParameterValueAttribute(object value)
	{
		this.value = value;
	}
}
