using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyStatistics : IMessage<ArmyStatistics>, IMessage, IEquatable<ArmyStatistics>, IDeepCloneable<ArmyStatistics>
{
	private static readonly MessageParser<ArmyStatistics> _parser = new MessageParser<ArmyStatistics>(() => new ArmyStatistics());

	private UnknownFieldSet _unknownFields;

	public const int DatasFieldNumber = 1;

	private static readonly MapField<int, int>.Codec _map_datas_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForInt32(16u, 0), 10u);

	private readonly MapField<int, int> datas_ = new MapField<int, int>();

	[DebuggerNonUserCode]
	public static MessageParser<ArmyStatistics> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[31];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public MapField<int, int> Datas => datas_;

	[DebuggerNonUserCode]
	public ArmyStatistics()
	{
	}

	[DebuggerNonUserCode]
	public ArmyStatistics(ArmyStatistics other)
		: this()
	{
		datas_ = other.datas_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyStatistics Clone()
	{
		return new ArmyStatistics(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyStatistics);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyStatistics other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!Datas.Equals(other.Datas))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= Datas.GetHashCode();
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
		datas_.WriteTo(output, _map_datas_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += datas_.CalculateSize(_map_datas_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyStatistics other)
	{
		if (other != null)
		{
			datas_.Add(other.datas_);
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
				datas_.AddEntriesFrom(input, _map_datas_codec);
			}
		}
	}
}
