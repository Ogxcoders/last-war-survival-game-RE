using System;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class MethodDescriptor : DescriptorBase
{
	private readonly MethodDescriptorProto proto;

	private readonly ServiceDescriptor service;

	private MessageDescriptor inputType;

	private MessageDescriptor outputType;

	public ServiceDescriptor Service => service;

	public MessageDescriptor InputType => inputType;

	public MessageDescriptor OutputType => outputType;

	public bool IsClientStreaming => proto.ClientStreaming;

	public bool IsServerStreaming => proto.ServerStreaming;

	[Obsolete("CustomOptions are obsolete. Use GetOption")]
	public CustomOptions CustomOptions => new CustomOptions(Proto.Options?._extensions?.ValuesByNumber);

	internal MethodDescriptorProto Proto => proto;

	public override string Name => proto.Name;

	public T GetOption<T>(Extension<MethodOptions, T> extension)
	{
		T extension2 = Proto.Options.GetExtension(extension);
		if (!(extension2 is IDeepCloneable<T>))
		{
			return extension2;
		}
		return (extension2 as IDeepCloneable<T>).Clone();
	}

	public RepeatedField<T> GetOption<T>(RepeatedExtension<MethodOptions, T> extension)
	{
		return Proto.Options.GetExtension(extension).Clone();
	}

	internal MethodDescriptor(MethodDescriptorProto proto, FileDescriptor file, ServiceDescriptor parent, int index)
		: base(file, parent.FullName + "." + proto.Name, index)
	{
		this.proto = proto;
		service = parent;
		file.DescriptorPool.AddSymbol(this);
	}

	internal void CrossLink()
	{
		IDescriptor descriptor = base.File.DescriptorPool.LookupSymbol(Proto.InputType, this);
		if (!(descriptor is MessageDescriptor))
		{
			throw new DescriptorValidationException(this, "\"" + Proto.InputType + "\" is not a message type.");
		}
		inputType = (MessageDescriptor)descriptor;
		descriptor = base.File.DescriptorPool.LookupSymbol(Proto.OutputType, this);
		if (!(descriptor is MessageDescriptor))
		{
			throw new DescriptorValidationException(this, "\"" + Proto.OutputType + "\" is not a message type.");
		}
		outputType = (MessageDescriptor)descriptor;
	}
}
