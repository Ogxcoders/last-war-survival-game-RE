namespace Joker;

public class AssemblyModuleAttribute : ClassAttribute
{
	public string Name { get; internal set; }

	public AssemblyModuleAttribute()
	{
	}

	public AssemblyModuleAttribute(string name)
	{
		Name = name;
	}
}
