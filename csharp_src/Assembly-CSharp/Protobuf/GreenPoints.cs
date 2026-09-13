using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class GreenPoints : IMessage<GreenPoints>, IMessage, IEquatable<GreenPoints>, IDeepCloneable<GreenPoints>
{
	private static readonly MessageParser<GreenPoints> _parser = new MessageParser<GreenPoints>(() => new GreenPoints());

	private UnknownFieldSet _unknownFields;

	public const int PointsFieldNumber = 1;

	private static readonly FieldCodec<int> _repeated_points_codec = FieldCodec.ForInt32(10u);

	private readonly RepeatedField<int> points_ = new RepeatedField<int>();

	public const int TypeFieldNumber = 2;

	private int type_;

	[DebuggerNonUserCode]
	public static MessageParser<GreenPoints> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => CityAreaGreenInfoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<int> Points => points_;

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public GreenPoints()
	{
	}

	[DebuggerNonUserCode]
	public GreenPoints(GreenPoints other)
		: this()
	{
		points_ = other.points_.Clone();
		type_ = other.type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GreenPoints Clone()
	{
		return new GreenPoints(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GreenPoints);
	}

	[DebuggerNonUserCode]
	public bool Equals(GreenPoints other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!points_.Equals(other.points_))
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= points_.GetHashCode();
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		points_.WriteTo(output, _repeated_points_codec);
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += points_.CalculateSize(_repeated_points_codec);
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GreenPoints other)
	{
		if (other != null)
		{
			points_.Add(other.points_);
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
			case 10u:
				points_.AddEntriesFrom(input, _repeated_points_codec);
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			}
		}
	}
}
