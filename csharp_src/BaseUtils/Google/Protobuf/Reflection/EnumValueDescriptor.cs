using System;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class EnumValueDescriptor : DescriptorBase
{
	private readonly EnumDescriptor enumDescriptor;

	private readonly EnumValueDescriptorProto proto;

	internal EnumValueDescriptorProto Proto => proto;

	public override string Name => proto.Name;

	public int Number => Proto.Number;

	public EnumDescriptor EnumDescriptor => enumDescriptor;

	[Obsolete("CustomOptions are obsolete. Use GetOption")]
	public CustomOptions CustomOptions => new CustomOptions(Proto.Options?._extensions?.ValuesByNumber);

	internal EnumValueDescriptor(EnumValueDescriptorProto proto, FileDescriptor file, EnumDescriptor parent, int index)
		: base(file, parent.FullName + "." + proto.Name, index)
	{
		this.proto = proto;
		enumDescriptor = parent;
		file.DescriptorPool.AddSymbol(this);
		file.DescriptorPool.AddEnumValueByNumber(this);
	}

	public T GetOption<T>(Extension<EnumValueOptions, T> extension)
	{
		T extension2 = Proto.Options.GetExtension(extension);
		if (!(extension2 is IDeepCloneable<T>))
		{
			return extension2;
		}
		return (extension2 as IDeepCloneable<T>).Clone();
	}

	public RepeatedField<T> GetOption<T>(RepeatedExtension<EnumValueOptions, T> extension)
	{
		return Proto.Options.GetExtension(extension).Clone();
	}
}
