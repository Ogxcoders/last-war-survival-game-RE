using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class ServiceDescriptor : DescriptorBase
{
	private readonly ServiceDescriptorProto proto;

	private readonly IList<MethodDescriptor> methods;

	public override string Name => proto.Name;

	internal ServiceDescriptorProto Proto => proto;

	public IList<MethodDescriptor> Methods => methods;

	[Obsolete("CustomOptions are obsolete. Use GetOption")]
	public CustomOptions CustomOptions => new CustomOptions(Proto.Options?._extensions?.ValuesByNumber);

	internal ServiceDescriptor(ServiceDescriptorProto proto, FileDescriptor file, int index)
		: base(file, file.ComputeFullName(null, proto.Name), index)
	{
		ServiceDescriptor parent = this;
		this.proto = proto;
		methods = DescriptorUtil.ConvertAndMakeReadOnly(proto.Method, (MethodDescriptorProto method, int i) => new MethodDescriptor(method, file, parent, i));
		file.DescriptorPool.AddSymbol(this);
	}

	internal override IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
	{
		if (fieldNumber == 2)
		{
			return (IReadOnlyList<DescriptorBase>)methods;
		}
		return null;
	}

	public MethodDescriptor FindMethodByName(string name)
	{
		return base.File.DescriptorPool.FindSymbol<MethodDescriptor>(base.FullName + "." + name);
	}

	public T GetOption<T>(Extension<ServiceOptions, T> extension)
	{
		T extension2 = Proto.Options.GetExtension(extension);
		if (!(extension2 is IDeepCloneable<T>))
		{
			return extension2;
		}
		return (extension2 as IDeepCloneable<T>).Clone();
	}

	public RepeatedField<T> GetOption<T>(RepeatedExtension<ServiceOptions, T> extension)
	{
		return Proto.Options.GetExtension(extension).Clone();
	}

	internal void CrossLink()
	{
		foreach (MethodDescriptor method in methods)
		{
			method.CrossLink();
		}
	}
}
