using System;

namespace Google.Protobuf;

public sealed class RepeatedExtension<TTarget, TValue> : Extension where TTarget : IExtendableMessage<TTarget>
{
	private readonly FieldCodec<TValue> codec;

	internal override Type TargetType => typeof(TTarget);

	public RepeatedExtension(int fieldNumber, FieldCodec<TValue> codec)
		: base(fieldNumber)
	{
		this.codec = codec;
	}

	internal override IExtensionValue CreateValue()
	{
		return new RepeatedExtensionValue<TValue>(codec);
	}
}
