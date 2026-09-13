using System;
using Google.Protobuf.Collections;

namespace Google.Protobuf;

internal sealed class RepeatedExtensionValue<T> : IExtensionValue, IEquatable<IExtensionValue>, IDeepCloneable<IExtensionValue>
{
	private RepeatedField<T> field;

	private readonly FieldCodec<T> codec;

	internal RepeatedExtensionValue(FieldCodec<T> codec)
	{
		this.codec = codec;
		field = new RepeatedField<T>();
	}

	public int CalculateSize()
	{
		return field.CalculateSize(codec);
	}

	public IExtensionValue Clone()
	{
		return new RepeatedExtensionValue<T>(codec)
		{
			field = field.Clone()
		};
	}

	public bool Equals(IExtensionValue other)
	{
		if (this == other)
		{
			return true;
		}
		if (other is RepeatedExtensionValue<T> && field.Equals((other as RepeatedExtensionValue<T>).field))
		{
			return codec.Equals((other as RepeatedExtensionValue<T>).codec);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (17 * 31 + field.GetHashCode()) * 31 + codec.GetHashCode();
	}

	public void MergeFrom(CodedInputStream input)
	{
		field.AddEntriesFrom(input, codec);
	}

	public void MergeFrom(IExtensionValue value)
	{
		if (value is RepeatedExtensionValue<T>)
		{
			field.Add((value as RepeatedExtensionValue<T>).field);
		}
	}

	public void WriteTo(CodedOutputStream output)
	{
		field.WriteTo(output, codec);
	}

	public RepeatedField<T> GetValue()
	{
		return field;
	}

	public bool IsInitialized()
	{
		for (int i = 0; i < field.Count; i++)
		{
			T val = field[i];
			if (!(val is IMessage))
			{
				break;
			}
			if (!(val as IMessage).IsInitialized())
			{
				return false;
			}
		}
		return true;
	}
}
