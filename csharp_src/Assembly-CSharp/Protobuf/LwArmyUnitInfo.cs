using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwArmyUnitInfo : IMessage<LwArmyUnitInfo>, IMessage, IEquatable<LwArmyUnitInfo>, IDeepCloneable<LwArmyUnitInfo>
{
	private static readonly MessageParser<LwArmyUnitInfo> _parser = new MessageParser<LwArmyUnitInfo>(() => new LwArmyUnitInfo());

	private UnknownFieldSet _unknownFields;

	public const int UnitsFieldNumber = 1;

	private static readonly FieldCodec<LwBattleUnit> _repeated_units_codec = FieldCodec.ForMessage(10u, LwBattleUnit.Parser);

	private readonly RepeatedField<LwBattleUnit> units_ = new RepeatedField<LwBattleUnit>();

	[DebuggerNonUserCode]
	public static MessageParser<LwArmyUnitInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<LwBattleUnit> Units => units_;

	[DebuggerNonUserCode]
	public LwArmyUnitInfo()
	{
	}

	[DebuggerNonUserCode]
	public LwArmyUnitInfo(LwArmyUnitInfo other)
		: this()
	{
		units_ = other.units_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwArmyUnitInfo Clone()
	{
		return new LwArmyUnitInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwArmyUnitInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwArmyUnitInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!units_.Equals(other.units_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= units_.GetHashCode();
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
		units_.WriteTo(output, _repeated_units_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += units_.CalculateSize(_repeated_units_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwArmyUnitInfo other)
	{
		if (other != null)
		{
			units_.Add(other.units_);
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
				units_.AddEntriesFrom(input, _repeated_units_codec);
			}
		}
	}
}
