using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyResult : IMessage<ArmyResult>, IMessage, IEquatable<ArmyResult>, IDeepCloneable<ArmyResult>
{
	private static readonly MessageParser<ArmyResult> _parser = new MessageParser<ArmyResult>(() => new ArmyResult());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	public const int IsDefeatFieldNumber = 2;

	private bool isDefeat_;

	public const int BuildArmyResultFieldNumber = 3;

	private BuildArmyResult buildArmyResult_;

	public const int CityArmyResultFieldNumber = 4;

	private CityArmyResult cityArmyResult_;

	public const int SimpleArmyResultFieldNumber = 5;

	private ArmyResultBase simpleArmyResult_;

	public const int MonsterArmyResultFieldNumber = 6;

	private MonsterArmyResult monsterArmyResult_;

	public const int AllianceCityArmyResultFieldNumber = 7;

	private AllianceCityArmyResult allianceCityArmyResult_;

	public const int PveArmyResultFieldNumber = 8;

	private PveArmyResult pveArmyResult_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyResult> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[21];

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
	public bool IsDefeat
	{
		get
		{
			return isDefeat_;
		}
		set
		{
			isDefeat_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BuildArmyResult BuildArmyResult
	{
		get
		{
			return buildArmyResult_;
		}
		set
		{
			buildArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityArmyResult CityArmyResult
	{
		get
		{
			return cityArmyResult_;
		}
		set
		{
			cityArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyResultBase SimpleArmyResult
	{
		get
		{
			return simpleArmyResult_;
		}
		set
		{
			simpleArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterArmyResult MonsterArmyResult
	{
		get
		{
			return monsterArmyResult_;
		}
		set
		{
			monsterArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceCityArmyResult AllianceCityArmyResult
	{
		get
		{
			return allianceCityArmyResult_;
		}
		set
		{
			allianceCityArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public PveArmyResult PveArmyResult
	{
		get
		{
			return pveArmyResult_;
		}
		set
		{
			pveArmyResult_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyResult()
	{
	}

	[DebuggerNonUserCode]
	public ArmyResult(ArmyResult other)
		: this()
	{
		type_ = other.type_;
		isDefeat_ = other.isDefeat_;
		buildArmyResult_ = ((other.buildArmyResult_ != null) ? other.buildArmyResult_.Clone() : null);
		cityArmyResult_ = ((other.cityArmyResult_ != null) ? other.cityArmyResult_.Clone() : null);
		simpleArmyResult_ = ((other.simpleArmyResult_ != null) ? other.simpleArmyResult_.Clone() : null);
		monsterArmyResult_ = ((other.monsterArmyResult_ != null) ? other.monsterArmyResult_.Clone() : null);
		allianceCityArmyResult_ = ((other.allianceCityArmyResult_ != null) ? other.allianceCityArmyResult_.Clone() : null);
		pveArmyResult_ = ((other.pveArmyResult_ != null) ? other.pveArmyResult_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyResult Clone()
	{
		return new ArmyResult(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyResult);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyResult other)
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
		if (IsDefeat != other.IsDefeat)
		{
			return false;
		}
		if (!object.Equals(BuildArmyResult, other.BuildArmyResult))
		{
			return false;
		}
		if (!object.Equals(CityArmyResult, other.CityArmyResult))
		{
			return false;
		}
		if (!object.Equals(SimpleArmyResult, other.SimpleArmyResult))
		{
			return false;
		}
		if (!object.Equals(MonsterArmyResult, other.MonsterArmyResult))
		{
			return false;
		}
		if (!object.Equals(AllianceCityArmyResult, other.AllianceCityArmyResult))
		{
			return false;
		}
		if (!object.Equals(PveArmyResult, other.PveArmyResult))
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
		if (IsDefeat)
		{
			num ^= IsDefeat.GetHashCode();
		}
		if (buildArmyResult_ != null)
		{
			num ^= BuildArmyResult.GetHashCode();
		}
		if (cityArmyResult_ != null)
		{
			num ^= CityArmyResult.GetHashCode();
		}
		if (simpleArmyResult_ != null)
		{
			num ^= SimpleArmyResult.GetHashCode();
		}
		if (monsterArmyResult_ != null)
		{
			num ^= MonsterArmyResult.GetHashCode();
		}
		if (allianceCityArmyResult_ != null)
		{
			num ^= AllianceCityArmyResult.GetHashCode();
		}
		if (pveArmyResult_ != null)
		{
			num ^= PveArmyResult.GetHashCode();
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
		if (IsDefeat)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsDefeat);
		}
		if (buildArmyResult_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(BuildArmyResult);
		}
		if (cityArmyResult_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(CityArmyResult);
		}
		if (simpleArmyResult_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(SimpleArmyResult);
		}
		if (monsterArmyResult_ != null)
		{
			output.WriteRawTag(50);
			output.WriteMessage(MonsterArmyResult);
		}
		if (allianceCityArmyResult_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(AllianceCityArmyResult);
		}
		if (pveArmyResult_ != null)
		{
			output.WriteRawTag(66);
			output.WriteMessage(PveArmyResult);
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
		if (IsDefeat)
		{
			num += 2;
		}
		if (buildArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BuildArmyResult);
		}
		if (cityArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CityArmyResult);
		}
		if (simpleArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SimpleArmyResult);
		}
		if (monsterArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(MonsterArmyResult);
		}
		if (allianceCityArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceCityArmyResult);
		}
		if (pveArmyResult_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(PveArmyResult);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyResult other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.IsDefeat)
		{
			IsDefeat = other.IsDefeat;
		}
		if (other.buildArmyResult_ != null)
		{
			if (buildArmyResult_ == null)
			{
				BuildArmyResult = new BuildArmyResult();
			}
			BuildArmyResult.MergeFrom(other.BuildArmyResult);
		}
		if (other.cityArmyResult_ != null)
		{
			if (cityArmyResult_ == null)
			{
				CityArmyResult = new CityArmyResult();
			}
			CityArmyResult.MergeFrom(other.CityArmyResult);
		}
		if (other.simpleArmyResult_ != null)
		{
			if (simpleArmyResult_ == null)
			{
				SimpleArmyResult = new ArmyResultBase();
			}
			SimpleArmyResult.MergeFrom(other.SimpleArmyResult);
		}
		if (other.monsterArmyResult_ != null)
		{
			if (monsterArmyResult_ == null)
			{
				MonsterArmyResult = new MonsterArmyResult();
			}
			MonsterArmyResult.MergeFrom(other.MonsterArmyResult);
		}
		if (other.allianceCityArmyResult_ != null)
		{
			if (allianceCityArmyResult_ == null)
			{
				AllianceCityArmyResult = new AllianceCityArmyResult();
			}
			AllianceCityArmyResult.MergeFrom(other.AllianceCityArmyResult);
		}
		if (other.pveArmyResult_ != null)
		{
			if (pveArmyResult_ == null)
			{
				PveArmyResult = new PveArmyResult();
			}
			PveArmyResult.MergeFrom(other.PveArmyResult);
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
			case 16u:
				IsDefeat = input.ReadBool();
				break;
			case 26u:
				if (buildArmyResult_ == null)
				{
					BuildArmyResult = new BuildArmyResult();
				}
				input.ReadMessage(BuildArmyResult);
				break;
			case 34u:
				if (cityArmyResult_ == null)
				{
					CityArmyResult = new CityArmyResult();
				}
				input.ReadMessage(CityArmyResult);
				break;
			case 42u:
				if (simpleArmyResult_ == null)
				{
					SimpleArmyResult = new ArmyResultBase();
				}
				input.ReadMessage(SimpleArmyResult);
				break;
			case 50u:
				if (monsterArmyResult_ == null)
				{
					MonsterArmyResult = new MonsterArmyResult();
				}
				input.ReadMessage(MonsterArmyResult);
				break;
			case 58u:
				if (allianceCityArmyResult_ == null)
				{
					AllianceCityArmyResult = new AllianceCityArmyResult();
				}
				input.ReadMessage(AllianceCityArmyResult);
				break;
			case 66u:
				if (pveArmyResult_ == null)
				{
					PveArmyResult = new PveArmyResult();
				}
				input.ReadMessage(PveArmyResult);
				break;
			}
		}
	}
}
