using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CombineCombatUnit : IMessage<CombineCombatUnit>, IMessage, IEquatable<CombineCombatUnit>, IDeepCloneable<CombineCombatUnit>
{
	private static readonly MessageParser<CombineCombatUnit> _parser = new MessageParser<CombineCombatUnit>(() => new CombineCombatUnit());

	private UnknownFieldSet _unknownFields;

	public const int SimpleCombatUnitFieldNumber = 1;

	private SimpleCombatUnit simpleCombatUnit_;

	public const int MembersFieldNumber = 2;

	private static readonly FieldCodec<ArmyCombatUnit> _repeated_members_codec = FieldCodec.ForMessage(18u, ArmyCombatUnit.Parser);

	private readonly RepeatedField<ArmyCombatUnit> members_ = new RepeatedField<ArmyCombatUnit>();

	[DebuggerNonUserCode]
	public static MessageParser<CombineCombatUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[11];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public SimpleCombatUnit SimpleCombatUnit
	{
		get
		{
			return simpleCombatUnit_;
		}
		set
		{
			simpleCombatUnit_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ArmyCombatUnit> Members => members_;

	[DebuggerNonUserCode]
	public CombineCombatUnit()
	{
	}

	[DebuggerNonUserCode]
	public CombineCombatUnit(CombineCombatUnit other)
		: this()
	{
		simpleCombatUnit_ = ((other.simpleCombatUnit_ != null) ? other.simpleCombatUnit_.Clone() : null);
		members_ = other.members_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CombineCombatUnit Clone()
	{
		return new CombineCombatUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CombineCombatUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(CombineCombatUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(SimpleCombatUnit, other.SimpleCombatUnit))
		{
			return false;
		}
		if (!members_.Equals(other.members_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (simpleCombatUnit_ != null)
		{
			num ^= SimpleCombatUnit.GetHashCode();
		}
		num ^= members_.GetHashCode();
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
		if (simpleCombatUnit_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(SimpleCombatUnit);
		}
		members_.WriteTo(output, _repeated_members_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (simpleCombatUnit_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SimpleCombatUnit);
		}
		num += members_.CalculateSize(_repeated_members_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CombineCombatUnit other)
	{
		if (other == null)
		{
			return;
		}
		if (other.simpleCombatUnit_ != null)
		{
			if (simpleCombatUnit_ == null)
			{
				SimpleCombatUnit = new SimpleCombatUnit();
			}
			SimpleCombatUnit.MergeFrom(other.SimpleCombatUnit);
		}
		members_.Add(other.members_);
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				if (simpleCombatUnit_ == null)
				{
					SimpleCombatUnit = new SimpleCombatUnit();
				}
				input.ReadMessage(SimpleCombatUnit);
				break;
			case 18u:
				members_.AddEntriesFrom(input, _repeated_members_codec);
				break;
			}
		}
	}
}
