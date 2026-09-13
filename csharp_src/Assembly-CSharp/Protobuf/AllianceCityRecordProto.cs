using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityRecordProto : IMessage<AllianceCityRecordProto>, IMessage, IEquatable<AllianceCityRecordProto>, IDeepCloneable<AllianceCityRecordProto>
{
	private static readonly MessageParser<AllianceCityRecordProto> _parser = new MessageParser<AllianceCityRecordProto>(() => new AllianceCityRecordProto());

	private UnknownFieldSet _unknownFields;

	public const int KillUserRecordsFieldNumber = 1;

	private static readonly FieldCodec<AllianceCityUserRecord> _repeated_killUserRecords_codec = FieldCodec.ForMessage(10u, AllianceCityUserRecord.Parser);

	private readonly RepeatedField<AllianceCityUserRecord> killUserRecords_ = new RepeatedField<AllianceCityUserRecord>();

	public const int DestroyUserRecordsFieldNumber = 2;

	private static readonly FieldCodec<AllianceCityUserRecord> _repeated_destroyUserRecords_codec = FieldCodec.ForMessage(18u, AllianceCityUserRecord.Parser);

	private readonly RepeatedField<AllianceCityUserRecord> destroyUserRecords_ = new RepeatedField<AllianceCityUserRecord>();

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityRecordProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<AllianceCityUserRecord> KillUserRecords => killUserRecords_;

	[DebuggerNonUserCode]
	public RepeatedField<AllianceCityUserRecord> DestroyUserRecords => destroyUserRecords_;

	[DebuggerNonUserCode]
	public AllianceCityRecordProto()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityRecordProto(AllianceCityRecordProto other)
		: this()
	{
		killUserRecords_ = other.killUserRecords_.Clone();
		destroyUserRecords_ = other.destroyUserRecords_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityRecordProto Clone()
	{
		return new AllianceCityRecordProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityRecordProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityRecordProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!killUserRecords_.Equals(other.killUserRecords_))
		{
			return false;
		}
		if (!destroyUserRecords_.Equals(other.destroyUserRecords_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= killUserRecords_.GetHashCode();
		num ^= destroyUserRecords_.GetHashCode();
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
		killUserRecords_.WriteTo(output, _repeated_killUserRecords_codec);
		destroyUserRecords_.WriteTo(output, _repeated_destroyUserRecords_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += killUserRecords_.CalculateSize(_repeated_killUserRecords_codec);
		num += destroyUserRecords_.CalculateSize(_repeated_destroyUserRecords_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceCityRecordProto other)
	{
		if (other != null)
		{
			killUserRecords_.Add(other.killUserRecords_);
			destroyUserRecords_.Add(other.destroyUserRecords_);
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
				killUserRecords_.AddEntriesFrom(input, _repeated_killUserRecords_codec);
				break;
			case 18u:
				destroyUserRecords_.AddEntriesFrom(input, _repeated_destroyUserRecords_codec);
				break;
			}
		}
	}
}
