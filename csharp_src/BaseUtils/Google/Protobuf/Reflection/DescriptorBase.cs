using System.Collections.Generic;

namespace Google.Protobuf.Reflection;

public abstract class DescriptorBase : IDescriptor
{
	public int Index { get; }

	public abstract string Name { get; }

	public string FullName { get; }

	public FileDescriptor File { get; }

	public DescriptorDeclaration Declaration => File.GetDeclaration(this);

	internal DescriptorBase(FileDescriptor file, string fullName, int index)
	{
		File = file;
		FullName = fullName;
		Index = index;
	}

	internal virtual IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
	{
		return null;
	}
}
