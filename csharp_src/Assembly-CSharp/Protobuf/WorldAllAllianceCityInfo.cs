using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldAllAllianceCityInfo : IMessage<WorldAllAllianceCityInfo>, IMessage, IEquatable<WorldAllAllianceCityInfo>, IDeepCloneable<WorldAllAllianceCityInfo>
{
	private static readonly MessageParser<WorldAllAllianceCityInfo> _parser = new MessageParser<WorldAllAllianceCityInfo>(() => new WorldAllAllianceCityInfo());

	private UnknownFieldSet _unknownFields;

	public const int InfoesFieldNumber = 1;

	private static readonly FieldCodec<AllianceCityOccupyInfo> _repeated_infoes_codec = FieldCodec.ForMessage(10u, AllianceCityOccupyInfo.Parser);

	private readonly RepeatedField<AllianceCityOccupyInfo> infoes_ = new RepeatedField<AllianceCityOccupyInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<WorldAllAllianceCityInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<AllianceCityOccupyInfo> Infoes => infoes_;

	[DebuggerNonUserCode]
	public WorldAllAllianceCityInfo()
	{
	}

	[DebuggerNonUserCode]
	public WorldAllAllianceCityInfo(WorldAllAllianceCityInfo other)
		: this()
	{
		infoes_ = other.infoes_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldAllAllianceCityInfo Clone()
	{
		return new WorldAllAllianceCityInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldAllAllianceCityInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldAllAllianceCityInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!infoes_.Equals(other.infoes_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= infoes_.GetHashCode();
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
		infoes_.WriteTo(output, _repeated_infoes_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += infoes_.CalculateSize(_repeated_infoes_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldAllAllianceCityInfo other)
	{
		if (other != null)
		{
			infoes_.Add(other.infoes_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				infoes_.AddEntriesFrom(input, _repeated_infoes_codec);
			}
		}
	}
}
