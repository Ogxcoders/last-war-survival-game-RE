using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityUserRecord : IMessage<AllianceCityUserRecord>, IMessage, IEquatable<AllianceCityUserRecord>, IDeepCloneable<AllianceCityUserRecord>
{
	private static readonly MessageParser<AllianceCityUserRecord> _parser = new MessageParser<AllianceCityUserRecord>(() => new AllianceCityUserRecord());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int PointFieldNumber = 2;

	private long point_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityUserRecord> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long Point
	{
		get
		{
			return point_;
		}
		set
		{
			point_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceCityUserRecord()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityUserRecord(AllianceCityUserRecord other)
		: this()
	{
		uid_ = other.uid_;
		point_ = other.point_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityUserRecord Clone()
	{
		return new AllianceCityUserRecord(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityUserRecord);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityUserRecord other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (Point != other.Point)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (Point != 0L)
		{
			num ^= Point.GetHashCode();
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
		if (Uid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uid);
		}
		if (Point != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Point);
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
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (Point != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Point);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceCityUserRecord other)
	{
		if (other != null)
		{
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.Point != 0L)
			{
				Point = other.Point;
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
			case 10u:
				Uid = input.ReadString();
				break;
			case 16u:
				Point = input.ReadInt64();
				break;
			}
		}
	}
}
