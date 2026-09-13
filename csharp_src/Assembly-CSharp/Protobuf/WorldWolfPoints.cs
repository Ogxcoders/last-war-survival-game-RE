using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldWolfPoints : IMessage<WorldWolfPoints>, IMessage, IEquatable<WorldWolfPoints>, IDeepCloneable<WorldWolfPoints>
{
	private static readonly MessageParser<WorldWolfPoints> _parser = new MessageParser<WorldWolfPoints>(() => new WorldWolfPoints());

	private UnknownFieldSet _unknownFields;

	public const int PointIdFieldNumber = 1;

	private static readonly FieldCodec<int> _repeated_pointId_codec = FieldCodec.ForInt32(10u);

	private readonly RepeatedField<int> pointId_ = new RepeatedField<int>();

	public const int AllianceIdFieldNumber = 2;

	private string allianceId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<WorldWolfPoints> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WolfHunterMessageReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<int> PointId => pointId_;

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public WorldWolfPoints()
	{
	}

	[DebuggerNonUserCode]
	public WorldWolfPoints(WorldWolfPoints other)
		: this()
	{
		pointId_ = other.pointId_.Clone();
		allianceId_ = other.allianceId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldWolfPoints Clone()
	{
		return new WorldWolfPoints(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldWolfPoints);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldWolfPoints other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!pointId_.Equals(other.pointId_))
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= pointId_.GetHashCode();
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
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
		pointId_.WriteTo(output, _repeated_pointId_codec);
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(AllianceId);
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
		num += pointId_.CalculateSize(_repeated_pointId_codec);
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldWolfPoints other)
	{
		if (other != null)
		{
			pointId_.Add(other.pointId_);
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
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
				pointId_.AddEntriesFrom(input, _repeated_pointId_codec);
				break;
			case 18u:
				AllianceId = input.ReadString();
				break;
			}
		}
	}
}
