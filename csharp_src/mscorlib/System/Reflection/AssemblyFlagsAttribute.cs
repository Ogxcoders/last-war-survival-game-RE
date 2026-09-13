namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyFlagsAttribute : Attribute
{
	private AssemblyNameFlags _flags;

	[Obsolete("This property has been deprecated. Please use AssemblyFlags instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	[CLSCompliant(false)]
	public uint Flags => (uint)_flags;

	public int AssemblyFlags => (int)_flags;

	[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	[CLSCompliant(false)]
	public AssemblyFlagsAttribute(uint flags)
	{
		_flags = (AssemblyNameFlags)flags;
	}

	[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	public AssemblyFlagsAttribute(int assemblyFlags)
	{
		_flags = (AssemblyNameFlags)assemblyFlags;
	}

	public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags)
	{
		_flags = assemblyFlags;
	}
}
