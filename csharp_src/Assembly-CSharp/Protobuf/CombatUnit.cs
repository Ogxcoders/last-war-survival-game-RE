using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CombatUnit : IMessage<CombatUnit>, IMessage, IEquatable<CombatUnit>, IDeepCloneable<CombatUnit>
{
	private static readonly MessageParser<CombatUnit> _parser = new MessageParser<CombatUnit>(() => new CombatUnit());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	public const int ArmyCombatUnitFieldNumber = 2;

	private ArmyCombatUnit armyCombatUnit_;

	public const int CombineCombatUnitFieldNumber = 3;

	private CombineCombatUnit combineCombatUnit_;

	public const int SimpleCombatUnitFieldNumber = 4;

	private SimpleCombatUnit simpleCombatUnit_;

	[DebuggerNonUserCode]
	public static MessageParser<CombatUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public ArmyCombatUnit ArmyCombatUnit
	{
		get
		{
			return armyCombatUnit_;
		}
		set
		{
			armyCombatUnit_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CombineCombatUnit CombineCombatUnit
	{
		get
		{
			return combineCombatUnit_;
		}
		set
		{
			combineCombatUnit_ = value;
		}
	}

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
	public CombatUnit()
	{
	}

	[DebuggerNonUserCode]
	public CombatUnit(CombatUnit other)
		: this()
	{
		type_ = other.type_;
		armyCombatUnit_ = ((other.armyCombatUnit_ != null) ? other.armyCombatUnit_.Clone() : null);
		combineCombatUnit_ = ((other.combineCombatUnit_ != null) ? other.combineCombatUnit_.Clone() : null);
		simpleCombatUnit_ = ((other.simpleCombatUnit_ != null) ? other.simpleCombatUnit_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CombatUnit Clone()
	{
		return new CombatUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CombatUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(CombatUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (!object.Equals(ArmyCombatUnit, other.ArmyCombatUnit))
		{
			return false;
		}
		if (!object.Equals(CombineCombatUnit, other.CombineCombatUnit))
		{
			return false;
		}
		if (!object.Equals(SimpleCombatUnit, other.SimpleCombatUnit))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (armyCombatUnit_ != null)
		{
			num ^= ArmyCombatUnit.GetHashCode();
		}
		if (combineCombatUnit_ != null)
		{
			num ^= CombineCombatUnit.GetHashCode();
		}
		if (simpleCombatUnit_ != null)
		{
			num ^= SimpleCombatUnit.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Type);
		}
		if (armyCombatUnit_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(ArmyCombatUnit);
		}
		if (combineCombatUnit_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(CombineCombatUnit);
		}
		if (simpleCombatUnit_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(SimpleCombatUnit);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (armyCombatUnit_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ArmyCombatUnit);
		}
		if (combineCombatUnit_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CombineCombatUnit);
		}
		if (simpleCombatUnit_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SimpleCombatUnit);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CombatUnit other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.armyCombatUnit_ != null)
		{
			if (armyCombatUnit_ == null)
			{
				ArmyCombatUnit = new ArmyCombatUnit();
			}
			ArmyCombatUnit.MergeFrom(other.ArmyCombatUnit);
		}
		if (other.combineCombatUnit_ != null)
		{
			if (combineCombatUnit_ == null)
			{
				CombineCombatUnit = new CombineCombatUnit();
			}
			CombineCombatUnit.MergeFrom(other.CombineCombatUnit);
		}
		if (other.simpleCombatUnit_ != null)
		{
			if (simpleCombatUnit_ == null)
			{
				SimpleCombatUnit = new SimpleCombatUnit();
			}
			SimpleCombatUnit.MergeFrom(other.SimpleCombatUnit);
		}
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
			case 8u:
				Type = input.ReadInt32();
				break;
			case 18u:
				if (armyCombatUnit_ == null)
				{
					ArmyCombatUnit = new ArmyCombatUnit();
				}
				input.ReadMessage(ArmyCombatUnit);
				break;
			case 26u:
				if (combineCombatUnit_ == null)
				{
					CombineCombatUnit = new CombineCombatUnit();
				}
				input.ReadMessage(CombineCombatUnit);
				break;
			case 34u:
				if (simpleCombatUnit_ == null)
				{
					SimpleCombatUnit = new SimpleCombatUnit();
				}
				input.ReadMessage(SimpleCombatUnit);
				break;
			}
		}
	}
}
