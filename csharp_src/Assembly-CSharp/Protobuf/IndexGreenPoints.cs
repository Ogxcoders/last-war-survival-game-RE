using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class IndexGreenPoints : IMessage<IndexGreenPoints>, IMessage, IEquatable<IndexGreenPoints>, IDeepCloneable<IndexGreenPoints>
{
	private static readonly MessageParser<IndexGreenPoints> _parser = new MessageParser<IndexGreenPoints>(() => new IndexGreenPoints());

	private UnknownFieldSet _unknownFields;

	public const int IndexFieldNumber = 1;

	private int index_;

	public const int PointsFieldNumber = 2;

	private static readonly FieldCodec<int> _repeated_points_codec = FieldCodec.ForInt32(18u);

	private readonly RepeatedField<int> points_ = new RepeatedField<int>();

	public const int TimeStampFieldNumber = 3;

	private long timeStamp_;

	[DebuggerNonUserCode]
	public static MessageParser<IndexGreenPoints> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => CityAreaGreenInfoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Index
	{
		get
		{
			return index_;
		}
		set
		{
			index_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> Points => points_;

	[DebuggerNonUserCode]
	public long TimeStamp
	{
		get
		{
			return timeStamp_;
		}
		set
		{
			timeStamp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public IndexGreenPoints()
	{
	}

	[DebuggerNonUserCode]
	public IndexGreenPoints(IndexGreenPoints other)
		: this()
	{
		index_ = other.index_;
		points_ = other.points_.Clone();
		timeStamp_ = other.timeStamp_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IndexGreenPoints Clone()
	{
		return new IndexGreenPoints(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IndexGreenPoints);
	}

	[DebuggerNonUserCode]
	public bool Equals(IndexGreenPoints other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (!points_.Equals(other.points_))
		{
			return false;
		}
		if (TimeStamp != other.TimeStamp)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		num ^= points_.GetHashCode();
		if (TimeStamp != 0L)
		{
			num ^= TimeStamp.GetHashCode();
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
		if (Index != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Index);
		}
		points_.WriteTo(output, _repeated_points_codec);
		if (TimeStamp != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(TimeStamp);
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
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		num += points_.CalculateSize(_repeated_points_codec);
		if (TimeStamp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TimeStamp);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IndexGreenPoints other)
	{
		if (other != null)
		{
			if (other.Index != 0)
			{
				Index = other.Index;
			}
			points_.Add(other.points_);
			if (other.TimeStamp != 0L)
			{
				TimeStamp = other.TimeStamp;
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
				Index = input.ReadInt32();
				break;
			case 16u:
			case 18u:
				points_.AddEntriesFrom(input, _repeated_points_codec);
				break;
			case 24u:
				TimeStamp = input.ReadInt64();
				break;
			}
		}
	}
}
