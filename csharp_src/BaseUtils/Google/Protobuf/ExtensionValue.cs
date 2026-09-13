using System;

namespace Google.Protobuf;

internal sealed class ExtensionValue<T> : IExtensionValue, IEquatable<IExtensionValue>, IDeepCloneable<IExtensionValue>
{
	private T field;

	private FieldCodec<T> codec;

	internal ExtensionValue(FieldCodec<T> codec)
	{
		this.codec = codec;
		field = codec.DefaultValue;
	}

	public int CalculateSize()
	{
		return codec.CalculateSizeWithTag(field);
	}

	public IExtensionValue Clone()
	{
		return new ExtensionValue<T>(codec)
		{
			field = ((field is IDeepCloneable<T>) ? (field as IDeepCloneable<T>).Clone() : field)
		};
	}

	public bool Equals(IExtensionValue other)
	{
		if (this == other)
		{
			return true;
		}
		if (other is ExtensionValue<T> && codec.Equals((other as ExtensionValue<T>).codec))
		{
			return object.Equals(field, (other as ExtensionValue<T>).field);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (17 * 31 + field.GetHashCode()) * 31 + codec.GetHashCode();
	}

	public void MergeFrom(CodedInputStream input)
	{
		codec.ValueMerger(input, ref field);
	}

	public void MergeFrom(IExtensionValue value)
	{
		if (value is ExtensionValue<T>)
		{
			ExtensionValue<T> extensionValue = value as ExtensionValue<T>;
			codec.FieldMerger(ref field, extensionValue.field);
		}
	}

	public void WriteTo(CodedOutputStream output)
	{
		output.WriteTag(codec.Tag);
		codec.ValueWriter(output, field);
		if (codec.EndTag != 0)
		{
			output.WriteTag(codec.EndTag);
		}
	}

	public T GetValue()
	{
		return field;
	}

	public void SetValue(T value)
	{
		field = value;
	}

	public bool IsInitialized()
	{
		if (field is IMessage)
		{
			return (field as IMessage).IsInitialized();
		}
		return true;
	}
}
