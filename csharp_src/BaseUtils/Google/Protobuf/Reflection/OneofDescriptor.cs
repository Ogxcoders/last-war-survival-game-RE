using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class OneofDescriptor : DescriptorBase
{
	private readonly OneofDescriptorProto proto;

	private MessageDescriptor containingType;

	private IList<FieldDescriptor> fields;

	private readonly OneofAccessor accessor;

	public override string Name => proto.Name;

	public MessageDescriptor ContainingType => containingType;

	public IList<FieldDescriptor> Fields => fields;

	public OneofAccessor Accessor => accessor;

	[Obsolete("CustomOptions are obsolete. Use GetOption")]
	public CustomOptions CustomOptions => new CustomOptions(proto.Options?._extensions?.ValuesByNumber);

	internal OneofDescriptor(OneofDescriptorProto proto, FileDescriptor file, MessageDescriptor parent, int index, string clrName)
		: base(file, file.ComputeFullName(parent, proto.Name), index)
	{
		this.proto = proto;
		containingType = parent;
		file.DescriptorPool.AddSymbol(this);
		accessor = CreateAccessor(clrName);
	}

	public T GetOption<T>(Extension<OneofOptions, T> extension)
	{
		T extension2 = proto.Options.GetExtension(extension);
		if (!(extension2 is IDeepCloneable<T>))
		{
			return extension2;
		}
		return (extension2 as IDeepCloneable<T>).Clone();
	}

	public RepeatedField<T> GetOption<T>(RepeatedExtension<OneofOptions, T> extension)
	{
		return proto.Options.GetExtension(extension).Clone();
	}

	internal void CrossLink()
	{
		List<FieldDescriptor> list = new List<FieldDescriptor>();
		foreach (FieldDescriptor item in ContainingType.Fields.InDeclarationOrder())
		{
			if (item.ContainingOneof == this)
			{
				list.Add(item);
			}
		}
		fields = new ReadOnlyCollection<FieldDescriptor>(list);
	}

	private OneofAccessor CreateAccessor(string clrName)
	{
		if (clrName == null)
		{
			return null;
		}
		PropertyInfo property = containingType.ClrType.GetProperty(clrName + "Case");
		if (property == null)
		{
			throw new DescriptorValidationException(this, $"Property {clrName}Case not found in {containingType.ClrType}");
		}
		MethodInfo method = containingType.ClrType.GetMethod("Clear" + clrName);
		if (method == null)
		{
			throw new DescriptorValidationException(this, $"Method Clear{clrName} not found in {containingType.ClrType}");
		}
		return new OneofAccessor(property, method, this);
	}
}
