using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf;

public static class FieldCodec
{
	private static class WrapperCodecs
	{
		private static readonly Dictionary<Type, object> Codecs = new Dictionary<Type, object>
		{
			{
				typeof(bool),
				ForBool(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
			},
			{
				typeof(int),
				ForInt32(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
			},
			{
				typeof(long),
				ForInt64(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
			},
			{
				typeof(uint),
				ForUInt32(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
			},
			{
				typeof(ulong),
				ForUInt64(WireFormat.MakeTag(1, WireFormat.WireType.Varint))
			},
			{
				typeof(float),
				ForFloat(WireFormat.MakeTag(1, WireFormat.WireType.Fixed32))
			},
			{
				typeof(double),
				ForDouble(WireFormat.MakeTag(1, WireFormat.WireType.Fixed64))
			},
			{
				typeof(string),
				ForString(WireFormat.MakeTag(1, WireFormat.WireType.LengthDelimited))
			},
			{
				typeof(ByteString),
				ForBytes(WireFormat.MakeTag(1, WireFormat.WireType.LengthDelimited))
			}
		};

		private static readonly Dictionary<Type, object> Readers = new Dictionary<Type, object>
		{
			{
				typeof(bool),
				new Func<CodedInputStream, bool?>(CodedInputStream.ReadBoolWrapper)
			},
			{
				typeof(int),
				new Func<CodedInputStream, int?>(CodedInputStream.ReadInt32Wrapper)
			},
			{
				typeof(long),
				new Func<CodedInputStream, long?>(CodedInputStream.ReadInt64Wrapper)
			},
			{
				typeof(uint),
				new Func<CodedInputStream, uint?>(CodedInputStream.ReadUInt32Wrapper)
			},
			{
				typeof(ulong),
				new Func<CodedInputStream, ulong?>(CodedInputStream.ReadUInt64Wrapper)
			},
			{
				typeof(float),
				BitConverter.IsLittleEndian ? new Func<CodedInputStream, float?>(CodedInputStream.ReadFloatWrapperLittleEndian) : new Func<CodedInputStream, float?>(CodedInputStream.ReadFloatWrapperSlow)
			},
			{
				typeof(double),
				BitConverter.IsLittleEndian ? new Func<CodedInputStream, double?>(CodedInputStream.ReadDoubleWrapperLittleEndian) : new Func<CodedInputStream, double?>(CodedInputStream.ReadDoubleWrapperSlow)
			},
			{
				typeof(string),
				null
			},
			{
				typeof(ByteString),
				null
			}
		};

		internal static FieldCodec<T> GetCodec<T>()
		{
			if (!Codecs.TryGetValue(typeof(T), out var value))
			{
				throw new InvalidOperationException("Invalid type argument requested for wrapper codec: " + typeof(T));
			}
			return (FieldCodec<T>)value;
		}

		internal static Func<CodedInputStream, T?> GetReader<T>() where T : struct
		{
			if (!Readers.TryGetValue(typeof(T), out var value))
			{
				throw new InvalidOperationException("Invalid type argument requested for wrapper reader: " + typeof(T));
			}
			if (value == null)
			{
				FieldCodec<T> nestedCoded = GetCodec<T>();
				return (CodedInputStream input) => Read(input, nestedCoded);
			}
			return (Func<CodedInputStream, T?>)value;
		}

		internal static T Read<T>(CodedInputStream input, FieldCodec<T> codec)
		{
			int byteLimit = input.ReadLength();
			int oldLimit = input.PushLimit(byteLimit);
			T result = codec.DefaultValue;
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				if (num == codec.Tag)
				{
					result = codec.Read(input);
				}
				else
				{
					input.SkipLastField();
				}
			}
			input.CheckReadEndOfStreamTag();
			input.PopLimit(oldLimit);
			return result;
		}

		internal static void Write<T>(CodedOutputStream output, T value, FieldCodec<T> codec)
		{
			output.WriteLength(codec.CalculateSizeWithTag(value));
			codec.WriteTagAndValue(output, value);
		}

		internal static int CalculateSize<T>(T value, FieldCodec<T> codec)
		{
			int num = codec.CalculateSizeWithTag(value);
			return CodedOutputStream.ComputeLengthSize(num) + num;
		}
	}

	public static FieldCodec<string> ForString(uint tag)
	{
		return ForString(tag, "");
	}

	public static FieldCodec<ByteString> ForBytes(uint tag)
	{
		return ForBytes(tag, ByteString.Empty);
	}

	public static FieldCodec<bool> ForBool(uint tag)
	{
		return ForBool(tag, defaultValue: false);
	}

	public static FieldCodec<int> ForInt32(uint tag)
	{
		return ForInt32(tag, 0);
	}

	public static FieldCodec<int> ForSInt32(uint tag)
	{
		return ForSInt32(tag, 0);
	}

	public static FieldCodec<uint> ForFixed32(uint tag)
	{
		return ForFixed32(tag, 0u);
	}

	public static FieldCodec<int> ForSFixed32(uint tag)
	{
		return ForSFixed32(tag, 0);
	}

	public static FieldCodec<uint> ForUInt32(uint tag)
	{
		return ForUInt32(tag, 0u);
	}

	public static FieldCodec<long> ForInt64(uint tag)
	{
		return ForInt64(tag, 0L);
	}

	public static FieldCodec<long> ForSInt64(uint tag)
	{
		return ForSInt64(tag, 0L);
	}

	public static FieldCodec<ulong> ForFixed64(uint tag)
	{
		return ForFixed64(tag, 0uL);
	}

	public static FieldCodec<long> ForSFixed64(uint tag)
	{
		return ForSFixed64(tag, 0L);
	}

	public static FieldCodec<ulong> ForUInt64(uint tag)
	{
		return ForUInt64(tag, 0uL);
	}

	public static FieldCodec<float> ForFloat(uint tag)
	{
		return ForFloat(tag, 0f);
	}

	public static FieldCodec<double> ForDouble(uint tag)
	{
		return ForDouble(tag, 0.0);
	}

	public static FieldCodec<T> ForEnum<T>(uint tag, Func<T, int> toInt32, Func<int, T> fromInt32)
	{
		return ForEnum(tag, toInt32, fromInt32, default(T));
	}

	public static FieldCodec<string> ForString(uint tag, string defaultValue)
	{
		return new FieldCodec<string>((CodedInputStream input) => input.ReadString(), delegate(CodedOutputStream output, string value)
		{
			output.WriteString(value);
		}, CodedOutputStream.ComputeStringSize, tag, defaultValue);
	}

	public static FieldCodec<ByteString> ForBytes(uint tag, ByteString defaultValue)
	{
		return new FieldCodec<ByteString>((CodedInputStream input) => input.ReadBytes(), delegate(CodedOutputStream output, ByteString value)
		{
			output.WriteBytes(value);
		}, CodedOutputStream.ComputeBytesSize, tag, defaultValue);
	}

	public static FieldCodec<bool> ForBool(uint tag, bool defaultValue)
	{
		return new FieldCodec<bool>((CodedInputStream input) => input.ReadBool(), delegate(CodedOutputStream output, bool value)
		{
			output.WriteBool(value);
		}, 1, tag, defaultValue);
	}

	public static FieldCodec<int> ForInt32(uint tag, int defaultValue)
	{
		return new FieldCodec<int>((CodedInputStream input) => input.ReadInt32(), delegate(CodedOutputStream output, int value)
		{
			output.WriteInt32(value);
		}, CodedOutputStream.ComputeInt32Size, tag, defaultValue);
	}

	public static FieldCodec<int> ForSInt32(uint tag, int defaultValue)
	{
		return new FieldCodec<int>((CodedInputStream input) => input.ReadSInt32(), delegate(CodedOutputStream output, int value)
		{
			output.WriteSInt32(value);
		}, CodedOutputStream.ComputeSInt32Size, tag, defaultValue);
	}

	public static FieldCodec<uint> ForFixed32(uint tag, uint defaultValue)
	{
		return new FieldCodec<uint>((CodedInputStream input) => input.ReadFixed32(), delegate(CodedOutputStream output, uint value)
		{
			output.WriteFixed32(value);
		}, 4, tag, defaultValue);
	}

	public static FieldCodec<int> ForSFixed32(uint tag, int defaultValue)
	{
		return new FieldCodec<int>((CodedInputStream input) => input.ReadSFixed32(), delegate(CodedOutputStream output, int value)
		{
			output.WriteSFixed32(value);
		}, 4, tag, defaultValue);
	}

	public static FieldCodec<uint> ForUInt32(uint tag, uint defaultValue)
	{
		return new FieldCodec<uint>((CodedInputStream input) => input.ReadUInt32(), delegate(CodedOutputStream output, uint value)
		{
			output.WriteUInt32(value);
		}, CodedOutputStream.ComputeUInt32Size, tag, defaultValue);
	}

	public static FieldCodec<long> ForInt64(uint tag, long defaultValue)
	{
		return new FieldCodec<long>((CodedInputStream input) => input.ReadInt64(), delegate(CodedOutputStream output, long value)
		{
			output.WriteInt64(value);
		}, CodedOutputStream.ComputeInt64Size, tag, defaultValue);
	}

	public static FieldCodec<long> ForSInt64(uint tag, long defaultValue)
	{
		return new FieldCodec<long>((CodedInputStream input) => input.ReadSInt64(), delegate(CodedOutputStream output, long value)
		{
			output.WriteSInt64(value);
		}, CodedOutputStream.ComputeSInt64Size, tag, defaultValue);
	}

	public static FieldCodec<ulong> ForFixed64(uint tag, ulong defaultValue)
	{
		return new FieldCodec<ulong>((CodedInputStream input) => input.ReadFixed64(), delegate(CodedOutputStream output, ulong value)
		{
			output.WriteFixed64(value);
		}, 8, tag, defaultValue);
	}

	public static FieldCodec<long> ForSFixed64(uint tag, long defaultValue)
	{
		return new FieldCodec<long>((CodedInputStream input) => input.ReadSFixed64(), delegate(CodedOutputStream output, long value)
		{
			output.WriteSFixed64(value);
		}, 8, tag, defaultValue);
	}

	public static FieldCodec<ulong> ForUInt64(uint tag, ulong defaultValue)
	{
		return new FieldCodec<ulong>((CodedInputStream input) => input.ReadUInt64(), delegate(CodedOutputStream output, ulong value)
		{
			output.WriteUInt64(value);
		}, CodedOutputStream.ComputeUInt64Size, tag, defaultValue);
	}

	public static FieldCodec<float> ForFloat(uint tag, float defaultValue)
	{
		return new FieldCodec<float>((CodedInputStream input) => input.ReadFloat(), delegate(CodedOutputStream output, float value)
		{
			output.WriteFloat(value);
		}, 4, tag, defaultValue);
	}

	public static FieldCodec<double> ForDouble(uint tag, double defaultValue)
	{
		return new FieldCodec<double>((CodedInputStream input) => input.ReadDouble(), delegate(CodedOutputStream output, double value)
		{
			output.WriteDouble(value);
		}, 8, tag, defaultValue);
	}

	public static FieldCodec<T> ForEnum<T>(uint tag, Func<T, int> toInt32, Func<int, T> fromInt32, T defaultValue)
	{
		return new FieldCodec<T>((CodedInputStream input) => fromInt32(input.ReadEnum()), delegate(CodedOutputStream output, T value)
		{
			output.WriteEnum(toInt32(value));
		}, (T value) => CodedOutputStream.ComputeEnumSize(toInt32(value)), tag, defaultValue);
	}

	public static FieldCodec<T> ForMessage<T>(uint tag, MessageParser<T> parser) where T : class, IMessage<T>
	{
		return new FieldCodec<T>(delegate(CodedInputStream input)
		{
			T val = parser.CreateTemplate();
			input.ReadMessage(val);
			return val;
		}, delegate(CodedOutputStream output, T value)
		{
			output.WriteMessage(value);
		}, delegate(CodedInputStream i, ref T v)
		{
			if (v == null)
			{
				v = parser.CreateTemplate();
			}
			i.ReadMessage(v);
		}, delegate(ref T v, T v2)
		{
			if (v2 == null)
			{
				return false;
			}
			if (v == null)
			{
				v = v2.Clone();
			}
			else
			{
				v.MergeFrom(v2);
			}
			return true;
		}, (T message) => CodedOutputStream.ComputeMessageSize(message), tag);
	}

	public static FieldCodec<T> ForGroup<T>(uint startTag, uint endTag, MessageParser<T> parser) where T : class, IMessage<T>
	{
		return new FieldCodec<T>(delegate(CodedInputStream input)
		{
			T val = parser.CreateTemplate();
			input.ReadGroup(val);
			return val;
		}, delegate(CodedOutputStream output, T value)
		{
			output.WriteGroup(value);
		}, delegate(CodedInputStream i, ref T v)
		{
			if (v == null)
			{
				v = parser.CreateTemplate();
			}
			i.ReadGroup(v);
		}, delegate(ref T v, T v2)
		{
			if (v2 == null)
			{
				return v == null;
			}
			if (v == null)
			{
				v = v2.Clone();
			}
			else
			{
				v.MergeFrom(v2);
			}
			return true;
		}, (T message) => CodedOutputStream.ComputeGroupSize(message), startTag, endTag);
	}

	public static FieldCodec<T> ForClassWrapper<T>(uint tag) where T : class
	{
		FieldCodec<T> nestedCodec = WrapperCodecs.GetCodec<T>();
		return new FieldCodec<T>((CodedInputStream input) => WrapperCodecs.Read(input, nestedCodec), delegate(CodedOutputStream output, T value)
		{
			WrapperCodecs.Write(output, value, nestedCodec);
		}, delegate(CodedInputStream i, ref T v)
		{
			v = WrapperCodecs.Read(i, nestedCodec);
		}, delegate(ref T v, T v2)
		{
			v = v2;
			return v == null;
		}, (T value) => WrapperCodecs.CalculateSize(value, nestedCodec), tag, 0u, null);
	}

	public static FieldCodec<T?> ForStructWrapper<T>(uint tag) where T : struct
	{
		FieldCodec<T> nestedCodec = WrapperCodecs.GetCodec<T>();
		return new FieldCodec<T?>(WrapperCodecs.GetReader<T>(), delegate(CodedOutputStream output, T? value)
		{
			WrapperCodecs.Write(output, value.Value, nestedCodec);
		}, delegate(CodedInputStream i, ref T? v)
		{
			v = WrapperCodecs.Read(i, nestedCodec);
		}, delegate(ref T? v, T? v2)
		{
			if (v2.HasValue)
			{
				v = v2;
			}
			return v.HasValue;
		}, (T? value) => value.HasValue ? WrapperCodecs.CalculateSize(value.Value, nestedCodec) : 0, tag, 0u, null);
	}
}
public sealed class FieldCodec<T>
{
	internal delegate void InputMerger(CodedInputStream input, ref T value);

	internal delegate bool ValuesMerger(ref T value, T other);

	private static readonly EqualityComparer<T> EqualityComparer;

	private static readonly T DefaultDefault;

	private static readonly bool TypeSupportsPacking;

	private readonly int tagSize;

	internal bool PackedRepeatedField { get; }

	internal Action<CodedOutputStream, T> ValueWriter { get; }

	internal Func<T, int> ValueSizeCalculator { get; }

	internal Func<CodedInputStream, T> ValueReader { get; }

	internal InputMerger ValueMerger { get; }

	internal ValuesMerger FieldMerger { get; }

	internal int FixedSize { get; }

	internal uint Tag { get; }

	internal uint EndTag { get; }

	internal T DefaultValue { get; }

	static FieldCodec()
	{
		EqualityComparer = ProtobufEqualityComparers.GetEqualityComparer<T>();
		TypeSupportsPacking = default(T) != null;
		if (typeof(T) == typeof(string))
		{
			DefaultDefault = (T)(object)"";
		}
		else if (typeof(T) == typeof(ByteString))
		{
			DefaultDefault = (T)(object)ByteString.Empty;
		}
	}

	internal static bool IsPackedRepeatedField(uint tag)
	{
		if (TypeSupportsPacking)
		{
			return WireFormat.GetTagWireType(tag) == WireFormat.WireType.LengthDelimited;
		}
		return false;
	}

	internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, int fixedSize, uint tag, T defaultValue)
		: this(reader, writer, (Func<T, int>)((T _) => fixedSize), tag, defaultValue)
	{
		FixedSize = fixedSize;
	}

	internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, Func<T, int> sizeCalculator, uint tag, T defaultValue)
		: this(reader, writer, (InputMerger)delegate(CodedInputStream i, ref T v)
		{
			v = reader(i);
		}, (ValuesMerger)delegate(ref T v, T v2)
		{
			v = v2;
			return true;
		}, sizeCalculator, tag, 0u, defaultValue)
	{
	}

	internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, InputMerger inputMerger, ValuesMerger valuesMerger, Func<T, int> sizeCalculator, uint tag, uint endTag = 0u)
		: this(reader, writer, inputMerger, valuesMerger, sizeCalculator, tag, endTag, DefaultDefault)
	{
	}

	internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, InputMerger inputMerger, ValuesMerger valuesMerger, Func<T, int> sizeCalculator, uint tag, uint endTag, T defaultValue)
	{
		ValueReader = reader;
		ValueWriter = writer;
		ValueMerger = inputMerger;
		FieldMerger = valuesMerger;
		ValueSizeCalculator = sizeCalculator;
		FixedSize = 0;
		Tag = tag;
		EndTag = endTag;
		DefaultValue = defaultValue;
		tagSize = CodedOutputStream.ComputeRawVarint32Size(tag);
		if (endTag != 0)
		{
			tagSize += CodedOutputStream.ComputeRawVarint32Size(endTag);
		}
		PackedRepeatedField = IsPackedRepeatedField(tag);
	}

	public void WriteTagAndValue(CodedOutputStream output, T value)
	{
		if (!IsDefault(value))
		{
			output.WriteTag(Tag);
			ValueWriter(output, value);
			if (EndTag != 0)
			{
				output.WriteTag(EndTag);
			}
		}
	}

	public T Read(CodedInputStream input)
	{
		return ValueReader(input);
	}

	public int CalculateSizeWithTag(T value)
	{
		if (!IsDefault(value))
		{
			return ValueSizeCalculator(value) + tagSize;
		}
		return 0;
	}

	private bool IsDefault(T value)
	{
		return EqualityComparer.Equals(value, DefaultValue);
	}
}
