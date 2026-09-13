using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyCombatUnit : IMessage<ArmyCombatUnit>, IMessage, IEquatable<ArmyCombatUnit>, IDeepCloneable<ArmyCombatUnit>
{
	private static readonly MessageParser<ArmyCombatUnit> _parser = new MessageParser<ArmyCombatUnit>(() => new ArmyCombatUnit());

	private UnknownFieldSet _unknownFields;

	public const int SimpleCombatUnitFieldNumber = 1;

	private SimpleCombatUnit simpleCombatUnit_;

	public const int ArmyInfoFieldNumber = 2;

	private ArmyUnitInfo armyInfo_;

	public const int SpecialArmyTypeFieldNumber = 3;

	private int specialArmyType_;

	public const int FormationIndexFieldNumber = 4;

	private int formationIndex_;

	public const int InAllianceTerritoryFieldNumber = 5;

	private int inAllianceTerritory_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyCombatUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[10];

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
	public ArmyUnitInfo ArmyInfo
	{
		get
		{
			return armyInfo_;
		}
		set
		{
			armyInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SpecialArmyType
	{
		get
		{
			return specialArmyType_;
		}
		set
		{
			specialArmyType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FormationIndex
	{
		get
		{
			return formationIndex_;
		}
		set
		{
			formationIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int InAllianceTerritory
	{
		get
		{
			return inAllianceTerritory_;
		}
		set
		{
			inAllianceTerritory_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyCombatUnit()
	{
	}

	[DebuggerNonUserCode]
	public ArmyCombatUnit(ArmyCombatUnit other)
		: this()
	{
		simpleCombatUnit_ = ((other.simpleCombatUnit_ != null) ? other.simpleCombatUnit_.Clone() : null);
		armyInfo_ = ((other.armyInfo_ != null) ? other.armyInfo_.Clone() : null);
		specialArmyType_ = other.specialArmyType_;
		formationIndex_ = other.formationIndex_;
		inAllianceTerritory_ = other.inAllianceTerritory_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyCombatUnit Clone()
	{
		return new ArmyCombatUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyCombatUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyCombatUnit other)
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
		if (!object.Equals(ArmyInfo, other.ArmyInfo))
		{
			return false;
		}
		if (SpecialArmyType != other.SpecialArmyType)
		{
			return false;
		}
		if (FormationIndex != other.FormationIndex)
		{
			return false;
		}
		if (InAllianceTerritory != other.InAllianceTerritory)
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
		if (armyInfo_ != null)
		{
			num ^= ArmyInfo.GetHashCode();
		}
		if (SpecialArmyType != 0)
		{
			num ^= SpecialArmyType.GetHashCode();
		}
		if (FormationIndex != 0)
		{
			num ^= FormationIndex.GetHashCode();
		}
		if (InAllianceTerritory != 0)
		{
			num ^= InAllianceTerritory.GetHashCode();
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
		if (simpleCombatUnit_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(SimpleCombatUnit);
		}
		if (armyInfo_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(ArmyInfo);
		}
		if (SpecialArmyType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(SpecialArmyType);
		}
		if (FormationIndex != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(FormationIndex);
		}
		if (InAllianceTerritory != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(InAllianceTerritory);
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
		if (simpleCombatUnit_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SimpleCombatUnit);
		}
		if (armyInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ArmyInfo);
		}
		if (SpecialArmyType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SpecialArmyType);
		}
		if (FormationIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FormationIndex);
		}
		if (InAllianceTerritory != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(InAllianceTerritory);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyCombatUnit other)
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
		if (other.armyInfo_ != null)
		{
			if (armyInfo_ == null)
			{
				ArmyInfo = new ArmyUnitInfo();
			}
			ArmyInfo.MergeFrom(other.ArmyInfo);
		}
		if (other.SpecialArmyType != 0)
		{
			SpecialArmyType = other.SpecialArmyType;
		}
		if (other.FormationIndex != 0)
		{
			FormationIndex = other.FormationIndex;
		}
		if (other.InAllianceTerritory != 0)
		{
			InAllianceTerritory = other.InAllianceTerritory;
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
			case 10u:
				if (simpleCombatUnit_ == null)
				{
					SimpleCombatUnit = new SimpleCombatUnit();
				}
				input.ReadMessage(SimpleCombatUnit);
				break;
			case 18u:
				if (armyInfo_ == null)
				{
					ArmyInfo = new ArmyUnitInfo();
				}
				input.ReadMessage(ArmyInfo);
				break;
			case 24u:
				SpecialArmyType = input.ReadInt32();
				break;
			case 32u:
				FormationIndex = input.ReadInt32();
				break;
			case 40u:
				InAllianceTerritory = input.ReadInt32();
				break;
			}
		}
	}
}
