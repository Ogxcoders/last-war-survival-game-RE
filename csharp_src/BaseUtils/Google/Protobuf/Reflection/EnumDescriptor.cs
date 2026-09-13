using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class EnumDescriptor : DescriptorBase
{
	private readonly EnumDescriptorProto proto;

	private readonly MessageDescriptor containingType;

	private readonly IList<EnumValueDescriptor> values;

	private readonly Type clrType;

	internal EnumDescriptorProto Proto => proto;

	public override string Name => proto.Name;

	public Type ClrType => clrType;

	public MessageDescriptor ContainingType => containingType;

	public IList<EnumValueDescriptor> Values => values;

	[Obsolete("CustomOptions are obsolete. Use GetOption")]
	public CustomOptions CustomOptions => new CustomOptions(Proto.Options?._extensions?.ValuesByNumber);

	internal EnumDescriptor(EnumDescriptorProto proto, FileDescriptor file, MessageDescriptor parent, int index, Type clrType)
		: base(file, file.ComputeFullName(parent, proto.Name), index)
	{
		EnumDescriptor parent2 = this;
		this.proto = proto;
		this.clrType = clrType;
		containingType = parent;
		if (proto.Value.Count == 0)
		{
			throw new DescriptorValidationException(this, "Enums must contain at least one value.");
		}
		values = DescriptorUtil.ConvertAndMakeReadOnly(proto.Value, (EnumValueDescriptorProto value, int i) => new EnumValueDescriptor(value, file, parent2, i));
		base.File.DescriptorPool.AddSymbol(this);
	}

	internal override IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
	{
		if (fieldNumber == 2)
		{
			return (IReadOnlyList<DescriptorBase>)Values;
		}
		return null;
	}

	public EnumValueDescriptor FindValueByNumber(int number)
	{
		return base.File.DescriptorPool.FindEnumValueByNumber(this, number);
	}

	public EnumValueDescriptor FindValueByName(string name)
	{
		return base.File.DescriptorPool.FindSymbol<EnumValueDescriptor>(base.FullName + "." + name);
	}

	public T GetOption<T>(Extension<EnumOptions, T> extension)
	{
		T extension2 = Proto.Options.GetExtension(extension);
		if (!(extension2 is IDeepCloneable<T>))
		{
			return extension2;
		}
		return (extension2 as IDeepCloneable<T>).Clone();
	}

	public RepeatedField<T> GetOption<T>(RepeatedExtension<EnumOptions, T> extension)
	{
		return Proto.Options.GetExtension(extension).Clone();
	}
}
