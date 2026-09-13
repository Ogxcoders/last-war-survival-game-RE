using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityArmyResult : IMessage<AllianceCityArmyResult>, IMessage, IEquatable<AllianceCityArmyResult>, IDeepCloneable<AllianceCityArmyResult>
{
	private static readonly MessageParser<AllianceCityArmyResult> _parser = new MessageParser<AllianceCityArmyResult>(() => new AllianceCityArmyResult());

	private UnknownFieldSet _unknownFields;

	public const int BaseFieldNumber = 1;

	private ArmyResultBase base_;

	public const int CityIdFieldNumber = 2;

	private int cityId_;

	public const int HealthFieldNumber = 3;

	private int health_;

	public const int InitHealthFieldNumber = 4;

	private int initHealth_;

	public const int AfterHealthFieldNumber = 5;

	private int afterHealth_;

	public const int CityNameFieldNumber = 6;

	private string cityName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityArmyResult> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public ArmyResultBase Base
	{
		get
		{
			return base_;
		}
		set
		{
			base_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CityId
	{
		get
		{
			return cityId_;
		}
		set
		{
			cityId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Health
	{
		get
		{
			return health_;
		}
		set
		{
			health_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int InitHealth
	{
		get
		{
			return initHealth_;
		}
		set
		{
			initHealth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AfterHealth
	{
		get
		{
			return afterHealth_;
		}
		set
		{
			afterHealth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CityName
	{
		get
		{
			return cityName_;
		}
		set
		{
			cityName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public AllianceCityArmyResult()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityArmyResult(AllianceCityArmyResult other)
		: this()
	{
		base_ = ((other.base_ != null) ? other.base_.Clone() : null);
		cityId_ = other.cityId_;
		health_ = other.health_;
		initHealth_ = other.initHealth_;
		afterHealth_ = other.afterHealth_;
		cityName_ = other.cityName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityArmyResult Clone()
	{
		return new AllianceCityArmyResult(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityArmyResult);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityArmyResult other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Base, other.Base))
		{
			return false;
		}
		if (CityId != other.CityId)
		{
			return false;
		}
		if (Health != other.Health)
		{
			return false;
		}
		if (InitHealth != other.InitHealth)
		{
			return false;
		}
		if (AfterHealth != other.AfterHealth)
		{
			return false;
		}
		if (CityName != other.CityName)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (base_ != null)
		{
			num ^= Base.GetHashCode();
		}
		if (CityId != 0)
		{
			num ^= CityId.GetHashCode();
		}
		if (Health != 0)
		{
			num ^= Health.GetHashCode();
		}
		if (InitHealth != 0)
		{
			num ^= InitHealth.GetHashCode();
		}
		if (AfterHealth != 0)
		{
			num ^= AfterHealth.GetHashCode();
		}
		if (CityName.Length != 0)
		{
			num ^= CityName.GetHashCode();
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
		if (base_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Base);
		}
		if (CityId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CityId);
		}
		if (Health != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Health);
		}
		if (InitHealth != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(InitHealth);
		}
		if (AfterHealth != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(AfterHealth);
		}
		if (CityName.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(CityName);
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
		if (base_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Base);
		}
		if (CityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CityId);
		}
		if (Health != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Health);
		}
		if (InitHealth != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(InitHealth);
		}
		if (AfterHealth != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AfterHealth);
		}
		if (CityName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CityName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceCityArmyResult other)
	{
		if (other == null)
		{
			return;
		}
		if (other.base_ != null)
		{
			if (base_ == null)
			{
				Base = new ArmyResultBase();
			}
			Base.MergeFrom(other.Base);
		}
		if (other.CityId != 0)
		{
			CityId = other.CityId;
		}
		if (other.Health != 0)
		{
			Health = other.Health;
		}
		if (other.InitHealth != 0)
		{
			InitHealth = other.InitHealth;
		}
		if (other.AfterHealth != 0)
		{
			AfterHealth = other.AfterHealth;
		}
		if (other.CityName.Length != 0)
		{
			CityName = other.CityName;
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
				if (base_ == null)
				{
					Base = new ArmyResultBase();
				}
				input.ReadMessage(Base);
				break;
			case 16u:
				CityId = input.ReadInt32();
				break;
			case 24u:
				Health = input.ReadInt32();
				break;
			case 32u:
				InitHealth = input.ReadInt32();
				break;
			case 40u:
				AfterHealth = input.ReadInt32();
				break;
			case 50u:
				CityName = input.ReadString();
				break;
			}
		}
	}
}
