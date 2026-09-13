using System;

namespace Google.Protobuf;

public abstract class Extension
{
	internal abstract Type TargetType { get; }

	public int FieldNumber { get; }

	protected Extension(int fieldNumber)
	{
		FieldNumber = fieldNumber;
	}

	internal abstract IExtensionValue CreateValue();
}
public sealed class Extension<TTarget, TValue> : Extension where TTarget : IExtendableMessage<TTarget>
{
	private readonly FieldCodec<TValue> codec;

	internal TValue DefaultValue => codec.DefaultValue;

	internal override Type TargetType => typeof(TTarget);

	public Extension(int fieldNumber, FieldCodec<TValue> codec)
		: base(fieldNumber)
	{
		this.codec = codec;
	}

	internal override IExtensionValue CreateValue()
	{
		return new ExtensionValue<TValue>(codec);
	}
}
