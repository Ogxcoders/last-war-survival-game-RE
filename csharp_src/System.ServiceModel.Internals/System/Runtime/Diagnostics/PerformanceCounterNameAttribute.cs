namespace System.Runtime.Diagnostics;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
internal sealed class PerformanceCounterNameAttribute : Attribute
{
	public string Name { get; set; }

	public PerformanceCounterNameAttribute(string name)
	{
		Name = name;
	}
}
