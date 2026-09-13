using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BatchArmyUnit : IMessage<BatchArmyUnit>, IMessage, IEquatable<BatchArmyUnit>, IDeepCloneable<BatchArmyUnit>
{
	private static readonly MessageParser<BatchArmyUnit> _parser = new MessageParser<BatchArmyUnit>(() => new BatchArmyUnit());

	private UnknownFieldSet _unknownFields;

	public const int ArmyUnitsFieldNumber = 1;

	private static readonly FieldCodec<ArmyDBUnitInfo> _repeated_armyUnits_codec = FieldCodec.ForMessage(10u, ArmyDBUnitInfo.Parser);

	private readonly RepeatedField<ArmyDBUnitInfo> armyUnits_ = new RepeatedField<ArmyDBUnitInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<BatchArmyUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<ArmyDBUnitInfo> ArmyUnits => armyUnits_;

	[DebuggerNonUserCode]
	public BatchArmyUnit()
	{
	}

	[DebuggerNonUserCode]
	public BatchArmyUnit(BatchArmyUnit other)
		: this()
	{
		armyUnits_ = other.armyUnits_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BatchArmyUnit Clone()
	{
		return new BatchArmyUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BatchArmyUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(BatchArmyUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!armyUnits_.Equals(other.armyUnits_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= armyUnits_.GetHashCode();
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
		armyUnits_.WriteTo(output, _repeated_armyUnits_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += armyUnits_.CalculateSize(_repeated_armyUnits_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BatchArmyUnit other)
	{
		if (other != null)
		{
			armyUnits_.Add(other.armyUnits_);
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
				armyUnits_.AddEntriesFrom(input, _repeated_armyUnits_codec);
			}
		}
	}
}
