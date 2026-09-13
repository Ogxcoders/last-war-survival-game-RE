namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
public sealed class KnownTypeAttribute : Attribute
{
	private string methodName;

	private Type type;

	public string MethodName => methodName;

	public Type Type => type;

	private KnownTypeAttribute()
	{
	}

	public KnownTypeAttribute(Type type)
	{
		this.type = type;
	}

	public KnownTypeAttribute(string methodName)
	{
		this.methodName = methodName;
	}
}
